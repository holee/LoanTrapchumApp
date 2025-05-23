using Applications.Enumerations;
using Applications.Extensions;
using Applications.Interfaces.Loans;
using Applications.Requests.Loans.CreditAgents;
using Applications.Requests.Loans.LoanRequests;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Customers;
using Applications.Responses.ViewModels.Loans.Requests;
using Dapper;
using Domains.Models.Collaterals;
using Domains.Models.Lists;
using Domains.Models.Lists.Customers;
using Domains.Models.Loans;
using Infrastructures.DataAccess;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace Infrastructures.Implements;

public sealed class LoanRequestService(
    ApplicationDbContext context,
    ILogger<LoanRequestService> logger,
    IDapperConnectionFactory dapperFactory,
    IDbContextFactory<DataContext> contextFactory) : ILoanRequestService
{
    public async Task<Result<string>> CreateLoanRequestAsync(LoanRequest request)
    {
        logger.LogInformation("try to create new loan request");
        try
        {
            await using var cxt = await contextFactory.CreateDbContextAsync();

            if (await CheckExistingLoanRequest(request.CustomerRefId))
            {
                return Result.Failure<string>(ErrorResponse.Conflict("Loan Request already exists."));
            }

            if (await CheckExistingHasLoan(request.CustomerRefId))
            {
                return Result.Failure<string>(ErrorResponse.Conflict("Customer has active loan."));
            }

            var existingRequest = await cxt.LoanRequests
                .Where(x => x.LoanRequestId == request.LoanRequestId && x.CustomerRefId == request.CustomerRefId)
                .Include(c => c.CollateralLinesLink)
                .ThenInclude(x => x.CollateralLine)
                .Include(g => g.GuarantorsLink)
                .ThenInclude(x => x.Guarantor)
                .Include(x => x.LoanDocuments)
                .Include(x => x.LoanDescriptions)
                .FirstOrDefaultAsync();

            if (existingRequest is null)
            {
                cxt.LoanRequests.Add(request);
            }
            else
            {
                cxt.Entry(existingRequest).CurrentValues.SetValues(request);

                foreach (var item in request.GuarantorsLink)
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


                foreach (var item in request.CollateralLinesLink)
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
                    if (!(request.CollateralLinesLink.Any(x => x.Id == item.Id)))
                    {
                        cxt.Remove(item);
                    }
                }

                foreach (var item in existingRequest.GuarantorsLink)
                {
                    if (!(request.GuarantorsLink.Any(x => x.Id == item.Id)))
                    {
                        cxt.Remove(item);
                    }
                }
            }

            await cxt.SaveChangesAsync();

            return Result.Success(new string("New Loan Request was created."));
        }

        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);

            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));
        }
    }


    public async Task<Result<bool>> CreateLoanRequestAsync(int? branchId, string? userId, string? loanRequestId)
    {
        logger.LogInformation("try to create new loan request");
        try
        {
            var existingRequest = await context.LoanRequests
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.LoanRequestId == loanRequestId &&
                                          x.BranchRefId == branchId &&
                                          x.RequestUserRefId == userId &&
                                          x.LoanRequestStatusRefId == (int)RequestStatus.New);

            if (existingRequest is null)
            {
                return Result.Failure<bool>(ErrorResponse.Conflict("Loan Request does not exist."));
            }

            var updated = await context.LoanRequests
                .Where(x => x.LoanRequestId == loanRequestId &&
                            x.BranchRefId == branchId &&
                            x.RequestUserRefId == userId &&
                            x.LoanRequestStatusRefId == (int)RequestStatus.New)
                .ExecuteUpdateAsync(props => props
                    .SetProperty(p => p.LoanRequestStatusRefId, (int)RequestStatus.Requested)
                    .SetProperty(p => p.ModifiedDt, DateTime.Now));
            if (updated > 0)
            {
                return Result.Success(true);
            }

            return Result.Failure<bool>(ErrorResponse.ServerError("Loan Request was not created."));
        }

        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);

            return Result.Failure<bool>(ErrorResponse.ServerError($"{ex.Message}"));
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

    public async Task<Result<string>> AddLoanRequestCollateralAsync(LoanCollateralRequest req)
    {
        logger.LogInformation("try to create new loan request");
        try
        {
            var existingRequest = await context.LoanRequests
                .Where(x => x.LoanRequestId == req.LoanRequestId && x.CustomerRefId == req.CustomerId &&
                            x.BranchRefId == req.BranchId)
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

    public async Task<Result<IEnumerable<VLoanRequestCollateralLines>>> GetLoanRequestCollateralAsync(int? branchId,
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

    public async Task<Result<string>> DeleteLoanRequestCollateralAsync(int? branchId, string? loanRequestId,
        long? collateralLinkId)
    {
        logger.LogInformation("try to delete guarantors");
        try
        {
            var existingRequest = await context.LoanRequests
                .FirstOrDefaultAsync(x => x.LoanRequestId == loanRequestId && x.BranchRefId == branchId);

            if (existingRequest is not null && existingRequest.LoanRequestStatusRefId == (int)RequestStatus.Completed)
            {
                return Result.Failure<string>(
                    ErrorResponse.NotFound("cannot remove this request due to it is in used."));
            }

            var gLink = await context.CollateralsLinks
                .FirstOrDefaultAsync(g => g.Id == collateralLinkId &&
                                     g.LoanRequestRefId == loanRequestId &&
                                     g.BranchId == branchId);


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

    public async Task<Result<string>> AddLoanRequestGuarantorAsync(LoanRequestGuarantors request)
    {
        logger.LogInformation("try to create new loan request");
        try
        {
            var existingRequest = await context.LoanRequests
                .Where(x => x.LoanRequestId == request.LoanRequestId &&
                            x.CustomerRefId == request.CustomerRefId &&
                            x.BranchRefId == request.BranchRefId)
                .Include(g => g.GuarantorsLink)
                .ThenInclude(x => x.Guarantor)
                .FirstOrDefaultAsync();

            if (existingRequest is null)
            {
                return Result.Failure<string>(ErrorResponse.NotFound("Loan Request does not exist."));
            }


            var collateralLinks = request.GuarantorsLink.Adapt<IEnumerable<LoanGuarantorLink>>();


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

    public async Task<Result<IEnumerable<VLoanRequestGuarantors>>> GetLoanRequestGuarantorAsync(int? branchId,
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

            return Result.Success(guarantors);
        }

        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);

            return Result.Failure<IEnumerable<VLoanRequestGuarantors>>(ErrorResponse.ServerError($"{ex.Message}"));
        }
    }

    public async Task<Result<string>> DeleteLoanRequestGuarantorAsync(string? loanRequestId, int? branchId,
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
                return Result.Failure<string>(
                    ErrorResponse.Conflict("cannot remove this request due to it is in used."));
            }


            var gLink = context.Guarantors.FirstOrDefault(g => g.Id == guarantorLinkId &&
                                                               g.LoanRequestRefId == loanRequestId &&
                                                               g.BranchId == branchId);
            if (gLink is null)
            {
                return Result.Failure<string>(ErrorResponse.NotFound("There is no any guarantor link."));
            }

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


    public async Task<Result<string>> AddLoanRequestDocumentAsync(LoanDocuments request)
    {
        logger.LogInformation("try to create new loan request");
        try
        {
            var existingRequest = await context.LoanRequests
                .Where(x => x.LoanRequestId == request.LoanRequestId &&
                            x.BranchRefId == request.BranchId)
                .Include(x => x.LoanDocuments)
                .FirstOrDefaultAsync();

            if (existingRequest is null)
            {
                return Result.Failure<string>(ErrorResponse.NotFound("Loan Request does not exist."));
            }

            var documents = request.Adapt<List<LoanDocuments>>();

            foreach (var item in documents)
            {
                var existingDoc = existingRequest.LoanDocuments.FirstOrDefault(x => x.Id == item.Id);

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
                    //context.Remove(item);
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

    public async Task<Result<string>> AddLoanRequestDocumentAsync(List<LoanDocuments> documents)
    {
        logger.LogInformation("try to create new loan request");
        try
        {
            context.LoanDocuments.UpdateRange(documents);

            await context.SaveChangesAsync();

            return Result.Success(new string("Documents were Added."));
        }

        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);

            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));
        }
    }

    public async Task<Result<string>> DeleteLoanRequestDocumentAsync(string? loanRequestId, int? branchId,
        long? documentId)
    {
        logger.LogInformation("try to delete guarantors");
        try
        {
            var existingRequest = await context.LoanRequests
                .Where(x => x.LoanRequestId == loanRequestId && x.BranchRefId == branchId)
                .FirstOrDefaultAsync();

            if (existingRequest is not null && existingRequest.LoanRequestStatusRefId == (int)RequestStatus.Completed)
            {
                return Result.Failure<string>(
                    ErrorResponse.Conflict("cannot remove this request due to it is in used."));
            }


            var gLink = context.LoanDocuments.FirstOrDefault(g => g.Id == documentId &&
                                                                  g.LoanRequestId == loanRequestId &&
                                                                  g.BranchId == branchId);
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


    public async Task<Result<VLoanRequests>> GetLoanRequestAsync(int? branchId, string? customerId,string? loanRequestId, bool include)
    {
        try
        {
            using var db = await dapperFactory.CreateConnectionAsync();

            var sql = @"SELECT * FROM VLoanRequests WHERE BranchId=@branchId AND 
                                                         CustomerId=@customerId AND  
                                                         LoanRequestId=@loanRequestId";

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

            // Fetch related data concurrently
            var guarantorsTask = GetLoanGuarantors(loanRequestId, branchId);

            var collateralsTask = GetLoanCollateralLines(loanRequestId, branchId);

            var documentsTask = GetLoanDocuments(loanRequestId, branchId);

            var customerTask = GetLoanCustomer(customerId);

            await Task.WhenAll(guarantorsTask, collateralsTask, documentsTask, customerTask);

            resultQueries.Guarantors = guarantorsTask.Result.ToList();

            resultQueries.Collaterals = collateralsTask.Result.ToList();

            resultQueries.Documents = documentsTask.Result.ToList();

            if (customerTask.Result is { IsSuccess: true, Data: not null })
            {
                resultQueries.Customer = customerTask.Result.Data;
            }

            return Result.Success(resultQueries);

        }
        catch (Exception ex)
        {
            return Result.Failure<VLoanRequests>(ErrorResponse.ServerError($"{ex.Message}"));
        }
    }


    public async Task<Result<VLoanRequests>> GetLoanRequestAsync(int? branchId, string? userId, string? customerId,
        string? loanRequestId, bool include)
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
                return Result.Failure<VLoanRequests>(ErrorResponse.NotFound("No Data not found."));
            }

            if (!include)
            {
                return Result.Success(resultQueries);
            }

            resultQueries.Guarantors = await GetLoanGuarantors(loanRequestId, branchId);

            resultQueries.Collaterals = await GetLoanCollateralLines(loanRequestId, branchId);

            resultQueries.Documents = await GetLoanDocuments(loanRequestId, branchId);

            var rsp = await GetLoanCustomer(customerId);

            if (rsp is { IsSuccess: true, Data: not null })
            {
                resultQueries.Customer = rsp.Data;
            }

            return Result.Success(resultQueries);
        }
        catch (Exception ex)
        {
            return Result.Failure<VLoanRequests>(ErrorResponse.ServerError($"{ex.Message}"));
        }
    }


    public async Task<Result<T>> GetLoanRequestAsync<T>(int? branchId, string? customerId, string? loanRequestId)
        where T : class
    {
        try
        {
            var item = await context
                .Database
                .SqlQuery<VLoanRequests>(@$"SELECT * FROM VLoanRequests 
                                            WHERE BranchId={branchId} AND 
                                            CustomerId={customerId} AND  
                                            LoanRequestId={loanRequestId}")
                .ProjectToType<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (item is null)
            {
                return Result.Failure<T>(ErrorResponse.NotFound("No Data not found."));
            }

            return Result.Success(item);
        }
        catch (Exception ex)
        {
            return Result.Failure<T>(ErrorResponse.ServerError($"{ex.Message}"));
        }
    }


    public async Task<Result<string>> ChangedLoanRequestAsync(LoanChangeStatusDtoCreate req)
    {
        var existingLoanRequest = await context.LoanRequests
            .Where(x => x.LoanRequestId == req.LoanRequestId && x.BranchRefId == req.BranchId)
            .FirstOrDefaultAsync();

        if (existingLoanRequest is null)
        {
            return Result.Failure<string>(ErrorResponse.NotFound($"No data founds,"));
        }

        var lastEditSequent = existingLoanRequest.EditSeq + 1;

        var updateResult = await context.LoanRequests
            .Where(x => x.LoanRequestId == req.LoanRequestId && x.BranchRefId == req.BranchId)
            .ExecuteUpdateAsync(prop => prop.SetProperty(p => p.LoanRequestStatusRefId, req.StatusId)
                .SetProperty(p => p.ModifiedDt, DateTime.UtcNow)
                .SetProperty(p => p.EditSeq, lastEditSequent));

        return updateResult > 0
            ? Result.Success<string>(new("Loan Request status was changed"))
            : Result.Failure<string>(ErrorResponse.ServerError("Cannot changed loan request."));
    }

    public async Task<Result<string>> AprovedLoanRequestAsync(AproveLoanDtoCreate req)
    {
        try
        {
            var existingLoanRequest = await context.LoanRequests
                .FirstOrDefaultAsync(x => x.LoanRequestId == req.LoanRequestId &&
                            x.BranchRefId == req.BranchId &&
                            x.CustomerRefId == req.CustomerId
                );

            if (existingLoanRequest is null)
            {
                return Result.Failure<string>(ErrorResponse.NotFound($"No data founds,"));
            }

            var lastEditSequent = existingLoanRequest.EditSeq + 1;

            var updateResult = await context.LoanRequests
                .Where(x => x.LoanRequestId == req.LoanRequestId && x.BranchRefId == req.BranchId)
                .ExecuteUpdateAsync(prop => prop.SetProperty(p => p.LoanRequestStatusRefId, (int)RequestStatus.Aprove)
                    .SetProperty(p => p.ModifiedDt, DateTime.UtcNow)
                    .SetProperty(p => p.AprovedDate, req.AprovedDate)
                    .SetProperty(p => p.AprovedAmount, req.AprovedAmount)
                    .SetProperty(p => p.AdminFeeAmount, req.AdminFeeAmount)
                    .SetProperty(p => p.Memo, req.Memo)
                    .SetProperty(p => p.EditSeq, lastEditSequent));

            return updateResult > 0
                ? Result.Success<string>(new("Loan Request was Aproved"))
                : Result.Failure<string>(ErrorResponse.ServerError($"cannot be aproved the loan Request."));
        }
        catch (Exception ex)
        {
            return Result.Failure<string>(ErrorResponse.NotFound($"{ex.Message}"));
        }
    }

    public async Task<Result<string>> DeniedLoanRequestAsync(DeniedRequestDtoCreate req)
    {
        await using var cxt = await contextFactory.CreateDbContextAsync();

        try
        {
            var existingLoanRequest = await cxt.LoanRequests
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.LoanRequestId == req.LoanRequestId && x.BranchRefId == req.BranchId);

            if (existingLoanRequest is null)
            {
                return Result.Failure<string>(ErrorResponse.NotFound($"no data founds,"));
            }

            var lastEditSequent = existingLoanRequest.EditSeq + 1;

            var updateResult = await cxt.LoanRequests
                .Where(x => x.LoanRequestId == req.LoanRequestId && x.BranchRefId == req.BranchId)
                .ExecuteUpdateAsync(prop => prop.SetProperty(p => p.LoanRequestStatusRefId, (int)RequestStatus.Denied)
                    .SetProperty(p => p.ModifiedDt, DateTime.UtcNow)
                    .SetProperty(p => p.AprovedDate, req.DeniedDate)
                    .SetProperty(p => p.Memo, req.Memo)
                    .SetProperty(p => p.EditSeq, lastEditSequent));

            return updateResult > 0
                ? Result.Success<string>(new("Loan Request was Aproved"))
                : Result.Failure<string>(ErrorResponse.ServerError($"cannot be aproved the loan Request."));
        }
        catch (Exception ex)
        {
            return Result.Failure<string>(ErrorResponse.NotFound($"{ex.Message}"));
        }
    }

    public async Task<Result<string>> RecheckedLoanRequestAsync(RecheckedRequestDtoCreate req)
    {
        await using var cxt = await contextFactory.CreateDbContextAsync();

        try
        {
            var existingLoanRequest = await cxt.LoanRequests
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.LoanRequestId == req.LoanRequestId &&
                                          x.BranchRefId == req.BranchId &&
                                          x.CustomerRefId == req.CustomerId);

            if (existingLoanRequest is null)
            {
                return Result.Failure<string>(ErrorResponse.NotFound($"No data founds"));
            }

            var lastEditSequent = existingLoanRequest.EditSeq + 1;

            var updateResult = await cxt.LoanRequests
                .Where(x => x.LoanRequestId == req.LoanRequestId &&
                            x.BranchRefId == req.BranchId &&
                            x.EditSeq == existingLoanRequest.EditSeq &&
                            x.CustomerRefId == req.CustomerId)
                .ExecuteUpdateAsync(prop => prop
                    .SetProperty(p => p.LoanRequestStatusRefId, (int)RequestStatus.Rechecked)
                    .SetProperty(p => p.ModifiedDt, DateTime.UtcNow)
                    .SetProperty(p => p.AprovedDate, req.CheckedDate)
                    .SetProperty(p => p.Memo, req.Memo)
                    .SetProperty(p => p.EditSeq, lastEditSequent));

            return updateResult > 0
                ? Result.Success<string>(new("Loan Request was Aproved"))
                : Result.Failure<string>(ErrorResponse.ServerError($"cannot be aproved the loan Request."));
        }
        catch (Exception ex)
        {
            return Result.Failure<string>(ErrorResponse.ServerError($"Error due to : {ex.Message}"));
        }
    }

    public async Task<Result<string>> VerifiedLoanRequestAsync(RequestDtoBaseCreate req)
    {
        await using var cxt = await contextFactory.CreateDbContextAsync();

        var existingLoanRequest = await cxt.LoanRequests
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.LoanRequestId == req.LoanRequestId &&
                                      x.BranchRefId == req.BranchId &&
                                      x.CustomerRefId == req.CustomerId);

        if (existingLoanRequest is null)
        {
            return Result.Failure<string>(ErrorResponse.NotFound($"no data founds,"));
        }

        var lastEditSequent = existingLoanRequest.EditSeq + 1;

        var updateResult = await cxt.LoanRequests
            .Where(x => x.LoanRequestId == req.LoanRequestId &&
                        x.BranchRefId == req.BranchId &&
                        x.CustomerRefId == req.CustomerId &&
                        x.EditSeq == existingLoanRequest.EditSeq)
            .ExecuteUpdateAsync(prop => prop.SetProperty(p => p.LoanRequestStatusRefId, (int)RequestStatus.Verified)
                .SetProperty(p => p.ModifiedDt, DateTime.UtcNow)
                .SetProperty(p => p.Memo, req.Memo)
                .SetProperty(p => p.EditSeq, lastEditSequent));

        return updateResult > 0
            ? Result.Success<string>(new("Loan Request was Aproved"))
            : Result.Failure<string>(ErrorResponse.ServerError($"cannot be aproved the loan Request."));
    }

    public async Task<Result<string>> AppliedLoanRequestAsync(RequestDtoBaseCreate req)
    {
        await using var cxt = await contextFactory.CreateDbContextAsync();

        var existingLoanRequest = await cxt.LoanRequests
            .FirstOrDefaultAsync(x => x.LoanRequestId == req.LoanRequestId &&
                                      x.BranchRefId == req.BranchId &&
                                      x.CustomerRefId == req.CustomerId);

        if (existingLoanRequest is null)
        {
            return Result.Failure<string>(ErrorResponse.NotFound($"no data founds,"));
        }

        var lastEditSequent = existingLoanRequest.EditSeq + 1;

        var updateResult = await cxt.LoanRequests
            .Where(x => x.LoanRequestId == req.LoanRequestId && x.BranchRefId == req.BranchId)
            .ExecuteUpdateAsync(prop => prop.SetProperty(p => p.LoanRequestStatusRefId, (int)RequestStatus.Requested)
                .SetProperty(p => p.ModifiedDt, DateTime.UtcNow)
                .SetProperty(p => p.Memo, req.Memo)
                .SetProperty(p => p.EditSeq, lastEditSequent));

        return updateResult > 0
            ? Result.Success<string>(new("Loan Request was Aproved"))
            : Result.Failure<string>(ErrorResponse.ServerError($"cannot be aproved the loan Request."));
    }


    public async Task<Result<IEnumerable<VLoanRequestsDocuments>>> GetLoanRequestDocumentAsync(string? loanRequestId,
    int? branchId)
    {
        using var db = await dapperFactory.CreateConnectionAsync();

        var sql =
            $"SELECT * FROM [dbo].[VLoanRequestDocuments] WHERE LoanRequestId=@loanRequestId AND branchRefId=@branchId";

        var loanDocuments = await db.QueryAsync<VLoanRequestsDocuments>(sql, new
        {
            loanRequestId,
            branchId
        });

        return Result.Success(loanDocuments);
    }

    public async Task<Pager<VLoanRequests>> GetLoanRequestsAsync(string? userId,bool canDisburse,int? branchId,RequestStatus requestStatus,QueryParam param)// e.g., "Account", "BranchManager", etc.
    {
        await using var cxt = await contextFactory.CreateDbContextAsync();

        if (branchId is null) return new Pager<VLoanRequests>([], 0); // Handle edge case

        var baseQuery = cxt.Database
                            .SqlQuery<VLoanRequests>($"SELECT r.* FROM VLoanRequests r WHERE r.BranchId={branchId}");

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

                RequestStatus.Requested or RequestStatus.Verified =>
                    baseQuery.Where(x => (x.StatusId == (int)RequestStatus.Requested ||
                                          x.StatusId == (int)RequestStatus.Verified)
                                         && x.AprovedUserId == userId),

                RequestStatus.Aprove =>
                    baseQuery.Where(x => x.StatusId == (int)RequestStatus.Aprove),

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
                : baseQuery.OrderBy(x => EF.Property<object>(x, param.SortColumn));
        }

        // Paging
        var totalCount = await baseQuery.CountAsync();

        var skip = (param.PageNumber - 1) * param.PageSize;

        var items = await baseQuery.Skip(param.PageNumber).Take(param.PageSize).AsNoTracking().ToListAsync();

        return new Pager<VLoanRequests>(items, totalCount);
    }



    public async Task<Pager<VLoanRequests>> GetLoanRequestsAsync(string? userId, int? branchId,RequestStatus requestStatus, QueryParam param)
    {
        await using var cxt = await contextFactory.CreateDbContextAsync();

        IQueryable<VLoanRequests> resultQueries = cxt.Database
            .SqlQuery<VLoanRequests>($"SELECT r.* FROM VLoanRequests r WHERE r.BranchId={branchId}")
            .WhereIf(!string.IsNullOrEmpty(param.Filter), x => EF.Functions.Like(x.BranchName!, $"%{param.Filter}%") ||
                                                               EF.Functions.Like(x.AltName!, $"%{param.Filter}%") ||
                                                               EF.Functions.Like(x.FullName!, $"%{param.Filter}%"));
        switch (requestStatus)
        {
            case RequestStatus.All:
                break;
            case RequestStatus.New:
            case RequestStatus.Rechecked:
            case RequestStatus.Denied:
                resultQueries = resultQueries.Where(x => (x.StatusId == (int)RequestStatus.New ||
                                                          x.StatusId == (int)RequestStatus.Rechecked ||
                                                          x.StatusId == (int)RequestStatus.Denied) &&
                                                         x.UserRequestId == userId);
                break;
            case RequestStatus.Requested:
            case RequestStatus.Verified:
                resultQueries = resultQueries.Where(x =>
                    (x.StatusId == (int)RequestStatus.Requested ||
                     x.StatusId == (int)RequestStatus.Verified) &&
                    x.AprovedUserId == userId);
                break;
            case RequestStatus.Aprove:
                resultQueries = resultQueries.Where(x => x.StatusId == (int)RequestStatus.Aprove);
                break;
            default:
                resultQueries = resultQueries.Where(x => x.StatusId == (int)requestStatus);
                break;
        }


        if (param.SortDescending)
        {
            resultQueries = resultQueries.OrderByDescending(x => EF.Property<object>(x, param.SortColumn));
        }

        var counts = resultQueries.Count();

        var items = await resultQueries.Skip(param.PageNumber).Take(param.PageSize).AsNoTracking().ToListAsync();

        return new Pager<VLoanRequests>(items, counts);
    }




    private async Task<Result<VCustomerList>> GetLoanCustomer(string? customerId)
    {
        if (string.IsNullOrWhiteSpace(customerId))
            return Result.Failure<VCustomerList>(ErrorResponse.NotFound("Customer ID is required."));

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

        var lookup = new Dictionary<string, VCustomerList>();

        await db.QueryAsync<VCustomerList, VCustomerIncomes, VCustomerExpenses, VCustomerList>(
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
            return Result.Failure<VCustomerList>(ErrorResponse.NotFound("Customer data not found."));

        return Result.Success(finalCustomer);
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

        var sql =
            $@"SELECT LoanRequestId,RequestAmount,CustomerId FROM VLoanRequests WHERE CustomerId=@CustomerId AND StatusId IN (1,2,3,6)";

        var existingLoanRequest = await db.QueryAsync<VLoanRequests>(sql, parameters);

        return existingLoanRequest.Any();
    }


    private async Task<IEnumerable<VLoanRequestGuarantors>> GetLoanGuarantors(string? loanRequestId, int? branchId)
    {
        if (!branchId.HasValue || string.IsNullOrEmpty(loanRequestId))
        {
            return Enumerable.Empty<VLoanRequestGuarantors>();
        }

        using var db = await dapperFactory.CreateConnectionAsync();

        const string sql = @"
        SELECT * 
        FROM VLoanRequestGuarators 
        WHERE LoanRequestRefId = @LoanRequestId 
          AND BranchRefId = @BranchId;";

        return await db.QueryAsync<VLoanRequestGuarantors>(
            sql,
            new { LoanRequestId = loanRequestId, BranchId = branchId }
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
        WHERE LoanRequestId = @LoanRequestId 
          AND BranchRefId = @BranchId;";

        return await db.QueryAsync<VLoanRequestsDocuments>(
            sql,
            new { LoanRequestId = loanRequestId, BranchId = branchId }
        );
    }

}
//resultQueries.Guarantors = await GetLoanGuarantors(loanRequestId, branchId);

//resultQueries.Collaterals = await GetLoanCollateralLines(loanRequestId, branchId);

//resultQueries.Documents = await GetLoanDocuments(loanRequestId, branchId);

//var rsp = await GetLoanCustomer(customerId);

//if (rsp is { IsSuccess: true, Data: not null })
//{
//    resultQueries.Customer = rsp.Data;
//}

//return Result.Success(resultQueries);

//private async Task<IEnumerable<VLoanRequestGuarantors>> GetLoanGuarantors(string? loanRequestId, int? branchId)
//{
//    using var db = await dapperFactory.CreateConnectionAsync();
//    var loanGuarantors = await db.QueryAsync<VLoanRequestGuarantors>(@"SELECT * FROM VLoanRequestGuarators 
//                                                                        WHERE LoanRequestRefId=@LoanRequestId AND 
//                                                                        BranchRefId=@BranchId", new
//    {
//        branchId,
//        loanRequestId
//    });
//    return loanGuarantors;
//}

//private async Task<IEnumerable<VLoanRequestCollateralLines>> GetLoanCollateralLines(string? loanRequestId,
//    int? branchId)
//{
//    await using var cxt = await contextFactory.CreateDbContextAsync();

//    var loanCollateralLines = await cxt.Database
//        .SqlQuery<VLoanRequestCollateralLines>($@"SELECT * FROM VLoanRequestCollaterals 
//                                                   WHERE LoanRequestId={loanRequestId} AND 
//                                                   BranchId={branchId}
//                                                ")
//        .AsNoTracking()
//        .ToListAsync();
//    return loanCollateralLines;
//}

//private async Task<IEnumerable<VLoanRequestsDocuments>> GetLoanDocuments(string? loanRequestId, int? branchId)
//{
//    await using var cxt = await contextFactory.CreateDbContextAsync();

//    var loanDocuments = await cxt.Database
//        .SqlQuery<VLoanRequestsDocuments>($@"SELECT * FROM VLoanRequestDocuments 
//                                             WHERE LoanRequestId={loanRequestId} AND 
//                                             branchRefId={branchId}
//        ").AsNoTracking()
//        .ToListAsync();
//    return loanDocuments;
//}


//private async Task<Result<VCustomerList>> GetLoanCustomer(string? customerId)
//{

//    using var db = await dapperFactory.CreateConnectionAsync();

//    var sql = @"
//           SELECT c.*,
//                   c1.IncomeId,
//                   COALESCE (c1.IncomeAmount, 0) AS IncomeAmount,
//                   c1.IncomeSource,
//                   c1.InDescription,
//               c2.ExpendId,
//               COALESCE (c2.ExpendAmount, 0) AS ExpendAmount,
//               c2.ExpenseTitle,
//               c2.ExDescription
//            FROM   VCustomerList AS c LEFT OUTER JOIN
//                   VCustomerIncomes AS c1 ON c1.CustomerRefId = c.CustomerId LEFT OUTER JOIN
//                   VCustomerExpenses AS c2 ON c2.CustomerRefId = c.CustomerId
//            WHERE  c.CustomerId = @customerId;";

//    var paramerters = new DynamicParameters();

//    paramerters.Add("@CustomerId", customerId);

//    var lookup = new Dictionary<string, VCustomerList>();

//    var customerDict = await db.QueryAsync<VCustomerList, VCustomerIncomes, VCustomerExpenses, VCustomerList>(sql,
//        (customer, income, expense) =>
//        {
//            if (!lookup.TryGetValue(customer.CustomerId!, out var existingCustomer))
//            {
//                customer.CustomerIncomes = new List<VCustomerIncomes>();
//                customer.CustomerExpenses = new List<VCustomerExpenses>();
//                lookup.Add(customer.CustomerId!, customer);
//                existingCustomer = customer;
//            }

//            if (income?.IncomeId != null)
//            {
//                existingCustomer.CustomerIncomes.Add(income);
//            }

//            if (expense?.ExpendId != null)
//            {
//                existingCustomer.CustomerExpenses.Add(expense);
//            }

//            return existingCustomer;


//        }, paramerters,
//        splitOn: "IncomeId,ExpendId");

//    var customer = lookup.Values.FirstOrDefault(x => x.CustomerId == customerId);

//    if (customer is null)
//    {
//        return Result.Failure<VCustomerList>(ErrorResponse.NotFound("No Data not found."));
//    }


//    return Result.Success(customer);
//}


// await using var cxt = await contextFactory.CreateDbContextAsync();
//
// var customer = await cxt.CustomerLists.Where(c => c.CustomerId == customerId)
//     .AsNoTracking()
//     .FirstOrDefaultAsync();


// if (customer is not null)
// {
//     var incomes = await cxt.Database
//         .SqlQuery<VCustomerIncomes>($"SELECT * FROM VCustomerIncomes WHERE CustomerRefId={customerId}")
//         .AsNoTracking()
//         .ToListAsync();
//
//     var expenses = await cxt.Database
//         .SqlQuery<VCustomerExpenses>($"SELECT * FROM VCustomerExpenses WHERE CustomerRefId={customerId}")
//         .AsNoTracking()
//         .ToListAsync();
//
//     customer.CustomerExpenses = expenses;
//
//     customer.CustomerIncomes = incomes;
// }