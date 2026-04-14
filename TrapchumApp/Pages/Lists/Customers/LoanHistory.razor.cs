using Applications.Features.Customers.Queries.GetLoanByCustomerId;
using Applications.Features.Loans.Dtos.Responses;
using TrapchumApp.Shared;

namespace TrapchumApp.Pages.Lists.Customers;

public partial class LoanHistory
{
    [CascadingParameter] public CascadingAppState bState { get; set; } = default!;

    [Parameter] public string? CustomerId { get; set; }

    [PersistentState]
    public IEnumerable<VLoanDisbursments>? VLoanDisbursements { get; set; }

    protected override async Task OnInitializedAsync()
    {
        if (VLoanDisbursements is null)
        {
            var rsp = await mediator.Send(new GetLoanByCustomerIdQuery(CustomerId));
            
            VLoanDisbursements = rsp.Items;
        }

        await base.OnInitializedAsync();
    }
}