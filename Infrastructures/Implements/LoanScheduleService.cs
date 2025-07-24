using Applications.Enumerations;
using Applications.Interfaces.Loans;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans.Payments;
using Applications.Responses.ViewModels.Loans.Schedules;
using Dapper;
using Infrastructures.Constants.Procedures;
using Infrastructures.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Infrastructures.Implements
{
    public class LoanScheduleService : ILoanScheduleService
    {
        private readonly DataContext _context;
        private readonly ILogger<LoanDisbursementRepository> _logger;
        private readonly IDapperConnectionFactory _connectionFactory;
        public LoanScheduleService(DataContext context, ILogger<LoanDisbursementRepository> logger, IDapperConnectionFactory connectionFactory)
        {
            _context = context;
            _logger = logger;
            _connectionFactory = connectionFactory;
        }


        public async Task<Result<IEnumerable<VSchedule>>> GetLoanPaymentSchedules(DateTime paymentDate, int branchId, bool includeLateSchedule = true, string? filter = null)
        {
            using var db = _context.Database.GetDbConnection();

            var schedules = await db.QueryAsync<VSchedule>("spGetCustomerPayementSchedules", new
            {
                paymentDate,
                branchId,
                includeLateSchedule,
                filter
            }, commandType: CommandType.StoredProcedure);
            if (!schedules.Any())
            {
                return Result.Failure<IEnumerable<VSchedule>>(ErrorResponse.NotFound("No Payment schedule found."));
            }
            return Result.Success(schedules);
        }

        public async Task<Result<VSchedule>> GetSchedulesByIdAsync(string? loanId, int? branchId, long? scheduleId)
        {
            using var db = _context.Database.GetDbConnection();


            var items = await db.QueryMultipleAsync(ProcedureConstant.GetLoanById,
               new
               {
                   @LoanId = loanId,
                   @BranchId = branchId,
                   @ScheduleId = scheduleId
               }, commandType: CommandType.StoredProcedure);


            var schedule = await items.ReadFirstOrDefaultAsync<VSchedule>();

            if (schedule is null)
            {
                return Result.Failure<VSchedule>(ErrorResponse.NotFound("No Schedule found."));
            }
            var lines = await items.ReadAsync<LoanPaymentLineVm>();

            schedule.PaymentLines = lines.ToList() ?? [];

            return Result.Success(schedule);
        }

        public async Task<Result<LoanScheduleVm>> GetLoanSchedulesAsync(int? branchId, string? loanId, string? customerId)
        {
            var connection = _connectionFactory.CreateConnection();

            var lookupDictionary = new Dictionary<string, LoanScheduleVm>();

            var items = await connection.QueryAsync<LoanScheduleVm, ScheduleViewModel, LoanScheduleVm>("getCustomerSchedules",
                (customer, schedule) =>
                {
                    if (lookupDictionary.TryGetValue(customer.LoanId!, out var existingCustomer))
                    {
                        customer = existingCustomer!;
                    }
                    else
                    {
                        lookupDictionary.Add(customer.LoanId!, customer);
                    }

                    customer.LoanSchedules.Add(schedule);

                    return customer;

                }, new
                {
                    loanId,
                    customerId,
                    branchId
                },
                commandType: CommandType.StoredProcedure,
                splitOn: "LoanScheduleId");

            if (items.Any())
            {
                return Result.Failure<LoanScheduleVm>(ErrorResponse.NotFound("No Schedule to print."));
            }

            var item = lookupDictionary[loanId!];

            return Result.Success(item);
        }


        public async Task<Pager<VSchedule>> GetLoanScheduleByIdAsync(string? loanId, int? branchId, ScheduleStatus status = ScheduleStatus.All, QueryParam? param = null)
        {
            var items = await _context.Database.SqlQuery<VSchedule>($"""
                                                                        EXEC spGetVSchedulesByLoanId
                                                                            {loanId},
                                                                            {branchId},
                                                                            {(int)status},
                                                                            {param?.Skip},
                                                                            {param?.PageSize},
                                                                            {param?.Filter}
                                                                      """)
                                           .AsNoTracking()
                                           .ToListAsync();

            var counts = items.Count();

            return new Pager<VSchedule>(items, counts);
        }


        public async Task<Result<IEnumerable<VSchedule>>> GetLoanScheduleByIdAsync(string? loanId, int? branchId, ScheduleStatus status = ScheduleStatus.All)
        {

            var db = _connectionFactory.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@LoanId", loanId);

            parameters.Add("@BranchId", branchId);

            parameters.Add("@ScheduleStatusId", (int)status);

            var items = await db.QueryAsync<VSchedule>(ProcedureConstant.SpGetSchedulesByLoanId, parameters, commandType: CommandType.StoredProcedure);

            return Result.Success(items);
        }



        //

        public async Task<Pager<VSchedule>> GetLoanSchedulesByPaymentDateAsync(DateTimeOffset? paymentDate, int? branchId, bool isIncludeLate = false, bool isLateOnly = false, QueryParam? param = null)
        {
            
            using var db = _connectionFactory.CreateConnection();

            param ??= new QueryParam();

            var pageNumber = (int)Math.Floor((param.Skip) / (double)(param.PageSize)) + 1;

            var parameters = new DynamicParameters();

            parameters.Add("@BranchId", branchId ?? 0, DbType.Int32);

            parameters.Add("@PaymentDate", paymentDate?.DateTime ?? DateTime.Now, DbType.DateTime);

            parameters.Add("@IsIncludeLate", isIncludeLate, DbType.Boolean);

            parameters.Add("@IsLateOnly", isLateOnly, DbType.Boolean);

            parameters.Add("@PageNumber", pageNumber, DbType.Int32);

            parameters.Add("@PageSize", param.PageSize, DbType.Int32);

            parameters.Add("@Filter", param.Filter ?? string.Empty, DbType.String);


            var result = await db.QueryAsync<VSchedule>(ProcedureConstant.SpGetVSchedulesByPaymentDate,
                                    parameters,
                                    commandType: CommandType.StoredProcedure
                                );

            var items=result.ToList();

            return new Pager<VSchedule>(items, items.Count);



        }



        public async Task<Pager<VSchedule>> GetLateSchedulesAsync(int? branchId, string? loanId, DateTimeOffset? paymentDate, CancellationToken cancellation = default)
        {
            using var db = await _connectionFactory.CreateConnectionAsync(cancellation);

            var queries = await db.QueryAsync<VSchedule>(ProcedureConstant.SpGetLateVSchedules,
                new
                {
                    branchId,
                    loanId,
                    paymentDate
                },
                commandType: CommandType.StoredProcedure);

            var counts = queries.Count();

            var items= queries.ToList();

            return new Pager<VSchedule>(items, counts);
        }


        public async Task<Result<IEnumerable<LoanPaymentLineVm>>> GetPayementScheduleLines(int branchId, long loanId, int installmentNo)
        {

            var db = _connectionFactory.CreateConnection();

            var sql = @"SELECT * FROM VLoanPaymentLines WHERE 
                                 BranchRefId=@branchId AND LoanRefId=@loanId AND 
                                 InstallmentNo=@installmentNo";
            var paymentLines = await db.QueryAsync<LoanPaymentLineVm>(sql, new
            {
                branchId,
                loanId,
                installmentNo
            });

            if (!paymentLines.Any())
            {
                return Result.Failure<IEnumerable<LoanPaymentLineVm>>(ErrorResponse.NotFound("No Payment schedule found."));
            }
            return Result.Success(paymentLines);
        }


    }
}


