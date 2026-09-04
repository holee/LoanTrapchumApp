using Applications.Enumerations;
using Applications.Interfaces.Loans;
using Applications.Responses.Message;
using Applications.Responses.ViewModels.Loans.Schedules;
using Dapper;
using Domains.Models.Transaction;
using Infrastructures.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Domains.Models.Lists;
using Applications.Commons.Messages.LoanPayments;
using Applications.Features.Schedules.Dto;
using ErrorOr;
using Applications.Features.LoanPayments.Dtos;

namespace Infrastructures.Implements;

public class LoanPaymentRepository(IDbContextFactory<DataContext> dbContextFactory, IDapperConnectionFactory connectionFactory) : ILoanPaymentService
{


    public async Task<ErrorOr<string>> RecievePayment(LoanTransaction req, CancellationToken cancellation = default)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync(cancellation);

        if (await IsOfficialInused(req, db, cancellation))
        {
            return Error.Conflict("409", "The official receipt number is already in use.");
        }

        var strategy = db.Database.CreateExecutionStrategy();

        return await strategy.ExecuteAsync(async () =>
        {
            using var tranx = await db.Database.BeginTransactionAsync(cancellation);

            try
            {
                req.LoanPayment?.OfficialReceiptNo = $"{DateTime.Now.Year}-{req.LoanPayment.OfficialReceiptNo}";

                var payment = db.LoanTransaction.Add(req).Entity;

                await db.SaveChangesAsync(cancellation);

                var schedules = payment.LoanPayment?.LoanRepaymentLines ?? [];

                if (schedules is not null && schedules.Any())
                {

                    var paidSchedules = schedules.FirstOrDefault();

                    await SetLoanScheduleComplete(db, paidSchedules?.BranchRefId, paidSchedules?.LoanRefId, paidSchedules?.LoanScheduleRefId, paidSchedules?.InstallmentNo);
                }


                await CloseLoanSchedule(db, req.LoanPayment?.BranchRefId, req.LoanPayment?.LoanRefId);

                await tranx.CommitAsync(cancellation);

                return LoanPaymentMessage.LOAN_PAYMENT_RECEIVED;
            }
            catch (Exception ex)
            {
                await tranx.RollbackAsync(cancellation);

                return (ErrorOr<string>)Error.Unexpected($"{ex.Message}");
            }
        });
    }

    public async Task<ErrorOr<PayOffResult>> PayOff(LoanTransaction trx, CancellationToken cancellation = default)
    {
        if (trx.LoanPayment is null)
        {
            return Error.Validation(description: "Loan payment information is required.");
        }

        await using var context = await dbContextFactory.CreateDbContextAsync(cancellation);

        if (await IsOfficialInused(trx, context, cancellation))
        {
            return Error.Conflict("409", "The official receipt number is already in use.");
        }

        var lastSchedule = await context.VSchedules
            .AsNoTracking()
            .Where(x => x.BranchId == trx.LoanPayment.BranchRefId &&
                        x.LoanId == trx.LoanPayment.LoanRefId &&
                        x.ScheduleStatusId != (int)ScheduleStatus.Paid)
            .OrderBy(x => x.InstallmentNo)
            .FirstOrDefaultAsync(cancellation);

        var strategy = context.Database.CreateExecutionStrategy();

        return await strategy.ExecuteAsync(async () =>
        {
            await using var tranx = await context.Database.BeginTransactionAsync(cancellation);

            try
            {
                context.LoanTransaction.Add(trx);

                await CloseLoan(context, trx.LoanPayment.BranchRefId, trx.LoanPayment.LoanRefId);

                await context.SaveChangesAsync(cancellation);

                await tranx.CommitAsync(cancellation);

                return new PayOffResult
                {
                    BranchId = trx.LoanPayment.BranchRefId,
                    LoanId = trx.LoanPayment.LoanRefId,
                    Principle = lastSchedule?.LeftPrinciple,
                    Interest = lastSchedule?.LeftInterestAmount,
                    CollectionFee = lastSchedule?.LeftCollectionFeeAmount,
                    Total = lastSchedule?.LeftAmount
                };
            }
            catch (Exception ex)
            {
                await tranx.RollbackAsync(cancellation);

                return (ErrorOr<PayOffResult>)Error.Unexpected(description: ex.Message);
            }
        });
    }

    public async Task<ErrorOr<IEnumerable<PaymentLineDetailsDto>>> GetPaymentLinesAsync(int? branchId, string? loanId, int? installmentNo)
    {

        await using var context = dbContextFactory.CreateDbContext();

        using var db = context.Database.GetDbConnection();

        var repaymentLines = await db.QueryAsync<PaymentLineDetailsDto>("spGetPayementScheduleLines", new
        {
            branchId,
            loanId,
            installmentNo
        }, commandType: CommandType.StoredProcedure);

        if (!repaymentLines.Any())
        {
            return Error.NotFound("No Schedule Lines found");
        }

        return repaymentLines.ToList();
    }


    public async Task<ErrorOr<IEnumerable<VSchedule>>> GetPaidScheduleAsync(int? branchId, string? loanRefId, CancellationToken cancellation = default)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync(cancellation);

        var queries = await db.VSchedules
            .Where(x => x.BranchId == branchId && x.LoanId == loanRefId &&
                        x.ScheduleStatusId == (int)ScheduleStatus.Paid)
            .AsNoTracking()
            .Select(x => new VSchedule
            {
                LoanScheduleId = x.LoanScheduleId,
                InstallmentNo = x.InstallmentNo,
                PaymentDate = x.PaymentDate,
                Principle = x.Principle,
                Interest = x.Interest,
                CollectionFee = x.CollectionFee,
                TotalPay = x.TotalPay,
            }).ToListAsync(cancellation);

        return queries;
    }

    public async Task<ErrorOr<ScheduleDetailsDto>> GetScheduleByIdAsync(long? loanScheduleId, int? branchId, string? loanId)
    {
        using var db = await connectionFactory.CreateConnectionAsync();

        string sql = @"
                SELECT 
                s.LoanScheduleId,
                s.Principle,
                s.Interest,
                s.CollectionFee,
                s.PaymentDate,
                (
                    SELECT 
                        SUM(s.SubTotal) FROM VLoanPaymentLines s
                    WHERE  s.LoanRefId = @LoanId AND 
                           s.BranchRefId = @BranchId AND
                           s.LoanScheduleRefId=@LoanScheduleId
                ) AS Total,
                    s1.Id,
                    s1.PaidPrinciple,
                    s1.PaidInterest,
                    s1.PaidCollectionFee,
                    s1.PenaltyAmount,
                    s1.SubTotal,
                    CONVERT(DATE, s1.Created) AS Created
            FROM VSchedules s 
            LEFT JOIN VLoanPaymentLines s1 
                ON s.LoanId = s1.LoanRefId 
                AND s.LoanScheduleId = s1.LoanScheduleRefId 
                AND s.InstallmentNo = s1.InstallmentNo
            WHERE 
                s.LoanScheduleId = @LoanScheduleId 
                AND s.LoanId = @LoanId 
                AND s.BranchId = @BranchId 
                AND s.ScheduleStatusId IN (1);";

        var lookup = new Dictionary<long, ScheduleDetailsDto>();

        await db.QueryAsync<ScheduleDetailsDto, PaymentLineDetailsDto, ScheduleDetailsDto>(sql,
            (schedule, paymentLine) =>
            {
                if (!lookup.TryGetValue(schedule.LoanScheduleId, out var existing))
                {
                    existing = schedule;

                    lookup.Add(schedule.LoanScheduleId, existing);
                }

                if (paymentLine != null && paymentLine.Id > 0)
                {
                    existing.PaymentLines.Add(paymentLine);
                }

                return existing;
            },
            new
            {
                LoanScheduleId = loanScheduleId,
                LoanId = loanId,
                BranchId = branchId
            },
            splitOn: "Id");


        var schedule = lookup.Values.FirstOrDefault();

        if (schedule is not null)
        {
            return schedule;
        }

        return Error.NotFound(Messages.DataNotFound);
    }


    public async Task<ErrorOr<Reciept>> GenerateRecieptNumber(CancellationToken cancellation = default)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync(cancellation);

        var reciept = new Reciept
        {
            RecieptNumber = await GetMaxRecieptNumber(context)
        };

        context.Reciepts.Add(reciept);

        await context.SaveChangesAsync(cancellation);

        return reciept;
    }


    private static decimal CalculatePenalty(decimal overdueAmount, int overdueDays)
    {
        const decimal penaltyRatePerDay = 0.001m; // 0.1%

        return overdueAmount * penaltyRatePerDay * overdueDays;
    }

    private static decimal CalculateInterest(decimal balance, decimal ratePerDay, DateTime fromDate, DateTime toDate)
    {
        var days = (toDate - fromDate).Days;

        if (days < 0)
            throw new ArgumentException("Invalid payoff date");

        return balance * ratePerDay * days;
    }

    private async Task SetLoanScheduleComplete(int? branchId, string? loanId, long? scheduleId, int? installmentId)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var leftAmount = await db.VSchedules
            .Where(x => x.LoanScheduleId == scheduleId &&
                        x.LoanId == loanId &&
                        x.BranchId == branchId)
            .Select(x => x.LeftAmount)
            .FirstOrDefaultAsync();

        var statusId = leftAmount == 0 ? (int)ScheduleStatus.Paid : (int)ScheduleStatus.Partial;

        db.LoanSchedules
            .Where(x => x.LoanScheduleId == scheduleId &&
                        x.LoanRefId == loanId &&
                        x.BranchId == branchId &&
                        x.InstallmentNo == installmentId)
            .ExecuteUpdate(x => x.SetProperty(p => p.ScheduleStatusRefId, statusId));
    }

    private async Task SetLoanScheduleComplete(DataContext db, int? branchId, string? loanId, long? scheduleId, int? installmentId)
    {
        if (branchId == null || loanId == null || scheduleId == null || installmentId == null)
        {
            throw new ArgumentException("Invalid schedule parameters.");
        }

        // Use parameterized query to get LeftAmount
        var leftAmount = await db.VSchedules
            .Where(x => x.LoanScheduleId == scheduleId &&
                        x.LoanId == loanId &&
                        x.BranchId == branchId)
            .Select(x => x.LeftAmount)
            .FirstOrDefaultAsync();

        var statusId = leftAmount == 0 ? ScheduleStatus.Paid : ScheduleStatus.Partial;

        // Update schedule status
        await db.LoanSchedules
            .Where(x => x.LoanScheduleId == scheduleId &&
                        x.LoanRefId == loanId &&
                        x.BranchId == branchId &&
                        x.InstallmentNo == installmentId)
            .ExecuteUpdateAsync(x => x.SetProperty(p => p.ScheduleStatusRefId, (int)statusId));
    }

    private async Task CloseLoanSchedule(DataContext db, int? branchId, string? loanId)
    {
        if (!branchId.HasValue && !string.IsNullOrEmpty(loanId))
        {
            return; // Skip if parameters are invalid
        }

        await IsLoanFullyPaidAsync(branchId, loanId);

        var existingLoans = await db.VLoanDisbursements
            .AnyAsync(x => x.LoanId == loanId &&
                           x.BranchId == branchId &&
                           x.OutStanding == 0);
        if (existingLoans)
        {
            await db.LoanDisbursments
                .Where(x => x.LoanId == loanId & x.BranchRefId == branchId)
                .ExecuteUpdateAsync(p => p.SetProperty(x => x.LoanStatusRefId, (int)LoanStatuses.Close)
                    .SetProperty(x => x.ModifiedDt, DateTime.Now));
        }
    }

    async Task CloseLoanSchedule(int? branchId, string? loanId)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();

        var existingLoans = await db.VLoanDisbursements
            .AnyAsync(x => x.LoanId == loanId &&
                           x.BranchId == branchId &&
                           x.OutStanding == 0);
        if (existingLoans)
        {
            db.LoanDisbursments
                .Where(x => x.LoanId == loanId & x.BranchRefId == branchId)
                .ExecuteUpdate(p => p.SetProperty(x => x.LoanStatusRefId, (int)LoanStatuses.Close));
        }
    }

    async Task CloseLoan(DataContext db, int? branchId, string? loanId, CancellationToken cancellation = default)
    {

        var existingLoans = await db.LoanDisbursments
                            .FirstOrDefaultAsync(x => x.LoanId == loanId &&
                                   x.BranchRefId == branchId &&
                                   x.LoanStatusRefId != (int)LoanStatuses.Close, cancellation);

        if (existingLoans != null)
        {
            existingLoans.LoanStatusRefId = (int)LoanStatuses.Close;

            var customer = await db.EntityNames
                                    .Include(x => x.Customer)
                                    .FirstOrDefaultAsync(x => x.Id == existingLoans.CustomerRefId, cancellation);

            if (customer is not null)
            {
                customer.Customer?.IsLoanActive = false;
            }
        }
    }

    private static async Task<bool> IsOfficialInused(LoanTransaction req, DataContext db, CancellationToken cancellation)
    {

        var existingReciept = await db.LoanRepayments.AnyAsync(x => x.OfficialReceiptNo == req.LoanPayment!.OfficialReceiptNo, cancellation);

        return existingReciept;
    }

    private async Task<bool> IsLoanFullyPaidAsync(int? branchId, string? loanId, CancellationToken cancellation = default)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync(cancellation);

        var totalCounts = await db.LoanSchedules
            .CountAsync(x => x.BranchId == branchId && x.LoanRefId == loanId, cancellation);

        var InstallmentNo = await db.LoanSchedules
            .Where(x => x.BranchId == branchId &&
                        x.LoanRefId == loanId &&
                        x.ScheduleStatusRefId == (int)ScheduleStatus.Paid)
            .OrderByDescending(x => x.InstallmentNo)
            .AsNoTracking()
            .Select(x => x.InstallmentNo)
            .FirstOrDefaultAsync(cancellation);

        return InstallmentNo != null && totalCounts == InstallmentNo;
    }

    private async Task<string> GetMaxRecieptNumber(DataContext context)
    {
        // Get max as int instead of string comparison
        var maxSequence = await context.Reciepts
            .AsNoTracking()
            .Select(x => x.RecieptNumber)
            .MaxAsync();
        var nextNumber = string.IsNullOrEmpty(maxSequence) ?
                                                            1 :
                                                            Convert.ToInt32(maxSequence) + 1;

        return nextNumber.ToString("D6"); // Always 6 digits padded with zeros
    }

}


