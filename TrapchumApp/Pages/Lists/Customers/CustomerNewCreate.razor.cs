using Applications.Enumerations;
using Applications.Features.Addresses.Dto;
using Applications.Features.Customers.Commands.CreateCustomer;
using Applications.Features.Customers.Dto;
using Applications.Features.Customers.Response;
using Applications.Responses.Lists.Entities;
using Applications.Services.Files;
using Applications.Services.Lists;
using Domains.Models.Addresses;
using Domains.Models.Lists;
using Radzen.Blazor;
using TrapchumApp.Helpers.Extensions;

namespace TrapchumApp.Pages.Lists.Customers;

public partial class CustomerNewCreate
{
    private readonly IFileUploadServiceImp _fileUploadService;
    private readonly INationalityServiceImpl _nationalityService;
    private readonly IOccupationServiceImpl _occupationService;
    private readonly ISalutationServiceImpl _salutationService;
    public CustomerNewCreate(IFileUploadServiceImp fileUploadService, INationalityServiceImpl ationalityService, IOccupationServiceImpl occupationService, ISalutationServiceImpl salutationService)
    {
        _fileUploadService = fileUploadService;
        _nationalityService = ationalityService;
        _occupationService = occupationService;
        _salutationService = salutationService;
    }

    [Parameter] public EventCallback<int> Closed { get; set; } = default!;
    [Parameter] public EventCallback<VCustomerList> UpdateState { get; set; }
    [Parameter] public EntityCustomerCreate CustomerModel { get; set; } = new();
    IEnumerable<string> Salutations = [];
    IEnumerable<ProvinceDto> Provinces = [];
    IEnumerable<string> Cities = [];
    IEnumerable<OcupationResponse> Occupations = [];
    IEnumerable<Country> Countries = [];
    protected IEnumerable<DistrictDto> Districts = [];
    protected IEnumerable<CommuneDto> Communes = [];
    protected IEnumerable<VillageDto> Villages = [];
    protected EditForm? editForm;
    private bool IsLoading = true;
    protected string? newSalution = "";
    protected override async Task OnInitializedAsync()
    {

        await GetAdditionalInformation();

        CustomerModel.City = "Siem Reap";

        CustomerModel.CountryName = "ប្រទេសកម្ពុជា";

        CustomerModel.Salutation = "Mr";

        CustomerModel.MarritalStatus = "Single";

        CustomerModel.Nationality = "Khmer";

        var rsp1 = await _salutationService.GetSalutions();

        if (rsp1.IsSuccess)
        {
            Salutations = rsp1.Data!.Select(x => x.Title!).ToList();
        }

        CustomerModel.Photo = "/Profiles/profile.png";

        await base.OnInitializedAsync();
    }

    IEnumerable<Nationality> Nationalities { get; set; } = [];

    RadzenFileInput<string> fileUpload = default!;

    string selectedTab = "info";

    string city = "";

    async Task GetAdditionalInformation()
    {
        Nationalities = await _nationalityService.GetNationalities();

        Provinces = await _addressService.GetsProvinces();

        Occupations = await _employeeService!.GetOccupationsAsync("Position");

        var rsp = await _addressService.GetCountries(105);

        if (rsp.IsSuccess)
        {
            Countries = rsp.Data ?? [];
        }
        Cities = await _addressService.GetCities();
    }
    private async Task GetSalutations()
    {
        var salutations = await _salutationService.GetSalutions();

        if (salutations.IsSuccess)
        {
            Salutations = salutations!.Data!.Select(x => x.Title!).ToList();
        }
        else
        {
            Salutations = [];
        }
    }

    protected async Task CreateAsync(bool saveClosed = false)
    {

        CustomerModel.Id = $"ent-{Guid.CreateVersion7()}";

        CustomerModel.EntType = EntityTypes.Customer.ToString();

        CustomerModel.Photo ??= "/Profiles/profile.png";

        var rsp = await mediator.Send(new CreateCustomerCommand(CustomerModel));

        if (rsp.IsSuccess())
        {
            NotificationService.Notify(NotificationSeverity.Success, "Customer was created successfully!");

            if (saveClosed)
            {
                await Close();

                await UpdateState.InvokeAsync(rsp.Value);
            }
            else
            {
                CustomerModel = new();

                await UpdateState.InvokeAsync(rsp.Value);
            }
        }
        else
        {

            NotificationService.Notify(NotificationSeverity.Error, rsp.GetMessage());
        }

    }



    protected async Task AddNewSalutation(KeyboardEventArgs e)
    {
        if (e.Code == "Enter")
        {
            await _salutationService.AddSalution(new Salutation
            {
                Title = newSalution
            });
            await GetSalutations();
        }
    }
    protected void OnTextChangedSalution(ChangeEventArgs e)
    {
        newSalution = e?.Value?.ToString();
    }
    protected async Task<IEnumerable<string>?> SearchSalutaions(string searchText = "")
    {
        var salution = Salutations?.Where(x => x.StartsWith(searchText, StringComparison.InvariantCultureIgnoreCase))
                                 .ToList();
        return await Task.FromResult(salution);
    }
    protected async Task Close()
    {
        await Closed.InvokeAsync(1);
    }

    protected async Task CloseTab()
    {
        await Closed.InvokeAsync();
    }

}


//protected async void FormSubmit()
//{

//    CustomerModel.EntType = EntityTypes.Customer.ToString();

//    CustomerModel.Photo ??= "/Profiles/profile.png";

//    var rsp = await mediator.Send(new CreateCustomerCommand(CustomerModel));  //await _customerService.CreateAsync(model);

//    if (rsp.IsSuccess())
//    {

//        await Close();

//        CustomerModel = new();

//        await UpdateState.InvokeAsync(rsp.Value);

//        NotificationService.Notify(NotificationSeverity.Success, "Customer was created successfully!");
//    }
//    else
//    {

//        NotificationService.Notify(NotificationSeverity.Error, rsp.GetMessage());
//    }

//}

