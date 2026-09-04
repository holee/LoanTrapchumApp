using Applications.Enumerations;
using Applications.Features.LoanRequests.Command.AddCoPawnCustomer;
using Applications.Features.LoanRequests.Command.AddCoPawnCustomers;
using Applications.Features.LoanRequests.Command.AddDescription;
using Applications.Features.LoanRequests.Command.AddLoanRequestCollateral;
using Applications.Features.LoanRequests.Command.AddLoanRequestDescription;
using Applications.Features.LoanRequests.Command.AddLoanRequestDocument;
using Applications.Features.LoanRequests.Command.AddLoanRequestGuarantor;
using Applications.Features.LoanRequests.Command.ApplyLoanRequest;
using Applications.Features.LoanRequests.Command.ApprovedLoanRequest;
using Applications.Features.LoanRequests.Command.CreateLoanRequest;
using Applications.Features.LoanRequests.Command.DeleteCoPawnCustomer;
using Applications.Features.LoanRequests.Command.DeleteLoanRequestCollateral;
using Applications.Features.LoanRequests.Command.DeleteLoanRequestDocument;
using Applications.Features.LoanRequests.Command.DeleteLoanRequestGuarantor;
using Applications.Features.LoanRequests.Command.DenyLoanRequest;
using Applications.Features.LoanRequests.Command.IsCustomerBusy;
using Applications.Features.LoanRequests.Command.PassRequest;
using Applications.Features.LoanRequests.Command.RecheckedLoanRequest;
using Applications.Features.LoanRequests.Command.SaveEditLoanRequest;
using Applications.Features.LoanRequests.Command.SaveLoanRequest;
using Applications.Features.LoanRequests.Command.SaveLoanRequests;
using Applications.Features.LoanRequests.Command.VerifiedLoanRequest;
using Applications.Features.LoanRequests.Dtos;
using Applications.Features.LoanRequests.Queryies.GetApprovedLoanRequest;
using Applications.Features.LoanRequests.Queryies.GetCustomerRequest;
using Applications.Features.LoanRequests.Queryies.GetLoanRequestAsPaging;
using Applications.Features.LoanRequests.Queryies.GetLoanRequestBy;
using Applications.Features.LoanRequests.Queryies.GetLoanRequestById;
using Applications.Features.LoanRequests.Queryies.GetLoanRequestDetailById;
using Applications.Features.LoanRequests.Queryies.GetLoanRequestGuarantor;
using Applications.Features.LoanRequests.Queryies.GetLoanRequestsDocuments;
using Applications.Features.LoanRequests.Queryies.GetLoanRequestsWithCanDisburse;
using Applications.Features.LoanRequests.Queryies.IsCoPawnExist;
using Applications.Features.LoanRequests.Requests;
using Applications.Responses.Paginations;
using Applications.Utils;
using Domains.Models.Loans;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers.Transactions;

