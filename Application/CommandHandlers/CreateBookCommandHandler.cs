using MediatR;
using my_app_backend.Application.Commands;
using my_app_backend.Domain.AggregateModel.BookAggregate;
using my_app_backend.Domain.SeedWork.Models;

namespace my_app_backend.Application.CommandHandlers
{
    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, Result<Guid>>
    {
        private readonly IBookEventStore _bookEventStore;
        public CreateBookCommandHandler(IBookEventStore bookEventStore)
        {
            _bookEventStore = bookEventStore;
        }
        public async Task<Result<Guid>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var aggregate = BookAggregateFactory.Init();
                aggregate.CreateBook(request.Name, request.Author, request.Type, request.Locked);
                await _bookEventStore.Save(aggregate);
                return Result<Guid>.Ok(aggregate.Id);
            }
            catch (Exception ex)
            {
                return Result<Guid>.Error($"Exception happened: {ex}");
            }
        }
    }
}
