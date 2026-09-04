using Applications.Enumerations;
using Applications.Features.LoanRequests.Requests;
using Applications.Features.Loans.Command.ApproveDeleteReuquest;
using Applications.Features.Loans.Command.ApproveEditLoanRequest;
using Applications.Features.Loans.Command.CloseLoanDisbursment;
using Applications.Features.Loans.Command.CreateLoan;
using Applications.Features.Loans.Command.DeleteLoan;
using Applications.Features.Loans.Command.EditLoanDisbursment;
using Applications.Features.Loans.Command.RequestDelete;
using Applications.Features.Loans.Command.RequestEditLoan;
using Applications.Features.Loans.Command.SaveLoanDisbursemenent;
using Applications.Features.Loans.Command.WidrawCollateral;
using Applications.Features.Loans.Dtos;
using Applications.Features.Loans.Queries.GetLateSchedules;
using Applications.Features.Loans.Queries.GetLoanByBranchId;
using Applications.Features.Loans.Queries.GetLoanById;
using Applications.Features.Loans.Queries.GetLoanCollaterals;
using Applications.Features.Loans.Queries.GetLoanDetailsById;
using Applications.Features.Loans.Queries.GetLoanForEdit;
using Applications.Features.Loans.Queries.GetLoanRelatedCollaterals;
using Applications.Features.Loans.Queries.GetLoanRelatedDocuments;
using Applications.Features.Loans.Queries.GetLoanRelatedGuarantors;
using Applications.Features.Loans.Queries.GetLoanSchedulesByPaymentDate;
using Applications.Features.Loans.Queries.GetLoanWithScedules;
using Applications.Responses.Paginations;
using Applications.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers.Transactions;

