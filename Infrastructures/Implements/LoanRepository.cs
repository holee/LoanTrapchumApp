using Applications.Commons.Messages.LoanDisbursements;
using Applications.Enumerations;
using Applications.Extensions;
using Applications.Features.Customers.Response;
using Applications.Features.Loans.Dtos;
using Applications.Features.Loans.Dtos.Responses;
using Applications.Interfaces.Loans;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans.Requests;
using Applications.Responses.ViewModels.Loans.Schedules;
using Dapper;
using Domains.Models.Loans;
using ErrorOr;
using Infrastructures.Constants;
using Infrastructures.Constants.Procedures;
using Infrastructures.DataAccess;
using Infrastructures.Implements.Extensions;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Infrastructures.Implements;


public sealed class LoanRepository(ILogger<LoanRepository> logger,
                                  IDapperConnectionFactory connectionFactory,
                                  IDbContextFactory<DataContext> dbContextFactory) : ILoanService
{
    public async Task<ErrorOr<DisburseResult>> CreateLoanDisbursementAsync(LoanDisbursment req, CancellationToken cancellation = default)
    {
        ArgumentNullException.ThrowIfNull(req);

        if (await CustomerAlreadyRequest(req.CustomerRefId))
        {
            return Error.Conflict("CreateLoan.AlreadyRequested", "This customer already has a pending request.");
        }

        if (await CustomerInActiveLoan(req.CustomerRefId))
        {
            return Error.Conflict("CreateLoan.ActiveLoan", "This customer already has an active loan.");
        }

        await using var context = await dbContextFactory.CreateDbContextAsync(cancellation);

        await using var transaction = await context.Database.BeginTransactionAsync(cancellation);

        try
        {
            req.LoanAccountCode = await GetMaxLoanAccountCode(context);

            var entry = context.LoanDisbursments.Add(req);

            await context.SaveChangesAsync(cancellation);

            var loanId = entry.Entity.LoanId;

            await context.Customers
                         .Where(c => c.CustomerId == req.CustomerRefId)
                         .ExecuteUpdateAsync(s => s.SetProperty(c => c.IsLoanActive, true), cancellation);

            await context.LoanRequests
                .Where(r => r.LoanRequestId == req.LoanRequestId
                         && r.CustomerRefId == req.CustomerRefId
                         && r.BranchRefId == req.BranchRefId)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(r => r.LoanRequestStatusRefId, (int)RequestStatus.Completed)
                    .SetProperty(r => r.ModifiedDt, DateTimeOffset.UtcNow), cancellation);

            await context.CollateralsLinks
                .Where(cl => cl.LoanRequestRefId == req.LoanRequestId
                          && cl.BranchId == req.BranchRefId)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(cl => cl.LoanRefId, loanId)
                    .SetProperty(cl => cl.ModifiedDt, DateTimeOffset.UtcNow), cancellation);

            await transaction.CommitAsync(cancellation);

            return new DisburseResult(true, loanId, req.BranchRefId, "Loan disbursed successfully.");
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Failed to disburse loan for customer {CustomerId}", req.CustomerRefId);

            await transaction.RollbackAsync(CancellationToken.None);

            return Error.Unexpected("CreateLoan.DbError", ex.InnerException?.Message ?? ex.Message);
        }
    }


    public async Task<bool> WidthrawalLoanCollateralsAsync(IEnumerable<WidthrawCollateralDtoCreate> req, CancellationToken cancellation = default)
    {
        var items = req as ICollection<WidthrawCollateralDtoCreate> ?? req.ToList();

        if (items.Count == 0) return false;

        var collateralIds = items.Select(r => r.CollateralLineId).Distinct().ToHashSet();

        var loanIds = items.Select(r => r.LoanId).Distinct().ToHashSet();

        var branchIds = items.Select(r => r.BranchId).Distinct().ToHashSet();

        await using var context = await dbContextFactory.CreateDbContextAsync(cancellation);

        try
        {
            var result = await context.CollateralsLinks
                .Where(x => x.IsActive == true
                         && collateralIds.Contains(x.CollateralLineRefId)
                         && loanIds.Contains(x.LoanRefId)
                         && branchIds.Contains(x.BranchId))
                .ExecuteUpdateAsync(s => s.SetProperty(cl => cl.IsActive, false), cancellation);

            return result > 0;
        }
        catch (DbUpdateException ex)
        {
            throw new Exception(ex.InnerException?.Message ?? ex.Message);
        }
    }

    public async Task<IEnumerable<VLoanDisbursementCollateral>> GetRelatedCollaterals(string? loanId, int? branchId, CancellationToken cancellation = default)
    {
        var db = await connectionFactory.CreateConnectionAsync(cancellation);

        string sql = @"SELECT 
	                    d.CollateralLineId,
	                    d.LoanRequestId,
	                    d.BranchId,
	                    d.CollateralNo,
	                    d.ReferenceNo,
	                    d.Location,
	                    d.OwnerName,
	                    d.Quantity,
	                    d.MarketPrice,
	                    d.HotPrice,
	                    d.PawnPrice,
	                    d.TypeName,
	                    d.SubTypeName
                    FROM VLoanDisbursementCollaterals  d 
                    WHERE d.LoanId=@LoanId AND d.BranchId=@BranchId";

        var result = await db.QueryAsync<VLoanDisbursementCollateral>(sql, new { LoanId = loanId, BranchId = branchId });

        return result;

    }

    public async Task<IEnumerable<VLoanDisbursementGuarantors>> GetRelatedGaurantors(string? loanId, int? branchId, CancellationToken cancellation = default)
    {
        var db = await connectionFactory.CreateConnectionAsync(cancellation);

        string sql = @"SELECT 
	                        d.Id,
	                        d.GuarantorId,
	                        d.GuarantorName,
	                        d.Gender,
	                        d.Phone,
                            d.ProInKhmer,
                            d.DisInKhmer,
                            d.ComInKhmer,
                            d.VilInKhmer,
	                        d.Address
                        FROM VLoanDisbursementGuarantors d 
                        WHERE d.LoanId=@LoanId AND d.BranchId=@BranchId";

        var result = await db.QueryAsync<VLoanDisbursementGuarantors>(sql, new { LoanId = loanId, BranchId = branchId });

        return result;

    }

    public async Task<IEnumerable<VLoanDisbursementGuarantors>> GetRelatedGaurantors(string? loanId, int? branchId, CancellationToken cancellation = default, params string[] columns)
    {
        var db = await connectionFactory.CreateConnectionAsync(cancellation);

        var cols = columns.Length == 0 ? "*" : string.Join(",", columns);

        string sql = @$"SELECT {cols}  FROM VLoanDisbursementGuarantors d 
                            WHERE d.LoanId=@LoanId AND d.BranchId=@BranchId
                           ";

        var result = await db.QueryAsync<VLoanDisbursementGuarantors>(sql, new { LoanId = loanId, BranchId = branchId });

        return result;

    }

    public async Task<IEnumerable<VLoanDisbursementDocuments>> GetRelatedDocuments(string? loanId, int? branchId, CancellationToken cancellation = default)
    {
        var db = await connectionFactory.CreateConnectionAsync(cancellation);

        string sql = @"SELECT 
	                        d.Id,
	                        d.OriginalName,
	                        d.AltText,
	                        d.FileUrl,
	                        d.Description
                        FROM VLoanDisbursementDocuments d
                            WHERE d.LoanId = @LoanId AND d.BranchId = @BranchId;
                        ";

        var result = await db.QueryAsync<VLoanDisbursementDocuments>(sql, new { LoanId = loanId, BranchId = branchId });

        return result;

    }

    public async Task<ErrorOr<string>> CloseLoanDisbursementAsync(int? branchId, string? loanId, string? customerId, CancellationToken cancellation = default)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync(cancellation);

        var existingLoan = await context.LoanDisbursments
                                        .FirstOrDefaultAsync(x => x.LoanId == loanId &&
                                                                x.BranchRefId == branchId &&
                                                                x.CustomerRefId == customerId &&
                                                                x.LoanStatusRefId == (int)LoanStatuses.Active,
                                                            cancellation);

        if (existingLoan is null)
        {
            return Error.NotFound("No Loan Data found");
        }
        // Start Transaction
        await using var transaction = await context.Database.BeginTransactionAsync(cancellation);

        try
        {
            existingLoan.LoanStatusRefId = (int)LoanStatuses.Close;


            await context.Customers.Where(x => x.CustomerId == customerId)
                             .ExecuteUpdateAsync(p => p.SetProperty(x => x.IsLoanActive, false), cancellation);

            await context.SaveChangesAsync(cancellation);

            await transaction.CommitAsync(cancellation);

            return "Loan disbursement was closed successfully.";
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.InnerException!.Message);
            await transaction.RollbackAsync(cancellation);
            return Error.Failure($"{ex.InnerException!.Message}");
        }
    }

    public async Task<Pager<VLoanCollateral>> GetLoanCollaterals(string? loanId, int? branchId, CancellationToken cancellation = default)
    {
        {
            using var _context = await connectionFactory.CreateConnectionAsync(cancellation);

            var query = await _context.QueryAsync<VLoanCollateral>(@"SELECT * FROM VLoanCollaterals WHERE LoanId = @LoanId AND BranchId = @BranchId",
                            new
                            {
                                loanId,
                                branchId
                            });

            var counts = query.Count();

            var items = query.ToList();

            return PagerExtensions.Create(items, counts);
        }
    }

    public async Task<Pager<VLoanDisbursments>> GetLoansByBranchId(int? branchId, string? creditAgentId, LoanStatuses status = LoanStatuses.Active, QueryParam? param = null, CancellationToken cancellation = default)
    {

        await using var _context = await dbContextFactory.CreateDbContextAsync(cancellation);

        var queries = _context.VLoanDisbursements
                             .Where(x => x.BranchId == branchId)
                             .WhereIf((int)status > 0, x => x.LoanstatusId == (int)status)
                             .WhereIf(!string.IsNullOrEmpty(creditAgentId), x => x.CreditAgentRefId == creditAgentId ||
                                                                                 x.AprovedUserId == creditAgentId ||
                                                                                 x.RequestUserRefId == creditAgentId);

        if (!string.IsNullOrEmpty(param?.Filter))
        {
            queries = queries.Where(x => EF.Functions.Like(x.CustomerName!, $"{param!.Filter}%") ||
                                         EF.Functions.Like(x.AgentName!, $"{param!.Filter}%") ||
                                         EF.Functions.Like(x.Phone!, $"{param!.Filter}%") ||
                                         EF.Functions.Like(x.AltName!, $"{param!.Filter}%"));
        }


        if (param!.SortDescending)
        {
            queries = queries.OrderByDescending(x => EF.Property<object>(x, param!.SortColumn));
        }

        var counts = await queries.CountAsync(cancellation);

        var items = await queries.Skip(param.Page).Take(param.Take)
                        .AsNoTracking()
                        .ToListAsync(cancellation);

        return new Pager<VLoanDisbursments>(items, counts);
    }

    public async Task<Pager<VLoanDisbursments>> GetLoansByBranchId(int? branchId, string? userId, LoanStatuses status = LoanStatuses.Active, DateTime? start = null, DateTime? to = null, QueryParam? param = null, CancellationToken cancellation = default)
    {

        param ??= new QueryParam(0, 15);

        await using var _context = await dbContextFactory.CreateDbContextAsync(cancellation);

        var queries = _context.VLoanDisbursements
                             .WhereIf(branchId.HasValue && branchId > 0, x => x.BranchId == branchId)
                             .WhereIf((int)status > 0, x => x.LoanstatusId == (int)status)
                             .WhereIf(!string.IsNullOrEmpty(userId), x => x.CreditAgentRefId == userId ||
                                                                                 x.AprovedUserId == userId ||
                                                                                 x.RequestUserRefId == userId);

        if (start.HasValue)
        {
            var filterTo = to ?? DateTime.Now;
            queries = queries.Where(x => x.RegisterDate >= start && x.RegisterDate <= filterTo);
        }

        if (!string.IsNullOrEmpty(param?.Filter))
        {

            string[] props = ["CustomerName", "AgentName", "Phone", "AltName"];
            queries = queries.StartWiths(param.Filter, props);
        }

        if (!string.IsNullOrWhiteSpace(param!.SortColumn))
        {
            queries = param.SortDescending
                ? queries.OrderByDescending(x => EF.Property<object>(x, param.SortColumn))
                : queries.OrderBy(x => EF.Property<object>(x, param.SortColumn));
        }

        var counts = await queries.CountAsync(cancellation);

        var items = await queries.Skip(param.Page).Take(param.Take)
                        .AsNoTracking()
                        .ToListAsync(cancellation);

        return new Pager<VLoanDisbursments>(items, counts);
    }

    public async Task<(Pager<VLoanDisbursments>, int Count)> GetLoansByBranchAsync(int? branchId, string? creditAgentId, LoanStatuses status = LoanStatuses.Active, QueryParam? param = null, CancellationToken cancellation = default)
    {

        await using var _context = await dbContextFactory.CreateDbContextAsync(cancellation);

        var queries = _context.VLoanDisbursements
                             .Where(x => x.BranchId == branchId)
                             .WhereIf((int)status > 0, x => x.LoanstatusId == (int)status)
                             .WhereIf(!string.IsNullOrEmpty(creditAgentId), x => x.CreditAgentRefId == creditAgentId ||
                                                                                 x.AprovedUserId == creditAgentId ||
                                                                                 x.RequestUserRefId == creditAgentId);

        if (!string.IsNullOrEmpty(param?.Filter))
        {
            queries = queries.Where(x => EF.Functions.Like(x.CustomerName!, $"%{param!.Filter}%") ||
                                         EF.Functions.Like(x.AgentName!, $"%{param!.Filter}%") ||
                                         EF.Functions.Like(x.Phone!, $"%{param!.Filter}%") ||
                                         EF.Functions.Like(x.AltName!, $"%{param!.Filter}%"));
        }


        if (param!.SortDescending)
        {
            queries = queries.OrderByDescending(x => EF.Property<object>(x, param!.SortColumn));
        }

        var counts = await queries.CountAsync(cancellation);

        var items = await queries.Skip(param.Page).Take(param.Take)
                        .AsNoTracking()
                        .ToListAsync(cancellation);

        return (new Pager<VLoanDisbursments>(items, counts), counts);
    }

    public async Task<ErrorOr<VLoanDisbursments>> GetLoanByIdAsync(string? loanId, int? branchId, ScheduleStatus status, bool include = false)
    {
        using var db = connectionFactory.CreateConnection();

        if (include)
        {
            var jointDictionary = new Dictionary<string, VLoanDisbursments>();

            var queries = await db.QueryAsync<VLoanDisbursments, VSchedule, VLoanDisbursments>(ProcedureConstant.GetLoanById,
                (disbursment, schedule) =>
                {
                    if (jointDictionary.TryGetValue(disbursment.LoanId!, out var existingDis))
                    {
                        disbursment = existingDis!;
                    }
                    else
                    {
                        jointDictionary.Add(disbursment.LoanId!, disbursment);
                    }

                    disbursment.Schedules?.Add(schedule);

                    return disbursment;
                },
                param: new
                {
                    @LoanId = loanId,

                    @BranchId = branchId,

                    @ScheduleStatusId = (int)status,

                    @Include = include
                }, splitOn: "LoanScheduleId"
                , commandType: CommandType.StoredProcedure);

            var item = queries.FirstOrDefault();

            if (item is null)
            {
                return Error.NotFound(LoanMessages.LoanNotFound);
            }

            return item;
        }

        var result = await db.QueryFirstOrDefaultAsync<VLoanDisbursments>("spGetLoanById",
            param: new
            {
                @LoanId = loanId,

                @BranchId = branchId,

                @ScheduleStatusId = (int)status,

                @Include = include
            }, commandType: CommandType.StoredProcedure);


        if (result is null)
        {
            return Error.NotFound(LoanMessages.LoanNotFound);
        }

        return result;
    }


    public async Task<ErrorOr<VLoanDisbursments>> GetLoanByIdAsync(string? loanId, int? branchId, ScheduleStatus status, bool include = false, CancellationToken cancellation = default)
    {

        await using var _context = await dbContextFactory.CreateDbContextAsync(cancellation);

        var queries = _context.VLoanDisbursements
                                 .Where(x => x.LoanId == loanId && x.BranchId == branchId)
                                 .AsNoTracking();


        if (!include && queries.Any())
        {

            var loanDisbursement = await queries.FirstOrDefaultAsync(cancellation);

            return loanDisbursement is not null
                ? loanDisbursement
                : ErrorOr.Error.NotFound("NotFound", "No loan disbursement found.");

        }

        int?[] filter = status switch
        {
            ScheduleStatus.Paid => [1],
            ScheduleStatus.Unpaid => [2, 4],
            ScheduleStatus.All => [1, 2, 4],
            _ => [1, 2, 4]
        };

        var item1 = await queries
                            .Include(x => x.Schedules!.Where(x => x.LoanId == loanId && filter.Contains(x.ScheduleStatusId)))
                            .AsNoTracking()
                            .AsSplitQuery()
                            .FirstOrDefaultAsync(cancellation);

        return item1 is not null
                            ? item1
                            : ErrorOr.Error.NotFound("NotFound", "No loan disbursement found.");
    }


    public async Task<Pager<VSchedule>> GetDueScheduleLoanByIdAsync(string? loanId, int? branchId, CancellationToken cancellation = default)
    {

        await using var _context = await dbContextFactory.CreateDbContextAsync(cancellation);

        var item1 = _context.Set<VSchedule>().Where(x => x.BranchId == branchId
                                    && x.LoanId == loanId
                                    && x.PaymentDate <= DateTime.UtcNow
                                    && x.ScheduleStatusId != (int)ScheduleStatus.Paid)
                                    .OrderBy(x => x.InstallmentNo)
                                    .ThenBy(x => x.PaymentDate);

        var counts = await item1.CountAsync(cancellation);

        var items = await item1.AsNoTracking().ToListAsync(cancellation);

        return PagerExtensions.Create(items, counts);
    }



    public async Task<Pager<VSchedule>> GetScheduleLoanByIdAsync(string? loanId, int? branchId, ScheduleStatus state, CancellationToken cancellation = default)
    {

        await using var _context = await dbContextFactory.CreateDbContextAsync(cancellation);

        var baseQueries = _context.Set<VSchedule>().Where(x => x.BranchId == branchId && x.LoanId == loanId);

        baseQueries = state switch
        {
            ScheduleStatus.Paid => baseQueries.Where(x => x.ScheduleStatusId == (int)ScheduleStatus.Paid),
            ScheduleStatus.Unpaid => baseQueries.Where(x => x.ScheduleStatusId != (int)ScheduleStatus.Paid && x.PaymentDate <= DateTime.Now),
            ScheduleStatus.All => baseQueries,
            _ => baseQueries
        };

        baseQueries = baseQueries.OrderBy(x => x.InstallmentNo).ThenBy(x => x.PaymentDate);

        var counts = await baseQueries.CountAsync(cancellation);

        var items = await baseQueries.AsNoTracking().ToListAsync(cancellation);

        return PagerExtensions.Create(items, counts);
    }


    public async Task<IEnumerable<VSchedule>> GetLoanSchedulesAsync(string? loanId, int? branchId,
                    ScheduleStatus status, bool include = false,
                    QueryParam? param = null,
                    CancellationToken cancellation = default)
    {

        await using var _context = await dbContextFactory.CreateDbContextAsync(cancellation);

        int?[] filter = status switch
        {
            ScheduleStatus.Paid => [1],
            ScheduleStatus.Unpaid => [2, 4],
            ScheduleStatus.All => [1, 2, 4],
            _ => [1, 2, 4]
        };


        var queries = _context.VSchedules
                                 .Where(x => x.LoanId == loanId &&
                                             x.BranchId == branchId &&
                                             filter.Contains(x.ScheduleStatusId))
                                 .OrderBy(x => x.InstallmentNo)
                                 .AsNoTracking();
        if (param?.Take > 0)
        {
            queries = queries.Take(param.Take);
        }

        var item1 = await queries.AsNoTracking().ToListAsync(cancellation);

        return item1;

    }



    public async Task<Result<VLoanDisbursments>> GetLoanByIdAsync(string? loanId, int? branchId)
    {
        using var db = connectionFactory.CreateConnection();

        var result = await db.QueryFirstOrDefaultAsync<VLoanDisbursments>(ProcedureConstant.GetLoanById,
            param: new
            {
                @LoanId = loanId,
                @BranchId = branchId,
                @ScheduleStatusId = 0,
                @Include = false
            }, commandType: CommandType.StoredProcedure);


        if (result is null)
        {
            return Applications.Responses.Result.Failure<VLoanDisbursments>(ErrorResponse.NotFound("No Data was found."));
        }

        return Applications.Responses.Result.Success(result);
    }




    public async Task<Result<VLoanDisbursments>> GetLoanWithSchedulesByIdAsync(string? loanId, int? branchId, ScheduleStatus status, bool include = false)
    {
        using var db = connectionFactory.CreateConnection();

        var parameters = new
        {
            LoanId = loanId,
            BranchId = branchId,
            ScheduleStatusId = (int)status,
            Include = include
        };

        if (include)
        {
            var loanMap = new Dictionary<string, VLoanDisbursments>();

            var queries = await db.QueryAsync<VLoanDisbursments, VSchedule, VLoanDisbursments>("spGetLoanById",
                (disbursment, schedule) =>
                {
                    if (!loanMap.TryGetValue(disbursment.LoanId!, out var existing))
                    {
                        disbursment.Schedules = new List<VSchedule>();

                        loanMap[disbursment.LoanId!] = disbursment;

                        existing = disbursment;
                    }

                    if (schedule != null)
                    {
                        existing.Schedules?.Add(schedule);
                    }

                    return existing;
                },
                param: parameters,
                splitOn: "LoanScheduleId",
                commandType: CommandType.StoredProcedure
            );

            var resultItem = loanMap.Values.FirstOrDefault();

            if (resultItem is null)
            {
                return Applications.Responses.Result.Failure<VLoanDisbursments>(ErrorResponse.NotFound("No data was found."));
            }

            return Applications.Responses.Result.Success(resultItem);
        }

        var result = await db.QueryFirstOrDefaultAsync<VLoanDisbursments>(
            "spGetLoanById",
            param: parameters,
            commandType: CommandType.StoredProcedure);

        if (result is null)
        {
            return Applications.Responses.Result.Failure<VLoanDisbursments>(
                ErrorResponse.NotFound("No data was found."));
        }

        return Applications.Responses.Result.Success(result);
    }



    public async Task<VLoanDisbursments?> GetLoanByCreditAgentIdAsync(string? creditAgentId, string? loanId, int? branchId, CancellationToken cancellation = default)
    {
        await using var _context = await dbContextFactory.CreateDbContextAsync(cancellation);

        var query = await _context.VLoanDisbursements
                                .FirstOrDefaultAsync(x => x.CreditAgentRefId == creditAgentId &&
                                          x.LoanId == loanId &&
                                          x.BranchId == branchId);
        return query;
    }

    public async Task<Pager<VLoanDisbursments>> GetLoanByCreditAgentAsync(string? creditAgentId, int? branchId, CancellationToken cancellation = default)
    {
        await using var _context = await dbContextFactory.CreateDbContextAsync(cancellation);

        var query = _context.VLoanDisbursements
                                  .Where(x => x.CreditAgentRefId == creditAgentId &&
                                                            x.BranchId == branchId
                                                            )
                                  .OrderByDescending(x => x.RegisterDate)
                                  .AsNoTracking();

        var counts = await query.CountAsync(cancellation);

        var items = await query.AsNoTracking().ToListAsync(cancellation);

        return new Pager<VLoanDisbursments>(items, counts);
    }



    private async Task<long> GetMaxLoanAccountCode(DataContext context)
    {

        var maxCode = await context.LoanDisbursments
                        .Select(x => (long?)x.LoanAccountCode)
                        .MaxAsync() ?? 0;

        return maxCode + 1;
    }

    private async Task<bool> CustomerAlreadyRequest(string? customerId)
    {
        if (string.IsNullOrEmpty(customerId)) return false;

        using var db = await connectionFactory.CreateConnectionAsync();

        var parameter = new DynamicParameters();

        parameter.Add("@CustomerId", customerId);

        var existingLoanRequest = await db.QueryAsync<VLoanRequests>(SqlConstant.CustomerAlreadyRequest, parameter);

        return existingLoanRequest.Any();
    }

    private async Task<bool> CustomerInActiveLoan(string? customerId)
    {
        if (string.IsNullOrEmpty(customerId)) return false;
        using var db = await connectionFactory.CreateConnectionAsync();
        var parameter = new DynamicParameters();
        parameter.Add("@CustomerId", customerId);
        parameter.Add("@IsLoanActive", true);
        var customerInActiveLoan = await db.QueryAsync<VCustomerDetails>(SqlConstant.CustomerInActiveLoan, parameter);
        return customerInActiveLoan.Any();
    }

    private async Task<bool> IsLoanIsActive(DataContext context, string? loanId, int? branchId)
    {
        return await context.VLoanDisbursements
               .AnyAsync(x => x.LoanId == loanId &&
                              x.BranchId == branchId &&
                              x.LoanstatusId == (int)LoanStatuses.Active);
    }


}


