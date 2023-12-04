namespace my_app_backend.Domain.AggregateModel.BookAggregate.Events
{
    public class BookUpdatedEvent : BookEvent
    {
        public int Quantity { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public Guid BookId { get; set; }
    }
}
