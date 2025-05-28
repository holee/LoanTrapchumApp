using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.OtherNames;
using Applications.Services.Lists;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen.Blazor;
using Radzen.Blazor.Rendering;

namespace TrapchumApp.Components.Pages.Transactions.Loans.LoanRequests
{
    public partial class GuarantorSearchList
    {
        private readonly IGuarantorServiceImpl _gService;
        public GuarantorSearchList(IGuarantorServiceImpl gService)
        {
            _gService = gService;
        }
        [Parameter] public EventCallback<VGuarantor> OnSelected { get; set; }
        [Parameter] public EventCallback<IEnumerable<VGuarantor>> SelectMany { get; set; }
        RadzenTextBox _guarantorSearchText = default!;
        Popup _guarantorPopup = default!;
        IEnumerable<VGuarantor> _guarantors = [];
        IEnumerable<VGuarantor> _selectGuarantors = [];
        IList<VGuarantor> SelectedGuarantors { get; set; } = [];
        RadzenDataGrid<VGuarantor> _gridGuarantors = default!;
        VGuarantor? _selectedGuarantor = new();
        string? customerId = string.Empty;
        string? FullName = "";
        string? filterText = "";
        int pageSize = 10;
        string sortColumn = "Name";
        int counts = 0;
        bool isLoading = false;
        int selectedIndex = 0;
        bool allowRowSelectOnRowClick = false;
        async Task GetGuarantors()
        {
            isLoading = true;

            var param = new QueryParam(1, 10, string.Empty, false, true, filterText);

            var guarantorResp = await _gService.GetGuarantorsAsync(null, param);

            counts = guarantorResp.Count;

            _guarantors = guarantorResp.Items;

            isLoading = false;

            StateHasChanged();
        }
        async Task OnRowGuarantorSelect(VGuarantor g)
        {
            if (g == null) return;

            FullName = "";

            FullName = g.FullName;

            await OnSelected.InvokeAsync(g);

            await _guarantorPopup.CloseAsync();

        }

        async Task OnGuarantorSearchText(ChangeEventArgs args)
        {
            selectedIndex = 0;

            filterText = args.Value!.ToString();

            await GetGuarantors();

            if (!string.IsNullOrEmpty(filterText))
            {
                await _guarantorPopup.ToggleAsync(_guarantorSearchText.Element);
            }
            await _gridGuarantors.Reload();
        }

        async Task OnKeyDown(KeyboardEventArgs args)
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

        async Task OpenPopup(MouseEventArgs arg)
        {
            SelectedGuarantors = [];
            await GetGuarantors();

            await _guarantorPopup.ToggleAsync(_guarantorSearchText.Element);
        }

        async Task GetSelectedGuarantor(VGuarantor? arg)
        {
            if (arg == null) return;

            await OnSelected.InvokeAsync(arg);

            await _guarantorPopup.CloseAsync();
        }

        async Task GetSelectedGuarantors(IEnumerable<VGuarantor> args)
        {

            if (!args.Any()) return;

            await SelectMany.InvokeAsync(args);

            await _guarantorPopup.CloseAsync();
        }

    }
}