using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Applications.Requests.Lists.Entities;
using Applications.Interfaces.Addresses;
using Applications.Interfaces.Banks;
using Applications.Requests.Lists.Addreses;
using Applications.Requests.Lists.Currency;
using Domains.Models.Addresses;
using Mapster;
using Domains.Models.Lists;

namespace TrapchumApp.Components.Pages.Lists.Branches
{
    public partial class Update
    {
        [Parameter] public int Id { get; set; }

        private BranchDtoUpdate model = new();
        [Inject] private IAddressService? AddressService { get; set; }
        [Inject] private IBankService? bankService { get; set; }
        protected IEnumerable<ProvinceDto>? ProvinceList;
        protected IEnumerable<CommuneDto>? CommuneList;
        protected IEnumerable<DistrictDto>? DistrictList;
        protected IEnumerable<VillageDto>? VillageList;
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
            try
            {
                var result = await _branchService!.GetUpdateBranchById(Id);

                model = result.Data!;

                ProvinceList = await AddressService!.GetsProvinces(string.Empty);

                Countries = await AddressService.GetCountries(105);

                Cities = await AddressService.GetCities();
                if (result.IsSuccess)
                {
                    if (model!.ProvinceRefId.HasValue)
                    {
                        SelectedProvince = ProvinceList!.FirstOrDefault(d => d.ProvinceId == model?.ProvinceRefId);
                    }
                    if (model.DistrictRefId.HasValue)
                    {
                        DistrictList = await AddressService.GetDistrictsByProvinceId(model?.ProvinceRefId);

                        SelectedDistrict = DistrictList.FirstOrDefault(d => d.DistrictId == model?.DistrictRefId);

                    }
                    if (model!.DistrictRefId.HasValue)
                    {
                        CommuneList = await AddressService.GetCommunesByDistrictId(model?.DistrictRefId);

                        SelectedCommune = CommuneList.FirstOrDefault(c => c.CommuneId == model?.CommuneRefId);
                    }
                    if (model!.DistrictRefId.HasValue)
                    {
                        VillageList = await AddressService.GetVillagesByCommuneId(model?.CommuneRefId);

                        SelectedVillage = VillageList.FirstOrDefault(x => x.VillageId == model?.VillageRefId);

                    }
                }
                await base.OnInitializedAsync();

                StateHasChanged();

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
                var result = await _branchService!.GetUpdateBranchById(Id);
                if (model.EditSeq != result?.Data?.EditSeq)
                {
                    if ((bool)await DialogService.Confirm("This record may already edited by another user, please refresh page to get new edit and try again later!", "Branch", new ConfirmOptions() { OkButtonText = "OK", CancelButtonText = "Close" }
                    ))
                    {

                        await OnInitializedAsync();
                        return;
                    }
                    DialogService.Close(true);
                    return;
                }

                var updateModel = model.Adapt<Branch>();

                var response = await _branchService.Update(updateModel);

                if (response!.IsSuccess)
                {
                    NotificationService.Notify(NotificationSeverity.Success, "Edit Branch", "Branch updated successfully!");
                    DialogService.Close(true);
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Exception ", ex.Message);
            }
        }


        protected async Task AddCity(KeyboardEventArgs e)
        {
            if (e.Code == "Enter")
            {
                var city = new City { CityName = model.City! };
                var message = await AddressService!.AddCity(city);
                if (message!.IsSuccess)
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

                DistrictList = await AddressService!.GetDistrictsByProvinceId(SelectedProvince.ProvinceId);
            }
            else
            {
                SelectedProvince = null;
                DistrictList = null;
            }
            StateHasChanged();

        }
        async Task SelectDistrict(DistrictDto e)
        {
            if (e is not null)
            {
                SelectedDistrict = e;
                CommuneList = await AddressService!.GetCommunesByDistrictId(SelectedDistrict.DistrictId);
            }
            else
            {
                SelectedDistrict = null;
                CommuneList = null;
            }
            StateHasChanged();
        }
        async Task SelectCommune(CommuneDto e)
        {
            if (e is not null)
            {
                SelectedCommune = e;
                VillageList = await AddressService!.GetVillagesByCommuneId(SelectedCommune.CommuneId);
            }
            else
            {
                SelectedCommune = null;
                VillageList = null;
            }
            StateHasChanged();
        }
    }
}