//public async Task<Result<VSchedule>> GetLoanScheduleByIdAsync(long loanId, long loanScheduleId, int branchId, bool related)
//{

//    var schedule = await _context.Database.SqlQuery<VSchedule>($"SELECT * FROM  VSchedules WHERE LoanId={loanId} AND  BranchId={branchId} AND LoanScheduleId={loanScheduleId}")
//                                          .FirstOrDefaultAsync();
//    if (schedule is null)
//    {
//        return Result.Failure<VSchedule>(Error.NotFound("404", ["No Schedule Found."]));
//    }

//    if (related)
//    {

//        var paymentLines = await _context.Database.SqlQuery<VPaymentLines>($"SELECT * FROM VLoanRepaymentLines WHERE LoanId={loanId} AND LoanScheduleRefId={loanScheduleId}")
//                                        .ToListAsync();

//        schedule.PaymentLines = paymentLines ?? [];

//    }

//    return Result.Success<VSchedule>(schedule);
//}

//private async Task<Result<VLoans>> GetLoanDetailWithRelation(long loanId, int branchId)
//{
//    using var db = _context.Database.GetDbConnection();
//    var multi = await db.QueryMultipleAsync("spGetLoanDetials",
//                                new
//                                {
//                                    @LoanId = loanId,
//                                    @BranchId = branchId
//                                }, commandType: CommandType.StoredProcedure);

//    var existongLoan = await multi.ReadFirstAsync<VLoans>();

//    if (existongLoan is null)
//    {
//        return Result.Failure<VLoans>(Error.NotFound("404", ["no data found."]));
//    }

//    var schedules = multi.Read<VSchedule>().ToList();

//    var paidSchedules = multi.Read<VSchedulePaid>().ToList();

//    existongLoan.Schedules = schedules ?? [];

//    existongLoan.PaidSchedules = paidSchedules ?? [];

//    return Result.Success(existongLoan);

//}

//private async Task<Result<VLoans>> GetOnlyLoanInfo(long loanId, int branchId)
//{
//    using var db = _context.Database.GetDbConnection();
//    var existongLoan = await db.QueryFirstOrDefaultAsync<VLoans>("spGetLoanOnlyDetials",
//                                new
//                                {
//                                    @LoanId = loanId,
//                                    @BranchId = branchId
//                                }, commandType: CommandType.StoredProcedure);

//    if (existongLoan is null)
//    {
//        return Result.Failure<VLoans>(Error.NotFound("404", ["no data found."]));
//    }
//    return Result.Success(existongLoan);


//}

//private async Task<Pager<VSchedule>> GetLateSchedules(int? branchId, DateTimeOffset? paymentDate = null, QueryParam? param = null)
//{

