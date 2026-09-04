using Applications.Features.Employees.Commands.CreateEmployee;
using Applications.Features.Employees.Commands.DeleteEmployee;
using Applications.Features.Employees.Commands.EditEmployee;
using Applications.Features.Employees.Dto;
using Applications.Features.Employees.Queries.GetEmployeeAsPaging;
using Applications.Features.Employees.Queries.GetEmployeeById;
using Applications.Responses.Paginations;
using Applications.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers.Lists;

public class EmployeesController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetEmployees(
        [FromQuery] int? branchId = 0,
        [FromQuery] string? employeeId = null,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 15,
        [FromQuery] string? sortColumn = "FirstName",
        [FromQuery] bool isDescending = false,
        [FromQuery] string? filter = null)
    {
        var param = new QueryParam(pageIndex, pageSize, sortColumn, isDescending, false, filter);
        var result = await mediator.Send(new GetEmployeeAsPagingQuery(branchId, employeeId, param));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmployeeById(string id, [FromQuery] int branchId = 0)
    {
        var result = await mediator.Send(new GetEmployeeByIdQuery(id, branchId));
        return HandleResult(result);
    }

    [HttpGet("{id}/details")]
    public async Task<IActionResult> GetEmployeeDetails(string id, [FromQuery] int branchId = 0, [FromQuery] bool include = true)
    {
        var result = await mediator.Send(new GetEmployeeDetailsByIdQuery(id, branchId, include));
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateEmployee([FromBody] EntityEmployeeCreate request)
    {
        var result = await mediator.Send(new CreateEmployeeCommand(request));
        return HandleResult(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployee(string id, [FromBody] EntityEmployeeCreate request)
    {
        request.EmployeeId = id;
        var result = await mediator.Send(new EditEmployeeCommand(request));
        return HandleResult(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(string id, [FromQuery] int branchId = 0)
    {
        var result = await mediator.Send(new DeleteEmployeeCommand(id, branchId));
        return HandleResult(result);
    }
}
