using Applications.Extensions;
using Applications.Requests.Collaterals;
using Applications.Responses.Collaterals;
using Applications.Responses.ViewModels.Customers;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace TrapchumApp.Components.Pages.Lists.Collaterals
{
    public partial class CollateralDetail
    {
        [Parameter] public EventCallback Close { get; set; }
        [Parameter] public EventCallback<Guid?> AddMoreCollateral { get; set; }
        [Parameter] public CollateralResponse? Model { get; set; } = new();
        async Task CloseTab()
        {
            await Close.InvokeAsync();
        }
        [Parameter] public long CustomerId { get; set; }
        [Parameter] public LoanCustomerQuery? SelectedCustomer { get; set; }
        [Parameter] public IQueryable<CollateralLineResponse>? CollateralLines { get; set; }

        private async Task AddCollateralByCustomer()
        {
            if (Model!.CollateralId.IsNotNullOrEmpty())
            {
                NotificationService.Notify(Radzen.NotificationSeverity.Warning, "", "Try getting data not found!");
                return;
            }
            await AddMoreCollateral.InvokeAsync(Model.CollateralId);
            Console.WriteLine("Click...!");
            StateHasChanged();
        }


    }
}