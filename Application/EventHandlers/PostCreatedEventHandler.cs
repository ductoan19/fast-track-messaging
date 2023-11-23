using MediatR;
using my_app_backend.Application.QueryRepositories;
using my_app_backend.Domain.AggregateModel.BookAggregate.Events;
using my_app_backend.Models;
using Newtonsoft.Json;

namespace my_app_backend.Application.EventHandlers
{
    public class PostCreatedEventHandler : INotificationHandler<BookCreatedEvent>
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<PostCreatedEventHandler> _logger;
        public PostCreatedEventHandler(IBookRepository bookRepository, ILogger<PostCreatedEventHandler> logger)
        {
            _bookRepository = bookRepository;
            _logger = logger;
        }

        public async Task Handle(BookCreatedEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                await _bookRepository.Insert(new Models.BookDto
                {
                    Id = notification.BookId,
                    Author = notification.Author,
                    Type = notification.Type,
                    Name = notification.Name,
                    Locked = notification.Locked,
                    Quantity = notification.Quantity,
                    InventoryHistories = new List<BookInventoryHistoryDto>()
                });
            }
            catch (Exception ex)
            {
                _logger.Equals($"Exception happened: sync to read repository fail for BookCreatedEvent: {JsonConvert.SerializeObject(notification)}, ex: {ex}");
            }
        }
    }
}
