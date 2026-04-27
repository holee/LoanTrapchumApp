using Applications.Features.Vendors.Dto.Responses;
using Applications.Features.Vendors.Queries.GetVendorById;
using Applications.Services.Lists;

namespace TrapchumApp.Pages.Lists.Vendors
{
    public partial class VendorDetailPage
    {
        VendorDto? model { get; set; } = new();
        [Parameter] public string? Id { get; set; }
        [Parameter] public EventCallback Closed { get; set; }
        [Parameter] public EventCallback<string> OnEdit { get; set; } 
        protected async Task CloseTab()
        {
            await Closed.InvokeAsync();
        }
        async Task ClickEdit()
        {
            await OnEdit.InvokeAsync(Id);
        }

        protected override async Task OnInitializedAsync()
        {
            await GetVendorById(Id);

            await base.OnInitializedAsync();
        }


        async Task GetVendorById(string? id)
        {

            var command = new GetVendorByIdQuery(id);

            var rsp =await mediator.Send(command); //VendorService.GetVendorByIdAsync(id);

            if (rsp.IsSuccess())
            {
                model = rsp.Value;
            }
            else
            {
                model = new();
            }
        }
    }
}