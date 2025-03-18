using AppilcationServices.Interfaces;
using ApplicationServices.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using TCLMS.Front.Core.Validations;
using TCLMS.Shared.Entities.Dtos.Lists;
using TCLMS.Shared.Entities.Dtos.Lists.Addreses;
using TCLMS.Shared.Entities.Dtos.Lists.OtherNames;
using TCLMS.Shared.Entities.Models.Lists;

namespace TCLMS.Front.Pages.Lists.OtherNames
{
    public partial class NameEditModal
    {
        [Inject] private IAddressService? AddressService { get; set; }
        [Inject] private IOtherNameService? NameService { get; set; }
        [Parameter] public IEnumerable<ProvinceDto>? ProvinceList { get;set; }
        [Parameter] public IEnumerable<CommuneDto>? CommuneList { get; set; }
        [Parameter] public IEnumerable<DistrictDto>? DistrictList { get; set; }
        [Parameter] public IEnumerable<VillageDto>? VillageList { get; set; }
        [Parameter] public ProvinceDto? SelectedProvince { get; set; }
        [Parameter] public DistrictDto? SelectedDistrict { get; set; }
        [Parameter] public CommuneDto? SelectedCommune { get; set; }
        [Parameter] public VillageDto? SelectedVillage { get; set; }
        [Parameter] public EntNameDtoUpdate model { get; set; } = new();
        private CustomValidation? customValidation;
        protected List<string> salutations = ["Mr.", "Mrs.", "Miss.", "Others"];
       
        protected async Task OnSaveChanges()
        {
            try
            {
                model!.EntType = "Other Name";
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
                var message = await NameService!.UpdateAsync(model.EntId, model);

                if (message.IsSuccess)
                {
                    NotificationService.Notify(Radzen.NotificationSeverity.Success, "Other Name", "Updated successfully!");
                    await SelectedIndex();
                    return;
                }
                else
                {
                    customValidation?.ClearErrors();
                    NotificationService.Notify(Radzen.NotificationSeverity.Error, "Other Name", "Try to update failed!");
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(Radzen.NotificationSeverity.Error, "Exception", ex.Message);

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
        void SelectVillage(VillageDto e)
        {
            if (e is not null)
            {
                SelectedVillage = e;
            }
            else
            {
                SelectedVillage = null;
            }
            StateHasChanged();
        }
        //Salutation implements
        [Parameter] public IEnumerable<string> Salutations { get; set; } = [];
        protected string? newSalution = "";
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
    }
}