public class LoanRequestsController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetLoanRequests(
        [FromQuery] string? userId = null,
        [FromQuery] int? branchId = 0,
        [FromQuery] RequestStatus requestStatus = RequestStatus.All,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 15,
        [FromQuery] string? sortColumn = "RequestDate",
        [FromQuery] bool isDescending = false,
        [FromQuery] string? filter = null)
    {
        var param = new QueryParam(pageIndex, pageSize, sortColumn, isDescending, false, filter);
        var result = await mediator.Send(new GetLoanRequestAsPagingQuery(userId, branchId, requestStatus, param));
        return Ok(result);
    }

    [HttpGet("disbursable")]
    public async Task<IActionResult> GetLoanRequestsWithCanDisburse(
        [FromQuery] string? userId = null,
        [FromQuery] bool canDisburse = true,
        [FromQuery] int? branchId = 0,
        [FromQuery] RequestStatus requestStatus = RequestStatus.All,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 15,
        [FromQuery] string? sortColumn = "RequestDate",
        [FromQuery] bool isDescending = false,
        [FromQuery] string? filter = null)
    {
        var param = new QueryParam(pageIndex, pageSize, sortColumn, isDescending, false, filter);
        var result = await mediator.Send(new GetLoanRequestsWithCanDisburseQuery(userId, canDisburse, branchId, requestStatus, param));
        return Ok(result);
    }

    [HttpGet("customer/{customerId}")]
    public async Task<IActionResult> GetCustomerRequests(string customerId)
    {
        var result = await mediator.Send(new GetCustomerRequestQuery(customerId));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetLoanRequestById(
        string id,
        [FromQuery] int? branchId = null,
        [FromQuery] string? customerId = null,
        [FromQuery] bool include = false)
    {
        var result = await mediator.Send(new GetLoanRequestByIdQuery(branchId, customerId, id, include));
        return HandleResult(result);
    }

    [HttpGet("{id}/details")]
    public async Task<IActionResult> GetLoanRequestDetails(
        string id,
        [FromQuery] int? branchId = null,
        [FromQuery] string? customerId = null,
        [FromQuery] bool include = true)
    {
        var result = await mediator.Send(new GetLoanRequestDetailByIdQuery(branchId, customerId, id, include));
        return HandleResult(result);
    }

    [HttpGet("{id}/edit")]
    public async Task<IActionResult> GetLoanRequestForEdit(
        string id,
        [FromQuery] int? branchId = null,
        [FromQuery] string? customerId = null,
        [FromQuery] bool include = false)
    {
        var result = await mediator.Send(new GetEditLoanRequestByIdQuery(branchId, customerId, id, include));
        return HandleResult(result);
    }

    [HttpGet("{id}/approved")]
    public async Task<IActionResult> GetLoanRequestApproved(
        string id,
        [FromQuery] int? branchId = null,
        [FromQuery] string? customerId = null)
    {
        var result = await mediator.Send(new GetLoanRequestApprovedQuery(branchId, customerId, id));
        return HandleResult(result);
    }

    [HttpGet("{id}/approved-details")]
    public async Task<IActionResult> GetApprovedLoanRequest(
        string id,
        [FromQuery] int? branchId = null,
        [FromQuery] string? customerId = null,
        [FromQuery] bool include = true)
    {
        var result = await mediator.Send(new GetApprovedLoanRequestQuery(branchId, customerId, id, include));
        return HandleResult(result);
    }

    [HttpGet("{id}/guarantors")]
    public async Task<IActionResult> GetLoanRequestGuarantors(
        string id,
        [FromQuery] int? branchId = null,
        [FromQuery] string? customerId = null)
    {
        var result = await mediator.Send(new GetLoanRequestGuarantorQuery(branchId, id, customerId));
        return HandleResult(result);
    }

    [HttpGet("{id}/documents")]
    public async Task<IActionResult> GetLoanRequestDocuments(string id, [FromQuery] int? branchId = null)
    {
        var result = await mediator.Send(new GetLoanRequestDocumentQuery(id, branchId));
        return HandleResult(result);
    }

    [HttpGet("co-pawn-exists")]
    public async Task<IActionResult> IsCoPawnExist([FromQuery] int? branchId = null, [FromQuery] string? customerId = null)
    {
        var result = await mediator.Send(new IsCoPawnExistQuery(branchId, customerId));
        return HandleResult(result);
    }

    [HttpGet("customer-busy")]
    public async Task<IActionResult> IsCustomerBusy([FromQuery] string? customerId = null)
    {
        var result = await mediator.Send(new IsCustomerBusyQuery(customerId));
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateLoanRequest([FromBody] LoanRequestDtoCreate request)
    {
        var result = await mediator.Send(new CreateLoanRequestCommand(request));
        return HandleResult(result);
    }

    [HttpPost("save")]
    public async Task<IActionResult> SaveLoanRequest([FromBody] LoanRequestDtoCreate request)
    {
        var result = await mediator.Send(new SaveLoanRequestCommand(request));
        return HandleResult(result);
    }

    [HttpPost("save-requests")]
    public async Task<IActionResult> SaveLoanRequests([FromBody] SaveLoanRequestDtoCreate request)
    {
        var result = await mediator.Send(new SaveLoanRequestsCommand(request));
        return HandleResult(result);
    }

    [HttpPost("{id}/save-edit")]
    public async Task<IActionResult> SaveEditLoanRequest(string id, [FromBody] LoanRequestDtoCreate request)
    {
        request.LoanRequestId = id;
        var result = await mediator.Send(new SaveEditLoanRequestCommand(request));
        return HandleResult(result);
    }

    [HttpPost("{id}/apply")]
    public async Task<IActionResult> ApplyLoanRequest(string id, [FromBody] RequestDtoBaseCreate request)
    {
        request.LoanRequestId = id;
        var result = await mediator.Send(new AppliedLoanRequestCommand(request));
        return HandleResult(result);
    }

    [HttpPost("{id}/verify")]
    public async Task<IActionResult> VerifyLoanRequest(string id, [FromBody] RequestDtoBaseCreate request)
    {
        request.LoanRequestId = id;
        var result = await mediator.Send(new VerifiedLoanRequestCommand(request));
        return HandleResult(result);
    }

    [HttpPost("{id}/pass")]
    public async Task<IActionResult> PassLoanRequest(string id, [FromBody] PassRequestDtoCreate request)
    {
        request.LoanRequestId = id;
        var result = await mediator.Send(new PassRequestCommand(request));
        return HandleResult(result);
    }

    [HttpPost("{id}/recheck")]
    public async Task<IActionResult> RecheckLoanRequest(string id, [FromBody] RecheckedRequestDtoCreate request)
    {
        request.LoanRequestId = id;
        var result = await mediator.Send(new RecheckedLoanRequestCommand(request));
        return HandleResult(result);
    }

    [HttpPost("{id}/approve")]
    public async Task<IActionResult> ApproveLoanRequest(string id, [FromBody] AproveLoanDtoCreate request)
    {
        request.LoanRequestId = id;
        var result = await mediator.Send(new ApprovedRequestCommand(request));
        return HandleResult(result);
    }

    [HttpPost("{id}/deny")]
    public async Task<IActionResult> DenyLoanRequest(string id, [FromBody] DeniedRequestDtoCreate request)
    {
        request.LoanRequestId = id;
        var result = await mediator.Send(new DeniedLoanRequestCommand(request));
        return HandleResult(result);
    }

    [HttpPost("{id}/guarantors")]
    public async Task<IActionResult> AddLoanRequestGuarantor(
        string id,
        [FromQuery] int? branchId,
        [FromBody] LoanRequestGuarantorsDtoCreate request)
    {
        request.LoanRequestId = id;
        request.BranchRefId = branchId;
        var result = await mediator.Send(new AddLoanRequestGuarantorCommand(request));
        return HandleResult(result);
    }

    [HttpDelete("{id}/guarantors/{guarantorLinkId:long}")]
    public async Task<IActionResult> DeleteLoanRequestGuarantor(string id, long guarantorLinkId, [FromQuery] int? branchId = null)
    {
        var result = await mediator.Send(new DeleteLoanRequestGuarantorCommand(id, branchId, guarantorLinkId));
        return HandleResult(result);
    }

    [HttpPost("{id}/documents")]
    public async Task<IActionResult> AddLoanRequestDocuments(
        string id,
        [FromQuery] int? branchId,
        [FromBody] List<LoanDocumentDtoCreate> requests)
    {
        foreach (var request in requests)
        {
            request.LoanRequestId = id;
            request.BranchId = branchId;
        }

        var result = await mediator.Send(new AddLoanRequestDocumentsCommand(requests));
        return HandleResult(result);
    }

    [HttpDelete("{id}/documents/{documentId:long}")]
    public async Task<IActionResult> DeleteLoanRequestDocument(string id, long documentId, [FromQuery] int? branchId = null)
    {
        var result = await mediator.Send(new DeleteLoanRequestDocumentCommand(id, branchId, documentId));
        return HandleResult(result);
    }

    [HttpPost("{id}/description")]
    public async Task<IActionResult> AddDescription(string id, [FromBody] LoanDescriptionsDtoCreate request)
    {
        request.LoanRequestRefId = id;
        var result = await mediator.Send(new AddDescriptionCommand(request));
        return HandleResult(result);
    }

    [HttpPost("{id}/descriptions")]
    public async Task<IActionResult> AddLoanRequestDescription(string id, [FromBody] LoanDescriptionsDtoCreate request)
    {
        request.LoanRequestRefId = id;
        var result = await mediator.Send(new AddLoanRequestDescriptionCommand(request));
        return HandleResult(result);
    }

    [HttpPost("{id}/collaterals")]
    public async Task<IActionResult> AddLoanRequestCollateral(
        string id,
        [FromBody] LoanCollateralRequest request,
        [FromQuery] int? branchId = null,
        [FromQuery] string? customerId = null)
    {
        request.LoanRequestId = id;
        request.BranchId = branchId;
        request.CustomerId = customerId;
        var result = await mediator.Send(new AddLoanRequestCollateralCommand(request));
        return HandleResult(result);
    }

    [HttpDelete("{id}/collaterals/{collateralLinkId:long}")]
    public async Task<IActionResult> DeleteLoanRequestCollateral(string id, long collateralLinkId, [FromQuery] int? branchId = null)
    {
        var result = await mediator.Send(new DeleteLoanRequestCollateralCommand(branchId, id, collateralLinkId));
        return HandleResult(result);
    }

    [HttpPost("{id}/co-pawn")]
    public async Task<IActionResult> AddCoPawnCustomer(string id, [FromBody] CoPawnRequestsDtoCreate request)
    {
        request.LoanRequestId = id;
        var result = await mediator.Send(new AddCoPawnCustomerCommand(request));
        return HandleResult(result);
    }

    [HttpPost("{id}/co-pawns")]
    public async Task<IActionResult> AddCoPawnCustomers(
        string id,
        [FromQuery] int? branchId,
        [FromBody] List<CoPawnRequestsDtoCreate> request)
    {
        foreach (var item in request)
        {
            item.LoanRequestId = id;
            item.BranchId = branchId;
        }

        var result = await mediator.Send(new AddCoPawnCustomersCommand(branchId, id, request));
        return HandleResult(result);
    }

    [HttpDelete("{id}/co-pawn")]
    public async Task<IActionResult> DeleteCoPawnCustomer(string id, [FromBody] CoPawnCustomerDto request)
    {
        request.RequestId = id;
        var result = await mediator.Send(new DeleteCoPawnCustomerCommand(request));
        return HandleResult(result);
    }
}
