using System.Data.SqlClient;
using Dapper;
using MediatR;
using Microsoft.Extensions.Options;
using my_app_backend.Domain.AggregateModel.BookAggregate;
using my_app_backend.Domain.AggregateModel.BookAggregate.Events;
using Newtonsoft.Json;

namespace my_app_backend.Infrastructure.BookEventStores
{
    public class BookEventStore : IBookEventStore
    {
        private string _writeConnectionString;
        private readonly IMediator _mediator;

        public BookEventStore(IOptions<BookWriteDatabaseSettings> bookStoreDatabaseSettings, IMediator mediator)
        {
            _writeConnectionString = bookStoreDatabaseSettings.Value.ConnectionString;
            _mediator = mediator;
        }

        public async Task<BookAggregate> Get(Guid id)
        {
            using (var connection = new SqlConnection(_writeConnectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT * FROM BookAggregate WHERE Id = @id";
                var aggregate = await connection.QueryFirstOrDefaultAsync<BookAggregate>(query, new { id });
                if (aggregate == null)
                {
                    throw new Exception($"Not found for book aggregate id = {id}");
                }
                BookAggregateFactory.InitState(aggregate);

                query = "SELECT * FROM BookEvent WHERE AggregateId = @id ORDER BY Version";
                var serealizedEvents = await connection.QueryAsync<BookSerializedEvent>(query, new { id });
                var events = new List<IBookEvent>();
                foreach (var se in serealizedEvents)
                {
                    var @event = JsonConvert.DeserializeObject(se.Data, Type.GetType(se.Type)) as IBookEvent;
                    @event.Id = se.Id;
                    @event.AggregateId = se.AggregateId;
                    @event.Version = se.Version;
                    @event.CreateDate = se.CreateDate;

                    events.Add(@event);
                }
                aggregate.Rehydrate(events);

                return aggregate;
            }
        }

        public async Task Save(BookAggregate aggregate)
        {
            using (var connection = new SqlConnection(_writeConnectionString))
            {
                await connection.OpenAsync();
                const string aggregateQuery = "SELECT Id, Version FROM BookAggregate WHERE Id = @id";
                var dbAggregate = await connection.QueryFirstOrDefaultAsync<BookAggregate>(aggregateQuery, new { id = aggregate.Id });
                if (dbAggregate != null && dbAggregate.Version != aggregate.Version)
                {
                    throw new Exception($"Book aggregate is updated to version: {dbAggregate.Version}, current is: {aggregate.Version}");
                }

                var trans = connection.BeginTransaction();
                try
                {
                    var events = aggregate.Flush();

                    if (dbAggregate == null)
                    {
                        const string insertAggregateCmd = "INSERT INTO post_write.dbo.BookAggregate(Id, Version, CreatedDate, ModifiedDate) VALUES (@Id, @Version, @CreatedDate, @ModifiedDate);";
                        await connection.ExecuteAsync(insertAggregateCmd, aggregate, transaction: trans);
                    }
                    else
                    {
                        const string updateAggregateCmd = "UPDATE BookAggregate SET Version=@Version, ModifiedDate=@ModifiedDate WHERE Id=@Id;";
                        await connection.ExecuteAsync(updateAggregateCmd, aggregate, transaction: trans);
                    }

                    var serializedEvents = events.Select(e => new BookSerializedEvent
                    {
                        Id = e.Id,
                        Version = e.Version,
                        AggregateId = e.AggregateId,
                        Data = JsonConvert.SerializeObject(e),
                        Type = e.GetType().AssemblyQualifiedName,
                        CreateDate = e.CreateDate
                    }).ToList();

                    const string insertEventCmd = "INSERT INTO post_write.dbo.BookEvent (Id, AggregateId, Version, CreateDate, [Data], [Type]) VALUES(@Id, @AggregateId, @Version, @CreateDate, @Data, @Type);";
                    foreach (var se in serializedEvents)
                    {
                        await connection.ExecuteAsync(insertEventCmd, se, transaction: trans);
                    }

                    foreach (var e in events)
                    {
                        await _mediator.Publish(e);
                    }

                    trans.Commit();
                }
                catch
                {
                    trans.Rollback();
                    throw;
                }
            }
        }
    }
}
