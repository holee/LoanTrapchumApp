using Applications.Features.CreditAgents.Requests;
using Applications.Responses.Paginations;
using Applications.Services.Lists;
using Microsoft.AspNetCore.Mvc;

using LoanChangedCaDtoCreate = Applications.Features.CreditAgents.Requests.LoanChangedCaDtoCreate;

namespace Web.Api.Controllers.Lists;

public class CreditAgentsController(ICreditAgentServiceImpl creditAgentService) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCreditAgents(
        [FromQuery] int? branchId = 0,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 15,
        [FromQuery] string? sortColumn = "FirstName",
        [FromQuery] bool isDescending = false,
        [FromQuery] string? filter = null)
    {
        var param = new QueryParam(pageIndex, pageSize, sortColumn, isDescending, false, filter);
        var result = await creditAgentService.GetCreditAgents(branchId, param);
        return Ok(result);
    }

    [HttpGet("available")]
    public async Task<IActionResult> GetEmployeeCreateCreditAgents([FromQuery] string? query = null)
    {
        var result = await creditAgentService.GetEmployeeCreateCreditAgents(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddCreditAgent(
        [FromQuery] string? employeeRefId,
        [FromBody] LoanCreditAgentDtoCreate request)
    {
        var result = await creditAgentService.AddCreditAgent(employeeRefId, request);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCreditAgent(string id, [FromBody] LoanCreditAgentDtoUpdate request)
    {
        var result = await creditAgentService.UpdateCreditAgent(id, request);
        return Ok(result);
    }

    [HttpPost("activate")]
    public async Task<IActionResult> ActivateOrDeactivate([FromBody] LoanCreditAgentDtoActivate request)
    {
        var result = await creditAgentService.ActivateOrDeactivateCreditAgent(request);
        return Ok(result);
    }

    [HttpPost("change-agents")]
    public async Task<IActionResult> ChangeCreditAgents(
        [FromQuery] int? branchId,
        [FromBody] List<LoanChangedCaDtoCreate> request)
    {
        var result = await creditAgentService.ChangeCreditAgents(branchId, request);
        return Ok(result);
    }
}
