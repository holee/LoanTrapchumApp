using Applications.Enumerations;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans.Requests;
using Applications.Services.Loans;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace TrapchumApp.Components.Pages.Transactions.Loans.LoanRequests
{
    public partial class LoanRequestPending
    {
        [Parameter] public int BranchId { get; set; } = 0;
        [Parameter] public Guid? UserId { get; set; } = Guid.Empty;
        bool isLoading = false;
        IEnumerable<VLoanRequests> LoanRequests = [];
        int counts = 0;
        int pageIndex = 0;
        int pageSize = 10;
        bool sortDescending = false;
        bool isIncludeInactive = false;
        string filtertext = "";
        string sortColumn = "RequestDate";

        async Task GetLoanRequestAsync()
        {
            isLoading = true;

            var param = new QueryParam(pageIndex, pageSize, sortColumn, sortDescending, isIncludeInactive, filtertext);

            var rsp = await _loanRequestService.GetLoanRequestsAsync(UserId, BranchId, RequestStatus.All, param);
            LoanRequests = rsp.Items;
            counts = rsp.Count;
            isLoading = false;
            StateHasChanged();
        }

        async Task LoadLoanData(LoadDataArgs args)
        {
            pageIndex = args.Skip!.Value;

            filtertext = args.Filter ?? string.Empty;

            if (!string.IsNullOrEmpty(args.OrderBy))
            {
                sortColumn = args.OrderBy.Split(" ")[0];

                string dir = args.OrderBy.Split(" ")[1];

                sortDescending = dir.Contains("asc") ? false : true;
            }

            await GetLoanRequestAsync();
        }

        void RowSelectedChanged()
        {
        }

        protected override async Task OnInitializedAsync()
        {
            await GetLoanRequestAsync();
            await base.OnInitializedAsync();
        }
    }
}