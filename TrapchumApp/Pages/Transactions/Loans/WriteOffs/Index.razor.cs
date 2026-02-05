using Applications.Features.Branches.Dtos;
using Applications.Features.Loans.Responses;
using Applications.Features.Loans.Services;
using Applications.Responses.Paginations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;

namespace TrapchumApp.Pages.Transactions.Loans.WriteOffs
{
    public partial class Index
    {

        private readonly ILoanServiceImpl _dService;

        public Index(ILoanServiceImpl dService)
        {
            _dService = dService;
        }

        [Parameter, SupplyParameterFromQuery] public int BranchId { get; set; } = 2;
        [Parameter] public Guid? UserId { get; set; }
        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;
        List<VLoanDisbursments> LoanItems = [];
        IEnumerable<VBranch> Branches = [];
        bool isLoading = false;
        int pageSize = 10;
        string sortField = "RegisterDate";
        bool sortDescending = true;
        int pageIndex = 0;
        bool includeInactive = false;
        string searchText = string.Empty;
        int counts = 0;



        protected override async Task OnInitializedAsync()
        {
            await GetLoans();

            // var branchResponse = await _branchService.GetBranchesAsync(BranchId, 0, string.Empty);
            //
            // if (branchResponse.IsSuccess)
            // {
            //     Branches = branchResponse.Data!;
            // }
            // else
            // {
            //     Branches = [];
            // }

            await base.OnInitializedAsync();
        }
        private async Task LoadData(LoadDataArgs args)
        {
            pageIndex = args.Skip!.Value;

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

                var param = new QueryParam(pageIndex, pageSize, sortField, sortDescending, false, searchText);

                isLoading = true;

                //var res = await _dService!.GetLoansAsync(BranchId,Credit)
                //LoanItems = res.Items;
                //counts = res.Count;

                await Task.CompletedTask;

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


        void RowSelectedArg(VLoanDisbursments e) { }

    }
}