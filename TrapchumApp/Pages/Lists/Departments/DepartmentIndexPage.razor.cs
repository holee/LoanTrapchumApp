using Applications.Extensions;
using Applications.Extensions.Attrs;
using Applications.Features.Branches.Dtos;
using Applications.Features.Departments.Commands.DeleteDepartment;
using Applications.Features.Departments.Dto;
using Applications.Features.Departments.Queries.GetDepartmentAsPaging;
using Applications.Interfaces.Lists;
using Radzen.Blazor;

namespace TrapchumApp.Pages.Lists.Departments
{
    public partial class DepartmentIndexPage
    {

        private readonly IDepartmentService _depService;

        public DepartmentIndexPage(IDepartmentService departmentService)
        {
            _depService = departmentService;
        }

        private List<VDepartment> Departments = [];
        private IEnumerable<BranchDto>? Branches;
        private DepartmentDtoCreate Req = new DepartmentDtoCreate();
        private string? searchText = string.Empty;
        private bool includeInactive = false;
        private RadzenDataGrid<VDepartment> _depRef = default!;
        private int? _lastDepId = 0;
        int pageIndex = 0;
        int pageSize = 15;
        bool sortDescending = true;
        string sortField = nameof(VDepartment.ModifiedDT);
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
            var dia = await DialogService!.OpenAsync<DepartmentModalCreate>("Department Create",
               options: new DialogOptions
               {
                   Draggable = true,
                   Resizable = true,
               });

            if (dia is VDepartment dep && dep is not null)
            {
                Departments.Insert(0, dep);

                _lastDepId = dep.DepartmentId;

                await _depRef.Reload();
            }

        }


        void OnCellRender(RowRenderEventArgs<VDepartment> e)
        {
            if (e.Data!.DepartmentId == _lastDepId)
            {
                e.Attributes.Add("class", "rz-state-highlight rz-data-row recently-edited");
            }
        }

        void OnRowSelect(VDepartment args) { }

        void OnRowRender(RowRenderEventArgs<VDepartment> args)
        {

            if (args.Data!.ModifiedDT.IsRecently())
            {
                args.Attributes.Add("class", "rz-state-highlight rz-data-row recently-edited");

            }

            args.Data.RowIndex = args.Index + 1; // store 1-based index
        }

        protected async Task ShowInactiveAndActive(bool e)
        {
            includeInactive = e;

            await GetDepartments(searchText);
        }




        protected async Task SearchChanged(string filter)
        {
            searchText = filter;

            await GetDepartments(filter);
        }


        private async Task GetDepartments(string? filter = null)
        {
            isLoading = true;
            var param = new QueryParam(pageIndex, pageSize, sortField, includeInactive, sortDescending, filter);
            var command = new GetDepartmentAsPagingQuery(null, param);
            var rsp = await mediator.Send(command);
            counts = rsp.Count;
            Departments = rsp.Items ?? [];
            isLoading = false;
            StateHasChanged();
        }
        protected async Task Edit(int departmentId)
        {
            bool? resultModal = await DialogService.OpenAsync<DepartmentModalUpdate>("Edit",
               new Dictionary<string, object> { { "Id", departmentId } },
               new DialogOptions
               {
                   Draggable = true,
               });
            if (resultModal is bool reslt && reslt == true)
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

            (pageIndex, pageSize, sortField, sortDescending) = args.Page();

            var filterColumns = args.GetFilterValue(nameof(VDepartment.DepartmentName), nameof(VDepartment.Description));

            var filterString = filterColumns ?? string.Empty;

            await GetDepartments(filterString);

        }

        protected async Task Delete(int departmentId)
        {

            var confirm = await DialogService.Confirm("Do you want to delete?", "Delete", new ConfirmOptions
            {
                CancelButtonText = "Cancel",
                OkButtonText = "OK",
            });

            if (confirm == false) return;

            var command = new DeleteDepartmentCommand(departmentId, 0);

            var result = await mediator.Send(command);

            if (result.IsSuccess())
            {
                NotificationService.Notify(NotificationSeverity.Success, result.Value);

                await GetDepartments(searchText);
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, result.GetMessage());
            }
        }

    }
}
