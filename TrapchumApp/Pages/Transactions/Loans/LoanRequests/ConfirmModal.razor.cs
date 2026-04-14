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
    public partial class ConfirmModal(ILogger<ConfirmModal> logger)
    {
        [CascadingParameter] private Task<AuthenticationState> AuthenticationState { get; set; } = default!;

        [Parameter] public string? CustomerId { get; set; }
        [Parameter] public int BranchId { get; set; } = 0;
        [Parameter] public string? LoanRequestId { get; set; }
        private string? _userId { get; set; } = string.Empty;

        ApprovedRequest? Data = new();

        AproveLoanDtoCreate? model = new();

        protected override async Task OnInitializedAsync()
        {
            if (!string.IsNullOrWhiteSpace(_userId))
            {
                _userId = (await AuthenticationState).GetUserId();
            }

            logger.LogInformation("Initializing ConfirmModal for loan request with id {LoanRequestId} by user {UserId}", LoanRequestId, _userId);

            await base.OnInitializedAsync();
        }



        async Task HandleFormRequest()
        {
            if (model is null) return;

            var confirm = await DialogService.Confirm("Are you sure to aprove this loan request", "Loan Request");

            if (confirm == false) return;

            logger.LogInformation("Start to aprove loan request with id {LoanRequestId} by user {UserId}", LoanRequestId, _userId);

            try
            {
                isSaving = true;

                var command = new DisburseLoanRequestCommand(model);

                var resp = await mediator.Send(command);

                if (resp.IsSuccess())
                {
                    var description = new LoanDescriptionsDtoCreate(model.Memo, _userId, LoanRequestId);

                    var cmd = new AddDescriptionCommand(description);

                    await mediator.Send(cmd);

                    NotificationService.Notify(NotificationSeverity.Success, resp.GetMessage());

                    isSaving = false;

                    logger.LogInformation("Successfully approved loan request with id {LoanRequestId} by user {UserId}", LoanRequestId, _userId);

                    DialogService.Close(true);
                }
                else
                {
                    
                    logger.LogInformation("Failed to approve loan request with id {LoanRequestId} by user {UserId}. Error: {ErrorDescription}", LoanRequestId, _userId, resp.FirstError.Description);
                   
                    NotificationService.Notify(NotificationSeverity.Error, resp.FirstError.Description);
                }
            }
            catch (Exception ex)
            {
                isSaving = false;
                logger.LogError(ex, "Error while approving loan request with id {LoanRequestId} by user {UserId}", LoanRequestId, _userId);
            }
            finally
            {
                isSaving = false;
            }

        }

        protected override async Task OnParametersSetAsync()
        {
            await GetLoanRequest();

            await base.OnParametersSetAsync();
        }

        string currencyFormat = "N2";

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