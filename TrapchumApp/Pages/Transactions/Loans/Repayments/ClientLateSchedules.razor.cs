using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans.Schedules;
using Applications.Services.Loans;
using Domains.Models.Identity;
namespace TrapchumApp.Pages.Transactions.Loans.Repayments;

public partial class ClientLateSchedules
{
    private readonly ILoanScheduleServiceImpl _scheduleService;
    public ClientLateSchedules(ILoanScheduleServiceImpl schedulesService)
    {
        _scheduleService = schedulesService;
    }
    [Parameter, EditorRequired] public int BranchId { get; set; } = 0;
    [Parameter] public EventCallback<int> GetCounts { get; set; }
    [Parameter] public EventCallback<VSchedule?> GetScheduleChanged { get; set; }
    [CascadingParameter]
    Task<AuthenticationState> AuthenticationState { get; set; } = default!;

    IList<VSchedule> LatePaymentSchedules = [];

    IList<VSchedule> SelectedPaymentSchedules = [];

    bool isLoading = false;

    int counts = 0;

    int pageSize = 15;

    int pageIndex = 0;

    string sortColumn = "LoanId";

    DateTime paymentDate = DateTime.Now;

    string? filterText = "";

    bool sortDescending = false;

    private string? _userId { get; set; } = string.Empty;
    void GetPaymentSchedules(VSchedule p)
    {
        ContextMenuService.Close();

        NavigationManager.NavigateTo($"/App/Loans/{BranchId}/Branches/{p.LoanId}/Schedules?CreditAgentId={p.CreditAgentId}&CurrencyId={p.CurrencyId}&UserId={_userId}");
    }


    void CustomerPayOff(VSchedule p)
    {
        ContextMenuService.Close();

    }

    async Task ShowLateSchedules(VSchedule? p) 
    {
        ContextMenuService.Close();

        if (p is not null)
        {
            await GetScheduleChanged.InvokeAsync(p);
        }


        // var parameters = new Dictionary<string, object?>{
        //             { "LoanId",p.LoanId },
        //             { "BranchId",BranchId },
        //             { "PaymentDate",paymentDate}
        //         };
        // var sideOptions = new SideDialogOptions() { 
        //     Width = "70vw",
        //     CloseDialogOnOverlayClick=true,
        //     ShowMask = true,
        // };
        //
        // await DialogService.OpenSideAsync<LateScheduleDetail>("Late Schedule Detail", parameters, sideOptions);


    }

    void ChangedPageSize(int page)
    {
        pageSize = page;
    }
    async Task GetCount()
    {
        await GetCounts.InvokeAsync(counts); 
    }

    async void SelectedDateChanged()
    {
        await GetCustomerLateSchedules();
    }
    async Task GetCustomerLateSchedules()
    {
        try
        {
            isLoading = true;

            var param = new QueryParam(pageIndex, pageSize, sortColumn, false, sortDescending, filterText);

            var responseData = await _scheduleService.GetLoanSchedulesByPaymentDateAsync(BranchId, paymentDate, true, true, param);

            LatePaymentSchedules = responseData.Items;

            counts = responseData.Count;

            await GetCount();

            isLoading = false;
        }
        catch
        {
            isLoading = false;
        }
        finally
        {
            isLoading = false;
        }
        StateHasChanged();
    }

    async void GetLoanDataFilter(string filter)
    {
        filterText = filter;
        pageIndex = 0;
        await GetCustomerLateSchedules();
    }

    protected override async Task OnInitializedAsync()
    {

        var auth = await AuthenticationState;

        _userId = auth.User.GetId();

        await GetCustomerLateSchedules();

        await GetCount();

        await base.OnInitializedAsync();
    }

    private void RowRenderChanged(DataGridCellRenderEventArgs<VSchedule> args)
    {
        switch (args.Data.DayArrea)
        {
            case >= 1 and <= 7:
                args.Attributes.Add("style", "background-color: orange; color: #721c24;"); // Light red background for late schedules
                break;
            case > 7 and <= 14:
                args.Attributes.Add("style", "background-color: orange; color: #856404;"); // Light orange background for schedules with more than 7 days late
                break;
            case > 14:
                args.Attributes.Add("style", "background-color: #E9D1FF; color: #856404;"); // Light yellow background for schedules with more than 14 days late
                break;
            default:
                args.Attributes.Add("style", "background-color: white; color: black;"); // Default style for non-late schedules
                break;
        }
    }
}

//void ShowCustomerPayment(VSchedule p)
//{

//    var parameters = new Dictionary<string, object?>{
//                { "LoanPayment",p},
//                { "CurrencyId",p.CurrencyId!.Value },
//                { "LoanId",p.LoanId! },
//                { "UserId",UserId },
//                { "CreditAgentId",p.CreditAgentId!
//                }};
//    NavigationManager.NavigateTo($"/Loans/Repayment/{p.LoanId}/Multiple?BranchId={p.BranchId}&CreditAgentId={p.CreditAgentId}&CurrencyId={p.CurrencyId}&UserId={UserId}");
//}