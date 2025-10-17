using Applications.Features.Customers.UseCases;
using Applications.Responses.Lists.Collaterals;
using Applications.Responses.ViewModels.Customers;
using Applications.Services.Lists;

namespace TrapchumApp.Pages.Lists.Collaterals
{
    public partial class CollateralDetail 
    {
        private readonly ICollateralsServiceImpl _collateralsService;
        private readonly CustomerCollateralServices _custCollateralServices;
        public CollateralDetail(ICollateralsServiceImpl collateralsService, CustomerCollateralServices custCollateralServices)
        {
            _collateralsService = collateralsService;
            _custCollateralServices = custCollateralServices;
        }

        [Parameter] public EventCallback Close { get; set; }
        [Parameter] public EventCallback<string?> AddMoreCollateral { get; set; }
        [Parameter] public VCustomerCollaterals? Model { get; set; } = new();
        async Task CloseTab()
        {
            await Close.InvokeAsync();
        }
        [Parameter] public string? CustomerId { get; set; }
        [Parameter] public LoanCustomerQuery? SelectedCustomer { get; set; }
        [Parameter] public List<VCollateralLines>? CollateralLines { get; set; } = [];

        async Task GetCollateralById(string collateralId)
        {
            var rsp = await _custCollateralServices.GetCustomerCollateralAsync(collateralId);

            if (rsp.IsSuccess)
            {
                Model = rsp.Data;

                //CollateralLines = rsp.Data!.CollateralLines;
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error,rsp.ErrMessage);
            }


        }

        protected override async Task OnParametersSetAsync()
        {
            await GetCollateralById(CustomerId!);

            await base.OnParametersSetAsync();
        }


       
    }
}