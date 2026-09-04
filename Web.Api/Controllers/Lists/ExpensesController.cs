using Applications.Features.Expenses.CreateExpenseCommands;
using Applications.Features.Expenses.CreateExpensesCommands;
using Applications.Features.Expenses.DeleteExpenseCommand;
using Applications.Features.Expenses.Dto;
using Applications.Features.Expenses.EditExpenseCommands;
using Applications.Features.Expenses.EditExpensesCommands;
using Applications.Features.Expenses.Queries.GetExpenseByCustomerId;
using Applications.Features.Expenses.Queries.GetExpenseById;
using Applications.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers.Lists;

public class ExpensesController(IMediator mediator) : ApiControllerBase
{
    [HttpGet("customer/{customerId}")]
    public async Task<IActionResult> GetExpensesByCustomer(string customerId)
    {
        var result = await mediator.Send(new GetExpenseByCustomerQuery(customerId));
        return HandleResult(result);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetExpenseById(long id, [FromQuery] string customerId)
    {
        var result = await mediator.Send(new GetExpenseByIdQuery(id, customerId));
        return HandleResult(result);
    }

    [HttpGet("{id:long}/edit")]
    public async Task<IActionResult> GetExpenseForEdit(long id, [FromQuery] string customerId)
    {
        var result = await mediator.Send(new GetExpenseByIdUpdateQuery(id, customerId));
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateExpense([FromBody] ExpenseDtoCreate request)
    {
        var result = await mediator.Send(new AddExpenseCommand(request));
        return HandleResult(result);
    }

    [HttpPost("customer/{customerId}/bulk")]
    public async Task<IActionResult> CreateExpenses(string customerId, [FromBody] List<ExpenseDtoCreate> requests)
    {
        var result = await mediator.Send(new AddExpensesCommand(customerId, requests));
        return HandleResult(result);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateExpense(long id, [FromBody] ExpenseDtoUpdate request)
    {
        request.Id = id;
        var result = await mediator.Send(new EditExpenseCommand(request));
        return HandleResult(result);
    }

    [HttpPut("customer/{customerId}/bulk")]
    public async Task<IActionResult> UpdateExpenses(string customerId, [FromBody] List<ExpensesDtoUpdate> requests)
    {
        var result = await mediator.Send(new EditExpensesCommand(customerId, requests));
        return HandleResult(result);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteExpense(long id, [FromQuery] string customerId)
    {
        var result = await mediator.Send(new DeleteExpenseCommand(id, customerId));
        return HandleResult(result);
    }
}
