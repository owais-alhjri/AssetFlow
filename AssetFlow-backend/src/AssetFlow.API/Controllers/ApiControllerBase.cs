using AssetFlow.Domain.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AssetFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected IActionResult Problem(Error error) => error.Code switch
        {
            var c when c.Contains("NotFound")      => NotFound(ToProblem(error)),
            var c when c.Contains("Duplicate")     => Conflict(ToProblem(error)),
            var c when c.Contains("AlreadyExists") => Conflict(ToProblem(error)),
            _                                            => BadRequest(ToProblem(error))
        };

        private static ProblemDetails ToProblem(Error error) => new()
        {
            Title = error.Code,
            Detail = error.Message
        };
    }
}