//public async Task<Result<string>> CreatePayment(LoanTransaction req, CancellationToken cancellation = default)
//{
//    await using var db = await dbContextFactory.CreateDbContextAsync(cancellation);

//    await using var tranx = await db.Database.BeginTransactionAsync(cancellation);

//    try
//    {
//        var payment = db.LoanTransaction.Add(req).Entity;

//        await db.SaveChangesAsync(cancellation);

//        var schedules = payment.LoanPayment?.LoanRepaymentLines ?? [];

//        if (schedules.Any())
//        {
//            var scheduleIds = schedules.Select(x => x.LoanScheduleRefId).ToList();
//            var installmentNos = schedules.Select(x => x.InstallmentNo).ToList();

//            // Fetch related VSchedules in one go
//            var scheduleStatusMap = await db.VSchedules
//                .Where(x =>
//                    x.BranchId == req.LoanPayment!.BranchRefId &&
//                    x.LoanId == req.LoanPayment.LoanRefId &&
//                    scheduleIds.Contains(x.LoanScheduleId ?? 0))
//                .Select(x => new
//                {
//                    x.LoanScheduleId,
//                    x.InstallmentNo,
//                    Status = x.LeftAmount == 0 ? ScheduleStatus.Paid : ScheduleStatus.Partial
//                })
//                .ToListAsync(cancellation);

