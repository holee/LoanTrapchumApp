using Applications.Interfaces.Lists;
using Applications.Requests.Lists.Entities;
using Domains.Models.Lists;
using Mapster;
using Radzen;

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

            var response = await depService.CreateAsync(model);

            if (response!.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Success, "CreateNewUser Success");
                DialogService.Close(true);
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Success, response.ErrMessage);
            }
        }


    }
}