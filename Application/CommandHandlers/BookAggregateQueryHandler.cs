using MediatR;
using my_app_backend.Application.Commands;
using my_app_backend.Domain.AggregateModel.BookAggregate;
using my_app_backend.Domain.SeedWork.Models;

namespace my_app_backend.Application.CommandHandlers
{
    public class BookAggregateQueryHandler : IRequestHandler<BookAggregateQuery, Result<BookAggregate>>
    {
        private readonly IBookEventStore _bookEventStore;

        public BookAggregateQueryHandler(IBookEventStore bookEventStore)
        {
            _bookEventStore = bookEventStore;
        }

        public async Task<Result<BookAggregate>> Handle(BookAggregateQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var aggregate = await _bookEventStore.Get(request.Id);
                return Result<BookAggregate>.Ok(aggregate);
            }
            catch (Exception ex)
            {
                return Result<BookAggregate>.Error($"Exception happened: {ex}");
            }
        }
    }
}
