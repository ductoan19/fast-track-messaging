namespace my_app_backend.Domain.AggregateModel.BookAggregate.Events
{
    public class BookUpdatedEvent
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public bool Locked { get; set; }
    }
}