//    using var db = _connectionFactory.CreateConnection();

//    paymentDate ??= DateTimeOffset.Now;

//    var items = await db.QueryAsync<VSchedule>("spGetCustomerLateSchedules",
//        new
//        {
//            branchId,
//            paymentDate,
//            param?.PageIndex,
//            param?.PageSize,
//            param?.SortColumn,
//            param?.SortDescending,
//            param?.Filter
//        },
//       commandType: CommandType.StoredProcedure)
//        ;

//    var counts = items.Count();

//    return new Pager<VSchedule>(items.ToList(), counts);



//}

//private async Task<Pager<VSchedule>> GetOnDateSchedules(int? branchId, DateTimeOffset? paymentDate = null, QueryParam? param = null)
//{

//    using var db = _connectionFactory.CreateConnection();

//    paymentDate ??= DateTimeOffset.Now;

//    var items = await db.QueryAsync<VSchedule>("spGetCustomerOnDateSchedules",
//        new
//        {
//            branchId,
//            paymentDate,
//            param?.PageIndex,
//            param?.PageSize,
//            param?.SortColumn,
//            param?.SortDescending,
//            param?.Filter
//        },
//       commandType: CommandType.StoredProcedure)
//        ;

//    var counts = items.Count();

//    return new Pager<VSchedule>(items.ToList(), counts);



//}

//private async Task<Pager<VSchedule>> GetUnPaidSchedules(int? branchId, DateTimeOffset? paymentDate = null, QueryParam? param = null)
//{

//    using var db = _connectionFactory.CreateConnection();

//    var items = await db.QueryAsync<VSchedule>("spGetCustomerUnpaidSchedules",
//        new
//        {
//            branchId,
//            paymentDate,
//            param?.PageIndex,
//            param?.PageSize,
//            param?.SortColumn,
//            param?.SortDescending,
//            param?.Filter
//        },
//       commandType: CommandType.StoredProcedure);

//    var counts = items.Count();

//    return new Pager<VSchedule>(items.ToList(), counts);



//}

//private async Task<Pager<VSchedule>> GetPaidSchedules(int? branchId, DateTimeOffset? paymentDate = null, QueryParam? param = null)
//{

//    using var db = _connectionFactory.CreateConnection();

//    paymentDate ??= DateTimeOffset.Now;

//    var items = await db.QueryAsync<VSchedule>("spGetCustomerPaidSchedules",
//        new
//        {
//            branchId,
//            paymentDate,
//            param?.PageIndex,
//            param?.PageSize,
//            param?.SortColumn,
//            param?.SortDescending,
//            param?.Filter
//        },
//       commandType: CommandType.StoredProcedure)
//        ;

//    var counts = items.Count();

//    return new Pager<VSchedule>(items.ToList(), counts);



//}

//private async Task<Pager<VSchedule>> GetPartialPaidSchedules(int? branchId, DateTimeOffset? paymentDate = null, QueryParam? param = null)
//{

//    using var db = _connectionFactory.CreateConnection();

//    var items = await db.QueryAsync<VSchedule>("spGetCustomerPartialPaidSchedules",
//        new
//        {
//            branchId,
//            paymentDate,
//            param?.PageIndex,
//            param?.PageSize,
//            param?.SortColumn,
//            param?.SortDescending,
//            param?.Filter
//        },
//       commandType: CommandType.StoredProcedure)
//        ;

//    var counts = items.Count();

//    return new Pager<VSchedule>(items.ToList(), counts);



//}


//public async Task<Pager<VSchedule>> GetLoanSchedules(int? branchId, ScheduleStatus schedule, DateTimeOffset? paymentDate = null, QueryParam? param = null)
//{

//    return schedule switch
//    {
//        ScheduleStatus.Paid => await GetPaidSchedules(branchId, paymentDate, param),
//        ScheduleStatus.Unpaid => await GetUnPaidSchedules(branchId, paymentDate, param),
//        ScheduleStatus.Partial => await GetPartialPaidSchedules(branchId, paymentDate, param),
//        ScheduleStatus.Late => await GetLateSchedules(branchId, paymentDate, param),
//        ScheduleStatus.OnDate => await GetOnDateSchedules(branchId, paymentDate, param),
//        _ => await GetUnPaidSchedules(branchId, paymentDate, param),
//    };
//}

//private async Task<Pager<VSchedule>> GetPaidSchedulesByIdAsync(long? loanId, int? branchId, QueryParam? param = null)
//{
//    //2 only paid 
//    using var db = _context.Database.GetDbConnection();
//    var query = await db.QueryAsync<VSchedule>("spGetVSchedulesByLoanId", new
//    {
//        branchId,
//        loanId,
//        @ScheduleStatusId = (int)ScheduleStatus.Paid,
//        param?.PageIndex,
//        param?.PageSize,
//        param?.Filter
//    }, commandType: CommandType.StoredProcedure);

//    var counts = query.Count();

//    var items = query.ToList();

//    return new Pager<VSchedule>(items, counts);

//}

