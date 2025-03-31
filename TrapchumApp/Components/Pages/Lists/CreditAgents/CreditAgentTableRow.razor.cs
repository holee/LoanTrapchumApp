using Applications.Requests.Loans.CreditAgents;
using Applications.Responses.ViewModels.Customers;
using Applications.Services.Lists;
using Microsoft.AspNetCore.Components;

namespace TrapchumApp.Components.Pages.Lists.CreditAgents
{
    public partial class CreditAgentTableRow
    {
        [Inject] protected ICreditAgentServiceImpl CreditAgentService { get; set; } = default!;
        [Parameter] public int Index { get; set; } = 1;
        [Parameter] public VCreditAgent CreditAgent { get; set; } = default!;
        [Parameter] public EventCallback<VCreditAgent> Remove { get; set; }

        async Task OnCheckChanged(bool x)
        {
            CreditAgent.IsRemove = x;

            var creditDto = new LoanCreditAgentDtoActivate
            {
                IsRemove = x,
                EditSeq = CreditAgent.EditSeq,
                EmployeeRefId = CreditAgent.EmployeeRefId,
            };
            var respnse = await CreditAgentService.ActivateOrDeactivateCreditAgent(creditDto);
            if (respnse.IsSuccess)
            {
                await Remove.InvokeAsync(CreditAgent);
            }
            else
            {
            }
        }

    }
}