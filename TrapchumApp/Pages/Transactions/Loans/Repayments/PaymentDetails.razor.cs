using Applications.Calculations;
using Applications.Enumerations;
using Applications.Requests.Loans.Payments;
using Applications.Responses.ViewModels.Loans;
using Applications.Responses.ViewModels.Loans.Schedules;
using Applications.Services.Loans;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Radzen;

namespace TrapchumApp.Pages.Transactions.Loans.Repayments;
public partial class PaymentDetails
{
    readonly ILoanServiceImpl _dService;

    public PaymentDetails(ILoanServiceImpl dService)
    {
        _dService = dService;
    }

    [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;
    [Parameter] public int BranchId { get; set; }
    [Parameter] public string? LoanId { get; set; }
    [Parameter, SupplyParameterFromQuery] public int CurrencyId { get; set; }
    [Parameter, SupplyParameterFromQuery] public string? CreditAgentId { get; set; }
    [Parameter, SupplyParameterFromQuery] public string? UserId { get; set; }
   
    private VLoanDisbursments? loanSchedule = new();

    IList<VSchedule> SelectedSchedules = [];

    bool collapse = false;
    string collapseIcon = "expand_more";
    bool isVible = false;
    int selectedTabIndex = 0;
    string receiptNo = "";
    bool ShowAll = false;
    void SelectedTabChanged(int index)
    {
        selectedTabIndex = index;
    }

    void CellRender(DataGridCellRenderEventArgs<VSchedule> args)
    {
        if (args.Data.IsPaid)
        {
            args.Attributes.Add("style", "background-color:var(--rz-danger-lighter)");
        }

    }

    void ShowPaidSchedules()
    {
        ContextMenuService.Close();
        selectedTabIndex = 1;
    }

    async void PayBySchedules(VSchedule p)
    {
        ContextMenuService.Close();

        List<LoanPaymentLineDtoCreate> loanRepaymentLines = [];

        if (p.IsPaid)
        {
            return;
        }

        var schedules = loanSchedule!.Schedules!.ToList();

        int selectedIndex = schedules.IndexOf(p);

        if (selectedIndex > 0)
        {
            var preSchedule = schedules[selectedIndex - 1];

            if (!preSchedule.IsPaid)
            {
                NotificationService.Notify(NotificationSeverity.Info, "Previous schedule not pay yet.");

                return;
            }
        }
        var options = new DialogOptions { };

        var paymentCreate = p.ToPaymentDtoCreate();

        var line = p.ToPaymentLine(paymentCreate);

        loanRepaymentLines.Add(line);
        PaymentCreate create = new PaymentCreate();
        create.TotalAmountWithPenalty = loanRepaymentLines.Total();
        create.OriginalPaidAmount = loanRepaymentLines.TotalAmountWithPenalty();
        create.TotalAmountNoPenalty = loanRepaymentLines.TotalAmountWithPenalty();
        create.PenaltyAmount = loanRepaymentLines.Sum(x => x.PaidPenaltyAmount);
        create.OriginalPenaltyAmount = loanRepaymentLines.Sum(x => x.PaidPenaltyAmount);
        create.RecieptNumber = "";
        if (CurrencyId == 1)
        {
            create.RecieveKhrAmount = loanRepaymentLines.Total();
        }
        else
        {
            create.RecieveAmount = loanRepaymentLines.Total();
        }
        create.ChangeAmount = 0;
        create.ChangeKhrAmount = 0;
        var data = new Dictionary<string, object?>
        {
                {"LoanRepaymentLines" , loanRepaymentLines},
                {"PaymentCreate" ,create},
                {"CurrencyId" , CurrencyId},
                {"LoanId" , LoanId},
                {"CreditAgentId" , CreditAgentId},
                { "BranchId",BranchId },
                { "ReceiptNo",receiptNo },
                { "CustomerName",p.AltName},
                {"UserId",UserId}
        };


        bool confirm = await DialogService.OpenAsync<PaymentModal>("Confirm", data, options);

        if (confirm)
        {
            await GetLoanSchedulesById(ScheduleStatus.Unpaid);
        }
    }
    void GetPaymentSchedules()
    {
        ContextMenuService.Close();

        NavigationManager.NavigateTo($"/App/Loans/{BranchId}/Branches/{LoanId}/Schedules?CreditAgentId={CreditAgentId}&CurrencyId={CurrencyId}&UserId={UserId}");
        return;
    }


    async Task OnChanged(bool args)
    {
        if (args)
        {
            await GetLoanSchedulesById(ScheduleStatus.All);
        }
        else
        {
            await GetLoanSchedulesById(ScheduleStatus.Unpaid);
        }
    }

    async Task GetLoanSchedulesById(ScheduleStatus status = ScheduleStatus.All)
    {
        isVible = true;

        var resultResponse = await _dService.GetLoanByIdAsync(LoanId, BranchId, status,true,default);

        if (resultResponse.IsSuccess)
        {
            loanSchedule = resultResponse.Data!;
        }
        else
        {
            loanSchedule = new();

            NotificationService.Notify(NotificationSeverity.Info, resultResponse.ErrMessage);
        }

        isVible = false;

        StateHasChanged();
    }




    async Task ShowScheduleDetails(VSchedule p)
    {
        //await popup.ToggleAsync(buttonItem.Element);
        ContextMenuService.Close();
        var param = new Dictionary<string, object?>
            {
                {"LoanId",LoanId },
                {"CurrencyId",CurrencyId },
                {"BranchId",BranchId },
                {"ScheduleId",p.LoanScheduleId },
            };
        var options = new SideDialogOptions
        {
            Position = DialogPosition.Right,
            ShowMask = true,
            CloseDialogOnOverlayClick = true,
            Width = "60vw"
        };
        await DialogService.OpenSideAsync<ScheduleDetails>($"Schedule-{p.LoanScheduleId}", parameters: param, options);


    }




    protected override async Task OnInitializedAsync()
    {
        try
        {
            await GetLoanSchedulesById(ScheduleStatus.Unpaid);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException!.Message);
        }
        await base.OnInitializedAsync();
    }

    private void DownLoadScheduleAsExcel(string loanRefId)
    {
        //var stream = await _dService.DownLoadScheduleAsExcel(loanRefId, BranchId);
        //using var streamRef = new DotNetStreamReference(stream: stream);
        //await js.InvokeVoidAsync("downloadFileFromStream", "Schedule.xlsx", streamRef);
    }

    private void DownLoadScheduleAsPdf(string loanRefId)
    {
        //var stream = await _dService.DownLoadScheduleAsPdf(loanRefId, BranchId);

        //using var streamRef = new DotNetStreamReference(stream: stream);

        //await js.InvokeVoidAsync("downloadFileFromStream", "Schedule.pdf", streamRef);
    }
}
