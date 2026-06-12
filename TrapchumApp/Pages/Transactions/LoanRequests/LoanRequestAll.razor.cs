using Applications.Features.Branches.Dtos;
using Applications.Features.Branches.Queries.GetBranchAsPaging;
using Applications.Features.LoanRequests.Queryies.GetLoanRequestAsPaging;
using Applications.Responses.ViewModels.Loans.Requests;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Security.Claims;
using TrapchumApp.Shared;

namespace TrapchumApp.Pages.Transactions.LoanRequests
{
    public partial class LoanRequestAll
    {
        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;
        [CascadingParameter] private CascadingAppState bState { get; set; } = default!;
        [Parameter] public int BranchId { get; set; }

        [PersistentState(RestoreBehavior = RestoreBehavior.Default)]
        public IEnumerable<VLoanRequests>? LoanRequests { get; set; }

        private ClaimsPrincipal? _user;

        private List<VBranch> _branches = [];

        int counts = 0;

        string uri = "";

        string filtertext = "";

        string sortColumn = nameof(VLoanRequests.ModifiedDT);

        string createRequestUrl = "";

        int pageIndex = 0;

        int pageSize = 15;

        bool sortDescending = true;

        bool isIncludeInactive = false;

        bool isLoading = false;

        int statusId = 1;

        private BaseEnum[] _dropDownItrem = [];

        private string SelectedStatus = "";

        private RequestStatus _status;

        private int _selectedBranchId;

        protected override async Task OnInitializedAsync()
        {
            createRequestUrl = $"/app/loan/request/{BranchId}/Branches/CreateNew";

            var auth = await AuthenticationState;

            _user = auth.User;

            _dropDownItrem = default(RequestStatus).ToEnumArray();

            SelectedStatus = RequestStatus.All.ToString();

            _status = RequestStatus.All;

            await branchService.GetCurrentBranchNameAsync();

            await userServices.GetLoanSizeByUserAsync();

            await bState.ManageLoanDisbursement();

            if (LoanRequests is null)
            {
                await GetLoanRequestAsync();
            }

            if (branchService.IsHeadOffice == true)
            {
                await GetBranches();
            }

            await base.OnInitializedAsync();
        }


        private async Task GetBranches()
        {

            var command = new GetBranchAsPagingQuery(0, new QueryParam(0, 0, ""));

            var rsp = await mediator.Send(command);

            var unionBranches = new List<VBranch>()
                {
                    new VBranch
                    {
                        BranchId=0,

                        BranchName="All"
                    }
                };

            var branches = rsp.Items.Union(unionBranches);

            _branches = branches.OrderBy(x => x.BranchId).ToList();

        }


        private async Task SelectedBranchChanged(object? args)
        {

            if (args is not null)
            {
                _selectedBranchId = ((VBranch)args).BranchId!.Value;

                await GetLoanRequestByHeadOffice(_selectedBranchId);
            }
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

            if (branchService.IsHeadOffice == true)
            {
                await GetLoanRequestByHeadOffice(_selectedBranchId);

                return;
            }

            if (bState.ManageDisbursement)
            {
                await GetLoanRequestForDisbursement();

                return;
            }


            await GetLoanRequests();

        }




        async Task GetLoanRequestByHeadOffice(int selectedBranchId = 0)
        {

            Tracker.Show();

            var param = new QueryParam(pageIndex, pageSize, sortColumn, false, sortDescending, filtertext);

            var query = new GetLoanRequestAsPagingQuery(null, selectedBranchId, _status, param);

            var rsp = await mediator.Send(query);

            LoanRequests = rsp.Items;

            counts = rsp.Count;

            Tracker.Hide();

            StateHasChanged();
        }

        async Task GetLoanRequestForDisbursement()
        {

            Tracker.Show();

            var param = new QueryParam(pageIndex, pageSize, sortColumn, false, sortDescending, filtertext);

            var query = new GetLoanRequestAsPagingQuery(null, BranchId, _status, param);

            var rsp = await mediator.Send(query);

            LoanRequests = rsp.Items;

            counts = rsp.Count;

            Tracker.Hide();

            StateHasChanged();
        }


        async Task GetLoanRequests()
        {

            Tracker.Show();

            await bState.GetUserId();

            var param = new QueryParam(pageIndex, pageSize, sortColumn, false, sortDescending, filtertext);

            var query = new GetLoanRequestAsPagingQuery(bState.UserId, BranchId, _status, param);

            var rsp = await mediator.Send(query);

            LoanRequests = rsp.Items;

            counts = rsp.Count;

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
                _status = status;

                _ = GetLoanRequestAsync();
            }
            else
            {
                _status = RequestStatus.All;

                _ = GetLoanRequestAsync();
            }
        }
    }
}