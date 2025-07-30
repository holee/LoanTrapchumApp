using Applications.Extensions;
using Applications.Features.Collaterals.Services;
using Applications.Responses.Lists.Collaterals;
using Applications.Services.LIsts;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace TrapchumApp.Pages.Lists.Collaterals
{
    public partial class CollateralsCustomer
    {

        private readonly ICollateralsServiceImpl _collateralService;

        private readonly CollateralServices _collateralsService;
        public CollateralsCustomer(ICollateralsServiceImpl collateralService, CollateralServices collateralsService)
        {
            _collateralService = collateralService;
            _collateralsService = collateralsService;
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
                Width = "100vw",
                Height = "100vh"
            };

            var dia = await DialogService.OpenAsync<CollateralModalAdd>("Add Collateral", parameters, options);

            if (dia is bool result && result == true)
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
            var resp = await _collateralsService.GetCustomerCollateralLines(CustomerId);

            if (resp.IsSuccess)
            {
                CollateralLines = resp.Data ?? [];
            }

            await refGrid.Reload();

            StateHasChanged();
        }

        void OnCollateralLineEdit(VCollateralLines arg)
        {
            var options = new SideDialogOptions
            {
                Width = "40rem"
            };

            var parameters = new Dictionary<string, object?>
            {
                {"Id",arg.Id },
                {"CollateralId",arg.CollateralRefId }
            };

            DialogService.OpenSide<CustomerCollateralLineEdit>("Collateral", parameters, options: options);
        }
    }
}