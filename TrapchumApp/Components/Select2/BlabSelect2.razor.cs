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

    // ── Size ────────────────────────────────────────────────────────
    /// <summary>Visual size variant: Small, Default, Large.</summary>
    [Parameter] public Select2Size Size { get; set; } = Select2Size.Default;

    // ── Selection constraints ───────────────────────────────────────
    /// <summary>Maximum number of selections allowed (0 = unlimited).</summary>
    [Parameter] public int MaxSelectionLength { get; set; }

    /// <summary>Close dropdown after a selection is made.</summary>
    [Parameter] public bool? CloseOnSelect { get; set; }

    /// <summary>Auto-select the highlighted item when the dropdown closes.</summary>
    [Parameter] public bool SelectOnClose { get; set; }

    // ── Input constraints ───────────────────────────────────────────
    /// <summary>Characters that auto-tokenize input into tags (multi-select).</summary>
    [Parameter] public char[]? TokenSeparators { get; set; }

    /// <summary>Maximum number of characters in the search input (0 = unlimited).</summary>
    [Parameter] public int MaximumInputLength { get; set; }

    // ── Appearance ──────────────────────────────────────────────────
    /// <summary>CSS width override (e.g. "300px", "50%").</summary>
    [Parameter] public string? Width { get; set; }

    /// <summary>Text direction ("rtl" or "ltr").</summary>
    [Parameter] public string? Dir { get; set; }

    /// <summary>Extra theme class suffix (e.g. "bootstrap-5" → .blab-select2--theme-bootstrap-5).</summary>
    [Parameter] public string? Theme { get; set; }

    // ── Per-option state ────────────────────────────────────────────
    /// <summary>Returns true if an item should be rendered as disabled.</summary>
    [Parameter] public Func<TItem, bool>? DisabledSelector { get; set; }

    /// <summary>Returns a group name for the item (null = ungrouped).</summary>
    [Parameter] public Func<TItem, string?>? GroupSelector { get; set; }

    // ── Localization ────────────────────────────────────────────────
    /// <summary>Localization strings. Falls back to English defaults.</summary>
    [Parameter] public Select2Language? Language { get; set; }

    // ── Remote / async data ─────────────────────────────────────────
    [Parameter] public Func<string, CancellationToken, Task<IEnumerable<TItem>>>? SearchAsync { get; set; }

    /// <summary>
    /// Resolves a single item by its value. Used in async mode to display
    /// pre-selected values that haven't been fetched via SearchAsync yet.
    /// </summary>
    [Parameter] public Func<TValue, CancellationToken, Task<TItem?>>? LoadItemByValue { get; set; }

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
    private bool _maxSelectionReached;
    private bool _initialItemsLoaded;

    private bool IsAsyncMode => SearchAsync is not null;

    /// <summary>Effective close-on-select: default true for single, false for multi.</summary>
    private bool EffectiveCloseOnSelect => CloseOnSelect ?? !Multiple;

    // ── Localization helpers ────────────────────────────────────────
    private string L_NoResults => Language?.NoResults ?? NoResultsText;
    private string L_Searching => Language?.Searching ?? "Searching…";

    private string L_InputTooShort
    {
        get
        {
            var remaining = MinimumInputLength - _searchText.Length;
            var fmt = Language?.InputTooShort ?? "Please enter {0} or more characters";
            return string.Format(fmt, remaining);
        }
    }

    private string L_MaximumSelected
    {
        get
        {
            if (Language?.MaximumSelected is not null)
                return Language.MaximumSelected(MaxSelectionLength);
            return $"You can only select {MaxSelectionLength} item(s)";
        }
    }

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

        // Reset max-selection flag
        _maxSelectionReached = false;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _dotNetRef = DotNetObjectReference.Create(this);
            _jsModule = await JS.InvokeAsync<IJSObjectReference>(
                "import", "./Components/Select2/BlabSelect2.razor.js");
            await _jsModule.InvokeVoidAsync("init", _containerId, _dotNetRef);

            // Pre-load selected items in async mode so they display correctly
            if (IsAsyncMode && !_initialItemsLoaded)
            {
                _initialItemsLoaded = true;
                await LoadInitialSelectedItems();
            }
        }
    }

    /// <summary>
    /// In async mode, pre-selected values need their items loaded
    /// so the component can display their text/template.
    /// </summary>
    private async Task LoadInitialSelectedItems()
    {
        var valuesToLoad = new List<TValue>();

        if (Multiple && _selectedValues.Count > 0)
            valuesToLoad.AddRange(_selectedValues);
        else if (!Multiple && Value is not null && !EqualityComparer<TValue>.Default.Equals(Value, default))
            valuesToLoad.Add(Value);

        if (valuesToLoad.Count == 0) return;

        var needsRefresh = false;

        foreach (var val in valuesToLoad)
        {
            // Already in cache or in Data?
            if (GetItemByValue(val) is not null) continue;

            if (LoadItemByValue is not null)
            {
                try
                {
                    var item = await LoadItemByValue(val, CancellationToken.None);
                    if (item is not null)
                    {
                        _asyncItemsCache![val] = item;
                        needsRefresh = true;
                    }
                }
                catch { }
            }
        }

        if (needsRefresh)
            StateHasChanged();
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
        _maxSelectionReached = false;

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

        // SelectOnClose: auto-select highlighted item before closing
        if (SelectOnClose && _highlightedIndex >= 0 && _highlightedIndex < _filteredItems.Count)
        {
            var item = _filteredItems[_highlightedIndex];
            if (DisabledSelector is null || !DisabledSelector(item))
            {
                await SelectItemCore(item, skipClose: true);
            }
        }

        _isOpen = false;
        _searchText = string.Empty;
        _highlightedIndex = -1;
        _maxSelectionReached = false;
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
        var raw = e.Value?.ToString() ?? string.Empty;

        // Enforce MaximumInputLength
        if (MaximumInputLength > 0 && raw.Length > MaximumInputLength)
            raw = raw[..MaximumInputLength];

        // TokenSeparators: split and auto-select matching tokens
        if (Multiple && TokenSeparators is { Length: > 0 } && raw.Length > 0)
        {
            var lastChar = raw[^1];
            if (TokenSeparators.Contains(lastChar))
            {
                var tokens = raw.Split(TokenSeparators, StringSplitOptions.RemoveEmptyEntries);
                foreach (var token in tokens)
                {
                    var trimmed = token.Trim();
                    if (string.IsNullOrEmpty(trimmed)) continue;

                    // Find matching item
                    var match = Data.FirstOrDefault(item =>
                        TextSelector(item).Equals(trimmed, StringComparison.OrdinalIgnoreCase));

                    if (match is not null)
                    {
                        await SelectItemCore(match, skipClose: true);
                    }
                    else if (AllowTagging)
                    {
                        await OnTagCreate.InvokeAsync(trimmed);
                    }
                }

                _searchText = string.Empty;
                if (!IsAsyncMode) ApplyFilter();
                StateHasChanged();
                return;
            }
        }

        _searchText = raw;
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
        // Guard: disabled items
        if (DisabledSelector is not null && DisabledSelector(item))
            return;

        await SelectItemCore(item, skipClose: false);
    }

    private async Task SelectItemCore(TItem item, bool skipClose)
    {
        var value = ValueSelector(item);

        if (Multiple)
        {
            // MaxSelectionLength check
            if (MaxSelectionLength > 0 && _selectedValues.Count >= MaxSelectionLength)
            {
                _maxSelectionReached = true;
                StateHasChanged();
                return;
            }

            if (!_selectedValues.Contains(value))
            {
                _selectedValues.Add(value);
                await ValuesChanged.InvokeAsync(_selectedValues);
                await OnSelect.InvokeAsync(item);
            }

            _searchText = string.Empty;
            _maxSelectionReached = false;
            if (!IsAsyncMode) ApplyFilter();
            StateHasChanged();

            if (!skipClose && EffectiveCloseOnSelect)
            {
                await CloseDropdown();
            }
            else
            {
                try { await _searchInputRef.FocusAsync(); } catch { }
            }
        }
        else
        {
            Value = value;
            await ValueChanged.InvokeAsync(value);
            await OnSelect.InvokeAsync(item);

            if (!skipClose && EffectiveCloseOnSelect)
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

        _maxSelectionReached = false;
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
        _maxSelectionReached = false;

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

    private bool IsItemDisabled(TItem item) =>
        DisabledSelector is not null && DisabledSelector(item);

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

    private string MinLengthHintText => L_InputTooShort;

    private bool ShowTypeToSearchHint =>
        IsAsyncMode && !ShowMinLengthHint && string.IsNullOrEmpty(_searchText) && _filteredItems.Count == 0;

    // ── Grouping helpers ────────────────────────────────────────────
    private bool HasGrouping => GroupSelector is not null;

    private IEnumerable<IGrouping<string?, TItem>> GetGroupedItems()
    {
        return _filteredItems.GroupBy(item => GroupSelector!(item) ?? string.Empty);
    }

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
                    MoveHighlight(1);
                    await ScrollToHighlighted();
                }
                break;

            case "ArrowUp":
                if (_isOpen && _filteredItems.Count > 0)
                {
                    MoveHighlight(-1);
                    await ScrollToHighlighted();
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

            case "Tab":
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

    /// <summary>
    /// Move the highlighted index by the given delta, skipping disabled items.
    /// </summary>
    private void MoveHighlight(int delta)
    {
        if (_filteredItems.Count == 0) return;

        var start = _highlightedIndex;
        var count = _filteredItems.Count;
        var next = start;

        for (var i = 0; i < count; i++)
        {
            next = ((next + delta) % count + count) % count;
            if (!IsItemDisabled(_filteredItems[next]))
            {
                _highlightedIndex = next;
                return;
            }
        }
    }

    private async Task ScrollToHighlighted()
    {
        if (_jsModule is not null && _highlightedIndex >= 0)
        {
            try
            {
                await _jsModule.InvokeVoidAsync("scrollToHighlighted", _containerId, _highlightedIndex);
            }
            catch { }
        }
    }

    // ── CSS helpers ─────────────────────────────────────────────────
    private string ContainerCssClass
    {
        get
        {
            var css = "blab-select2";

            // Size modifier
            css += Size switch
            {
                Select2Size.Small => " blab-select2--sm",
                Select2Size.Large => " blab-select2--lg",
                _ => ""
            };

            if (_isOpen) css += " blab-select2--open";
            if (Disabled) css += " blab-select2--disabled";
            if (Multiple) css += " blab-select2--multiple";
            if (HasSelection) css += " blab-select2--has-value";

            // Direction
            if (string.Equals(Dir, "rtl", StringComparison.OrdinalIgnoreCase))
                css += " blab-select2--rtl";

            // Theme
            if (!string.IsNullOrWhiteSpace(Theme))
                css += $" blab-select2--theme-{Theme}";

            return css;
        }
    }

    private string? ContainerStyle
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(Width))
                return $"width:{Width}";
            return null;
        }
    }

    private string GetItemCssClass(int index, TItem item)
    {
        var css = "blab-select2__option";
        if (index == _highlightedIndex) css += " blab-select2__option--highlighted";
        if (IsSelected(item)) css += " blab-select2__option--selected";
        if (IsItemDisabled(item)) css += " blab-select2__option--disabled";
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
