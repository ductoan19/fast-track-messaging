namespace my_app_backend.Domain.AggregateModel.BookAggregate.Events
{
    public class BookDeletedEvent : BookEvent, IBookEvent
    {
        public Guid BookId { get; set; }
    }
}