//            // Bulk update LoanSchedules
//            foreach (var group in scheduleStatusMap.GroupBy(x => x.Status))
//            {
//                var scheduleIdList = group.Select(x => x.LoanScheduleId).ToList();
//                var installmentList = group.Select(x => x.InstallmentNo).ToList();

//                await db.LoanSchedules
//                    .Where(x =>
//                        x.LoanRefId == req.LoanPayment!.LoanRefId &&
//                        x.BranchId == req.LoanPayment.BranchRefId &&
//                        scheduleIdList.Contains(x.LoanScheduleId) &&
//                        installmentList.Contains(x.InstallmentNo))
//                    .ExecuteUpdateAsync(x => x.SetProperty(p => p.ScheduleStatusRefId, (int)group.Key));
//            }
//        }

//        await CloseLoanSchedule(db, req.LoanPayment?.BranchRefId, req.LoanPayment?.LoanRefId);

//        await tranx.CommitAsync(cancellation);

//        return Result.Success<string>("Payment successfully.");
//    }
//    catch (Exception ex)
//    {
//        await tranx.RollbackAsync(cancellation);
//        return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));
//    }
//}

//public async Task<Result<string>> RecievePayment(LoanPaymentDtoCreate req)
//{
//    await using var db = await dbContextFactory.CreateDbContextAsync();

