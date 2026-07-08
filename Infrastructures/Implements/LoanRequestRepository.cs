using Applications.Commons.ExceptionHandlers;
using Applications.Commons.Mappers;
using Applications.Commons.Messages.Loanrequests;
using Applications.Enumerations;
using Applications.Extensions;
using Applications.Features.CreditAgents.Requests;
using Applications.Features.Customers.Response;
using Applications.Features.LoanRequests.Dtos;
using Applications.Features.LoanRequests.Requests;
using Applications.Features.Loans.Dtos.Responses;
using Applications.Interfaces.Identity;
using Applications.Interfaces.Loans;
using Applications.Responses.Lists.Entities;
using Applications.Responses.Message;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Customers;
using Applications.Responses.ViewModels.Loans.LoanRequests;
using Applications.Responses.ViewModels.Loans.Requests;
using Dapper;
using Domains.Enumerations;
using Domains.Models;
using Domains.Models.Lists.Customers;
using Domains.Models.Loans;
using ErrorOr;
using Infrastructures.DataAccess;
using Infrastructures.Implements.Extensions;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Linq.Expressions;

namespace Infrastructures.Implements;

public sealed class LoanRequestRepository(ILogger<LoanRequestRepository> logger,
        IDapperConnectionFactory dapperFactory,
        IDbContextFactory<DataContext> contextFactory,
        ICurrentUserService currentUserService) : ILoanRequestService
{

    public async Task<ErrorOr<RequestResult>> CreateNewOrSaveLoanRequestAsync(LoanRequest req, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Creating loan request for CustomerRefId: {CustomerRefId}", req.CustomerRefId);

        await using var cxt = await contextFactory.CreateDbContextAsync(cancellationToken);

        var hasExistingRequestTask = CheckExistingLoanRequest(req.CustomerRefId);

        var hasActiveLoanTask = CheckExistingHasLoan(req.CustomerRefId);

        await Task.WhenAll(hasExistingRequestTask, hasActiveLoanTask);

        if (hasExistingRequestTask.Result)
        {
            return Error.Conflict("Loan Request already exists.");
        }

        if (hasActiveLoanTask.Result)
        {
            return Error.Conflict("Customer has active loan.");
        }

        var existingRequest = await cxt.LoanRequests
            .Include(c => c.CollateralLinesLink)
                .ThenInclude(x => x.CollateralLine)
            .Include(g => g.GuarantorsLink)
                .ThenInclude(x => x.Guarantor)
            .Include(x => x.LoanDocuments)
            .Include(x => x.LoanDescriptions)
            .Include(x => x.CoPawnRequests)
            .FirstOrDefaultAsync(x => x.LoanRequestId == req.LoanRequestId &&
                        x.CustomerRefId == req.CustomerRefId &&
                        x.LoanRequestStatusRefId == (int)RequestStatus.New, cancellationToken);

        await using var transaction = await cxt.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            if (existingRequest is null)
            {
                cxt.LoanRequests.Add(req);
            }
            else
            {
                MergeLoanRequestForCreateOrSave(cxt, existingRequest, req);
            }

            await cxt.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return new RequestResult(true, "New Loan Request was created.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create loan request for CustomerRefId: {CustomerRefId}", req.CustomerRefId);

            await transaction.RollbackAsync(cancellationToken);

            return Error.Unexpected(ex.Message);
        }
    }
    public async Task<ErrorOr<RequestResult>> EditLoanRequestAsync(LoanRequest req, CancellationToken cancellationToken = default)
    {

        logger.LogInformation("Creating loan request for CustomerRefId: {CustomerRefId}", req.CustomerRefId);
        try
        {

            if (await CheckExistingLoanRequestNotRequested(req.CustomerRefId))
            {
                return Error.Conflict("Loan Request already exists.");
            }

            if (await CheckExistingHasLoan(req.CustomerRefId))
            {
                return Error.Conflict("Customer has active loan.");
            }

            await using var cxt = await contextFactory.CreateDbContextAsync();

            var existingRequest = await cxt.LoanRequests
                .Include(x => x.LoanDescriptions)
                .FirstOrDefaultAsync(x => x.LoanRequestId == req.LoanRequestId &&
                            x.CustomerRefId == req.CustomerRefId &&
                            (x.LoanRequestStatusRefId == (int)RequestStatus.New ||
                            x.LoanRequestStatusRefId == (int)RequestStatus.Rechecked), cancellationToken);
            using var transaction = await cxt.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                if (existingRequest is null)
                {
                    return Error.Conflict("There is any requests yet.");
                }

                cxt.Entry(existingRequest).CurrentValues.SetValues(req);

                if (req.LoanDescriptions.Any())
                {
                    existingRequest.LoanDescriptions.AddRange(req.LoanDescriptions);
                }

                await cxt.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                return new RequestResult(true, "Loan Request was updated.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                return Error.Unexpected(ex.Message);
            }

        }

        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create loan request for CustomerRefId: {CustomerRefId}", req.CustomerRefId);

            return Error.Unexpected(ex.Message);
        }
    }
    public async Task<ErrorOr<RequestResult>> ApprovedLoanRequestAsync(AproveLoanDtoCreate req, CancellationToken cancellationToken = default)
    {


        logger.LogInformation("Approving loan request for LoanRequestId: {LoanRequestId}, BranchRefId: {BranchRefId}, CustomerRefId: {CustomerId}",
            req.LoanRequestId, req.BranchId, req.CustomerId);


        await using var cxt = await contextFactory.CreateDbContextAsync();

        var existingLoanRequest = await cxt.LoanRequests
                                            .FirstOrDefaultAsync(x => x.LoanRequestId == req.LoanRequestId &&
                                                    x.BranchRefId == req.BranchId &&
                                                    x.CustomerRefId == req.CustomerId, cancellationToken);
        if (existingLoanRequest is null)
        {
            return Error.NotFound(LoanRequestMessage.REQUEST_NOT_FOUND);
        }

        if (existingLoanRequest.EditSeq != existingLoanRequest.EditSeq)
        {
            return Error.NotFound(LoanRequestMessage.REQUEST_NOT_FOUND);
        }

        using var transaction = await cxt.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            existingLoanRequest.LoanRequestStatusRefId = (int)RequestStatus.Approved;

            existingLoanRequest.ModifiedDt = DateTime.UtcNow;

            existingLoanRequest.AprovedDate = req.AprovedDate;

            existingLoanRequest.AprovedAmount = req.AprovedAmount;

            existingLoanRequest.AdminFeeAmount = req.AdminFeeAmount;

            existingLoanRequest.ApprovedBy = currentUserService.UserId;

            await cxt.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return new RequestResult(true, "Loan Request was Aproved");
        }
        catch (Exception ex)
        {

            logger.LogError(ex, "Failed to approve loan request for LoanRequestId: {LoanRequestId}, BranchRefId: {BranchRefId}, CustomerRefId: {CustomerId}", req.LoanRequestId, req.BranchId, req.CustomerId);

            await transaction.RollbackAsync(cancellationToken);

            return Error.Unexpected($"{ex.Message}");
        }
    }
    public async Task<ErrorOr<RequestResult>> SaveLoanRequestByManagerAsync(SaveLoanRequestDtoCreate req, CancellationToken cancellationToken = default)
    {

        await using var cxt = await contextFactory.CreateDbContextAsync();

        var existingLoanRequest = await cxt.LoanRequests
                        .FirstOrDefaultAsync(x => x.LoanRequestId == req.LoanRequestId &&
                                                x.BranchRefId == req.BranchId &&
                                                x.CustomerRefId == req.CustomerId, cancellationToken);
        if (existingLoanRequest is null)
        {
            return Error.NotFound(LoanRequestMessage.REQUEST_NOT_FOUND);
        }

        if (existingLoanRequest.EditSeq != existingLoanRequest.EditSeq)
        {
            return Error.NotFound(LoanRequestMessage.REQUEST_NOT_FOUND);
        }

        try
        {
            existingLoanRequest.LoanRequestStatusRefId = (int)RequestStatus.Requested;

            await cxt.SaveChangesAsync(cancellationToken);

            return new RequestResult(true, "Loan Request was saved successfully");
        }
        catch (Exception ex)
        {
            return Error.Unexpected($"{ex.Message}");
        }
    }
    public async Task<ErrorOr<RequestResult>> SaveLoanRequestAsync(SaveLoanRequestDtoCreate req, CancellationToken cancellationToken = default)
    {

        await using var cxt = await contextFactory.CreateDbContextAsync(cancellationToken);

        var existingLoanRequest = await cxt.LoanRequests
                        .Include(x => x.LoanDescriptions)
                        .FirstOrDefaultAsync(x => x.LoanRequestId == req.LoanRequestId &&
                                                x.BranchRefId == req.BranchId &&
                                                x.CustomerRefId == req.CustomerId, cancellationToken);
        if (existingLoanRequest is null)
        {
            return Error.NotFound(LoanRequestMessage.REQUEST_NOT_FOUND);
        }

        if (existingLoanRequest.EditSeq != existingLoanRequest.EditSeq)
        {
            return Error.NotFound(LoanRequestMessage.REQUEST_NOT_FOUND);
        }

        try
        {
            existingLoanRequest.LoanRequestStatusRefId = (int)req.RequestStatus;

            existingLoanRequest.IsSubmitted = false;

            existingLoanRequest.ModifiedDt = DateTime.UtcNow;

            existingLoanRequest.Remark = req.Remark;

            existingLoanRequest.LoanDescriptions.Add(new LoanDescriptions
            {
                Memo = req.Memo,

                UserRefId = currentUserService.UserId,
            });

            await cxt.SaveChangesAsync(cancellationToken);

            return new RequestResult(true, "Loan Request was saved successfully");
        }
        catch (Exception ex)
        {
            return Error.Unexpected($"{ex.Message}");
        }
    }
    public async Task<ErrorOr<RequestResult>> DeniedLoanRequestAsync(DeniedRequestDtoCreate req, CancellationToken cancellationToken = default)
    {
        await using var cxt = await contextFactory.CreateDbContextAsync(cancellationToken);

        try
        {
            var existingRequest = await cxt.LoanRequests
                .FirstOrDefaultAsync(x => x.LoanRequestId == req.LoanRequestId &&
                                          x.BranchRefId == req.BranchId &&
                                          x.CustomerRefId == req.CustomerId);

            if (existingRequest is null)
            {
                return Error.NotFound(LoanRequestMessage.REQUEST_NOT_FOUND);
            }

            if (existingRequest.EditSeq != existingRequest.EditSeq)
            {
                return Error.NotFound(LoanRequestMessage.REQUEST_NOT_FOUND);
            }

            existingRequest.LoanRequestStatusRefId = (int)RequestStatus.Denied;

            existingRequest.IsSubmitted = false;

            existingRequest.ModifiedDt = DateTime.UtcNow;

            existingRequest.Remark = req.Remark;

            await cxt.SaveChangesAsync(cancellationToken);

            return new RequestResult(true, "Loan Request was Denied");
        }
        catch (Exception ex)
        {
            return Error.Unexpected($"{ex.Message}");
        }
    }
    public async Task<ErrorOr<RequestResult>> VerifiedLoanRequestAsync(RequestDtoBaseCreate req, CancellationToken cancellationToken = default)
    {
        await using var cxt = await contextFactory.CreateDbContextAsync(cancellationToken);

        try
        {
            var existingRequest = await cxt.LoanRequests
                .FirstOrDefaultAsync(x => x.LoanRequestId == req.LoanRequestId &&
                                          x.BranchRefId == req.BranchId &&
                                          x.CustomerRefId == req.CustomerId);

            if (existingRequest is null)
            {
                return Error.NotFound(LoanRequestMessage.REQUEST_NOT_FOUND);
            }

            if (existingRequest.EditSeq != existingRequest.EditSeq)
            {
                return Error.NotFound(LoanRequestMessage.REQUEST_NOT_FOUND);
            }

            existingRequest.LoanRequestStatusRefId = (int)RequestStatus.Checking;
            existingRequest.ModifiedDt = DateTime.UtcNow;
            existingRequest.AprovedDate = DateTime.Now;
            existingRequest.Remark = req.Remark;
            await cxt.SaveChangesAsync(cancellationToken);
            return new RequestResult(true, "Loan Request was Verified");
        }
        catch (Exception ex)
        {
            return Error.Unexpected($"{ex.Message}");
        }
    }
    public async Task<ErrorOr<RequestResult>> RecheckedLoanRequestAsync(RecheckedRequestDtoCreate req, CancellationToken cancellationToken = default)
    {

        logger.LogInformation("Rechecking loan request for LoanRequestId: {LoanRequestId}", req.LoanRequestId);

        await using var cxt = await contextFactory.CreateDbContextAsync(cancellationToken);

        try
        {
            var existingLoanRequest = await cxt.LoanRequests
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.LoanRequestId == req.LoanRequestId &&
                                          x.BranchRefId == req.BranchId &&
                                          x.CustomerRefId == req.CustomerId, cancellationToken);

            if (existingLoanRequest is null)
            {
                return Error.NotFound(Messages.DataNotFound);
            }

            var updateResult = await cxt.LoanRequests
                                .FirstOrDefaultAsync(x => x.LoanRequestId == req.LoanRequestId &&
                                            x.BranchRefId == req.BranchId &&
                                            x.EditSeq == existingLoanRequest.EditSeq &&
                                            x.CustomerRefId == req.CustomerId, cancellationToken);

            if (updateResult is null)
            {
                return Error.NotFound(Messages.DataNotFound);
            }

            updateResult.LoanRequestStatusRefId = (int)RequestStatus.Rechecked;

            updateResult.ModifiedDt = DateTime.UtcNow;

            updateResult.Remark = req.Remark;

            await cxt.SaveChangesAsync(cancellationToken);

            return new RequestResult
            {
                Success = true,
                Message = "Loan Request was Rechecked"
            };

        }
        catch (Exception ex)
        {

            logger.LogError(ex, "Failed to recheck loan request for LoanRequestId: {LoanRequestId}", req.LoanRequestId);

            return Error.Unexpected($"Error due to : {ex.Message}");
        }
    }
    public async Task<ErrorOr<RequestResult>> AppliedLoanRequestAsync(RequestDtoBaseCreate req, CancellationToken cancellation = default)
    {
        await using var cxt = await contextFactory.CreateDbContextAsync(cancellation);

        var existingLoanRequest = await cxt.LoanRequests
                                    .FirstOrDefaultAsync(x => x.LoanRequestId == req.LoanRequestId &&
                                      x.BranchRefId == req.BranchId &&
                                      x.CustomerRefId == req.CustomerId, cancellation);

        if (existingLoanRequest is null)
        {
            return Error.NotFound(LoanRequestMessage.REQUEST_NOT_FOUND);
        }

        var lastEditSequent = existingLoanRequest.EditSeq + 1;

        var updateResult = await cxt.LoanRequests
            .Where(x => x.LoanRequestId == req.LoanRequestId && x.BranchRefId == req.BranchId)
            .ExecuteUpdateAsync(prop => prop
                .SetProperty(p => p.LoanRequestStatusRefId, (int)RequestStatus.Requested)
                .SetProperty(p => p.ModifiedDt, DateTime.UtcNow)
                .SetProperty(p => p.Remark, req.Remark)
                .SetProperty(p => p.EditSeq, lastEditSequent), cancellation);

        return updateResult > 0
            ? new RequestResult(true, "Loan Request was Approved")
            : Error.Unexpected("cannot be approved the loan Request.");
    }
    public async Task<ErrorOr<RequestResult>> PassLoanRequestAsync(PassRequestDtoCreate req, CancellationToken cancellationToken = default)
    {

        logger.LogInformation("Approving loan request for LoanRequestId: {LoanRequestId}, BranchRefId: {BranchRefId}, CustomerRefId: {CustomerId}",
            req.LoanRequestId, req.BranchId, req.CustomerId);


        await using var cxt = await contextFactory.CreateDbContextAsync();

        var existingLoanRequest = await cxt.LoanRequests
                                            .Include(x => x.LoanDescriptions)
                                            .FirstOrDefaultAsync(x => x.LoanRequestId == req.LoanRequestId &&
                                                    x.BranchRefId == req.BranchId &&
                                                    x.CustomerRefId == req.CustomerId, cancellationToken);
        if (existingLoanRequest is null)
        {
            return Error.NotFound(LoanRequestMessage.REQUEST_NOT_FOUND);
        }

        if (existingLoanRequest.EditSeq != existingLoanRequest.EditSeq)
        {
            return Error.NotFound(LoanRequestMessage.REQUEST_NOT_FOUND);
        }

        using var transaction = await cxt.Database.BeginTransactionAsync(cancellationToken);

        try
        {

            existingLoanRequest.LoanDescriptions.Add(new LoanDescriptions
            {
                LoanRequestRefId = req.LoanRequestId,
                Memo = req.Description,
                UserRefId = currentUserService.UserId
            });

            existingLoanRequest.LoanRequestStatusRefId = (int)RequestStatus.Requested;
            existingLoanRequest.IsSubmitted = true;
            existingLoanRequest.ModifiedDt = DateTime.UtcNow;
            await cxt.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return new RequestResult(true, "Loan Request was send");
        }
        catch (Exception ex)
        {

            logger.LogError(ex, "Failed to approve loan request for LoanRequestId: {LoanRequestId}, BranchRefId: {BranchRefId}, CustomerRefId: {CustomerId}", req.LoanRequestId, req.BranchId, req.CustomerId);

            await transaction.RollbackAsync(cancellationToken);

            return Error.Unexpected($"{ex.Message}");
        }
    }
    public async Task<ErrorOr<bool>> IsCustomerBusy(string? customerId, CancellationToken cancellation = default)
    {
        if (string.IsNullOrEmpty(customerId)) return false;

        using var db = await dapperFactory.CreateConnectionAsync(cancellation);
        var parameters = new DynamicParameters();
        parameters.Add("@CustomerId", customerId);
        parameters.Add("@IsLoanActive", true);
        var sql = @"SELECT LoanRequestId,CustomerId FROM VLoanRequests WHERE CustomerId=@CustomerId AND StatusId IN (1,2,3,6)";
        var sql1 = "SELECT CustomerId,IsLoanActive FROM VCustomerList WHERE CustomerId=@CustomerId AND IsLoanActive=@IsLoanActive";
        var existingLoanRequest = await db.QueryAsync<VLoanRequests>(sql, parameters);
        var customerInActiveLoan = await db.QueryAsync<Customer>(sql1, parameters);
        return existingLoanRequest.Any() || customerInActiveLoan.Any();

    }

    public async Task<ErrorOr<string>> AddLoanRequestCollateralAsync(LoanCollateralRequest req, CancellationToken cancellation = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellation);

        logger.LogInformation("Try to create new loan request collateral for LoanRequestId={LoanRequestId}", req.LoanRequestId);
        try
        {
            var existingRequest = await context.LoanRequests
                .Include(c => c.CollateralLinesLink)
                    .ThenInclude(x => x.CollateralLine)
                .FirstOrDefaultAsync(x => x.LoanRequestId == req.LoanRequestId &&
                            x.CustomerRefId == req.CustomerId &&
                            x.BranchRefId == req.BranchId, cancellation);

            if (existingRequest is null)
            {
                return Error.Conflict("Loan Request does not exist.");
            }

            var collerals = req.MapToCollateralLinkCreate();

            foreach (var item in collerals)
            {
                var existingCollateralLine = existingRequest.CollateralLinesLink
                                                .FirstOrDefault(x => x.Id == item.Id);
                if (existingCollateralLine is null || existingCollateralLine.Id == 0)
                {
                    existingRequest.CollateralLinesLink.Add(item);
                }
                else
                {
                    context.Entry(existingCollateralLine).CurrentValues.SetValues(item);

                    existingCollateralLine.ModifiedDt = DateTime.UtcNow; // track update
                }
            }

            await context.SaveChangesAsync(cancellation);

            return "Collateal was Added.";
        }

        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);

            return Error.Unexpected($"{ex.Message}");
        }
    }

    public async Task<CoPawnCustomerDto?> AddCoPawnCustomerAsync(CoPawnCustomer req, CancellationToken cancellation = default)
    {
        logger.LogInformation("Try to add co-pawn customer for LoanRequestId={LoanRequestId}", req.RequestId);

        await using var context = await contextFactory.CreateDbContextAsync(cancellation);

        try
        {

            var existLoan = await context.EntityNames
                                    .Include(x => x.Customer)
                                    .AnyAsync(x => x.Id == req.CustomerId &&
                                                   x.Customer!.IsLoanActive == true, cancellation);
            if (existLoan)
                throw new DuplicateException("Customer has active loan and cannot be added as co-pawn.");

            var customerRequest = await context.LoanRequests
                                    .AnyAsync(x => x.CustomerRefId == req.CustomerId &&
                                                   (x.LoanRequestStatusRefId == (int)RequestStatus.New ||
                                                    x.LoanRequestStatusRefId == (int)RequestStatus.Checking ||
                                                    x.LoanRequestStatusRefId == (int)RequestStatus.Rechecking ||
                                                    x.LoanRequestStatusRefId == (int)RequestStatus.Approved), cancellation);

            if (customerRequest)
            {
                throw new DuplicateException("Customer has an active loan request and cannot be added as co-pawn.");
            }

            var existingRequest = await context.LoanRequests
                .Include(c => c.CoPawnRequests)
                .FirstOrDefaultAsync(x =>
                    x.LoanRequestId == req.RequestId &&
                    x.BranchRefId == req.BranchId, cancellation);

            if (existingRequest is null)
                throw new NotFoundException("Loan Request does not exist.");

            var isDuplicate = existingRequest.CoPawnRequests
                .Any(r => r.CustomerId == req.CustomerId && r.Status == true);

            if (isDuplicate)
                throw new DuplicateException("Co-Pawn customer already exists for this loan request.");

            var countsCoPawn = existingRequest.CoPawnRequests.Count();

            if (countsCoPawn > 3)
                throw new MaximumException("Cannot add more than 3 co-pawn customers for this loan request.");


            context.CoPawnCustomers.Add(req);

            await context.SaveChangesAsync(cancellation);

            return await GetCoPawnCustomer(req.Id, req.CustomerId, req.BranchId);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to add co-pawn customer for LoanRequestId={LoanRequestId}", req.RequestId);

            throw;
        }
    }

    public async Task<IEnumerable<CoPawnCustomerDto?>> AddCoPawnCustomersAsync(int? branchId, string? requestId, IEnumerable<CoPawnCustomer> reqs, CancellationToken cancellation = default)
    {
        logger.LogInformation("Try to add co-pawn customers");

        await using var context = await contextFactory.CreateDbContextAsync(cancellation);

        try
        {
            var existingRequest = await context.LoanRequests
               .Include(c => c.CoPawnRequests)
               .FirstOrDefaultAsync(x =>
                   x.LoanRequestId == requestId &&
                   x.BranchRefId == branchId, cancellation);

            if (existingRequest is null)
                throw new NotFoundException("Loan Request does not exist.");


            var activeCoPawns = existingRequest.CoPawnRequests
                                             .Where(x => x.Status == true)
                                             .ToList();

            var totalCoPawn = activeCoPawns.Count + reqs.Count();

            if (totalCoPawn > 3)
                throw new MaximumException("Cannot add more than 3 co-pawn customers for this loan request.");


            foreach (var req in reqs)
            {
                var existLoan = await context.EntityNames
                                    .Include(x => x.Customer)
                                    .AnyAsync(x => x.Id == req.CustomerId &&
                                                   x.Customer!.IsLoanActive == true, cancellation);
                if (existLoan)
                    throw new DuplicateException($"Customer {req.CustomerId} has active loan and cannot be added as co-pawn.");

                var customerRequest = await context.LoanRequests
                                    .AnyAsync(x => x.CustomerRefId == req.CustomerId &&
                                                   (x.LoanRequestStatusRefId == (int)RequestStatus.New ||
                                                    x.LoanRequestStatusRefId == (int)RequestStatus.Checking ||
                                                    x.LoanRequestStatusRefId == (int)RequestStatus.Rechecking ||
                                                    x.LoanRequestStatusRefId == (int)RequestStatus.Approved), cancellation);

                if (customerRequest)
                    throw new DuplicateException($"Customer {req.CustomerId} has an active loan request and cannot be added as co-pawn.");



                var isDuplicate = existingRequest.CoPawnRequests
                             .Any(r => r.CustomerId == req.CustomerId && r.Status == true);

                if (isDuplicate)
                    throw new DuplicateException($"Co-Pawn customer {req.CustomerId} already exists for this loan request.");


            }


            context.CoPawnCustomers.AddRange(reqs);

            await context.SaveChangesAsync(cancellation);

            return await GetCoPawnCustomers(branchId, requestId, cancellation);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to add co-pawn customers");

            throw;
        }
    }

    public async Task<IEnumerable<CoPawnCustomerDto?>> AddCoPawnCustomers2Async(int? branchId, string? requestId,
    IEnumerable<CoPawnCustomer> reqs, CancellationToken cancellation = default)
    {
        logger.LogInformation("Try to add co-pawn customers");

        await using var context = await contextFactory.CreateDbContextAsync(cancellation);

        try
        {
            var requests = reqs.ToList();

            if (!requests.Any())
                return Enumerable.Empty<CoPawnCustomerDto?>();

            // Get loan request with existing co-pawn customers
            var existingRequest = await context.LoanRequests
                .Include(x => x.CoPawnRequests)
                .FirstOrDefaultAsync(x =>
                    x.LoanRequestId == requestId &&
                    x.BranchRefId == branchId,
                    cancellation);

            if (existingRequest is null)
                throw new NotFoundException("Loan Request does not exist.");

            // Active existing co-pawn customers
            var existingCustomerIds = existingRequest.CoPawnRequests
                .Where(x => x.Status == true)
                .Select(x => x.CustomerId)
                .ToHashSet();

            // Incoming customer ids
            var requestCustomerIds = requests
                                    .Select(x => x.CustomerId)
                                    .ToList();

            // Prevent duplicate customers in request payload
            var duplicateInRequest = requestCustomerIds
                                    .GroupBy(x => x)
                                    .Where(g => g.Count() > 1)
                                    .Select(g => g.Key)
                                    .FirstOrDefault();

            if (duplicateInRequest is not null)
                throw new DuplicateException(
                    $"Duplicate customer {duplicateInRequest} found in request.");

            // Prevent existing duplicate co-pawn
            var existingDuplicate = requestCustomerIds
                .FirstOrDefault(id => existingCustomerIds.Contains(id));

            if (existingDuplicate is not null)
                throw new DuplicateException(
                    $"Co-Pawn customer {existingDuplicate} already exists for this loan request.");

            // Maximum validation
            var totalCoPawn = existingCustomerIds.Count + requestCustomerIds.Count;

            if (totalCoPawn > 3)
                throw new MaximumException(
                    "Cannot add more than 3 co-pawn customers for this loan request.");

            // Query customers with active loan in ONE query
            var activeLoanCustomerIds = await context.EntityNames
                                            .Include(x => x.Customer)
                                            .Where(x =>
                                                requestCustomerIds.Contains(x.Id) &&
                                                x.Customer!.IsLoanActive == true)
                                            .Select(x => x.Id)
                                            .ToListAsync(cancellation);

            if (activeLoanCustomerIds.Any())
                throw new DuplicateException(
                    $"Customer {activeLoanCustomerIds.First()} has active loan and cannot be added as co-pawn.");

            // Query customers with active loan request in ONE query
            var activeRequestCustomerIds = await context.LoanRequests
                .Where(x =>
                    requestCustomerIds.Contains(x.CustomerRefId) &&
                    (
                        x.LoanRequestStatusRefId == (int)RequestStatus.New ||
                        x.LoanRequestStatusRefId == (int)RequestStatus.Checking ||
                        x.LoanRequestStatusRefId == (int)RequestStatus.Rechecking ||
                        x.LoanRequestStatusRefId == (int)RequestStatus.Approved
                    ))
                .Select(x => x.CustomerRefId)
                .Distinct()
                .ToListAsync(cancellation);

            if (activeRequestCustomerIds.Any())
                throw new DuplicateException(
                    $"Customer {activeRequestCustomerIds.First()} has an active loan request and cannot be added as co-pawn.");

            context.CoPawnCustomers.AddRange(requests);

            await context.SaveChangesAsync(cancellation);

            return await GetCoPawnCustomers(branchId, requestId, cancellation);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to add co-pawn customers");
            throw;
        }
    }


    public async Task<bool> DeleteCoPawnCustomersAsync(CoPawnCustomer req, CancellationToken cancellation = default)
    {
        logger.LogInformation("Try to delete co-pawn customers {BranchId},{RequestId},{CustomerId}", req.BranchId, req.RequestId, req.CustomerId);

        await using var context = await contextFactory.CreateDbContextAsync(cancellation);

        try
        {
            var existingRequest = await context.CoPawnCustomers
               .FirstOrDefaultAsync(x =>
                   x.RequestId == req.RequestId &&
                   x.BranchId == req.BranchId &&
                   x.CustomerId == req.CustomerId, cancellation);

            if (existingRequest is null)
                throw new NotFoundException("Co-Pawn customer does not exist for this loan request.");

            context.CoPawnCustomers.Remove(existingRequest);

            await context.SaveChangesAsync(cancellation);

            return true;

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to add co-pawn customers");

            throw;
        }
    }


    public async Task<bool> IsCoPawnExistAsync(int? branchId, string? customerId, CancellationToken cancellation = default)
    {

        logger.LogInformation("Checking if co-pawn customer exists for CustomerId={CustomerId}", customerId);

        await using var context = await contextFactory.CreateDbContextAsync(cancellation);

        var existingCoPawn = await context.CoPawnCustomers
            .FirstOrDefaultAsync(x => x.CustomerId == customerId &&
                                      x.BranchId == branchId &&
                                      x.Status == true, cancellation);

        return existingCoPawn is not null;
    }

    public async Task<ErrorOr<IEnumerable<VLoanRequestCollateralLines>>> GetLoanRequestCollateralAsync(int? branchId,
        string? loanRequestId)
    {
        logger.LogInformation("try to create new loan request");
        try
        {
            var db = await dapperFactory.CreateConnectionAsync();

            var sql = "SELECT * FROM VLoanRequestCollaterals WHERE branchId=@branchId AND loanRequestId=loanRequestId;";

            var collaterals = await db.QueryAsync<VLoanRequestCollateralLines>(sql, new
            {
                branchId,
                loanRequestId
            });

            if (!collaterals.Any())
            {
                return Error.NotFound("Loan Request does not exist.");
            }

            return ErrorOrFactory.From(collaterals);
        }

        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);

            return Error.Unexpected($"{ex.Message}");
        }
    }

    public async Task<ErrorOr<string>> DeleteLoanRequestCollateralAsync(int? branchId, string? loanRequestId, long? collateralLinkId)
    {
        logger.LogInformation("try to delete guarantors");
        try
        {

            await using var cxt = await contextFactory.CreateDbContextAsync();

            var existingRequest = await cxt.LoanRequests
                .FirstOrDefaultAsync(x => x.LoanRequestId == loanRequestId && x.BranchRefId == branchId);

            if (existingRequest is not null &&
                existingRequest.LoanRequestStatusRefId == (int)RequestStatus.Disbursed)
            {
                return Error.NotFound("cannot remove this request due to it is in used.");
            }

            var gLink = await cxt.CollateralsLinks
                .FirstOrDefaultAsync(g => g.Id == collateralLinkId &&
                                     g.LoanRequestRefId == loanRequestId &&
                                     g.BranchId == branchId);


            if (gLink is null)
            {
                return Error.NotFound(LoanRequestMessage.NO_GUARANTOR);
            }

            existingRequest?.CollateralLinesLink.Remove(gLink);

            cxt.CollateralsLinks.Remove(gLink);

            await cxt.SaveChangesAsync();

            return LoanRequestMessage.GUARANTOR_REMOVED;
        }

        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);

            return Error.Unexpected($"{ex.Message}");
        }
    }

    public async Task<ErrorOr<string>> AddLoanRequestGuarantorAsync(LoanRequestGuarantors request, CancellationToken cancellation = default)
    {

        await using var context = await contextFactory.CreateDbContextAsync();

        logger.LogInformation("try to create new loan request");

        try
        {
            var existingRequest = await context.LoanRequests
                .Include(g => g.GuarantorsLink)
                .ThenInclude(x => x.Guarantor)
                .FirstOrDefaultAsync(x => x.LoanRequestId == request.LoanRequestId &&
                            x.CustomerRefId == request.CustomerRefId &&
                            x.BranchRefId == request.BranchRefId, cancellation);

            if (existingRequest is null)
            {
                return Error.NotFound(LoanRequestMessage.REQUEST_NOT_FOUND);
            }


            var gLinks = request.GuarantorsLink;

            foreach (var item in gLinks)
            {
                var existingGuarantor = existingRequest.GuarantorsLink
                    .Where(x => x.Id == item.Id).FirstOrDefault();

                if (existingGuarantor is null || existingGuarantor.Id == 0)
                {
                    existingRequest.GuarantorsLink.Add(item);
                }
                else
                {
                    context.Entry(existingGuarantor).CurrentValues.SetValues(item);
                }
            }

            await context.SaveChangesAsync(cancellation);

            return LoanRequestMessage.GUARANTOR_ADDED;
        }

        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);

            return Error.Unexpected($"{ex.Message}");
        }
    }

    public async Task<ErrorOr<IEnumerable<VLoanRequestGuarantors>>> GetLoanRequestGuarantorAsync(int? branchId,
        string? loanRequestId, string? customerId)
    {
        logger.LogInformation("try to get guarantors");
        try
        {
            var db = await dapperFactory.CreateConnectionAsync();

            var sql = @"SELECT * FROM VLoanRequestGuarators WHERE LoanRequestId=@LoanRequestId
                               AND BranchId=@BranchId AND CustomerId=@CustomerId";

            var guarantors = await db.QueryAsync<VLoanRequestGuarantors>(sql, new
            {
                loanRequestId,
                branchId,
                customerId
            });

            return ErrorOrFactory.From(guarantors);
        }

        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);

            return Error.Unexpected($"{ex.Message}");
        }
    }

    public async Task<ErrorOr<string>> DeleteLoanRequestGuarantorAsync(string? loanRequestId, int? branchId, long? guarantorLinkId)
    {

        await using var context = await contextFactory.CreateDbContextAsync();


        logger.LogInformation("try to delete guarantors");
        try
        {
            var existingRequest = await context.LoanRequests
                .Where(x => x.LoanRequestId == loanRequestId && x.BranchRefId == branchId)
                .FirstOrDefaultAsync();

            if (existingRequest is not null && existingRequest.LoanRequestStatusRefId == (int)RequestStatus.Disbursed)
            {
                return Error.Conflict(LoanRequestMessage.REQUEST_IN_USED);
            }


            var gLink = context.Guarantors.FirstOrDefault(g => g.Id == guarantorLinkId &&
                                                               g.LoanRequestRefId == loanRequestId &&
                                                               g.BranchId == branchId);
            if (gLink is null)
            {
                return Error.NotFound(LoanRequestMessage.NO_GUARANTOR);
            }

            context.Guarantors.Remove(gLink);

            await context.SaveChangesAsync();

            return "Guarantor is removed.";
        }

        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);

            return Error.Unexpected(ex.Message);
        }
    }


    public async Task<ErrorOr<string>> AddLoanRequestDocumentAsync(LoanDocuments request, CancellationToken cancellation = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync();

        logger.LogInformation("try to create new loan request");

        try
        {
            var existingRequest = await context.LoanRequests
                .Where(x => x.LoanRequestId == request.LoanRequestId &&
                            x.BranchRefId == request.BranchId)
                .Include(x => x.LoanDocuments)
                .FirstOrDefaultAsync(cancellation);

            if (existingRequest is null)
            {
                return Error.NotFound("Loan Request does not exist.");
            }

            var existingDoc = existingRequest.LoanDocuments.FirstOrDefault(x => x.Id == request.Id);

            if (existingDoc is null || request.Id == 0)
            {
                existingRequest.LoanDocuments.Add(request);
            }
            else
            {
                context.Entry(existingDoc).CurrentValues.SetValues(request);
            }

            await context.SaveChangesAsync(cancellation);

            return "Documents were Added.";
        }

        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);

            return Error.Unexpected("NOT ADDED");
        }
    }

    public async Task<ErrorOr<string>> AddLoanRequestDocumentAsync(List<LoanDocuments> documents, CancellationToken cancellation = default)
    {
        logger.LogInformation("try to create new loan request");
        try
        {

            await using var ctx = await contextFactory.CreateDbContextAsync(cancellation);

            if (!documents.Any())
            {
                return Error.NotFound("There is no any document to add.");
            }

            var ids = documents.Where(x => x.Id > 0).Select(x => x.Id).ToList();

            var existingDocs = await ctx.LoanDocuments
                                    .Where(x => ids.Contains(x.Id))
                                    .AsNoTracking()
                                    .ToListAsync(cancellation);

            foreach (var doc in documents)
            {
                if (existingDocs.Any(x => x.Id == doc.Id))
                {
                    ctx.LoanDocuments.Update(doc);
                }
                else
                {
                    ctx.LoanDocuments.Add(doc);
                }

            }

            foreach (var existing in existingDocs)
            {
                if (!documents.Any(x => x.Id == existing.Id))
                {
                    ctx.Remove(existing);
                }
            }

            await ctx.SaveChangesAsync(cancellation);

            return "Documents were Added.";
        }

        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);

            return Error.Unexpected(ex.Message);
        }
    }


    public async Task<ErrorOr<string>> AddLoanRequestDocumentAsync(int? branchId, string? loanRequestId, List<LoanDocuments> documents, CancellationToken cancellation = default)
    {

        await using var context = await contextFactory.CreateDbContextAsync();

        logger.LogInformation("try to create new loan request");

        try
        {

            if (!documents.Any())
            {
                return Error.NotFound("There is no any document to add.");
            }

            var existingDocument = await context.LoanRequests
                                         .Include(x => x.LoanDocuments)
                                         .FirstOrDefaultAsync(x => x.LoanRequestId == loanRequestId &&
                                                                   x.BranchRefId == branchId);
            if (existingDocument is null)
            {
                return Error.NotFound("There is no any document to add.");
            }

            existingDocument.LoanDocuments = documents;

            await context.SaveChangesAsync(cancellation);

            return "Documents were Added.";
        }

        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);

            return Error.Unexpected(ex.Message);
        }
    }

    public async Task<ErrorOr<string>> DeleteLoanRequestDocumentAsync(string? loanRequestId, int? branchId, long? documentId)
    {
        await using var context = await contextFactory.CreateDbContextAsync();

        logger.LogInformation("try to delete documents");
        try
        {
            var existingRequest = await context.LoanRequests
                .Where(x => x.LoanRequestId == loanRequestId && x.BranchRefId == branchId)
                .FirstOrDefaultAsync();

            if (existingRequest is not null && existingRequest.LoanRequestStatusRefId == (int)RequestStatus.Disbursed)
            {
                return Error.Conflict("cannot remove this request due to it is in used.");
            }


            var gLink = context.LoanDocuments.FirstOrDefault(g => g.Id == documentId &&
                                                                  g.LoanRequestId == loanRequestId &&
                                                                  g.BranchId == branchId);
            if (gLink is null)
            {
                return Error.NotFound("There is no document to delete.");
            }


            existingRequest?.LoanDocuments.Remove(gLink);

            await context.SaveChangesAsync();

            return "Guarantor is removed.";
        }

        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);

            return Error.Unexpected(ex.Message);
        }
    }

    public async Task<ErrorOr<VLoanRequests>> GetLoanRequestAsync(int? branchId, string? customerId, string? loanRequestId, bool include)
    {
        try
        {
            using var db = await dapperFactory.CreateConnectionAsync();

            var multiMap = await db.QueryMultipleAsync("spGetLoanRequestForEdit", new
            {
                branchId,
                customerId,
                loanRequestId,
                include
            }, commandType: CommandType.StoredProcedure);

            var requestResult = await multiMap.ReadFirstOrDefaultAsync<VLoanRequests>();

            if (requestResult is null)
            {
                return Error.NotFound(description: "Loan request not found.");
            }

            if (include)
            {
                await PopulateLoanRequestDetailsAsync(multiMap, requestResult);
            }

            return requestResult;
        }
        catch (Exception ex)
        {
            return Error.Unexpected(description: ex.Message);
        }
    }

    private static async Task PopulateLoanRequestDetailsAsync(SqlMapper.GridReader multiMap, VLoanRequests request)
    {
        request.Guarantors = (await multiMap.ReadAsync<VLoanRequestGuarantor>()).AsList();

        request.Descriptions = (await multiMap.ReadAsync<VLoanRequestDescription>()).AsList();

        request.Collaterals = (await multiMap.ReadAsync<VLoanRequestCollateralLines>()).AsList();

        request.Documents = (await multiMap.ReadAsync<VLoanRequestsDocuments>()).AsList();

        request.CoPawnRequests = (await multiMap.ReadAsync<CoPawnCustomerDto>()).AsList();

        request.Customer = await multiMap.ReadFirstOrDefaultAsync<VCustomerDetails>();

        if (request.Customer is not null)
        {
            request.Customer.CustomerIncomes = (await multiMap.ReadAsync<VCustomerIncomes>()).AsList();

            request.Customer.CustomerExpenses = (await multiMap.ReadAsync<VCustomerExpenses>()).AsList();
        }
    }


    public async Task<ErrorOr<LoanRequest>> GetEditLoanRequestAsync(int? branchId, string? customerId, string? loanRequestId, CancellationToken cancellation = default)
    {
        await using var ctx = await contextFactory.CreateDbContextAsync(cancellation);

        var existingRequest = await ctx.LoanRequests
           .Include(x => x.Customer)
                .ThenInclude(x => x!.EntityNames)
           .Include(c => c.CollateralLinesLink)
               .ThenInclude(x => x.CollateralLine)
           .Include(g => g.GuarantorsLink)
               .ThenInclude(x => x.Guarantor)
           .Include(x => x.LoanDocuments)
           .Include(x => x.LoanDescriptions)
           .FirstOrDefaultAsync(x => x.LoanRequestId == loanRequestId &&
                       x.CustomerRefId == customerId &&
                       x.BranchRefId == branchId &&
                       (x.LoanRequestStatusRefId == (int)RequestStatus.New ||
                       x.LoanRequestStatusRefId == (int)RequestStatus.Rechecked), cancellation);


        if (existingRequest is null)
        {
            return Error.NotFound("No Data not found.");
        }
        return existingRequest;
    }


    public async Task<ErrorOr<VLoanRequests>> GetApprovedLoanRequestAsync(int? branchId, string? customerId, string? loanRequestId, bool include)
    {
        logger.LogInformation("try to get approved loan request for LoanRequestId: {LoanRequestId}", loanRequestId);

        try
        {
            using var db = await dapperFactory.CreateConnectionAsync();

            var sql = @"SELECT * FROM VLoanRequests 
                                    WHERE BranchId=@branchId AND 
                                    CustomerId=@customerId AND  
                                    LoanRequestId=@loanRequestId AND
                                    StatusId IN (4)";
            var resultQueries = await db.QueryFirstOrDefaultAsync<VLoanRequests>(sql, new
            {
                branchId,
                customerId,
                loanRequestId
            });

            if (resultQueries is null)
            {
                return Error.NotFound("No Data not found.");
            }

            if (!include)
            {
                return resultQueries;
            }

            // Fetch related data concurrently
            var guarantorsTask = GetLoanGuarantors(loanRequestId, branchId);

            var collateralsTask = GetLoanCollateralLines(loanRequestId, branchId);

            var documentsTask = GetLoanDocuments(loanRequestId, branchId);

            var customerTask = GetLoanCustomer(customerId);

            var descriptionsTask = GetLoanRequestDescriptions(loanRequestId);

            await Task.WhenAll(guarantorsTask, collateralsTask, documentsTask, customerTask, descriptionsTask);

            resultQueries.Guarantors = guarantorsTask.Result.ToList();

            resultQueries.Collaterals = collateralsTask.Result.ToList();

            resultQueries.Documents = documentsTask.Result.ToList();

            resultQueries.Descriptions = descriptionsTask.Result.ToList();

            if (customerTask.Result is { IsError: false, Value: not null })
            {
                resultQueries.Customer = customerTask.Result.Value;
            }

            return resultQueries;

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get approved loan request for LoanRequestId: {LoanRequestId}", loanRequestId);
            return Error.Unexpected($"{ex.Message}");
        }
    }


    public async Task<ErrorOr<VLoanRequests>> GetLoanRequestAsync(int? branchId, string? userId, string? customerId, string? loanRequestId, bool include)
    {
        try
        {
            using var db = await dapperFactory.CreateConnectionAsync();

            const string sql = @"SELECT * FROM VLoanRequests WHERE BranchId=@branchId AND 
                                  CustomerId=@customerId AND  
                                  UserRequestId=@userId AND  
                                  LoanRequestId=@loanRequestId";

            var resultQueries = await db.QueryFirstOrDefaultAsync<VLoanRequests>(sql, new
            {
                branchId,
                customerId,
                loanRequestId,
                userId
            });

            if (resultQueries is null)
            {
                return Error.NotFound("No Data not found.");
            }

            if (!include)
            {
                return resultQueries;
            }

            resultQueries.Guarantors = await GetLoanGuarantors(loanRequestId, branchId);

            resultQueries.Collaterals = await GetLoanCollateralLines(loanRequestId, branchId);

            resultQueries.Documents = await GetLoanDocuments(loanRequestId, branchId);

            var rsp = await GetLoanCustomer(customerId);

            if (!rsp.IsError)
            {
                resultQueries.Customer = rsp.Value;
            }

            return resultQueries;
        }
        catch (Exception ex)
        {
            return Error.Unexpected($"{ex.Message}");
        }
    }


    public async Task<ErrorOr<T>> GetLoanRequestAsync<T>(int? branchId, string? customerId, string? loanRequestId) where T : class
    {
        await using var context = await contextFactory.CreateDbContextAsync();

        try
        {
            var item = await context.VLoanRequests
                .Where(x => x.BranchId == branchId &&
                             x.CustomerId == customerId &&
                             x.LoanRequestId == loanRequestId)
                .AsNoTracking()
                .ProjectToType<T>()
                .FirstOrDefaultAsync();

            if (item is null)
            {
                return Error.NotFound("No Data not found.");
            }

            return item;
        }
        catch (Exception ex)
        {
            return Error.Unexpected($"{ex.Message}");
        }
    }

    public async Task<ErrorOr<IEnumerable<VLoanRequestsDocuments>>> GetLoanRequestDocumentAsync(string? loanRequestId, int? branchId)
    {
        using var db = await dapperFactory.CreateConnectionAsync();

        var sql =
            $"SELECT * FROM [dbo].[VLoanRequestDocuments] WHERE LoanRequestId=@loanRequestId AND branchRefId=@branchId";

        var loanDocuments = await db.QueryAsync<VLoanRequestsDocuments>(sql, new
        {
            loanRequestId,
            branchId
        });

        return ErrorOrFactory.From(loanDocuments);
    }

    public async Task<Pager<VLoanRequests>> GetLoanRequestsAsync(string? userId, bool canDisburse, int? branchId, RequestStatus requestStatus, QueryParam param)// e.g., "Account", "BranchManager", etc.
    {
        await using var cxt = await contextFactory.CreateDbContextAsync();

        if (branchId is null) return new Pager<VLoanRequests>([], 0); // Handle edge case

        var baseQuery = cxt.VLoanRequests
                            .Where(x => x.BranchId == branchId)
                            .OrderByDescending(x => x.RequestDate)
                                .ThenByDescending(x => x.Status)
                            .AsNoTracking();
        // Apply filtering for non-account users

        if (!canDisburse)
        {
            baseQuery = requestStatus switch
            {
                RequestStatus.New or RequestStatus.Rechecked or RequestStatus.Denied =>
                    baseQuery.Where(x => (x.StatusId == (int)RequestStatus.New ||
                                          x.StatusId == (int)RequestStatus.Rechecked ||
                                          x.StatusId == (int)RequestStatus.Denied)
                                         && x.UserRequestId == userId),

                RequestStatus.Requested or RequestStatus.Checking =>
                    baseQuery.Where(x => (x.StatusId == (int)RequestStatus.Requested ||
                                          x.StatusId == (int)RequestStatus.Checking)
                                         && x.AprovedUserId == userId),

                RequestStatus.Approved =>
                    baseQuery.Where(x => x.StatusId == (int)RequestStatus.Approved),

                RequestStatus.All => baseQuery,

                _ => baseQuery.Where(x => x.StatusId == (int)requestStatus)
            };
        }
        else
        {
            // Account role can see all within branch
            if (requestStatus != RequestStatus.All)
            {
                baseQuery = baseQuery.Where(x => x.StatusId == (int)requestStatus);
            }
        }

        // Apply Filter (search)
        if (!string.IsNullOrWhiteSpace(param.Filter))
        {
            baseQuery = baseQuery.Where(x =>
                EF.Functions.Like(x.BranchName!, $"%{param.Filter}%") ||
                EF.Functions.Like(x.AltName!, $"%{param.Filter}%") ||
                EF.Functions.Like(x.FullName!, $"%{param.Filter}%"));
        }

        // Apply sorting
        if (!string.IsNullOrEmpty(param.SortColumn))
        {
            baseQuery = param.SortDescending
                ? baseQuery.OrderByDescending(x => EF.Property<object>(x, param.SortColumn))
                           .ThenByDescending(x => x.Status)
                : baseQuery.OrderBy(x => EF.Property<object>(x, param.SortColumn))
                           .ThenBy(x => x.Status);
        }

        // Paging
        var totalCount = await baseQuery.CountAsync();

        var skip = (param.Page - 1) * param.Take;

        var items = await baseQuery.Skip(param.Page).Take(param.Take).AsNoTracking().ToListAsync();

        return new Pager<VLoanRequests>(items, totalCount);
    }

    private static readonly string[] _loanRequestSearchColumns =
    [
        nameof(VLoanRequests.BranchName),
        nameof(VLoanRequests.AltName),
        nameof(VLoanRequests.FullName)
    ];

    public async Task<Pager<VLoanRequests>> GetLoanRequestsAsync(string? userId, int? branchId,
           RequestStatus requestStatus,
           QueryParam param,
           CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var queries = context.VLoanRequests.WhereIf(branchId is not null && branchId > 0, x => x.BranchId == branchId);

        if (!string.IsNullOrWhiteSpace(param.Filter))
        {
            queries = queries.StartWiths(param.Filter.Trim(), _loanRequestSearchColumns);
        }

        if (!string.IsNullOrEmpty(userId))
        {
            queries = requestStatus switch
            {
                RequestStatus.All
                    => queries.Where(x => x.AprovedUserId == userId || x.UserRequestId == userId),
                RequestStatus.New or RequestStatus.Rechecked or RequestStatus.Denied
                    => queries.Where(x =>
                        (x.StatusId == (int)RequestStatus.New ||
                         x.StatusId == (int)RequestStatus.Rechecked ||
                         x.StatusId == (int)RequestStatus.Denied) &&
                        x.UserRequestId == userId),
                RequestStatus.Requested or RequestStatus.Checking
                    => queries.Where(x =>
                        (x.StatusId == (int)RequestStatus.Requested ||
                         x.StatusId == (int)RequestStatus.Checking) &&
                        x.AprovedUserId == userId),
                RequestStatus.Disbursed
                    => queries.Where(x => x.StatusId == (int)RequestStatus.Disbursed &&
                        (x.AprovedUserId == userId || x.UserRequestId == userId)),
                _ => queries.Where(x => x.StatusId == (int)requestStatus)
            };
        }
        else
        {
            queries = requestStatus switch
            {
                RequestStatus.All => queries,
                _ => queries.Where(x => x.StatusId == (int)requestStatus)
            };
        }

        // --- Sorting
        if (!string.IsNullOrEmpty(param.FilterColumn))
        {
            queries = param.SortDescending
                ? queries.OrderByDescending(x => EF.Property<object>(x, param.FilterColumn))
                          .ThenByDescending(x => x.ModifiedDT)
                          .ThenByDescending(x => x.StatusId)
                          .ThenByDescending(x => x.RequestDate)
                : queries.OrderBy(x => EF.Property<object>(x, param.FilterColumn))
                            .ThenBy(x => x.ModifiedDT)
                            .ThenBy(x => x.StatusId)
                            .ThenBy(x => x.RequestDate);
        }
        else
        {
            queries = param.SortDescending
                ? queries.OrderByDescending(x => x.ModifiedDT)
                                    .ThenByDescending(x => x.Status)
                                    .ThenByDescending(x => x.RequestDate)
                : queries.OrderBy(x => x.ModifiedDT)
                                    .ThenBy(x => x.Status)
                                    .ThenBy(x => x.RequestDate);
        }

        // --- Paging: run count and data fetch concurrently

        var skip = param.Page == 0 ? param.Page : (param.Page - 1) * param.Take;

        var countTask = await queries.CountAsync(cancellationToken);

        var itemsTask = await queries.Skip(skip)
                               .Take(param.Take)
                               .AsNoTracking()
                               .Select(x => new VLoanRequests
                               {
                                   LoanRequestId = x.LoanRequestId,
                                   BranchId = x.BranchId,
                                   CurrencyId = x.CurrencyId,
                                   CustomerId = x.CustomerId,
                                   BranchName = x.BranchName,
                                   AltName = x.AltName,
                                   RequestDate = x.RequestDate,
                                   StatusId = x.StatusId,
                                   Status = x.Status,
                                   CurrencyName = x.CurrencyName,
                                   AprovedAmount = x.AprovedAmount,
                                   InterestRate = x.InterestRate,
                                   CollectionFeeRate = x.CollectionFeeRate,
                                   AdminFeeRate = x.AdminFeeRate,
                                   Summiter = x.Summiter,
                                   AprovedBy = x.AprovedBy,
                                   ModifiedDT = x.ModifiedDT
                               })
                               .ToListAsync(cancellationToken);
        return new Pager<VLoanRequests>(itemsTask, countTask);
    }




    public async Task<ErrorOr<string>> AddDescription(LoanDescriptions descriptions, CancellationToken cancellation = default)
    {
        try
        {
            await using var ctx = await contextFactory.CreateDbContextAsync(cancellation);

            ctx.LoanDescriptions.Add(descriptions);

            await ctx.SaveChangesAsync(cancellation);

            return Messages.DataCreated;
        }
        catch (Exception ex)
        {
            return Error.Unexpected(ex.Message);
        }

    }


    private async Task<CoPawnCustomerDto?> GetCoPawnCustomer(Guid? id, string? customerId, int? branchId, CancellationToken cancellation = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellation);

        const string sql = @"
        SELECT  co.Id,
                co.BranchId, 
                co.RequestId, 
                co.LoanId, 
                co.CustomerId,
                e.AltName AS CustomerName,
                e.Phone AS CustomerPhone,
                e.IdCard As CustomerIdCard,
                CONCAT(e.ProInKhmer, ' ', e.DisInKhmer, ' ', e.ComInKhmer, ' ', e.VilInKhmer) AS CustomerAddress,
                co.Status, 
                co.Description
        FROM dbo.lt_entityNames e
            LEFT JOIN dbo.lt_CoPawnCustomers co ON e.Id = co.CustomerId
        WHERE co.Id={0} AND co.BranchId = {1} AND co.CustomerId = {2}";

        return await context.Database
            .SqlQueryRaw<CoPawnCustomerDto>(sql, id!, branchId!, customerId!)
            .FirstOrDefaultAsync(cancellation);
    }


    private async Task<IEnumerable<CoPawnCustomerDto?>> GetCoPawnCustomers(int? branchId, string? requestId, CancellationToken cancellation = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellation);

        const string sql = @"
        SELECT  co.Id,
                co.BranchId, 
                co.RequestId, 
                co.LoanId, 
                co.CustomerId,
                e.AltName AS CustomerName,
                e.Phone AS CustomerPhone,
                e.IdCard As CustomerIdCard,
                CONCAT(e.ProInKhmer, ' ', e.DisInKhmer, ' ', e.ComInKhmer, ' ', e.VilInKhmer) AS CustomerAddress,
                co.Status, 
                co.Description
        FROM dbo.lt_entityNames e
            LEFT JOIN dbo.lt_CoPawnCustomers co ON e.Id = co.CustomerId
        WHERE co.RequestId={0} AND co.BranchId = {1}";

        return await context.Database
            .SqlQueryRaw<CoPawnCustomerDto>(sql, requestId!, branchId!)
            .ToListAsync(cancellation);
    }

    private async Task<bool> CheckExistingHasLoan(string? customerId)
    {
        if (string.IsNullOrEmpty(customerId)) return false;

        using var db = await dapperFactory.CreateConnectionAsync();

        var parameter = new DynamicParameters();

        parameter.Add("@CustomerId", customerId);

        parameter.Add("@IsLoanActive", true);
        var sql =
            "SELECT CustomerId,FirstName,LastName,IsLoanActive FROM VCustomerList WHERE CustomerId=@CustomerId AND IsLoanActive=@IsLoanActive";
        var customerInActiveLoan = await db.QueryAsync<Customer>(sql, parameter);
        return customerInActiveLoan.Any();
    }

    private async Task<bool> CheckExistingLoanRequest(string? customerId)
    {
        if (string.IsNullOrEmpty(customerId)) return false;

        using var db = await dapperFactory.CreateConnectionAsync();

        var parameters = new DynamicParameters();

        parameters.Add("@CustomerId", customerId);

        var sql = @"SELECT LoanRequestId,CustomerId FROM VLoanRequests WHERE CustomerId=@CustomerId AND StatusId IN (2,3,6)";

        var existingLoanRequest = await db.QueryAsync<VLoanRequests>(sql, parameters);

        return existingLoanRequest.Any();
    }

    private async Task<bool> CheckExistingLoanRequestNotRequested(string? customerId)
    {
        if (string.IsNullOrEmpty(customerId)) return false;

        using var db = await dapperFactory.CreateConnectionAsync();

        var parameters = new DynamicParameters();

        parameters.Add("@CustomerId", customerId);

        var sql = @"SELECT LoanRequestId,CustomerId FROM VLoanRequests WHERE CustomerId=@CustomerId AND StatusId IN (2,3)";

        var existingLoanRequest = await db.QueryAsync<VLoanRequests>(sql, parameters);

        return existingLoanRequest.Any();
    }

    private async Task<IEnumerable<VLoanRequestGuarantor>> GetLoanGuarantors(string? loanRequestId, int? branchId)
    {
        if (!branchId.HasValue || string.IsNullOrEmpty(loanRequestId))
        {
            return Enumerable.Empty<VLoanRequestGuarantor>();
        }

        using var db = await dapperFactory.CreateConnectionAsync();

        const string sql = @"
                            SELECT * 
                            FROM VLoanRequestGuarantors 
                            WHERE LoanRequestRefId = @LoanRequestId 
                            AND BranchId = @BranchId;";

        return await db.QueryAsync<VLoanRequestGuarantor>(
            sql,
            new { LoanRequestId = loanRequestId, BranchId = branchId }
        );
    }

    private async Task<IEnumerable<VLoanRequestDescription>> GetLoanRequestDescriptions(string? loanRequestId)
    {
        if (string.IsNullOrEmpty(loanRequestId))
        {
            return Enumerable.Empty<VLoanRequestDescription>();
        }

        using var db = await dapperFactory.CreateConnectionAsync();
        //LoanRequestRefId
        const string sql = @"
                            SELECT * 
                            FROM VLoanRequestDescriptions 
                            WHERE LoanRequestRefId=@LoanRequestId
                            ORDER BY CreatedDT DESC;";

        return await db.QueryAsync<VLoanRequestDescription>(
            sql,
            new { LoanRequestId = loanRequestId }
        );
    }

    private async Task<IEnumerable<VLoanRequestCollateralLines>> GetLoanCollateralLines(string? loanRequestId, int? branchId)
    {
        if (!branchId.HasValue || string.IsNullOrEmpty(loanRequestId))
        {
            return Enumerable.Empty<VLoanRequestCollateralLines>();
        }

        using var db = await dapperFactory.CreateConnectionAsync();

        const string sql = @"
                            SELECT * 
                                FROM VLoanRequestCollaterals 
                            WHERE LoanRequestId = @LoanRequestId 
                              AND BranchId = @BranchId;";

        return await db.QueryAsync<VLoanRequestCollateralLines>(
            sql,
            new { LoanRequestId = loanRequestId, BranchId = branchId }
        );
    }

    private async Task<IEnumerable<VLoanRequestsDocuments>> GetLoanDocuments(string? loanRequestId, int? branchId)
    {
        if (!branchId.HasValue || string.IsNullOrEmpty(loanRequestId))
        {
            return Enumerable.Empty<VLoanRequestsDocuments>();
        }

        using var db = await dapperFactory.CreateConnectionAsync();

        const string sql = @"
                            SELECT * 
                                FROM VLoanRequestDocuments 
                            WHERE LoanRequestId = @LoanRequestId AND BranchRefId = @BranchId;";

        return await db.QueryAsync<VLoanRequestsDocuments>(
            sql,
            new { LoanRequestId = loanRequestId, BranchId = branchId }
        );
    }

    private async Task<IEnumerable<CoPawnCustomerDto>> GetCoPawnCustomers(string? loanRequestId, int? branchId, CancellationToken cancellation = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellation);

        const string sql = @"
                    SELECT  co.Id,
                            co.BranchId, 
                            co.RequestId, 
                            co.LoanId, 
                            co.CustomerId,
                            e.AltName AS CustomerName,
                            e.Phone AS CustomerPhone,
                            e.IdCard As CustomerIdCard,
                            CONCAT(e.ProInKhmer, ' ', e.DisInKhmer, ' ', e.ComInKhmer, ' ', e.VilInKhmer) AS CustomerAddress,
                            co.Status, 
                            co.Description
                    FROM dbo.lt_entityNames e
                        LEFT JOIN dbo.lt_CoPawnCustomers co ON e.Id = co.CustomerId
                    WHERE co.RequestId={0} AND co.BranchId = {1}";

        return await context.Database
            .SqlQueryRaw<CoPawnCustomerDto>(sql, loanRequestId!, branchId!)
            .ToListAsync(cancellation);
    }

    private async Task<ErrorOr<VCustomerDetails>> GetLoanCustomer(string? customerId)
    {
        if (string.IsNullOrWhiteSpace(customerId))
            return Error.NotFound("Customer ID is required.");

        using var db = await dapperFactory.CreateConnectionAsync();

        var sql = @"
        SELECT c.*,
               c1.IncomeId,
               COALESCE(c1.IncomeAmount, 0) AS IncomeAmount,
               c1.IncomeSource,
               c1.InDescription,
               c2.ExpendId,
               COALESCE(c2.ExpendAmount, 0) AS ExpendAmount,
               c2.ExpenseTitle,
               c2.ExDescription
        FROM VCustomerList AS c
        LEFT JOIN VCustomerIncomes AS c1 ON c1.CustomerRefId = c.CustomerId
        LEFT JOIN VCustomerExpenses AS c2 ON c2.CustomerRefId = c.CustomerId
        WHERE c.CustomerId = @CustomerId;";

        var parameters = new DynamicParameters();
        parameters.Add("@CustomerId", customerId);

        var lookup = new Dictionary<string, VCustomerDetails>();

        await db.QueryAsync<VCustomerDetails, VCustomerIncomes, VCustomerExpenses, VCustomerDetails>(
            sql,
            (customer, income, expense) =>
            {
                if (!lookup.TryGetValue(customer.CustomerId!, out var existingCustomer))
                {
                    customer.CustomerIncomes = new List<VCustomerIncomes>();
                    customer.CustomerExpenses = new List<VCustomerExpenses>();
                    lookup[customer.CustomerId!] = customer;
                }

                var currentCustomer = lookup[customer.CustomerId!];

                if (income?.IncomeId != null)
                    currentCustomer.CustomerIncomes.Add(income);

                if (expense?.ExpendId != null)
                    currentCustomer.CustomerExpenses.Add(expense);

                return currentCustomer;
            },
            param: parameters,
            splitOn: "IncomeId,ExpendId"
        );

        if (!lookup.TryGetValue(customerId, out var finalCustomer))
            return Error.NotFound("Customer data not found.");

        return finalCustomer;
    }

    private void UpdateCollectionAsync<T>(DbContext context, ICollection<T> existingItems, ICollection<T> newItems, Func<T, T, bool> matchPredicate) where T : class
    {
        // Remove items not in new collection
        var itemsToRemove = existingItems
            .Where(existing => !newItems.Any(newItem => matchPredicate(existing, newItem)))
            .ToList();

        foreach (var item in itemsToRemove)
        {
            context.Remove(item);
        }

        // Add or update items
        foreach (var newItem in newItems)
        {
            var existingItem = existingItems.FirstOrDefault(e => matchPredicate(e, newItem));

            if (existingItem is null)
            {
                existingItems.Add(newItem);
            }
            else
            {
                context.Entry(existingItem).CurrentValues.SetValues(newItem);
            }
        }

    }

    private void UpdateExistingRequestAsync(DbContext context, LoanRequest existingRequest, LoanRequest req)
    {
        context.Entry(existingRequest).CurrentValues.SetValues(req);

        // Update Guarantors
        UpdateCollectionAsync(
            context,
            existingRequest.GuarantorsLink,
            req.GuarantorsLink,
            (existing, incoming) => existing.Id == incoming.Id);

        // Update Collateral Lines
        UpdateCollectionAsync(
            context,
            existingRequest.CollateralLinesLink,
            req.CollateralLinesLink,
            (existing, incoming) => existing.Id == incoming.Id);

        // Update CoPawn Requests
        UpdateCollectionAsync(
            context,
            existingRequest.CoPawnRequests,
            req.CoPawnRequests,
            (existing, incoming) => existing.Id == incoming.Id);
    }


    private static IQueryable<VLoanRequests> ApplyStatusFilter(IQueryable<VLoanRequests> query, RequestStatus requestStatus)
    {
        return requestStatus switch
        {
            RequestStatus.All => query,

            RequestStatus.New or RequestStatus.Rechecked or RequestStatus.Denied =>
                query.Where(x => x.StatusId == (int)RequestStatus.New ||
                                 x.StatusId == (int)RequestStatus.Rechecked ||
                                 x.StatusId == (int)RequestStatus.Denied),

            RequestStatus.Requested or RequestStatus.Checking =>
                query.Where(x => x.StatusId == (int)RequestStatus.Requested ||
                                 x.StatusId == (int)RequestStatus.Checking),

            RequestStatus.Approved =>
                query.Where(x => x.StatusId == (int)RequestStatus.Approved),

            _ => query.Where(x => x.StatusId == (int)requestStatus)
        };
    }

    private void AddNotification(DataContext context, LoanRequest req, NotificationType type = NotificationType.New)
    {
        var notification = new Notification
        {
            UserId = req.RequestUserRefId,
            Type = type,
            BranchId = req.BranchRefId,
            IsRead = false,
            Message = "Loan Request",
            Title = "Request",
            ReferenceId = req.LoanRequestId,
            CustomerId = req.CustomerRefId,
            CreatedAt = DateTime.UtcNow,
        };

        context.Notifications.Add(notification);
    }


    private void MergeLoanRequestForCreateOrSave(DataContext context, LoanRequest existingRequest, LoanRequest req)
    {
        context.Entry(existingRequest).CurrentValues.SetValues(req);

        if (req.LoanDescriptions?.Any() == true)
        {
            existingRequest.LoanDescriptions.AddRange(req.LoanDescriptions);
        }

        UpdateCollectionAsync(
            context,
            existingRequest.GuarantorsLink,
            req.GuarantorsLink,
            (existing, incoming) => existing.Id == incoming.Id);

        UpdateCollectionAsync(
            context,
            existingRequest.CollateralLinesLink,
            req.CollateralLinesLink,
            (existing, incoming) => existing.Id == incoming.Id);

        UpdateCollectionAsync(
            context,
            existingRequest.CoPawnRequests,
            req.CoPawnRequests,
            (existing, incoming) => existing.Id == incoming.Id);
    }

}

