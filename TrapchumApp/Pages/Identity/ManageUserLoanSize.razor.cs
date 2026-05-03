using Applications.Features.Settings.Commands.UserLoanSizes.DeleteUserLoanSize;
using Applications.Features.Settings.Commands.UserLoanSizes.ToggleUserLoanSize;
using Applications.Features.Settings.Dto;
using Applications.Features.Settings.Queries.UserLoanSizes.GetUserLoanSizes;

namespace TrapchumApp.Pages.Identity;

public partial class ManageUserLoanSize
{
    private List<VUserLoanSizes> _loanSizes = [];
    private int _counts = 0;
    private int pageSize = 15;
    private int _pageIndex = 0;
    private string _sortField = nameof(VUserLoanSizes.Id);
    private bool _sortDescending = false;
    private string? _filter;
    private bool isIncludeInActive = false;
    private bool isLoading = false;

    protected override async Task OnInitializedAsync()
    {
        await GetUserLoanSizes();

        await base.OnInitializedAsync();
    }

    private async Task GetUserLoanSizes()
    {
        isLoading = true;

        var param = new QueryParam(_pageIndex, pageSize, _sortField, isIncludeInActive, _sortDescending, _filter);

        var rsp = await mediator.Send(new GetUserLoanSizesQuery(null, param));

        _loanSizes = rsp.Items ?? [];

        _counts = rsp.Count;

        isLoading = false;

        StateHasChanged();
    }

    private async Task LoadData(LoadDataArgs args)
    {
        _pageIndex = args.Skip ?? 0;

        if (args.Sorts?.Any() == true)
        {
            _sortField = args.Sorts.First().Property ?? nameof(UserLoanSizeDto.Id);

            _sortDescending = args.Sorts.First().SortOrder == SortOrder.Descending;
        }
        await GetUserLoanSizes();
    }

    private async Task SearchTextChanged(string? filter)
    {
        _filter = filter;

        await GetUserLoanSizes();
    }

    private async Task CheckedChanged(bool value)
    {
        isIncludeInActive = value;
        await GetUserLoanSizes();
    }

    private async Task PageSizeChanged(int size)
    {
        pageSize = size;

        await GetUserLoanSizes();
    }

    private async Task EditLoanSize(VUserLoanSizes item)
    {
        var result = await DialogService.OpenAsync<SetUserLoanSizeEdit>("Edit Loan Size",
            new Dictionary<string, object?> { { nameof(SetUserLoanSizeEdit.Item), item } },
            options: new DialogOptions { AutoFocusFirstElement = false });

        if (result is bool b && b)
            await GetUserLoanSizes();
    }

    private async Task ToggleActive(VUserLoanSizes item)
    {
        if (item.Id is null || item.UserRefId is null)
        {

            _toastService.ShowError("Invalid data", "Error");

            return;
        }

        var rsp = await mediator.Send(new ToggleUserLoanSizeCommand(item.Id.Value, item.UserRefId!));

        if (rsp.IsError)
            await DialogService.Alert(rsp.FirstError.Description, "Error");
        else
        {
            NotificationService.Notify(NotificationSeverity.Success, "Updated", "Loan size status changed.");
            await GetUserLoanSizes();
        }
    }

    private async Task DeleteLoanSize(VUserLoanSizes item)
    {
        if (item.Id is null) return;

        var confirm = await DialogService.Confirm(
            $"Delete loan size for '{item.FullName}'?",
            "Confirm Delete",
            new ConfirmOptions { OkButtonText = "Delete", CancelButtonText = "Cancel" });

        if (confirm != true) return;

        var rsp = await mediator.Send(new DeleteUserLoanSizeCommand(item.Id.Value));

        if (rsp.IsError)
            await DialogService.Alert(rsp.FirstError.Description, "Error");
        else
        {
            NotificationService.Notify(NotificationSeverity.Success, "Deleted", $"Loan size for '{item.FullName}' removed.");
            await GetUserLoanSizes();
        }
    }
}
