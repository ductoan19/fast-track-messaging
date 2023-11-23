using MediatR;
using my_app_backend.Domain.SeedWork.Models;

namespace my_app_backend.Application.Commands
{
    public class UpdateBookQuantityCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public int Direction { get; set; }
        public string Note { get; set; }
    }
}
