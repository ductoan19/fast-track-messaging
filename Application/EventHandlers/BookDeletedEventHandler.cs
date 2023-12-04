using MediatR;
using my_app_backend.Application.QueryRepositories;
using my_app_backend.Domain.AggregateModel.BookAggregate.Events;
using my_app_backend.Models;
using Newtonsoft.Json;

namespace my_app_backend.Application.EventHandlers
{
    public class BookDeletedEventHandler : INotificationHandler<BookDeletedEvent>
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<BookDeletedEventHandler> _logger;
        public BookDeletedEventHandler(IBookRepository bookRepository, ILogger<BookDeletedEventHandler> logger)
        {
            _bookRepository = bookRepository;
            _logger = logger;
        }

        public async Task Handle(BookDeletedEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                var rs = await _bookRepository.GetByIdAsync(notification.BookId);
                if (!rs.IsSuccessful || rs.Data is null)
                {
                    throw new Exception(rs.Message);
                }

                await _bookRepository.DeleteAsync(new BookDto { Id = notification.BookId });
            }
            catch (Exception ex)
            {
                _logger.Equals($"Exception happened: sync to read repository fail for BookDeletedEvent: {JsonConvert.SerializeObject(notification)}, ex: {ex}");
            }
        }
    }
}
