using Applications.Features.OtherNames.Dto;
using Applications.Features.Vendors.Commands.CreateVendor;

namespace TrapchumApp.Pages.Lists.Vendors;

public partial class VendorCreatePage
{
    [Parameter] public EventCallback Closed { get; set; } = default!;

    protected EntityVendorCreate model = new();

    private bool _isLoading = false;

    protected override async Task OnInitializedAsync()
    {
        model.City = "Siem Reap";
        model.CountryName = "ប្រទេសកម្ពុជា";
        model.Salutation = "Mr";
        model.MarritalStatus = "Single";
        model.Nationality = "Khmer";

        await base.OnInitializedAsync();
    }

    protected async void FormSubmit()
    {
        var command = new CreateVendorCommand(model);

        var response = await mediator.Send(command);

        if (response.IsSuccess())
        {
            _toastService.ShowSuccess("Success", "Vendor created successfully.");
        }
        else
        {
            _toastService.ShowError("Error", response.GetMessage());
        }

    }

    protected async Task Close()
    {
        await Closed.InvokeAsync();
    }

}

