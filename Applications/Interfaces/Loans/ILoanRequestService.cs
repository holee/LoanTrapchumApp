using Applications.Enumerations;
using Applications.Features.CreditAgents.Requests;
using Applications.Features.LoanRequests.Requests;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans.Requests;
using Domains.Models.Loans;

namespace Applications.Interfaces.Loans
{
    public interface ILoanRequestService
    {
        Task<Result<string>> ChangedLoanRequestAsync(LoanChangeStatusDtoCreate req);
        Task<Result<string>> CreateNewLoanRequestAsync(LoanRequest request, CancellationToken cancellationToken = default);
        //Task<Result<string>> CreateLoanRequestAsync(LoanRequest req, CancellationToken cancellation = default);
        Task<Result<string>> SaveLoanRequestAsync(LoanRequest req, CancellationToken cancellationToken = default);
        Task<Result<string>> AprovedLoanRequestAsync(AproveLoanDtoCreate req);
        Task<Result<VLoanRequests>> RecheckedLoanRequestAsync(RecheckedRequestDtoCreate req);
        Task<Result<string>> DeniedLoanRequestAsync(DeniedRequestDtoCreate req);
        Task<Result<string>> VerifiedLoanRequestAsync(RequestDtoBaseCreate req);
        Task<Result<string>> AppliedLoanRequestAsync(RequestDtoBaseCreate req);
        Task<Result<string>> EditLoanRequestAsync(LoanRequest request);
        Task<Result<string>> AddLoanRequestDocumentAsync(LoanDocuments request);
        Task<Result<string>> AddLoanRequestGuarantorAsync(LoanRequestGuarantors request);
        Task<Result<string>> AddLoanRequestCollateralAsync(LoanCollateralRequest req);
        Task<Result<IEnumerable<VLoanRequestGuarantors>>> GetLoanRequestGuarantorAsync(int? branchId, string? loanRequestId, string? customerId);
        Task<Result<string>> DeleteLoanRequestGuarantorAsync(string? loanRequestId, int? branchId, long? guarantorLinkId);
        Task<Result<string>> DeleteLoanRequestCollateralAsync(int? branchId, string? loanRequestId, long? collateralLinkId);
        Task<Result<IEnumerable<VLoanRequestCollateralLines>>> GetLoanRequestCollateralAsync(int? branchId, string? loanRequestId);
        Task<Result<string>> DeleteLoanRequestDocumentAsync(string? loanRequestId, int? branchId, long? documentId);
        Task<Result<IEnumerable<VLoanRequestsDocuments>>> GetLoanRequestDocumentAsync(string? loanRequestId, int? branchId);
        Task<Result<T>> GetLoanRequestAsync<T>(int? branchId, string? customerId, string? loanRequestId) where T : class;
        Task<Result<VLoanRequests>> GetLoanRequestAsync(int? branchId, string? customerId, string? loanRequestId, bool include);
        Task<Pager<VLoanRequests>> GetLoanRequestsAsync(string? userId, int? branchId, RequestStatus requestStatus, QueryParam param);
        Task<Result<string>> AddLoanRequestDocumentAsync(List<LoanDocuments> documents);
        Task<Result<VLoanRequests>> GetLoanRequestAsync(int? branchId, string? userId, string? customerId, string? loanRequestId, bool include);
        Task<Pager<VLoanRequests>> GetLoanRequestsAsync(string? userId, bool canDisburse, int? branchId, RequestStatus requestStatus, QueryParam param);
        Task<Result<string>> AddLoanRequestDocumentAsync(int? branchId, string? loanRequestId, List<LoanDocuments> documents);
        Task<bool> IsCustomerBusy(string? customerId, CancellationToken cancellation = default);
        Task<Result<string>> AddDescription(LoanDescriptions descriptions, CancellationToken cancellation = default);
        Task<Result<VLoanRequests>> GetApprovedLoanRequestAsync(int? branchId, string? customerId, string? loanRequestId, bool include);
        Task<Result<string>> EditLoanRequestAsync(LoanRequest req, CancellationToken cancellationToken = default);
    }
}