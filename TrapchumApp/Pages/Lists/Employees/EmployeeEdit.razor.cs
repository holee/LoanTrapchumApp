using Applications.Features.Addresses.Responses;
using Applications.Features.Employees.Dto;
using Applications.Features.Entities;
using Applications.Responses.Lists.Entities;
using Applications.Responses.Paginations;
using Applications.Services.Lists;
using Domains.Models.Addresses;
using Domains.Models.Files;
using Domains.Models.Lists;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace TrapchumApp.Pages.Lists.Employees
{
    public partial class EmployeeEdit
    {
        [Parameter] public EventCallback<bool> OnClose { get; set; }
        [Parameter] public string? EmployeeId { get; set; }
        private readonly INationalityServiceImpl _nationalityService;
        private readonly IDepartmentServiceImpl _departmentService;
        private readonly ISalutationServiceImpl _salutationService;
        public EmployeeEdit(INationalityServiceImpl nationalityService,
                              IDepartmentServiceImpl departmentService,
                              ISalutationServiceImpl salutationService)
        {
            _nationalityService = nationalityService;
            _departmentService = departmentService;
            _salutationService = salutationService;
        }

        long? fileSize;
        IEnumerable<DistrictDto> Districts = [];
        IEnumerable<CommuneDto> Communes = [];
        IEnumerable<VillageDto> Villages = [];
        IEnumerable<ProvinceDto> Provinces = [];
        IEnumerable<BranchDto> Branches = [];
        IEnumerable<DepartmentDto> Departments = [];
        IEnumerable<Country> Countries = [];
        IEnumerable<Nationality> Nationalities = [];
        IEnumerable<OcupationResponse> Occuptions = [];
        IEnumerable<string> Cities = [];
        protected EntityEmployeeCreate model = new EntityEmployeeCreate
        {
            Id=$"emp-{Guid.CreateVersion7()}"
        };
        protected FileModel? fileModelDTO;
        protected EditForm? editForm;
        protected string? strText = "";
        protected bool IsActive = false;
        private IEnumerable<int> relations = new List<int> { 1, 2, 3, 4 };
        private IEnumerable<string> supervisors = new List<string> { "Supervisor", "Manager", "CEO" };
        private IEnumerable<string> genders = new List<string> { "Male", "Female", "Other" };
        private IEnumerable<string> maritalStatuses = new List<string> { "Single", "Married", "Divorced", "Widowed" };
        async void SelectedProvinceChanged(object arg)
        {
            if (arg is not null)
            {
                model.ProInKhmer = ((ProvinceDto)arg).ProInKhmer;
                Districts = await _addressService!.GetDistrictsByProvinceId(((ProvinceDto)arg).ProvinceId);
                StateHasChanged();
            }

        }
        async void SelectedDistrictChanged(object arg)
        {
            if (arg is not null)
            {
                model.DisInKhmer = ((DistrictDto)arg).DisInKhmer;
                Communes = await _addressService!.GetCommunesByDistrictId(((DistrictDto)arg).DistrictId);
                StateHasChanged();
            }

        }
        async void SelectedCommuneChanged(object arg)
        {
            if (arg is not null)
            {
                model.ComInKhmer = ((CommuneDto)arg).ComInKhmer;
                Villages = await _addressService!.GetVillagesByCommuneId(((CommuneDto)arg).CommuneId);
                StateHasChanged();
            }

        }
        void SelectedVillageChanged(object arg)
        {
            if (arg is not null)
            {
                model.VilInKhmer = ((VillageDto)arg).VilInKhmer;
                StateHasChanged();
            }

        }

        void OnProgress(UploadProgressArgs args, string name)
        {
            if (args.Progress == 100)
            {
                foreach (var file in args.Files)
                {

                }
            }
        }
        void SetFirstNameToName()
        {
            model.Name = model.FirstName;
        }

        void OnChange(string value, string name)
        {

        }

        void OnError(UploadErrorEventArgs args, string name)
        {
        }

        [Parameter] public EventCallback Reloaded { get; set; }

        async void Reload()
        {
            await Reloaded.InvokeAsync();
        }
        private async Task CloseTab()
        {
            await OnClose.InvokeAsync(false);
        }
        IEnumerable<string> Salutations { get; set; } = [];

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
            var salutations = await _salutationService.GetsSalutions();

            if (salutations.IsSuccess)
            {
                Salutations = salutations!.Data ?? [];
            }
            else
            {
                Salutations = [];
            }

        }


        protected override async Task OnInitializedAsync()
        {
            await GetAdditionalInformation();

            await GetEditEmployee();

            await base.OnInitializedAsync();
        }


        async Task GetEditEmployee()
        {
            var rsp = await _employeeService.GetEmployeeForUpdate(EmployeeId);

            if (rsp.IsSuccess)
            {
                model = rsp.Data!;
            }

            Districts = model is not null && model.ProvinceRefId == 0 ? [] : await _addressService.GetDistrictsByProvinceId(model?.ProvinceRefId);
            Communes = model is not null && model.DistrictRefId == 0 ? [] : await _addressService.GetCommunesByDistrictId(model?.DistrictRefId);
            Villages = model is not null && model.CommuneRefId == 0 ? [] : await _addressService.GetVillagesByCommuneId(model?.CommuneRefId);

        }

        async Task GetAdditionalInformation()
        {
            var results = await _branchService.GetBranchesAsync<BranchDto>(0, new QueryParam(0, 25, string.Empty));

            if (results.IsSuccess)
            {
                Branches = results!.Data!.Items;
            }
            var deptResponse = await _departmentService.GetDepartments<DepartmentDto>(0, 0);

            if (deptResponse.IsSuccess)
            {
                Departments = deptResponse.Data!;
            }

            Nationalities = await _nationalityService!.GetNationalities();

            Provinces = await _addressService.GetsProvinces();

            Occuptions = await _employeeService!.GetOccupationsAsync("Position");

            var rsp = await _addressService.GetCountries(105);

            if (rsp.IsSuccess)
            {
                Countries = rsp.Data ?? [];
            }

            Cities = await _addressService.GetCities();

            await GetSalutations();

            StateHasChanged();
        }



        protected async Task FormValidSubmit()
        {
            try
            {

                var rsp = await _employeeService.UpdateAsync(model);

                if (rsp.IsSuccess)
                {
                    NotificationService.Notify(NotificationSeverity.Success, rsp.Data);

                    await CloseTab();

                    Reload();
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, rsp.ErrMessage);
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.Message);
            }

        }
    }
}
