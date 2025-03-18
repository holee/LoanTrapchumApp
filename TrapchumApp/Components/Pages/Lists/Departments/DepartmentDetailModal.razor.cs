using Applications.Interfaces.Lists;
using Applications.Requests.Lists.Entities;
using Mapster;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace TrapchumApp.Components.Pages.Lists.Departments
{
    public partial class DepartmentDetailModal
    {
        [Parameter] public int Id { get; set; }
        private DepartmentDto? department = new DepartmentDto();
        private readonly IDepartmentService _departmentService;

        public DepartmentDetailModal(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        protected override async Task OnInitializedAsync()
        {
            var resultResponse = await _departmentService!.GetDepartments(Id);

            if (resultResponse.IsSuccess)
            {
                department = resultResponse.Data.Adapt<DepartmentDto>();
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, resultResponse.ErrMessage);
            }

            await base.OnInitializedAsync();
        }
    }
}