public class LoansController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetLoans(
        [FromQuery] int? branchId = 0,
        [FromQuery] string? userId = null,
        [FromQuery] LoanStatuses state = LoanStatuses.Active,
        [FromQuery] DateTime? start = null,
        [FromQuery] DateTime? to = null,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 15,
        [FromQuery] string? sortColumn = "StartDate",
        [FromQuery] bool isDescending = false,
        [FromQuery] string? filter = null)
    {
        var param = new QueryParam(pageIndex, pageSize, sortColumn, isDescending, false, filter);
        var result = await mediator.Send(new GetLoanByBranchIdQuery(branchId, userId, state, start, to, param));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetLoanById(string id, [FromQuery] int? branchId = null)
    {
        var result = await mediator.Send(new GetLoanByIdQuery(id, branchId));
        return HandleResult(result);
    }

    [HttpGet("{id}/details")]
    public async Task<IActionResult> GetLoanDetails(
        string id,
        [FromQuery] int? branchId = null,
        [FromQuery] ScheduleStatus status = ScheduleStatus.All,
        [FromQuery] bool include = false)
    {
        var result = await mediator.Send(new GetLoanDetailsByIdQuery(id, branchId, status, include));
        return HandleResult(result);
    }

    [HttpGet("{id}/edit")]
    public async Task<IActionResult> GetLoanForEdit(string id, [FromQuery] int? branchId = null)
    {
        var result = await mediator.Send(new GetLoanForEditQuery(id, branchId));
        return HandleResult(result);
    }

    [HttpGet("{id}/schedules")]
    public async Task<IActionResult> GetLoanWithSchedules(string id, [FromQuery] int? branchId = null)
    {
        var result = await mediator.Send(new GetLoanWithScedulesQuery(branchId, id));
        return HandleResult(result);
    }

    [HttpGet("{id}/collaterals")]
    public async Task<IActionResult> GetLoanCollaterals(string id, [FromQuery] int? branchId = null)
    {
        var result = await mediator.Send(new GetLoanCollateralsQuery(id, branchId));
        return Ok(result);
    }

    [HttpGet("{id}/related-collaterals")]
    public async Task<IActionResult> GetLoanRelatedCollaterals(string id, [FromQuery] int? branchId = null)
    {
        var result = await mediator.Send(new GetLoanRelatedCollateralsQuery(id, branchId));
        return Ok(result);
    }

    [HttpGet("{id}/related-documents")]
    public async Task<IActionResult> GetLoanRelatedDocuments(string id, [FromQuery] int? branchId = null)
    {
        var result = await mediator.Send(new GetLoanRelatedDocumentsQuery(id, branchId));
        return Ok(result);
    }

    [HttpGet("{id}/related-guarantors")]
    public async Task<IActionResult> GetLoanRelatedGuarantors(string id, [FromQuery] int? branchId = null)
    {
        var result = await mediator.Send(new GetLoanRelatedGuarantorsQuery(id, branchId));
        return Ok(result);
    }

    [HttpGet("late-schedules")]
    public async Task<IActionResult> GetLateSchedules(
        [FromQuery] int? branchId = null,
        [FromQuery] string? loanId = null,
        [FromQuery] DateTimeOffset? paymentDate = null)
    {
        var result = await mediator.Send(new GetLateSchedulesQuery(branchId, loanId, paymentDate));
        return Ok(result);
    }

    [HttpGet("payment-date-schedules")]
    public async Task<IActionResult> GetLoanSchedulesByPaymentDate(
        [FromQuery] int? branchId = null,
        [FromQuery] DateTimeOffset? paymentDate = null,
        [FromQuery] bool isIncludeLate = true,
        [FromQuery] bool isLateOnly = false,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 15,
        [FromQuery] string? sortColumn = "PaymentDate",
        [FromQuery] bool isDescending = false,
        [FromQuery] string? filter = null)
    {
        var param = new QueryParam(pageIndex, pageSize, sortColumn, isDescending, false, filter);
        var result = await mediator.Send(new GetLoanSchedulesByPaymentDateQuery(branchId, paymentDate, isIncludeLate, isLateOnly, param));
        return Ok(result);
    }

    [HttpGet("month-to-date-schedules")]
    public async Task<IActionResult> GetLoanSchedulesMonthToDate(
        [FromQuery] int? branchId = null,
        [FromQuery] DateTimeOffset? startDate = null,
        [FromQuery] DateTimeOffset? endDate = null,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 15,
        [FromQuery] string? sortColumn = "PaymentDate",
        [FromQuery] bool isDescending = false,
        [FromQuery] string? filter = null)
    {
        var param = new QueryParam(pageIndex, pageSize, sortColumn, isDescending, false, filter);
        var result = await mediator.Send(new GetLoanSchedulesMonthToDateQuery(branchId, startDate, endDate, param));
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateLoan([FromBody] LoanDtoCreate request)
    {
        var result = await mediator.Send(new CreateLoanCommand(request));
        return HandleResult(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLoan(string id, [FromBody] LoanDtoCreate request)
    {
        request.LoanId = id;
        var result = await mediator.Send(new EditLoanDisbursmentCommand(request));
        return HandleResult(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLoan(string id, [FromQuery] int? branchId = null, [FromQuery] string? customerId = null)
    {
        var result = await mediator.Send(new DeleteLoanCommand(id, branchId, customerId));
        return HandleResult(result);
    }

    [HttpPost("{id}/request-delete")]
    public async Task<IActionResult> RequestDeleteLoan(string id, [FromBody] RequestDeleteDtoCreate request)
    {
        request = request with { LoanId = id };
        var result = await mediator.Send(new RequestDeleteCommand(request));
        return HandleResult(result);
    }

    [HttpPost("{id}/approve-delete")]
    public async Task<IActionResult> ApproveDeleteLoan(string id, [FromBody] RequestDeleteDtoCreate request)
    {
        request = request with { LoanId = id };
        var result = await mediator.Send(new ApproveDeleteRequestCommand(request));
        return HandleResult(result);
    }

    [HttpPost("{id}/request-edit")]
    public async Task<IActionResult> RequestEditLoan(string id, [FromBody] RequestDeleteDtoCreate request)
    {
        request = request with { LoanId = id };
        var result = await mediator.Send(new RequestEditLoanCommand(request));
        return HandleResult(result);
    }

    [HttpPost("{id}/approve-edit")]
    public async Task<IActionResult> ApproveEditLoan(string id, [FromBody] RequestDeleteDtoCreate request)
    {
        request = request with { LoanId = id };
        var result = await mediator.Send(new ApproveEditLoanRequestCommand(request));
        return HandleResult(result);
    }

    [HttpPost("manager/save")]
    public async Task<IActionResult> SaveLoanRequestByManager([FromBody] SaveLoanRequestDtoCreate request)
    {
        var result = await mediator.Send(new SaveLoanRequestByManagerCommand(request));
        return HandleResult(result);
    }

    [HttpPost("{id}/withdraw-collaterals")]
    public async Task<IActionResult> WithdrawCollaterals(
        string id,
        [FromQuery] int? branchId,
        [FromBody] IEnumerable<WidthrawCollateralDtoCreate> request)
    {
        foreach (var item in request)
        {
            item.LoanId = id;
            item.BranchId = branchId;
        }

        var result = await mediator.Send(new WidrawalCollateralCommand(request));
        return HandleResult(result);
    }

    [HttpPost("{id}/close-disbursement")]
    public async Task<IActionResult> CloseLoanDisbursement(string id, [FromQuery] int? branchId = null, [FromQuery] string? customerId = null)
    {
        var result = await mediator.Send(new CloseLoanDisbursementCommand(branchId, id, customerId));
        return HandleResult(result);
    }
}
