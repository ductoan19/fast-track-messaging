using System.Reflection;
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
        [HttpGet("view-aggregate/{id:guid}")]
        public async Task<ActionResult<ApiResponse<BookAggregate>>> ViewAggregateAsync(Guid id)
        {
            var rs = await _mediator.Send(new BookAggregateQuery { Id = id });
            return Ok(rs.ToApiResponse());
        }
        #endregion

        #region Write side
        [HttpPost]
        [Authorize(Roles = Constants.Roles.Admin)]
        public async Task<ActionResult<ApiResponse<Guid>>> CreateBookAsync([FromBody] CreateBookCommand command)
        {
            var rs = await _mediator.Send(command);
            return Ok(rs.ToApiResponse());
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = Constants.Roles.Admin)]
        public async Task<ActionResult<ApiResponse<Guid>>> UpdateBookAsync(Guid id, [FromBody] UpdateBookCommand command)
        {
            command.Id = id;
            var rs = await _mediator.Send(command);
            return Ok(rs.ToApiResponse());
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = Constants.Roles.Admin)]
        public async Task<ActionResult<ApiResponse<Guid>>> DeleteBookAsync(Guid id)
        {
            var rs = await _mediator.Send(new DeleteBookCommand(id));
            return Ok(rs.ToApiResponse());
        }

        [HttpPut("update-inventory")]
        public async Task<ActionResult<ApiResponse>> UpdateQuantityAsync([FromBody] UpdateBookQuantityCommand command)
        {
            var rs = await _mediator.Send(command);

            return Ok(rs.ToApiResponse());
        }
        #endregion
    }
}
