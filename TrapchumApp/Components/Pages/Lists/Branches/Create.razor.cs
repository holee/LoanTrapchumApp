using Applications.Interfaces.Addresses;
using Applications.Interfaces.Banks;
using Applications.Requests.Lists.Addreses;
using Applications.Requests.Lists.Currency;
using Applications.Requests.Lists.Entities;
using Domains.Models.Addresses;
using Domains.Models.Lists;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace TrapchumApp.Components.Pages.Lists.Branches
{
    public partial class Create
    {
        private BranchDtoCreate model = new();
        [Inject] IAddressService AddressService { get; set; } = default!;
        [Inject] IBankService bankService { get; set; } = default!;
        protected IEnumerable<ProvinceDto>? provinceList;
        protected IEnumerable<CommuneDto>? communeList;
        protected IEnumerable<DistrictDto>? districtList;
        protected IEnumerable<VillageDto>? villageList;
        protected IEnumerable<BankDto>? bankList;
        protected IEnumerable<string>? Cities;
        protected ProvinceDto? SelectedProvince;
        protected DistrictDto? SelectedDistrict;
        protected CommuneDto? SelectedCommune;
        protected VillageDto? SelectedVillage;
        protected BankDto? SelectedBank;
        protected IEnumerable<Country>? Countries = new List<Country>();
        protected IEnumerable<string>? strCountries = new List<string>();

        protected override async Task OnInitializedAsync()
        {
            Countries = await AddressService!.GetCountries(105);
            strCountries = Countries?.Select(x => x.DesInKhmer).ToList()!;
            var provinces = await AddressService.GetsProvinces(string.Empty);
            Cities = await AddressService.GetCities();
            var results = await bankService!.GetAllAsync();
            if (results.Any())
            {
                bankList = results.Adapt<IEnumerable<BankDto>>();
            }
            await base.OnInitializedAsync();
        }

        async Task OnSubmit()
        {
            try
            {
                //editForm?.EditContext?.Validate();
                var bank = bankList?.FirstOrDefault();
                model.BankRefId = bank?.BankId;
                model.ProvinceRefId = SelectedProvince?.ProvinceId;
                model.ProDescription = SelectedProvince?.ProDescription;
                model.ProInKhmer = SelectedProvince?.ProInKhmer;
                model.DistrictRefId = SelectedDistrict?.DistrictId;
                model.DisDescription = SelectedDistrict?.DisDescription;
                model.DisInKhmer = SelectedDistrict?.DisInKhmer;
                model.CommuneRefId = SelectedCommune?.CommuneId;
                model.ComDescription = SelectedCommune?.ComDescription;
                model.ComInKhmer = SelectedCommune?.ComInKhmer;
                model.VillageRefId = SelectedVillage?.VillageId;
                model.VillDescription = SelectedVillage?.VillDescription;
                model.VilInKhmer = SelectedVillage?.VilInKhmer;


                var createModel = model.Adapt<Branch>();

                var response = await _branchService!.Create(createModel);

                if (response!.IsSuccess)
                {
                    NotificationService.Notify(Radzen.NotificationSeverity.Success,
                        "New Branch", "Branch created successfully!");


                    DialogService.Close(true);
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(Radzen.NotificationSeverity.Error, "Exception", ex.Message);
            }
        }
        protected async Task AddCity(KeyboardEventArgs e)
        {
            if (e.Code == "Enter")
            {
                var city = new City { CityName = model.City! };
                var message = await AddressService.AddCity(city);
                if (message.IsSuccess)
                {
                    Cities = await AddressService.GetCities();

                    StateHasChanged();
                }
            }
        }
        async void SelectProvince(ProvinceDto e)
        {
            SelectedProvince = e;
            if (SelectedProvince is not null)
            {

                districtList = await AddressService!.GetDistrictsByProvinceId(SelectedProvince.ProvinceId);
            }
            else
            {
                SelectedProvince = null;
                districtList = null;
            }
            StateHasChanged();

        }
        async Task SelectDistrict(DistrictDto e)
        {
            if (e is not null)
            {
                SelectedDistrict = e;
                communeList = await AddressService!.GetCommunesByDistrictId(SelectedDistrict.DistrictId);
            }
            else
            {
                SelectedDistrict = null;
                communeList = null;
            }
            StateHasChanged();
        }
        async Task SelectCommune(CommuneDto e)
        {
            if (e is not null)
            {
                SelectedCommune = e;
                villageList = await AddressService!.GetVillagesByCommuneId(SelectedCommune.CommuneId);
            }
            else
            {
                SelectedCommune = null;
                villageList = null;
            }
            StateHasChanged();
        }
    }
}