using MediatR;
using my_app_backend.Application.QueryRepositories;
using my_app_backend.Domain.AggregateModel.BookAggregate.Events;
using my_app_backend.Models;
using Newtonsoft.Json;

namespace my_app_backend.Application.EventHandlers
{
    public class BookUpdatedEventHandler : INotificationHandler<BookUpdatedEvent>
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<BookUpdatedEventHandler> _logger;
        public BookUpdatedEventHandler(IBookRepository bookRepository, ILogger<BookUpdatedEventHandler> logger)
        {
            _bookRepository = bookRepository;
            _logger = logger;
        }

        public async Task Handle(BookUpdatedEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                var rs = await _bookRepository.GetByIdAsync(notification.BookId);
                if (!rs.IsSuccessful || rs.Data is null)
                {
                    throw new Exception(rs.Message);
                }

                await _bookRepository.UpdateAsync(new BookDto
                {
                    Id = notification.BookId,
                    Author = notification.Author,
                    Type = notification.Type,
                    Name = notification.Name,
                    Quantity = notification.Quantity,
                    InventoryHistories = rs.Data.InventoryHistories
                });
            }
            catch (Exception ex)
            {
                _logger.Equals($"Exception happened: sync to read repository fail for BookUpdatedEvent: {JsonConvert.SerializeObject(notification)}, ex: {ex}");
            }
        }
    }
}
