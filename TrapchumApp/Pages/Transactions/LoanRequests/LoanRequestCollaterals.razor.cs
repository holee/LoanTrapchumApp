using Applications.Responses.ViewModels.Loans.Requests;
using Radzen.Blazor;

namespace TrapchumApp.Pages.Transactions.LoanRequests
{
    public partial class LoanRequestCollaterals
    {
        [Parameter, EditorRequired] public List<VLoanRequestCollateralLines> Collaterals { get; set; } = [];
        [Parameter, EditorRequired] public string? CustomerId { get; set; }
        [Parameter] public bool ShowAdd { get; set; } = false;
        [Parameter] public bool ShowCommand { get; set; } = false;
        [Parameter] public bool ShowRemove { get; set; } = false;
        [Parameter] public EventCallback Add { get; set; }
        [Parameter] public EventCallback<VLoanRequestCollateralLines> RemoveCollateral { get; set; }

        RadzenDataGrid<VLoanRequestCollateralLines> gridColaterals = default!;
        async void ClickedAdd()
        {
            await Add.InvokeAsync();

            await gridColaterals.Reload();
        }

        async void ClickedRemove(VLoanRequestCollateralLines c)
        {
            await RemoveCollateral.InvokeAsync(c);
        }


        public async Task CollateralReload()
        {
            await gridColaterals.Reload();
        }
    }
}

