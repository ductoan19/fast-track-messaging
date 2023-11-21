namespace my_app_backend.Models
{
    public class LoginRes
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public required string Username { get; set; }
        public required string Role { get; set; }
        public required string Description { get; set; }
    }
}
