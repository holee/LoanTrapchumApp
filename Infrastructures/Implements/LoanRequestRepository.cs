using Applications.Commons.Mappers;
using Applications.Commons.Messages.Loanrequests;
using Applications.Enumerations;
using Applications.Extensions;
using Applications.Features.CreditAgents.Requests;
using Applications.Features.Customers.Response;
using Applications.Features.LoanRequests.Requests;
using Applications.Features.Loans.Responses;
using Applications.Interfaces.Loans;
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
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructures.Implements;

public sealed class LoanRequestRepository(
    DataContext context,
    ILogger<LoanRequestRepository> logger,
    IDapperConnectionFactory dapperFactory,
    IDbContextFactory<DataContext> contextFactory) : ILoanRequestService
{
    public async Task<ErrorOr<RequestResult>> CreateNewLoanRequestAsync(LoanRequest req, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Creating loan request for CustomerRefId: {CustomerRefId}", req.CustomerRefId);

        await using var cxt = await contextFactory.CreateDbContextAsync();

        if (await CheckExistingLoanRequest(req.CustomerRefId))
        {
            return Error.Conflict("Loan Request already exists.");
        }

        if (await CheckExistingHasLoan(req.CustomerRefId))
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
            .FirstOrDefaultAsync(x => x.LoanRequestId == req.LoanRequestId &&
                        x.CustomerRefId == req.CustomerRefId &&
                        x.LoanRequestStatusRefId == (int)RequestStatus.New, cancellationToken);

        using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            if (existingRequest is null)
            {
                cxt.LoanRequests.Add(req);
            }
            else
            {
                cxt.Entry(existingRequest).CurrentValues.SetValues(req);

                if (req.LoanDescriptions.Any())
                {
                    existingRequest.LoanDescriptions.AddRange(req.LoanDescriptions);
                }

                foreach (var item in req.GuarantorsLink)
                {
                    var existingGuarantor = existingRequest.GuarantorsLink.FirstOrDefault(x => x.Id == item.Id);

                    if (existingGuarantor is null || existingGuarantor.Id == 0)
                    {
                        existingRequest.GuarantorsLink.Add(item);
                    }
                    else
                    {
                        cxt.Entry(existingGuarantor).CurrentValues.SetValues(item);
                    }
                }

                foreach (var item in req.CollateralLinesLink)
                {
                    var existingCollateralLine =
                        existingRequest.CollateralLinesLink.FirstOrDefault(x => x.Id == item.Id);

                    if (existingCollateralLine is null || existingCollateralLine.Id == 0)
                    {
                        existingRequest.CollateralLinesLink.Add(item);
                    }
                    else
                    {
                        cxt.Entry(existingCollateralLine).CurrentValues.SetValues(item);
                    }
                }

                foreach (var item in existingRequest.CollateralLinesLink)
                {
                    if (!(req.CollateralLinesLink.Any(x => x.Id == item.Id)))
                    {
                        cxt.Remove(item);
                    }
                }

                foreach (var item in existingRequest.GuarantorsLink)
                {
                    if (!(req.GuarantorsLink.Any(x => x.Id == item.Id)))
                    {
                        cxt.Remove(item);
                    }
                }

            }

            AddNotification(cxt, req, NotificationType.Request);

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

    public async Task<ErrorOr<RequestResult>> SaveLoanRequestAsync(LoanRequest req, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Creating loan request for CustomerRefId: {CustomerRefId}", req.CustomerRefId);
        try
        {
            await using var cxt = await contextFactory.CreateDbContextAsync(cancellationToken);

            if (await CheckExistingLoanRequestNotRequested(req.CustomerRefId))
            {
                return Error.Conflict("Loan Request already exists.");
            }

            if (await CheckExistingHasLoan(req.CustomerRefId))
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
                .FirstOrDefaultAsync(x => x.LoanRequestId == req.LoanRequestId &&
                            x.CustomerRefId == req.CustomerRefId &&
                            (x.LoanRequestStatusRefId == (int)RequestStatus.New ||
                            x.LoanRequestStatusRefId == (int)RequestStatus.Rechecked), cancellationToken);

            using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                if (existingRequest is null)
                {
                    cxt.LoanRequests.Add(req);
                }
                else
                {
                    cxt.Entry(existingRequest).CurrentValues.SetValues(req);

                    if (req.LoanDescriptions.Any())
                    {
                        existingRequest.LoanDescriptions.AddRange(req.LoanDescriptions);
                    }

                    foreach (var item in req.GuarantorsLink)
                    {
                        var existingGuarantor = existingRequest.GuarantorsLink.FirstOrDefault(x => x.Id == item.Id);

                        if (existingGuarantor is null || existingGuarantor.Id == 0)
                        {
                            existingRequest.GuarantorsLink.Add(item);
                        }
                        else
                        {
                            cxt.Entry(existingGuarantor).CurrentValues.SetValues(item);
                        }
                    }

                    foreach (var item in req.CollateralLinesLink)
                    {
                        var existingCollateralLine =
                            existingRequest.CollateralLinesLink.FirstOrDefault(x => x.Id == item.Id);

                        if (existingCollateralLine is null || existingCollateralLine.Id == 0)
                        {
                            existingRequest.CollateralLinesLink.Add(item);
                        }
                        else
                        {
                            cxt.Entry(existingCollateralLine).CurrentValues.SetValues(item);
                        }
                    }

                    foreach (var item in existingRequest.CollateralLinesLink)
                    {
                        if (!(req.CollateralLinesLink.Any(x => x.Id == item.Id)))
                        {
                            cxt.Remove(item);
                        }
                    }

                    foreach (var item in existingRequest.GuarantorsLink)
                    {
                        if (!(req.GuarantorsLink.Any(x => x.Id == item.Id)))
                        {
                            cxt.Remove(item);
                        }
                    }


                }

                AddNotification(cxt, req, NotificationType.New);

                await cxt.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                return new RequestResult(true, "New Loan Request was created.");
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
            using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

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

                var type = req.LoanRequestStatusRefId switch
                {
                    (int)RequestStatus.New => NotificationType.New,
                    (int)RequestStatus.Requested => NotificationType.Request,
                    _ => NotificationType.Request
                };

                AddNotification(cxt, req, type);

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

    public async Task<ErrorOr<string>> ChangedLoanRequestAsync(LoanChangeStatusDtoCreate req)
    {
        var existingLoanRequest = await context.LoanRequests
            .Where(x => x.LoanRequestId == req.LoanRequestId && x.BranchRefId == req.BranchId)
            .FirstOrDefaultAsync();

        if (existingLoanRequest is null)
        {
            return Error.NotFound(LoanRequestMessage.REQUEST_NOT_FOUND);
        }

        var lastEditSequent = existingLoanRequest.EditSeq + 1;

        var updateResult = await context.LoanRequests
            .Where(x => x.LoanRequestId == req.LoanRequestId && x.BranchRefId == req.BranchId)
            .ExecuteUpdateAsync(prop => prop.SetProperty(p => p.LoanRequestStatusRefId, req.StatusId)
                .SetProperty(p => p.ModifiedDt, DateTime.UtcNow)
                .SetProperty(p => p.EditSeq, lastEditSequent));

        return updateResult > 0
            ? "Loan Request status was changed"
            : Error.Unexpected("Cannot changed loan request.");
    }

    public async Task<ErrorOr<RequestResult>> AprovedLoanRequestAsync(AproveLoanDtoCreate req, CancellationToken cancellationToken = default)
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
        using var transaction = await cxt.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            existingLoanRequest.LoanRequestStatusRefId = (int)RequestStatus.Approved;
            existingLoanRequest.ModifiedDt = DateTime.UtcNow;
            existingLoanRequest.AprovedDate = req.AprovedDate;
            existingLoanRequest.AprovedAmount = req.AprovedAmount;
            existingLoanRequest.AdminFeeAmount = req.AdminFeeAmount;
            AddNotification(cxt, existingLoanRequest, NotificationType.Approved);
            await cxt.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return new RequestResult(true, "Loan Request was Aproved");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
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
            existingRequest.ModifiedDt = DateTime.UtcNow;
            existingRequest.AprovedDate = req.DeniedDate;
            existingRequest.Remark = req.Remark;
            await context.SaveChangesAsync(cancellationToken);
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
            await context.SaveChangesAsync(cancellationToken);
            return new RequestResult(true, "Loan Request was Verified");
        }
        catch (Exception ex)
        {
            return Error.Unexpected($"{ex.Message}");
        }
    }

    public async Task<ErrorOr<RequestResult>> RecheckedLoanRequestAsync(RecheckedRequestDtoCreate req, CancellationToken cancellationToken = default)
    {
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
                return ErrorOr.Error.NotFound(Messages.DataNotFound);
            }

            var updateResult = await cxt.LoanRequests
                .FirstOrDefaultAsync(x => x.LoanRequestId == req.LoanRequestId &&
                            x.BranchRefId == req.BranchId &&
                            x.EditSeq == existingLoanRequest.EditSeq &&
                            x.CustomerRefId == req.CustomerId, cancellationToken);

            if (updateResult is null)
            {
                return ErrorOr.Error.NotFound(Messages.DataNotFound);
            }
            updateResult.LoanRequestStatusRefId = (int)RequestStatus.Rechecked;
            updateResult.ModifiedDt = DateTime.UtcNow;
            updateResult.AprovedDate = req.CheckedDate;
            updateResult.Remark = req.Remark;
            await context.SaveChangesAsync(cancellationToken);
            return new RequestResult
            {
                Success = true,
                Message = "Loan Request was Rechecked"
            };

        }
        catch (Exception ex)
        {
            return ErrorOr.Error.Unexpected(($"Error due to : {ex.Message}"));
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

    public async Task<ErrorOr<string>> DeleteLoanRequestCollateralAsync(int? branchId, string? loanRequestId,
        long? collateralLinkId)
    {
        logger.LogInformation("try to delete guarantors");
        try
        {

            await using var cxt = await contextFactory.CreateDbContextAsync();

            var existingRequest = await cxt.LoanRequests
                .FirstOrDefaultAsync(x => x.LoanRequestId == loanRequestId && x.BranchRefId == branchId);

            if (existingRequest is not null &&
                existingRequest.LoanRequestStatusRefId == (int)RequestStatus.Completed)
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

            //foreach (var item in existingRequest.GuarantorsLink)
            //{
            //    if (!(collateralLinks.Any(x => x.Id == item.Id)))
            //    {
            //        context.Remove(item);
            //    }
            //}


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

    public async Task<ErrorOr<string>> DeleteLoanRequestGuarantorAsync(string? loanRequestId, int? branchId,
        long? guarantorLinkId)
    {
        logger.LogInformation("try to delete guarantors");
        try
        {
            var existingRequest = await context.LoanRequests
                .Where(x => x.LoanRequestId == loanRequestId && x.BranchRefId == branchId)
                .FirstOrDefaultAsync();

            if (existingRequest is not null && existingRequest.LoanRequestStatusRefId == (int)RequestStatus.Completed)
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

    public async Task<ErrorOr<string>> DeleteLoanRequestDocumentAsync(string? loanRequestId, int? branchId,
        long? documentId)
    {
        logger.LogInformation("try to delete documents");
        try
        {
            var existingRequest = await context.LoanRequests
                .Where(x => x.LoanRequestId == loanRequestId && x.BranchRefId == branchId)
                .FirstOrDefaultAsync();

            if (existingRequest is not null && existingRequest.LoanRequestStatusRefId == (int)RequestStatus.Completed)
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

            var sql = @"SELECT * 
                        FROM VLoanRequests WHERE BranchId=@branchId AND 
                            CustomerId=@customerId AND LoanRequestId=@loanRequestId";

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
            var guarantorsTask = GetLoanGuarantors(loanRequestId, branchId);
            var collateralsTask = GetLoanCollateralLines(loanRequestId, branchId);
            var documentsTask = GetLoanDocuments(loanRequestId, branchId);
            var descriptionsTask = GetLoanRequestDescriptions(loanRequestId);
            var customerTask = GetLoanCustomer(customerId);
          
            await Task.WhenAll(guarantorsTask, collateralsTask, documentsTask, descriptionsTask, customerTask);

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
            return Error.Unexpected($"{ex.Message}");
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

        var skip = (param.Skip - 1) * param.PageSize;

        var items = await baseQuery.Skip(param.Skip).Take(param.PageSize).AsNoTracking().ToListAsync();

        return new Pager<VLoanRequests>(items, totalCount);
    }

    public async Task<Pager<VLoanRequests>> GetLoanRequestsAsync(string? userId, int? branchId, RequestStatus requestStatus, QueryParam param)
    {
        await using var context = await contextFactory.CreateDbContextAsync();

        IQueryable<VLoanRequests> query = context.VLoanRequests
            .Where(x => x.BranchId == branchId)
            .WhereIf(!string.IsNullOrEmpty(param.Filter),
                x => EF.Functions.Like(x.BranchName!, $"%{param.Filter}%") ||
                     EF.Functions.Like(x.AltName!, $"%{param.Filter}%") ||
                     EF.Functions.Like(x.FullName!, $"%{param.Filter}%"));

        // --- CASE 1: Anonymous (no userId)
        if (string.IsNullOrEmpty(userId))
        {
            query = query.Where(x => x.StatusId == (int)RequestStatus.Approved &&
                                     x.BranchId == branchId);
        }
        else
        {
            // --- CASE 2: Authenticated user (has userId)
            query = requestStatus switch
            {
                RequestStatus.All
                    => query.Where(x => x.AprovedUserId == userId || x.UserRequestId == userId), // no filter
                RequestStatus.New or RequestStatus.Rechecked or RequestStatus.Denied
                    => query.Where(x =>
                        (x.StatusId == (int)RequestStatus.New ||
                         x.StatusId == (int)RequestStatus.Rechecked ||
                         x.StatusId == (int)RequestStatus.Denied) &&
                        x.UserRequestId == userId),

                RequestStatus.Requested or RequestStatus.Checking
                    => query.Where(x =>
                        (x.StatusId == (int)RequestStatus.Requested ||
                         x.StatusId == (int)RequestStatus.Checking) &&
                        x.AprovedUserId == userId),

                RequestStatus.Completed
                    => query.Where(x => x.StatusId == (int)RequestStatus.Completed &&
                        (x.AprovedUserId == userId || x.UserRequestId == userId)),

                _ => query.Where(x => x.StatusId == (int)requestStatus)
            };
        }

        // --- Sorting
        if (param.SortDescending)
            query = query.OrderByDescending(x => EF.Property<object>(x, param.SortColumn));
        else
            query = query.OrderBy(x => EF.Property<object>(x, param.SortColumn));

        // --- Paging
        var totalCount = await query.CountAsync();
        var items = await query.Skip(param.Skip).Take(param.PageSize).AsNoTracking().ToListAsync();

        return new Pager<VLoanRequests>(items, totalCount);
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
            (existing, newItem) => existing.Id == newItem.Id);

        // Update Collateral Lines
        UpdateCollectionAsync(
            context,
            existingRequest.CollateralLinesLink,
            req.CollateralLinesLink,
            (existing, newItem) => existing.Id == newItem.Id);
    }

    public async Task<Pager<VLoanRequests>> GetLoanRequestsAsync(string? userId, string? userRole, int? branchId, RequestStatus requestStatus, QueryParam param)
    {
        await using var context = await contextFactory.CreateDbContextAsync();

        IQueryable<VLoanRequests> query = context.VLoanRequests
            .Where(x => x.BranchId == branchId)
            .WhereIf(!string.IsNullOrEmpty(param.Filter),
                x => EF.Functions.Like(x.BranchName!, $"%{param.Filter}%") ||
                     EF.Functions.Like(x.AltName!, $"%{param.Filter}%") ||
                     EF.Functions.Like(x.FullName!, $"%{param.Filter}%"));

        // CASE 1: Anonymous user
        if (string.IsNullOrEmpty(userId))
        {
            query = query.Where(x => x.StatusId == (int)RequestStatus.Approved);
        }
        else
        {
            // CASE 2: Authenticated user — filter based on role
            switch (userRole?.ToLower())
            {
                case "admin":
                case "manager":
                    // Admin or manager can view all loan requests for the branch
                    query = ApplyStatusFilter(query, requestStatus);
                    break;

                case "loanofficer":
                    // Loan officer sees only their own requests
                    query = ApplyStatusFilter(query.Where(x => x.UserRequestId == userId), requestStatus);
                    break;

                case "approver":
                case "checker":
                    // Approvers/checkers see requests assigned to them
                    query = ApplyStatusFilter(query.Where(x => x.AprovedUserId == userId), requestStatus);
                    break;

                default:
                    // Default fallback: treat as normal user
                    query = ApplyStatusFilter(query.Where(x => x.UserRequestId == userId), requestStatus);
                    break;
            }
        }

        // Sorting
        query = param.SortDescending
            ? query.OrderByDescending(x => EF.Property<object>(x, param.SortColumn))
            : query.OrderBy(x => EF.Property<object>(x, param.SortColumn));

        // Paging
        var totalCount = await query.CountAsync();
        var items = await query.Skip(param.Skip).Take(param.PageSize).AsNoTracking().ToListAsync();

        return new Pager<VLoanRequests>(items, totalCount);
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

}


