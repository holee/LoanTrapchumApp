using Applications.Responses.Lists.Collaterals;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.Web;
using Radzen.Blazor;
using Radzen.Blazor.Rendering;

namespace TrapchumApp.Pages.Transactions.Loans.LoanRequests
{
    public partial class CollateralSearchList
    {
        [Parameter] public EventCallback<VCollateralLines> SingleSelected { get; set; }
        [Parameter] public EventCallback<IEnumerable<VCollateralLines>> MultipleSelected { get; set; }
        [Parameter] public IEnumerable<VCollateralLines> SelectedItems { get; set; } = [];
        [Parameter] public string? CustomerId { get; set; } = string.Empty;

        private IEnumerable<VCollateralLines> _collateralLines = [];

        private IList<VCollateralLines> _selectedCollateralLines = [];

        private RadzenTextBox _collateralSearchText = default!;

        private Popup _collateralPopup = default!;

        private bool allowRowSelectOnRowClick = false;

        private RadzenDataGrid<VCollateralLines> _gridCollaterals = default!;

        private VCollateralLines? _selectedCollateral = new();

        private string? filterText = "";

        private int pageSize = 10;

        private string sortColumn = "Name";

        private int counts = 0;

        private bool isLoading = false;

        private int selectedIndex = 0;

        private long collateralLineId = 0;


        private IEnumerable<VCollateralLines> FilteredCollateralLines =>
        string.IsNullOrEmpty(filterText)
            ? _collateralLines
            : _collateralLines.Where(c =>
                (c.CollateralNo?.Contains(filterText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (c.OwnerName?.Contains(filterText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (c.TypeName?.Contains(filterText, StringComparison.OrdinalIgnoreCase) ?? false));
        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        async Task OnRowCollateralSelect(VCollateralLines g)
        {
            if (_collateralLines.Any(x => x.Id == g.Id))
            {
                await SingleSelected.InvokeAsync(g);

                await _collateralPopup.CloseAsync();
            }
            else
            {
                await _collateralPopup.CloseAsync();
            }


        }

        async Task OnCollateralSearchText(ChangeEventArgs args)
        {
            selectedIndex = 0;

            filterText = args.Value!.ToString();

            if (!string.IsNullOrEmpty(args.Value!.ToString()))
            {
                await GetCustomerCollaterals();
                await _collateralPopup.ToggleAsync(_collateralSearchText.Element);
            }
            else
            {
                await _collateralPopup.CloseAsync();
            }
            await _gridCollaterals.Reload();
        }

        async Task OnKeyDown(KeyboardEventArgs args)
        {

            if (!_collateralLines.Any()) return;

            var isShiftKey = args.ShiftKey;
            
            var arrowDownKey = args.Code == "ArrowDown" || args.Key == "ArrowDown";

            var arrowUpKey = args.Code == "ArrowUp" || args.Key == "ArrowUp";


            var items = _collateralLines;

            var popupOpened = await js.InvokeAsync<bool>("Radzen.popupOpened", "_collateralPopup");

            var key = args.Code != null ? args.Code : args.Key;

            if (!args.AltKey && (key == "ArrowDown" || key == "ArrowUp"))
            {

                var result = await js.InvokeAsync<int[]>("Radzen.focusTableRow", "_gridCollaterals", key, selectedIndex, null, false);

                selectedIndex = result.First();

            }
            else if (args.AltKey && key == "ArrowDown" || key == "Enter" || key == "NumpadEnter")
            {

                if (popupOpened && (key == "Enter" || key == "NumpadEnter"))
                {

                    collateralLineId = items.ElementAtOrDefault(selectedIndex)!.Id;

                }

                await _collateralPopup.ToggleAsync(_collateralSearchText.Element);

            }
            else if (key == "Escape" || key == "Tab")
            {
                await _collateralPopup.CloseAsync();
            }
        }

        async Task OpenPopup(MouseEventArgs arg)
        {

            await GetCustomerCollaterals();

            _selectedCollateralLines = SelectedItems.Take(1).ToList();

            await _collateralPopup.ToggleAsync(_collateralSearchText.Element);
        }


        async Task GetSelectedGuarantors(IEnumerable<VCollateralLines> args)
        {
            if (!SelectedMultiple)
            {
                await _collateralPopup.CloseAsync();

                return;
            }
            if (!args.Any())
            {
                await _collateralPopup.CloseAsync();
            }
            else
            {
                await MultipleSelected.InvokeAsync(args);

                await _collateralPopup.CloseAsync();
            }


        }
        async Task GetSelectedCollater(VCollateralLines g)
        {

            if (_collateralLines.Any(x => x.Id == g.Id))
            {
                await SingleSelected.InvokeAsync(g);
            }
            await _collateralPopup.CloseAsync();
        }

        async Task GetCustomerCollaterals()
        {
            var resp = await _customerService.GetCustomerCollateralLines(CustomerId);

            if (resp.IsSuccess)
            {
                _collateralLines = resp.Data!;
            }
            else
            {
                _collateralLines = [];
            }
                StateHasChanged();
        }

        private async Task ToggleRowSelection(VCollateralLines item)
        {
            if (_selectedCollateralLines.Contains(item))
            {
                _selectedCollateralLines.Remove(item);
            }
            else
            {
                _selectedCollateralLines.Add(item);
            }
            await _gridCollaterals.Reload();
        }
        private void CheckedChanged(bool args)
        {
            SelectedMultiple = args;
        }
    }
}