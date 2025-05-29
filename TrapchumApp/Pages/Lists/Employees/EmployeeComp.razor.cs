using Applications.Requests.Lists.Addreses;
using Applications.Requests.Lists.Entities;
using Applications.Requests.Lists.OtherNames;
using Applications.Responses.Lists.Entities;
using Applications.Responses.Paginations;
using Applications.Services.Lists;
using Domains.Models.Files;
using Domains.Models.Lists;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace TrapchumApp.Pages.Lists.Employees
{
    public partial class EmployeeComp
    {
        [Parameter, EditorRequired] public EntityNamesCreate model { get; set; } = new();
        [Parameter] public EventCallback<bool> OnClose { get; set; }

        private readonly ISalutationServiceImpl _salutationService;
        private readonly INationalityServiceImpl _nationalityService;
        private readonly IDepartmentServiceImpl _departmentService;
        public EmployeeComp(ISalutationServiceImpl salutationService, INationalityServiceImpl nationalityService, IDepartmentServiceImpl departmentService)
        {
            _salutationService = salutationService;
            _nationalityService = nationalityService;
            _departmentService = departmentService;
        }
        long? fileSize;
        IEnumerable<int> relations = new List<int> { 1, 2, 3, 4 };
        IEnumerable<string> supervisors = new List<string> { "Supervisor", "Manager", "CEO" };
        IEnumerable<string> genders = new List<string> { "Male", "Female", "Other" };
        IEnumerable<string> maritalStatuses = new List<string> { "Single", "Married", "Divorced", "Widowed" };
        IEnumerable<VillageDto> Villages = [];
        IEnumerable<ProvinceDto> Provinces = [];
        IEnumerable<BranchDto> Branches = [];
        IEnumerable<DepartmentDto> Departments = [];
        IEnumerable<Nationality> Nationalities = [];
        IEnumerable<OcupationResponse> Occuptions = [];
        IEnumerable<string> Cities = [];
        protected FileModel? fileModelDTO;
        protected EditForm? editForm;
        protected string? strText = "";
        protected bool IsActive = false;

        protected override async Task OnInitializedAsync()
        {
            await GetAdditionalInformation();

            await base.OnInitializedAsync();
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

            Occuptions = await _employeeService!.GetOccupationsAsync("Position");

            await GetSalutations();

            StateHasChanged();
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

        private async Task GetSalutations()
        {
            var rsp = await _salutationService.GetsSalutions();

            if (rsp.IsSuccess)
            {
                Salutations = rsp!.Data ?? [];
            }
            else
            {
                Salutations = [];
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
    }
}