//private async Task<Pager<VSchedule>> GetUnpaidSchedulesByIdAsync(long? loanId, int? branchId, QueryParam? param = null)
//{
//    //2 only paid 
//    using var db = _context.Database.GetDbConnection();
//    var query = await db.QueryAsync<VSchedule>("spGetUnPaidSchedulesById", new
//    {
//        branchId,
//        loanId,
//        statusId = (int)ScheduleStatus.Unpaid,
//        param?.PageIndex,
//        @param?.PageSize,
//        param?.Filter
//    }, commandType: CommandType.StoredProcedure);

//    var counts = query.Count();

//    var items = query.ToList();

//    return new Pager<VSchedule>(items, counts);

//}

//private async Task<Pager<VSchedule>> GetAllSchedulesByIdAsync(long? loanId, int? branchId, QueryParam? param = null)
//{
//    //2 only paid 
//    using var db = _context.Database.GetDbConnection();
//    var query = await db.QueryAsync<VSchedule>("spGetAllSchedulesById", new
//    {
//        branchId,
//        loanId,
//        statusId = (int)ScheduleStatus.Unpaid,
//        param?.PageIndex,
//        @param?.PageSize,
//        param?.Filter
//    }, commandType: CommandType.StoredProcedure);

//    var counts = query.Count();

//    var items = query.ToList();

//    return new Pager<VSchedule>(items, counts);

//}

//public async Task<Pager<VLoans>> GetLoansAsync(long creditAgentId, int branchId, QueryParam param)
//{

//    IQueryable<VLoans> queries = _context.Database.SqlQuery<VLoans>(@$"SELECT * FROM VLoanDisbursments d WHERE d.BranchId={branchId}");

//    if (!queries.Any())
//    {
//        return new Pager<VLoans>();
//    }

//    if (!string.IsNullOrEmpty(param.Filter))
//    {
//        queries = queries.Where(p => EF.Functions.Like(p.FirstName, $"%{param.Filter}%") ||
//                                 EF.Functions.Like(p.LastName, $"%{param.Filter}%") ||
//                                 EF.Functions.Like(p.AltName, $"%{param.Filter}%") ||
//                                 EF.Functions.Like(p.AgentName, $"%{param.Filter}%") ||
//                                 EF.Functions.Like(p.Phone, $"%{param.Filter}%"));
//    }

//    if (creditAgentId > 0)
//    {
//        queries = queries.Where(x => x.CreditAgentRefId == creditAgentId && x.BranchId == branchId);
//    }

//    if (!param.IsIncludeInActive)
//    {
//        queries = queries.Where(p => p.LoanstatusId == 4 && p.BranchId == branchId);
//    }

//    if (param.SortDescending)
//    {
//        queries = queries.OrderByDescending(p => EF.Property<object>(p, param.SortColumn));
//    }
//    var counts = await queries.CountAsync();

//    var items = await queries.Skip(param.PageIndex)
//                                .Take(param.PageSize)
//                                .AsNoTracking()
//                                .ToListAsync();
//    return new Pager<VLoans>(items, counts);
//}
//public async Task<Result<Loan>> GetLoanByIdAsync(long loanId, int branchId, params string[] includes)
//{
//    IQueryable<Loan> queries = _context.LoanDisbursments
//                                        .Where(x => x.Id == loanId &&
//                                                    x.BranchRefId == branchId);

//    if (includes.Any())
//    {

//        foreach (var prop in includes)
//        {
//            queries = queries.Include(prop);
//        }
//    }

//    if (!queries.Any())
//    {

//        return Result.Failure<Loan>(Error.NotFound("404", ["No data found"]));

//    }

//    var query = await queries.FirstOrDefaultAsync();

//    return Result.Success(query!);

//}
//public async Task<Result<VLoans>> GetLoanDetailByIdAsync(long loanId, int branchId, bool related = false)
//{
//    if (related)
//    {
//        return await GetLoanDetailWithRelation(loanId, branchId);
//    }

//    return await GetOnlyLoanInfo(loanId, branchId);
//}


////using var db = _context.Database.GetDbConnection();
//var query = await db.QueryAsync<VSchedule>("spGetLoanSchedulesById", new
//{
//    branchId,
//    loanId,
//    statusId = (int)status,
//    param?.PageIndex,
//    @param?.PageSize,
//    param?.Filter
//}, commandType: CommandType.StoredProcedure);

//var counts = query.Count();

//var items = query.ToList();

//return new Pager<VSchedule>(items, counts);
//public async Task<Result<IEnumerable<VSchedule>>> GetCustomerPaymentSchedules(long? loanId, int? branchId, ScheduleStatus status, DateTime? selectedDate, bool onScheduleOnly = false, string? filter = null)
//{
//    //0 all
//    //1 not paid
//    //2 only paid
//    //3 only partial
//    var queries = _context.Database
//                        .SqlQuery<VSchedule>($"SELECT * FROM VSchedules WHERE BranchId={branchId}");

//    if (loanId > 0)
//    {
//        queries = queries.Where(x => x.LoanId == loanId && x.BranchId == branchId);
//    }

