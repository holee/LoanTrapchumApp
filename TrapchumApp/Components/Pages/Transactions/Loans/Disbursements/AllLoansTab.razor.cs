using Applications.Enumerations;
using Applications.Interfaces.Loans;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Branches;
using Applications.Responses.ViewModels.Loans;
using Applications.Responses.ViewModels.Loans.Payments;
using Applications.Services.Loans;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Radzen;
namespace TrapchumApp.Components.Pages.Transactions.Loans.Disbursements
{
    public partial class AllLoansTab
    {
       private readonly ILoanServiceImpl _dService;

        public AllLoansTab(ILoanServiceImpl dService)
        {
            _dService = dService;
        }

        [Parameter] public int BranchId { get; set; } = 0;
        [Parameter] public string? UserId { get; set; }
        [Parameter] public EventCallback<int> GetLoanChanged { get; set; }
        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;
        private List<VLoanDisbursments> LoanItems = [];
        private IList<VLoanDisbursments> SelectedLoans = [];
        private string? filter = string.Empty;
        protected IEnumerable<VBranchResponse> Branches = [];
        private int selectedBranchId;
        bool branchStatus = false;
        int counts = 0;
        int pageSize = 10;
        bool isLoading = false;
        bool isProgess = false;
        bool includeInactive = false;
        string sortField = "Id";
        bool sortDescending = true;
        int pageIndex = 1;
        async Task ChangedPageSize(int page)
        {
            pageSize = page;
            await GetLoans();
        }
        async void TextLoanSearchChanged(string filterText)
        {
            filter = filterText;
            await GetLoans();
        }
        async void CheckedIncludeChanged()
        {
            await GetLoans();
        }
        void GetLoanDetails(VLoanDisbursments p)
        {
            NavigationManager.NavigateTo($"/App/Loan/{BranchId}/Branches/{p.LoanId}/Details?CreditAgentId={p.CreditAgentRefId}&CurrencyId={p.CurrencyId}&UserId={UserId}");
            return;
        }
        void GetPaymentSchedules(VLoanDisbursments p)
        {
            NavigationManager.NavigateTo($"/App/Loans/{p.BranchId}/Branches/{p.LoanId}/Schedules?CreditAgentId={p.CreditAgentRefId}&CurrencyId={p.CurrencyId}&UserId={UserId}");
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
        protected override async Task OnInitializedAsync()
        {
            await GetLoans();
            await GetLoanCount();
            await base.OnInitializedAsync();
        }

        private async Task LoadData(LoadDataArgs args)
        {
            pageIndex = args.Skip!.Value == 0 ? 1 : args.Skip!.Value;

            if (!string.IsNullOrEmpty(args.OrderBy))
            {
                sortField = args.OrderBy.Split(" ")[0];

                string dir = args.OrderBy.Split(" ")[1];

                sortDescending = dir.Contains("asc") ? false : true;
            }
            await GetLoans();
        }

        private async Task GetLoans()
        {
            try
            {
                var param = new QueryParam(pageIndex, pageSize, sortField, sortDescending, filter);

                isLoading = true;

                var res = await _dService.GetLoansAsync(BranchId, null, LoanStatuses.Active, param);

                LoanItems = res.Items;

                counts = res.Count;

                isLoading = false;

                StateHasChanged();
            }
            catch (Exception ex)
            {
                isLoading = false;

                NotificationService.Notify(NotificationSeverity.Error, ex.Message);
            }
            finally
            {
                isLoading = false;
            }

        }


        async Task DownLoadScheduleAsExcel(string loanRefId, int branchId)
        {
            ContextMenuService.Close();
            isProgess = true;

            var stream = await _dService.DownLoadScheduleAsExcel(loanRefId, branchId);

            using var streamRef = new DotNetStreamReference(stream: stream);

            await js.InvokeVoidAsync("downloadFileFromStream", "Schedule.xlsx", streamRef);

            isProgess = false;

            StateHasChanged();
        }

        async Task GetLoanCount()
        {
            await GetLoanChanged.InvokeAsync(counts);
        }





        async Task DownLoadScheduleAsPdf(string? loanRefId, int? branchId)
        {

            ContextMenuService.Close();

            isProgess = true;

            //var stream = await _dService.DownLoadScheduleAsPdf(loanRefId, branchId);

            //using var streamRef = new DotNetStreamReference(stream: stream);

            await js.InvokeVoidAsync("open", $"/Reports/Schedules/{loanRefId}/{branchId}", "_blank");
           
            isProgess = false;
            
            StateHasChanged();
        }



        //async void ShowPaidSchedules(long loanId, int branchId)
        //{
        //    ContextMenuService.Close();

        //    var response = await _dService.GetPaymentSchedulesByLoanId(loanId, branchId);

        //    if (response.IsSuccess)
        //    {
        //        PaidSchedules = response.Data!;
        //    }
        //    else
        //    {

        //        PaidSchedules = [];

        //        var error = response.Errors!.FirstOrDefault();

        //        NotificationService.Notify(NotificationSeverity.Error, error);
        //    }
        //    StateHasChanged();
        //}

    }
}
