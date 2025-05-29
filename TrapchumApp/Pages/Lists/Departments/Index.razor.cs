using Applications.Interfaces.Lists;
using Applications.Requests.Lists.Entities;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Departments;
using Radzen;

namespace TrapchumApp.Pages.Lists.Departments
{
    public partial class Index
    {

        private readonly IDepartmentService _depService;

        public Index(IDepartmentService departmentService)
        {
            _depService = departmentService;
        }

        protected List<VDepartment> Departments = [];
        protected IEnumerable<BranchDto>? Branches;
        protected DepartmentDtoCreate Req = new DepartmentDtoCreate();
        protected string? searchText = string.Empty;
        protected bool includeInactive = false;
        int pageIndex = 0;
        int pageSize = 10;
        bool sortDescending = false;
        string sortField = "DepartmentId";
        bool isLoading = false;
        protected override async Task OnInitializedAsync()
        {
            try
            {
                await GetDepartments();
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.Message);
            }
            await base.OnInitializedAsync();
        }

        protected async Task ShowCreate()
        {
            var resultModal = await DialogService!.OpenAsync<DepartmentModalCreate>("Department",
               options: new DialogOptions
               {
                   Draggable = true,
                   Resizable = true,
               });
            if (resultModal != null)
            {
                await InvokeAsync(StateHasChanged);
            }
        }
        protected async Task ShowInactiveAndActive(bool e)
        {
            includeInactive = e;
            await OnInitializedAsync();
            await InvokeAsync(StateHasChanged);
        }

        protected async Task TextChanged()
        {
            await GetDepartments(searchText);
        }


        private async Task GetDepartments(string? filter = null)
        {
            try
            {
                isLoading = true;
                var param = new QueryParam(pageIndex, pageSize, sortField, sortDescending, includeInactive, filter);
                var resultResponse = await _depService.GetDepartments(param);
                var counts = resultResponse.Count;
                Departments = resultResponse.Items ?? [];
                isLoading = false;
            }
            catch
            {
                isLoading = false;
            }


        }
        protected async Task Edit(int departmentId)
        {
            var resultModal = await DialogService!.OpenAsync<DepartmentModalUpdate>("Edit Branch",
               new Dictionary<string, object> { { "Id", departmentId } },
               new DialogOptions
               {
                   Draggable = true,
                   Resizable = true,

               });
            if (resultModal != null)
            {
                await OnInitializedAsync();
                Console.WriteLine("Updated...");
            }
            await InvokeAsync(StateHasChanged);

        }
        protected async Task Details(int departmentId)
        {
            await DialogService.OpenAsync<DepartmentDetailModal>("Department", new Dictionary<string, object>
            {
                {"id",departmentId }
            }, new DialogOptions
            {
                CloseDialogOnEsc = true,
                Draggable = true,
                Resizable = false
            });
        }
        protected async Task Delete(int departmentId)
        {
            var confirm = await DialogService.Confirm("Do you want to delete?", "Delete", new ConfirmOptions
            {
                CancelButtonText = "Cancel",
                OkButtonText = "OK",
            });
            if (confirm == true)
            {
                var result = await _depService.DeleteAsync(departmentId);

                if (result.IsSuccess)
                {
                    NotificationService.Notify(NotificationSeverity.Success,"CreateNewUser Success");
                    await GetDepartments(searchText);
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Success, result.ErrMessage);
                }
            }

        }

    }
}
