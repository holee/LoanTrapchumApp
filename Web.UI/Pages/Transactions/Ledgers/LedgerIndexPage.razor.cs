using Applications.Features.Ledgers.Commands.ChangeLedgerStatus;
using Applications.Features.Ledgers.Commands.DeleteLedger;
using Applications.Features.Ledgers.Dto;
using Applications.Features.Ledgers.Queries.GetLedgers;
using Applications.Interfaces.Transactions;

namespace Web.UI.Pages.Transactions.Ledgers;

public partial class LedgerIndexPage
{
    [Parameter] public int? BranchId { get; set; } = 0;

    protected string searchField { get; set; } = string.Empty;

    private int currentPage = 1;

    private int pageSize = 15;

    int pageIndex = 0;

    string sortColumn = "LedgerId";

    bool sortDescending = false;

    int counts = 0;

    protected bool isLoading = false;

    protected int ledgerId;

    protected IEnumerable<VLedger>? Ledgers = [];

    protected int currencyId = 0;

    bool includeInactive = false;
    async Task LoadData(LoadDataArgs e)
    {
        pageIndex = e.Skip ?? 0;

        searchField = e.Filter ?? string.Empty;

        (sortColumn, sortDescending) = e.PageInfo();

        await LoadLedgerPageList(searchField);
    }
    protected async override Task OnInitializedAsync()
    {
        isLoading = true;

        await LoadLedgerPageList();

        isLoading = false;

        await base.OnInitializedAsync();
    }
    async Task LoadLedgerPageList(string? filter = null)
    {
        try
        {

            Tracker.Show();

            var param = new QueryParam(pageIndex, pageSize, sortColumn, includeInactive, sortDescending, filter);

            var res = await mediator.Send(new GetLedgersPaginatedQuery(currencyId, param));

            Ledgers = res.Items;

            counts = res.TotalCount;

            Tracker.Hide();

            StateHasChanged();
        }
        catch (Exception ex)
        {
            _toastService.ShowError($"{ex.Message}");
        }
    }

    void LedgerDetails(int? ledgerId)
    {

    }
    protected async void Edit(int id)
    {
        var options = new DialogOptions
        {
            Width = "48rem",
            Draggable = true,
            Resizable = false
        };
        var parameters = new Dictionary<string, object?>
        {
            {"Id",id }
        };
        var confirm = await DialogService.OpenAsync<EditLedgerModal>("Edit", parameters, options: options);

        if ((bool)confirm == true)
        {
            await LoadLedgerPageList(searchField);
        }
    }

    protected async Task OnFilters(string searchText)
    {
        await LoadLedgerPageList(searchText);

    }
    protected async Task SelectedCurrencyChanged()
    {
        await LoadLedgerPageList(searchField);
    }
    protected async Task OnCheckChanged(bool? arg, int ledgerId)
    {

        var state = arg ?? false;

        var command = new ChangeLedgerStatusCommand(new LedgerChangeStatus
        {
            LedgerId = ledgerId,

            Status = state
        });

        var result = await mediator.Send(command);    //_ledgerService!.ChangeLedgerStatus(new(ledgerId, arg!.Value));

        if (!result.IsError)
        {
            _toastService.ShowSuccess(result.Value);

            if (!includeInactive)
            {
                await LoadLedgerPageList();
            }
        }
        else
        {
            _toastService.ShowError(result.GetMessage());
        }
    }
    protected async Task OnIncludeInActive()
    {
        await LoadLedgerPageList(searchField);

        await InvokeAsync(StateHasChanged);
    }

    public async void Reload()
    {
        await OnInitializedAsync();

        StateHasChanged();
    }

    protected async Task DisplayModal()
    {
        var options = new DialogOptions
        {
            Width = "48rem",
            Draggable = true,
            Resizable = false
        };
        var confirm = await DialogService.OpenAsync<CreateLedgerModal>("Create Ledger", options: options);

        if ((bool)confirm == true)
        {
            await LoadLedgerPageList(searchField);
        }

    }

    async Task OnDelete(int ledgerId)
    {

        var cmf = await DialogService!.Confirm("Ledger deleted will not be able to recover, would you like to continue?", "Delete Ledger", new ConfirmOptions
        {
            OkButtonText = "OK",
            CancelButtonText = "Cancel"
        });

        if (cmf == false) return;

        var isDe = await mediator.Send(new DeleteLedgerCommand(ledgerId)); //_ledgerService!.DeleteLedger(ledgerId);

        if (!isDe.IsError)
        {

            _toastService.ShowSuccess(isDe.Value);

            await LoadLedgerPageList(searchField);

            return;
        }
        var messageErr = isDe.FirstError.Description;

        var used = messageErr.Contains("used");

        if (!used)
        {
            _toastService.ShowInfo(messageErr);

            return;
        }

        var confirm = await MsgBox.ConfirmAsync($"You cannot delete ledger #<b>{ledgerId}</b>.<br/>Do you want to deactivate ledger<b>#{ledgerId}</b>?");

        if (confirm == MsgBoxResult.No) return;

        var resp = await mediator.Send(new ChangeLedgerStatusCommand(new LedgerChangeStatus
        {
            LedgerId = ledgerId,

            Status = false
        }));

        if (!resp.IsError)
        {
            await LoadLedgerPageList(searchField);
        }
        else
        {
            _toastService.ShowError(resp.GetMessage());
        }

    }

}
