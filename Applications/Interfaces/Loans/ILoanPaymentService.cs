using Applications.Requests.Loans.Payments;
using Applications.Responses;
using Applications.Responses.ViewModels.Loans.Payments;

namespace Applications.Interfaces.Loans;

public interface ILoanPaymentService
{
    Task<Result<IEnumerable<VLoanPaymentLines>>> GetPaymentLinesAsync(int? branchId,string? loanRefId, int? installmentNo);
    Task<Result<string>> RecievePayment(LoanRepaymentDtoCreate req);
}
