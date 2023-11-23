using MediatR;
using my_app_backend.Domain.AggregateModel.BookAggregate;
using my_app_backend.Domain.SeedWork.Models;

namespace my_app_backend.Application.Queries
{
    public class BookAggregateQuery : IRequest<Result<BookAggregate>>
    {
        public Guid Id { get; set; }
    }
}
