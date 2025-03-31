using Applications.Enumerations;
using Applications.Interfaces.Loans;
using Applications.Requests.Loans.CreditAgents;
using Applications.Requests.Loans.LoanRequests;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans.Requests;
using Domains.Models.Loans;
using Mapster;

namespace Applications.Services.Loans
{
    public sealed class LoanRequestService(ILoanRequestService reqService) : ILoanRequestServiceImpl
    {
        public async Task<Result<string>> AddLoanRequestCollateralAsync(LoanCollateralRequest req)
        {
            return await reqService.AddLoanRequestCollateralAsync(req);
        }

        public async Task<Result<string>> AddLoanRequestDocumentAsync(LoanRequestDocuments request)
        {
            return await reqService.AddLoanRequestDocumentAsync(request);
        }


        public async Task<Result<string>> AddLoanRequestGuarantorAsync(LoanRequestGuarantorsDtoCreate request)
        {

            var req = request.Adapt<LoanRequestGuarantors>();

            return await reqService.AddLoanRequestGuarantorAsync(req);
        }

        public async Task<Result<string>> AppliedLoanRequestAsync(RequestDtoBaseCreate req)
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

        public async Task<Result<string>> CreateLoanRequestAsync(LoanRequestDtoCreate request)
        {
            var req = request.Adapt<LoanRequest>();

            return await reqService.CreateLoanRequestAsync(req);
        }

        public async Task<Result<string>> DeleteLoanRequestCollateralAsync(int? branchId, Guid? loanRequestId, long? collateralLinkId)
        {
            return await DeleteLoanRequestCollateralAsync(branchId, loanRequestId, collateralLinkId);
        }

        public async Task<Result<string>> DeleteLoanRequestDocumentAsync(Guid? loanRequestId, int? branchId, long? documentId)
        {
            return await reqService.DeleteLoanRequestDocumentAsync(loanRequestId, branchId, documentId);
        }

        public async Task<Result<string>> DeleteLoanRequestGuarantorAsync(Guid? loanRequestId, int? branchId, long? guarantorLinkId)
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


        public async Task<Result<VLoanRequests>> GetLoanRequestByCustomerAsync(int? branchId, Guid? customerId, Guid? loanRequestId, bool include)
        {
            return await reqService.GetLoanRequestByCustomerAsync(branchId, customerId, loanRequestId, include);
        }

        public async Task<Result<IEnumerable<VLoanRequestCollateralLines>>> GetLoanRequestCollateralAsync(int? branchId, Guid? loanRequestId)
        {
            return await reqService.GetLoanRequestCollateralAsync(branchId, loanRequestId);
        }

        public async Task<Result<IEnumerable<VLoanRequestsDocuments>>> GetLoanRequestDocumentAsync(Guid? loanRequestId, int? branchId)
        {
            return await reqService.GetLoanRequestDocumentAsync(loanRequestId, branchId);
        }

        public async Task<Result<IEnumerable<VLoanRequestGuarantors>>> GetLoanRequestGuarantorAsync(int? branchId, Guid? loanRequestId, Guid? customerId)
        {
            return await GetLoanRequestGuarantorAsync(branchId, loanRequestId, customerId);
        }

        public async Task<Pager<VLoanRequests>> GetLoanRequestsAsync(Guid? userId, int? branchId, RequestStatus requestStatus, QueryParam param)
        {
            return await reqService.GetLoanRequestsAsync(userId, branchId, requestStatus, param);
        }

        public Task<Result<VLoanRequests>> GetLoanRequestsAsync(int? branchId, Guid? customerId, Guid? loanRequestId, bool v)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<string>> RecheckedLoanRequestAsync(RecheckedRequestDtoCreate req)
        {
            return await reqService.RecheckedLoanRequestAsync(req);
        }

        public async Task<Result<string>> VerifiedLoanRequestAsync(RequestDtoBaseCreate req)
        {
            return await reqService.VerifiedLoanRequestAsync(req);
        }


    }
}
