using Applications.Features.Departments.Commands.CreateDepartment;
using Applications.Features.Departments.Dto;
using Applications.Interfaces.Lists;
using Domains.Models.Lists;
using Mapster;
using TrapchumApp.Helpers.Extensions;

namespace TrapchumApp.Pages.Lists.Departments
{
    public partial class DepartmentModalCreate
    {
        private readonly IDepartmentService depService;

        public DepartmentModalCreate(IDepartmentService departmentService)
        {
            depService = departmentService;
        }

        private DepartmentDtoCreate Req = new DepartmentDtoCreate();
        protected async Task Create()
        {

            var model = Req.Adapt<Department>();

            var command = new CreateDepartmentCommand(Req);

            var response = await mediator.Send(command);

            if (response.IsSuccess())
            {
                NotificationService.Notify(NotificationSeverity.Success, "Department record was created successful!");
              
                DialogService.Close(true);
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, response.GetMessage());
            }

        }


    }
}