//public async Task<Result<string>> ChangedCreditAgent(int? branchId, LoanChangedCaDtoCreate req)
//{


//    await using var _context = await dbContextFactory.CreateDbContextAsync();

//    var exstingLoan = _context.LoanDisbursments.Where(x =>
//        x.LoanId == req.LoanRefId && x.BranchRefId == branchId &&
//        x.CreditAgentRefId == req.OldCreditAgentRefId);

//    if (!exstingLoan.Any())
//    {
//        return Result.Failure<string>(ErrorResponse.NotFound("No Loan Data found"));
//    }

//    var trx = _context.Database.BeginTransaction();

//    try
//    {
//        var model = req.Adapt<LoanChangedCa>();

//        _context.LoanChangedCa.Add(model);

//        await _context.SaveChangesAsync();
//        var editSeq = exstingLoan.First().EditSeq + 1;
//        _context.LoanDisbursments
//            .Where(x => x.LoanId == req.LoanRefId &&
//                        x.BranchRefId == branchId &&
//                        x.CreditAgentRefId == req.OldCreditAgentRefId)
//            .ExecuteUpdate(prop => prop.SetProperty(p => p.CreditAgentRefId, req.NewCreditAgentRefId)
//                .SetProperty(p => p.ModifiedDt, DateTime.UtcNow)
//                .SetProperty(p => p.EditSeq, editSeq));

