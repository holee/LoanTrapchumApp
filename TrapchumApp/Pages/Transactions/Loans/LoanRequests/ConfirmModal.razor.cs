using Applications.Commons.Mappers.LoanRequests;
using Applications.Features.LoanRequests.Command.AddDescription;
using Applications.Features.LoanRequests.Command.ApprovedLoanRequest;
using Applications.Features.LoanRequests.Queryies.GetLoanRequestBy;
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

                var command = new ApprovedLoanRequestCommand(model);

                var resp = await mediator.Send(command);// _loanRequestService.AprovedRequestAsync(model);

                if (!resp.IsError)
                {

                    var description = new LoanDescriptionsDtoCreate(model.Memo, UserId, LoanRequestId);

                    var cmd = new AddDescriptionCommand(description);

                    await mediator.Send(cmd);

                    NotificationService.Notify(NotificationSeverity.Success, resp.Value.Message);

                    DialogService.Close(true);
                }
                else
                {

                    NotificationService.Notify(NotificationSeverity.Error, resp.FirstError.Description);
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

            var query = new GetLoanRequestApprovedQuery(BranchId, CustomerId, LoanRequestId);

            var rsp = await mediator.Send(query);

            if (!rsp.IsError)
            {
                Data = rsp.Value;

                model = rsp.Value.ApprovedLoanRequestDtoCreate();
            }
        }

    }
}