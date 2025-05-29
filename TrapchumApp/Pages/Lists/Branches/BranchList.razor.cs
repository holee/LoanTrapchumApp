using Applications.Enumerations;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Branches;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace TrapchumApp.Pages.Lists.Branches
{
    public partial class BranchList
    {
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
        IEnumerable<VBranchResponse>? Branches;
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
        async Task OnCheckChanged(VBranchResponse branchResponse)
        {
            try
            {

                var resultResponse = await _branchService!.UpdateStatusAsync(branchResponse.BranchId);

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
        protected async Task OnDetail(VBranchResponse? branch)
        {
            await DialogService!.OpenAsync<BranchDetailModal>("Branch Details",new Dictionary<string, object> { { "Id", branch!.BranchId } });
        }
        protected async Task OnEdit(VBranchResponse? branch)
        {

            var resultModal = await DialogService!.OpenAsync<BranchUpdateModal>("Edit Branch",new Dictionary<string, object> { { "Id", branch!.BranchId } });
            if (resultModal != null)
            {
                await GetBranches(searchText);
            }

        }
        string BranchName = "Siem Reap";
        protected async void OnCreate()
        {
            bool? resultModal = await DialogService.OpenAsync<BranchCreateModal>("Add Branch");

            if (resultModal != null && resultModal == true)
            {
                await GetBranches(searchText);
            }
        }

        async Task OnDelete(VBranchResponse? branch)
        {

            var options = new ConfirmOptions()
            {
                OkButtonText = "Yes",
                CancelButtonText = "No"
            };


            bool? confirm = await DialogService.Confirm("Branch deleted will not be able to recover, would you like to continue?",options:options);

            if (confirm == false) return;


            var isDe = await _branchService!.DeleteAsync(branch!.BranchId);

            if (isDe.IsSuccess)
            {
                await GetBranches(searchText);
            }

            var message=isDe.ErrMessage;

            if (!message.Contains("Used",StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            var options1 = new ConfirmOptions()
            {
                OkButtonText = "Yes",
                CancelButtonText = "No"
            };

            var confirm1 = await DialogService.Confirm("Branch you are trying to delete has started its operation, so you cannot delete it. Make inactive is strongly recommmended!", options: options1);

            if(confirm1==false) return;


            var rsp = await _branchService!.UpdateStatusAsync(branch!.BranchId);

            if (rsp!.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Success,"Changed");

                await GetBranches(searchText);
            }

            NotificationService.Notify(NotificationSeverity.Error, rsp.ErrMessage);

        }
    }
}