using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using my_app_backend.Application.Commands;
using my_app_backend.Domain.AggregateModel.BookAggregate;
using my_app_backend.Domain.SeedWork.Models;
using my_app_backend.Models;

namespace my_app_backend.Controllers.Writes
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WriteBookController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WriteBookController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Test for write side
        // GET api/<BookController>/5
        [HttpGet("view-aggregate/{id}")]
        public async Task<ActionResult<ApiResponse<BookAggregate>>> ViewAggregate(Guid id)
        {
            var rs = await _mediator.Send(new BookAggregateQuery { Id = id });
            return Ok(rs.ToApiResponse());
        }
        #endregion

        #region Write side
        // POST api/<BookController>
        [HttpPost("create")]
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
