using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using my_app_backend.Application.QueryRepositories;
using my_app_backend.Domain.SeedWork.Models;
using my_app_backend.Models;

namespace my_app_backend.Infrastructure.QueryRepositories
{
    public class BookRepository : IBookRepository
    {
        private readonly IMongoCollection<BookDto> _booksCollection;

        public BookRepository(IOptions<BookReadDatabaseSettings> bookStoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(bookStoreDatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(bookStoreDatabaseSettings.Value.DatabaseName);
            _booksCollection = mongoDatabase.GetCollection<BookDto>(bookStoreDatabaseSettings.Value.BooksCollectionName);
        }


        public async Task<Result<IEnumerable<BookDto>>> GetAll(string bookName)
        {
            if (!string.IsNullOrWhiteSpace(bookName))
            {
                var filter = Builders<BookDto>.Filter.Regex("Name", new BsonRegularExpression(bookName));
                var books = await _booksCollection.Find(filter).ToListAsync();
                return Result<IEnumerable<BookDto>>.Ok(books);
            }
            else
            {
                var books = await _booksCollection.Find(_ => true).ToListAsync();
                return Result<IEnumerable<BookDto>>.Ok(books);
            }

        }

        public async Task<Result<BookDto>> GetById(Guid id)
        {
            var book = await _booksCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (book == null)
            {
                return Result<BookDto>.Error($"Not found book with id = {id}");
            }
            else
            {
                return Result<BookDto>.Ok(book);
            }
        }

        public async Task<Result> Insert(BookDto bookDto)
        {
            try
            {
                await _booksCollection.InsertOneAsync(bookDto);

                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Error($"Exception happened for insert book: {ex}");
            }
        }

        public async Task<Result> Update(BookDto bookDto)
        {
            try
            {
                await _booksCollection.ReplaceOneAsync(x => x.Id == bookDto.Id, bookDto);

                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Error($"Exception happened for update book: {ex}");
            }
        }
    }
}
