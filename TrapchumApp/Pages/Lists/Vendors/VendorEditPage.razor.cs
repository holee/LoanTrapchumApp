using Applications.Features.OtherNames.Dto;
using Applications.Features.Vendors.Commands.UpdateVendor;
using Applications.Features.Vendors.Queries.GetUpdateVendorById;

namespace TrapchumApp.Pages.Lists.Vendors
{
    public partial class VendorEditPage(ILogger<VendorEditPage> logger)
    {
        [Parameter] public EventCallback Closed { get; set; }
        [Parameter] public string? Id { get; set; }

        private EntityVendorCreate model = new();

        bool _isLoading = false;
        protected override async Task OnInitializedAsync()
        {

            await ShowEditVendor(Id);

            await base.OnInitializedAsync();
        }
        protected async Task HandleFormSubmit()
        {

            var command = new UpdateVendorCommand(model);


            var rsp = await mediator.Send(command); //vendorService.UpdateAsync(model);

            if (rsp.IsSuccess())
            {
                _toastService.ShowSuccess("Success", "Vendor updated successfully");

                await Close();

                return;
            }
            else
            {
                _toastService.ShowError("Error", rsp.GetMessage());
            }

        }

        protected async Task ShowEditVendor(string? id)
        {
            try
            {
                _isLoading = true;

                var command = new GetUpdateVendorByIdQuery(id);

                var resp = await mediator.Send(command);// vendorService!.GetEditVendorAsync(id);

                if (!resp.IsSuccess())
                {
                    model = new();

                    return;
                }

                model = resp.Value;

                _isLoading = false;
            }
            catch (Exception ex)
            {
                _isLoading = false;

                logger.LogError(ex, "Error while loading vendor with id {Id}", id);
            }
            finally
            {
                _isLoading = false;
            }

            StateHasChanged();
        }

        protected async Task Close()
        {
            await Closed.InvokeAsync();
        }
    }
}


