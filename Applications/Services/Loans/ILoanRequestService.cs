using Applications.Enums;
using Applications.Requests.Loans;
using Applications.Requests.Loans.LoanRequests;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans.Requests;
using Domains.Models.Loans;

namespace Applications.Services.Loans
{
    public interface ILoanRequestService
    {
        Task<Result<string>> ChangedLoanRequestAsync(LoanChangeStatusDtoCreate req);
        Task<Result<string>> CreateLoanRequestAsync(LoanRequest request);
        Task<Result<string>> AprovedLoanRequestAsync(AproveLoanDtoCreate req);
        Task<Result<string>> RecheckedLoanRequestAsync(RecheckedRequestDtoCreate req);
        Task<Result<string>> DeniedLoanRequestAsync(DeniedRequestDtoCreate req);
        Task<Result<string>> VerifiedLoanRequestAsync(RequestDtoBase req);
        Task<Result<string>> AppliedLoanRequestAsync(RequestDtoBase req);
        Task<Result<string>> EditLoanRequestAsync(LoanRequest request);
        //Task<Result<VLoanRequests>> GetLoanRequestAsync(int branchId, long customerId, long loanRequestId, int statusId, long AproveUserId = 0);
        //Task<Pager<VLoanRequests>> GetLoanRequestsAsync(long aproveUseId,long credetAgentId, int statusId, int branchId, QueryParam param);
     
        //Task<Result<LoanRequest>> GetLoanRequestAsync(long customerId, long loanRequestId, int branchId = 0, bool include=false);
        Task<Result<VLoanRequests>> GetLoanRequestByCustomerAsync(int branchId, long customerId, long loanRequestId, bool include);
        Task<Result<string>> AddLoanRequestDocumentAsync(LoanRequestDocuments request);
        Task<Result<string>> AddLoanRequestGuarantorAsync(LoanRequestGuarantors request);
        Task<Result<string>> AddLoanRequestCollateralAsync(LoanCollateralRequest req);
        Task<Result<IEnumerable<VLoanRequestGuarantors>>> GetLoanRequestGuarantorAsync(long loanRequestId, int branchId, long customerId);
        Task<Result<string>> DeleteLoanRequestGuarantorAsync(long? loanRequestId, int? branchId,long? guarantorLinkId);
        Task<Result<string>> DeleteLoanRequestCollateralAsync(int? branchId, long? loanRequestId, long? collateralLinkId);
        Task<Result<IEnumerable<VLoanRequestCollateralLines>>> GetLoanRequestCollateralAsync(int? branchId, long? loanRequestId);
        Task<Result<string>> DeleteLoanRequestDocumentAsync(long? loanRequestId, int? branchId, long? documentId);
        Task<Result<IEnumerable<VLoanRequestsDocuments>>> GetLoanRequestDocumentAsync(long loanRequestId, int branchId);
        //Task<Pager<VLoanRequests>> GetLoanRequestsAsync(long userId, int statusId, int branchId, QueryParam param);
        Task<Pager<VLoanRequests>> GetLoanRequestsAsync(long userId, int branchId, RequestStatus requestStatus, QueryParam param);
    }
}

//Task<Result<IEnumerable<VLoanRequests>>> GetLoanRequestsAsync(long AprovedId, string? filter = null);