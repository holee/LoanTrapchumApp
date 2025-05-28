using Applications.Interfaces.Addresses;
using Applications.Interfaces.Banks;
using Applications.Requests.Lists.Addreses;
using Applications.Requests.Lists.Entities;
using Applications.Responses.Lists.Currency;
using Domains.Models.Addresses;
using Domains.Models.Lists;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace TrapchumApp.Components.Pages.Lists.Branches
{
    public partial class BranchCreateModal
    {
        private BranchDtoCreate model = new();
        [Inject] IBankService bankService { get; set; } = default!;
        protected IEnumerable<ProvinceDto> provinceList = [];
        protected IEnumerable<CommuneDto> communeList = [];
        protected IEnumerable<DistrictDto> districtList = [];
        protected IEnumerable<VillageDto> villageList = [];
        protected IEnumerable<BankResponse> bankList = [];
        protected IEnumerable<Country> Countries = [];
        protected IEnumerable<string> strCountries = [];

        protected IEnumerable<string> Cities = [];
        protected IEnumerable<string> filteredItems = [];
        protected override async Task OnInitializedAsync()
        {
            var rsp = await _addressService!.GetCountries(105);

            if (rsp.IsSuccess)
            {
                Countries = rsp.Data ?? [];
            }

            provinceList = await _addressService.GetsProvinces(string.Empty);

            await GetCities();

            var results = await bankService!.GetAllAsync();

            if (results.Any())
            {
                bankList = results.Adapt<IEnumerable<BankResponse>>();
            }
            await base.OnInitializedAsync();
        }


        async Task GetCities(string? filter = null)
        {
            Cities = await _addressService.GetCities(filter);

            filteredItems = Cities;
        }
        async Task OnSubmit()
        {
            try
            {
                var bank = bankList?.FirstOrDefault();

                model.BankRefId = bank?.BankId;

                var createModel = model.Adapt<Branch>();

                var response = await _branchService.CreateAsync(createModel);

                if (response!.IsSuccess)
                {
                    NotificationService.Notify(Radzen.NotificationSeverity.Success, "New Branch", "Branch created successfully!");
                    DialogService.Close(true);
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(Radzen.NotificationSeverity.Error, "Exception", ex.Message);
            }
        }
        protected async Task AddCity()
        {
            var city = new CityDtoCreate { CityName = searchText };

            var message = await _addressService.AddCity(city);

            if (message.IsSuccess)
            {
                await GetCities();

                StateHasChanged();
            }
        }
        async void SelectProvince(object e)
        {
            if (e is not null)
            {
                var id = ((ProvinceDto)e).ProvinceId;
                districtList = await _addressService.GetDistrictsByProvinceId(id);
                model.ProDescription = ((ProvinceDto)e).ProDescription;
                model.ProInKhmer = ((ProvinceDto)e).ProInKhmer;
            }
            StateHasChanged();

        }
        async Task SelectDistrict(object e)
        {
            if (e is not null)
            {
                var id = ((DistrictDto)e).DistrictId;

                communeList = await _addressService!.GetCommunesByDistrictId(id);
                model.DisDescription = ((DistrictDto)e).DisDescription;
                model.DisInKhmer = ((DistrictDto)e).DisInKhmer;
            }
            else
            {
                communeList = [];
            }
            StateHasChanged();
        }
        async Task SelectCommune(object e)
        {
            if (e is not null)
            {
                var id = ((CommuneDto)e).CommuneId;
                villageList = await _addressService!.GetVillagesByCommuneId(id);
                model.ComDescription = ((CommuneDto)e).ComDescription;
                model.ComInKhmer = ((CommuneDto)e).ComInKhmer;
            }
            else
            {
                villageList = [];
            }
            StateHasChanged();
        }

        void SelectVillage(object e)
        {
            if (e is not null)
            {
                model.ComDescription = ((VillageDto)e)?.VillDescription;
                model.ComInKhmer = ((VillageDto)e!)?.VilInKhmer;
            }
            StateHasChanged();
        }



    }
}