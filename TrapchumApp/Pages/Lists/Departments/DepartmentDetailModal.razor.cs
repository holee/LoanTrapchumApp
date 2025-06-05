using Applications.Interfaces.Lists;
using Applications.Responses.Lists.Entities;
using Applications.Responses.ViewModels.Departments;
using Mapster;
using Microsoft.AspNetCore.Components;
using Radzen;

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
            var resultResponse = await _departmentService.GetDepartmentDetails(Id);

            if (resultResponse.IsSuccess)
            {
                department = resultResponse.Data;
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, resultResponse.ErrMessage);
            }

            await base.OnInitializedAsync();
        }
    }
}