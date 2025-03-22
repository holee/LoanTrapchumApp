using Applications.Extensions;
using Applications.Responses.ViewModels.Loans.Requests;
using Applications.Services.Loans;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;
namespace TrapchumApp.Components.Pages.Transactions.Loans.Disbursements;

public partial class LoanCreate
{

    private readonly ILoanRequestServiceImpl _loanRequestService;

    public LoanCreate(ILoanRequestServiceImpl loanRequestService)
    {
        _loanRequestService = loanRequestService;
    }

    [Parameter, SupplyParameterFromQuery] public int BranchId { get; set; }
    [Parameter, SupplyParameterFromQuery] public Guid? UserId { get; set; }
    [Parameter, SupplyParameterFromQuery] public Guid? CustomerRequestId { get; set; } = 0;
    [Parameter, SupplyParameterFromQuery] public Guid LoanRequestId { get; set; } = 0;
    VLoanRequests req = new VLoanRequests();
    DateTime firstPayment;
    EditForm editForm = default!;
    decimal penaltyRate = 0.0M;
    List<DateTime> Holidays = [];
    async Task GetLoanRequest()
    {
        IsLoading = true;

        var request = await _loanRequestService.GetLoanRequestByCustomerAsync(BranchId, CustomerRequestId, LoanRequestId, true);

        if (request.IsSuccess)
        {
            req = request.Data!;
        }

        IsLoading = false;
    }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            if (CustomerRequestId.NotEmpty() && LoanRequestId.NotEmpty())
            {
                await GetLoanRequest();

                model = req.ToLoanDtoCreate();
            }
        }
        catch (Exception ex)
        {
            NotificationService.Notify(NotificationSeverity.Error, ex.Message);
        }

        await base.OnInitializedAsync();
    }
    void ShowLoanPreviews()
    {
        if (model.Principle == 0) return;
        model.BankRefId = 11;
        model.TermTypeId = 11;
        model.LoanStatusRefId = 4;
        model.LoanSubTypeRefId = 1;
        model.UserRefId = UserId;
        model.PenaltyRate = penaltyRate;
        var param = model.ToLoanInfo();

        var builder = Calculators.CreateBuilder(model.RepaymentModeRefId, model.CurrencyRefId);
        {
            var app = builder.Build(param);
            previewSchedule = app.SetHoliday(Holidays)
                                 .Calculate(model.Installments, model.InterestRate, model.Principle)
                                  .ToSchedules(BranchId, model.RepaymentModeRefId);
            selectedTabIndex = 2;
        }
    }

    protected async Task HandleFormSubmit()
    {

        var confirm = await DialogService.Confirm("Do you want to disburse this loan request?", "Confirm");

        if (confirm == false)
        {
            NotificationService.Notify(NotificationSeverity.Info, "loan disburse cancel.");
            return;
        }
        model.BankRefId = 11;
        model.TermTypeId = 11;
        model.LoanStatusRefId = 4;
        model.LoanSubTypeRefId = 1;
        model.BranchRefId = BranchId;
        model.UserRefId = UserId;
        model.PenaltyRate = penaltyRate;
        var param = model.ToLoanInfo();
        var builder = Calculators.CreateBuilder(model.RepaymentModeRefId, model.CurrencyRefId);
        var app = builder.Build(param);
        var scheduleDto = app.SetHoliday(Holidays)
                             .Calculate(model.Installments, model.InterestRate, model.Principle)
                             .ToSchedules(BranchId, model.CreditAgentRefId, model.RepaymentModeRefId);
        var lastDate = scheduleDto!.LastOrDefault()!.PaymentDate;
        model.LastPaymentDate = lastDate;
        try
        {
            editForm.EditContext!.Validate();
            model.LoanSchedules = scheduleDto.ToList();
            var createResponse = await _loanRequestService!.CreateLoanAsync(model);
            if (!createResponse.IsSuccess)
            {
                var error = createResponse.Errors!.FirstOrDefault();
                NotificationService.Notify(NotificationSeverity.Error, error);
                return;
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Success, createResponse.Data!.Message);
            }
        }
        catch (Exception ex)
        {
            NotificationService.Notify(NotificationSeverity.Error, ex.Message);
        }

    }

}

