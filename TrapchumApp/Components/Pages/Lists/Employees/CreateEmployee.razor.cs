using Microsoft.AspNetCore.Components;
using TCLMS.Shared.Entities.Dtos.Lists;
using TCLMS.Shared.Entities.Dtos.Lists.Entities;
using TCLMS.Shared.Entities.Dtos.Lists.Addreses;
using Microsoft.AspNetCore.Components.Forms;
using TCLMS.Shared.Entities.Models.Addresses;
using Microsoft.AspNetCore.Components.Web;
using TCLMS.Shared.Entities.Models.Loans;
using System.Net.Http.Json;
using TCLMS.Shared.Entities.Dtos.Lists.Employees;
using TCLMS.Shared.Entities.Models.Files;

namespace TCLMS.Front.Pages.Lists.Employees
{
    public partial class CreateEmployee
    {
        [Parameter] public IEnumerable<ProvinceDto> Provinces { get; set; } = [];
        [Parameter] public IEnumerable<BranchDto> Branches { get; set; } = []; 
        [Parameter] public IEnumerable<DepartmentDto> Departments { get; set; } = [];
        [Parameter] public IEnumerable<Country> Countries { get; set; } = [];
        [Parameter] public IEnumerable<Nationality> Nationalities { get; set; } = [];
        [Parameter] public IEnumerable<StOcupationDto> Occuptions { get; set; } = [];
        [Parameter] public IEnumerable<string> Cities { get; set; } = [];
        [Parameter] public EventCallback Close { get; set; }
        [Inject] private HttpClient? http { get; set; }
        
        protected EmployeeDtoCreate model = new EmployeeDtoCreate();
        protected FileModel? fileModelDTO;
        protected EditForm? editForm;
        protected string? strText = "";
        protected bool IsActive = false;
        protected IEnumerable<DistrictDto> Districts = [];
        protected IEnumerable<CommuneDto> Communes = [];
        protected IEnumerable<VillageDto> Villages = [];
        protected override async Task OnInitializedAsync()
        {
            model.Salutation = "Mr";
            model.Nationality = "Khmer";
            await base.OnInitializedAsync();
        }
        protected async Task FormValidSubmit()
        {
            try
            {
                model.EntType = "Employee";
                var createResponse = await _employeeService.CreateEmployee(model);

                if (createResponse.IsSuccess)
                {
                    NotificationService.Notify(Radzen.NotificationSeverity.Success, createResponse.Data!.Message);
                    model = new();
                    Reload();
                }
                else
                {
                    var error = createResponse.Errors!.FirstOrDefault();
                    NotificationService.Notify(Radzen.NotificationSeverity.Error, error);
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(Radzen.NotificationSeverity.Error, ex.Message);
            }

        }
    }
}
