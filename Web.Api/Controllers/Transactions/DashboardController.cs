using Application.Features.Dashbaord.Accountants.Queries.GetDbAccountLoanSummaery;
using Application.Features.Dashbaord.Accountants.Queries.GetDbApprovedLoanRequest;
using Application.Features.Dashbaord.CreditOfficers.Queries.GetDbCreditOfficeLoanSummary;
using Application.Features.Dashbaord.CreditOfficers.Queries.GetNumberLoanRequest;
using Application.Features.Dashbaord.Managers.Queries.GetDbManagerLoanSummary;
using Application.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers.Transactions;

public class DashboardController(IMediator mediator) : ApiControllerBase
{
    [HttpGet("accountant/loan-summary")]
    public async Task<IActionResult> GetAccountantLoanSummary(
        [FromQuery] int? branchId = null,
        [FromQuery] DateTime? dueDate = null)
    {
        var result = await mediator.Send(new GetDbAccountLoanSummaryQuery(branchId, dueDate));
        return HandleResult(result);
    }

    [HttpGet("accountant/approved-requests")]
    public async Task<IActionResult> GetApprovedLoanRequestsCount([FromQuery] int? branchId = null)
    {
        var result = await mediator.Send(new GetDbApprovedRequestQuery(branchId));
        return HandleResult(result);
    }

    [HttpGet("credit-officer/loan-summary")]
    public async Task<IActionResult> GetCreditOfficerLoanSummary(
        [FromQuery] int? branchId = null,
        [FromQuery] string? userId = null,
        [FromQuery] DateTime? dueDate = null)
    {
        var result = await mediator.Send(new GetDbCreditOfficeLoanSummaryQuery(branchId, userId, dueDate));
        return HandleResult(result);
    }

    [HttpGet("credit-officer/number-requests")]
    public async Task<IActionResult> GetCreditOfficerNumberLoanRequests(
        [FromQuery] int? branchId = null,
        [FromQuery] string? userId = null)
    {
        var result = await mediator.Send(new GetNumberLoanRequestQuery(branchId, userId));
        return HandleResult(result);
    }

    [HttpGet("manager/loan-summary")]
    public async Task<IActionResult> GetManagerLoanSummary(
        [FromQuery] int? branchId = null,
        [FromQuery] string? userId = null)
    {
        var result = await mediator.Send(new GetDbManagerLoanSummaryQuery(branchId, userId));
        return HandleResult(result);
    }
}