//public async Task<ErrorOr<RequestResult>> SaveLoanRequestAsync(LoanRequest req, CancellationToken cancellationToken = default)
//{
//    logger.LogInformation("Creating loan request for CustomerRefId: {CustomerRefId}", req.CustomerRefId);
//    try
//    {
//        await using var cxt = await contextFactory.CreateDbContextAsync(cancellationToken);

//        if (await CheckExistingLoanRequestNotRequested(req.CustomerRefId))
//        {
//            return Error.Conflict("Loan Request already exists.");
//        }

//        if (await CheckExistingHasLoan(req.CustomerRefId))
//        {
//            return Error.Conflict("Customer has active loan.");
//        }

//        var existingRequest = await cxt.LoanRequests
//            .Include(c => c.CollateralLinesLink)
//                .ThenInclude(x => x.CollateralLine)
//            .Include(g => g.GuarantorsLink)
//                .ThenInclude(x => x.Guarantor)
//            .Include(x => x.LoanDocuments)
//            .Include(x => x.LoanDescriptions)
//            .FirstOrDefaultAsync(x => x.LoanRequestId == req.LoanRequestId &&
//                        x.CustomerRefId == req.CustomerRefId &&
//                        (x.LoanRequestStatusRefId == (int)RequestStatus.New ||
//                        x.LoanRequestStatusRefId == (int)RequestStatus.Rechecked), cancellationToken);

