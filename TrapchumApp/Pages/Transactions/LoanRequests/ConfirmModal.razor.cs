using Applications.Commons.Mappers.LoanRequests;
using Applications.Features.LoanRequests.Command.AddDescription;
using Applications.Features.LoanRequests.Command.ApprovedLoanRequest;
using Applications.Features.LoanRequests.Queryies.GetLoanRequestBy;
using Applications.Features.LoanRequests.Requests;
using Applications.Responses.ViewModels.Loans.Requests;

namespace TrapchumApp.Pages.Transactions.LoanRequests
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

            var dia = await MsgBox.ShowAsync("Processing your request. Please wait...", "Loan Request", MsgBoxButtons.YesNo, MsgBoxBoxIcon.Information);

            if (dia == MsgBoxResult.No)
            {
                _toastService.ShowInfo("Request was cancelled.");

                return;
            }

            logger.LogInformation("Start to aprove loan request with id {LoanRequestId} by user {UserId}", LoanRequestId, _userId);

            try
            {
                isSaving = true;

                var command = new ApprovedRequestCommand(model);

                var resp = await mediator.Send(command);

                if (resp.IsSuccess())
                {
                    var description = new LoanDescriptionsDtoCreate(model.Memo, _userId, LoanRequestId);

                    var cmd = new AddDescriptionCommand(description);

                    await mediator.Send(cmd);

                    _toastService.ShowSuccess("Successfully approved loan request");

                    isSaving = false;

                    logger.LogInformation("Successfully approved loan request with id {LoanRequestId} by user {UserId}", LoanRequestId, _userId);

                    DialogService.Close(true);
                }
                else
                {

                    logger.LogInformation("Failed to approve loan request with id {LoanRequestId} by user {UserId}. Error: {ErrorDescription}", LoanRequestId, _userId, resp.FirstError.Description);

                    isSaving = false;

                    _toastService.ShowError(resp.GetMessage());
                }
            }
            catch (Exception ex)
            {
                isSaving = false;

                logger.LogError(ex, "Error while approving loan request with id {LoanRequestId} by user {UserId}", LoanRequestId, _userId);
                
                _toastService.ShowError("An error occurred while approving the loan request.");

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