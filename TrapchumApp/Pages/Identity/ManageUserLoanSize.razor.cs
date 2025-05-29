using Applications.Extensions;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Settings;
using Applications.Services.Settings;
using Radzen;

namespace TrapchumApp.Pages.Identity;
public partial class ManageUserLoanSize
{
    private readonly ISettingServiceImpl _settingService;

    public ManageUserLoanSize(ISettingServiceImpl settingService)
    {
        _settingService = settingService;
    }

    private IEnumerable<VUserLoanSizes> _loanSizes = [];

    private int pageNumber=0;

    private int pageSize=10;

    private string sortField="Id";

    private bool isIncludeInActive=false;

    private bool sortDescending=false;

    private int _counts = 0;

    private bool isLoading = false;

    private string _filter = "";
    protected override async Task OnInitializedAsync()
    {
        await GetUserLoanSizesAsync();

        await base.OnInitializedAsync();
    }


    async Task GetUserLoanSizesAsync(string? userRefId=null,string? filter=null)
    {

        isLoading = true;   

        var q = new QueryParam(pageNumber, pageSize, sortField, isIncludeInActive, sortDescending, filter);

        var rsp =await _settingService.GetUserLoanSizes(userRefId, q);

        _loanSizes = rsp.Items;

        _counts=rsp.Count;

        isLoading = false;

    }

    async Task ManageLoanSize(long? id,string? userId) 
    {

        if(id<=0 || userId.IsNullOrEmpty())
        {
            return;
        }

        var options = new DialogOptions { };

        var param = new Dictionary<string, object?>
        {
            {"UserId",userId},
            {"Id",id}
        };

        bool? dialogueResult = await DialogService.OpenAsync<SetUserLoanSizeEdit>("Loan Size", param, options);

        if (dialogueResult is not null && dialogueResult==true)
        {
            await GetUserLoanSizesAsync();
        }
    }

    private async Task PageSizeChanged(int args)
    {
        pageNumber = args;

        pageNumber = 0;

        await GetUserLoanSizesAsync();

    }
    private async Task LoadData(LoadDataArgs args)
    {
        pageNumber = args.Skip ?? 0;

        if (args.OrderBy.Any()) 
        {
            var sort = args.Sorts;

            sortField = sort.First().Property ?? "Id";

            sortDescending = sort.First().SortOrder == SortOrder.Descending;

        }

        await GetUserLoanSizesAsync();
    }
    private async Task CheckedChanged(bool args)
    {
        isIncludeInActive = args;

        await GetUserLoanSizesAsync(filter: _filter);
    }
    private async Task SearchTextChanged(string args)
    {
        _filter = args;

        await GetUserLoanSizesAsync(filter: _filter);
    }
    private void SelectedRowChanged(VUserLoanSizes args)
    {}
}