//    if (status == ScheduleStatus.Unpaid)
//    {
//        int?[] states = [2, 4];
//        queries = queries.Where(x => states.Contains(x.ScheduleStatusId) && x.BranchId == branchId);
//    }

//    if (status == ScheduleStatus.Paid)
//    {
//        queries = queries.Where(x => x.ScheduleStatusId == (int)ScheduleStatus.Paid && x.BranchId == branchId);
//    }

//    if (selectedDate is not null && onScheduleOnly)
//    {
//        queries = queries.Where(x => x.PaymentDate == selectedDate && x.BranchId == branchId);
//    }

//    if (selectedDate is not null && !onScheduleOnly)
//    {
//        queries = queries.Where(x => x.PaymentDate <= selectedDate && x.BranchId == branchId);
//    }

//    if (!string.IsNullOrWhiteSpace(filter))
//    {
//        queries = queries.Where(x => EF.Functions.Like(x.AltName!, $"%{filter}%") ||
//                                     EF.Functions.Like(x.FullName!, $"%{filter}%") ||
//                                     EF.Functions.Like(x.BranchId!.ToString(), $"%{filter}%") ||
//                                     EF.Functions.Like(x.CreditAgentNameKh!, $"%{filter}%") ||
//                                     EF.Functions.Like(x.CreditAgentNameEn!, $"%{filter}%"));
//    }



//    if (!queries.Any())
//    {
//        return Result.Failure<IEnumerable<VSchedule>>(Error.NotFound("404", ["No Schedule Found."]));
//    }
//    var items = await queries.AsNoTracking().ToListAsync();
//    return Result.Success<IEnumerable<VSchedule>>(items);
//}



//var items = await _context.Database
//                    .SqlQuery<VSchedule>($"EXEC  {},{loanId},{status}")
//                    .AsNoTracking()
//                    .ToListAsync();

//public async Task<Result<IEnumerable<VSchedule>>> GetTobePaidSchedules(DateTime paymentDate, int branchId, string? filter = null)
//{
//    IQueryable<VSchedule> queries = _context.Database.SqlQuery<VSchedule>($"SELECT * FROM [dbo].[fGetPaymentSchedules]({paymentDate},{branchId})")
//                                          .OrderByDescending(x => x.PaymentDate)
//                                           .ThenByDescending(x => x.FullName);
//    if (!queries.Any())
//    {
//        return Result.Failure<IEnumerable<VSchedule>>(Error.NotFound("404", ["No Payment schedule found."]));
//    }

//    if (!string.IsNullOrWhiteSpace(filter))
//    {
//        queries = queries.Where(p => EF.Functions.Like(p.FullName!, $"%{filter}%") || EF.Functions.Like(p.AltName!, $"%{filter}%"));
//    }


//    var schedules = await queries.AsNoTracking().ToListAsync();


//    return Result.Success<IEnumerable<VSchedule>>(schedules);
//}

//public async Task<Result<IEnumerable<VSchedule>>> GetCustomerLatePaymentScheduleDetails(DateTime paymentDate, int branchId, long loanId)
//{
//    using var db = _context.Database.GetDbConnection();
//    var schedules = await db.QueryAsync<VSchedule>("spGetCustomerLatePaymentScheduleDetails", new
//    {
//        paymentDate,
//        branchId,
//        loanId
//    }, commandType: CommandType.StoredProcedure);
//    if (!schedules.Any())
//    {
//        return Result.Failure<IEnumerable<VSchedule>>(Error.NotFound("404", ["No Payment schedule found."]));
//    }
//    return Result.Success<IEnumerable<VSchedule>>(schedules);
//}

//public async Task<Result<IEnumerable<VSchedule>>> GetCustomerPaymentSchedules(long? loanId, int? branchId, int? status, DateTime? selectedDate, bool onScheduleOnly = false)
//{
//    //0 all
//    //1 not paid
//    //2 only paid
//    //3 only partial
//    var queries = _context.Database
//                        .SqlQuery<VSchedule>($"SELECT * FROM VSchedules WHERE BranchId={branchId} AND LoanId={loanId}");

//    if (status == 1)
//    {
//        int?[] states = [2, 4];
//        queries = queries.Where(x => states.Contains(x.ScheduleStatusId));
//    }

//    if (status == 2)
//    {
//        queries = queries.Where(x => x.ScheduleStatusId == 1);
//    }

//    if (selectedDate is not null && onScheduleOnly)
//    {
//        queries = queries.Where(x => x.PaymentDate == selectedDate);
//    }

//    if (selectedDate is not null && !onScheduleOnly)
//    {
//        queries = queries.Where(x => x.PaymentDate <= selectedDate);
//    }

//    if (!queries.Any())
//    {
//        return Result.Failure<IEnumerable<VSchedule>>(Error.NotFound("404", ["No Schedule Found."]));
//    }
//    var items = await queries.AsNoTracking().ToListAsync();
//    return Result.Success<IEnumerable<VSchedule>>(items);
//}

