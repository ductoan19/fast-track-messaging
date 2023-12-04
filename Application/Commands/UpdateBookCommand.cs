using MediatR;
using my_app_backend.Domain.SeedWork.Models;

namespace my_app_backend.Application.Commands
{
    public class UpdateBookCommand : IRequest<Result<Guid>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Type { get; set; }
    }
}
