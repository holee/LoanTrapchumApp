using Applications.Responses.Lists.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Linq.Expressions;

namespace TrapchumApp.Components.Pages.Lists
{
    public partial class OccupationDropDown
    {
        [Parameter] public string Id { get; set; } = $"{Guid.NewGuid().ToString("N")}";
        [Parameter] public string Name { get; set; } = $"{Guid.NewGuid().ToString("N")}";
        [Parameter] public IEnumerable<OcupationResponse> Occuptions { get; set; } = [];
        [Parameter] public string Value { get; set; } = string.Empty;
        [Parameter] public bool ShowHeader { get; set; } = false;
        [Parameter] public bool AllowClear { get; set; } = false;
        [Parameter] public string SearchText { get; set; } = string.Empty;
        [Parameter] public EventCallback<KeyboardEventArgs> AddOnEnter { get; set; }
        [Parameter] public EventCallback<string> SearchTextChanged { get; set; }
        [Parameter] public EventCallback<string> ValueChanged { get; set; }
        [Parameter] public Expression<Func<string>> ValueExpression { get; set; } = default!;
        IEnumerable<OcupationResponse> FilterOccuptions { get; set; } = [];


        private async Task OnValueChanged(object? newValue)
        {
            Value = newValue?.ToString() ?? string.Empty;

            await ValueChanged.InvokeAsync(Value);
        }
        private void OnSearchTextChanged(ChangeEventArgs e)
        {
            SearchText = e.Value?.ToString() ?? string.Empty;

            SearchTextChanged.InvokeAsync(SearchText);

            if (!string.IsNullOrEmpty(SearchText))
            {
                FilterOccuptions = Occuptions.Where(x => x.OccupationTitle!.Contains(SearchText, StringComparison.InvariantCultureIgnoreCase));
            }
            else
            {
                FilterOccuptions = Occuptions;
            }
        }

        private void OnKeyPress(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                AddOnEnter.InvokeAsync(e);
            }
        }
        protected override async Task OnInitializedAsync()
        {
            if (!Occuptions.Any())
            {
                Occuptions = await _employeeService!.GetOccupationsAsync("Position");
            }

            FilterOccuptions = Occuptions;

            await base.OnInitializedAsync();
        }

    }
}