//public async Task<Result<IEnumerable<VLoanRepaymentLine>>> GetCustomerPayementScheduleLines(long loanId, int installmentNo)
//{
//    using var db = _context.Database.GetDbConnection();
//    var scheduleLine = await db.QueryAsync<VLoanRepaymentLine>("spGetCustomerRepayementScheduleLines", new
//    {
//        @loanId = loanId,
//        @installmentNo = installmentNo
//    }, commandType: CommandType.StoredProcedure);
//    if (!scheduleLine.Any())
//    {
//        return Result.Failure<IEnumerable<VLoanRepaymentLine>>(Error.NotFound("404", ["No Payment schedule found."]));
//    }
//    return Result.Success<IEnumerable<VLoanRepaymentLine>>(scheduleLine);
//}
//public async Task<Result<IEnumerable<VSchedule>>> GetPaymentSchedulesByLoanId(long? loanId, int? branchId, DateTime? paymentDate, ScheduleStatus status = ScheduleStatus.All)
//{
//    //0 all
//    //1 not paid
//    //2 only paid
//    //3 only partial 
//    var items = await _context.Database
//                        .SqlQuery<VSchedule>($"EXEC spGetSchedulesById {branchId},{loanId},{paymentDate},{status}")
//                        .AsNoTracking()
//                        .ToListAsync();

//    if (!items.Any())
//    {
//        return Result.Failure<IEnumerable<VSchedule>>(Error.NotFound("404", ["No Schedule Found."]));
//    }
//    return Result.Success<IEnumerable<VSchedule>>(items);
//}


//public async Task<Result<VSchedule>> GetCustomerPaymentScheduleById(long loanId, int branchId, int installmentNo)
//{

//    using var db = _context.Database.GetDbConnection();

//    var schedule = await db.QueryFirstAsync<VSchedule>("spGetCustomerPayementSchedule",
//        new
//        {
//            @loanId = loanId,
//            @branchId = branchId,
//            @installmentNo = installmentNo
//        }, commandType: CommandType.StoredProcedure);

//    if (schedule is null)
//    {
//        return Result.Failure<VSchedule>(Error.NotFound("404", ["No Schedule Found."]));
//    }
//    return Result.Success<VSchedule>(schedule);
//}

//public async Task<Result<Pager<VLoans>>> GetLoansListAsync(int branchId, long creditAgentId, QueryParam param)
//{
//    try
//    {
//        _logger.LogInformation("Get Loan by credit agent");

//        IQueryable<VLoans> queries = _context.Database.SqlQuery<VLoans>(@$"SELECT * FROM VLoanDisbursments d WHERE d.BranchId={branchId} AND d.CreditAgentRefId={creditAgentId}");

//        if (!queries.Any())
//        {
//            return Result.Failure<Pager<VLoans>>(Error.NotFound("404", ["no data found."]));
//        }
//        if (!param.IsIncludeInActive)
//        {
//            queries = queries.Where(p => p.LoanstatusId == 4);
//        }

//        param.SortColumn = param.SortColumn!.Length == 0 ? "RegisterDate" : param.SortColumn;

//        if (param.SortDescending)
//        {
//            queries = queries.OrderByDescending(p => EF.Property<object>(p, param.SortColumn));
//        }
//        var counts = await queries.CountAsync();
//        var items = await queries.Skip(param.PageIndex)
//                                    .Take(param.PageSize)
//                                    .AsNoTracking()
//                                    .ToListAsync();
//        return Result.Success<Pager<VLoans>>(new(items, counts));
//    }
//    catch (Exception ex)
//    {
//        _logger.LogError(ex, ex.Message);

//        return Result.Failure<Pager<VLoans>>(Error.NotFound("500", [$"{ex.Message}"]));
//    }
//}



//public async Task<Result<IEnumerable<VSchedulePaid>>> GetCustomerAlreadyPaidSchedules(long loanRefId, int branchId)
//{

//    using var db = _context.Database.GetDbConnection();

//    var paidQueries = await db.QueryAsync<VSchedulePaid>("[spGetCustomerAlreadyPaidSchedules]",
//        new
//        {
//            @loanId = loanRefId,
//            branchId = branchId
//        }, commandType: CommandType.StoredProcedure);
//    if (!paidQueries.Any())
//    {
//        return Result.Failure<IEnumerable<VSchedulePaid>>(Error.NotFound("404", ["No Schedule Found."]));
//    }

//    return Result.Success<IEnumerable<VSchedulePaid>>(paidQueries);
//}


//public async Task<Result<IEnumerable<VSchedule>>> GetCustomerPaymentSchedules(long loanId, int branchId, int status)
//{
//    //0 all
//    //1 not paid
//    //2 only paid
//    var items = await _context.Database
//                            .SqlQuery<VSchedule>($"EXEC spGetCustomerPaymentSchedulesByLoan {branchId},{loanId},{status}")
//                            .AsNoTracking()
//                            .ToListAsync();

//    if (!items.Any())
//    {
//        return Result.Failure<IEnumerable<VSchedule>>(Error.NotFound("404", ["No Schedule Found."]));
//    }

//    return Result.Success<IEnumerable<VSchedule>>(items);
//}


//var guarantorIds = model.LoanGuarantors?.Select(x => x.EntId).ToList();

