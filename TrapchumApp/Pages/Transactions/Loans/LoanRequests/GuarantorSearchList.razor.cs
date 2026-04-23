using Applications.Features.Guarantor.Dtos;
using Applications.Features.Guarantor.Queries.GetGuarantorsAsPaging;
using Applications.Responses.Paginations;
using Applications.Services.Lists;
using Microsoft.JSInterop;
using Radzen.Blazor;
using Radzen.Blazor.Rendering;

namespace TrapchumApp.Pages.Transactions.Loans.LoanRequests
{
    public partial class GuarantorSearchList
    {
        [Parameter] public EventCallback<VGuarantor> OnSelected { get; set; }
        [Parameter] public EventCallback<IEnumerable<VGuarantor>> SelectMany { get; set; }

        private RadzenTextBox _guarantorSearchText = default!;

        private Popup _guarantorPopup = default!;

        private RadzenDataGrid<VGuarantor> _gridGuarantors = default!;

        private IEnumerable<VGuarantor> _guarantors = [];
        private IList<VGuarantor> SelectedGuarantors { get; set; } = [];

        private string? FullName = string.Empty;

        private string? _filterText = string.Empty;

        private int counts = 0;

        private bool isLoading = false;

        private const int pageSize = 10;

        private bool _popupOpen = false;

        // ── Data loading ─────────────────────────────────────────────────────
        private async Task LoadGuarantorsAsync()
        {

            Tracker.Show();

            var param = new QueryParam(0, 0, nameof(VGuarantor.FullName), false, false, _filterText);

            var command = new GetGuarantorsAsPagingQuery(null, param);

            var result = await mediator.Send(command);

            counts = result.Count;

            _guarantors = result.Items;

            Tracker.Hide();

        }

        // ── Event handlers ───────────────────────────────────────────────────

        /// <summary>Opens popup and loads all guarantors when the textbox is clicked.</summary>
        private async Task OpenPopup(MouseEventArgs _)
        {
            SelectedGuarantors = [];

            _filterText = string.Empty;

            await LoadGuarantorsAsync();

            await EnsurePopupOpenAsync();
        }

        /// <summary>Fires on every keystroke in the textbox — debounces & searches.</summary>
        private async Task OnGuarantorSearchText(ChangeEventArgs args)
        {

            _filterText = args.Value?.ToString() ?? string.Empty;

            Console.WriteLine(_filterText);

            //await LoadGuarantorsAsync();

            // Ensure popup is open (it may have been closed by Escape or a previous click)
            await EnsurePopupOpenAsync();
        }

        /// <summary>Keyboard navigation inside the textbox.</summary>
        private async Task OnKeyDown(KeyboardEventArgs args)
        {

            if (!_guarantors.Any()) return;

            var items = _guarantors;

            var popupOpened = await js.InvokeAsync<bool>("Radzen.popupOpened", "_guarantorPopup");

            var key = args.Code != null ? args.Code : args.Key;

            if (!args.AltKey && (key == "ArrowDown" || key == "ArrowUp"))
            {

                var result = await js.InvokeAsync<int[]>("Radzen.focusTableRow", "gridGuarantors", key, selectedIndex, null, false);

                selectedIndex = result.First();

            }
            else if (args.AltKey && key == "ArrowDown" || key == "Enter" || key == "NumpadEnter")
            {

                if (popupOpened && (key == "Enter" || key == "NumpadEnter"))
                {

                    customerId = items.ElementAtOrDefault(selectedIndex)?.Id;

                }

                await _guarantorPopup.ToggleAsync(_guarantorSearchText.Element);

            }
            else if (key == "Escape" || key == "Tab")
            {
                await _guarantorPopup.CloseAsync();
            }

            //var key = args.Code ?? args.Key;

            //switch (key)
            //{
            //    case "Escape":
            //    case "Tab":
            //        await ClosePopup();
            //        break;

            //    case "Enter":
            //    case "NumpadEnter":
            //        // Accept first item if nothing is explicitly selected
            //        if (!SelectedGuarantors.Any() && _guarantors.Any())
            //            await GetSelectedGuarantor(_guarantors.First());
            //        else if (SelectedGuarantors.Any())
            //            await GetSelectedGuarantors(SelectedGuarantors);
            //        break;
            //}
        }

        /// <summary>Accepts a single guarantor (add-button or row double-click).</summary>
        private async Task GetSelectedGuarantor(VGuarantor? g)
        {
            if (g is null) return;

            FullName = g.FullName;

            await OnSelected.InvokeAsync(g);

            await ClosePopup();
        }

        /// <summary>Accepts all checked guarantors (Accept Selected button).</summary>
        private async Task GetSelectedGuarantors(IEnumerable<VGuarantor> selected)
        {
            if (!selected.Any()) return;

            FullName = string.Join(", ", selected.Select(x => x.FullName));

            await SelectMany.InvokeAsync(selected);

            await ClosePopup();
        }

        private async Task EnsurePopupOpenAsync()
        {
            if (!_popupOpen)
            {
                _popupOpen = true;

                await _guarantorPopup.ToggleAsync(_guarantorSearchText.Element);
            }
        }

        private async Task ClosePopup()
        {
            SelectedGuarantors = [];

            if (_popupOpen)
            {
                _popupOpen = false;

                await js.InvokeVoidAsync("eval", "document.activeElement instanceof HTMLElement && document.activeElement.blur()");

                await _guarantorPopup.CloseAsync();
            }
        }
    }
}
