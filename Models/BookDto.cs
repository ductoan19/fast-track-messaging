namespace my_app_backend.Models
{
    public class BookDto
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public int Quantity { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public bool Locked { get; set; }

        public IEnumerable<BookInventoryHistoryDto> InventoryHistories { get; set; }
    }
}
