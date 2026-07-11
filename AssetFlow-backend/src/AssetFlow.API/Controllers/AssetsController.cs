using AssetFlow.Application.Assets.Commands.CreateAsset;
using AssetFlow.Application.Assets.Commands.DeleteAsset;
using AssetFlow.Application.Assets.Commands.UpdateAsset;
using AssetFlow.Application.Assets.Queries.GetAssets;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetFlow.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class AssetsController(ISender sender) : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAssets(
            [FromQuery] GetAssetsQuery query, CancellationToken ct)
        {
            var result = await sender.Send(query, ct);
            return result.IsSuccess ? Ok(result.Value) : Problem(result.Error!);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetAssetById(Guid id, CancellationToken ct)
        {
            var result = await sender.Send(new GetAssetByIdQuery(id), ct);
            return result.IsSuccess ? Ok(result.Value) : Problem(result.Error!);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAsset(
            [FromBody] CreateAssetCommand command, CancellationToken ct)
        {
            var result = await sender.Send(command, ct);
            return result.IsSuccess
                ? CreatedAtAction(nameof(GetAssetById), new { id = result.Value!.Id }, result.Value)
                : Problem(result.Error!);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAsset(
            Guid id, [FromBody] UpdateAssetCommand command, CancellationToken ct)
        {
            var result = await sender.Send(command with { Id = id }, ct);
            return result.IsSuccess ? Ok(result.Value) : Problem(result.Error!);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAsset(Guid id, CancellationToken ct)
        {
            var result = await sender.Send(new DeleteAssetCommand(id), ct);
            return result.IsSuccess ? NoContent() : Problem(result.Error!);
        }
    }
}
