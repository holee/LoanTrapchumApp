using Applications.Features.LoanRequests.Dtos;
using Radzen.Blazor;

namespace TrapchumApp.Pages.Transactions.LoanRequests;
public partial class CoRequestCustomerBox
{
    [Parameter] public IEnumerable<CoPawnCustomerDto> Customers { get; set; } = []; 
    [Parameter] public bool ShowAdd { get; set; } = false;
    [Parameter] public bool ShowCommand { get; set; } = false;
    [Parameter] public bool ShowRemove { get; set; } = false;
    [Parameter] public EventCallback Add { get; set; }
    [Parameter] public EventCallback Reload { get; set; }
    [Parameter] public EventCallback<CoPawnCustomerDto> Remove { get; set; }

    RadzenDataGrid<CoPawnCustomerDto> gridCustomers = default!;
     
    private async Task ClickedAdd()
    {
        await Add.InvokeAsync();
    }

    private async Task ClickedRemove(CoPawnCustomerDto g)
    {
        await Remove.InvokeAsync(g);
    }

    public async Task CustomerReload()
    {
        await gridCustomers.Reload();
    }

}
