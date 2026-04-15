using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace TrapchumApp.Components.Select2;

public partial class BlabSelect2<TItem, TValue> : ComponentBase, IAsyncDisposable
{
    [Inject] private IJSRuntime JS { get; set; } = default!;

    // ── Data ────────────────────────────────────────────────────────
    [Parameter] public IEnumerable<TItem> Data { get; set; } = [];
    [Parameter] public Func<TItem, TValue> ValueSelector { get; set; } = default!;
    [Parameter] public Func<TItem, string> TextSelector { get; set; } = default!;

    // ── Single-select binding ───────────────────────────────────────
    [Parameter] public TValue? Value { get; set; }
    [Parameter] public EventCallback<TValue?> ValueChanged { get; set; }

    // ── Multi-select binding ────────────────────────────────────────
    [Parameter] public IEnumerable<TValue>? Values { get; set; }
    [Parameter] public EventCallback<IEnumerable<TValue>> ValuesChanged { get; set; }

    // ── Behaviour ───────────────────────────────────────────────────
    [Parameter] public bool Multiple { get; set; }
    [Parameter] public bool AllowClear { get; set; }
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public bool AllowTagging { get; set; }
    [Parameter] public string Placeholder { get; set; } = "Select...";
    [Parameter] public string NoResultsText { get; set; } = "No results found";
    [Parameter] public int MinimumInputLength { get; set; }
    [Parameter] public int DebounceMs { get; set; } = 300;
    [Parameter] public int MaxDropdownHeight { get; set; } = 250;

    // ── Remote / async data ─────────────────────────────────────────
    [Parameter] public Func<string, CancellationToken, Task<IEnumerable<TItem>>>? SearchAsync { get; set; }

    // ── Events ──────────────────────────────────────────────────────
    [Parameter] public EventCallback<TItem> OnSelect { get; set; }
    [Parameter] public EventCallback<TItem> OnUnselect { get; set; }
    [Parameter] public EventCallback OnOpen { get; set; }
    [Parameter] public EventCallback OnClose { get; set; }
    [Parameter] public EventCallback<string> OnTagCreate { get; set; }

    // ── Templates ───────────────────────────────────────────────────
    [Parameter] public RenderFragment<TItem>? ItemTemplate { get; set; }
    [Parameter] public RenderFragment<TItem>? SelectionTemplate { get; set; }

    // ── Additional attributes ───────────────────────────────────────
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    // ── Internal state ──────────────────────────────────────────────
    private string _searchText = string.Empty;
    private bool _isOpen;
    private bool _isLoading;
    private int _highlightedIndex = -1;
    private List<TItem> _filteredItems = [];
    private List<TValue> _selectedValues = [];
    private readonly Dictionary<TValue, TItem>? _asyncItemsCache = new();
    private ElementReference _searchInputRef;
    private ElementReference _containerRef;
    private CancellationTokenSource? _debounceCts;
    private CancellationTokenSource? _searchCts;
    private DotNetObjectReference<BlabSelect2<TItem, TValue>>? _dotNetRef;
    private IJSObjectReference? _jsModule;
    private string _containerId = $"blab-select2-{Guid.NewGuid():N}";

    private bool IsAsyncMode => SearchAsync is not null;

    // ── Lifecycle ───────────────────────────────────────────────────
    protected override void OnInitialized()
    {
        if (Multiple && Values is not null)
            _selectedValues = Values.ToList();
    }

    protected override void OnParametersSet()
    {
        if (Multiple && Values is not null)
        {
            _selectedValues = Values.ToList();
        }

        if (!_isOpen)
        {
            ApplyFilter();
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _dotNetRef = DotNetObjectReference.Create(this);
            _jsModule = await JS.InvokeAsync<IJSObjectReference>(
                "import", "./Components/Select2/BlabSelect2.razor.js");
            await _jsModule.InvokeVoidAsync("init", _containerId, _dotNetRef);
        }
    }

    // ── Open / Close ────────────────────────────────────────────────
    private async Task ToggleDropdown()
    {
        if (Disabled) return;

        if (_isOpen)
            await CloseDropdown();
        else
            await OpenDropdown();
    }

