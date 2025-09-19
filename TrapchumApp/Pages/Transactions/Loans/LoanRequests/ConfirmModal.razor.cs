using Applications.Features.LoanRequests.Requests;
using Applications.Responses.ViewModels.Loans.Requests;
using Mapster;
using TrapchumApp.Helpers.Extensions;

namespace TrapchumApp.Pages.Transactions.Loans.LoanRequests
{
    public partial class ConfirmModal
    {
        [CascadingParameter] private Task<AuthenticationState> AuthenticationState { get; set; } = default!;

        [Parameter] public string? CustomerId { get; set; }
        [Parameter] public int BranchId { get; set; } = 0;
        [Parameter] public string? LoanRequestId { get; set; }
        [Parameter] public string? UserId { get; set; } = string.Empty;

        ApprovedRequest? Data = new();

        AproveLoanDtoCreate? model = new();

        EditForm? refEditForm;


        protected override async Task OnInitializedAsync()
        {
            if (!string.IsNullOrWhiteSpace(UserId))
            {
                UserId = (await AuthenticationState).GetUserId();
            }

            await base.OnInitializedAsync();
        }



        async Task HandleFormRequest()
        {


            if (!refEditForm!.EditContext!.Validate())
                return;

            var confirm = await DialogService.Confirm("Are you sure to aprove this loan request", "Loan Request");

            if (confirm == true && model is not null)
            {

                var resp = await _loanRequestService.AprovedRequestAsync(model);

                if (resp.IsSuccess)
                {

                    var description = new LoanDescriptionsDtoCreate(model.Memo, UserId, LoanRequestId);

                    await _loanRequestService.AddDescriptionAsync(description);

                    NotificationService.Notify(NotificationSeverity.Success, resp.Data!.message);

                    DialogService.Close(true);
                }
                else
                {

                    NotificationService.Notify(NotificationSeverity.Error, resp.ErrMessage);
                }
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            await GetLoanRequest();

            await base.OnParametersSetAsync();
        }

        async Task GetLoanRequest()
        {
            var rsp = await _loanRequestService.GetLoanRequestAsync<ApprovedRequest>(BranchId, CustomerId, LoanRequestId);

            if (rsp.IsSuccess)
            {
                Data = rsp.Data;

                model = Data!.Adapt<AproveLoanDtoCreate>();
            }
        }

    }
}