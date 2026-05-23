using Applications.Commons.Mappers.LoanRequests;
using Applications.Features.LoanRequests.Command.AddDescription;
using Applications.Features.LoanRequests.Command.ApprovedLoanRequest;
using Applications.Features.LoanRequests.Command.PassRequest;
using Applications.Features.LoanRequests.Queryies.GetLoanRequestBy;
using Applications.Features.LoanRequests.Requests;
using Applications.Features.Settings.Queries.GetApprovedUsers;
using Applications.Responses.ViewModels.Loans.Requests;
namespace TrapchumApp.Pages.Transactions.LoanRequests;

public partial class PassToModal(ILogger<ConfirmModal> logger)
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

        //await GetApprovedUser();

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

            var command = new PassRequestCommand(new PassRequestDtoCreate
            {
                BranchId = BranchId,
                LoanRequestId = LoanRequestId,
                CustomerId = CustomerId,
                Description = model.Memo,
            });

            var resp = await mediator.Send(command);

            if (resp.IsSuccess())
            {

                _toastService.ShowSuccess(resp.Value.Message!);

                logger.LogInformation("Successfully approved loan request with id {LoanRequestId} by user {UserId}", LoanRequestId, _userId);

                isSaving = false;

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

    private async Task GetApprovedUser()
    {

        var command = new GetApprovedUserQuery(4003, null, null, null);

        var rsp = await mediator.Send(command); 

        AprovedUsers = rsp.Value ?? [];
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
