using Applications.Enumerations;
using Applications.Requests.Loans.CreditAgents;
using Applications.Requests.Loans.LoanRequests;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans.Requests;
using Domains.Models.Loans;

namespace Applications.Interfaces.Loans
{
    public interface ILoanRequestService
    {
        Task<Result<string>> ChangedLoanRequestAsync(LoanChangeStatusDtoCreate req);
        Task<Result<string>> CreateLoanRequestAsync(LoanRequest request);
        Task<Result<string>> AprovedLoanRequestAsync(AproveLoanDtoCreate req);
        Task<Result<string>> RecheckedLoanRequestAsync(RecheckedRequestDtoCreate req);
        Task<Result<string>> DeniedLoanRequestAsync(DeniedRequestDtoCreate req);
        Task<Result<string>> VerifiedLoanRequestAsync(RequestDtoBaseCreate req);
        Task<Result<string>> AppliedLoanRequestAsync(RequestDtoBaseCreate req);
        Task<Result<string>> EditLoanRequestAsync(LoanRequest request);
        Task<Result<VLoanRequests>> GetLoanRequestByCustomerAsync(int? branchId, Guid? customerId, Guid? loanRequestId, bool include);
        Task<Result<string>> AddLoanRequestDocumentAsync(LoanRequestDocuments request);
        Task<Result<string>> AddLoanRequestGuarantorAsync(LoanRequestGuarantors request);
        Task<Result<string>> AddLoanRequestCollateralAsync(LoanCollateralRequest req);
        Task<Result<IEnumerable<VLoanRequestGuarantors>>> GetLoanRequestGuarantorAsync(int? branchId, Guid? loanRequestId, Guid? customerId);
        Task<Result<string>> DeleteLoanRequestGuarantorAsync(Guid? loanRequestId, int? branchId, long? guarantorLinkId);
        Task<Result<string>> DeleteLoanRequestCollateralAsync(int? branchId, Guid? loanRequestId, long? collateralLinkId);
        Task<Result<IEnumerable<VLoanRequestCollateralLines>>> GetLoanRequestCollateralAsync(int? branchId, Guid? loanRequestId);
        Task<Result<string>> DeleteLoanRequestDocumentAsync(Guid? loanRequestId, int? branchId, long? documentId);
        Task<Result<IEnumerable<VLoanRequestsDocuments>>> GetLoanRequestDocumentAsync(Guid? loanRequestId, int? branchId);

        Task<Pager<VLoanRequests>> GetLoanRequestsAsync(Guid? userId, int? branchId, RequestStatus requestStatus, QueryParam param);
    }
}
//Task<Pager<VLoanRequests>> GetLoanRequestsAsync(long userId, int statusId, int branchId, QueryParam param);
//Task<Result<VLoanRequests>> GetLoanRequestAsync(int branchId, long customerId, long loanRequestId, int statusId, long AproveUserId = 0);
//Task<Pager<VLoanRequests>> GetLoanRequestsAsync(long aproveUseId,long credetAgentId, int statusId, int branchId, QueryParam param);
//Task<Result<LoanRequest>> GetLoanRequestAsync(long customerId, long loanRequestId, int branchId = 0, bool include=false);
//Task<Result<IEnumerable<VLoanRequests>>> GetLoanRequestsAsync(long AprovedId, string? filter = null);