    private async Task OpenDropdown()
    {
        if (_isOpen || Disabled) return;

        _isOpen = true;
        _highlightedIndex = -1;
        _searchText = string.Empty;

        if (IsAsyncMode)
            _filteredItems = [];
        else
            ApplyFilter();

        await OnOpen.InvokeAsync();
        StateHasChanged();

        await Task.Yield();
        try { await _searchInputRef.FocusAsync(); } catch { }
    }

    private async Task CloseDropdown()
    {
        if (!_isOpen) return;

        _isOpen = false;
        _searchText = string.Empty;
        _highlightedIndex = -1;
        await OnClose.InvokeAsync();
        StateHasChanged();
    }

    [JSInvokable]
    public async Task OnClickOutside()
    {
        await CloseDropdown();
    }

    // ── Search / Filter ─────────────────────────────────────────────
    private async Task OnSearchInput(ChangeEventArgs e)
    {
        _searchText = e.Value?.ToString() ?? string.Empty;
        _highlightedIndex = -1;

        if (IsAsyncMode)
        {
            _debounceCts?.Cancel();
            _debounceCts = new CancellationTokenSource();
            var token = _debounceCts.Token;

            try
            {
                await Task.Delay(DebounceMs, token);
                await PerformSearch();
                StateHasChanged();
            }
            catch (TaskCanceledException) { }
        }
        else
        {
            ApplyFilter();
        }
    }

    private async Task PerformSearch()
    {
        if (MinimumInputLength > 0 && _searchText.Length < MinimumInputLength)
        {
            _filteredItems = [];
            return;
        }

        if (SearchAsync is not null)
        {
            _searchCts?.Cancel();
            _searchCts = new CancellationTokenSource();

            try
            {
                _isLoading = true;
                StateHasChanged();

                var results = await SearchAsync(_searchText, _searchCts.Token);
                _filteredItems = results.ToList();

                foreach (var item in _filteredItems)
                {
                    var key = ValueSelector(item);
                    _asyncItemsCache![key] = item;
                }
            }
            catch (OperationCanceledException) { }
            finally
            {
                _isLoading = false;
            }
        }
        else
        {
            ApplyFilter();
        }
    }

