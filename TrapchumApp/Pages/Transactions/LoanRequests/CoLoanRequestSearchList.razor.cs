using Applications.Features.Customers.Queries.GetCustomerAsPaging;
using Applications.Features.Customers.Response;
using Microsoft.JSInterop;
using Radzen.Blazor;
using Radzen.Blazor.Rendering;

namespace TrapchumApp.Pages.Transactions.LoanRequests;

public partial class CoLoanRequestSearchList
{
    [Parameter] public EventCallback<VCustomerList> OnSelected { get; set; }
    [Parameter] public EventCallback<IEnumerable<VCustomerList>> SelectMany { get; set; } 

    private RadzenTextBox _customerSearchText = default!;

    private Popup _coRequestPopup = default!;

    private RadzenDataGrid<VCustomerList> _gridCoLoanRequest = default!;

    private IEnumerable<VCustomerList> _customers = [];
    private IList<VCustomerList> _selectedCustomers { get; set; } = [];

    private string? FullName = string.Empty;

    private string? _filterText = string.Empty;

    private int selectedIndex = 0;

    private int counts = 0;

    private bool isLoading = false;

    private const int pageSize = 25;

    private bool _popupOpen = false;

    private string? customerId = null;

    // ── Data loading ─────────────────────────────────────────────────────
    private async Task LoadCustomerAsync()
    {
        var param = new QueryParam(0, pageSize, nameof(VCustomerList.AltName), false, false, _filterText);

        var command = new GetCustomerAsPagingQuery(null, null, null, param);

        var result = await mediator.Send(command);

        counts = result.Count;

        _customers = result.Items;
    }

    // ── Event handlers ───────────────────────────────────────────────────

    /// <summary>Opens popup and loads all guarantors when the textbox is clicked.</summary>
    private async Task OpenPopup(MouseEventArgs _)
    {
        _selectedCustomers = [];

        _filterText = string.Empty;

        await LoadCustomerAsync();

        await _coRequestPopup.ToggleAsync(_customerSearchText.Element); 
    }

    /// <summary>Fires on every keystroke in the textbox — debounces & searches.</summary>
    private async Task OnCustomerSearchText(ChangeEventArgs args)
    {

        selectedIndex = 0;

        _filterText = args.Value?.ToString() ?? string.Empty;

        await LoadCustomerAsync();

        await _gridCoLoanRequest.Reload();
    }

    private async Task OnKeyDown(KeyboardEventArgs args)
    {

        var items = _customers;

        var popupOpened = await js.InvokeAsync<bool>("Radzen.popupOpened", "coLoanRequestPopup");

        var key = args.Code != null ? args.Code : args.Key;

        if (!args.AltKey && (key == "ArrowDown" || key == "ArrowUp"))
        {

            var result = await js.InvokeAsync<int[]>("Radzen.focusTableRow", "gridCoLoanRequest", key, selectedIndex, null, false);

            selectedIndex = result.First();

        }
        else if (args.AltKey && key == "ArrowDown" || key == "Enter" || key == "NumpadEnter")
        {

            if (popupOpened && (key == "Enter" || key == "NumpadEnter"))
            {
                customerId = items.ElementAtOrDefault(selectedIndex)?.CustomerId;
            }

            await _coRequestPopup.ToggleAsync(_customerSearchText.Element);
        }
        else if (key == "Escape" || key == "Tab")
        {
            await _coRequestPopup.CloseAsync();
        }
    }


    private async Task GetSelectedGuarantor(VCustomerList? g) 
    {
        if (g is null) return;

        FullName = g.FullName;

        await OnSelected.InvokeAsync(g);

        await _coRequestPopup.CloseAsync();
    }

    /// <summary>Accepts all checked guarantors (Accept Selected button).</summary>
    private async Task GetSelectedGuarantors(IEnumerable<VCustomerList> selected)
    {
        if (!selected.Any()) return;

        FullName = string.Join(", ", selected.Select(x => x.FullName));

        await SelectMany.InvokeAsync(selected);

        await _coRequestPopup.CloseAsync();
    }


    private async Task ClosePopup()
    {
        _selectedCustomers = [];

        await _coRequestPopup.CloseAsync();

    }
}
