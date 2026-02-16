using Applications.Features.Addresses.Requests;
using Applications.Features.Addresses.Responses;
using Applications.Features.Branches.Command.CreateBranch;
using Applications.Features.Branches.Dtos;
using Applications.Features.Currencies.Dto;
using Applications.Interfaces.Banks;
using Domains.Models.Addresses;
using Domains.Models.Lists;
using Mapster;
using TrapchumApp.Helpers.Extensions;
using TrapchumApp.Helpers.Validations;

namespace TrapchumApp.Pages.Lists.Branches
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

        EditContext editContext = default!;

        protected override async Task OnInitializedAsync()
        {

            editContext = new EditContext(model);

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

            var bank = bankList?.FirstOrDefault();

            model.BankRefId = bank?.BankId;

            var command = new CreateBranchCommand(model);

            var response = await mediator.Send(command);

            if (response.IsSuccess())
            {
                NotificationService.Notify(NotificationSeverity.Success, "New Branch", "Branch created successfully!");

                DialogService.Close(true);
            }
            else
            {
                editContext.ApplyValidationSummary<BranchDtoCreate>(response.GetMessage());
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