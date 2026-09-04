using Applications.Responses.Lists.Entities;
using Domains.Models.Addresses;
using Domains.Models.Lists;
using Radzen.Blazor;
using Applications.Features.Customers.Dto;
using Applications.Features.Addresses.Dto;
using Applications.Features.Customers.Commands.CreateCustomer;

namespace Web.UI.Pages.Lists.Customers;

public partial class CustomerCreate(ILogger<CustomerCreate> logger)
{
    [Parameter] public EventCallback<int> Closed { get; set; } = default!;

    IEnumerable<string> Salutations = [];
    IEnumerable<ProvinceDto> Provinces = [];
    IEnumerable<string> Cities = [];
    IEnumerable<OcupationResponse> Occupations = [];
    IEnumerable<Country> Countries = [];
    protected EntityCustomerCreate model = new();
    protected IEnumerable<DistrictDto> Districts = [];
    protected IEnumerable<CommuneDto> Communes = [];
    protected IEnumerable<VillageDto> Villages = [];
    protected EditForm? editForm;
    private bool IsLoading = true;
    protected string? newSalution = "";
    protected override async Task OnInitializedAsync()
    {

        model.City = "Siem Reap";

        model.CountryName = "ប្រទេសកម្ពុជា";

        model.Salutation = "Mr";

        model.MarritalStatus = "Single";

        model.Nationality = "Khmer";

        model.Photo = "/assets/profiles/profile.png";

        await base.OnInitializedAsync();
    }

    IEnumerable<Nationality> Nationalities { get; set; } = [];

    RadzenFileInput<string> fileUpload = default!;

    protected async void FormSubmit()
    {
        try
        {
            model.EntType = EntityTypes.Customer.ToString();

            model.Photo ??= "/assets/profiles/profile.png";

            var rsp = await mediator.Send(new CreateCustomerCommand(model));  //await _customerService.CreateAsync(model);

            if (rsp.IsSuccess())
            {

                await Close();

                model = new();

                NotificationService.Notify(NotificationSeverity.Success, "Customer was created successfully!");
            }
            else
            {

                NotificationService.Notify(NotificationSeverity.Error, rsp.GetMessage());
            }

        }
        catch (Exception ex)
        {
            NotificationService.Notify(NotificationSeverity.Error, ex.Message);
        }
    }



    protected async Task CreateAsync(bool saveClosed = false)
    {
        logger.LogInformation("Creating new customer with name: {Name}", model.Name);
        try
        {
            model.EntType = EntityTypes.Customer.ToString();

            model.Photo ??= "/assets/profiles/profile.png";

            var rsp = await mediator.Send(new CreateCustomerCommand(model));  //await _customerService.CreateAsync(model);

            if (rsp.IsSuccess())
            {
                NotificationService.Notify(NotificationSeverity.Success, "Customer was created successfully!");

                if (saveClosed)
                {
                    await Close();
                }
                else
                {
                    model = new();
                }
            }
            else
            {

                NotificationService.Notify(NotificationSeverity.Error, rsp.GetMessage());
            }

            logger.LogInformation("Finished creating customer with name: {Name}", model.Name);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating customer with name: {Name}. Error: {Message}", model.Name, ex.Message);

            NotificationService.Notify(NotificationSeverity.Error, ex.Message);
        }
    }


    protected async Task Close()
    {
        await Closed.InvokeAsync(1);
    }


}


