using Applications.Interfaces.Lists;
using Applications.Requests.Lists.Entities;
using Applications.Responses.Message;
using Domains.Models.Lists;
using Mapster;
using Microsoft.AspNetCore.Components;
using Radzen;

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

                var confirm = await DialogService.Confirm($"Do you want to update with ID#{Id}?",options:new ConfirmOptions
                {
                    CancelButtonText = "Cancel",

                    OkButtonText = "OK",
                });

                if(confirm != true)
                {
                    return;
                }

                var req = model.Adapt<Department>();

                var response = await _depService.UpdateAsync(req);

                if (response!.IsSuccess)
                {
                    NotificationService.Notify(NotificationSeverity.Success, "Department was updated successful!");

                    DialogService.Close(true);
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, response.ErrMessage);
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
                var response = await _depService.GetDepartmentById(Id);

                if (response.IsSuccess)
                {
                    model = response.Data!.Adapt<DepartmentDtoUpdate>();
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, "Department", response.ErrMessage);
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