using Applications.Extensions;
using Applications.Features.AdditionalInformations.Dto;
using Applications.Features.Addresses.Dto;
using Applications.Features.Branches.Dtos;
using Applications.Features.Branches.Queries.GetBranchAsPaging;
using Applications.Features.Departments.Dto;
using Applications.Features.Departments.Queries.GetDepartmentAsPaging;
using Applications.Features.Employees.Dto;
using Domains.Models.Files;
using Domains.Models.Lists;

namespace Web.UI.Pages.Lists.Employees;

public partial class EmployeeComp(ILogger<EmployeeComp> logger)
{
    [Parameter, EditorRequired] public EntityEmployeeCreate model { get; set; } = new();
    [Parameter] public EventCallback<bool> OnClose { get; set; }

    long? fileSize;
    IEnumerable<int> relations = new List<int> { 1, 2, 3, 4 };
    IEnumerable<string> supervisors = new List<string> { "Supervisor", "Manager", "CEO" };
    IEnumerable<string> genders = new List<string> { "Male", "Female", "Other" };
    IEnumerable<string> maritalStatuses = new List<string> { "Single", "Married", "Divorced", "Widowed" };
    IEnumerable<VillageDto> Villages = [];
    IEnumerable<ProvinceDto> Provinces = [];
    IEnumerable<VBranch> Branches = [];
    IEnumerable<VDepartment> Departments = [];
    IEnumerable<Nationality> Nationalities = [];
    IEnumerable<OccupationDto> _occupations = [];
    IEnumerable<string> Cities = [];
    protected FileModel? fileModelDTO;
    protected EditForm? editForm;
    protected string? strText = "";
    protected bool IsActive = false;
    private string tempPath = "/assets/profiles/profile.png";

    protected override async Task OnInitializedAsync()
    {
        await GetAdditionalInformation();

        if (model.Photo.IsNotNullOrEmpty())
        {
            tempPath = model.Photo ?? "/assets/profiles/profile.png";
        }

        await base.OnInitializedAsync();
    }


    async Task GetAdditionalInformation()
    {

        var command = new GetBranchAsPagingQuery(null, new QueryParam(0, 0));

        var results = await mediator.Send(command);

        Branches = results.Items;

        var query = new GetDepartmentAsPagingQuery(null, new QueryParam(0, 0));

        var rsp1 = await mediator.Send(query);

        Departments = rsp1.Items;

        StateHasChanged();
    }

    void SetFirstNameToName()
    {
        model.Name = model.FirstName;
    }
}