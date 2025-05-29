using Applications.Interfaces.Banks;
using Applications.Requests.Lists.Addreses;
using Applications.Requests.Lists.Entities;
using Applications.Responses.Lists.Currency;
using Domains.Models.Addresses;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace TrapchumApp.Pages.Lists.Branches
{
    public partial class BranchUpdateModal
    {
        [Parameter] public int Id { get; set; }

        private BranchDtoUpdate model = new();
        [Inject] private IBankService? bankService { get; set; }
        protected IEnumerable<ProvinceDto> ProvinceList = [];
        protected IEnumerable<CommuneDto> CommuneList = [];
        protected IEnumerable<DistrictDto> DistrictList = [];
        protected IEnumerable<VillageDto> VillageList = [];
        protected IEnumerable<BankResponse> bankList = [];
        protected IEnumerable<string> Cities = [];
        protected IEnumerable<string> filteredItems = [];
        protected IEnumerable<Country> Countries = new List<Country>();
        protected IEnumerable<string>? strCountries = new List<string>();
        string? searchText = string.Empty;

        void OnSearchTextChanged(ChangeEventArgs e)
        {
            searchText = e.Value?.ToString() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(searchText))
            {
                filteredItems = Cities; // Show all items if search is empty
            }
            else
            {
                filteredItems = Cities
                    .Where(i => i.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
        }

        async Task OnKeyPress(KeyboardEventArgs args)
        {
            if (args.Key == "Enter" && !string.IsNullOrWhiteSpace(searchText))
            {
                if (!Cities.Any(i => i.Equals(searchText, StringComparison.OrdinalIgnoreCase)))
                {
                    await AddCity();

                    searchText = ""; //Clear the textbox
                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var result = await _branchService!.GetUpdateBranchById(Id);

                model = result.Data!;

                ProvinceList = await _addressService!.GetsProvinces(string.Empty);

                var rsp = await _addressService.GetCountries(105);

                if (rsp.IsSuccess)
                {
                    Countries = rsp.Data ?? [];
                }

                Cities = await _addressService.GetCities();

                if (result.IsSuccess)
                {
                    if (model.DistrictRefId.HasValue)
                    {
                        DistrictList = await _addressService.GetDistrictsByProvinceId(model?.ProvinceRefId);
                    }
                    if (model!.DistrictRefId.HasValue)
                    {
                        CommuneList = await _addressService.GetCommunesByDistrictId(model?.DistrictRefId);
                    }
                    if (model!.DistrictRefId.HasValue)
                    {
                        VillageList = await _addressService.GetVillagesByCommuneId(model?.CommuneRefId);
                    }
                }
                await base.OnInitializedAsync();

            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Exception", ex.Message);

                Console.WriteLine(ex.Message);
            }

        }
        async Task OnSubmit()
        {
            try
            {

                var response = await _branchService.UpdateAsync(model);

                if (response!.IsSuccess)
                {
                    NotificationService.Notify(NotificationSeverity.Success, "Branch updated successfully!");

                    DialogService.Close(true);
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Exception ", ex.Message);
            }
        }

        async Task GetCities(string? filter = null)
        {
            Cities = await _addressService.GetCities(filter);

            filteredItems = Cities;
        }
        protected async Task AddCity()
        {
            var city = new CityDtoCreate { CityName = model.City! };

            var message = await _addressService!.AddCity(city);

            if (message!.IsSuccess)
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
                DistrictList = await _addressService.GetDistrictsByProvinceId(id);
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

                CommuneList = await _addressService!.GetCommunesByDistrictId(id);

                model.DisDescription = ((DistrictDto)e).DisDescription;

                model.DisInKhmer = ((DistrictDto)e).DisInKhmer;
            }
            else
            {
                CommuneList = [];
            }
            StateHasChanged();
        }
        async Task SelectCommune(object e)
        {
            if (e is not null)
            {
                var id = ((CommuneDto)e).CommuneId;

                VillageList = await _addressService!.GetVillagesByCommuneId(id);

                model.ComDescription = ((CommuneDto)e).ComDescription;

                model.ComInKhmer = ((CommuneDto)e).ComInKhmer;
            }
            else
            {
                VillageList = [];
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