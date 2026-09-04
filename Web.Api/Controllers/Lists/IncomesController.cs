using Applications.Features.Incomes.Command.CreateIncome;
using Applications.Features.Incomes.Command.CreateIncomeExpenseSource;
using Applications.Features.Incomes.Command.CreateIncomes;
using Applications.Features.Incomes.Command.DeleteIncome;
using Applications.Features.Incomes.Command.DeleteIncomeExpense;
using Applications.Features.Incomes.Command.EditIncome;
using Applications.Features.Incomes.Command.EditIncomeExpenseSource;
using Applications.Features.Incomes.Command.EditIncomes;
using Applications.Features.Incomes.Command.GetEditIncomeExpenseSource;
using Applications.Features.Incomes.Dto;
using Applications.Features.Incomes.Queries.GetEditIncome;
using Applications.Features.Incomes.Queries.GetIncomeByCustomerId;
using Applications.Features.Incomes.Queries.GetIncomeById;
using Applications.Features.Incomes.Queries.GetIncomeExpense;
using Applications.Responses.Paginations;
using Applications.Utils;
using Domains.Enumerations;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers.Lists;

public class IncomesController(IMediator mediator) : ApiControllerBase
{
    [HttpGet("customer/{customerId}")]
    public async Task<IActionResult> GetIncomesByCustomer(string customerId)
    {
        var result = await mediator.Send(new GetIncomeByCustomerQuery(customerId));
        return HandleResult(result);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetIncomeById(long id, [FromQuery] string customerId)
    {
        var result = await mediator.Send(new GetIncomeByIdQuery(id, customerId));
        return HandleResult(result);
    }

    [HttpGet("{id:long}/edit")]
    public async Task<IActionResult> GetIncomeForEdit(long id, [FromQuery] string? customerId = null)
    {
        var result = await mediator.Send(new GetEditIncomeCommand(id, customerId));
        return HandleResult(result);
    }

    [HttpGet("sources")]
    public async Task<IActionResult> GetIncomeExpenseSources(
        [FromQuery] int? id = null,
        [FromQuery] IncomeExpenseTypes type = IncomeExpenseTypes.Income,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 15,
        [FromQuery] string? sortColumn = "Description",
        [FromQuery] bool isDescending = false,
        [FromQuery] string? filter = null)
    {
        var param = new QueryParam(pageIndex, pageSize, sortColumn, isDescending, false, filter);
        var result = await mediator.Send(new GetIncomeExpenseQuery(id, type, param));
        return Ok(result);
    }

    [HttpGet("sources/{id:int}")]
    public async Task<IActionResult> GetIncomeExpenseSourceById(int id)
    {
        var result = await mediator.Send(new GetEditIncomeExpenseQuery(id));
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateIncome([FromBody] CreateIncomeDto request)
    {
        var result = await mediator.Send(new CreateIncomeCommand(request));
        return HandleResult(result);
    }

    [HttpPost("customer/{customerId}/bulk")]
    public async Task<IActionResult> CreateIncomes(string customerId, [FromBody] List<CreateIncomeDto> request)
    {
        var result = await mediator.Send(new CreateIncomesCommand(customerId, request));
        return HandleResult(result);
    }

    [HttpPost("sources")]
    public async Task<IActionResult> CreateIncomeExpenseSource([FromBody] CreateIncomeExpenseDto request)
    {
        var result = await mediator.Send(new CreateIncomeExpenseCommand(request));
        return HandleResult(result);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateIncome(long id, [FromBody] EditIncomeDto request)
    {
        request.Id = id;
        var result = await mediator.Send(new EditIncomeCommand(request));
        return HandleResult(result);
    }

    [HttpPut("customer/{customerId}/bulk")]
    public async Task<IActionResult> UpdateIncomes(string customerId, [FromBody] List<EditIncomeDto> requests)
    {
        var result = await mediator.Send(new EditIncomesCommand(customerId, requests));
        return HandleResult(result);
    }

    [HttpPut("sources/{id:int}")]
    public async Task<IActionResult> UpdateIncomeExpenseSource(int id, [FromBody] CreateIncomeExpenseDto request)
    {
        request.Id = id;
        var result = await mediator.Send(new EditIncomeExpenseCommand(request));
        return HandleResult(result);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteIncome(long id, [FromQuery] string? customerId = null)
    {
        var result = await mediator.Send(new DeleteIncomeCommand(id, customerId));
        return HandleResult(result);
    }

    [HttpDelete("sources/{id:int}")]
    public async Task<IActionResult> DeleteIncomeExpenseSource(int id)
    {
        var result = await mediator.Send(new DeleteIncomeExpenseCommand(id));
        return HandleResult(result);
    }
}
