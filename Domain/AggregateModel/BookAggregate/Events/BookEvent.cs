namespace my_app_backend.Domain.AggregateModel.BookAggregate.Events
{
    public class BookEvent : IBookEvent
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public int Version { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
