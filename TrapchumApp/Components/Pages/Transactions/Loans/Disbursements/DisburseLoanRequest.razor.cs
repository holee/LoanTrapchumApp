using Applications.Interfaces.Loans;
using Applications.Requests.Loans.LoanRequests;
using Applications.Responses.ViewModels.Collaterals;
using Applications.Responses.ViewModels.Customers;
using Applications.Responses.ViewModels.Loans.Requests;
using Applications.Services.Loans;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace TrapchumApp.Components.Pages.Transactions.Loans.Disbursements
{
    public partial class DisburseLoanRequest
    {

        private readonly ILoanRequestServiceImpl _requestService;

        public DisburseLoanRequest(ILoanRequestServiceImpl requestService)
        {
            _requestService = requestService;
        }

        [Parameter] public Guid? LoanRequestId { get; set; }
        [Parameter] public Guid? CustomerId { get; set; }
        [Parameter, SupplyParameterFromQuery] public int BranchId { get; set; }
        IEnumerable<VCollateralLines> CollateralLines = [];
        VLoanRequests? data = new();
        IEnumerable<VCustomerIncomes> VCustomerIncomes = [];
        IEnumerable<VCustomerExpenses> VCustomerExpenses = [];
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            await GetLoanRequestDetails();
        }


        async Task GetLoanRequestDetails()
        {
            var resp = await _requestService.GetLoanRequestsAsync(BranchId, CustomerId, LoanRequestId, true);

            if (resp.IsSuccess)
            {
                data = resp.Data;
            }
        }


        async Task AprovedLoanRequest()
        {

            var req = new AproveLoanDtoCreate(BranchId, CustomerId, LoanRequestId, 0, 0, DateTime.UtcNow, null);

            var rsp = await _requestService.AprovedLoanRequestAsync(req);

            if (rsp.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Success, rsp.Data);

                NavigationManager.NavigateTo("/Loan/Request");
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, rsp.ErrMessage);
            }
        }

        async Task DeniedLoanRequest()
        {
            var req = new DeniedRequestDtoCreate(BranchId, CustomerId, LoanRequestId, DateTime.UtcNow, string.Empty);

            var rsp = await _requestService.DeniedLoanRequestAsync(req);

            if (rsp.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Success, rsp.Data);
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, rsp.ErrMessage);
            }
        }
        async Task RecheckedLoanRequest()
        {
            var req = new RecheckedRequestDtoCreate(BranchId, CustomerId, LoanRequestId, DateTime.UtcNow, string.Empty);

            var rsp = await _requestService.RecheckedLoanRequestAsync(req);

            if (rsp.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Success, rsp.Data);
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, rsp.ErrMessage);
            }
        }

        async Task SaveLoanRequest()
        {
            var req = new RequestDtoBaseCreate(BranchId, CustomerId, LoanRequestId, string.Empty);

            var rsp = await _requestService.VerifiedLoanRequestAsync(req);

            if (rsp.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Success, rsp.Data);
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, rsp.ErrMessage);
            }
        }






        // async Task GetCustomerIncomeAndExpense()
        // {
        //     var incomeResponse = await _customerService.GetCustomerIncomesAsync(CustomerId);

        //     var expenseResponse = await _customerService.GetCustomerExpensesAsync(CustomerId);

        //     if (incomeResponse.IsSuccess)
        //     {
        //         VCustomerIncomes = incomeResponse.Data!;
        //     }
        //     else
        //     {
        //         VCustomerIncomes = [];
        //     }

        //     if (expenseResponse.IsSuccess)
        //     {
        //         VCustomerExpenses = expenseResponse.Data!;
        //     }
        //     else
        //     {
        //         VCustomerExpenses = [];
        //     }
        // }



    }
}