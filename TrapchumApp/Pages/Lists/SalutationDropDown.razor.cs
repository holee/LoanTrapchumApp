using System.Linq.Expressions;

namespace TrapchumApp.Pages.Lists
{
    public partial class SalutationDropDown
    {
        [Parameter] public string Id { get; set; } = $"{Guid.NewGuid().ToString("N")}";
        [Parameter] public string Name { get; set; } = $"{Guid.NewGuid().ToString("N")}";
        [Parameter] public IEnumerable<string> Items { get; set; } = [];
        [Parameter] public string Value { get; set; } = string.Empty;
        [Parameter] public bool ShowHeader { get; set; } = false;
        [Parameter] public bool AllowClear { get; set; } = false;
        [Parameter] public string SearchText { get; set; } = string.Empty;
        [Parameter] public string Placeholder { get; set; } = "Salutation";
        [Parameter] public EventCallback<KeyboardEventArgs> AddItem { get; set; }
        [Parameter] public EventCallback<string> SearchTextChanged { get; set; }
        [Parameter] public EventCallback<string> ValueChanged { get; set; }
        [Parameter] public Expression<Func<string>> ValueExpression { get; set; } = default!;
        IEnumerable<string> FilterSalutations { get; set; } = [];

        private async Task OnValueChanged(object? newValue)
        {
            Value = newValue?.ToString() ?? string.Empty;

            await ValueChanged.InvokeAsync(Value);
        }
        private void OnSearchTextChanged(ChangeEventArgs e)
        {
            SearchText = e.Value?.ToString() ?? string.Empty;

            SearchTextChanged.InvokeAsync(SearchText);

            if (ShowHeader && !string.IsNullOrEmpty(SearchText))
            {
                FilterSalutations = Items.Where(x => x.Contains(SearchText, StringComparison.InvariantCultureIgnoreCase));
            }
            else
            {
                FilterSalutations = Items;
            }
        }

        private void OnKeyPress(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                AddItem.InvokeAsync(e);
            }
        }
        protected override async Task OnInitializedAsync()
        {
            FilterSalutations = Items;

            await base.OnInitializedAsync();
        }

    }
}