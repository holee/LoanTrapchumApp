using Applications.Interfaces.Loans;
using Applications.Requests.Loans.Payments;
using Applications.Responses;
using Applications.Responses.ViewModels.Loans.Payments;

namespace Applications.Services.Loans;


public interface ILoanPaymentServiceImpl
{
    Task<Result<IEnumerable<VLoanPaymentLines>>> GetPaymentLinesAsync(int? branchId, string? loanRefId, int? installmentNo);
    Task<Result<string>> RecievePayment(LoanRepaymentDtoCreate req);
}



public sealed class LoanPaymentService(ILoanPaymentService lnService) : ILoanPaymentServiceImpl
{
    public async Task<Result<IEnumerable<VLoanPaymentLines>>> GetPaymentLinesAsync(int? branchId, string? loanRefId, int? installmentNo)
    {
        return await lnService.GetPaymentLinesAsync(branchId, loanRefId, installmentNo);    
    }

    public async Task<Result<string>> RecievePayment(LoanRepaymentDtoCreate req)
    {
       return await lnService.RecievePayment(req);
    }
}
