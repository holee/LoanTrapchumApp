using Applications.Responses.ViewModels.Loans;
using Applications.Services.Loans;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace TrapchumApp.Pages.Transactions.Loans.Disbursements
{
    public partial class IndexLoan
    {


        private readonly ILoanServiceImpl _dService;

        public IndexLoan(ILoanServiceImpl dService)
        {
            _dService = dService;
        }
        [Parameter] public int BranchId { get; set; } = 0;
        [Parameter, SupplyParameterFromQuery] public string? UserId { get; set; }
        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;

        bool branchStatus = false;
        int counts = 0;
        int loanCount = 0;
        int pageSize = 10;
        bool isLoading = false;
        bool isProgess = false;
        bool isAdmin = false;
        bool isNewLoan = false;
        bool isScheduleTab = false;
        bool includeInactive = false;
        int selectedTabIndex = 0;
        List<int> tabCounts = [0];
        string sortField = "RegisterDate";
        bool sortDescending = true;
        int pageIndex = 0;

        void ChangedPageSize(int page)
        {
            pageSize = page;
        }


        void AddNewLoanTab()
        {
            NavigationManager.NavigateTo("/Loan/Disbursement/CreateNewUser");
        }
        void CloseLoanTab()
        {

            if (tabCounts.Count == 1) return;
            if (selectedTabIndex == 1)
            {
                tabCounts.Remove(1);
                isNewLoan = false;
            }
            if (selectedTabIndex == 2)
            {
                tabCounts.Remove(2);
                isScheduleTab = false;
            }
            selectedTabIndex = tabCounts.Count - 1;
        }


        void SelectedTabChanged(int selectedIndex)
        {
            selectedTabIndex = selectedIndex;
        }


        void DownLoadScheduleAsExcel(string? loanRefId, int? branchId)
        {
            ContextMenuService.Close();
            isProgess = true;

            //var stream = await _dService.DownLoadScheduleAsExcel(loanRefId, branchId);
            //using var streamRef = new DotNetStreamReference(stream: stream);
            //await js.InvokeVoidAsync("downloadFileFromStream", "Schedule.xlsx", streamRef);
            isProgess = false;
            StateHasChanged();
        }



        void DownLoadScheduleAsPdf(string? loanRefId, int? branchId)
        {

            ContextMenuService.Close();

            isProgess = true;

            //var stream = await _dService.DownLoadScheduleAsPdf(loanRefId, branchId);

            //using var streamRef = new DotNetStreamReference(stream: stream);

            //await js.InvokeVoidAsync("downloadFileFromStream", "Schedule.pdf", streamRef);
            //isProgess = false;
            StateHasChanged();
        }


        void GetLoanCount(int count)
        {
            loanCount = count;
        }

        void GetLoanDetails(VLoanDisbursments p)
        {
            NavigationManager.NavigateTo($"/Loans/Details/{p.LoanId}?BranchId={p.BranchId}&CreditAgentId={p.CreditAgentRefId}&CurrencyId={p.CurrencyId}&UserId={UserId}");
            return;
        }
        void GetPaymentSchedules(VLoanDisbursments p)
        {
            NavigationManager.NavigateTo($"/Loans/{p.LoanId}/Repayment?BranchId={p.BranchId}&CreditAgentId={p.CreditAgentRefId}&CurrencyId={p.CurrencyId}&UserId={UserId}");
            return;
        }

        void ChangeCreditAgent(VLoanDisbursments p)
        {
            NavigationManager.NavigateTo($"/Loan/CreditAgent/{p.LoanId}/Changed?CustomerId={p.CustomerId}&BranchId={p.BranchId}&CreditAgentId={p.CreditAgentRefId}&CurrencyId={p.CurrencyId}&UserId={UserId}");
            return;
        }
        void CustomerPayOff(VLoanDisbursments p)
        {
            NavigationManager.NavigateTo($"/Loan/CreditAgent/{p.LoanId}/Changed?CustomerId={p.CustomerId}&BranchId={p.BranchId}&CreditAgentId={p.CreditAgentRefId}&CurrencyId={p.CurrencyId}&UserId={UserId}");
            return;
        }

        //async Task GetBranchStatus(int branchId)
        //{
        //    var resultResponse = await _branchService.GetBranchStatus(branchId);

        //    if (resultResponse.IsSuccess)
        //    {
        //        branchStatus = resultResponse.Data!.IsOpen;

        //    }
        //}

    }
}
//int pageSize = 10;
//bool isLoading = false;
//bool isProgess = false;
//bool isAdmin = false;
//bool isNewLoan = false;
//bool isScheduleTab = false;
//bool includeInactive = false;
//int selectedTabIndex = 0;
//List<int> tabCounts = [0];
//string sortField = "RegisterDate";
//bool sortDescending = true;
//int pageIndex = 0;
//private List<VLoans> LoanItems = [];
//private IList<VLoans> SelectedLoans = [];
//private string? filter = string.Empty;
//protected IEnumerable<VBranch> Branches = [];
//protected IEnumerable<VSchedulePaid> PaidSchedules = [];
//private int selectedBranchId;
//protected string filterText = string.Empty;
// async void TextLoanSearchChanged()
// {
//     await GetLoans();
// }

