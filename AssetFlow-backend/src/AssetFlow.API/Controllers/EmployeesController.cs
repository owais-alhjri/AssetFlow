using AssetFlow.Application.Employees.Commands.CreateEmployee;
using AssetFlow.Application.Employees.Queries.GetEmployee;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetFlow.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class EmployeesController(ISender sender) : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetEmployees(CancellationToken ct)
        {
            var result = await sender.Send(new GetEmployeesQuery(), ct);
            return result.IsSuccess ? Ok(result.Value) : Problem(result.Error!);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetEmployeeById(Guid id, CancellationToken ct)
        {
            var result = await sender.Send(new GetEmployeeByIdQuery(id), ct);
            return result.IsSuccess ? Ok(result.Value) : Problem(result.Error!);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeCommand command, CancellationToken ct)
        {
            var result = await sender.Send(command, ct);
            return result.IsSuccess ? Ok(result.Value) : Problem(result.Error!);
        }
    }
}