//var collateralLineIds = model.LoanCollateralLines?.Select(x => x.CollateralRefId).ToList();

// var guarantos = _context.OtherNames.Where(x => guarantorIds!.Contains(x.EntId)).ToList();

//var collateralLines = _context.CollateralLines.Where(x => collateralLineIds!.Contains(x.Id)).ToList();

//req!.Guarantors = guarantos;

//req!.CollateralLines = collateralLines;

//private async Task<bool> IsAddedLoanTransaction(Loan model)
//{
//    var tranx = new LoanTransaction
//    {
//        UserRefID = model.UserRefId,
//        TransactionTypeName = "Loan Disbursment",
//        TranTypeRefId = 19,
//        IsActive = true
//    };

//    var totalPrinciplePayment = model.Principle - model.AdminFeeAmount;

//    var cashMgt = new CashManagement
//    {
//        BankRefId = 11,
//        BranchRefId = model.BranchRefId,
//        Amount = totalPrinciplePayment,
//        CurrencyRefId = model.CurrencyRefId,
//        CashStatusRefId = 2,
//        Remark = "Cash goes through transaction",
//        StaffRefId = model.UserRefId,
//        Receive = "N",
//        Reject = "N",
//        TransactionDate = model.RegisterDate,
//        BusinessDate = model.RegisterDate,
//        Balance = 0
//    };

//    tranx.CashManagements!.Add(cashMgt);

//    _context.LoanTransaction.Add(tranx);

//    return await _context.SaveChangesAsync() > 0;
//}

//private async Task<bool> IsAddedLoan(Loan model)
//{
//    //var collateralLineIds = model.CollateralLines!.Select(x => x.Id).ToList();
//    //var collateralLines = _context.CollateralLines
//    //                            .Where(c => collateralLineIds.Contains(c.Id))
//    //                            .ToList();
//    //var guarantors = model.Guarantors!;
//    //var schedules = model.LoanSchedules;
//    //var disbur = ToDisbursment(model);
//    //disbur.CollateralLines!.AddRange(collateralLines);
//    //disbur.Guarantors!.AddRange(guarantors);
//    //disbur.LoanSchedules!.AddRange(schedules!);
//    _context.LoanDisbursments.Add(model);
//    return await _context.SaveChangesAsync() > 0;
//}
//private async Task<bool> IsLoanActive(long? customerId, bool isAactive)
//{
//    var updatedRow = await _context.Customers.Where(x => x.CustomerId == customerId)
//                                        .ExecuteUpdateAsync(p => p.SetProperty(x => x.IsLoanActive, true));

//    return updatedRow > 0;
//}


//public async Task<Result<IEnumerable<VSchedule>>> GetCustomerPaymentSchedules(long loanRefId, int branchId, bool includePaid = false)
//{

//    using var db = _context.Database.GetDbConnection();

//    var paymentSchedules = await db.QueryAsync<VSchedule>("[spGetCustomerPaymentSchedulesByLoan]",
//        new
//        {

//        }, commandType: CommandType.StoredProcedure);



//    IQueryable<VSchedule> queries = _context.Database.SqlQuery<VSchedule>($"SELECT * FROM [dbo].[fGetPaymentSchedulesByLoan]({loanRefId},{branchId})")
//                                        .OrderBy(x => x.PaymentDate)
//                                            .ThenBy(x => x.InstallmentNo);

//    if (!includePaid)
//    {
//        queries = queries.Where(x => x.ScheduleStatusId == 2 || x.ScheduleStatusId == 4);
//    }

//    if (!queries.Any())
//    {
//        return Result.Failure<IEnumerable<VSchedule>>(Error.NotFound("404", ["No Schedule Found."]));
//    }

//    var schedule = await queries.AsNoTracking().ToListAsync();

//    return Result.Success<IEnumerable<VSchedule>>(schedule);
//}




//private static Loan ToDisbursment(Loan model)
//{
//    return new Loan
//    {
//        LoanStatusRefId = model.LoanStatusRefId,
//        RepaymentModeRefId = model.RepaymentModeRefId,
//        LoanSubTypeRefId = model.LoanSubTypeRefId,
//        LoanCategoryRefId = model.LoanCategoryRefId,
//        BankRefId = model.BankRefId,
//        UserRefId = model.UserRefId,
//        LoanProductRefId = model.LoanProductRefId,
//        LoanAccountCode = model.LoanAccountCode,
//        Principle = model.Principle,
//        Period = model.Period,
//        PeriodName = model.PeriodName,
//        Installments = model.Installments,
//        CurrencyRefId = model.CurrencyRefId,
//        InterestRate = model.InterestRate,
//        StartDate = model.StartDate,
//        FirstPaymentDate = model.FirstPaymentDate,
//        LastDate = model.LastDate,
//        TermTypeId = model.TermTypeId,
//        InstallmentTypeId = model.InstallmentTypeId,
//        CreditAgentRefId = model.CreditAgentRefId,
//        CustomerRefId = model.CustomerRefId,
//        RegisterDate = model.RegisterDate,
//        PayEvery = model.PayEvery,
//        AdminFee = model.AdminFee,
//        AdminFeeAmount = model.AdminFeeAmount,
//        InstallmentType = model.InstallmentType,
//        CollectionFeeRate = model.CollectionFeeRate,
//        Remark = model.Remark,
//        BranchRefId = model.BranchRefId,
//        BranchName = model.BranchName,
//        Circle = model.Circle,
//        Guarantors = new List<Guarantor>(),
//        CollateralLines = new List<CollateralLine>(),
//        LoanSchedules = new List<LoanSchedule>(),
//    };
//}
//public async Task<Result<Pager<VSchedule>>> GetPaymentSchedules(DateTime paymentDate, int branchId,
//     int pageIndex = 0,
//     int pageSize = 10,
//     string? filter = null)
//{
//    IQueryable<VSchedule> queries = _context.Database
//                                                .SqlQuery<VSchedule>($"SELECT * FROM fGetloanPayments({paymentDate},{branchId})")
//                                                .OrderByDescending(x => x.PaymentDate)
//                                                    .ThenByDescending(x => x.FullName);

