using MediatR;
using my_app_backend.Application.Commands;
using my_app_backend.Domain.AggregateModel.BookAggregate;
using my_app_backend.Domain.SeedWork.Models;

namespace my_app_backend.Application.CommandHandlers
{
    public class UpdateBookQuantityCommandHandler : IRequestHandler<UpdateBookQuantityCommand, Result>
    {
        private readonly IBookEventStore _bookEventStore;
        public UpdateBookQuantityCommandHandler(IBookEventStore bookEventStore)
        {
            _bookEventStore = bookEventStore;
        }
        public async Task<Result> Handle(UpdateBookQuantityCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var aggregate = await _bookEventStore.Get(request.Id);
                aggregate.UpdateQuantity(request.Quantity, request.Direction, request.Note);
                await _bookEventStore.Save(aggregate);
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Error($"Exception happened: {ex}");
            }
        }
    }
}
