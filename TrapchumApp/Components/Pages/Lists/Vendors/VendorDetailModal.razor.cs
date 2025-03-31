using Applications.Interfaces.Vendors;
using Applications.Responses.Lists.Vendors;
using Microsoft.AspNetCore.Components;

namespace TrapchumApp.Components.Pages.Lists.Vendors
{
    public partial class VendorDetailModal
    {
        private readonly IVendorService VendorService;

        public VendorDetailModal(IVendorService vendorService)
        {
            VendorService = vendorService;
        }

        [Parameter] public VendorResponse? model { get; set; } = new();
        [Parameter] public long Id { get; set; }
        [Parameter]
        public EventCallback<int> SelectIndex { get; set; }

        protected async void SelectedIndex()
        {
            await SelectIndex.InvokeAsync(0);
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        void ShowList()
        {
            // ModalService!.Hide();
        }
    }
}