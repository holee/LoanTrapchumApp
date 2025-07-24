using Applications.Commons.Mappers;
using Applications.Enumerations;
using Applications.Features.Loans.Responses;
using Applications.Interfaces.Loans;
using Applications.Requests.Loans.CreditAgents;
using Applications.Requests.Loans.LoanRequests;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans.Requests;
using Domains.Models.Loans;
using Mapster;

namespace Applications.Features.LoanRequests.Services
{
    public sealed class LoanRequestService(ILoanRequestService reqService) : ILoanRequestServiceImpl
    {
        public async Task<Result<string>> AddLoanRequestCollateralAsync(LoanCollateralRequest req)
        {
            return await reqService.AddLoanRequestCollateralAsync(req);
        }

        public async Task<Result<string>> AddLoanRequestDocumentAsync(LoanDocumentDtoCreate request)
        {
            var model = request.Adapt<LoanDocuments>();

            return await reqService.AddLoanRequestDocumentAsync(model);
        }

        public async Task<Result<string>> AddLoanRequestDocumentAsync(List<LoanDocumentDtoCreate> request)
        {
            var model = request.DocumentsDtoCreate();

            return await reqService.AddLoanRequestDocumentAsync(model);
        }

        public async Task<Result<string>> AddLoanRequestGuarantorAsync(LoanRequestGuarantorsDtoCreate request)
        {
            var req = request.MapTo();

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


        public async Task<Result<DisburseResult>> AprovedRequestAsync(AproveLoanDtoCreate req)
        {
            var rsp = await reqService.AprovedLoanRequestAsync(req);

            if (rsp.IsSuccess)
            {
                return Result.Success(new DisburseResult(true, rsp.Data));
            }

            return Result.Failure<DisburseResult>(ErrorResponse.Validation(rsp.ErrMessage));
        }

        public async Task<Result<string>> ChangedLoanRequestAsync(LoanChangeStatusDtoCreate req)
        {
            return await reqService.ChangedLoanRequestAsync(req);
        }

        public async Task<Result<string>> CreateNewLoanRequestAsync(LoanRequestDtoCreate request, CancellationToken cancellation = default)
        {

            var req = request.MapToLoanCreateNew();

            return await reqService.CreateNewLoanRequestAsync(req, cancellation);
        }


        public async Task<Result<DisburseResult>> CreateNewRequestAsync(LoanRequestDtoCreate request, CancellationToken cancellation = default)
        {

            var req = request.MapToLoanCreateNew();

            var rsp = await reqService.CreateNewLoanRequestAsync(req, cancellation);

            if (rsp.IsSuccess)
            {
                return Result.Success<DisburseResult>(new(true, rsp.Data));
            }

            return Result.Failure<DisburseResult>(ErrorResponse.ServerError(rsp.ErrMessage));
        }

        public async Task<Result<string>> SaveLoanRequestAsync(LoanRequestDtoCreate request, CancellationToken cancellation = default)
        {
            var req = request.MapToLoanRequestCreate();

            return await reqService.SaveLoanRequestAsync(req, cancellation);
        }

        public async Task<Result<string>> CreateLoanRequestAsync(LoanRequestDtoCreate request, CancellationToken cancellation = default)
        {
            var model = request.MapToLoanRequestCreate();

            return await reqService.CreateLoanRequestAsync(model, cancellation);
        }

        public async Task<Result<string>> DeleteLoanRequestCollateralAsync(int? branchId, string? loanRequestId,
            long? collateralLinkId)
        {
            return await reqService.DeleteLoanRequestCollateralAsync(branchId, loanRequestId, collateralLinkId);
        }

        public async Task<Result<string>> DeleteLoanRequestDocumentAsync(string? loanRequestId, int? branchId,
            long? documentId)
        {
            return await reqService.DeleteLoanRequestDocumentAsync(loanRequestId, branchId, documentId);
        }

        public async Task<Result<string>> DeleteLoanRequestGuarantorAsync(string? loanRequestId, int? branchId,
            long? guarantorLinkId)
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

        public async Task<Result<IEnumerable<VLoanRequestCollateralLines>>> GetLoanRequestCollateralAsync(int? branchId,
            string? loanRequestId)
        {
            return await reqService.GetLoanRequestCollateralAsync(branchId, loanRequestId);
        }

        public async Task<Result<IEnumerable<VLoanRequestsDocuments>>> GetLoanRequestDocumentAsync(
            string? loanRequestId, int? branchId)
        {
            return await reqService.GetLoanRequestDocumentAsync(loanRequestId, branchId);
        }

        public async Task<Result<IEnumerable<VLoanRequestGuarantors>>> GetLoanRequestGuarantorAsync(int? branchId,
            string? loanRequestId, string? customerId)
        {
            return await GetLoanRequestGuarantorAsync(branchId, loanRequestId, customerId);
        }

        public async Task<Pager<VLoanRequests>> GetLoanRequestsAsync(string? userId, int? branchId,
            RequestStatus requestStatus, QueryParam param)
        {
            return await reqService.GetLoanRequestsAsync(userId, branchId, requestStatus, param);
        }

        public async Task<Pager<VLoanRequests>> GetLoanRequestsAsync(string? userId, bool candisburse, int? branchId, RequestStatus requestStatus, QueryParam param)
        {
            return await reqService.GetLoanRequestsAsync(userId, candisburse, branchId, requestStatus, param);
        }


        public async Task<Result<VLoanRequests>> GetLoanRequestAsync(int? branchId, string? customerId, string? loanRequestId, bool include)
        {
            return await reqService.GetLoanRequestAsync(branchId, customerId, loanRequestId, include);
        }

        public async Task<Result<VLoanRequests>> GetApprovedLoanRequestAsync(int? branchId, string? customerId, string? loanRequestId, bool include)
        {
            return await reqService.GetApprovedLoanRequestAsync(branchId, customerId, loanRequestId, include);
        }

        public async Task<Result<VLoanRequests>> GetLoanRequestAsync(int? branchId, string? userId, string? customerId, string? loanRequestId, bool v)
        {
            return await reqService.GetLoanRequestAsync(branchId, userId, customerId, loanRequestId, v);
        }

        public async Task<Result<T>> GetLoanRequestAsync<T>(int? branchId, string? customerId, string? loanRequestId)
            where T : class
        {
            return await reqService.GetLoanRequestAsync<T>(branchId, customerId, loanRequestId);
        }

        public async Task<Result<VLoanRequests>> RecheckedLoanRequestAsync(RecheckedRequestDtoCreate req)
        {
            return await reqService.RecheckedLoanRequestAsync(req);
        }

        public async Task<Result<string>> AddDescriptionAsync(LoanDescriptionsDtoCreate req, CancellationToken cancellation = default)
        {
            var model = req.MapToDescriptionCreate();
            return await reqService.AddDescription(model, cancellation);
        }

        public async Task<Result<string>> VerifiedLoanRequestAsync(RequestDtoBaseCreate req)
        {
            return await reqService.VerifiedLoanRequestAsync(req);
        }


        public async Task<bool> IsCustomerBusy(string? customerId, CancellationToken cancellation = default)
        {

            ArgumentException.ThrowIfNullOrWhiteSpace(nameof(customerId));

            return await reqService.IsCustomerBusy(customerId, cancellation);

        }

    }
}