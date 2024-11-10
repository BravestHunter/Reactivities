using MediatR;
using Microsoft.AspNetCore.Mvc;
using Reactivities.Api.Extensions;
using Reactivities.Domain.Core;
using Reactivities.Domain.Exceptions;

namespace Reactivities.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        private readonly IMediator _mediator;
        protected IMediator Mediator => _mediator;

        public BaseApiController(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected ActionResult HandleResult(Result result)
        {
            if (result.IsFailure)
            {
                return HandleFailure(result.Exception);
            }

            return Ok();
        }

        protected ActionResult HandleResult<T>(Result<T> result)
        {
            if (result.IsFailure)
            {
                return HandleFailure(result.Exception);
            }

            var value = result.GetOrThrow();

            return Ok(value);
        }

        protected ActionResult HandlePagedResult<T>(Result<PagedList<T>> result)
        {
            if (result.IsFailure)
            {
                return HandleFailure(result.Exception);
            }

            var value = result.GetOrThrow();

            Response.AddPaginationHeader(
                value.CurrentPage,
                value.PageSize,
                value.TotalCount,
                value.TotalPages
            );

            return Ok(value);
        }

        private ActionResult HandleFailure(Exception exception)
        {
            switch (exception)
            {
                case NotFoundException ex:
                    return NotFound(ex.Message);

                case BadRequestException ex:
                    return BadRequest(ex.Message);

                default:
                    return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }
    }
}