//    using var tranx = db.StartTransaction();

//    try
//    {
//        var loanRepayment = req.Adapt<LoanPayment>();

//        var payment = db.LoanRepayments.Add(loanRepayment).Entity;

//        await db.SaveChangesAsync();

//        var schedules = payment.LoanRepaymentLines?.ToList();

//        schedules?.ForEach(async item =>
//        {
//            await SetLoanScheduleComplete(req.BranchRefId, req.LoanRefId, item.LoanScheduleRefId,
//                item.InstallmentNo);
//        });

//        await CloseLoanSchedule(req.BranchRefId, req.LoanRefId);

//        tranx.Commit();

//        return Result.Success<string>(new("Payment successfully."));
//    }
//    catch (Exception ex)
//    {
//        tranx.Rollback();

//        return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));
//    }
//}


//foreach (var item in schedules!)
//{
//    //var pp = _context.Database
//    //                .SqlQuery<decimal?>(@$"""SELECT 
//    //                                            v.LeftAmount
//    //                                            FROM VSchedules v
//    //                                            WHERE LoanScheduleId={item.LoanScheduleRefId} AND LoanId={item.LoanRefId}
//    //                                            """)
//    //                .FirstOrDefault();

//    var pp = GetLastPaidAmount(item.BranchRefId, item.LoanRefId, item.LoanScheduleRefId);
//    if (pp == 0)
//    {
//        _context.LoanSchedules.Where(x => x.LoanScheduleId == item.LoanScheduleRefId &&
//                                          x.LoanRefId == item.LoanRefId &&
//                                          x.InstallmentNo == item.InstallmentNo)
//                           .ExecuteUpdate(x => x.SetProperty(p => p.ScheduleStatusRefId, (int)ScheduleStatus.Paid));
//    }
//    if (pp > 0)
//    {
//        _context.LoanSchedules.Where(x => x.LoanScheduleId == item.LoanScheduleRefId &&
//                                          x.LoanRefId == item.LoanRefId &&
//                                          x.InstallmentNo == item.InstallmentNo)
//                           .ExecuteUpdate(x => x.SetProperty(p => p.ScheduleStatusRefId, (int)ScheduleStatus.Partial));

