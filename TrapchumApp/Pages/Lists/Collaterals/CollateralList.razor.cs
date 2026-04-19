using Applications.Extensions;
using Applications.Features.Collaterals.Dtos;
using Applications.Features.Customers.Response;
using Applications.Features.Customers.Services;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Customers;
using Applications.Services.Lists;
using Radzen;
using Radzen.Blazor;

namespace TrapchumApp.Pages.Lists.Collaterals;

public partial class CollateralList
{
    private readonly ICollateralsServiceImpl _colService;
    private readonly ICustomerServiceImpl _cusService;
    protected List<VCollateral> Collaterals = [];
    protected IEnumerable<VCollateral> model { get; set; } = [];
    private bool cancelClose;
    private bool InActive = false;
    protected string searchText { get; set; } = string.Empty;
    List<int> SelectedRow = [10, 20, 50, 100];
    bool CustomerInActive = false;
    private bool IsLoading = true;
    bool IsProgress = false;
    RadzenTabs? tabs;
    int selectedIndex = 0;
    string CustomerId = string.Empty;
    VCustomerCollaterals? Collateral { get; set; } = new();
    IEnumerable<VCollateralLines>? CollateralLines = [];
    VCustomerCollaterals? SelectedCollateral = new();
    LoanCustomerQuery? SelectedCustomers = new();
    string? CollateralId = string.Empty;
    IEnumerable<VCustomerDetails> custlist = [];
    IEnumerable<CustomerCollateral>? cusName = [];
    CustomerCollateral? SelectedCustomer = new();
    IEnumerable<CollateralTypeResponse> collateralTypes = [];
    IEnumerable<CollateralSubTypeResponse> collateralProperties = [];
    IEnumerable<CollateralSubTypeResponse> collateralPersonals = [];

    private List<CustomTab> _tabs = [];

    public CollateralList(ICollateralsServiceImpl collateralNewService, ICustomerServiceImpl customerService)
    {
        _colService = collateralNewService;
        _cusService = customerService;
    }

    async Task SearchCollateral()
    {
        await GetCollaterals();
    }

    void PageSizeChanged(int page)
    {
        pageSize = page;

        pageIndex = 0;
    }

    void Delete(string? customerId) { }

    void AddDetailsTab(string? customerId)
    {
        ContextMenuService.Close();

        if (string.IsNullOrEmpty(customerId)) return;

        var existingTabIndex = _tabs.FindIndex(t => t.Key == customerId);

        if (existingTabIndex != -1)
        {
            selectedIndex = existingTabIndex + 1;
            return;
        }

        RenderFragment childContent = builder =>
        {
            builder.OpenComponent(0, typeof(CollateralDetail));
            builder.AddAttribute(1, "CustomerId", customerId);
            builder.AddAttribute(2, "Close", EventCallback.Factory.Create(this, () =>
            {
                CloseTab(customerId);
            }));
            builder.CloseComponent();
        };

        var newTab = new CustomTab(customerId, "Details", TabActions.Details, childContent);

        _tabs.Add(newTab);

        selectedIndex = _tabs.Count;

        StateHasChanged();
    }

    void CreateCollateralTab()
    {
        CreateCollateralTabForCustomer(null);
    }

    void CreateCollateralTabForCustomer(string? customerId = null)
    {
        ContextMenuService.Close();

        var newTabId = Guid.NewGuid().ToString("N");

        RenderFragment childContent = builder =>
        {
            builder.OpenComponent(0, typeof(CustomerCollateralLineCreate));

            if (!string.IsNullOrEmpty(customerId))
            {
                builder.AddAttribute(1, "CollateralId", customerId);
            }
            builder.SetKey(newTabId);

            builder.CloseComponent();
        };

        var newTab = new CustomTab(newTabId, "Add Collateral", TabActions.Create, childContent);

        _tabs.Add(newTab);

        selectedIndex = _tabs.Count;

        StateHasChanged();
    }

