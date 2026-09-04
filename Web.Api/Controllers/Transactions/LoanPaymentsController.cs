using Application.Enumerations;
using Application.Features.LoanPayments.Command.PayOff;
using Application.Features.LoanPayments.Command.RecievePayment;
using Application.Features.LoanPayments.Queries.GetLoanSchedulesByLoanIdAsPaging;
using Application.Features.LoanPayments.Queries.GetPaidSchedule;
using Application.Features.LoanPayments.Queries.GetScheduleById;
using Application.Features.Transactions.LoanTransactions;
using Application.Responses.Paginations;
using Application.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers.Transactions;

public class LoanPaymentsController(IMediator mediator) : ApiControllerBase
{
    [HttpGet("loans/{loanId}/schedules")]
    public async Task<IActionResult> GetLoanSchedules(
        string loanId,
        [FromQuery] int? branchId = null,
        [FromQuery] ScheduleStatus status = ScheduleStatus.All,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 15,
        [FromQuery] string? sortColumn = "ScheduleNum",
        [FromQuery] bool isDescending = false,
        [FromQuery] string? filter = null)
    {
        var param = new QueryParam(pageIndex, pageSize, sortColumn, isDescending, false, filter);
        var result = await mediator.Send(new GetLoanSchedulesByLoanIdQuery(loanId, branchId, status, param));
        return HandleResult(result);
    }

    [HttpGet("loans/{loanId}/paid-schedules")]
    public async Task<IActionResult> GetPaidSchedules(string loanId, [FromQuery] int? branchId = null)
    {
        var result = await mediator.Send(new GetPaidScheduleQuery(branchId, loanId));
        return HandleResult(result);
    }

    [HttpGet("schedules/{scheduleId:long}")]
    public async Task<IActionResult> GetScheduleById(
        long scheduleId,
        [FromQuery] int? branchId = null,
        [FromQuery] string? loanId = null)
    {
        var result = await mediator.Send(new GetScheduleByIdQuery(scheduleId, branchId, loanId));
        return HandleResult(result);
    }

    [HttpPost("receive")]
    public async Task<IActionResult> ReceivePayment([FromBody] LoanPaymentTransactionDtoCreate request)
    {
        var result = await mediator.Send(new RecievePaymentCommand(request));
        return HandleResult(result);
    }

    [HttpPost("pay-off")]
    public async Task<IActionResult> PayOff([FromBody] LoanPaymentTransactionDtoCreate request)
    {
        var result = await mediator.Send(new PayOffCommand(request));
        return HandleResult(result);
    }
}
