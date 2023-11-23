using MediatR;

namespace my_app_backend.Domain.AggregateModel.BookAggregate.Events
{
    public class BookCreatedEvent : BookEvent, IBookEvent
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public int Quantity { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public bool Locked { get; set; }
    }
}
