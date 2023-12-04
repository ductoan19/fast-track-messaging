using MediatR;
using my_app_backend.Application.Commands;
using my_app_backend.Domain.AggregateModel.BookAggregate;
using my_app_backend.Domain.SeedWork.Models;

namespace my_app_backend.Application.CommandHandlers
{
    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, Result<Guid>>
    {
        private readonly IBookEventStore _bookEventStore;
        
        public UpdateBookCommandHandler(IBookEventStore bookEventStore)
        {
            _bookEventStore = bookEventStore;
        }

        public async Task<Result<Guid>> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var aggregate = await _bookEventStore.Get(request.Id);
                if (aggregate is null)
                {
                    return Result<Guid>.Error("Could not find booking with the provided Id");
                }

                aggregate.UpdateBook(request.Name, request.Author, request.Type);
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
