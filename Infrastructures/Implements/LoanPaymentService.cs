using System.Data;
using Applications.Enumerations;
using Applications.Interfaces.Loans;
using Applications.Requests.Loans.Payments;
using Applications.Responses;
using Applications.Responses.ViewModels.Loans;
using Applications.Responses.ViewModels.Loans.Payments;
using Applications.Responses.ViewModels.Loans.Schedules;
using Dapper;
using Domains.Models.Loans;
using Domains.Models.Transaction;
using Infrastructures.DataAccess;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Implements;
public class LoanPaymentService : ILoanPaymentService
{
    private readonly ApplicationDbContext _context;
    public LoanPaymentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<string>> RecievePayment(LoanRepaymentDtoCreate req)
    {
        using var tranx = _context.StartTransaction();

        try
        {
            
            var tranXchange = req.ForeignExchangeTrx.Adapt<LoanTransaction>();

            tranXchange.TransactionsId = $"trx-{Guid.CreateVersion7()}";


            var tranLoan = req.LoanTransaction.Adapt<LoanTransaction>();

            tranLoan.TransactionsId = $"trx-{Guid.CreateVersion7()}";

            var loanRepayment = req.Adapt<LoanPayment>();

            var payment = _context.LoanRepayments.Add(loanRepayment).Entity;

            _context.LoanTransaction.Add(tranLoan);

            _context.LoanTransaction.Add(tranXchange);

            await _context.SaveChangesAsync();

            var schedules = payment.LoanRepaymentLines?.ToList();

            schedules?.ForEach(item =>
            {
                SetLoanScheduleComplete(req.BranchRefId, req.LoanRefId, item.LoanScheduleRefId, item.InstallmentNo);
            });

            await CloseLoanSchedule(req.BranchRefId, req.LoanRefId);

            tranx.Commit();

            return Result.Success<string>(new("Payment successfully."));

        }
        catch (Exception ex)
        {
            tranx.Rollback();

            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));
        }
    }

    public async Task<Result<string>> PayOff(LoanRepaymentDtoCreate req)
    {

        using var tranx = _context.StartTransaction();

        try
        {

            var tranXchange = req.ForeignExchangeTrx.Adapt<LoanTransaction>();

            var tranLoan = req.LoanTransaction.Adapt<LoanTransaction>();

            var loanRepayment = req.Adapt<LoanPayment>();

            var payment = _context.LoanRepayments.Add(loanRepayment).Entity;

            _context.LoanTransaction.Add(tranLoan);

            _context.LoanTransaction.Add(tranXchange);

            await _context.SaveChangesAsync();

            var schedules = payment.LoanRepaymentLines?.ToList();

            schedules?.ForEach(item =>
            {
                SetLoanScheduleComplete(req.BranchRefId, req.LoanRefId, item.LoanScheduleRefId, item.InstallmentNo);
            });


            await CloseLoanSchedule(req.BranchRefId, req.LoanRefId);

            tranx.Commit();

            return Result.Success<string>(new("Payment successfully."));

        }
        catch (Exception ex)
        {

            tranx.Rollback();
            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));
        }
    }

    public async Task<Result<IEnumerable<VLoanPaymentLines>>> GetPaymentLinesAsync(int? branchId, string? loanId, int? installmentNo)
    {

        using var db = _context.Database.GetDbConnection();

        var repaymentLines = await db.QueryAsync<VLoanPaymentLines>("spGetPayementScheduleLines", new
        {
            branchId,
            loanId,
            installmentNo
        }, commandType: CommandType.StoredProcedure);

        if (!repaymentLines.Any())
        {
            return Result.Failure<IEnumerable<VLoanPaymentLines>>(ErrorResponse.NotFound("No Schedule Lines found"));
        }

        return Result.Success(repaymentLines);
    }

    private void SetLoanScheduleComplete(int? branchId, string? loanId, long? scheduleId, int? installmentId)
    {
        var leftAmount = _context.Database
                                .SqlQuery<VSchedule?>($"""
                                       SELECT * FROM VSchedules
                                       WHERE LoanScheduleId={scheduleId}
                                       AND LoanId={loanId}                                                    
                                       AND BranchId={branchId}
                                      """)
                            .Select(x => x!.LeftAmount)
                            .FirstOrDefault();

        var statusId = leftAmount == 0 ? (int)ScheduleStatus.Paid : (int)ScheduleStatus.Partial;

        _context.LoanSchedules
                .Where(x => x.LoanScheduleId == scheduleId &&
                         x.LoanRefId == loanId && x.InstallmentNo == installmentId)
                .ExecuteUpdate(x => x.SetProperty(p => p.ScheduleStatusRefId, statusId));
    }

    async Task CloseLoanSchedule(int? branchId, string? loanId)
    {
        var existingLoans = await _context.Database
                                    .SqlQuery<VLoanDisbursments>($"""
                                                                    SELECT * FROM VLoanDisbursments
                                                                    WHERE LoanId={loanId} AND 
                                                                    BranchId={branchId} AND 
                                                                    OutStanding=0
                                                                  """)
                                    .Where(x => x.LoanId == loanId && x.BranchId == branchId && x.OutStanding == 0)
                                    .AsNoTracking()
                                    .ToListAsync();
        if (existingLoans.Any())
        {
            _context.LoanDisbursments
                 .Where(x => x.LoanId == loanId & x.BranchRefId == branchId)
                 .ExecuteUpdate(p => p.SetProperty(x => x.LoanStatusRefId, (int)LoanStatuses.Close));
        }
    }

}


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