//        trx.Commit();

//        return Result.Success<string>(new("Loan has changed new credit agent."));
//    }
//    catch (Exception ex)
//    {
//        logger.LogError(ex, ex.InnerException!.Message);

//        trx.Rollback();

//        return Result.Failure<string>(ErrorResponse.NotFound($"{ex.InnerException!.Message}"));
//    }
//}

//public async Task<Result<string>> ChangedCreditAgents(int? branchId, List<LoanChangedCa> req)
//{

//    await using var _context = await dbContextFactory.CreateDbContextAsync();


//    var trx = _context.Database.BeginTransaction();

//    try
//    {
//        var model = req.Adapt<IEnumerable<LoanChangedCa>>();

//        _context.LoanChangedCa.AddRange(model);

//        await _context.SaveChangesAsync();

//        var oldCreditAgentId = req.First().OldCreditAgentRefId;

//        var newCreditAgentRefId = req.First().NewCreditAgentRefId;

//        _context.LoanDisbursments
//            .Where(x => x.BranchRefId == branchId && x.CreditAgentRefId == oldCreditAgentId)
//            .ExecuteUpdate(prop => prop.SetProperty(p => p.CreditAgentRefId, newCreditAgentRefId)
//                .SetProperty(p => p.ModifiedDt, DateTime.UtcNow));

