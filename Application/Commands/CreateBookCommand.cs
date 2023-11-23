using MediatR;
using my_app_backend.Domain.SeedWork.Models;

namespace my_app_backend.Application.Commands
{
    public class CreateBookCommand : IRequest<Result<Guid>>
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string Type { get; set; }
        public bool Locked { get; set; }
    }
}
