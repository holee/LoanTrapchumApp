using Applications.Enumerations;
using Applications.Features.CreditAgents.Requests;
using Applications.Features.LoanRequests.Requests;
using Applications.Features.Loans.Responses;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans.Requests;
using Domains.Models.Loans;
using ErrorOr;

namespace Applications.Interfaces.Loans
{
    public interface ILoanRequestService
    {
        Task<ErrorOr<string>> ChangedLoanRequestAsync(LoanChangeStatusDtoCreate req);
        Task<ErrorOr<RequestResult>> CreateNewLoanRequestAsync(LoanRequest request, CancellationToken cancellationToken = default);
        Task<ErrorOr<RequestResult>> SaveLoanRequestAsync(LoanRequest req, CancellationToken cancellationToken = default);
        Task<ErrorOr<RequestResult>> AprovedLoanRequestAsync(AproveLoanDtoCreate req, CancellationToken cancellationToken = default);
        Task<ErrorOr<RequestResult>> RecheckedLoanRequestAsync(RecheckedRequestDtoCreate req, CancellationToken cancellationToken = default);
        Task<ErrorOr<RequestResult>> DeniedLoanRequestAsync(DeniedRequestDtoCreate req, CancellationToken cancellationToken = default);
        Task<ErrorOr<RequestResult>> VerifiedLoanRequestAsync(RequestDtoBaseCreate req, CancellationToken cancellationToken = default);
        Task<ErrorOr<RequestResult>> AppliedLoanRequestAsync(RequestDtoBaseCreate req, CancellationToken cancellationToken = default);
        Task<ErrorOr<RequestResult>> EditLoanRequestAsync(LoanRequest request, CancellationToken cancellation = default);
        Task<ErrorOr<string>> AddLoanRequestDocumentAsync(LoanDocuments request, CancellationToken cancellationToken = default); 
        Task<ErrorOr<string>> AddLoanRequestGuarantorAsync(LoanRequestGuarantors request, CancellationToken cancellationToken = default);
        Task<ErrorOr<string>> AddLoanRequestCollateralAsync(LoanCollateralRequest req, CancellationToken cancellationToken = default);
        Task<ErrorOr<IEnumerable<VLoanRequestGuarantors>>> GetLoanRequestGuarantorAsync(int? branchId, string? loanRequestId, string? customerId);
        Task<ErrorOr<string>> DeleteLoanRequestGuarantorAsync(string? loanRequestId, int? branchId, long? guarantorLinkId);
        Task<ErrorOr<string>> DeleteLoanRequestCollateralAsync(int? branchId, string? loanRequestId, long? collateralLinkId);
        Task<ErrorOr<IEnumerable<VLoanRequestCollateralLines>>> GetLoanRequestCollateralAsync(int? branchId, string? loanRequestId);
        Task<ErrorOr<string>> DeleteLoanRequestDocumentAsync(string? loanRequestId, int? branchId, long? documentId);
        Task<ErrorOr<IEnumerable<VLoanRequestsDocuments>>> GetLoanRequestDocumentAsync(string? loanRequestId, int? branchId);
        Task<ErrorOr<T>> GetLoanRequestAsync<T>(int? branchId, string? customerId, string? loanRequestId) where T : class;
        Task<ErrorOr<VLoanRequests>> GetLoanRequestAsync(int? branchId, string? customerId, string? loanRequestId, bool include);
        Task<ErrorOr<string>> AddLoanRequestDocumentAsync(List<LoanDocuments> documents, CancellationToken cancellation = default);
        Task<ErrorOr<VLoanRequests>> GetLoanRequestAsync(int? branchId, string? userId, string? customerId, string? loanRequestId, bool include);
        Task<Pager<VLoanRequests>> GetLoanRequestsAsync(string? userId, bool canDisburse, int? branchId, RequestStatus requestStatus, QueryParam param);
        Task<Pager<VLoanRequests>> GetLoanRequestsAsync(string? userId, int? branchId, RequestStatus requestStatus, QueryParam param);
        Task<ErrorOr<string>> AddLoanRequestDocumentAsync(int? branchId, string? loanRequestId, List<LoanDocuments> documents,CancellationToken cancellation = default);
        Task<ErrorOr<bool>> IsCustomerBusy(string? customerId, CancellationToken cancellation = default);
        Task<ErrorOr<string>> AddDescription(LoanDescriptions descriptions, CancellationToken cancellation = default);
        Task<ErrorOr<VLoanRequests>> GetApprovedLoanRequestAsync(int? branchId, string? customerId, string? loanRequestId, bool include);
    }
}