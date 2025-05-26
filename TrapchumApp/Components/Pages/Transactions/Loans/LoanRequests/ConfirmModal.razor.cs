using Applications.Requests.Loans.LoanRequests;
using Applications.Responses.ViewModels.Loans.Requests;
using Mapster;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace TrapchumApp.Components.Pages.Transactions.Loans.LoanRequests
{
    public partial class ConfirmModal
    {
        [Parameter] public string? CustomerId { get; set; }
        [Parameter] public int BranchId { get; set; } = 0;
        [Parameter] public string? LoanRequestId { get; set; }
        [Parameter] public string? UserId { get; set; }=string.Empty;

        ApprovedRequest? Data = new();

        AproveLoanDtoCreate? model=new();

        async Task HandleFormRequest()
        {

            var confirm = await DialogService.Confirm("Are you sure to aprove this loan request", "Loan Request");

            if (confirm == true && model is not null)
            {

                var resp = await _loanRequestService.AprovedLoanRequestAsync(model);

                if (resp.IsSuccess)
                {

                    var description = new LoanDescriptionsDtoCreate(model.Memo, UserId, LoanRequestId);

                   await _loanRequestService.AddDescriptionAsync(description);

                    NotificationService.Notify(NotificationSeverity.Success, resp.Data);

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
            var rsp= await _loanRequestService.GetLoanRequestAsync<ApprovedRequest>(BranchId,CustomerId,LoanRequestId);

            if (rsp.IsSuccess) 
            {
                Data = rsp.Data;

                model = Data!.Adapt<AproveLoanDtoCreate>();   
            }
        }

    }
}