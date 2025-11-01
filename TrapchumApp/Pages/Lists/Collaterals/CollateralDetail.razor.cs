using Applications.Features.Collaterals.Dtos;
using Applications.Features.Collaterals.Queries.GetCollateralDetails;
using Applications.Responses.ViewModels.Customers;
using Applications.Services.Lists;

namespace TrapchumApp.Pages.Lists.Collaterals
{
    public partial class CollateralDetail 
    {
        private readonly ICollateralsServiceImpl _collateralsService;

        public CollateralDetail(ICollateralsServiceImpl collateralsService)
        {
            _collateralsService = collateralsService;
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

            var query = new GetCollateralDetailQuery(collateralId);


            var rsp = await mediator.Send(query);

            if (!rsp.IsError)
            {
                Model = rsp.Value;
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error,rsp.FirstError.Description);
            }


        }

        protected override async Task OnParametersSetAsync()
        {
            await GetCollateralById(CustomerId!);

            await base.OnParametersSetAsync();
        }


       
    }
}