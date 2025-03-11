using Applications.Requests.Loans;
using Applications.Responses;
using Applications.Responses.ViewModels.Loans.Payments;

namespace Applications.Services.Loans;

public interface ILoanPaymentService
{
    Task<Result<IEnumerable<VLoanPaymentLines>>> GetPaymentLinesAsync(int? branchId,long? loanRefId, int? installmentNo);
    Task<Result<string>> RecievePayment(LoanRepaymentDtoCreate req);
}
