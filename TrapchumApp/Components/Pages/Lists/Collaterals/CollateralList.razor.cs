using Applications.Extensions;
using Applications.Requests.Collaterals;
using Applications.Responses.Collaterals;
using Applications.Responses;
using Applications.Responses.ViewModels.Collaterals;
using Applications.Responses.ViewModels.Customers;
using Applications.Services.Customers;
using Applications.Services.LIsts;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using Radzen;
namespace TrapchumApp.Components.Pages.Lists.Collaterals;

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
    bool IsShowDetails = false;
    bool IsShowAdd = false;
    bool IsProgress = false;
    RadzenTabs? tabs;
    List<TabItems> tabItems = [];
    int tabIndex = 1;
    int selectedIndex = 0;
    CollateralResponse? Collateral { get; set; } = new();
    IEnumerable<CollateralLineResponse>? CollateralLines = [];
    CollateralResponse? SelectedCollateral = new();
    LoanCustomerQuery? SelectedCustomers = new();
    Guid? CollateralId = Guid.Empty;
    IEnumerable<VCustomerList> custlist = [];
    IEnumerable<CustomerCollateral>? cusName = [];
    CustomerCollateral? SelectedCustomer = new();
    IEnumerable<CollateralTypeResponse> collateralTypes = [];
    IEnumerable<CollateralSubTypeResponse> collateralProperties = [];
    IEnumerable<CollateralSubTypeResponse> collateralPersonals = [];

    public CollateralList(ICollateralsServiceImpl collateralNewService, ICustomerServiceImpl customerService)
    {
        _colService = collateralNewService;
        _cusService = customerService;
    }

    void SearchCollateral()
    {
        GetCollaterals(searchText);
        StateHasChanged();
    }

    async Task AddedMoreCollateral(Guid? Id)
    {
        try
        {
            var response = await _cusService.GetCustomersAsync(null, 25, CustomerInActive, false);

            if (response.IsSuccess)
            {
                custlist = response.Data!;

                cusName = custlist.Select(x => new CustomerCollateral
                {
                    CustomerId = x.CustomerId,
                    Name = x.AltName,
                });
            }
            collateralTypes = await _colService.GetCollateralTypes();
            collateralProperties = await _colService.GetCollateralSubTypes(1);
            collateralPersonals = await _colService.GetCollateralSubTypes(3);
            IsShowAdd = true;
            selectedIndex = 1;
            tabs!.Reload();
            var rsp = await _colService.GetCollateralByIdAsync(Id!.Value);
            if (rsp.IsSuccess)
            {
                var collateral = rsp.Data!;
                CollateralId = collateral!.CollateralId;
                SelectedCollateral!.CollateralId = collateral!.CollateralId;
                SelectedCustomer!.CustomerId = collateral!.CollateralId;
                SelectedCustomer!.Name = collateral!.CustomerName;
                SelectedCollateral.CustomerName = collateral.CustomerName;
                SelectedCollateral.CollateralLines = collateral!.CollateralLines;
                SelectedCollateral.Remark = collateral!.Remark;
                SelectedCollateral.DocumentKeeping = collateral!.DocumentKeeping;
                SelectedCollateral.RegisterDate = collateral!.RegisterDate;

            }
            StateHasChanged();

            // IsProgress = false;
        }
        catch (Exception ex)
        {
            NotificationService.Notify(Radzen.NotificationSeverity.Error, "Exception", ex!.Message);
            Console.WriteLine(ex.Message);
            // IsProgress = false;

        }
        await InvokeAsync(StateHasChanged);
    }
    async void AddNewTab()
    {
        // IsProgress = true;
        try
        {
            IsShowAdd = true;
            selectedIndex = 1;
            tabs!.Reload();
            SelectedCollateral = new();
            SelectedCustomer = new();
            var response = await _cusService.GetCustomersAsync(null, 25, CustomerInActive, false);
            if (response.IsSuccess)
            {
                custlist = response.Data!;
                if (custlist.Any())
                {
                    cusName = custlist.Select(x => new CustomerCollateral
                    {
                        CustomerId = x.CustomerId,
                        Name = x.AltName,
                    });
                }
            }
            collateralTypes = await _colService.GetCollateralTypes();
            collateralProperties = await _colService.GetCollateralSubTypes(1);
            collateralPersonals = await _colService.GetCollateralSubTypes(3);
            StateHasChanged();
        }
        catch (Exception ex)
        {
            NotificationService.Notify(Radzen.NotificationSeverity.Error, "Exception", ex!.Message);
            Console.WriteLine(ex.Message);
            // IsProgress = false;

        }

    }
    async void AddDetailsTab(Guid? customerId)
    {
        try
        {
            var rsp = await _colService.GetCollateralByIdAsync(customerId);

            if ((rsp.IsSuccess))
            {
                SelectedCustomers!.CustomerId = rsp.Data!.CollateralId;

                SelectedCustomers.AltName = Collateral!.CustomerName;

                CollateralLines = rsp.Data.CollateralLines;

                IsShowDetails = true;

                selectedIndex = 2;

                tabs!.Reload();

                StateHasChanged();
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Warning, "", "Try getting data not found!");

            }
        }
        catch (Exception ex)
        {
            NotificationService.Notify(NotificationSeverity.Error, "", ex.InnerException!.Message);

        }

    }
    void IndexChanged(int index)
    {
        selectedIndex = index;
    }
    void CloseDetail()
    {
        IsShowDetails = false;
        selectedIndex = 0;
        StateHasChanged();
    }
    void RemoveTab()
    {
        IsShowAdd = false;
        selectedIndex = 0;
        StateHasChanged();
        // if (tabItems.Count > 0 && selectedIndex > 0)
        // {
        //     tabItems.RemoveAt(selectedIndex);
        // }
        // if (tabItems.Count == 0)
        // {
        //     selectedIndex = 0;
        // }
    }
    protected void GetCollaterals(string? filter = null)
    {
        try
        {
            //collateralGrid = async req =>
            //{
            //    var sortColumnName = req.SortByColumn!.Title ?? "#";
            //    var sortByColumn = sortColumnName switch
            //    {
            //        "#" => "CustomerId",
            //        "Date" => "RegisterDate",
            //        "Customer" => "CustomerName",
            //        _ => "Id"
            //    };
            //    var sortOder = req.SortByAscending;
            //    var sortIndex = req.StartIndex == 0 ? 0 : req.StartIndex;
            //    var sortCount = req.Count ?? 10;
            //    var res = await CollateralNewService!.GetsCollateralAysnc(sortIndex, sortCount, sortByColumn, sortOder, filter);
            //    Collaterals!.AddRange(res.Items);
            //    return GridItemsProviderResult.From(res.Items!, res.Count!);
            //};
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    protected async override Task OnInitializedAsync()
    {
        GetCollaterals();
        await base.OnInitializedAsync();
    }
    async void Reload()
    {
        await OnInitializedAsync();
        StateHasChanged();
    }
    void OnSearch(ChangeEventArgs e)
    {
        searchText = e.Value!.ToString()!;
        GetCollaterals(searchText);

    }
    private async void ShowModal()
    {

        await Task.CompletedTask;
    }
}