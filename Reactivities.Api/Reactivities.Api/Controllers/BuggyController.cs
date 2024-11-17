using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reactivities.Api.Attributes;
using Reactivities.Api.Exceptions;

namespace Reactivities.Api.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class BuggyController : ControllerBase
    {
        [HttpGet("not-found")]
        [DevOnly]
        public ActionResult GetNotFound()
        {
            return NotFound();
        }

        [HttpGet("bad-request")]
        [DevOnly]
        public ActionResult GetBadRequest()
        {
            return BadRequest("This is a bad request");
        }

        [HttpGet("server-error")]
        [DevOnly]
        public ActionResult GetServerError()
        {
            throw new SampleException();
        }

        [HttpGet("unauthorised")]
        [DevOnly]
        public ActionResult GetUnauthorised()
        {
            return Unauthorized();
        }
    }
}
