using Applications.Responses.Collaterals;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;

namespace TrapchumApp.Components.Pages.Lists.Collaterals
{
    public partial class CollateralCreateTab
    {
        RadzenDataGrid<CollateralLineResponse> personalGrid = default!;
        RadzenDataGrid<CollateralLineResponse> propertiesGrid = default!;
        [Parameter]
        public EventCallback Close { get; set; }
        [Parameter]
        public EventCallback SaveChanged { get; set; }

        async void CloseTab()
        {
            await Close.InvokeAsync();
        }
    }
}