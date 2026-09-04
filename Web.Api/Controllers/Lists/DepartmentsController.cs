using Applications.Features.Departments.Commands.CreateDepartment;
using Applications.Features.Departments.Commands.DeleteDepartment;
using Applications.Features.Departments.Commands.EditDepartment;
using Applications.Features.Departments.Dto;
using Applications.Features.Departments.Queries.GetDepartmentAsPaging;
using Applications.Features.Departments.Queries.GetDepartmentById;
using Applications.Responses.Paginations;
using Applications.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers.Lists;

public class DepartmentsController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetDepartments(
        [FromQuery] int? departmentId = null,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 15,
        [FromQuery] string? sortColumn = "DepartmentName",
        [FromQuery] bool isDescending = false,
        [FromQuery] string? filter = null)
    {
        var param = new QueryParam(pageIndex, pageSize, sortColumn, isDescending, false, filter);
        var result = await mediator.Send(new GetDepartmentAsPagingQuery(departmentId, param));
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetDepartmentById(int id, [FromQuery] int? branchId = 0)
    {
        var result = await mediator.Send(new GetDepartmentById(id, branchId));
        return HandleResult(result);
    }

    [HttpGet("{id:int}/details")]
    public async Task<IActionResult> GetDepartmentDetails(int id, [FromQuery] int? branchId = 0)
    {
        var result = await mediator.Send(new GetDepartmentDetailsById(id, branchId));
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateDepartment([FromBody] DepartmentDtoCreate request)
    {
        var result = await mediator.Send(new CreateDepartmentCommand(request));
        return HandleResult(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateDepartment(int id, [FromBody] DepartmentDtoUpdate request)
    {
        request.DepartmentId = id;
        var result = await mediator.Send(new EditDepartmentCommand(request));
        return HandleResult(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteDepartment(int id, [FromQuery] int? branchId = 0)
    {
        var result = await mediator.Send(new DeleteDepartmentCommand(id, branchId));
        return HandleResult(result);
    }
}
