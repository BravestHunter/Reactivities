using API.Extensions;
using Application.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
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

        protected ActionResult HandleResult<T>(Result<T> result)
        {
            if (result == null)
            {
                return NotFound();
            }

            if (result.IsSuccess)
            {
                if (result.Value != null)
                {
                    return Ok(result.Value);
                }

                return NotFound();
            }

            return BadRequest(result.Error);
        }

        protected ActionResult HandlePagedResult<T>(Result<PagedList<T>> result)
        {
            if (result == null)
            {
                return NotFound();
            }

            if (result.IsSuccess)
            {
                if (result.Value != null)
                {
                    Response.AddPaginationHeader(
                        result.Value.CurrentPage,
                        result.Value.PageSize,
                        result.Value.TotalCount,
                        result.Value.TotalPages
                        );
                    return Ok(result.Value);
                }

                return NotFound();
            }

            return BadRequest(result.Error);
        }
    }
}
