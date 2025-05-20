using Applications.Interfaces.Loans;
using Applications.Requests.Loans.Payments;
using Applications.Requests.Transactions.LoanTransactions;
using Applications.Responses;
using Applications.Responses.ViewModels.Loans.Payments;
using Applications.Responses.ViewModels.Loans.Schedules;

namespace Applications.Services.Loans;


public interface ILoanPaymentServiceImpl
{
    Task<Result<IEnumerable<VSchedule>>> GetPaidedScheduleAsync(int? branchId, string? loanRefId);
    Task<Result<IEnumerable<LoanPaymentLineVm>>> GetPaymentLinesAsync(int? branchId, string? loanRefId, int? installmentNo);
    Task<Result<ScheduleVm>> GetScheduleByIdAsync(long? scheduleId, int? branchId, string? loanRefId);
    Task<Result<string>> RecievePayment(LoanPaymentDtoCreate req);
    Task<Result<string>> RecievePayment(LoanPaymentTransactionDtoCreate req, int? brancId);
}



public sealed class LoanPaymentService(ILoanPaymentService lnService) : ILoanPaymentServiceImpl
{
    public async Task<Result<IEnumerable<VSchedule>>> GetPaidedScheduleAsync(int? branchId, string? loanRefId)
    {
        return await lnService.GetPaidScheduleAsync(branchId, loanRefId);
    }

    public async Task<Result<ScheduleVm>> GetScheduleByIdAsync(long? scheduleId,int? branchId, string? loanRefId)
    {
        return await lnService.GetScheduleByIdAsync(scheduleId,branchId, loanRefId); 
    }



    public async Task<Result<IEnumerable<LoanPaymentLineVm>>> GetPaymentLinesAsync(int? branchId, string? loanRefId, int? installmentNo)
    {
        return await lnService.GetPaymentLinesAsync(branchId, loanRefId, installmentNo);
    }

    public async Task<Result<string>> RecievePayment(LoanPaymentDtoCreate req)
    {
        return await lnService.RecievePayment(req);
    }

    public async Task<Result<string>> RecievePayment(LoanPaymentTransactionDtoCreate req,int? brancId)
    {

        var model = req.ToLoanTransction(brancId);

        return await lnService.RecievePayment(model);

    }


}
