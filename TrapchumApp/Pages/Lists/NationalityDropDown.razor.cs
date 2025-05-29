using System.Linq.Expressions;
using Domains.Models.Lists;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
namespace TrapchumApp.Pages.Lists
{
    public partial class NationalityDropDown
    {
        [Parameter] public IEnumerable<Nationality> Nationalities { get; set; } = [];
        [Parameter] public string Value { get; set; } = string.Empty;
        [Parameter] public string Name { get; set; } = "Nationality"; 
        [Parameter] public EventCallback<string> ValueChanged { get; set; }
        [Parameter] public EventCallback<object> OnChanged { get; set; }
        [Parameter] public string Placeholder { get; set; } = "Nationality";
        [Parameter] public string SearchText { get; set; } = string.Empty;
        [Parameter] public EventCallback<KeyboardEventArgs> AddOnEnter { get; set; }
        [Parameter] public Expression<Func<string>> ValueExpression { get; set; } = default!;
        [Parameter] public EventCallback<string> SearchTextChanged { get; set; }
        [Parameter] public bool ShowHeader { get; set; } = false;
        [Parameter] public bool AllowClear { get; set; } = false;
        IEnumerable<Nationality> FilterNationality { get; set; } = [];
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
                FilterNationality = Nationalities.Where(x => x.NationalityName!.Contains(SearchText, StringComparison.InvariantCultureIgnoreCase));
            }
            else
            {
                FilterNationality = Nationalities;
            }
        }

        private void OnKeyPress(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                AddOnEnter.InvokeAsync(e);
            }
        }
    }
}