using Applications.Enumerations;
using Applications.Features.LoanRequests.Queryies.GetLoanRequestAsPaging;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans.Requests;
using Domains.Models.Identity;
using System.Security.Claims;
using TrapchumApp.Helpers.Extensions;
using TrapchumApp.Shared;

namespace TrapchumApp.Pages.Transactions.LoanRequests
{
    public partial class LoanRequestAll
    {
        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;
        [CascadingParameter] private CascadingAppState bState { get; set; } = default!;
        [Parameter] public int BranchId { get; set; }
        [Parameter] public string? UserId { get; set; }

        ClaimsPrincipal? _user;

        IEnumerable<VLoanRequests> LoanRequests = [];

        int counts = 0;
        string uri = "";
        string filtertext = "";
        string sortColumn = "RequestDate";
        string createRequestUrl = "";
        int pageIndex = 0;
        int pageSize = 15;
        bool sortDescending = true;
        bool isIncludeInactive = false;
        bool isLoading = false;
        int statusId = 1;
        private BaseEnum[] _dropDownItrem = [];
        private string SelectedStatus = "";
        protected override async Task OnInitializedAsync()
        {
            createRequestUrl = $"/app/loan/request/{BranchId}/Branches/CreateNew";

            var auth = await AuthenticationState;

            _user = auth.User;

            UserId = _user.GetId();

            _dropDownItrem = default(RequestStatus).ToEnumArray();

            SelectedStatus = RequestStatus.All.ToString();

            await bState.CheckedCanDisbursemnt(_user);

            await GetLoanRequestAsync();

            await base.OnInitializedAsync();
        }

        async Task LoadLoanData(LoadDataArgs args)
        {
            pageIndex = args.Skip ?? 0;

            filtertext = args.Filter ?? string.Empty;

            (sortColumn, sortDescending) = args.PageInfo("AltName");

            await GetLoanRequestAsync();
        }

        async Task GetLoanRequestAsync()
        {

            Tracker.Show();

            string? userId = string.Empty;

            var param = new QueryParam(pageIndex, pageSize, sortColumn, false, sortDescending, filtertext);

            if (bState.CanDisbursed)
            {
                userId = string.Empty;
            }
            else
            {
                await bState.GetUserId();

                userId = bState.UserId;
            }

            var query = new GetLoanRequestAsPagingQuery(userId, BranchId, RequestStatus.All, param);

            var rsp = await mediator.Send(query);

            LoanRequests = rsp.Items;

            counts = rsp.Count;

            isLoading = false;

            Tracker.Hide();

            StateHasChanged();
        }


        async Task GetLoanRequestAsync(RequestStatus status)
        {
            isLoading = true;

            string? userId = string.Empty;

            var param = new QueryParam(pageIndex, pageSize, sortColumn, false, sortDescending, filtertext);

            if (bState.CanDisbursed)
            {
                userId = string.Empty;
            }
            else
            {
                await bState.GetUserId();
                userId = bState.UserId;
            }

            var query = new GetLoanRequestAsPagingQuery(userId, BranchId, status, param);

            var rsp = await mediator.Send(query);

            LoanRequests = rsp.Items;

            counts = rsp.Count;

            isLoading = false;

            StateHasChanged();
        }


        async void PageChanged(int size)
        {
            pageSize = size;

            pageIndex = 0;

            await GetLoanRequestAsync();
        }

        async void FilterTextChanged(string? searchText = null)
        {
            filtertext = searchText ?? "";

            await GetLoanRequestAsync();
        }

        void RowSelectedChanged()
        {

        }

        void SelectedItemChanged(object? value)
        {
            var SelectedStatus = (BaseEnum?)value;

            if (SelectedStatus is not null && SelectedStatus.Name.TryToEnum<RequestStatus>(out var status))
            {
                _ = GetLoanRequestAsync(status);
            }
            else
            {
                _ = GetLoanRequestAsync(RequestStatus.All);
            }
        }
    }
}