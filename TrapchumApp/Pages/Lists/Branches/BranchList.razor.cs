using Applications.Bases;
using Applications.Enumerations;
using Applications.Features.Branches.Command.ChangeBranchState;
using Applications.Features.Branches.Command.DeleteBranch;
using Applications.Features.Branches.Dtos;
using Applications.Features.Branches.Queries.GetBranchAsPaging;
using Applications.Responses.Paginations;
using TrapchumApp.Helpers.Extensions;

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

        int pageSize = 15;

        bool sortDescending = false;

        string sortColumn = "Id";

        bool isLoading = false;

        IEnumerable<VBranch>? Branches;

        IEnumerable<RowNumber>? rowNumbers;

        protected async Task SearchChanged(string arg)
        {
            searchText = arg ?? string.Empty;

            await GetBranches(arg);
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
            includeInactive = e;

            await GetBranches(searchText);
        }
        async Task OnCheckChanged(int? branchId, bool state)
        {

            var confirm = await DialogService.Confirm($"Do you want to deactivate Branch with ID#{branchId}?", options: new ConfirmOptions
            {
                CancelButtonText = "Cancel",

                OkButtonText = "OK",
            });

            if (confirm == false) return;

            var command = new ChangeBranchStateCommand(branchId, state);

            var rsp = await mediator.Send(command);// _branchService!.UpdateStatusAsync(command);

            if (rsp!.IsSuccess())
            {
                NotificationService.Notify(NotificationSeverity.Success, "Branch state updated successfully.");

                await GetBranches(searchText);

            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, rsp.GetMessage());
            }
        }
        protected override async Task OnInitializedAsync()
        {
            await GetBranches(searchText);

            await base.OnInitializedAsync();

        }
        async Task GetBranches(string? filterText = "")
        {
            Tracker.Show();

            var param = new QueryParam(pageIndex, pageSize, sortField, includeInactive, sortDescending, filterText);

            var command = new GetBranchAsPagingQuery(0, param);

            var rsp = await mediator.Send(command);

            counts = rsp.Count;

            Branches = rsp.Items;

            Tracker.Hide();

            StateHasChanged();
        }
        async void LoadData(LoadDataArgs e)
        {

            (pageIndex, pageSize, sortField, sortDescending, searchText) = e.Page(nameof(VBranch.BranchId), nameof(VBranch.BranchName), nameof(VBranch.BranchId));

            await GetBranches(searchText);
        }
        protected async Task OnDetail(VBranch? branch)
        {
            await DialogService!.OpenAsync<BranchDetailModal>("Branch Details", new Dictionary<string, object?> { { "Id", branch!.BranchId } });
        }
        protected async Task OnEdit(VBranch? branch)
        {

            var resultModal = await DialogService!.OpenAsync<BranchUpdateModal>("Edit Branch", new Dictionary<string, object?> { { "Id", branch!.BranchId } });
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

        async Task OnDelete(VBranch? branch)
        {

            var options = new ConfirmOptions()
            {
                OkButtonText = "Yes",
                CancelButtonText = "No"
            };


            bool? confirm = await DialogService.Confirm("Branch deleted will not be able to recover, would you like to continue?", options: options);

            if (confirm == false) return;

            var command = new DeleteBranchCommand(branch!.BranchId);

            var isDe = await mediator.Send(command);

            if (isDe.Value.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Success, "Branch deleted successfully.");
                await GetBranches(searchText);
                return;
            }

            if (isDe.Value.Status == DeleteStatus.InUse)
            {
                NotificationService.Notify(NotificationSeverity.Warning, isDe.Value.Message ?? string.Empty);
            }
             
            var options1 = new ConfirmOptions()
            {
                OkButtonText = "Yes",
                CancelButtonText = "No"
            };

            var confirm1 = await DialogService.Confirm("Branch you are trying to delete has started its operation, so you cannot delete it. Make inactive is strongly recommmended!", options: options1);

            if (confirm1 == false) return;

            var commandStatus = new ChangeBranchStateCommand(branch!.BranchId, false);

            var rsp = await mediator.Send(commandStatus);

            if (rsp!.IsSuccess())
            {
                NotificationService.Notify(NotificationSeverity.Success,"Branch state changed successfully.");

                await GetBranches(searchText);
            }

            NotificationService.Notify(NotificationSeverity.Error, rsp.GetMessage());

        }
        private async Task PageSizeChanged(int args)
        {
            pageSize = args;

            pageIndex = 0; // Reset to first page when page size changes

            await GetBranches(searchText);
        }
    }
}