using Applications.Responses.ViewModels.Vendors;
using Applications.Services.LIsts;
using Microsoft.AspNetCore.Components;

namespace TrapchumApp.Pages.Lists.Vendors
{
    public partial class VendorDetailModal
    {
        private readonly IVendorServiceImpl VendorService;

        public VendorDetailModal(IVendorServiceImpl vendorService)
        {
            VendorService = vendorService;
        }

        VendorVm? model { get; set; } = new();
        [Parameter] public string? Id { get; set; }
        [Parameter] public EventCallback<int> Closing { get; set; }
        [Parameter] public EventCallback<string> OnEdit { get; set; } 
        protected async void SelectedIndex()
        {
            await Closing.InvokeAsync(0);
        }

        async Task ClickEdit()
        {
            await OnEdit.InvokeAsync(Id);
        }


        protected override async Task OnParametersSetAsync()
        {
            await GetVendorById(Id);
            await base.OnParametersSetAsync();
        }

        async Task GetVendorById(string? id)
        {
            var rsp =await VendorService.GetVendorByIdAsync(id);

            if (rsp.IsSuccess)
            {
                model = rsp.Data;
            }
            else
            {
                model = new();
            }
        }
    }
}