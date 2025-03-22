using Applications.Enumerations;
using Applications.Interfaces.Loans;
using Applications.Requests.Loans.CreditAgents;
using Applications.Requests.Loans.LoanRequests;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Customers;
using Applications.Responses.ViewModels.Loans.LoanRequests;
using Applications.Responses.ViewModels.Loans.Requests;
using Dapper;
using Domains.Models.Collaterals;
using Domains.Models.Lists;
using Domains.Models.Loans;
using Infrastructures.DataAccess;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructures.Implements;

public class LoanRequestService(ApplicationDbContext context, ILogger<LoanRequestService> logger, IDapperConnectionFactory dapperFactory) : ILoanRequestService
{
    public async Task<Result<string>> CreateLoanRequestAsync(LoanRequest request)
    {
        logger.LogInformation("try to create new loan request");
        try
        {

            var existingLoanRequest = await context.Database
                                                    .SqlQuery<VLoanRequestVm>($"SELECT * FROM VCustomerLoanRequests WHERE CustomerId={request.CustomerRefId}")
                                                    .Where(x => x.IsActive == true).ToListAsync();
            if (existingLoanRequest.Any())
            {
                return Result.Failure<string>(ErrorResponse.Conflict("this customer is already on loan request."));

            }
            var existingRequest = await context.LoanRequests
                                    .Where(x => x.Id == request.Id && x.CustomerRefId == request.CustomerRefId)
                                    .Include(c => c.CollateralLinesLink)
                                            .ThenInclude(x => x.CollateralLine)
                                    .Include(g => g.GuarantorsLink!)
                                            .ThenInclude(x => x.Guarantor)
                                    .Include(x => x.LoanDocuments)
                                    .FirstOrDefaultAsync();

            if (existingRequest is null)
            {
                context.LoanRequests.Add(request);
            }
            else
            {
                context.Entry(existingRequest).CurrentValues.SetValues(request);

                foreach (var item in request.GuarantorsLink)
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


                foreach (var item in request.CollateralLinesLink)
                {

                    var existingCollateralLine = existingRequest.CollateralLinesLink
                                                            .Where(x => x.Id == item.Id)
                                                            .FirstOrDefault();
                    if (existingCollateralLine is null || existingCollateralLine.Id == 0)
                    {
                        existingRequest.CollateralLinesLink.Add(item);
                    }
                    else
                    {
                        context.Entry(existingCollateralLine).CurrentValues.SetValues(item);
                    }
                }


                foreach (var item in existingRequest.CollateralLinesLink)
                {
                    if (!request.CollateralLinesLink.Any(x => x.Id == item.Id))
                    {
                        context.Remove(item);
                    }
                }

                foreach (var item in existingRequest.GuarantorsLink)
                {
                    if (!(request.GuarantorsLink.Any(x => x.Id == item.Id)))
                    {
                        context.Remove(item);
                    }
                }

            }

            await context.SaveChangesAsync();

            return Result.Success(new string("New Loan Request was created."));

        }

        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);

            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));

        }

    }

    public async Task<Result<string>> EditLoanRequestAsync(LoanRequest request)
    {
        logger.LogInformation("try to create new loan request");
        try
        {
            context.LoanRequests.Update(request);
            await context.SaveChangesAsync();
            return Result.Success(new string("New Loan Request was updated."));
        }
        catch (DbUpdateConcurrencyException ex)
        {
            logger.LogError(ex, ex.Message);

            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));

        }
    }

    /// <summary>
    /// Loan Request and Collaterals
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public async Task<Result<string>> AddLoanRequestCollateralAsync(LoanCollateralRequest req)
    {
        logger.LogInformation("try to create new loan request");
        try
        {
            var existingRequest = await context.LoanRequests
                                    .Where(x => x.LoanRequestId == req.LoanRequestId && x.CustomerRefId == req.CustomerId && x.BranchRefId == req.BranchId)
                                    .Include(c => c.CollateralLinesLink)
                                            .ThenInclude(x => x.CollateralLine)
                                    .FirstOrDefaultAsync();

            if (existingRequest is null)
            {
                return Result.Failure<string>(ErrorResponse.Conflict("Loan Request does not exist."));
            }

            var collerals = req.CollateralLinesLink.Adapt<IEnumerable<CollateralLink>>();

            foreach (var item in collerals)
            {

                var existingCollateralLine = existingRequest.CollateralLinesLink
                                                        .Where(x => x.Id == item.Id)
                                                        .FirstOrDefault();
                if (existingCollateralLine is null || existingCollateralLine.Id == 0)
                {
                    existingRequest.CollateralLinesLink.Add(item);
                }
                else
                {
                    context.Entry(existingCollateralLine).CurrentValues.SetValues(item);
                }
            }

            foreach (var item in existingRequest.CollateralLinesLink)
            {
                if (!collerals.Any(x => x.Id == item.Id))
                {
                    context.Remove(item);
                }
            }
            await context.SaveChangesAsync();

            return Result.Success(new string("Collateal was Added."));

        }

        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);

            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));

        }

    }

    public async Task<Result<IEnumerable<VLoanRequestCollateralLines>>> GetLoanRequestCollateralAsync(int? branchId, Guid? loanRequestId)
    {
        logger.LogInformation("try to create new loan request");
        try
        {
            var db = dapperFactory.CreateConnection();
            var sql = "SELECT * FTOM VLoanRequestCollaterals WHERE branchId=@branchId AND loanRequestId=loanRequestId;";
            var collaterals = await db.QueryAsync<VLoanRequestCollateralLines>(sql, new
            {
                branchId,
                loanRequestId
            });

            if (!collaterals.Any())
            {
                return Result.Failure<IEnumerable<VLoanRequestCollateralLines>>(ErrorResponse.NotFound("Loan Request does not exist."));
            }

            return Result.Success(collaterals);

        }

        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);

            return Result.Failure<IEnumerable<VLoanRequestCollateralLines>>(ErrorResponse.ServerError($"{ex.Message}"));

        }

    }

    public async Task<Result<string>> DeleteLoanRequestCollateralAsync(int? branchId, Guid? loanRequestId, long? collateralLinkId)
    {
        logger.LogInformation("try to delete guarantors");
        try
        {

            var existingRequest = await context.LoanRequests
                                    .Where(x => x.LoanRequestId == loanRequestId && x.BranchRefId == branchId)
                                    .FirstOrDefaultAsync();

            if (existingRequest is not null && existingRequest.LoanRequestStatusRefId == (int)RequestStatus.Completed)
            {
                return Result.Failure<string>(ErrorResponse.NotFound("cannot remove this request due to it is in used."));
            }

            var gLink = context.CollateralsLinks.Where(g => g.Id == collateralLinkId &&
                                                      g.LoanRequestRefId == loanRequestId &&
                                                      g.BranchId == branchId)
                                        .FirstOrDefault();


            if (gLink is null)
            {
                return Result.Failure<string>(ErrorResponse.NotFound("There is no any guarantor link."));
            }

            existingRequest?.CollateralLinesLink.Remove(gLink);

            context.CollateralsLinks.Remove(gLink);

            await context.SaveChangesAsync();

            return Result.Success(new string("Guarantor is removed."));
        }

        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);

            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));

        }

    }


    /// <summary>
    /// Loan Request and guarantors
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<Result<string>> AddLoanRequestGuarantorAsync(LoanRequestGuarantors request)
    {
        logger.LogInformation("try to create new loan request");
        try
        {
            var existingRequest = await context.LoanRequests
                                    .Where(x => x.LoanRequestId == request.LoanRequestId &&
                                                x.CustomerRefId == request.CustomerRefId &&
                                                x.BranchRefId == request.BranchRefId)
                                    .Include(g => g.GuarantorsLink!)
                                            .ThenInclude(x => x.Guarantor)
                                    .FirstOrDefaultAsync();

            if (existingRequest is null)
            {
                return Result.Failure<string>(ErrorResponse.NotFound("Loan Request does not exist."));
            }


            var collateralLinks = request.GuarantorsLink.Adapt<IEnumerable<GuarantorLink>>();


            foreach (var item in collateralLinks)
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

            foreach (var item in existingRequest.GuarantorsLink)
            {
                if (!(collateralLinks.Any(x => x.Id == item.Id)))
                {
                    context.Remove(item);
                }
            }


            await context.SaveChangesAsync();

            return Result.Success(new string("Guarnator Added."));

        }

        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);

            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));

        }

    }
    public async Task<Result<IEnumerable<VLoanRequestGuarantors>>> GetLoanRequestGuarantorAsync(Guid loanRequestId, int branchId, long customerId)
    {
        logger.LogInformation("try to get guarantors");
        try
        {
            var db = dapperFactory.CreateConnection();

            var sql = @"SELECT * FROM VLoanRequestGuarators WHERE LoanRequestId=@LoanRequestId
                               AND BranchId=@BranchId AND CustomerId=@CustomerId";

            var guarantors = await db.QueryAsync<VLoanRequestGuarantors>(sql, new
            {
                loanRequestId,
                branchId,
                customerId
            });

            return Result.Success(guarantors);
        }

        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);

            return Result.Failure<IEnumerable<VLoanRequestGuarantors>>(ErrorResponse.ServerError($"{ex.Message}"));

        }

    }
    public async Task<Result<string>> DeleteLoanRequestGuarantorAsync(Guid? loanRequestId, int? branchId, long? guarantorLinkId)
    {
        logger.LogInformation("try to delete guarantors");
        try
        {

            var existingRequest = await context.LoanRequests
                                    .Where(x => x.LoanRequestId == loanRequestId && x.BranchRefId == branchId)
                                    .FirstOrDefaultAsync();

            if (existingRequest is not null && existingRequest.LoanRequestStatusRefId == (int)RequestStatus.Completed)
            {
                return Result.Failure<string>(ErrorResponse.Conflict("cannot remove this request due to it is in used."));
            }


            var gLink = context.Guarantors.Where(g => g.Id == guarantorLinkId &&
                                                      g.LoanRequestRefId == loanRequestId &&
                                                      g.BranchId == branchId)
                                        .FirstOrDefault();


            if (gLink is null)
            {
                return Result.Failure<string>(ErrorResponse.NotFound("There is no any guarantor link."));
            }

            //existingRequest.GuarantorsLink.Remove(gLink);

            context.Guarantors.Remove(gLink);

            await context.SaveChangesAsync();

            return Result.Success(new string("Guarantor is removed."));
        }

        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);

            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));

        }

    }



    /// <summary>
    /// Loan Request and Document
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<Result<string>> AddLoanRequestDocumentAsync(LoanRequestDocuments request)
    {
        logger.LogInformation("try to create new loan request");
        try
        {

            var existingRequest = await context.LoanRequests
                                    .Where(x => x.LoanRequestId == request.LoanRequestId &&
                                                x.CustomerRefId == request.CustomerRefId &&
                                                x.BranchRefId == request.BranchRefId)
                                    .Include(x => x.LoanDocuments)
                                    .FirstOrDefaultAsync();

            if (existingRequest is null)
            {
                return Result.Failure<string>(ErrorResponse.NotFound("Loan Request does not exist."));
            }

            var documents = request.LoanDocuments.Adapt<List<LoanDocuments>>();

            foreach (var item in documents)
            {

                var existingDoc = existingRequest.LoanDocuments
                                               .Where(x => x.Id == item.Id)
                                               .FirstOrDefault();

                if (existingDoc is null || item.Id == 0)
                {
                    existingRequest.LoanDocuments.Add(item);
                }
                else
                {
                    context.Entry(existingDoc).CurrentValues.SetValues(item);
                }

            }


            foreach (var item in existingRequest.LoanDocuments)
            {
                if (!(documents.Any(x => x.Id == item.Id)))
                {
                    context.Remove(item);
                }
            }

            await context.SaveChangesAsync();

            return Result.Success(new string("Documents were Added."));

        }

        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);

            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));

        }

    }
    public async Task<Result<IEnumerable<VLoanRequestsDocuments>>> GetLoanRequestDocumentAsync(long loanRequestId, int branchId)
    {
        logger.LogInformation("try to get guarantors");
        try
        {
            var db = dapperFactory.CreateConnection();

            var sql = @"SELECT * FROM VLoanRequestDocuments WHERE LoanRequestId=@LoanRequestId AND BranchId=@BranchId;";

            var guarantors = await db.QueryAsync<VLoanRequestsDocuments>(sql, new
            {
                loanRequestId,
                branchId
            });

            return Result.Success(guarantors);
        }

        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);

            return Result.Failure<IEnumerable<VLoanRequestsDocuments>>(ErrorResponse.ServerError($"{ex.Message}"));

        }

    }
    public async Task<Result<string>> DeleteLoanRequestDocumentAsync(Guid? loanRequestId, int? branchId, long? documentId)
    {
        logger.LogInformation("try to delete guarantors");
        try
        {

            var existingRequest = await context.LoanRequests
                                    .Where(x => x.LoanRequestId == loanRequestId && x.BranchRefId == branchId)
                                    .FirstOrDefaultAsync();

            if (existingRequest is not null && existingRequest.LoanRequestStatusRefId == (int)RequestStatus.Completed)
            {
                return Result.Failure<string>(ErrorResponse.Conflict("cannot remove this request due to it is in used."));
            }


            var gLink = context.LoanDocuments.Where(g => g.Id == documentId &&
                                                      g.LoanRequestId == loanRequestId &&
                                                      g.BranchId == branchId)
                                        .FirstOrDefault();


            if (gLink is null)
            {
                return Result.Failure<string>(ErrorResponse.NotFound("There is no document to delete."));
            }


            existingRequest?.LoanDocuments.Remove(gLink);

            await context.SaveChangesAsync();

            return Result.Success(new string("Guarantor is removed."));
        }

        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);

            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));

        }

    }

    /// <summary>
    /// Loan Request
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="credetAgentId"></param> 
    /// <param name="statusId"></param>
    /// <param name="branchId"></param>
    /// <param name="param"></param>
    /// <returns></returns>

    public async Task<Pager<VLoanRequests>> GetLoanRequestsAsync(string userId, int branchId, RequestStatus requestStatus, QueryParam param)
    {
        IQueryable<VLoanRequests> resultQueries = context.Database
                                                 .SqlQuery<VLoanRequests>($@"SELECT * FROM VLoanReuests WHERE BranchId={branchId}");

        if (!string.IsNullOrEmpty(param.Filter))
        {
            resultQueries = resultQueries.Where(x => EF.Functions.Like(x.BranchName!, $"%{param.Filter}%") ||
                                                     EF.Functions.Like(x.AltName!, $"%{param.Filter}%") ||
                                                     EF.Functions.Like(x.FullName!, $"%{param.Filter}%"));
        }

        switch (requestStatus)
        {
            case RequestStatus.All:
                break;
            case RequestStatus.New:
                resultQueries = resultQueries.Where(x => x.StatusId == (int)RequestStatus.New);
                break;
            case RequestStatus.Requested:
            case RequestStatus.Verified:
                resultQueries = resultQueries.Where(x => x.StatusId == (int)RequestStatus.Requested || x.StatusId == (int)RequestStatus.Verified);
                break;
            default:
                resultQueries = resultQueries.Where(x => x.StatusId == (int)requestStatus);
                break;
        }

        if (!string.IsNullOrEmpty(userId))
        {
            resultQueries = resultQueries.Where(x => x.AprovedUserId == userId || x.UserRequestId == userId);
        }

        if (param.SortDescending)
        {
            resultQueries = resultQueries.OrderByDescending(x => EF.Property<object>(x, param.SortColumn));
        }

        var counts = await resultQueries.CountAsync();

        var items = await resultQueries.Skip(param.PageIndex).Take(param.PageSize).AsNoTracking().ToListAsync();

        return new Pager<VLoanRequests>(items, counts);

    }


    public async Task<Result<VLoanRequests>> GetLoanRequestByCustomerAsync(int branchId, long customerId, long loanRequestId, bool include)
    {
        try
        {
            var db = dapperFactory.CreateConnection();
            var sql = $@"SELECT * FROM VLoanReuests WHERE BranchId=@branchId
                                                          AND CustomerId=@customerId
                                                          AND LoanRequestId=@loanRequestId";
            var resultQueries = await db.QueryFirstOrDefaultAsync<VLoanRequests>(sql, new
            {
                branchId,
                customerId,
                loanRequestId
            });

            if (resultQueries is null)
            {
                return Result.Failure<VLoanRequests>(ErrorResponse.NotFound("No Data not found."));
            }
            if (!include)
            {
                return Result.Success(resultQueries);
            }

            resultQueries.Guarantors = await GetLoanGuarantors(loanRequestId, branchId);

            resultQueries.Collaterals = await GetLoanCollateralLines(loanRequestId, branchId);

            resultQueries.Documents = await GetLoanDocuments(loanRequestId, branchId);

            resultQueries.Customer = await GetLoanCustomer(customerId);

            return Result.Success(resultQueries!);
        }
        catch (Exception ex)
        {
            return Result.Failure<VLoanRequests>(ErrorResponse.ServerError($"{ex.Message}"));
        }


    }

    /// <summary>
    /// Loan Request Actions
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>

    public async Task<Result<string>> ChangedLoanRequestAsync(LoanChangeStatusDtoCreate req)
    {

        var existingLoanRequest = await context.LoanRequests
                        .Where(x => x.Id == req.LoanRequestId && x.BranchRefId == req.BranchId)
                        .FirstOrDefaultAsync();

        if (existingLoanRequest is null)
        {
            return Result.Failure<string>(ErrorResponse.NotFound($"No data founds,"));

        }

        var lastEditSequent = existingLoanRequest!.EditSeq + 1;

        var updateResult = await context.LoanRequests.Where(x => x.Id == req.LoanRequestId && x.BranchRefId == req.BranchId)
                             .ExecuteUpdateAsync(
                                     prop => prop.SetProperty(p => p.LoanRequestStatusRefId, req.StatusId)
                                             .SetProperty(p => p.ModifiedDt, DateTime.UtcNow)
                                             .SetProperty(p => p.EditSeq, lastEditSequent));

        return updateResult > 0 ?
                            Result.Success<string>(new("Loan Request status was changed"))
                            :
                            Result.Failure<string>(ErrorResponse.ServerError("Cannot changed loan request."));


    }
    public async Task<Result<string>> AprovedLoanRequestAsync(AproveLoanDtoCreate req)
    {
        try
        {
            var existingLoanRequest = await context.LoanRequests
                       .Where(x => x.LoanRequestId == req.LoanRequestId &&
                                   x.BranchRefId == req.BranchId &&
                                   x.CustomerRefId == req.CustomerId
                       ).FirstOrDefaultAsync();

            if (existingLoanRequest is null)
            {
                return Result.Failure<string>(ErrorResponse.NotFound($"No data founds,"));
            }

            var lastEditSequent = existingLoanRequest!.EditSeq + 1;

            var updateResult = await context.LoanRequests
                                            .Where(x => x.LoanRequestId == req.LoanRequestId && x.BranchRefId == req.BranchId)
                                            .ExecuteUpdateAsync(
                                                    prop => prop.SetProperty(p => p.LoanRequestStatusRefId, (int)RequestStatus.Aprove)
                                                                .SetProperty(p => p.ModifiedDt, DateTime.UtcNow)
                                                                .SetProperty(p => p.AprovedDate, req.AprovedDate)
                                                                .SetProperty(p => p.AprovedAmount, req.AprovedAmount)
                                                                .SetProperty(p => p.AdminFeeAmount, req.AdminFeeAmount)
                                                                .SetProperty(p => p.Memo, req.Memo)
                                                                .SetProperty(p => p.EditSeq, lastEditSequent));

            return updateResult > 0 ?
                                Result.Success<string>(new("Loan Request was Aproved"))
                                :
                                Result.Failure<string>(ErrorResponse.ServerError($"cannot be aproved the loan Request."));
        }
        catch (Exception ex)
        {
            return Result.Failure<string>(ErrorResponse.NotFound($"{ex.Message}"));
        }
    }
    public async Task<Result<string>> DeniedLoanRequestAsync(DeniedRequestDtoCreate req)
    {

        try
        {
            var existingLoanRequest = await context.LoanRequests
                        .Where(x => x.LoanRequestId == req.LoanRequestId && x.BranchRefId == req.BranchId)
                        .FirstOrDefaultAsync();

            if (existingLoanRequest is null)
            {
                return Result.Failure<string>(ErrorResponse.NotFound($"no data founds,"));
            }

            var lastEditSequent = existingLoanRequest!.EditSeq + 1;

            var updateResult = await context.LoanRequests
                                            .Where(x => x.LoanRequestId == req.LoanRequestId && x.BranchRefId == req.BranchId)
                                            .ExecuteUpdateAsync(
                                                    prop => prop.SetProperty(p => p.LoanRequestStatusRefId, (int)RequestStatus.Denied)
                                                                .SetProperty(p => p.ModifiedDt, DateTime.UtcNow)
                                                                .SetProperty(p => p.AprovedDate, req.DeniedDate)
                                                                .SetProperty(p => p.Memo, req.Memo)
                                                                .SetProperty(p => p.EditSeq, lastEditSequent));

            return updateResult > 0 ?
                                Result.Success<string>(new("Loan Request was Aproved"))
                                :
                                Result.Failure<string>(ErrorResponse.ServerError($"cannot be aproved the loan Request."));

        }
        catch (Exception ex)
        {
            return Result.Failure<string>(ErrorResponse.NotFound($"{ex.Message}"));
        }



    }
    public async Task<Result<string>> RecheckedLoanRequestAsync(RecheckedRequestDtoCreate req)
    {

        var existingLoanRequest = await context.LoanRequests
                        .Where(x => x.LoanRequestId == req.LoanRequestId &&
                               x.BranchRefId == req.BranchId &&
                               x.CustomerRefId == req.CustomerId
                        ).FirstOrDefaultAsync();

        if (existingLoanRequest is null)
        {
            return Result.Failure<string>(ErrorResponse.NotFound($"No data founds"));

        }

        var lastEditSequent = existingLoanRequest!.EditSeq + 1;

        var updateResult = await context.LoanRequests
                                        .Where(x => x.LoanRequestId == req.LoanRequestId && x.BranchRefId == req.BranchId)
                                        .ExecuteUpdateAsync(
                                                prop => prop.SetProperty(p => p.LoanRequestStatusRefId, (int)RequestStatus.Rechecked)
                                                            .SetProperty(p => p.ModifiedDt, DateTime.UtcNow)
                                                            .SetProperty(p => p.AprovedDate, req.CheckedDate)
                                                            .SetProperty(p => p.Memo, req.Memo)
                                                            .SetProperty(p => p.EditSeq, lastEditSequent));

        return updateResult > 0 ?
                            Result.Success<string>(new("Loan Request was Aproved"))
                            :
                            Result.Failure<string>(ErrorResponse.ServerError($"cannot be aproved the loan Request."));


    }
    public async Task<Result<string>> VerifiedLoanRequestAsync(RequestDtoBaseCreate req)
    {

        var existingLoanRequest = await context.LoanRequests
                        .Where(x => x.LoanRequestId == req.LoanRequestId && x.BranchRefId == req.BranchId && x.CustomerRefId == req.CustomerId).FirstOrDefaultAsync();

        if (existingLoanRequest is null)
        {
            return Result.Failure<string>(ErrorResponse.NotFound($"no data founds,"));

        }

        var lastEditSequent = existingLoanRequest!.EditSeq + 1;

        var updateResult = await context.LoanRequests
                                        .Where(x => x.LoanRequestId == req.LoanRequestId && x.BranchRefId == req.BranchId)
                                        .ExecuteUpdateAsync(
                                                prop => prop.SetProperty(p => p.LoanRequestStatusRefId, (int)RequestStatus.Verified)
                                                            .SetProperty(p => p.ModifiedDt, DateTime.UtcNow)
                                                            .SetProperty(p => p.Memo, req.Memo)
                                                            .SetProperty(p => p.EditSeq, lastEditSequent));

        return updateResult > 0 ?
                            Result.Success<string>(new("Loan Request was Aproved"))
                            :
                            Result.Failure<string>(ErrorResponse.ServerError($"cannot be aproved the loan Request."));


    }
    public async Task<Result<string>> AppliedLoanRequestAsync(RequestDtoBaseCreate req)
    {

        var existingLoanRequest = await context.LoanRequests
                        .Where(x => x.LoanRequestId == req.LoanRequestId &&
                               x.BranchRefId == req.BranchId &&
                               x.CustomerRefId == req.CustomerId
                        ).FirstOrDefaultAsync();

        if (existingLoanRequest is null)
        {
            return Result.Failure<string>(ErrorResponse.NotFound($"no data founds,"));

        }

        var lastEditSequent = existingLoanRequest!.EditSeq + 1;

        var updateResult = await context.LoanRequests
                                        .Where(x => x.LoanRequestId == req.LoanRequestId && x.BranchRefId == req.BranchId)
                                        .ExecuteUpdateAsync(
                                                prop => prop.SetProperty(p => p.LoanRequestStatusRefId, (int)RequestStatus.Requested)
                                                            .SetProperty(p => p.ModifiedDt, DateTime.UtcNow)
                                                            .SetProperty(p => p.Memo, req.Memo)
                                                            .SetProperty(p => p.EditSeq, lastEditSequent));

        return updateResult > 0 ?
                            Result.Success<string>(new("Loan Request was Aproved"))
                            :
                            Result.Failure<string>(ErrorResponse.ServerError($"cannot be aproved the loan Request."));


    }



    /// <summary>
    /// Private methods
    /// </summary>
    /// <param name="customerId"></param>
    /// <returns></returns>
    private async Task<VCustomerList> GetLoanCustomer(long customerId)
    {
        var customer = await context.CustomerLists
                                .Where(c => c.Id == customerId)
                                .AsNoTracking()
                                .FirstOrDefaultAsync();

        if (customer is not null)
        {
            var incomes = await context.Database
                           .SqlQuery<VCustomerIncomes>($"SELECT * FROM VCustomerIncomes WHERE CustomerRefId={customerId}")
                           .AsNoTracking()
                           .ToListAsync();

            var expenses = await context.Database
                                .SqlQuery<VCustomerExpenses>($"SELECT * FROM VCustomerExpenses WHERE CustomerRefId={customerId}")
                                .AsNoTracking()
                                .ToListAsync();

            customer.CustomerExpenses = expenses ?? [];

            customer.CustomerIncomes = incomes ?? [];

        }

        return customer ?? new();
    }
    private async Task<IEnumerable<VLoanRequestGuarantors>> GetLoanGuarantors(long? loanRequestId, int? branchId)
    {
        var db = dapperFactory.CreateConnection();
        var sql = $@"SELECT * FROM VLoanRequestGuarators WHERE LoanRequestId=@loanRequestId AND BranchId=@branchId;";
        var loanGuarantors = await db.QueryAsync<VLoanRequestGuarantors>(sql, new
        {
            loanRequestId,
            branchId
        });
        return loanGuarantors ?? [];
    }
    private async Task<IEnumerable<VLoanRequestCollateralLines>> GetLoanCollateralLines(long? loanRequestId, int? branchId)
    {

        var db = dapperFactory.CreateConnection();
        var sql = "SELECT * FROM VLoanRequestCollaterals WHERE LoanRequestId=@loanRequestId AND BranchId=@branchId";
        var loanCollateralLines = await db.QueryAsync<VLoanRequestCollateralLines>(sql, new
        {
            loanRequestId,
            branchId
        });
        return loanCollateralLines ?? [];
    }
    private async Task<IEnumerable<VLoanRequestsDocuments>> GetLoanDocuments(long? loanRequestId, int branchId)
    {
        var db = dapperFactory.CreateConnection();

        var sql = $"SELECT * FROM [dbo].[VLoanRequestDocuments] WHERE LoanRequestId=@loanRequestId AND branchId=@branchId";

        var loanDocuments = await db.QueryAsync<VLoanRequestsDocuments>(sql, new
        {
            loanRequestId,
            branchId
        });

        db.Close();

        return loanDocuments ?? [];
    }

    public Task<Result<VLoanRequests>> GetLoanRequestByCustomerAsync(int? branchId, Guid? customerId, Guid? loanRequestId, bool include)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<VLoanRequestGuarantors>>> GetLoanRequestGuarantorAsync(int? branchId, Guid? loanRequestId, Guid? customerId)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<VLoanRequestsDocuments>>> GetLoanRequestDocumentAsync(Guid? loanRequestId, int? branchId)
    {
        throw new NotImplementedException();
    }

    public Task<Pager<VLoanRequests>> GetLoanRequestsAsync(Guid? userId, int? branchId, RequestStatus requestStatus, QueryParam param)
    {
        throw new NotImplementedException();
    }
}