//        trx.Commit();

//        return Result.Success<string>(new("Loan has changed new credit agent."));
//    }
//    catch (Exception ex)
//    {
//        logger.LogError(ex, ex.InnerException!.Message);
//        trx.Rollback();
//        return Result.Failure<string>(ErrorResponse.NotFound($"{ex.InnerException!.Message}"));
//    }
//}



//public async Task<ErrorOr.ErrorOr<DeleteResult>> DeleteAsync(string? loanId, int? branchId, CancellationToken cancellation = default)
//{

//    await using var _context = await dbContextFactory.CreateDbContextAsync(cancellation);

//    if (await IsLoanIsActive(_context, loanId, branchId))
//    {
//        return ErrorOr.Error.Conflict("Cannot delete an active loan.");
//    }

//    var loan = _context.LoanDisbursments
//                        .FirstOrDefault(x => x.LoanId == loanId &&
//                                    x.BranchRefId == branchId);

//    if (loan is null)
//    {
//        return ErrorOr.Error.NotFound("No Loan Data.");
//    }

//    _context.LoanDisbursments.Remove(loan);

//    await _context.SaveChangesAsync(cancellation);

//    return DeleteResult.Success("Loan disbursement was deleted successfully.");
//}

//public async Task<Result<string>> UpdateAsync(LoanDisbursment disbursement)
//{

