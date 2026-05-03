using Applications.Features.Departments.Commands.EditDepartment;
using Applications.Features.Departments.Dto;
using Applications.Features.Departments.Queries.GetDepartmentById;
using Applications.Interfaces.Lists;
using Domains.Models.Lists;
using Mapster;

namespace TrapchumApp.Pages.Lists.Departments
{
    public partial class DepartmentModalUpdate
    {

        private DepartmentDtoUpdate model = new DepartmentDtoUpdate();

        private readonly IDepartmentService _depService;

        public DepartmentModalUpdate(IDepartmentService departmentService)
        {
            _depService = departmentService;
        }

        [Parameter] public int Id { get; set; }
        protected async Task Update()
        {
            try
            {

                var confirm = await DialogService.Confirm($"Do you want to update with ID#{Id}?", options: new ConfirmOptions
                {
                    CancelButtonText = "Cancel",

                    OkButtonText = "OK",
                });

                if (confirm != true)
                {
                    return;
                }

                var req = model.Adapt<Department>();

                var command = new EditDepartmentCommand(model);

                var response = await mediator.Send(command);

                if (response.IsSuccess())
                {
                    NotificationService.Notify(NotificationSeverity.Success, "Department was updated successful!");

                    DialogService.Close(true);
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, response.GetMessage());
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.Message);
            }
        }


        protected override async Task OnInitializedAsync()
        {
            try
            {

                var command = new GetDepartmentById(Id, 0);

                var rsp = await mediator.Send(command);

                if (rsp.IsSuccess())
                {
                    model = rsp.Value.Adapt<DepartmentDtoUpdate>();
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, "Department", rsp.GetMessage());
                }

            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Department", ex.Message);

            }
            await base.OnInitializedAsync();
        }

    }
}