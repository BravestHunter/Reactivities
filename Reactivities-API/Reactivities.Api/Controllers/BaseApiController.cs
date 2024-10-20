using MediatR;
using Microsoft.AspNetCore.Mvc;
using Reactivities.Api.Extensions;
using Reactivities.Application.Core;

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
                return NotFound();
            }

            return Ok();
        }

        protected ActionResult HandleResult<T>(Result<T> result)
        {
            if (result.IsFailure)
            {
                return NotFound();
            }

            var value = result.GetOrThrow();

            return Ok(value);
        }

        protected ActionResult HandlePagedResult<T>(Result<PagedList<T>> result)
        {
            if (result.IsFailure)
            {
                return NotFound();
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
    }
}