//    await using var _context = await dbContextFactory.CreateDbContextAsync();

//    _context.LoanDisbursments.Update(disbursement);

//    await _context.SaveChangesAsync();

//    return Result.Success(new string("New Loan Disbursement was added."));
//}



//public async Task<Result<VSchedule>> GetPaymentScheduleById(string? loanId, long? loanScheduleId, int? branchId, bool related)
//{

//    var schedule = await _context.Database.SqlQuery<VSchedule>($"SELECT * FROM  VSchedules WHERE LoanId={loanId} AND  BranchId={branchId} AND LoanScheduleId={loanScheduleId}")
//                                          .FirstOrDefaultAsync();
//    if (schedule is null)
//    {
//        return Result.Failure<VSchedule>(ErrorResponse.NotFound("No Schedule Found."));
//    }

//    if (related)
//    {

//        var paymentLines = await _context.Database.SqlQuery<VPaymentLines>($"SELECT * FROM VLoanRepaymentLines WHERE LoanId={loanId} AND LoanScheduleRefId={loanScheduleId}")
//                                        .ToListAsync();

//        schedule.PaymentLines = paymentLines;

//    }

//    return Result.Success(schedule);
//}
//public async Task<Result<IEnumerable<VSchedule>>> GetLoanPaymentSchedules(DateTime paymentDate, int branchId, bool includeLateSchedule = true, string? filter = null)
//{
//    using var db = _connectionFactory.CreateConnection();

