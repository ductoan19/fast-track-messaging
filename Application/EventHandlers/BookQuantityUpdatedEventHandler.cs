using MediatR;
using my_app_backend.Application.QueryRepositories;
using my_app_backend.Domain.AggregateModel.BookAggregate.Events;
using my_app_backend.Models;
using Newtonsoft.Json;

namespace my_app_backend.Application.EventHandlers
{
    public class BookQuantityUpdatedEventHandler : INotificationHandler<BookQuantityUpdatedEvent>
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<PostCreatedEventHandler> _logger;
        public BookQuantityUpdatedEventHandler(IBookRepository bookRepository, ILogger<PostCreatedEventHandler> logger)
        {
            _bookRepository = bookRepository;
            _logger = logger;
        }
        public async Task Handle(BookQuantityUpdatedEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                var rs = await _bookRepository.GetById(notification.BookId);
                if (!rs.IsSuccessful)
                {
                    throw new Exception(rs.Message);
                }

                var book = rs.Data;
                var histories = book.InventoryHistories?.ToList() ?? new List<BookInventoryHistoryDto>();
                histories.Add(new BookInventoryHistoryDto
                {
                    Id = notification.Id,
                    Quantity = notification.Quantity,
                    Direction = notification.Direction,
                    CreatedDate = notification.CreateDate,
                    Note = notification.Note
                });
                book.Quantity += notification.Quantity * notification.Direction;
                book.InventoryHistories = histories;

                var updateRs = await _bookRepository.Update(book);
                if (!updateRs.IsSuccessful)
                {
                    throw new Exception(updateRs.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.Equals($"Exception happened: sync to read repository fail for BookQuantityUpdatedEvent: {JsonConvert.SerializeObject(notification)}, ex: {ex}");
            }
        }
    }
}
