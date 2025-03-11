using Microsoft.AspNetCore.Components;
using TCLMS.Shared.Entities.Dtos.Lists.Addreses;
using TCLMS.Shared.Entities.Dtos.Lists.Currency;
using TCLMS.Shared.Entities.Dtos.Lists;
using TCLMS.Shared.Entities.Models.Addresses;
using ApplicationServices.Interfaces;
using AppilcationServices.Interfaces;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace TCLMS.Front.Pages.Lists.Branches
{
    public partial class Update
    {
        [Parameter] public int Id { get; set; }
        private BranchDtoUpdate model = new();
        [Inject] private IAddressService? AddressService { get; set; }
        [Inject] private IBankService? bankService { get; set; }
        [Inject] private IBranchService? BranchService { get; set; }
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
                var result = await BranchService!.GetBranchByIdToEditAsync(Id);
                model = result.Data!;

                ProvinceList = await AddressService!.GetAllProvinceAsync();
                Countries = await AddressService.GetCountries(105);
                Cities = await AddressService.GetCities();
                if (result.IsSuccess)
                {
                    if (model!.ProvinceRefId.HasValue)
                    {
                        SelectedProvince = ProvinceList!.FirstOrDefault(d => d.ProvinceID == model?.ProvinceRefId);
                    }
                    if (model.DistrictRefId.HasValue)
                    {
                        this.DistrictList = await AddressService.GetDistrictByProvinceAsync(model?.ProvinceRefId);
                        SelectedDistrict = DistrictList.FirstOrDefault(d => d.DistrictId == model?.DistrictRefId);

                    }
                    if (model!.DistrictRefId.HasValue)
                    {
                        this.CommuneList = await AddressService.GetCommuneByDistricteAsync(model?.DistrictRefId);
                        SelectedCommune = this.CommuneList.FirstOrDefault(c => c.CommuneId == model?.CommuneRefId);
                    }
                    if (model!.DistrictRefId.HasValue)
                    {
                        this.VillageList = await AddressService.GetVillageByCommuneAsync(model?.CommuneRefId);

                        SelectedVillage = this.VillageList.FirstOrDefault(x => x.VillageId == model?.VillageRefId);

                    }
                }
                await base.OnInitializedAsync();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error,"Exception",ex.Message);
                Console.WriteLine(ex.Message);
            }

        }
        async Task OnSubmit()
        {
            try
            {

                model.ProvinceRefId = SelectedProvince?.ProvinceID;
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
                var result = await BranchService!.GetBranchByIdToEditAsync(Id);
                if (model.EditSeq != result?.Data?.EditSeq)
                {
                    if ((bool)await DialogService.Confirm("This record may already edited by another user, please refresh page to get new edit and try again later!", "Branch",new ConfirmOptions() { OkButtonText = "OK", CancelButtonText = "Close" }
                    ))
                    {

                        await OnInitializedAsync();
                        return;
                    }
                    DialogService.Close(true);
                    return;
                }
                var response = await BranchService.UpdateAsync(Id, model);
                if (response!.IsSuccess)
                {
                    NotificationService.Notify(Radzen.NotificationSeverity.Success,
                        "Edit Branch", "Branch updated successfully!");


                    DialogService.Close(true);
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(Radzen.NotificationSeverity.Error,
                        "Exception ", ex.Message);
            }
        }


        protected async Task AddCity(KeyboardEventArgs e)
        {
            if (e.Code == "Enter")
            {
                var city = new CityDtoCreate { CityName = model.City };
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

                DistrictList = await AddressService!.GetDistrictByProvinceAsync(SelectedProvince.ProvinceID);
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
                CommuneList = await AddressService!.GetCommuneByDistricteAsync(SelectedDistrict.DistrictId);
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
                VillageList = await AddressService!.GetVillageByCommuneAsync(SelectedCommune.CommuneId);
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