//    var schedules = await db.QueryAsync<VSchedule>("spGetCustomerPayementSchedules", new
//    {
//        paymentDate,
//        branchId,
//        includeLateSchedule,
//        filter
//    }, commandType: CommandType.StoredProcedure);
//    if (!schedules.Any())
//    {
//        return Result.Failure<IEnumerable<VSchedule>>(ErrorResponse.NotFound("No Payment schedule found."));
//    }
//    return Result.Success(schedules);
//}
//public async Task<Result<IEnumerable<VSchedule>>> GetPaymentSchedulesByLoanId(string? loanId, int? branchId, ScheduleStatus status = ScheduleStatus.All)
//{
//    //0 all
//    //1 not paid
//    //2 only paid
//    //3 only partial 
//    var db = _connectionFactory.CreateConnection();

//    var items = await db.QueryAsync<VSchedule>("spGetSchedulesById", new
//    {
//        branchId,
//        loanId,
//        @statusId = (int)status
//    }, commandType: CommandType.StoredProcedure);

//    if (!items.Any())
//    {
//        return Result.Failure<IEnumerable<VSchedule>>(ErrorResponse.NotFound("No Schedule Found."));
//    }
//    return Result.Success(items);
//}


//private async Task<Result<VLoanDisbursments>> GetLoanByIdWithSchedulesAsync(string? loanId, int? branchId, ScheduleStatus status)
//{
//    using var db = _connectionFactory.CreateConnection(); 

