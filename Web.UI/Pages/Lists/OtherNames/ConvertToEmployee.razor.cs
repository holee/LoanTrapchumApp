using Applications.Features.Branches.Dtos;
using Applications.Features.Branches.Queries.GetBranchAsPaging;
using Applications.Features.Departments.Dto;
using Applications.Features.Departments.Queries.GetDepartmentAsPaging;
using Applications.Features.OtherNames.Dto;
using Applications.Responses.ViewModels.OtherNames;
using Applications.Services.Lists;

namespace Web.UI.Pages.Lists.OtherNames
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
        IEnumerable<VBranch> branches = [];

        IEnumerable<VDepartment> depts = [];

        VOtherNames? name = new VOtherNames();

        ConvertCreateEmployee model = new ConvertCreateEmployee();

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
                NotificationService.Notify(NotificationSeverity.Success, "Success", rsp.Data);
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
                var rsp = await _nameService.GetDetailAsync(id);

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

            var command = new GetBranchAsPagingQuery(0, new QueryParam(0, 0, string.Empty));

            var rsp = await mediator.Send(command);

            branches = rsp.Items;

        }

        async Task GetDepartment()
        {

            var command = new GetDepartmentAsPagingQuery(0, new QueryParam(0, 0, string.Empty));

            var rsp = await mediator.Send(command);

            depts = rsp.Items;

        }

    }
}