using Applications.Features.Collaterals.Dtos;
using Applications.Features.Collaterals.Queries.GetCollateralLinesAsPaging;
using Applications.Features.Customers.Queries.GetCustomerAsPaging;
using Applications.Features.Customers.Response;

namespace Web.UI.Pages.Lists.Collaterals;

public partial class CollateralLinesIndex
{

    private IEnumerable<VCollateralLines>? _CollateralLines = [];
    private IEnumerable<VCustomerList> _customers = [];
    private int _count = 0;
    private int _pageSize = 15;
    private int _skip = 0;
    private string _filterText = string.Empty;
    private string _customerId = string.Empty;
    private string _sortColumn = nameof(VCollateralLines.LoanStatus);
    private bool _sortDescending = false;

    protected override async Task OnInitializedAsync()
    {
        await GetCollateralLines(string.Empty, _filterText);

        await GetCustomers(string.Empty);

        await base.OnInitializedAsync();
    }

    void OnRowRender(RowRenderEventArgs<VCollateralLines> args)
    {
        if (args.Data is null) return;

        args.Data.RowIndex = args.Index + 1;
    }

    async Task OnLoadCustomerData(LoadDataArgs arg)
    {
        var filter = arg.Filter ?? string.Empty;

        await GetCustomers(filter);

        if (string.IsNullOrEmpty(filter))
        {
            await GetCollateralLines(_customerId, _filterText);
        }
    }

    async Task OnCustomerChange(object? arg)
    {
        if (arg is null)
        {
            await GetCollateralLines(string.Empty, _filterText);
        }
    }

    async Task SelectedItemChanged(object? arg)
    {
        if (arg is not null)
        {
            _customerId = (arg as VCustomerList)?.CustomerId!;

            _filterText = string.Empty;

            _skip = 0;

            await GetCollateralLines(_customerId, _filterText);
        }
    }

    async Task OnSearch(string e)
    {
        _filterText = e;

        await GetCollateralLines(e);
    }

    void PageSizeChanged(int page)
    {
        _pageSize = page;
        _skip = 0;
    }

    void AddCollateralTab()
    {
        if (string.IsNullOrEmpty(_customerId))
        {
            _toastService.ShowError("Please select a customer first");

            return;
        }

        var tabKey = $"create_{_customerId}_{Guid.NewGuid():N}";

        RenderFragment content = builder =>
        {
            builder.OpenComponent<CustomerCollateralLineCreate>(0);

            builder.AddAttribute(1, "CollateralId", _customerId);

            builder.AddAttribute(2, "OnClose", EventCallback.Factory.Create(this, async () =>
            {
                CloseTab(tabKey);

                await GetCollateralLines(_customerId);

            }));

            builder.SetKey(tabKey);

            builder.CloseComponent();
        };

        _tabs.Add(new CustomTab(tabKey, "Add Collateral", TabActions.Create, content));

        selectedIndex = _tabs.Count;

        StateHasChanged();
    }

    private async Task GetCustomers(string? filter)
    {
        var param = new QueryParam(0, 15, nameof(VCustomerList.FullName), filter);

        var query = new GetCustomerAsPagingQuery(0, null, _customerId, param);

        var rsp = await mediator.Send(query);

        _customers = rsp.Items ?? [];
    }

    protected async Task GetCollateralLines(string? customerId = null, string? filter = null)
    {
        Tracker.Show();
        try
        {
            var param = new QueryParam
            {
                Page = _skip,
                Take = _pageSize,
                SortColumn = _sortColumn,
                SortDescending = _sortDescending,
                Filter = filter
            };

            var query = new GetCollateralLineAsPagingQuery(customerId, param);

            var rsp = await mediator.Send(query);

            _CollateralLines = rsp.Items ?? [];

            _count = rsp.Count;


            Tracker.Hide();

            StateHasChanged();
        }
        catch 
        {
            Tracker.Hide();
        }
    }
}
