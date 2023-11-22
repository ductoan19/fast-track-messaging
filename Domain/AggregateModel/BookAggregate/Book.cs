using my_app_backend.Domain.SeedWork.Models;

namespace my_app_backend.Domain.AggregateModel.BookAggregate
{
    public class Book
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public bool Locked { get; set; }
    }
}
