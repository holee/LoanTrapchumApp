using Applications.Extensions;
using Applications.Responses.ViewModels.Collaterals;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace TrapchumApp.Components.Pages.Lists.Collaterals
{
    public partial class CollateralLoanRequest
    {
        [Parameter] public Guid? CustomerId { get; set; } = Guid.Empty;
        [Parameter] public IList<VCollateralLines> SelectedCollateralLines { get; set; } = [];
        IEnumerable<VCollateralLines> CollateralLines = [];
        bool allowRowSelectOnRowClick = true;
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


            bool result = await DialogService.OpenAsync<CollateralModalAdd>("Add Collateral", parameters, options);

            if (bool.TryParse(result.ToString(), out bool success) && success)
            {
                await GetCustomerCollaterals();
            }
        }

        async Task GetCustomerCollaterals()
        {
            var resp = await _customerService.GetCustomerCollateralLines(CustomerId);

            if (resp.IsSuccess)
            {
                CollateralLines = resp.Data!;
            }
            StateHasChanged();
        }
    }
}