//    }

//}

//var loanInfo = await _context.VLoanDisbursements
//                        .AsNoTracking()
//                        .Where(x => x.Id == loanRepayment.LoanRefId &&
//                               x.BranchId == loanRepayment.BranchRefId &&
//                               x.OutStanding == 0)
//                        .ToListAsync();

//if (loanInfo.Any())
//{
//    _context.LoanDisbursments
//            .Where(x => x.Id == loanRepayment.LoanRefId & x.BranchRefId == loanRepayment.BranchRefId)
//            .ExecuteUpdate(p => p.SetProperty(x => x.LoanStatusRefId, (int)LoanStatuses.Close));
//}

//_tran.Commit();

//return Result.Success<string>(new("Payment successfully."));

//foreach (var item in schedules!)
//{
//    var pp = _context.Database
//                    .SqlQuery<PaymentStatus>(@$"SELECT 
//                                                   v.LeftAmount,
//                                                   v.LoanScheduleId,
//                                                   v.PaidPrinciple,
//                                                   v.PaidInterest,
//                                                   v.PaidCollectionFee,
//                                                   v.PaidAmount,
//                                                   v.LeftInterestAmount,
//                                                   v.LeftCollectionFeeAmount,
//                                                   v.LeftPrinciple
//                                                FROM VSchedules v
//                                                WHERE LoanScheduleId={item.LoanScheduleRefId}")
//                    .FirstOrDefault();

