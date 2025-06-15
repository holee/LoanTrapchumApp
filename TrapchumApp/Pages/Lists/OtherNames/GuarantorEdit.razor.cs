using Applications.Extensions;
using Applications.Requests.Lists.OtherNames;
using Applications.Services.Lists;
using Domains.Models.Lists;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace TrapchumApp.Pages.Lists.OtherNames
{
    public partial class GuarantorEdit
    {


        private readonly IGuarantorServiceImpl _gService;

        private readonly ISalutationServiceImpl _salutationService;
        public GuarantorEdit(IGuarantorServiceImpl gService, ISalutationServiceImpl salutationService)
        {
            _gService = gService;

            _salutationService = salutationService;
        }

        [Parameter] public string? NameId { get; set; }
        [Parameter] public EntityGuarantorCreate model { get; set; } = new();
        [Parameter] public IEnumerable<string> Salutations { get; set; } = [];
        [Parameter] public EventCallback<int> Closing { get; set; }

        protected string? newSalution = "";

        protected List<string> salutations = ["Mr.", "Mrs.", "Miss.", "Others"];

        protected override async Task OnInitializedAsync()
        {

            if (model.Id.IsNullOrEmpty() || NameId.IsNotNullOrEmpty())
            {
                var resp = await _gService.GetGuarantorEditAsync(NameId);

                if (resp.IsSuccess)
                {
                    model = resp.Data!;
                }
            }

            await base.OnInitializedAsync();
        }

        protected async Task OnSaveChanges()
        {
            try
            {
                var resultResponse = await _gService.UpdateAsync(model);

                if (resultResponse.IsSuccess)
                {
                    NotificationService.Notify(NotificationSeverity.Success, resultResponse.Data);
                    await TabClosing();
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, resultResponse.ErrMessage);
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Exception", ex.Message);

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

        async Task TabClosing()
        {
            await Closing.InvokeAsync(2);
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



        void SetFirstName()
        {
            model.FirstName = model.Name;
        }
    }
}

//async void GetDistrictByProvince(object e)
//{
//    if (e is not null)
//    {
//        Model.ProInKhmer = ((ProvinceDto)e).ProInKhmer;
//        var id = ((ProvinceDto)e).ProvinceId;
//        DistrictList = await _addressService!.GetDistrictsByProvinceId(id);
//    }
//    StateHasChanged();
//}
//async void GeCommunesByDistrict(object e)
//{
//    if (e is not null)
//    {
//        Model.DisInKhmer = ((DistrictDto)e).DisInKhmer;
//        var id = ((DistrictDto)e).DistrictId;
//        CommuneList = await _addressService!.GetCommunesByDistrictId(id);
//    }
//    StateHasChanged();
//}

//async void GetVillagesByCommune(object e)
//{
//    if (e is not null)
//    {
//        Model.ComInKhmer = ((CommuneDto)e).ComInKhmer;
//        var id = ((CommuneDto)e).CommuneId;
//        VillageList = await _addressService!.GetVillagesByCommuneId(id);
//    }
//    StateHasChanged();
//}
//void GetVillagesName(object e)
//{
//    if (e is not null)
//    {
//        Model.VilInKhmer = ((VillageDto)e).VilInKhmer;
//    }
//    StateHasChanged();
//}

//async void SelectProvince(ProvinceDto e)
//{
//    SelectedProvince = e;
//    if (SelectedProvince is not null)
//    {
//        DistrictList = await _addressService!.GetDistrictsByProvinceId(SelectedProvince.ProvinceId);
//    }
//    else
//    {
//        SelectedProvince = null;
//        DistrictList = null;
//    }
//    StateHasChanged();

//}
//async Task SelectDistrict(DistrictDto e)
//{
//    if (e is not null)
//    {
//        SelectedDistrict = e;
//        CommuneList = await _addressService!.GetCommunesByDistrictId(SelectedDistrict.DistrictId);
//    }
//    else
//    {
//        SelectedDistrict = null;
//        CommuneList = null;
//    }
//    StateHasChanged();
//}
//async Task SelectCommune(CommuneDto e)
//{
//    if (e is not null)
//    {
//        SelectedCommune = e;
//        VillageList = await _addressService!.GetVillagesByCommuneId(SelectedCommune.CommuneId);
//    }
//    else
//    {
//        SelectedCommune = null;
//        VillageList = null;
//    }
//    StateHasChanged();
//}
//void SelectVillage(VillageDto e)
//{
//    if (e is not null)
//    {
//        SelectedVillage = e;
//    }
//    else
//    {
//        SelectedVillage = null;
//    }
//    StateHasChanged();
//}
//Salutation implements
