using System.Security.Claims;
using Applications.Commons.Mappers;
using Applications.Enumerations;
using Applications.Extensions;
using Applications.Extensions.Identity;
using Applications.Requests.Loans.LoanRequests;
using Applications.Responses.ViewModels.Loans.Requests;
using Infrastructures.Authorizations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;
using TrapchumApp.Shared;

namespace TrapchumApp.Pages.Transactions.Loans.LoanRequests
{
    public partial class LoanRequestDetail
    {
        [CascadingParameter] Task<AuthenticationState> State { get; set; } = default!;
        [CascadingParameter] public CascadingAppState AppState { get; set; } = default!;
        [Parameter, SupplyParameterFromQuery] public string? LoanRequestId { get; set; }
        [Parameter, SupplyParameterFromQuery] public string? CustomerId { get; set; }
        [Parameter, SupplyParameterFromQuery] public string? UserId { get; set; }
        [Parameter] public int? BranchId { get; set; }


        VLoanRequests? data = new();

        string memo = "";

        string? aprovedUserId = string.Empty;

        bool success = false;

        bool canApprove = false;

        bool canAllow = false;

        ClaimsPrincipal? user;

        protected override async Task OnInitializedAsync()
        {

            user = (await State).User;

            aprovedUserId = user.GetId();

            await AppState.CheckedCanApprved(user);

            await AppState.CheckedCanDisbursemnt(user);

            await AppState.CheckedCanRequest(user);

            await GetLoanRequestDetails(user);

            await base.OnInitializedAsync();
        }

        async Task CreateLoanRequest()
        {
            var confirm = await DialogService.Confirm("Do you want to aprove this request?", "Loan Request");

            if (confirm == false)
            {
                NotificationService.Notify(NotificationSeverity.Info, "Reuqest wa cancel.");

                return;
            }

            var model = data?.MapToLoanRequestCreate();

            if (memo.IsNotNullOrEmpty() && model is not null)
            {
                var description = new LoanDescriptionsDtoCreate(memo, aprovedUserId, LoanRequestId);

                model.LoanDescriptions.Add(description);

            }

            if(model is null)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Loan request data is null.");

                return;
            }

            var rsp = await _loanRequestService.CreateLoanRequestAsync(model);

            if (rsp.IsSuccess)
            {
                //success = rsp.Data;

                NotificationService.Notify(NotificationSeverity.Success, "Success");

                StateHasChanged();
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, rsp.ErrMessage);
            }
        }


        async Task GetLoanRequestDetails(ClaimsPrincipal user, bool include = true)
        {
            
            var resp = await _loanRequestService.GetLoanRequestAsync(BranchId, CustomerId, LoanRequestId, include);

            if (resp.IsSuccess)
            {
                data = resp.Data;

                canAllow = (await _authorizeService.AuthorizeAsync(user, data, new SameUserRequestRequirement())).Succeeded;
               
                canApprove = (await _authorizeService.AuthorizeAsync(user, data, new SameUserApprovedRequirement())).Succeeded;
               
                success = data?.StatusId == 2 ? false : true;
            }
        }

        async Task AprovedLoanRequest()
        {
            var options = new DialogOptions
            {
            };
            var param = new Dictionary<string, object?>
            {
                { "LoanRequestId", LoanRequestId },
                { "BranchId", BranchId },
                { "CustomerId", CustomerId },
                { "UserId", UserId }
            };

            bool dialogueResult = await DialogService.OpenAsync<ConfirmModal>("Confirm", param, options);

            if (dialogueResult && user is not null)
            {
                await GetLoanRequestDetails(user, true);

                StateHasChanged();
            }
        }

        async Task DeniedLoanRequest()
        {
            var confirm = await DialogService.Confirm("Do you want to reject this request?", "Loan Request");

            if (confirm == false)
            {
                NotificationService.Notify(NotificationSeverity.Info, "Reuqest wa cancel.");
                return;
            }

            var req = new DeniedRequestDtoCreate(BranchId, CustomerId, LoanRequestId, DateTime.UtcNow, memo);

            var rsp = await _loanRequestService.DeniedLoanRequestAsync(req);

            if (rsp.IsSuccess)
            {

                var description = new LoanDescriptionsDtoCreate(memo,UserId,LoanRequestId);

                await _loanRequestService.AddDescriptionAsync(description);

                NotificationService.Notify(NotificationSeverity.Success, rsp.Data);
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, rsp.ErrMessage);
            }
        }

        async Task RecheckedLoanRequest()
        {
            var confirm = await DialogService.Confirm("Do you want to return back this request?", "Loan Request");

            if (confirm == false)
            {
                NotificationService.Notify(NotificationSeverity.Info, "Reuqest wa cancel.");

                return;
            }

            var req = new RecheckedRequestDtoCreate(BranchId, CustomerId, LoanRequestId, DateTime.UtcNow,RequestStatus.Rechecked, memo);

            var rsp = await _loanRequestService.RecheckedLoanRequestAsync(req);

            if (rsp.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Success, "");
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, rsp.ErrMessage);
            }
        }

        async Task SaveLoanRequest()
        {
            var confirm = await DialogService.Confirm("Do you want to save this request?", "Loan Request");

            if (confirm == false)
            {
                NotificationService.Notify(NotificationSeverity.Info, "Reuqest wa cancel.");

                return;
            }

            var req = new RecheckedRequestDtoCreate(BranchId, CustomerId, LoanRequestId, DateTime.UtcNow,RequestStatus.Rechecked, memo);

            var rsp = await _loanRequestService.VerifiedLoanRequestAsync(req);

            if (rsp.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Success, rsp.Data);
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, rsp.ErrMessage);
            }
        }
    }
}