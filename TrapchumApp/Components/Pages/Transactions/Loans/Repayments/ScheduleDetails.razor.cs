using Applications.Responses.ViewModels.Loans.Payments;
using Applications.Responses.ViewModels.Loans.Schedules;
using Microsoft.AspNetCore.Components;

namespace TrapchumApp.Components.Pages.Transactions.Loans.Repayments;
public partial class ScheduleDetails
{

    [Parameter, EditorRequired] public int BranchId { get; set; } = 0;
    [Parameter, EditorRequired] public int CurrencyId { get; set; } = 1;
    [Parameter, EditorRequired] public long LoanId { get; set; } = 0;
    [Parameter, EditorRequired] public long ScheduleId { get; set; } = 0;

    VSchedule schedule = new();

    IEnumerable<VPaymentLines> PaymentLines = [];

    protected override async Task OnInitializedAsync()
    {
        //var resp = await _scheduleService.GetScheduleByIdAsync(LoanId, BranchId, ScheduleId, (int)ScheduleStatus.All);

        //if (resp.IsSuccess)
        //{
        //    schedule = resp.Data!;

        //    PaymentLines = schedule?.PaymentLines != null ? schedule!.PaymentLines : [];
        //}

        await base.OnInitializedAsync();
    }






}