//    var jointDictionary = new Dictionary<string, VLoanDisbursments>();

//    var queries = await db.QueryAsync<VLoanDisbursments, VSchedule, VLoanDisbursments>("spGetLoanById",

//             (disbursment, schedule) =>
//             {
//                 if (jointDictionary.TryGetValue(disbursment.LoanId!, out var existingDis))
//                 {
//                     disbursment = existingDis!;
//                 }
//                 else
//                 {
//                     jointDictionary.Add(disbursment.LoanId!, disbursment);
//                 }

//                 disbursment.Schedules?.Add(schedule);

//                 return disbursment;
//             },
//            param: new
//            {
//                @LoanId = loanId,

//                @BranchId = branchId,

//                @ScheduleStatusId = (int)status,

//            }, splitOn: "LoanScheduleId"
//            , commandType: CommandType.StoredProcedure);

//    var item = queries.FirstOrDefault();

//    if (item is null)
//    {
//        return Result.Failure<VLoanDisbursments>(ErrorResponse.NotFound("No Data was found."));
//    }

//    return Result.Success(item);
//}


//private async Task<Result<VLoanDisbursments>> GetLoanByIdOnlyAsync(string? loanId, int? branchId, ScheduleStatus status)
//{
//    using var db = _connectionFactory.CreateConnection();

//    var item = await db.QueryFirstAsync<VLoanDisbursments>(@"SELECT * FROM VLoansDisbursments WHERE LoanId=@LoanId AND
//                                                             BranchId=@BranchId AND ScheduleStatusId=@ScheduleStatusId
//                                                             ",
//            param: new
//            {
//                @LoanId = loanId,

//                @BranchId = branchId,

//                @ScheduleStatusId = (int)status,

//            });

//    if (item is null)
//    {
//        return Result.Failure<VLoanDisbursments>(ErrorResponse.NotFound("No Data was found."));
//    }

//    return Result.Success(item);
//}


////var items = _context.Database
////                    .SqlQuery<VLoanDisbursments>($"""
////                                                  SELECT * FROM VLoanDisbursments WHERE 
////                                                  ({branchId} > 0 AND BranchId={branchId} OR {branchId}=0) AND LoanStatusId={status}
////                                                  AND ({creditAgentId!.Length}> 0 AND CreditAgentId={creditAgentId} OR {creditAgentId!.Length}=0
////                                                  """);

////if (!string.IsNullOrEmpty(param?.Filter))
////{

////}
///        //public async Task<Result<string>> CreateLoanAsync(LoanDtoCreate req)
//{


//    await using var context = await dbContextFactory.CreateDbContextAsync();


//    await using var trans = await context.Database.BeginTransactionAsync();

