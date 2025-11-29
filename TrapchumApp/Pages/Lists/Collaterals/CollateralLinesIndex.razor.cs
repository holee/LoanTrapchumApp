using Applications.Features.Collaterals.Dtos;
using Applications.Features.Collaterals.Queries.GetCollateralLinesAsPaging;
using Applications.Features.Customers.Queries.GetCustomerAsPaging;
using Applications.Features.Customers.Response;
using Applications.Responses.Paginations;
using System.Threading.Tasks;

namespace TrapchumApp.Pages.Lists.Collaterals;

public partial class CollateralLinesIndex
{

    private IEnumerable<VCollateralLines>? _CollateralLines = [];
    private IEnumerable<VCustomerList> _customers = [];
    private int _count = 0;
    private int _pageSize = 15;
    private int _skip = 0;
    private string _filterText = string.Empty;
    private string _customerId = string.Empty;
    private string _sortColumn = nameof(VCollateralLines.TypeName);
    private bool _sortDescending = false;
    protected override async Task OnInitializedAsync()
    {

        await GetCollateralLines(string.Empty, _filterText);
        await GetCustomers(string.Empty);
        await base.OnInitializedAsync();
    }

    void OnRowRender(RowRenderEventArgs<VCollateralLines> args)
    {
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

    async void AddCollateral()
    {
        if (string.IsNullOrEmpty(_customerId))
        {
            NotificationService.Notify(NotificationSeverity.Error, "Please selected customer first");
            return;
        }

        var parameters = new Dictionary<string, object?> {

            {"CollateralId", _customerId }
        };

        var options = new DialogOptions
        {
            Width = "50vw"
        };

        var dia = await DialogService.OpenAsync<CustomerCollateralLineCreate>("Add Collateral", parameters, options);

        if (dia is bool result && result == true)
        {
            await GetCollateralLines(_customerId, _filterText);
        }
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
        try
        {
            var param = new QueryParam
            {
                Skip = _skip,
                PageSize = _pageSize,
                SortColumn = _sortColumn,
                SortDescending = _sortDescending,
                Filter = filter
            };

            var query = new GetCollateralLineAsPagingQuery(customerId, param);

            var rsp = await mediator.Send(query);

            _CollateralLines = rsp.Items ?? [];

            _count = rsp.Count;

            StateHasChanged();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }



}
