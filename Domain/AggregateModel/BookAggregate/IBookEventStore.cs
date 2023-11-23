namespace my_app_backend.Domain.AggregateModel.BookAggregate
{
    public interface IBookEventStore
    {
        public Task<BookAggregate> Get(Guid id);
        public Task Save(BookAggregate aggregate);
    }
}
