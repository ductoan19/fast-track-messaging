namespace my_app_backend.Infrastructure.QueryRepositories
{
    public class BookReadDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string BooksCollectionName { get; set; }
    }
}