//    if (!string.IsNullOrWhiteSpace(filter))
//    {
//        queries = queries.Where(p => EF.Functions.Like(p.FullName, $"%{filter}%") ||
//                                    EF.Functions.Like(p.AltName, $"%{filter}%"));
//    }

//    if (!queries.Any())
//    {
//        return Result.Failure<Pager<VSchedule>>(Error.NotFound("404", ["No Payment schedule found."]));
//    }

//    var counts = await queries.CountAsync();

//    var items = await queries
//                    .Skip(pageIndex)
//                    .Take(pageSize)
//                    .AsNoTracking()
//                    .ToListAsync();
//    return Result.Success<Pager<VSchedule>>(new(items, counts));
//}

//public async Task<MsgResponse> CreateRePayment(LoanRepayment loanRepayment, LoanTransaction transaction)
//{

//    try
//    {
//        var schedules = loanRepayment.LoanRepaymentLines;
//        _cxt.Database.BeginTransaction();
//        _cxt.LoanRepayments.Add(loanRepayment);
//        _cxt.LoanTransaction.Add(transaction);
//        await _cxt.SaveChangesAsync();
//        foreach (var item in schedules!)
//        {
//            var pp = _cxt.VSchedules.FromSql<VSchedule>($"SELECT * FROM VSchedules")
//                                   .Where(x => x.LoanScheduleId == item.LoanScheduleRefId)
//                                   .FirstOrDefault();

//            if (pp!.Principal == pp.PaidPrinciple)
//            {
//                _cxt.LoanSchedules.Where(x => x.LoanScheduleId == item.LoanScheduleRefId)
//                                   .ExecuteUpdate(x => x.SetProperty(p => p.ScheduleStatusRefId, 1));
//            }
//            if (pp!.Principal > pp.PaidPrinciple)
//            {
//                _cxt.LoanSchedules.Where(x => x.LoanScheduleId == item.LoanScheduleRefId)
//                                   .ExecuteUpdate(x => x.SetProperty(p => p.ScheduleStatusRefId, 4));
//            }
//        }
//        var loanInfo = _cxt.VLoanDisbursements.FirstOrDefault(x => x.Id == loanRepayment.LoanRefId);

//        if (loanInfo is not null && loanInfo.OutStanding == 0)
//        {

//            _cxt.LoanDisbursments.Where(x => x.Id == loanRepayment.LoanRefId)
//                                    .ExecuteUpdate(p => p.SetProperty(x => x.LoanStatusRefId, 2));

//        }
//        _cxt.Database.CommitTransaction();

//        return new MsgResponse
//        {
//            IsSuccess = true,
//            Message = "Success"
//        };

//    }
//    catch (Exception ex)
//    {

//        _cxt.Database.RollbackTransaction();

//        return new MsgResponse
//        {
//            IsSuccess = false,
//            Message = ex.Message,
//        };
//    }
//public async Task<Pager<VLoans>> GetLoansAsync(int branchId, int pageIndex, int pageSize, string sortColumn, bool sortDescending = true, bool includeInactive = false, string? q = null)
//{
//    IQueryable<VLoans> queries = _context.VLoanDisbursements
//                                        .Where(x => x.BranchId == branchId);

//    if (includeInactive)
//    {
//        queries = queries.IgnoreQueryFilters();
//    }

//    if (!string.IsNullOrWhiteSpace(q))
//    {
//        queries = queries.Where(p => EF.Functions.Like(p.AltName, $"%{q}%") ||
//                                     EF.Functions.Like(p.FirstName, $"%{q}%") ||
//                                     EF.Functions.Like(p.LastName, $"%{q}%") ||
//                                     EF.Functions.Like(p.Phone, $"%{q}%"));
//    }

//    if (sortDescending)
//    {
//        queries = queries.OrderByDescending(x => EF.Property<object>(x, sortColumn));
//    }

//    var counts = await queries.CountAsync();

//    var items = await queries.Skip(pageIndex).Take(pageSize).ToListAsync();

//    return new Pager<VLoans>(items, counts);
//}

//}


