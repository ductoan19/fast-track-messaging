using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using my_app_backend.Domain.AggregateModel.BookAggregate;
using my_app_backend.Domain.SeedWork.Models;
using my_app_backend.Models;

namespace my_app_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookController : ControllerBase
    {
        public static object _lock = new object();
        public static int _id = 1;
        public static List<Book> _books = new List<Book>()
        {
            new Book
            {
                Id = _id++,
                Type = "Programming",
                Author = "NghiaNv18",
                Name = "C# fundamental",
                Locked = true
            },
            new Book
            {
                Id = _id++,
                Author = "NghiaNv18",
                Type = "Programming",
                Name = "Angular fundamental",
                Locked = true
            },
            new Book
            {
                Id = _id++,
                Author = "NghiaNv18",
                Type = "Programming",
                Name = "SQL Server fundamental",
                Locked = false
            },
            new Book
            {
                Id = _id++,
                Author = "NghiaNv18",
                Type = "English",
                Name = "English grammar basic",
                Locked = true
            }
        };

        #region Read side
        // GET: api/<BookController>
        [HttpGet]
        [Authorize(Roles = $"{Constants.Roles.Admin},{Constants.Roles.Normal}")]
        public ActionResult<ApiResponse<List<BookDto>>> Get()
        {
            lock (_lock)
            {
                StatisticController._noOfView++;
            }

            return Ok(ApiResponse<List<BookDto>>.Ok(_books.OrderByDescending(b => b.Id).Select(MapBookDto).ToList()));
        }

        // GET api/<BookController>/5
        [HttpGet("{id}")]
        public ActionResult<ApiResponse<BookDto>> Get(int id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                lock (_lock)
                {
                    StatisticController._noOfView++;
                }

                return Ok(ApiResponse<BookDto>.Ok(MapBookDto(book)));
            }
            else
            {
                return Ok(ApiResponse<BookDto>.Error("Not found!"));
            }
        }
        #endregion

        #region Write side
        // POST api/<BookController>
        [HttpPost()]
        [Authorize(Roles = Constants.Roles.Admin)]
        public ActionResult<ApiResponse<int>> Post([FromBody] CreateBookReq book)
        {
            var dbDuplicateBook = _books.FirstOrDefault(b => b.Name == book.Name && b.Author == book.Author);
            if (dbDuplicateBook != null)
            {
                return Ok(ApiResponse<int>.Error("Duplicate book!"));
            }

            var newBook = new BookDto()
            {
                Id = _id++,
                Author = book.Author,
                Name = book.Name,
                Type = book.Type,
                Locked = book.Locked
            };
            _books.Add(MapBook(newBook));

            lock (_lock)
            {
                StatisticController._noOfCreation++;
            }

            return Ok(ApiResponse<int>.Ok(newBook.Id));

        }

        // PUT api/<BookController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = Constants.Roles.Admin)]
        public ActionResult<ApiResponse> Put([FromRoute] int id, [FromBody] UpdateBookReq book)
        {
            var dbBook = _books.FirstOrDefault(b => b.Id == id);
            if (dbBook == null)
            {
                return Ok(ApiResponse.Error("Not found!"));
            }

            var dbDuplicateBook = _books.FirstOrDefault(b => b.Name == book.Name && b.Author == book.Author && b.Id != id);
            if (dbDuplicateBook != null)
            {
                return Ok(ApiResponse.Error("Duplicate book!"));
            }

            dbBook.Name = book.Name;
            dbBook.Author = book.Author;
            dbBook.Type = book.Type;
            dbBook.Locked = book.Locked;

            lock (_lock)
            {
                StatisticController._noOfUpdate++;
            }

            return Ok(ApiResponse.Ok());
        }

        // DELETE api/<BookController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = Constants.Roles.Admin)]
        public ActionResult<ApiResponse> Delete(int id)
        {
            var dbBook = _books.FirstOrDefault(b => b.Id == id);
            if (dbBook == null)
            {
                return Ok(ApiResponse.Error("Not found!"));
            }
            else if (dbBook.Locked)
            {
                return Ok(ApiResponse.Error("Book is locked to delete!"));
            }

            _books = _books.Where(b => b.Id != id).ToList();
            lock (_lock)
            {
                StatisticController._noOfDelete++;
            }

            return Ok(ApiResponse.Ok());
        }

        private BookDto MapBookDto(Book book)
        {
            return new BookDto
            {
                Id = book.Id,
                Type = book.Type,
                Author = book.Author,
                Name = book.Name,
                Locked = book.Locked
            };
        }

        private Book MapBook(BookDto book)
        {
            return new Book
            {
                Id = book.Id,
                Type = book.Type,
                Author = book.Author,
                Name = book.Name,
                Locked = book.Locked
            };
        }
        #endregion
    }
}
