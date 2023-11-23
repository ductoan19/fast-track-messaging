using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using my_app_backend.Application.Commands;
using my_app_backend.Application.QueryRepositories;
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
        private readonly IMediator _mediator;
        private readonly IBookRepository _bookRepository;

        public BookController(IMediator mediator, IBookRepository bookRepository)
        {
            _mediator = mediator;
            _bookRepository = bookRepository;
        }

        #region Read side
        //// GET api/<BookController>/5
        [HttpGet("view-aggregate/{id}")]
        public async Task<ActionResult<ApiResponse<BookAggregate>>> ViewAggregate(Guid id)
        {
            var rs = await _mediator.Send(new BookAggregateQuery { Id = id });
            return Ok(rs.ToApiResponse());
        }

        //// GET api/<BookController>/5
        [HttpGet("{id}")]
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

        #region Write side
        // POST api/<BookController>
        [HttpPost()]
        [Authorize(Roles = Constants.Roles.Admin)]
        public async Task<ActionResult<ApiResponse<Guid>>> Post([FromBody] CreateBookCommand command)
        {
            var rs = await _mediator.Send(command);
            return Ok(rs.ToApiResponse());
        }

        // PUT api/<BookController>/5
        [HttpPut("update-inventory")]
        //[Authorize(Roles = Constants.Roles.Admin)]
        public async Task<ActionResult<ApiResponse>> UpdateQuantity([FromBody] UpdateBookQuantityCommand command)
        {
            var rs = await _mediator.Send(command);

            return Ok(rs.ToApiResponse());
        }
        #endregion
    }
}
