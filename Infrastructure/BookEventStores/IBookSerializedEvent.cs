namespace my_app_backend.Infrastructure.BookEventStores
{
    public interface IBookSerializedEvent
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public string Type { get; set; }
        public string Data { get; set; }
        public int Version { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
