using AssetFlow.Application.Assignments.Commands.AssignAsset;
using AssetFlow.Application.Assignments.Commands.ReturnAsset;
using AssetFlow.Application.Assignments.Queries.GetActiveAssignments;
using AssetFlow.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetFlow.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AssignmentsController(ISender sender) : ApiControllerBase
    {
        [HttpGet("active")]
        public async Task<IActionResult> GetActive(CancellationToken ct)
        {
            var result = await sender.Send(new GetActiveAssignmentsQuery(), ct);
            return result.IsSuccess ? Ok(result.Value) : Problem(result.Error!);
        }

        [HttpPost]
        public async Task<IActionResult> Assign(
            [FromBody] AssignAssetCommand command, CancellationToken ct)
        {
            var result = await sender.Send(command, ct);
            return result.IsSuccess ? Ok(result.Value) : Problem(result.Error!);
        }

        [HttpPut("{id:guid}/return")]
        public async Task<IActionResult> Return(
            Guid id, [FromBody] ReturnAssetRequest request, CancellationToken ct)
        {
            var command = new ReturnAssetCommand(
                id, request.ReturnedDate, request.ConditionAtReturn, request.ReturnNotes);
            var result = await sender.Send(command, ct);
            return result.IsSuccess ? Ok(result.Value) : Problem(result.Error!);
        }
    }

    public record ReturnAssetRequest(
        DateOnly ReturnedDate,
        AssetCondition ConditionAtReturn,
        string? ReturnNotes);
}