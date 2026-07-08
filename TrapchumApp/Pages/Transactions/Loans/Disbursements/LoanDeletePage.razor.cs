using Applications.Exceptions;
using Applications.Features.Customers.Response;
using Applications.Features.Loans.Command.EditLoanDisbursment;
using Applications.Features.Loans.Dtos;
using Applications.Features.Loans.Mappers;
using Applications.Features.Loans.Queries.GetLoanById;
using Applications.Features.Loans.Queries.GetLoanForEdit;
using Applications.Responses.ViewModels.Loans.Requests;
using Domains.Enumerations;
using Domains.Models.Loans;
using TrapchumApp.Shared;

namespace TrapchumApp.Pages.Transactions.Loans.Disbursements;

public partial class LoanDeletePage
{

    [Parameter] public int BranchId { get; set; }
    [Parameter, SupplyParameterFromQuery] public string? CustomerId { get; set; } = string.Empty;

    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationState { get; set; } = default!;

    [CascadingParameter]
    private CascadingAppState BState { get; set; } = default!;
    [Parameter] public string? LoanId { get; set; } = string.Empty;

    [PersistentState]
    public LoanDtoCreate? _model { get; set; }

    private const int FixedTabCount = 3;

    private int selectedTabIndex;

    private bool IsLoading;

    private bool isWaiting = false;

    private string? _userId = string.Empty;

    private decimal penaltyRate;

    private List<CustomTab> _tabs = [];

    protected EditForm editForm = default!;

    private VLoanRequests req = new();

    private VCustomerDetails _customer = new();

    private LoanDisbursment? resource = null;

    private string? Note = string.Empty;
    protected override async Task OnInitializedAsync()
    {
        try
        {
            await GetLoanEdit();

            await BState.AllowApproveDeleteLoanDisbursment(resource);

            await BState.AllowDeleteLoanDisbursment(resource); 

        }
        catch (Exception ex)
        {
            _toastService.ShowError($"An error occurred while loading the loan request due to {ex.Message}");
        }

        await base.OnInitializedAsync();
    }



    private async Task GetLoanEdit()
    {

        try
        {
            IsLoading = true;

            if (_model is null)
            {
                var query = new GetLoanForEditQuery(LoanId, BranchId);

                var rsp = await mediator.Send(query);

                _model = rsp.Value;

                ChangedSchedule(_model.RepaymentModeRefId);
            }

            var command = new GetLoanByIdQuery(LoanId, BranchId);

            var result = await mediator.Send(command);

            if (result.IsSuccess())
            {
                resource = result.Value;
            }

            IsLoading = false;
        }
        catch (MediatorException ex)
        {
            IsLoading = false;

            _toastService.ShowError($"An error occurred while loading the loan request due to {ex.ErrorMessage}");
        }
        catch (Exception ex)
        {
            IsLoading = false;

            _toastService.ShowError($"An error occurred while loading the loan request due to {ex.Message}");
        }

        StateHasChanged();

    }

    private void ChangedSchedule(int? arg)
    {
        if (_model is not null)
        {
            _model.ScheduleMode = arg switch
            {
                1 => AdjustmentOption.Before,

                2 => AdjustmentOption.After,

                _ => AdjustmentOption.Skip
            };
        }


    }

    private async Task HandleFormEdit()
    {

        isWaiting = true;

        if (_model is null)
        {
            _toastService.ShowError("Loan model is not initialized.");

            isWaiting = false;

            return;
        }

        var confirmed = await MsgBox.ConfirmAsync("Do you want to disburse this loan request?", "Confirm");


        if (confirmed == MsgBoxResult.No)
        {
            _toastService.ShowError("Loan disburse was cancelled.");

            isWaiting = false;

            return;
        }

        var scheduleDto = _calSceduleService.Calculate(_model).MapToLoanSchedules();

        _model.LastPaymentDate = scheduleDto!.LastOrDefault()!.PaymentDate;

        try
        {
            editForm.EditContext?.Validate();

            _model.LoanSchedules = scheduleDto.ToList();

            var command = new EditLoanDisbursmentCommand(_model);

            var rsp = await mediator.Send(command);

            if (!rsp.IsSuccess())
            {

                _toastService.ShowError(rsp.GetMessage());

                isWaiting = false;

                return;
            }

            _toastService.ShowSuccess(rsp.Value.Message);

            isWaiting = false;

            string loanUrl = $"/App/Loan/{rsp.Value.LoanId}/Branches/{BranchId}/Currencies/{_model.CurrencyId}";

            NavigationManager.NavigateTo(loanUrl, true);

            StateHasChanged();
        }
        catch (MediatorException ex)
        {
            _toastService.ShowError($"An error occurred while disbursing the loan request due to {ex.ErrorMessage}");

            isWaiting = false;
        }
        catch (Exception ex)
        {
            _toastService.ShowError($"An error occurred while disbursing the loan request due to {ex.InnerException?.Message}");

            isWaiting = false;
        }
    }

}