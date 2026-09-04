using Applications.Extensions;
using Applications.Features.Collaterals.Dtos;
using Applications.Features.Customers.Queries.GetCustomerCollateralLine;
using Radzen.Blazor;

namespace Web.UI.Pages.Lists.Collaterals
{
    public partial class CollateralLoanRequest
    {
        [Parameter] public string? CustomerId { get; set; } = string.Empty;
        [Parameter] public IList<VCollateralLines> SelectedCollateralLines { get; set; } = [];

        IEnumerable<VCollateralLines> CollateralLines = [];

        bool allowRowSelectOnRowClick = false;

        RadzenDataGrid<VCollateralLines> grid = default!;
        protected override async Task OnInitializedAsync()
        {
            await GetCustomerCollaterals();

            await base.OnInitializedAsync();
        }

        bool HeaderChecked => !SelectedCollateralLines.Any() ? false : !CollateralLines.All(i => SelectedCollateralLines.Contains(i)) ? false : CollateralLines.Any(i => SelectedCollateralLines.Contains(i));

        async void Add()
        {
            if (CustomerId.IsNullOrEmpty())
            {
                NotificationService.Notify(NotificationSeverity.Error, "Please selected customer first");
                return;
            }
            var parameters = new Dictionary<string, object?> {

            {"CustomerId", CustomerId }
        };

            var options = new DialogOptions
            {
                Width = "90vw",
                Draggable = true,
                Resizable = false
            };


            bool result = await DialogService.OpenAsync<CollateralModalAdd>("Add Collateral", parameters!, options);

            if (bool.TryParse(result.ToString(), out bool success) && success)
            {
                await GetCustomerCollaterals();
            }
        }

        async Task GetCustomerCollaterals()
        {
            var resp = await mediator.Send(new GetCustomerCollateralLinesQuery(CustomerId));

            CollateralLines = resp.Items;

            StateHasChanged();
        }


    }
}