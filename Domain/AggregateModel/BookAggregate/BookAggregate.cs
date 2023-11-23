using my_app_backend.Domain.AggregateModel.BookAggregate.Events;

namespace my_app_backend.Domain.AggregateModel.BookAggregate
{
    public class BookAggregate
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public BookState State { get; set; }

        private List<IBookEvent> _pendingEvents = new List<IBookEvent>();


        public BookCreatedEvent CreateBook(string name, string author, string type, bool locked)
        {
            var @event = new BookCreatedEvent
            {
                BookId = this.Id,
                Name = name,
                Author = author,
                Type = type,
                Locked = locked
            };

            Apply(@event);

            return @event;
        }

        public BookQuantityUpdatedEvent UdateQuantity(int quantity, int direction, string note)
        {
            var @event = new BookQuantityUpdatedEvent
            {
                BookId = this.Id,
                Quantity = quantity,
                Direction = direction,
                Note = note
            };


            Apply(@event);

            return @event;
        }

        private void Apply(IBookEvent @event)
        {
            State.Apply(@event);
            _pendingEvents.Add(@event);
        }

        public List<IBookEvent> Flush()
        {
            var events = new List<IBookEvent>();
            foreach (var @event in _pendingEvents)
            {
                Version++;
                @event.Id = Guid.NewGuid();
                @event.AggregateId = Id;
                @event.Version = Version;
                @event.CreateDate = DateTime.Now;
                events.Add(@event);
            }

            _pendingEvents.Clear();

            if (events.Any())
            {
                ModifiedDate = DateTime.Now;
            }

            return events;
        }

        public void Rehydate(List<IBookEvent> events)
        {
            var previousVersion = 0;
            foreach (var @event in events)
            {
                if (@event.Version != previousVersion + 1)
                {
                    throw new Exception($"Event isn't in sequence for version = {@event.Version}, previous = {previousVersion}");
                }

                State.Apply(@event);
                previousVersion = @event.Version;
            }
        }
    }
}
