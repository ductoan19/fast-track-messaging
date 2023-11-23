using my_app_backend.Domain.AggregateModel.BookAggregate.Events;

namespace my_app_backend.Domain.AggregateModel.BookAggregate
{
    public class BookState
    {
        public Book Book { get; set; }
        public List<BookInventoryHistory> InventoryHistories { get; set; }

        public void When(BookCreatedEvent @event)
        {
            Book = new Book
            {
                Name = @event.Name,
                Author = @event.Author,
                Type = @event.Type,
                Locked = @event.Locked
            };
        }

        public void When(BookQuantityUpdatedEvent @event)
        {
            Book.Quantity += @event.Quantity * @event.Direction;
            InventoryHistories.Add(new BookInventoryHistory
            {
                Direction = @event.Direction,
                Quantity = @event.Quantity,
                Note = @event.Note,
                CreatedDate = @event.CreateDate
            });
        }

        public void Apply(IBookEvent @event)
        {
            try
            {
                var when = GetType().GetMethod("When", new[] { @event.GetType() });

                if (when == null)
                    throw new Exception(@"No method to apply: {@event.GetType()}");

                when.Invoke(this, new object[] { @event });
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class Book
    {
        public int Quantity { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public bool Locked { get; set; }
    }

    public class BookInventoryHistory
    {
        public int Quantity { get; set; }
        public int Direction { get; set; }
        public string Note { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
