using Applications.Interfaces.Lists;
using Applications.Requests.Lists.Entities;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Departments;
using Radzen;
using System.Threading.Tasks;

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
        int counts = 0;
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

            await GetDepartments(searchText);
        }




        protected async Task TextChanged(string filter)
        {
            searchText = filter;

            await GetDepartments(filter);
        }


        private async Task GetDepartments(string? filter = null)
        {
            try
            {
                isLoading = true;
                var param = new QueryParam(pageIndex, pageSize, sortField, includeInactive, sortDescending, filter);
                var resultResponse = await _depService.GetDepartments(param);
                counts = resultResponse.Count;
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
            bool? resultModal = await DialogService.OpenAsync<DepartmentModalUpdate>("Edit",
               new Dictionary<string, object> { { "Id", departmentId } },
               new DialogOptions
               {
                   Draggable = true,
                   Resizable = true,

               });
            if (resultModal is bool reslt && reslt==true)
            {
                await GetDepartments(searchText);
            }

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

        private async Task LoadDepartmentData(LoadDataArgs args)
        {

            pageIndex = args.Skip ?? 0;

            pageSize = args.Top ?? 10;

            if (!string.IsNullOrEmpty(args.OrderBy))
            {
                sortField = args.OrderBy.Split(" ")[0];

                string dir = args.OrderBy.Split(" ")[1];

                sortDescending = dir.Contains("asc") ? false : true;
            }


            var filters = new List<string>();

            if (args.Filters != null && args.Filters.Any())
            {
                foreach (var filter in args.Filters)
                {
                    if (filter.Property == nameof(VDepartment.DepartmentName) ||
                        filter.Property == nameof(VDepartment.Description) ||
                        filter.Property == nameof(VDepartment.DepartmentId))
                    {
                        filters.Add(filter.FilterValue?.ToString() ?? string.Empty);
                    }
                }
            }

            // Include search text from PageDropDown
            if (!string.IsNullOrEmpty(searchText))
            {
                filters.Add(searchText);
            }

            // Combine filters into a single string (space-separated)
            var filterString = filters.Any() ? string.Join(" ", filters.Where(f => !string.IsNullOrEmpty(f))) : null;


            await GetDepartments(filterString);

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
                    NotificationService.Notify(NotificationSeverity.Success, "CreateNewUser Success");
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
