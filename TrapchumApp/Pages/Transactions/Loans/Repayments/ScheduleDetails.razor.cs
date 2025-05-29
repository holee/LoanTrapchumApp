using Applications.Responses.ViewModels.Loans.Payments;
using Microsoft.AspNetCore.Components;

namespace TrapchumApp.Pages.Transactions.Loans.Repayments;
public partial class ScheduleDetails
{

    [Parameter] public int BranchId { get; set; } = 0;
    [Parameter] public int CurrencyId { get; set; } = 1;
    [Parameter] public string? LoanId { get; set; } = string.Empty;
    [Parameter] public long ScheduleId { get; set; } = 0;
    [Parameter] public ScheduleVm Schedule { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {

        if (Schedule is { LoanScheduleId: 0 })
        {

        }

        await base.OnInitializedAsync();
    }

}
