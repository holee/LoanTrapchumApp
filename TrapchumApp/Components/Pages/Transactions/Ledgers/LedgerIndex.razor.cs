using Applications.Interfaces.Transactions;
using Applications.Responses.ViewModels.Ledgers;
using Applications.Services.Transactions.Journals;
using Radzen;
namespace TrapchumApp.Components.Pages.Transactions.Ledgers;

public partial class LedgerIndex
{
    private readonly ILedgerService _ledgerService;

    public LedgerIndex(ILedgerService ledgerService)
    {
        _ledgerService = ledgerService;
    }

    protected string searchField { get; set; } = string.Empty;
    private int currentPage = 1;
    private int pageSize = 10;
    int pageIndex = 0;
    string sortColumn = "LedgerId";
    bool sortDescending = false;
    int counts = 0;
    protected bool isLoading = false;
    protected int ledgerId;
    protected List<VLedger>? Ledgers { get; set; } = [];
    protected int currencyId = 0;
    bool includeInactive = false;
    async Task LoadData(LoadDataArgs e)
    {
        pageIndex = e.Skip ?? 0;
        searchField = e.Filter;
        if (!string.IsNullOrEmpty(e.OrderBy))
        {
            sortColumn = e.OrderBy.Split(" ")[0];
            string dir = e.OrderBy.Split(" ")[1];
            sortDescending = dir.Contains("asc") ? false : true;
        }

        await LoadLedgerPageList(searchField);
    }
    protected async override Task OnInitializedAsync()
    {
        try
        {
            await LoadLedgerPageList();

        }
        catch (Exception ex)
        {
            NotificationService.Notify(NotificationSeverity.Error, ex.Message);
        }
        await base.OnInitializedAsync();
    }
    async Task LoadLedgerPageList(string? filter = null)
    {
        try
        {
            isLoading = true;
            var res = await _ledgerService.GetLedgersAsync(pageIndex, pageSize, sortColumn, sortDescending, currencyId, includeInactive, filter);
            Ledgers = res.Items;
            counts = res.Count;
            isLoading = false;
            StateHasChanged();
        }
        catch (Exception ex)
        {
            NotificationService.Notify(NotificationSeverity.Error, ex.Message);
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
        var parameters = new Dictionary<string, object>
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
        var result = await _ledgerService!.ChangeLedgerStatus(new(ledgerId, arg!.Value));

        if (result.IsSuccess)
        {
            NotificationService.Notify(NotificationSeverity.Success, result.Data);
            if (!includeInactive)
            {
                await LoadLedgerPageList();
            }
        }
        else
        {
            NotificationService.Notify(NotificationSeverity.Error, result.ErrMessage);
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
        var confirm = await DialogService.OpenAsync<CreateLedgerModal>("Create", options: options);

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
        var isDe = await _ledgerService!.DeleteLedger(ledgerId);
        if (isDe.IsSuccess)
        {
            NotificationService.Notify(NotificationSeverity.Success, isDe.Data);
            await LoadLedgerPageList(searchField);
            return;
        }
        var messageErr = isDe.ErrMessage;

        var used = messageErr.Contains("used");

        if (!used)
        {
            NotificationService.Notify(NotificationSeverity.Error, messageErr);
            return;
        }
        var confirm = await DialogService!.Confirm($"You cannot delete ledger #<b>{ledgerId}</b>.<br/>Do you want to deactivate ledger<b>#{ledgerId}</b>?", "Ledger", new ConfirmOptions
        {
            OkButtonText = "OK",
            CancelButtonText = "Cancel"
        });
        if (confirm == false) return;
        var resp = await _ledgerService.ChangeLedgerStatus(new(ledgerId, false));
        if (resp.IsSuccess)
        {
            await LoadLedgerPageList(searchField);
        }
        else
        {
            NotificationService.Notify(NotificationSeverity.Error, resp.ErrMessage);
        }

    }


}
