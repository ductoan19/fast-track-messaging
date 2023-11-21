namespace my_app_backend.Models
{
    public class UpdateBookReq
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Type { get; set; }
        public bool Locked { get; set; }
    }
}
