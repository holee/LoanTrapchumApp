using Applications.Features.Collaterals.Dtos;
using Applications.Features.Collaterals.Queries.GetCollateralLinesAsPaging;
using Applications.Features.Customers.Queries.GetCustomerAsPaging;
using Applications.Features.Customers.Response;
using Applications.Responses.Paginations;

namespace TrapchumApp.Pages.Lists.Collaterals;

public partial class CollateralLinesIndex
{

    private IEnumerable<VCollateralLines>? _CollateralLines = [];
    private IEnumerable<VCustomerList> _customers = [];
    private int _count = 0;
    private int _pageSize = 15;
    private int _skip = 0;
    private string _searchString = string.Empty;
    private string _customerId = string.Empty;
    private string _sortColumn = nameof(VCollateralLines.TypeName);
    private bool _sortDescending = false;
    protected override async Task OnInitializedAsync()
    {

        await GetCollateralLines(_searchString);
        //await GetCustomers(null);
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
            await GetCollateralLines();
        }

    }

    async Task OnCustomerChange(object? arg)
    {
        if(arg is not null)
        {
            _customerId = (arg as VCustomerList)?.CustomerId!; 
        }
        else
        {
            _customerId = string.Empty;

            await GetCollateralLines();
        }  
    }
    async Task SelectedItemChanged(object? arg)
    {
        if (arg is not null)
        {
            _customerId = (arg as VCustomerList)?.CustomerId!;

            _searchString = string.Empty;

            _skip = 0;

            await GetCollateralLines();
        }

    }

    async Task OnSearch(string e)
    {
        _searchString = e;

        await GetCollateralLines(e);

    }


    void PageSizeChanged(int page)
    {
        _pageSize = page;
        _skip = 0;
    }


    private async Task GetCustomers(string? filter)
    {
        var param = new QueryParam(0, 15, nameof(VCustomerList.FullName), filter);

        var query = new GetCustomerAsPagingQuery(0, null, _customerId, param);

        var rsp = await mediator.Send(query);

        _customers = rsp.Items ?? [];

    }

    protected async Task GetCollateralLines(string? filter = null)
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

            var query = new GetCollateralLineAsPagingQuery(_customerId, param);

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
