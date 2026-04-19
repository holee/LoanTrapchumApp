using Applications.Features.AdditionalInformations.Dto;
using Applications.Features.Addresses.Dto;
using Applications.Features.OtherNames.Dto;
using Applications.Responses.Lists.Entities;
using Applications.Services.Lists;
using Domains.Models.Addresses;
using Domains.Models.Lists;

namespace TrapchumApp.Pages.Lists.Vendors
{
    public partial class VendorCreatePage(IVendorServiceImpl vendorService, ISalutationServiceImpl salutationService)
    {
        [Parameter] public EventCallback<int> Closed { get; set; } = default!;
        [Parameter] public int SelectIndex { get; set; } = 0;
        [Parameter] public EventCallback<int?> SelectedValueChanged { get; set; }

        IEnumerable<string> Salutations = [];
        IEnumerable<ProvinceDto> Provinces = [];
        IEnumerable<string> Cities = [];
        IEnumerable<OcupationResponse> Occupations = [];
        IEnumerable<Country> Countries = [];
        protected EntityVendorCreate model = new();
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

            await base.OnInitializedAsync();
        }

        IEnumerable<NationalityDto> _nationalities { get; set; } = [];

        IEnumerable<string> Statuses = ["Single", "Married", "Divorced", "Widow"];

        string selectedTab = "info";

        string city = "";

        async Task OnKeyPress(KeyboardEventArgs e)
        {
            if (e.Code == "Enter")
            {
                await _addressService.AddCity(new CityDtoCreate
                {
                    CityName = city
                });
                Cities = await _addressService.GetCities();
            }

        }
        async Task GetAdditionalInformation()
        {
            //await GetNationalities();

            //Provinces = await _addressService.GetsProvinces();

            //Occupations = await _employeeService!.GetOccupationsAsync("Position");

            //var rsp = await _addressService.GetCountries(105);

            //if (rsp.IsSuccess)
            //{
            //    Countries = rsp.Data ?? [];
            //}

            //Cities = await _addressService.GetCities();

            //var rsp1 = await salutationService.GetSalutions();

            //if (rsp1.IsSuccess)
            //{
            //    Salutations = rsp1.Data!.Select(x => x.Title!).ToList();
            //}
        }

        
        private async Task GetSalutations()
        {
            var salutations = await salutationService.GetSalutions();

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
                var response = await vendorService.CreateAsync(model);

                if (response.IsSuccess)
                {
                    NotificationService.Notify(NotificationSeverity.Success, response.Data!);

                    await Close();

                }
                else
                {

                    NotificationService.Notify(NotificationSeverity.Error, response.ErrMessage);
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
                await salutationService.AddSalution(new Salutation
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
            await Closed.InvokeAsync(SelectIndex);
        }

    }


}


//private async Task GetNationalities()
//{
//    var command = new GetNationalitiesQuery(0, new QueryParam());

//    var rsp2 = await mediator.Send(command);

//    _nationalities = rsp2.Items;  //await _nationalityService.GetNationalities();
//}

//async void SelectedProvinceChanged(object arg)
//{
//    if (arg is not null)
//    {
//        model.ProInKhmer = ((ProvinceDto)arg).ProInKhmer;

//        Districts = await _addressService!.GetDistrictsByProvinceId(((ProvinceDto)arg).ProvinceId);

//        StateHasChanged();
//    }

//}
//async void SelectedDistrictChanged(object arg)
//{
//    if (arg is not null)
//    {
//        model.DisInKhmer = ((DistrictDto)arg).DisInKhmer;

//        Communes = await _addressService!.GetCommunesByDistrictId(((DistrictDto)arg).DistrictId);

//        StateHasChanged();
//    }

//}
//async void SelectedCommuneChanged(object arg)
//{
//    if (arg is not null)
//    {
//        model.ComInKhmer = ((CommuneDto)arg).ComInKhmer;

//        Villages = await _addressService!.GetVillagesByCommuneId(((CommuneDto)arg).CommuneId);

//        StateHasChanged();
//    }

//}
//void SelectedVillageChanged(object arg)
//{
//    if (arg is not null)
//    {
//        model.VilInKhmer = ((VillageDto)arg).VilInKhmer;
//        StateHasChanged();
//    }

//}

//private async Task GetCities()
//{
//    Cities = await _addressService.GetCities();
//}
//private async Task GetOccupation()
//{
//    var pager = await occupationService.GetOccupations<OcupationResponse>(0, string.Empty);
//    Occupations = pager.Items;
//}
//private async Task GetProvinces()
//{
//    Provinces = await _addressService.GetsProvinces();
//}
//private async Task GetCountries()
//{
//    var rsp = await _addressService.GetCountries(105);

//    if (rsp.IsSuccess)
//        Countries = rsp.Data ?? [];

//}