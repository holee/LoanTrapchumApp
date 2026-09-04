using Applications.Features.Schedules.Dto;
using Applications.Features.Schedules.Queries.GetScheduleDetailById;

namespace Web.UI.Pages.Transactions.Loans.Repayments;

public partial class ScheduleDetailsPage
{

    [Parameter] public int BranchId { get; set; } = 0;
    [Parameter] public int CurrencyId { get; set; } = 1;
    [Parameter] public string? LoanId { get; set; } = string.Empty;
    [Parameter] public long ScheduleId { get; set; } = 0;
    [Parameter] public ScheduleDetailsDto Schedule { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    protected override async Task OnParametersSetAsync()
    {

        var param = new GetScheduleDetailByIdQuery(BranchId, LoanId, ScheduleId);

        var rsp = await mediator.Send(param);

        if (rsp.IsSuccess())
        {
            Schedule = rsp.Value;
        }

        await base.OnParametersSetAsync();
    }



}
