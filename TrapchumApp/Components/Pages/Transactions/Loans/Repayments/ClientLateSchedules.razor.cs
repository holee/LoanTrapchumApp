using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans.Schedules;
using Applications.Services.Loans;
using Microsoft.AspNetCore.Components;
using Radzen;
namespace TrapchumApp.Components.Pages.Transactions.Loans.Repayments
{
    public partial class ClientLateSchedules
    {
        private readonly ILoanScheduleService _scheduleService;
        public ClientLateSchedules(ILoanScheduleService schedulesService)
        {
            _scheduleService = schedulesService;
        }

        [Parameter, EditorRequired] public int BranchId { get; set; } = 0;
        [Parameter, EditorRequired] public long UserId { get; set; } = 0;
        [Parameter] public EventCallback<int> GetCountChanged { get; set; }
        IList<VSchedule> LatePaymentSchedules = [];
        IList<VSchedule> SelectedPaymentSchedules = [];
        bool isLoading = false;
        int counts = 0;
        int pageSize = 10;
        int pageIndex = 1;
        string sortColumn = "LoanId";
        DateTime paymentDate = DateTime.Now;
        string? filterText = "";
        bool sortDescending = false;
        void ShowCustomerPayment(VSchedule p)
        {

            var parameters = new Dictionary<string, object>{
                        { "LoanPayment",p},
                        { "CurrencyId",p.CurrencyId!.Value },
                        { "LoanId",p.LoanId!.Value },
                        { "UserId",UserId },
                        { "CreditAgentId",p.CreditAgentId!.Value
                        }};
            NavigationManager.NavigateTo($"/Loans/Repayment/{p.LoanId}/Multiple?BranchId={p.BranchId}&CreditAgentId={p.CreditAgentId}&CurrencyId={p.CurrencyId}&UserId={UserId}");
        }

        void CustomerPayOff(VSchedule p)
        {
            ContextMenuService.Close();

        }

        void GetCustomerLateSchedules(VSchedule p)
        {
            ContextMenuService.Close();
            StateHasChanged();

        }

        void ChangedPageSize(int page)
        {
            pageSize = page;
        }
        async Task GetCount()
        {
            await GetCountChanged.InvokeAsync(counts);
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

                var param = new QueryParam(pageIndex, pageSize, sortColumn, sortDescending, filterText);

                var responseData = await _scheduleService.GetLoanSchedulesByPaymentDateAsync(BranchId, paymentDate, true, true, param);

                LatePaymentSchedules = responseData.Items;

                counts = responseData.Count;

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
            await GetCustomerLateSchedules();
        }

        protected override async Task OnInitializedAsync()
        {

            await GetCustomerLateSchedules();

            await GetCount();

            await base.OnInitializedAsync();
        }

    }
}