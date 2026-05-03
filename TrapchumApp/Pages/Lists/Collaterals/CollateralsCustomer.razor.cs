using Applications.Extensions;
using Radzen.Blazor;
using Applications.Services.Lists;
using Applications.Features.Collaterals.Dtos;
using Applications.Features.Collaterals.Queries.GetCollateralLineByCollateralId;

namespace TrapchumApp.Pages.Lists.Collaterals
{
    public partial class CollateralsCustomer
    {

        private readonly ICollateralsServiceImpl _collateralService;
        public CollateralsCustomer(ICollateralsServiceImpl collateralService)
        {
            _collateralService = collateralService;
        }

        [Parameter] public string CustomerId { get; set; } = string.Empty;
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



        async void AddOne()
        {
            if (CustomerId.IsNullOrEmpty())
            {
                NotificationService.Notify(NotificationSeverity.Error, "Please selected customer first");
                return;
            }

            var parameters = new Dictionary<string, object?> {

            {"CollateralId", CustomerId }
        };

            var options = new DialogOptions
            {
                Width = "50vw"
            };

            var dia = await DialogService.OpenAsync<CustomerCollateralLineCreate>("Add Collateral", parameters, options);

            if (dia is bool result && result == true)
            {
                await GetCustomerCollaterals();
            }
        }


        async Task GetCustomerCollaterals()
        {

            var resp = await mediator.Send(new GetCollateralLineByCollateralIdQuery(CustomerId));

            if (!resp.IsError)
            {
                CollateralLines = resp.Value;
            }

            await refGrid.Reload();

            StateHasChanged();
        }


        async Task OnCollateralLineEdit(VCollateralLines arg)
        {

            ContextMenuService.Close();

            var options = new DialogOptions
            {
              
            };

            var parameters = new Dictionary<string, object?>
            {
                {"Id",arg.Id },
                {"CollateralId",arg.CollateralId }
            };

            var dia = await DialogService.OpenAsync<CustomerCollateralLineEdit>("Edit", parameters, options: options);

            if (dia is bool result && result == true)
            {
                await GetCustomerCollaterals();
            }

        }
    }
}