using Applications.Requests.Lists.OtherNames;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Branches;
using Applications.Responses.ViewModels.OtherNames;
using Applications.Services.Lists;
using Domains.Models.Lists;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace TrapchumApp.Pages.Lists.OtherNames
{
    public partial class ConvertToEmployee
    {
        [Parameter] public string? Id { get; set; } = string.Empty;


        private readonly IOtherNameServiceImpl _nameService;
        private readonly IDepartmentServiceImpl _departmentService;
        public ConvertToEmployee(IOtherNameServiceImpl otherNameService, IDepartmentServiceImpl departmentService)
        {
            _nameService = otherNameService;
            _departmentService = departmentService;
        }
        IEnumerable<VBranchResponse> branches = [];
        IEnumerable<Department> depts = [];
        VOtherNames? name = new VOtherNames();
        EmployeeConvertCreate model = new EmployeeConvertCreate();
        IEnumerable<string> Statuses = ["Single", "Married", "Divorced", "Widow"];
        protected override async Task OnParametersSetAsync()
        {
            await GetName(Id);
            await GetBranches();
            await GetDepartment();
            await base.OnParametersSetAsync();
        }

        async Task HandleFormSubmit()
        {
            var rsp = await _nameService.ConvertToEmployeeAsync(Id, model);

            if (rsp.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Success, "Success",rsp.Data);
                DialogService.Close(true);
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, "Error", rsp.ErrMessage);
            }
        }


        async Task GetName(string? id)
        {
            try
            {
                var rsp = await _nameService.GetDetailAsync<VOtherNames>(id);

                if (rsp.IsSuccess)
                {
                    name = rsp.Data!;
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, "Error", rsp.ErrMessage);
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Exception", ex.Message);
            }
        }

        async Task GetBranches()
        {
            var rsp = await _branchService.GetBranchesAsync<VBranchResponse>(0, new QueryParam(0, 0, string.Empty));
            if (rsp.IsSuccess)
            {
                branches = rsp.Data!.Items;
            }
            else
            {
                branches = [];
            }
        }

        async Task GetDepartment()
        {
            var rsp = await _departmentService.GetDepartments(0,0);

            if (rsp.IsSuccess)
            {
                depts = rsp.Data!;
            }
            else
            {
                depts = [];
            }
        }

    }
}