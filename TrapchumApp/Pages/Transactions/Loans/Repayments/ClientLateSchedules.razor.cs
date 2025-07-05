using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans;
using Applications.Responses.ViewModels.Loans.Schedules;
using Applications.Services.Loans;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
namespace TrapchumApp.Pages.Transactions.Loans.Repayments
{
    public partial class ClientLateSchedules
    {
        private readonly ILoanScheduleServiceImpl _scheduleService;
        public ClientLateSchedules(ILoanScheduleServiceImpl schedulesService)
        {
            _scheduleService = schedulesService;
        }

        [Parameter, EditorRequired] public int BranchId { get; set; } = 0;
        [Parameter, EditorRequired] public string? UserId { get; set; } = string.Empty;
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


        void GetPaymentSchedules(VSchedule p)
        {
            ContextMenuService.Close();
            NavigationManager.NavigateTo($"/App/Loans/{BranchId}/Branches/{p.LoanId}/Schedules?CreditAgentId={p.CreditAgentId}&CurrencyId={p.CurrencyId}&UserId={UserId}");
        }


        void CustomerPayOff(VSchedule p)
        {
            ContextMenuService.Close();

        }

        async Task GetCustomerLateSchedules(VSchedule p)
        {
            ContextMenuService.Close();

            var parameters = new Dictionary<string, object?>{
                        { "LoanId",p.LoanId },
                        { "BranchId",BranchId },
                        { "PaymentDate",paymentDate}
                    };
            var sideOptions = new SideDialogOptions() { 
                Width = "70vw",
                CloseDialogOnOverlayClick=true,
                ShowMask = true,
            };

            await DialogService.OpenSideAsync<LateScheduleDetail>("Late Schedule Detail", parameters, sideOptions);


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

            await GetCustomerLateSchedules();
        }

        protected override async Task OnInitializedAsync()
        {

            await GetCustomerLateSchedules();

            await GetCount();

            await base.OnInitializedAsync();
        }

        private void RowRenderChanged(DataGridCellRenderEventArgs<VSchedule> args)
        {

            if (args.Data.DayArrea >= 1 && args.Data.DayArrea <= 7)
            {
                args.Attributes.Add("style", "background-color: orange; color: #721c24;"); // Light red background for late schedules
            }
            else if (args.Data.DayArrea > 7 && args.Data.DayArrea <= 14)
            {
                args.Attributes.Add("style", "background-color: orange; color: #856404;"); // Light orange background for schedules with more than 7 days late
            }
            else if (args.Data.DayArrea > 14)
            {
                args.Attributes.Add("style", "background-color: yellow; color: #856404;"); // Light yellow background for schedules with more than 14 days late
            }
            else
            {
                args.Attributes.Add("style", "background-color: white; color: black;"); // Default style for non-late schedules
            }
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