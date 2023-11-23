namespace my_app_backend.Domain.AggregateModel.BookAggregate.Events
{
    public class BookQuantityUpdatedEvent : BookEvent, IBookEvent
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public int Direction { get; set; }
        public string Note { get; set; }
    }
}
