using my_app_backend.Domain.SeedWork.Models;
using my_app_backend.Models;

namespace my_app_backend.Application.QueryRepositories
{
    public interface IBookRepository
    {
        Task<Result<IEnumerable<BookDto>>> GetAllAsync(string bookName);
        Task<Result<BookDto>> GetByIdAsync(Guid id);
        Task<Result> InsertAsync(BookDto bookDto);
        Task<Result> UpdateAsync(BookDto bookDto);
        Task<Result> DeleteAsync(BookDto bookDto);
    }
}
