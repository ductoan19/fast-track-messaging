namespace my_app_backend.Domain.AggregateModel.BookAggregate
{
    public class BookAggregateFactory
    {
        public static BookAggregate Init()
        {
            var aggregate = new BookAggregate()
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                Version = 0
            };

            InitState(aggregate);

            return aggregate;
        }

        public static void InitState(BookAggregate aggregate)
        {
            aggregate.State = new BookState
            {
                Book = new Book(),
                InventoryHistories = new List<BookInventoryHistory>()
            };
        }
    }
}
