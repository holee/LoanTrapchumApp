using Applications.Bases;
using Applications.Features.Branches.Command.ChangeBranchState;
using Applications.Features.Branches.Command.DeleteBranch;
using Applications.Features.Branches.Dtos;
using Applications.Features.Branches.Queries.GetBranchAsPaging;

namespace TrapchumApp.Pages.Lists.Branches;

public partial class BranchList
{
    protected string searchText = string.Empty;

    int pageIndex = 0;

    int counts = 0;

    string sortField = "BranchId";

    bool includeInactive = false;

    protected bool InActive = false;

    int pageSize = 15;

    bool sortDescending = false;

    string sortColumn = "Id";

    bool isLoading = false;

    IEnumerable<VBranch>? Branches = Array.Empty<VBranch>();

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

        var confirm = await DialogService.Confirm($"តើអ្នកចង់កាត់បន្ថយសាខាដែលមាន ID#{branchId} ឬទេ?", options: new ConfirmOptions
        {
            CancelButtonText = "បដិសេធ",

            OkButtonText = "យល់ព្រម",
        });

        if (confirm == false) return;

        var command = new ChangeBranchStateCommand(branchId, state);

        var rsp = await mediator.Send(command);

        if (rsp.IsSuccess())
        {

            _toastService.ShowSuccess("ស្ថានភាពសាខាត្រូវបានធ្វើបច្ចុប្បន្នភាពដោយជោគជ័យ។");

            await GetBranches(searchText);

        }
        else
        {
            _toastService.ShowError(rsp.GetMessage());
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

    async Task OnDelete(VBranch? branch)
    {

        var confirm = await MsgBox.ConfirmAsync("សាខាដែលបានលុបនឹងមិនអាចស្ដារឡើងវិញបានទេ តើអ្នកចង់បន្តទេ?");

        if (confirm == MsgBoxResult.No) return;

        var command = new DeleteBranchCommand(branch!.BranchId);

        var isDe = await mediator.Send(command);

        if (isDe.Value.IsSuccess)
        {
            _toastService.ShowSuccess("សាខាត្រូវបានលុបដោយជោគជ័យ។");

            await GetBranches(searchText);

            return;
        }

        if (isDe.Value.Status == DeleteStatus.InUse)
        {
            _toastService.ShowWarning(isDe.Value.Message ?? string.Empty);
        }

        var confirm1 = await MsgBox.ConfirmAsync("សាខាដែលអ្នកកំពុងព្យាយាមលុបបានចាប់ផ្តើមប្រតិបត្តិការរួចហើយ ដូច្នេះអ្នកមិនអាចលុបវាបានទេ។ ការធ្វើឱ្យអសកម្មត្រូវបានណែនាំយ៉ាងខ្លាំង!");


        if (confirm1 == MsgBoxResult.No) return;

        var commandStatus = new ChangeBranchStateCommand(branch!.BranchId, false);

        var rsp = await mediator.Send(commandStatus);

        if (rsp!.IsSuccess())
        {

            _toastService.ShowWarning("សាខាដែលអ្នកកំពុងព្យាយាមលុបបានចាប់ផ្តើមប្រតិបត្តិការរួចហើយ ដូច្នេះអ្នកមិនអាចលុបវាបានទេ។ ការធ្វើឱ្យអសកម្មត្រូវបានណែនាំយ៉ាងខ្លាំង!");

            await GetBranches(searchText);
        }

        _toastService.ShowError(rsp.GetMessage());

    }
  
    private async Task PageSizeChanged(int args)
    {
        pageSize = args;

        pageIndex = 0; // Reset to first page when page size changes

        await GetBranches(searchText);
    }
}