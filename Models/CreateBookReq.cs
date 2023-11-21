namespace my_app_backend.Models
{
    public class CreateBookReq
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string Type { get; set; }
        public bool Locked { get; set; }
    }
}
