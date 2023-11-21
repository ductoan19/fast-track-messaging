using my_app_backend.Domain.SeedWork.Models;

namespace my_app_backend.Domain.AggregateModel.UserAggregate
{
    public class User
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; }
        public required string Description { get; set; }
    }
}
