using AssetFlow.API.Requests;
using AssetFlow.Application.Auth.Commands.ApproveUser;
using AssetFlow.Application.Auth.Commands.RejectUser;
using AssetFlow.Application.Auth.Queries.GetPendingUsers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetFlow.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class UsersController(ISender sender) : ApiControllerBase
    {
        [HttpGet("pending")]
        public async Task<IActionResult> GetPending(CancellationToken ct)
        {
            var result = await sender.Send(new GetPendingUsersQuery(), ct);
            return result.IsSuccess ? Ok(result.Value) : Problem(result.Error!);
        }

        [HttpPut("{id:guid}/approve")]
        public async Task<IActionResult> Approve(
            Guid id, [FromBody] ApproveUserRequest request, CancellationToken ct)
        {
            var command = new ApproveUserCommand(
                id, request.EmployeeNumber, request.FirstName, request.LastName,
                request.Department, request.JobTitle, request.HireDate);
            var result = await sender.Send(command, ct);
            return result.IsSuccess ? Ok(result.Value) : Problem(result.Error!);
        }

        [HttpPut("{id:guid}/reject")]
        public async Task<IActionResult> Reject(Guid id, CancellationToken ct)
        {
            var result = await sender.Send(new RejectUserCommand(id), ct);
            return result.IsSuccess ? NoContent() : Problem(result.Error!);
        }
    }

}