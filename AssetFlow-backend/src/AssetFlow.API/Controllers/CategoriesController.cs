using AssetFlow.Application.Categories.Queries.GetCategories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetFlow.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class CategoriesController(ISender sender) : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllCategories(CancellationToken cancellationToken)
        {
            var result = await sender.Send(new GetCategoriesQuery(), cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : Problem(result.Error!);
        }
        
    }
}