    private void ApplyFilter()
    {
        if (string.IsNullOrEmpty(_searchText))
        {
            _filteredItems = Data.ToList();
        }
        else
        {
            _filteredItems = Data
                .Where(item => TextSelector(item)
                    .Contains(_searchText, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }

    // ── Selection ───────────────────────────────────────────────────
    private async Task SelectItem(TItem item)
    {
        var value = ValueSelector(item);

        if (Multiple)
        {
            if (!_selectedValues.Contains(value))
            {
                _selectedValues.Add(value);
                await ValuesChanged.InvokeAsync(_selectedValues);
                await OnSelect.InvokeAsync(item);
            }

            _searchText = string.Empty;
            if (!IsAsyncMode) ApplyFilter();
            StateHasChanged();
            try { await _searchInputRef.FocusAsync(); } catch { }
        }
        else
        {
            Value = value;
            await ValueChanged.InvokeAsync(value);
            await OnSelect.InvokeAsync(item);
            await CloseDropdown();
        }
    }

    private async Task UnselectValue(TValue value)
    {
        if (Disabled) return;

        _selectedValues.Remove(value);
        await ValuesChanged.InvokeAsync(_selectedValues);

        var item = GetItemByValue(value);
        if (item is not null)
            await OnUnselect.InvokeAsync(item);

        StateHasChanged();
    }

    private async Task ClearSelection()
    {
        if (Multiple)
        {
            _selectedValues.Clear();
            await ValuesChanged.InvokeAsync(_selectedValues);
        }
        else
        {
            Value = default;
            await ValueChanged.InvokeAsync(default);
        }

        _searchText = string.Empty;

        if (IsAsyncMode)
            _filteredItems = [];
        else
            ApplyFilter();

        StateHasChanged();
    }

    private bool IsSelected(TItem item)
    {
        var value = ValueSelector(item);

        if (Multiple)
            return _selectedValues.Any(v =>
                EqualityComparer<TValue>.Default.Equals(v, value));

        return EqualityComparer<TValue>.Default.Equals(Value, value);
    }

    private bool HasSelection =>
        Multiple ? _selectedValues.Count > 0 : Value is not null && !EqualityComparer<TValue>.Default.Equals(Value, default);

    // ── Display helpers ─────────────────────────────────────────────
    private string GetSelectedText()
    {
        if (Value is null || EqualityComparer<TValue>.Default.Equals(Value, default))
            return string.Empty;

        var item = GetItemByValue(Value);
        return item is not null ? TextSelector(item) : string.Empty;
    }

    private TItem? GetItemByValue(TValue value)
    {
        var item = Data.FirstOrDefault(i =>
            EqualityComparer<TValue>.Default.Equals(ValueSelector(i), value));

        if (item is null && _asyncItemsCache!.TryGetValue(value, out var cached))
            return cached;

        return item;
    }

    private string GetTextByValue(TValue value)
    {
        var item = GetItemByValue(value);
        return item is not null ? TextSelector(item) : value?.ToString() ?? string.Empty;
    }

    private bool ShowMinLengthHint =>
        MinimumInputLength > 0 && _searchText.Length < MinimumInputLength;

    private string MinLengthHintText =>
        $"Please enter {MinimumInputLength - _searchText.Length} or more characters";

    private bool ShowTypeToSearchHint =>
        IsAsyncMode && !ShowMinLengthHint && string.IsNullOrEmpty(_searchText) && _filteredItems.Count == 0;

    // ── Keyboard navigation ─────────────────────────────────────────
    private async Task OnKeyDown(KeyboardEventArgs e)
    {
        switch (e.Key)
        {
            case "ArrowDown":
                if (!_isOpen)
                {
                    await OpenDropdown();
                }
                else if (_filteredItems.Count > 0)
                {
                    _highlightedIndex = (_highlightedIndex + 1) % _filteredItems.Count;
                }
                break;

            case "ArrowUp":
                if (_isOpen && _filteredItems.Count > 0)
                {
                    _highlightedIndex = _highlightedIndex <= 0
                        ? _filteredItems.Count - 1
                        : _highlightedIndex - 1;
                }
                break;

            case "Enter":
                if (_isOpen && _highlightedIndex >= 0 && _highlightedIndex < _filteredItems.Count)
                {
                    await SelectItem(_filteredItems[_highlightedIndex]);
                }
                else if (_isOpen && AllowTagging && !string.IsNullOrWhiteSpace(_searchText))
                {
                    await OnTagCreate.InvokeAsync(_searchText);
                    _searchText = string.Empty;
                    ApplyFilter();
                }
                break;

            case "Escape":
                await CloseDropdown();
                break;

            case "Backspace":
                if (Multiple && string.IsNullOrEmpty(_searchText) && _selectedValues.Count > 0)
                {
                    await UnselectValue(_selectedValues[^1]);
                }
                break;
        }
    }

    // ── CSS helpers ─────────────────────────────────────────────────
    private string ContainerCssClass
    {
        get
        {
            var css = "blab-select2";
            if (_isOpen) css += " blab-select2--open";
            if (Disabled) css += " blab-select2--disabled";
            if (Multiple) css += " blab-select2--multiple";
            if (HasSelection) css += " blab-select2--has-value";
            return css;
        }
    }

    private string GetItemCssClass(int index, TItem item)
    {
        var css = "blab-select2__option";
        if (index == _highlightedIndex) css += " blab-select2__option--highlighted";
        if (IsSelected(item)) css += " blab-select2__option--selected";
        return css;
    }

    // ── Dispose ─────────────────────────────────────────────────────
    public async ValueTask DisposeAsync()
    {
        _debounceCts?.Cancel();
        _debounceCts?.Dispose();
        _searchCts?.Cancel();
        _searchCts?.Dispose();

        if (_jsModule is not null)
        {
            try
            {
                await _jsModule.InvokeVoidAsync("destroy", _containerId);
                await _jsModule.DisposeAsync();
            }
            catch { }
        }

        _dotNetRef?.Dispose();
    }
}
