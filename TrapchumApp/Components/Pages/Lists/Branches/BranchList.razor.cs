using Applications.Enumerations;
using Applications.Interfaces.Employees;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Branches;
using Microsoft.AspNetCore.Components;
using Radzen;
namespace TrapchumApp.Components.Pages.Lists.Branches
{
    public partial class BranchList
    {
        [Inject] IEmployeeService? EmployeeService { get; set; }
        protected string searchText { get; set; } = string.Empty;
        int pageIndex = 0;
        int counts = 0;
        string sortField = "BranchId";
        bool includeInactive = false;
        protected bool InActive = false;
        int pageSize = 10;
        bool sortDescending = false;
        string sortColumn = "Id";
        bool isLoading = false;
        IEnumerable<VBranch>? Branches;
        IEnumerable<RowNumber>? rowNumbers;
        protected async Task PerformSearch(ChangeEventArgs e)
        {
            searchText = e.Value?.ToString() ?? string.Empty;
            await GetBranches(searchText);
        }

        protected async Task PerformSearch(string? searchText = null)
        {
            searchText = searchText ?? string.Empty;
            await GetBranches(searchText);
        }

        protected async void Reload()
        {
            await GetBranches(searchText);
        }

        protected async Task OnIncludeInactive(bool e)
        {
            await GetBranches(searchText);
        }
        async Task OnCheckChanged(VBranch branch)
        {
            try
            {

                var resultResponse = await _branchService!.UpdateStatusAsync(branch.BranchId);

                if (resultResponse!.IsSuccess)
                {
                    NotificationService.Notify(NotificationSeverity.Success, "Update Branch");

                    await GetBranches(searchText);

                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, resultResponse.ErrMessage);
                }

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.InnerException!.Message);
            }

        }
        protected override async Task OnInitializedAsync()
        {
            await GetBranches(searchText);
            await base.OnInitializedAsync();

        }
        async Task GetBranches(string? filterText = "")
        {
            isLoading = true;

            var param = new QueryParam(pageIndex, pageSize, sortField, sortDescending, includeInactive, filterText);

            var resultResponse = await _branchService.GetBranchesAsync(param);

            var counts = resultResponse.Count;

            Branches = resultResponse.Items;

            isLoading = false;

            StateHasChanged();
        }
        async void LoadData(LoadDataArgs e)
        {
            searchText = e.Filter!.ToString() ?? string.Empty;
            pageIndex = e.Skip!.Value;
            if (!string.IsNullOrEmpty(e.OrderBy))
            {
                sortField = e.OrderBy.Split(" ")[0];
                string dir = e.OrderBy.Split(" ")[1];
                sortDescending = dir.Contains("asc") ? false : true;
            }
            await GetBranches(searchText);

        }
        protected async Task OnDetail(VBranch? branch)
        {
            var resultModal = await DialogService!.OpenAsync<BranchDetailModal>("Branch Details",
               new Dictionary<string, object> { { "Id", branch!.BranchId } },
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
        protected async Task OnEdit(VBranch? branch)
        {

            var resultModal = await DialogService!.OpenAsync<Update>("Edit Branch",
               new Dictionary<string, object> { { "Id", branch!.BranchId } },
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
        string BranchName = "Siem Reap";
        protected async void OnCreate()
        {
            var resultModal = await DialogService!.OpenAsync<Create>("Add Branch",
                new Dictionary<string, object> { },
                new DialogOptions
                {
                    Draggable = true,
                    Resizable = true,

                });
            if (resultModal != null)
            {

                Console.WriteLine("Created...");
            }
        }

        async Task OnDelete(VBranch? branch)
        {

            if ((bool)await DialogService!.Confirm("Branch deleted will not be able to recover, would you like to continue?", "Delete Branch",
                new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" }
                ))
            {
                try
                {
                    var responseResult = await EmployeeService!.GetEmployeesAsync(0, branch!.BranchId, true);

                    if (!responseResult.IsSuccess) return;

                    var employees = responseResult.Data;

                    if (employees!.Any())
                    {
                        if ((bool)await DialogService!.Confirm("Branch you are trying to delete has started its operation, so you cannot delete it. Make inactive is strongly recommmended!", "Delete Branch",
                            new ConfirmOptions() { OkButtonText = "Okay", CancelButtonText = "Cancel" }))
                        {

                            var message = await _branchService!.UpdateStatusAsync(branch!.BranchId);
                            if (message!.IsSuccess)
                            {
                                await OnInitializedAsync();
                            }

                            return;
                        }
                    }
                    else
                    {
                        var isDe = await _branchService!.DeleteAsync(branch!.BranchId);
                        if (isDe!.IsSuccess == true)
                        {
                            await OnInitializedAsync();
                            await InvokeAsync(StateHasChanged);

                        }

                        await OnInitializedAsync();
                        await InvokeAsync(StateHasChanged);
                    }


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                }

            }

        }
    }
}