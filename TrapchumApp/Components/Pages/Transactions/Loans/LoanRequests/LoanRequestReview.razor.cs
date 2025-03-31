using Applications.Extensions;
using Applications.Extensions.Identity;
using Applications.Extensions.Maths;
using Applications.Extensions.NumberExtions;
using Applications.Interfaces.Identity;
using Applications.Requests.Loans.LoanRequests;
using Applications.Responses.ViewModels.Loans.Requests;
using Applications.Services.Loans;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;

namespace TrapchumApp.Components.Pages.Transactions.Loans.LoanRequests
{
    public partial class LoanRequestReview
    {
        [CascadingParameter] Task<AuthenticationState> State { get; set; } = default!;
        [Parameter, EditorRequired] public Guid? LoanRequestId { get; set; }
        [Parameter, SupplyParameterFromQuery] public Guid? CustomerId { get; set; }
        [Parameter, SupplyParameterFromQuery] public Guid? UserId { get; set; }
        [Parameter, SupplyParameterFromQuery] public int BranchId { get; set; }
        VLoanRequests? data = new();
        string Memo = "";
        decimal? aprovedAmount = 0;
        decimal? adminFeeAmount = 0;
        Guid? requestUserId = Guid.Empty;
        string? requestUserName = string.Empty;
        string? aprovedUserId = string.Empty;
        string aprovedUserName = string.Empty;
        bool success = false;

        protected override async Task OnParametersSetAsync()
        {
            await GetLoanRequestDetails();
            await base.OnParametersSetAsync();
        }
        protected override async Task OnInitializedAsync()
        {
            var auth = await State;

            aprovedUserId = auth.User.GetId();

            var resp = await _userService.GetUserDetailsById(aprovedUserId);

            if (resp.IsSuccess)
            {
                aprovedUserName = resp.Data!.UserName;
            }

            await base.OnInitializedAsync();

        }

        void CalculateAdminFeeAmount(ChangeEventArgs e)
        {
            var value = e.Value?.ToNumer<decimal>();

            if (data!.CurrencyId == 1)
            {
                adminFeeAmount = ((data.AdminFeeRate / 100) * value).RoundUpKhr(2);
            }
            else
            {
                adminFeeAmount = ((data.AdminFeeRate / 100) * value).RoundUp(2);
            }

        }

        async Task GetLoanRequestDetails()
        {
            var resp = await _loanRequestService.GetLoanRequestsAsync(BranchId, CustomerId, LoanRequestId, true);

            if (resp.IsSuccess)
            {
                data = resp.Data;
                aprovedAmount = data?.RequestAmount;
                requestUserId = data?.UserRequestId?.ToGuid();
                requestUserName = data?.UserName;
                adminFeeAmount = data?.AdminFeeAmount;
                success = (data?.StatusId == 2 || data?.StatusId == 3) ? false : true;
            }
        }

        async Task AprovedLoanRequest()
        {

            var confirm = await DialogService.Confirm("Do you want to aprove this request?", "Loan Request");

            if (confirm == false)
            {
                NotificationService.Notify(NotificationSeverity.Info, "Reuqest wa cancel.");

                return;
            }

            var options = new DialogOptions
            {

            };
            var param = new Dictionary<string, object?>
        {
            {"AltName",data!.AltName},
            {"BranchName",data!.BranchName},
            {"RequestUserId",requestUserId},
            {"RequestUserName",requestUserName},
            {"AprovedUserId",aprovedUserId},
            {"AprovedUserName",aprovedUserName},
            {"LoanRequestId",LoanRequestId},
            {"AdmindFeeAmount",adminFeeAmount},
            {"AprovedAmount",aprovedAmount},
            {"BranchId",BranchId},
            {"CustomerId",CustomerId}
        };

            bool dialogueResult = await DialogService.OpenAsync<ConfirmModal>("Confirm", param, options);

            if (dialogueResult)
            {
                success = true;
                NavigationManager.NavigateTo($"/Loan/Request>BranchId={BranchId}&UserId={UserId}");
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

            var req = new RequestDtoBaseCreate(BranchId, CustomerId, LoanRequestId, Memo);

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