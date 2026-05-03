using Applications.Features.Departments.Dto;
using Applications.Features.Departments.Queries.GetDepartmentById;
using Applications.Interfaces.Lists;

namespace TrapchumApp.Pages.Lists.Departments
{
    public partial class DepartmentDetailModal
    {
        [Parameter] public int Id { get; set; }

        private VDepartment? department = new VDepartment();

        private readonly IDepartmentService _departmentService;

        public DepartmentDetailModal(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        protected override async Task OnInitializedAsync()
        {
            var command = new GetDepartmentDetailsById(Id, null);

            var rsp = await mediator.Send(command);

            if (rsp.IsSuccess())
            {
                department = rsp.Value;
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, rsp.GetMessage());
            }

            await base.OnInitializedAsync();
        }
    }
}