// async void CheckedIncludeChanged()
// {
//     await GetLoans();
// }
// void AddNewLoanTab()
// {
//     NavigationManager.NavigateTo("/Loan/Disbursement/CreateNewUser");
// }
// void CloseLoanTab()
// {

//     if (tabCounts.Count == 1) return;
//     if (selectedTabIndex == 1)
//     {
//         tabCounts.Remove(1);
//         isNewLoan = false;
//     }
//     if (selectedTabIndex == 2)
//     {
//         tabCounts.Remove(2);
//         isScheduleTab = false;
//     }
//     selectedTabIndex = tabCounts.Count - 1;
// }


// void SelectedTabChanged(int selectedIndex)
// {
//     selectedTabIndex = selectedIndex;
// }


// protected override void OnParametersSet()
// {
//     selectedBranchId = BranchId;

//     base.OnParametersSet();
// }

// async Task SelectedPageChanged()
// {
//     await GetLoans();
// }


// private async Task LoadData(LoadDataArgs args)
// {
//     pageIndex = args.Skip!.Value;

//     if (!string.IsNullOrEmpty(args.OrderBy))
//     {
//         sortField = args.OrderBy.Split(" ")[0];

//         string dir = args.OrderBy.Split(" ")[1];

//         sortDescending = dir.Contains("asc") ? false : true;
//     }
//     await GetLoans();
// }

// private async Task GetLoans()
// {
//     try
//     {
//         isLoading = true;
//         var res = await _dService!.GetLoansAsync(BranchId, sortField, pageIndex, pageSize, 0,1, sortDescending, includeInactive, filterText);
//         LoanItems = res.Items;
//         counts = res.Count;
//         isLoading = false;
//         StateHasChanged();
//     }
//     catch (Exception ex)
//     {
//         isLoading = false;
//         NotificationService.Notify(NotificationSeverity.Error, ex.Message);
//     }
//     finally
//     {
//         isLoading = false;
//     }

// }

// async Task SelecedBranchChanged()
// {
//     await GetLoans();
// }

// async Task DownLoadScheduleAsExcel(long loanRefId, int branchId)
// {
//     ContextMenuService.Close();
//     isProgess = true;

//     var stream = await _dService.DownLoadScheduleAsExcel(loanRefId, branchId);
//     using var streamRef = new DotNetStreamReference(stream: stream);
//     await _js.InvokeVoidAsync("downloadFileFromStream", "Schedule.xlsx", streamRef);
//     isProgess = false;
//     StateHasChanged();
// }
// async Task DownLoadScheduleAsPdf(long loanRefId, int branchId)
// {

//     ContextMenuService.Close();

//     isProgess = true;
//     var stream = await _dService.DownLoadScheduleAsPdf(loanRefId, branchId);

//     using var streamRef = new DotNetStreamReference(stream: stream);

//     await _js.InvokeVoidAsync("downloadFileFromStream", "Schedule.pdf", streamRef);
//     isProgess = false;
//     StateHasChanged();
// }

// async void ShowPaidSchedules(long? loanId, int? branchId)
// {
//     ContextMenuService.Close();
//     if (!isScheduleTab)
//     {
//         isScheduleTab = true;
//         tabCounts.Add(2);
//     }

//     selectedTabIndex = 2;

//     var response = await _dService.GetLoansPaidSchedules(loanId, branchId);

//     if (response.IsSuccess)
//     {
//         PaidSchedules = response.Data!;
//     }
//     else
//     {

//         PaidSchedules = [];

//         var error = response.Errors!.FirstOrDefault();

//         NotificationService.Notify(NotificationSeverity.Error, error);
//     }
//     StateHasChanged();
// }


// void ShowContextMenuWithContent(MouseEventArgs args, VLoans p)
// {
//     ContextMenuService.Open(args, ds =>@<RadzenMenu Style="font-size:0.8rem;">
//     <RadzenMenuItem Icon="money"
//                     Click="@(()=>GetPaymentSchedules(p))"
//                     Text="Recieve Payment"></RadzenMenuItem>
//     <RadzenMenuItem Icon="paid"
//                     Click="@(()=>CustomerPayOff(p))"
//                     Text="PayOff"></RadzenMenuItem>
//     <RadzenMenuItem Icon="change_circle"
//                     Click="@(()=>ChangeCreditAgent(p))"
//                     Text="Change Credit Agent"></RadzenMenuItem>
//     <RadzenMenuItem Icon="info"
//                     Click="@(()=>GetLoanDetails(p))"
//                     Text="Loan Details"></RadzenMenuItem>
//     <RadzenMenuItem Icon="file_download"
//                     Click="@(()=>DownLoadScheduleAsExcel(p.Id!.Value,p.BranchId!.Value))"
//                     Text="Export Loan to Excel" />
//     <RadzenMenuItem Icon="picture_as_pdf"
//                     Click="@(()=>DownLoadScheduleAsPdf(p.Id!.Value,p.BranchId!.Value))"
//                     Text="Export Loan to Pdf" />
// </RadzenMenu>
// );
// }

// void ChangedPageSize(int page)
// {
//     pageSize = page;
// }