//    try
//    {
//        var customerInActiveLoan = await context.Customers
//            .AsNoTracking()
//            .FirstOrDefaultAsync(c => c.CustomerId == req.CustomerRefId && c.IsLoanActive == true);

//        if (customerInActiveLoan is not null)
//        {
//            return Result.Failure<string>(ErrorResponse.Conflict("This Customer Is Already disburse"));
//        }

//        var tranx = new LoanTransaction
//        {
//            TransactionsId = $"trx-{Guid.CreateVersion7()}",
//            UserRefId = req.UserRefId,
//            TransactionTypeName = "Loan Disbursement",
//            TranTypeRefId = 19,
//            IsActive = true
//        };

//        var cashMgt = new CashManagement
//        {
//            Id = $"cash-{Guid.CreateVersion7()}",

//            BankRefId = req.BankRefId,

//            BranchRefId = req.BranchId,

//            Amount = req.PaidAmount,

//            CurrencyRefId = req.CurrencyRefId,

//            CashStatusRefId = (int)CashStatusEnum.OUT,

//            Remark = "Cash goes through transaction",

//            TransactionDate = req.RegisterDate,

//            BusinessDate = req.RegisterDate,

//            Balance = 0,

//            EmployeeRefId = req.UserRefId
//        };

//        tranx.CashManagements.Add(cashMgt);

//        var model = req.MapLoanCreate();

//        model.LoanAccountCode = await GetMaxLoanAccountCode(context);

//        model.LoanTransaction = tranx;

//        context.LoanDisbursments.Add(model);

//        await context.SaveChangesAsync();

//        await context.Customers.Where(x => x.CustomerId == req.CustomerRefId)
//            .ExecuteUpdateAsync(p => p.SetProperty(x => x.IsLoanActive, true));

//        await context.LoanRequests.Where(x => x.LoanRequestId == req.LoanRequestId &&
//                                              x.CustomerRefId == req.CustomerRefId)
//            .ExecuteUpdateAsync(prop => prop
//                .SetProperty(p => p.LoanRequestStatusRefId, (int)RequestStatus.Completed)
//                .SetProperty(p => p.ModifiedDt, DateTime.UtcNow));

//        await trans.CommitAsync();

//        return Result.Success(new string("New Loan Disbursement was added."));
//    }
//    catch (DbUpdateException ex)
//    {
//        await trans.RollbackAsync();

//        return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));
//    }
//}

//public async Task<Pager<VLoanDisbursments>> GetLoansByBranchId(int? branchId, string? creditAgentId, LoanStatuses status = LoanStatuses.Active, QueryParam? param = null)
//{
//    using var db = connectionFactory.CreateConnection();

//    var queries = await db.QueryAsync<VLoanDisbursments>(ProcedureConstant.SpGetLoansByBranchId, new
//    {
//        @BranchId = branchId,
//        @CreditAgentId = creditAgentId,
//        @LoanStatusId = (int)status,
//        @SortColumn = param?.SortColumn,
//        @SortDescending = param?.SortDescending,
//        @PageIndex = param?.Skip ?? 1,
//        @PageSize = param?.PageSize ?? 25,
//        @Filter = param?.Filter ?? string.Empty
//    }, commandType: CommandType.StoredProcedure);

//    var counts = queries.Count();

//    var items = queries.ToList();

//    return new Pager<VLoanDisbursments>(items, counts);
//}

//public async Task<Result<bool>> CreateLoanAsync(LoanDisbursment req, CancellationToken cancellation = default)
//{

//    if (await CustomerAlreadyRequest(req.CustomerRefId))
//    {
//        return Result.Failure<bool>(ErrorResponse.Conflict("This Customer Is Already Requested"));
//    }

//    if (await CustomerInActiveLoan(req.CustomerRefId))
//    {
//        return Result.Failure<bool>(ErrorResponse.Conflict("This Customer Is Already disburse"));
//    }

//    await using var context = await dbContextFactory.CreateDbContextAsync(cancellation);

//    await using var trans = await context.Database.BeginTransactionAsync(cancellation);

//    try
//    {
//        req.LoanAccountCode = await GetMaxLoanAccountCode(context);

//        context.LoanDisbursments.Add(req);

//        await context.SaveChangesAsync(cancellation);

//        await context.Customers
//                     .Where(x => x.CustomerId == req.CustomerRefId)
//                     .ExecuteUpdateAsync(p => p.SetProperty(x => x.IsLoanActive, true));

//        await context.LoanRequests.Where(x => x.LoanRequestId == req.LoanRequestId &&
//                                              x.CustomerRefId == req.CustomerRefId)
//            .ExecuteUpdateAsync(prop => prop
//                .SetProperty(p => p.LoanRequestStatusRefId, (int)RequestStatus.Completed)
//                .SetProperty(p => p.ModifiedDt, DateTime.UtcNow));

//        await trans.CommitAsync();

//        return Result.Success(true);
//    }
//    catch (DbUpdateException ex)
//    {
//        await trans.RollbackAsync(cancellation);

//        return Result.Failure<bool>(ErrorResponse.ServerError($"{ex.Message}"));
//    }
//}