//    if (pp!.LeftAmount == 0)
//    {
//        _context.LoanSchedules.Where(x => x.LoanScheduleId == item.LoanScheduleRefId &&
//                                          x.LoanRefId == item.LoanRefId &&
//                                          x.InstallmentNo == item.InstallmentNo)
//                           .ExecuteUpdate(x => x.SetProperty(p => p.ScheduleStatusRefId, (int)ScheduleStatus.Paid));
//    }
//    if (pp!.LeftAmount > 0)
//    {
//        _context.LoanSchedules.Where(x => x.LoanScheduleId == item.LoanScheduleRefId &&
//                                          x.LoanRefId == item.LoanRefId &&
//                                          x.InstallmentNo == item.InstallmentNo)
//                           .ExecuteUpdate(x => x.SetProperty(p => p.ScheduleStatusRefId, (int)ScheduleStatus.Partial));
//    }

//}

//var loanInfo = await _context.VLoanDisbursements
//                        .AsNoTracking()
//                        .Where(x => x.Id == loanRepayment.LoanRefId &&
//                               x.BranchId == loanRepayment.BranchRefId &&
//                               x.OutStanding == 0)
//                        .ToListAsync();

//if (loanInfo.Any())
//{
//    _context.LoanDisbursments.Where(x => x.Id == loanRepayment.LoanRefId)
//                            .ExecuteUpdate(p => p.SetProperty(x => x.LoanStatusRefId, (int)LoanStatuses.Close));
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
//}


//public async Task<Result<string>> RecievePayment(LoanRepayment loanRepayment)
//{
//    _context.Database.BeginTransaction();

//    try
//    {
//        var schedules = loanRepayment.LoanRepaymentLines;

//        _context.LoanRepayments.Add(loanRepayment);

//        var tranxPayment = new LoanTransaction
//        {
//            TransactionTypeName = "Loan Principal Repayment",
//            TranTypeRefId = 20,
//            IsActive = true,
//            UserRefID = loanRepayment.UserRefId
//        };
//        var totalPayment = loanRepayment.Total(); //loanRepayment.Principle + loanRepayment.Interest + loanRepayment.CollectionFeeAmount + loanRepayment.PenaltyAmount;
//        var tranxPayments = new CashManagement
//        {
//            BankRefId = 11,
//            BranchRefId = loanRepayment.BranchRefId,
//            Amount = totalPayment,
//            CurrencyRefId = loanRepayment.CurrencyRefId,
//            CashStatusRefId = 2,
//            Remark = "Cash goes through transaction",
//            StaffRefId = loanRepayment.UserRefId,
//            Receive = "N",
//            Reject = "N",
//            TransactionDate = DateTime.Now,
//            BusinessDate = DateTime.Now,
//            Balance = 0
//        };
//        tranxPayment.CashManagements?.Add(tranxPayments);
//        _context.LoanTransaction.Add(tranxPayment);
//        await _context.SaveChangesAsync();
//        foreach (var item in schedules!)
//        {
//            var pp = _context.Database.SqlQuery<VSchedule>($"SELECT * FROM VSchedules")
//                                   .Where(x => x.LoanScheduleId == item.LoanScheduleRefId &&
//                                               x.LoanId == item.LoanRefId)
//                                   .FirstOrDefault();