//        using var transaction = await cxt.Database.BeginTransactionAsync(cancellationToken);

//        try
//        {
//            if (existingRequest is null)
//            {
//                cxt.LoanRequests.Add(req);
//            }
//            else
//            {
//                cxt.Entry(existingRequest).CurrentValues.SetValues(req);

//                if (req.LoanDescriptions.Any())
//                {
//                    existingRequest.LoanDescriptions.AddRange(req.LoanDescriptions);
//                }

//                foreach (var item in req.GuarantorsLink)
//                {
//                    var existingGuarantor = existingRequest.GuarantorsLink.FirstOrDefault(x => x.Id == item.Id);

//                    if (existingGuarantor is null || existingGuarantor.Id == 0)
//                    {
//                        existingRequest.GuarantorsLink.Add(item);
//                    }
//                    else
//                    {
//                        cxt.Entry(existingGuarantor).CurrentValues.SetValues(item);
//                    }
//                }

//                foreach (var item in req.CollateralLinesLink)
//                {
//                    var existingCollateralLine =
//                        existingRequest.CollateralLinesLink.FirstOrDefault(x => x.Id == item.Id);

//                    if (existingCollateralLine is null || existingCollateralLine.Id == 0)
//                    {
//                        existingRequest.CollateralLinesLink.Add(item);
//                    }
//                    else
//                    {
//                        cxt.Entry(existingCollateralLine).CurrentValues.SetValues(item);
//                    }
//                }

//                foreach (var item in existingRequest.CollateralLinesLink)
//                {
//                    if (!(req.CollateralLinesLink.Any(x => x.Id == item.Id)))
//                    //context.Remove(item);
//                    {
//                        cxt.Remove(item);
//                    }
//                }

//                foreach (var item in existingRequest.GuarantorsLink)
//                {
//                    if (!(req.GuarantorsLink.Any(x => x.Id == item.Id)))
//                    //context.Remove(item);
//                    {
//                        cxt.Remove(item);
//                    }
//                }


//            }

//            AddNotification(cxt, req, NotificationType.New);

//            await cxt.SaveChangesAsync(cancellationToken);

//            await transaction.CommitAsync(cancellationToken);

//            return new RequestResult(true, "New Loan Request was created.");
//        }
//        catch (Exception ex)
//        {
//            await transaction.RollbackAsync(cancellationToken);

//            return Error.Unexpected(ex.Message);
//        }

//    }

//    catch (Exception ex)
//    {
//        logger.LogError(ex, "Failed to create loan request for CustomerRefId: {CustomerRefId}", req.CustomerRefId);
//        return Error.Unexpected(ex.Message);
//    }
//}
