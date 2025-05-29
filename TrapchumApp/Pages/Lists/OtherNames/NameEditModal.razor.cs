using Applications.Requests.Lists.Addreses;
using Applications.Requests.Lists.OtherNames;
using Applications.Services.Lists;
using Domains.Models.Lists;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace TrapchumApp.Pages.Lists.OtherNames
{
    public partial class NameEditModal
    {
        [Parameter] public string? Id { get; set; }
        [Parameter] public EventCallback<int> SetSelectedIndexe { get; set; } 
        [Parameter] public EventCallback<bool> OnClose { get; set; }
        [Parameter] public IEnumerable<string> Salutations { get; set; } = [];

        private readonly IOtherNameServiceImpl _otherNameService;
        private readonly ISalutationServiceImpl _salutationService;
        public NameEditModal(IOtherNameServiceImpl otherNameService, ISalutationServiceImpl salutationService)
        {
            _otherNameService = otherNameService;
            _salutationService = salutationService;
        }

        EntityNamesCreate? model = new EntityNamesCreate();
        IEnumerable<ProvinceDto>? ProvinceList = [];
        IEnumerable<CommuneDto>? CommuneList = [];
        IEnumerable<DistrictDto>? DistrictList = [];
        IEnumerable<VillageDto>? VillageList = [];      
        private bool isValid = true;
        protected List<string> salutations = ["Mr.", "Mrs.", "Miss.", "Others"];
        protected override async Task OnInitializedAsync()
        {
            try
            {

                ProvinceList = await _addressService!.GetsProvinces();

                await GetEditNameById(Id);
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Exception", ex.Message);


            }

            await base.OnInitializedAsync();
        }
        protected async Task OnSaveChanges()
        {
            try
            {
                var rsp = await _otherNameService!.UpdateAsync(model!);

                if (rsp.IsSuccess)
                {
                    NotificationService.Notify(NotificationSeverity.Success, rsp.Data);
                    await Close();
                    return;
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, rsp.ErrMessage);
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Exception", ex.Message);
            }

        }

        protected async void SetSelectedIndex()
        {
            await SetSelectedIndexe.InvokeAsync(0);
        }

        async Task Close()
        {
            await OnClose.InvokeAsync(false);
            model = new();
        }

        async void SelectProvinceChanged(object e)
        {

            var province = e as ProvinceDto;

            if (province is not null)
            {
                model!.ProInKhmer = province.ProInKhmer;
                DistrictList = await _addressService!.GetDistrictsByProvinceId(province.ProvinceId);
            }
            else
            {
                DistrictList = [];
            }
            StateHasChanged();

        }
        async Task SelectDistrictChanged(object e)
        {
            var district = e as DistrictDto;

            if (district is not null)
            {
                model!.DisInKhmer = district.DisInKhmer;
                CommuneList = await _addressService!.GetCommunesByDistrictId(district.DistrictId);
            }
            else
            {
                CommuneList = [];
            }
            StateHasChanged();
        }
        async Task SelectCommuneChanged(object e)
        {

            var commune = e as CommuneDto;

            if (commune is not null)
            {
                model!.ComInKhmer = commune.ComInKhmer;
                VillageList = await _addressService!.GetVillagesByCommuneId(commune.CommuneId);
            }
            else
            {
                VillageList = [];
            }
            StateHasChanged();
        }
        //Salutation implements
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


        async Task GetEditNameById(string? id)
        {
            var rsp = await _otherNameService.GetEditNameByIdAsync(id);

            if (rsp.IsSuccess && rsp.Data is not null)
            {
                model = rsp.Data;
            }
            if (model is not null)
            {
                if (model!.ProvinceRefId!.HasValue)
                {
                    DistrictList = await _addressService.GetDistrictsByProvinceId(model?.ProvinceRefId);
                }

                if (model!.DistrictRefId.HasValue)
                {
                    CommuneList = await _addressService.GetCommunesByDistrictId(model?.DistrictRefId);
                }

                if (model!.CommuneRefId.HasValue)
                {
                    VillageList = await _addressService.GetVillagesByCommuneId(model?.CommuneRefId);
                }
            }

        }
    }
}