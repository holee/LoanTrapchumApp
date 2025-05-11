using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen.Blazor.Rendering;
using Radzen.Blazor;
using Applications.Responses.Lists.Collaterals;
using Applications.Responses.ViewModels.OtherNames;

namespace TrapchumApp.Components.Pages.Transactions.Loans.LoanRequests
{
    public partial class CollateralSearchList
    {
        [Parameter] public EventCallback<VCollateralLines> OnSelected { get; set; }
        [Parameter] public EventCallback<IEnumerable<VCollateralLines>> SelectMany { get; set; }
        [Parameter] public string? CustomerId { get; set; } = string.Empty;

        IEnumerable<VCollateralLines> _collateralLines = [];

        IList<VCollateralLines> _selectedCollateralLines = [];

        RadzenTextBox _collateralSearchText = default!;

        Popup _collateralPopup = default!;


        RadzenDataGrid<VCollateralLines> _gridCollaterals = default!;

        VCollateralLines? _selectedCollateral = new();

        string? filterText = "";
        int pageSize = 10;
        string sortColumn = "Name";
        int counts = 0;
        bool isLoading = false;
        int selectedIndex = 0;
        long collateralLineId = 0;
        bool allowRowSelectOnRowClick = false;
        async Task OnRowCollateralSelect(VCollateralLines g)
        {
            if (_collateralLines.Any(x => x.Id == g.Id))
            {
                await OnSelected.InvokeAsync(g);
            }
            await _collateralPopup.CloseAsync();

        }

        async Task OnCollateralSearchText(ChangeEventArgs args)
        {
            selectedIndex = 0;

            filterText = args.Value!.ToString();

            if (!string.IsNullOrEmpty(args.Value!.ToString()))
            {
                await _collateralPopup.ToggleAsync(_collateralSearchText.Element);
            }
            await _gridCollaterals.Reload();
        }

        async Task OnKeyDown(KeyboardEventArgs args)
        {

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

            await _collateralPopup.ToggleAsync(_collateralSearchText.Element);
        }


        async Task GetSelectedGuarantors(IEnumerable<VCollateralLines> args)
        {

            if (!args.Any()) return;

            await SelectMany.InvokeAsync(args);

            await _collateralPopup.CloseAsync();
        }
        async Task GetSelectedCollater(VCollateralLines g)
        {

            if (_collateralLines.Any(x => x.Id == g.Id))
            {
                // FullName = _selectedGuarantor.FullName;

                await OnSelected.InvokeAsync(g);
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
            StateHasChanged();
        }
    }
}