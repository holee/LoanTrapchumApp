using Applications.Extensions;
using Applications.Responses.Lists.Collaterals;
using Applications.Responses.ViewModels.Collaterals;
using Applications.Services.LIsts;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace TrapchumApp.Components.Pages.Lists.Collaterals
{
    public partial class CollateralsCustomer
    {

        private readonly ICollateralsServiceImpl _collateralService;

        public CollateralsCustomer(ICollateralsServiceImpl collateralService)
        {
            _collateralService = collateralService;
        }

        [Parameter] public string? CustomerId { get; set; } = string.Empty;
        [Parameter] public bool ShowAdd { get; set; } = true;
        [Parameter] public bool ShowCommand { get; set; } = false;
        [Parameter] public bool ShowAcction { get; set; } = false; 
        [Parameter] public EventCallback Reload { get; set; }

        IEnumerable<VCollateralLines> CollateralLines = [];

        RadzenDataGrid<VCollateralLines> refGrid = default!;
        protected override async Task OnInitializedAsync()
        {
            await GetCustomerCollaterals();
            await base.OnInitializedAsync();
        }

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

            if (result == true)
            {
                await GetCustomerCollaterals();
            }
        }

        async void CollateralReload()
        {
            await Reload.InvokeAsync();

            await GetCustomerCollaterals();

        }

        async Task GetCustomerCollaterals()
        {
            var resp = await _customerService.GetCustomerCollateralLines(CustomerId, true);

            if (resp.IsSuccess)
            {
                CollateralLines = resp.Data!;
            }
            await refGrid.Reload();
            StateHasChanged();
        }
    }
}