using Applications.Enumerations;
using Applications.Features.Loans.Responses;
using Applications.Responses.Paginations;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Infrastructures.Implements
{
    public static class LoanDisbursementQueryExtensions
    {
        public static IQueryable<VLoanDisbursments> ApplyBranch(this IQueryable<VLoanDisbursments> query, int? branchId)
        {
            return query.Where(x => x.BranchId == branchId);
        }
        public static IQueryable<VLoanDisbursments> ApplyStatus(this IQueryable<VLoanDisbursments> query, LoanStatuses status)
        {
            if (status <= 0)
                return query;
            var statusId = (int)status;
            return query.Where(x => x.LoanstatusId == statusId);
        }

        public static IQueryable<VLoanDisbursments> ApplyUser(this IQueryable<VLoanDisbursments> query, string? userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return query;

            return query.Where(x =>
                x.CreditAgentRefId == userId ||
                x.AprovedUserId == userId ||
                x.RequestUserRefId == userId);
        }

        public static IQueryable<VLoanDisbursments> ApplyDateRange(this IQueryable<VLoanDisbursments> query, DateTime? start, DateTime? to)
        {
            if (!start.HasValue)
                return query;

            var endDate = to ?? DateTime.Now;

            return query.Where(x =>
                x.RegisterDate >= start.Value &&
                x.RegisterDate <= endDate);
        }

        public static IQueryable<VLoanDisbursments> ApplySearch(this IQueryable<VLoanDisbursments> query, string? keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return query;

            var filter = $"{keyword}%";

            return query.Where(x =>
                EF.Functions.Like(x.CustomerName!, filter) ||
                EF.Functions.Like(x.AgentName!, filter) ||
                EF.Functions.Like(x.Phone!, filter) ||
                EF.Functions.Like(x.AltName!, filter));
        }

        public static IQueryable<VLoanDisbursments> ApplySorting(this IQueryable<VLoanDisbursments> query, QueryParam param)
        {
            if (string.IsNullOrWhiteSpace(param.SortColumn))
                return query;

            return param.SortDescending
                ? query.OrderByDescending(x => EF.Property<object>(x, param.SortColumn))
                : query.OrderBy(x => EF.Property<object>(x, param.SortColumn));
        }
    }
}

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

