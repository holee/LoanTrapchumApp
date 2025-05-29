using Applications.Responses.Lists.Collaterals;
using Applications.Responses.ViewModels.Customers;
using Applications.Services.LIsts;
using Microsoft.AspNetCore.Components;
using Radzen;

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
        //private async Task AddCollateralByCustomer()
        //{
        //    if (Model!.CollateralId.IsNullOrEmpty())
        //    {
        //        NotificationService.Notify(NotificationSeverity.Info, "Try getting data not found!");
        //        return;
        //    }
        //    await AddMoreCollateral.InvokeAsync(Model.CollateralId);

        //    StateHasChanged();
        //}

        async Task GetCollateralById(string collateralId)
        {
            var rsp = await _collateralsService.GetCustomerCollateralAsync(collateralId);

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