using Applications.Extensions.Identity;
using Applications.Requests.Loans.LoanRequests;
using Applications.Responses.ViewModels.Loans.Requests;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;
using TrapchumApp.Components.Shared;

namespace TrapchumApp.Components.Pages.Transactions.Loans.LoanRequests
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

        string Memo = "";

        string? aprovedUserId = string.Empty;

        bool success = false;

        bool canApprove = false;


        protected override async Task OnInitializedAsync()
        {
            await GetLoanRequestDetails();

            var auth = await State;

            aprovedUserId = auth.User.GetId();


            await AppState.CheckedCanApprved(auth.User);
            await AppState.CheckedCanDisbursemnt(auth.User);
            await AppState.CheckedCanRequest(auth.User);

            await base.OnInitializedAsync();
        }

        async Task ApplyLoanRequest()
        {
            var confirm = await DialogService.Confirm("Do you want to aprove this request?", "Loan Request");

            if (confirm == false)
            {
                NotificationService.Notify(NotificationSeverity.Info, "Reuqest wa cancel.");

                return;
            }

            var rsp = await _loanRequestService.CreateLoanRequestAsync(BranchId, UserId, LoanRequestId);

            if (rsp.IsSuccess)
            {
                success = rsp.Data;

                NotificationService.Notify(NotificationSeverity.Success, "Success");

                StateHasChanged();
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, rsp.ErrMessage);
            }
        }


        async Task GetLoanRequestDetails(bool include = true)
        {
            var resp = await _loanRequestService.GetLoanRequestAsync(BranchId, CustomerId, LoanRequestId, include);

            if (resp.IsSuccess)
            {
                data = resp.Data;

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
                { "CustomerId", CustomerId }
            };

            bool dialogueResult = await DialogService.OpenAsync<ConfirmModal>("Confirm", param, options);

            if (dialogueResult)
            {
                await GetLoanRequestDetails(true);

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

            var req = new DeniedRequestDtoCreate(BranchId, CustomerId, LoanRequestId, DateTime.UtcNow, Memo);

            var rsp = await _loanRequestService.DeniedLoanRequestAsync(req);

            if (rsp.IsSuccess)
            {
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

            var req = new RecheckedRequestDtoCreate(BranchId, CustomerId, LoanRequestId, DateTime.UtcNow, Memo);

            var rsp = await _loanRequestService.RecheckedLoanRequestAsync(req);

            if (rsp.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Success, rsp.Data);
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

            var req = new RecheckedRequestDtoCreate(BranchId, CustomerId, LoanRequestId, DateTime.UtcNow, Memo);

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