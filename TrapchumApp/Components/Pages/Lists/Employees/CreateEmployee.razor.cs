using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Applications.Requests.Lists.Employees;
using Domains.Models.Files;
using Applications.Requests.Lists.Addreses;
using Applications.Requests.Lists.Entities;
using Domains.Models.Addresses;
using Domains.Models.Lists;
using Applications.Interfaces.Employees;
using Applications.Requests.Lists.OtherNames;
using Applications.Responses.Lists.Entities;
using Applications.Services.Employees;

namespace TrapchumApp.Components.Pages.Lists.Employees
{
    public partial class CreateEmployee
    {
        
        [Parameter] public IEnumerable<ProvinceDto> Provinces { get; set; } = [];
        [Parameter] public IEnumerable<BranchDto> Branches { get; set; } = []; 
        [Parameter] public IEnumerable<DepartmentDto> Departments { get; set; } = [];
        [Parameter] public IEnumerable<Country> Countries { get; set; } = [];
        [Parameter] public IEnumerable<Nationality> Nationalities { get; set; } = [];
        [Parameter] public IEnumerable<OcupationResponse> Occuptions { get; set; } = [];
        [Parameter] public IEnumerable<string> Cities { get; set; } = [];
        [Parameter] public EventCallback Close { get; set; }
        [Inject] private HttpClient? http { get; set; }
        
        protected EntityNamesCreate model = new EntityNamesCreate();
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
                var createResponse = await _employeeService.CreateAsync(model);

                if (createResponse.IsSuccess)
                {
                    NotificationService.Notify(Radzen.NotificationSeverity.Success, createResponse.Data);
                    model = new();
                    Reload();
                }
                else
                {
                    NotificationService.Notify(Radzen.NotificationSeverity.Error, createResponse.ErrMessage);
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(Radzen.NotificationSeverity.Error, ex.Message);
            }

        }
    }
}
