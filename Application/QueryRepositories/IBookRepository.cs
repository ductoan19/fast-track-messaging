using my_app_backend.Domain.SeedWork.Models;
using my_app_backend.Models;

namespace my_app_backend.Application.QueryRepositories
{
    public interface IBookRepository
    {
        Task<Result<IEnumerable<BookDto>>> GetAll(string bookName);
        Task<Result<BookDto>> GetById(Guid id);
        Task<Result> Insert(BookDto bookDto);
        Task<Result> Update(BookDto bookDto);
    }
}
