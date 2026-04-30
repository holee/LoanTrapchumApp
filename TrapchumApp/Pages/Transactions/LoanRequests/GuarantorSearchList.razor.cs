using Applications.Features.Guarantor.Dtos;
using Applications.Features.Guarantor.Queries.GetGuarantorsAsPaging;
using Microsoft.JSInterop;
using Radzen.Blazor;
using Radzen.Blazor.Rendering;

namespace TrapchumApp.Pages.Transactions.LoanRequests
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

        private int selectedIndex = 0;

        private int counts = 0;

        private bool isLoading = false;

        private const int pageSize = 10;

        private bool _popupOpen = false;

        private string? customerId = null;

        // ── Data loading ─────────────────────────────────────────────────────
        private async Task LoadGuarantorsAsync()
        {
            var param = new QueryParam(0, 0, nameof(VGuarantor.FullName), false, false, _filterText);

            var command = new GetGuarantorsAsPagingQuery(null, param);

            var result = await mediator.Send(command);

            counts = result.Count;

            _guarantors = result.Items;
        }

        // ── Event handlers ───────────────────────────────────────────────────

        /// <summary>Opens popup and loads all guarantors when the textbox is clicked.</summary>
        private async Task OpenPopup(MouseEventArgs _)
        {
            SelectedGuarantors = [];

            _filterText = string.Empty;

            await LoadGuarantorsAsync();

            await _guarantorPopup.ToggleAsync(_guarantorSearchText.Element);
        }

        /// <summary>Fires on every keystroke in the textbox — debounces & searches.</summary>
        private async Task OnGuarantorSearchText(ChangeEventArgs args)
        {

            selectedIndex = 0;

            _filterText = args.Value?.ToString() ?? string.Empty;

            await LoadGuarantorsAsync();

            await _gridGuarantors.Reload();
        }

        private async Task OnKeyDown(KeyboardEventArgs args)
        {

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
        }


        private async Task GetSelectedGuarantor(VGuarantor? g)
        {
            if (g is null) return;

            FullName = g.FullName;

            await OnSelected.InvokeAsync(g);

            await _guarantorPopup.CloseAsync();
        }

        /// <summary>Accepts all checked guarantors (Accept Selected button).</summary>
        private async Task GetSelectedGuarantors(IEnumerable<VGuarantor> selected)
        {
            if (!selected.Any()) return;

            FullName = string.Join(", ", selected.Select(x => x.FullName));

            await SelectMany.InvokeAsync(selected);

            await _guarantorPopup.CloseAsync();
        }


        private async Task ClosePopup()
        {
            SelectedGuarantors = [];

            await _guarantorPopup.CloseAsync();

        }
    }
}
