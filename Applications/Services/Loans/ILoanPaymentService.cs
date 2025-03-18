using Applications.Interfaces.Loans;
using Applications.Requests.Loans;
using Applications.Responses;
using Applications.Responses.ViewModels.Loans.Payments;

namespace Applications.Services.Loans;

public sealed class LoanPaymentService(ILoanPaymentService lnService) : ILoanPaymentService
{
    public async Task<Result<IEnumerable<VLoanPaymentLines>>> GetPaymentLinesAsync(int? branchId, long? loanRefId, int? installmentNo)
    {
        return await lnService.GetPaymentLinesAsync(branchId, loanRefId, installmentNo);    
    }

    public async Task<Result<string>> RecievePayment(LoanRepaymentDtoCreate req)
    {
       return await lnService.RecievePayment(req);
    }
}
