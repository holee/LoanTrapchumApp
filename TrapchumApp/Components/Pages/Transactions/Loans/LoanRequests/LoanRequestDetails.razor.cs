using Applications.Extensions;
using Applications.Responses;
using Applications.Responses.ViewModels.Loans.Requests;
using Applications.Services.Customers;
using Applications.Services.Loans;
using Microsoft.AspNetCore.Components;

namespace TrapchumApp.Components.Pages.Transactions.Loans.LoanRequests
{
    public partial class LoanRequestDetails
    {

        private readonly ICustomerServiceImpl _customerService;
        private readonly ILoanRequestServiceImpl _loanRequestService;

        public LoanRequestDetails(ICustomerServiceImpl customerService, ILoanRequestServiceImpl loanRequestService)
        {
            _customerService = customerService;
            _loanRequestService = loanRequestService;
        }

        [Parameter] public Guid? LoanRequestId { get; set; }
        [Parameter] public Guid? CustomerId { get; set; }
        [Parameter] public int BranchId { get; set; }
        [Parameter] public VLoanRequests? Data { get; set; } = null;
        [Parameter] public VCustomerList? Customer { get; set; } = null;
        protected override async Task OnParametersSetAsync()
        {
            if (Data is null && CustomerId.NotEmpty() && BranchId > 0 && LoanRequestId.NotEmpty())
            {
                var rsp = await _loanRequestService.GetLoanRequestsAsync(BranchId, CustomerId, LoanRequestId, true);

                if (rsp.IsSuccess)
                {
                    Data = rsp.Data;
                }
            }
            await GetCustomerLoanReuqest(CustomerId);

            await base.OnParametersSetAsync();
        }


        async Task GetCustomerLoanReuqest(Guid? customerId)
        {
            if (customerId.NotEmpty() && Customer is null)
            {
                var resp = await _customerService.GetCustomerForRequestLoanAsync(customerId);

                if (resp.IsSuccess)
                {
                    Customer = resp.Data!;
                }
            }
        }

    }
}