    void CloseTab(string? id)
    {
        var tab = _tabs.FirstOrDefault(t => t.Key == id);

        if (tab != null)
        {
            var idx = _tabs.IndexOf(tab);

            _tabs.Remove(tab);

            selectedIndex = Math.Max(0, idx);

            StateHasChanged();
        }
    }

    void IndexChanged(int index)
    {
        selectedIndex = index;
    }

    int pageIndex = 0;
    int pageSize = 15;
    bool sortDecending = false;
    string sortColumn = "CollateralId";
    int counts = 0;
    protected async Task GetCollaterals(string? filter = null)
    {
        try
        {
            var param = new QueryParam
            {
                Page = pageIndex,
                PageSize = pageSize,
                SortColumn = sortColumn,
                SortDescending = sortDecending,
                Filter = filter
            };

            var rsp = await _colService.GetCollateralsAysnc(param);

            Collaterals = rsp.Items ?? [];

            counts = rsp.Count;

            StateHasChanged();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    protected async override Task OnInitializedAsync()
    {
        await GetCollaterals();

        await base.OnInitializedAsync();
    }

    async Task OnSearch(string e)
    {
        searchText = e;
        await GetCollaterals(e);

    }
}



//async Task AddedMoreCollateral(string? Id)
//{
//    try
//    {
//        var response = await _cusService.GetCustomersAsync(null, 25, CustomerInActive, false);

//        if (response.IsSuccess)
//        {
//            custlist = response.Data!;

//            cusName = custlist.Select(x => new CustomerCollateral
//            {
//                CustomerId = x.CustomerId,
//                Name = x.AltName,
//            });
//        }
//        collateralTypes = await _colService.GetCollateralTypes();
//        collateralProperties = await _colService.GetCollateralSubTypes(1);
//        collateralPersonals = await _colService.GetCollateralSubTypes(3);
//        IsShowAdd = true;
//        selectedIndex = 1;
//        tabs!.Reload();
//        var rsp = await _colService.GetCollateralByIdAsync(Id);
//        if (rsp.IsSuccess)
//        {
//            var collateral = rsp.Data!;
//            CollateralId = collateral!.CollateralId;
//            SelectedCollateral!.CollateralId = collateral!.CollateralId;
//            SelectedCustomer!.CustomerId = collateral!.CollateralId;
//            SelectedCustomer!.Name = collateral!.CustomerName;
//            SelectedCollateral.CustomerName = collateral.CustomerName;
//            SelectedCollateral.CollateralLines = collateral!.CollateralLines;
//            SelectedCollateral.Remark = collateral!.Remark;
//            SelectedCollateral.DocumentKeeping = collateral!.DocumentKeeping;
//            SelectedCollateral.RegisterDate = collateral!.RegisterDate;

//        }
//        StateHasChanged();

//        // IsProgress = false;
//    }
//    catch (Exception ex)
//    {
//        NotificationService.Notify(NotificationSeverity.Error, "Exception", ex!.Message);
//        Console.WriteLine(ex.Message);
//        // IsProgress = false;

//    }
//    await InvokeAsync(StateHasChanged);
//}

//async void AddNewTab()
//{
//    // IsProgress = true;
//    try
//    {
//        IsShowAdd = true;
//        selectedIndex = 1;
//        tabs!.Reload();
//        SelectedCollateral = new();
//        SelectedCustomer = new();
//        var response = await _cusService.GetCustomersAsync(null, 25, CustomerInActive, false);
//        if (response.IsSuccess)
//        {
//            custlist = response.Data!;
//            if (custlist.Any())
//            {
//                cusName = custlist.Select(x => new CustomerCollateral
//                {
//                    CustomerId = x.CustomerId,
//                    Name = x.AltName,
//                });
//            }
//        }
//        collateralTypes = await _colService.GetCollateralTypes();
//        collateralProperties = await _colService.GetCollateralSubTypes(1);
//        collateralPersonals = await _colService.GetCollateralSubTypes(3);
//        StateHasChanged();
//    }
//    catch (Exception ex)
//    {
//        NotificationService.Notify(NotificationSeverity.Error, "Exception", ex!.Message);
//        Console.WriteLine(ex.Message);
//        // IsProgress = false;

//    }

//}