//            if (pp!.Principal == pp.PaidPrinciple)
//            {
//                _context.LoanSchedules
//                            .Where(x => x.LoanScheduleId == item.LoanScheduleRefId
//                                        && x.LoanRefId == item.LoanRefId)
//                            .ExecuteUpdate(x => x.SetProperty(p => p.ScheduleStatusRefId, 1));
//            }
//            if (pp!.Principal > pp.PaidPrinciple)
//            {
//                _context.LoanSchedules.Where(x => x.LoanScheduleId == item.LoanScheduleRefId
//                                                  && x.LoanRefId == item.LoanRefId)
//                                   .ExecuteUpdate(x => x.SetProperty(p => p.ScheduleStatusRefId, 4));
//            }
//        }

//        var loanInfo = _context.VLoanDisbursements.FirstOrDefault(x => x.Id == loanRepayment.LoanRefId &&
//                                                                      x.BranchId == loanRepayment.BranchRefId);

//        if (loanInfo is not null && loanInfo.OutStanding == 0)
//        {

//            _context.LoanDisbursments.Where(x => x.Id == loanRepayment.LoanRefId &&
//                                                 x.BranchRefId == loanRepayment.BranchRefId)
//                                    .ExecuteUpdate(p => p.SetProperty(x => x.LoanStatusRefId, (int)LoanStatuses.Close));

//        }

//        _context.Database.CommitTransaction();

//        return Result.Success<string>(new("Payment successfully."));

//    }
//    catch (Exception ex)
//    {

//        _context.Database.RollbackTransaction();

//        return Result.Failure<string>(Error.ServerError("500", [$"{ex.Message}"]));
//    }
//}

//public async Task<Result<string>> RecievePayment(LoanRepayment loanRepayment, LoanTransaction transaction)
//{

//    using var _transaction = _context.Database.BeginTransaction();

//    try
//    {
//        var schedules = loanRepayment.LoanRepaymentLines;

//        _context.LoanRepayments.Add(loanRepayment);

//        _context.LoanTransaction.Add(transaction);

//        await _context.SaveChangesAsync();

//        foreach (var item in schedules!)
//        {
//            var pp = _context.Database
//                            .SqlQuery<PaymentStatus>(@$"SELECT v.LoanScheduleId,v.PaidPrinciple,
//                                                        v.PaidInterest,v.PaidCollectionFee,
//                                                        v.PaidAmount,v.LeftInterestAmount,
//                                                        v.LeftCollectionFeeAmount,
//                                                        v.LeftPrinciple,v.LeftAmount
//                                                        FROM VSchedules v WHERE v.LoanScheduleId={item.LoanScheduleRefId}")
//                            .Where(x => x.LoanScheduleId == item.LoanScheduleRefId)
//                            .FirstOrDefault();

//            if (pp!.LeftAmount == 0)
//            {
//                _context.LoanSchedules.Where(x => x.LoanScheduleId == item.LoanScheduleRefId &&
//                                                  x.LoanRefId == item.LoanRefId &&
//                                                  x.InstallmentNo == item.InstallmentNo)
//                                   .ExecuteUpdate(x => x.SetProperty(p => p.ScheduleStatusRefId, 1));
//            }
//            if (pp!.LeftAmount > 0)
//            {
//                _context.LoanSchedules.Where(x => x.LoanScheduleId == item.LoanScheduleRefId &&
//                                                  x.LoanRefId == item.LoanRefId &&
//                                                  x.InstallmentNo == item.InstallmentNo)
//                                   .ExecuteUpdate(x => x.SetProperty(p => p.ScheduleStatusRefId, 4));
//            }
//        }
//        var loanInfo = _context.VLoanDisbursements.FirstOrDefault(x => x.Id == loanRepayment.LoanRefId);

//        if (loanInfo is not null && loanInfo.OutStanding == 0)
//        {

//            _context.LoanDisbursments.Where(x => x.Id == loanRepayment.LoanRefId)
//                                    .ExecuteUpdate(p => p.SetProperty(x => x.LoanStatusRefId, 2));

//        }

//        _transaction.Commit();

//        return Result.Success<string>(new("Payment successfully."));

//    }
//    catch (Exception ex)
//    {

//        _transaction.Rollback();

//        return Result.Failure<string>(Error.ServerError("500", [$"{ex.Message}"]));
//    }
//}