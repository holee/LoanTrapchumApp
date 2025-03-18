using AppilcationServices.Interfaces;
using ApplicationServices.Interfaces;
using Microsoft.AspNetCore.Components;
using TCLMS.Shared.Entities.ViewModels.Collaterals;
namespace TCLMS.Front.Pages.Lists.Collaterals;

public partial class CollateralList
{
    [Inject] ICollateralServiceImpl CollateralNewService { get; set; } = default!;
    [Inject] ICustomerServiceImpl CustomerService { get; set; } = default!;

    protected List<VCollateral> Collaterals = [];
    protected IEnumerable<VCollateral> model { get; set; } = [];
    private bool cancelClose;
    private bool InActive = false;
    protected string searchText { get; set; } = string.Empty;
    List<int> SelectedRow = [10, 20, 50, 100];
    bool CustomerInActive = false;
    private bool IsLoading = true;
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