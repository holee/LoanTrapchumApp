using Applications.Enumerations;
using Applications.Requests.Loans;
using Applications.Requests.Loans.LoanRequests;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans.Requests;
using Domains.Models.Loans;

namespace Applications.Services.Loans
{
    public sealed class LoanRequestService(ILoanRequestService reqService) : ILoanRequestService
    {
        public async Task<Result<string>> AddLoanRequestCollateralAsync(LoanCollateralRequest req)
        {
            return await reqService.AddLoanRequestCollateralAsync(req);
        }

        public async Task<Result<string>> AddLoanRequestDocumentAsync(LoanRequestDocuments request)
        {
            return await reqService.AddLoanRequestDocumentAsync(request);
        }

        public async Task<Result<string>> AddLoanRequestGuarantorAsync(LoanRequestGuarantors request)
        {
            return await reqService.AddLoanRequestGuarantorAsync(request);
        }

        public async Task<Result<string>> AppliedLoanRequestAsync(RequestDtoBase req)
        {
            return await reqService.AppliedLoanRequestAsync(req);
        }

        public async Task<Result<string>> AprovedLoanRequestAsync(AproveLoanDtoCreate req)
        {
            return await reqService.AprovedLoanRequestAsync(req);
        }

        public async Task<Result<string>> ChangedLoanRequestAsync(LoanChangeStatusDtoCreate req)
        {
            return await reqService.ChangedLoanRequestAsync(req);
        }

        public async Task<Result<string>> CreateLoanRequestAsync(LoanRequest request)
        {
            return await reqService.CreateLoanRequestAsync(request);
        }

        public async Task<Result<string>> DeleteLoanRequestCollateralAsync(int? branchId, long? loanRequestId, long? collateralLinkId)
        {
            return await DeleteLoanRequestCollateralAsync(branchId, loanRequestId, collateralLinkId);
        }

        public async Task<Result<string>> DeleteLoanRequestDocumentAsync(long? loanRequestId, int? branchId, long? documentId)
        {
            return await reqService.DeleteLoanRequestDocumentAsync(loanRequestId, branchId, documentId);
        }

        public async Task<Result<string>> DeleteLoanRequestGuarantorAsync(long? loanRequestId, int? branchId, long? guarantorLinkId)
        {
            return await reqService.DeleteLoanRequestGuarantorAsync(loanRequestId, branchId, guarantorLinkId);
        }

        public async Task<Result<string>> DeniedLoanRequestAsync(DeniedRequestDtoCreate req)
        {
            return await reqService.DeniedLoanRequestAsync(req);
        }

        public async Task<Result<string>> EditLoanRequestAsync(LoanRequest request)
        {
            return await reqService.EditLoanRequestAsync(request);
        }

        public async Task<Result<VLoanRequests>> GetLoanRequestByCustomerAsync(int branchId, long customerId, long loanRequestId, bool include)
        {
            return await reqService.GetLoanRequestByCustomerAsync(branchId, customerId, loanRequestId, include);
        }

        public async Task<Result<IEnumerable<VLoanRequestCollateralLines>>> GetLoanRequestCollateralAsync(int? branchId, long? loanRequestId)
        {
            return await reqService.GetLoanRequestCollateralAsync(branchId, loanRequestId);
        }

        public async Task<Result<IEnumerable<VLoanRequestsDocuments>>> GetLoanRequestDocumentAsync(long loanRequestId, int branchId)
        {
            return await reqService.GetLoanRequestDocumentAsync(loanRequestId, branchId);
        }

        public async Task<Result<IEnumerable<VLoanRequestGuarantors>>> GetLoanRequestGuarantorAsync(long loanRequestId, int branchId, long customerId)
        {
            return await GetLoanRequestGuarantorAsync(loanRequestId, branchId, customerId);
        }

        public async Task<Pager<VLoanRequests>> GetLoanRequestsAsync(long userId, int branchId, RequestStatus requestStatus, QueryParam param)
        {
            return await reqService.GetLoanRequestsAsync(userId, branchId, requestStatus, param);
        }

        public async Task<Result<string>> RecheckedLoanRequestAsync(RecheckedRequestDtoCreate req)
        {
            return await reqService.RecheckedLoanRequestAsync(req);
        }

        public async Task<Result<string>> VerifiedLoanRequestAsync(RequestDtoBase req)
        {
            return await reqService.VerifiedLoanRequestAsync(req);    
        }
    }
}
