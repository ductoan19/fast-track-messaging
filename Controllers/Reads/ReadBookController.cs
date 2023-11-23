using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using my_app_backend.Application.QueryRepositories;
using my_app_backend.Domain.SeedWork.Models;
using my_app_backend.Models;

namespace my_app_backend.Controllers.Reads
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReadBookController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public ReadBookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        #region Read side
        // GET api/<BookController>/5
        [HttpGet("get-by-id/{id}")]
        public async Task<ActionResult<ApiResponse<BookDto>>> Get(Guid id)
        {
            var rs = await _bookRepository.GetById(id);

            return Ok(rs.ToApiResponse());
        }

        // GET: api/<BookController>
        [HttpGet("get-all")]
        [Authorize(Roles = $"{Constants.Roles.Admin},{Constants.Roles.Normal}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<BookDto>>>> Get([FromQuery] string? name)
        {
            var rs = await _bookRepository.GetAll(name);

            return Ok(rs.ToApiResponse());
        }
        #endregion
    }
}
