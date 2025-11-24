using Applications.Responses.Lists.Entities;
using Applications.Services.Files;
using Applications.Services.Lists;
using Domains.Models.Addresses;
using Domains.Models.Lists;
using Radzen.Blazor;
using Applications.Enumerations;
using Applications.Features.Addresses.Responses;
using Applications.Features.Customers.Dto;
using Applications.Features.Customers.Commands.Customers.CreateCustomer;

namespace TrapchumApp.Pages.Lists.Customers
{
    public partial class CustomerCreate
    {

        private readonly IFileUploadServiceImp _fileUploadService;
        private readonly INationalityServiceImpl _nationalityService;
        private readonly IOccupationServiceImpl _occupationService;
        private readonly ISalutationServiceImpl _salutationService;
        public CustomerCreate(IFileUploadServiceImp fileUploadService, INationalityServiceImpl ationalityService, IOccupationServiceImpl occupationService, ISalutationServiceImpl salutationService)
        {
            _fileUploadService = fileUploadService;
            _nationalityService = ationalityService;
            _occupationService = occupationService;
            _salutationService = salutationService;
        }

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

            await GetAdditionalInformation();
            model.City = "Siem Reap";
            model.CountryName = "ប្រទេសកម្ពុជា";
            model.Salutation = "Mr";
            model.MarritalStatus = "Single";
            model.Nationality = "Khmer";

            var rsp1 = await _salutationService.GetSalutions();

            if (rsp1.IsSuccess)
            {
                Salutations = rsp1.Data!.Select(x => x.Title!).ToList();
            }

            model.Photo = "/Profiles/profile.png";

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

        protected async void FormSubmit()
        {
            try
            {
                model.EntType = EntityTypes.Customer.ToString();

                model.Photo ??= "/Profiles/profile.png";

                var rsp = await mediator.Send(new CreateCustomerCommand(model));  //await _customerService.CreateAsync(model);

                if (rsp.IsSuccess)
                {

                    await Close();

                    model = new();

                    NotificationService.Notify(NotificationSeverity.Success, "Customer was created successfully!");
                }
                else
                {

                    NotificationService.Notify(NotificationSeverity.Error, rsp.ErrMessage);
                }

            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.Message);
            }
        }



        protected async Task CreateAsync(bool saveClosed = false)
        {
            try
            {
                model.EntType = EntityTypes.Customer.ToString();

                model.Photo ??= "/Profiles/profile.png";

                var rsp = await mediator.Send(new CreateCustomerCommand(model));  //await _customerService.CreateAsync(model);

                if (rsp.IsSuccess)
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

                    NotificationService.Notify(NotificationSeverity.Error, rsp.ErrMessage);
                }

            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.Message);
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
}


