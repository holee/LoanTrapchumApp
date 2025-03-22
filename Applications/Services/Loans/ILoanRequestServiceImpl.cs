using Applications.Enumerations;
using Applications.Requests.Loans.CreditAgents;
using Applications.Requests.Loans.LoanRequests;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans.Requests;
using Domains.Models.Loans;

namespace Applications.Services.Loans
{
    public interface ILoanRequestServiceImpl
    {
        Task<Result<string>> AddLoanRequestCollateralAsync(LoanCollateralRequest req);
        Task<Result<string>> AddLoanRequestDocumentAsync(LoanRequestDocuments request);
        Task<Result<string>> AddLoanRequestGuarantorAsync(LoanRequestGuarantorsDtoCreate request);
        Task<Result<string>> AppliedLoanRequestAsync(RequestDtoBaseCreate req);
        Task<Result<string>> AprovedLoanRequestAsync(AproveLoanDtoCreate req);
        Task<Result<string>> ChangedLoanRequestAsync(LoanChangeStatusDtoCreate req);
        Task<Result<string>> CreateLoanRequestAsync(LoanRequestDtoCreate request);
        Task<Result<string>> DeleteLoanRequestCollateralAsync(int? branchId, Guid? loanRequestId, long? collateralLinkId);
        Task<Result<string>> DeleteLoanRequestDocumentAsync(Guid? loanRequestId, int? branchId, long? documentId);
        Task<Result<string>> DeleteLoanRequestGuarantorAsync(Guid? loanRequestId, int? branchId, long? guarantorLinkId);
        Task<Result<string>> DeniedLoanRequestAsync(DeniedRequestDtoCreate req);
        Task<Result<string>> EditLoanRequestAsync(LoanRequest request);
        Task<Result<VLoanRequests>> GetLoanRequestByCustomerAsync(int? branchId, Guid? customerId, Guid? loanRequestId, bool include);
        Task<Result<IEnumerable<VLoanRequestCollateralLines>>> GetLoanRequestCollateralAsync(int? branchId, Guid? loanRequestId);
        Task<Result<IEnumerable<VLoanRequestsDocuments>>> GetLoanRequestDocumentAsync(Guid? loanRequestId, int? branchId);
        Task<Result<IEnumerable<VLoanRequestGuarantors>>> GetLoanRequestGuarantorAsync(int? branchId, Guid? loanRequestId, Guid? customerId);
        Task<Pager<VLoanRequests>> GetLoanRequestsAsync(Guid? userId, int? branchId, RequestStatus requestStatus, QueryParam param);
        Task<Result<VLoanRequests>> GetLoanRequestsAsync(int? branchId, Guid? customerId, Guid? loanRequestId, bool v);
        Task<Result<string>> RecheckedLoanRequestAsync(RecheckedRequestDtoCreate req);
        Task<Result<string>> VerifiedLoanRequestAsync(RequestDtoBaseCreate req);
    }
}
