using Applications.Features.Customers.Commands.CreateCustomer;
using Applications.Features.Customers.Commands.DeleteCustomer;
using Applications.Features.Customers.Commands.EditCustomer;
using Applications.Features.Customers.Dto;
using Applications.Features.Customers.Queries.GetCustomerAsPaging;
using Applications.Features.Customers.Queries.GetCustomerById;
using Applications.Features.Customers.Queries.GetCustomerDetailById;
using Applications.Responses.Paginations;
using Applications.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers.Lists;

public class CustomersController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCustomers(
        [FromQuery] int? branchId = 0,
        [FromQuery] string? userId = null,
        [FromQuery] string? customerId = null,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 15,
        [FromQuery] string? sortColumn = "FirstName",
        [FromQuery] bool isDescending = false,
        [FromQuery] string? filter = null,
        [FromQuery] bool isLoanActive = false)
    {
        var param = new QueryParam(pageIndex, pageSize, sortColumn, isDescending, false, filter);
        var result = await mediator.Send(new GetCustomerAsPagingQuery(branchId, userId, customerId, param, isLoanActive));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomerById(string id)
    {
        var result = await mediator.Send(new GetCustomerByIdQuery(id));
        return HandleResult(result);
    }

    [HttpGet("{id}/details")]
    public async Task<IActionResult> GetCustomerDetails(string id, [FromQuery] bool include = false)
    {
        var result = await mediator.Send(new GetCustomerDetailByIdQuery(id, include));
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomer([FromBody] EntityCustomerCreate request)
    {
        var result = await mediator.Send(new CreateCustomerCommand(request));
        return HandleResult(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomer(string id, [FromBody] EntityCustomerCreate request)
    {
        request.CustomerId = id;
        var result = await mediator.Send(new EditCustomerCommand(request));
        return HandleResult(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(string id)
    {
        var result = await mediator.Send(new DeleteCustomerCommand(id));
        return Ok(result);
    }
}
