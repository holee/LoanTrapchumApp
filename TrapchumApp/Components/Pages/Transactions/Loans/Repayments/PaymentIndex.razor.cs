using Applications.Interfaces.Loans;
using Applications.Responses.ViewModels.Branches;
using Applications.Responses.ViewModels.Loans.Schedules;
using Applications.Services.Loans;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;
using Radzen.Blazor;

namespace TrapchumApp.Components.Pages.Transactions.Loans.Repayments
{
    public partial class PaymentIndex
    {
        [CascadingParameter] Task<AuthenticationState> authenticationState { get; set; } = default!;
        [Parameter] public int BranchId { get; set; } = 0;
        [Parameter] public string? UserId { get; set; } = string.Empty;
        bool isLoading = false;
        bool isProgress = false;
        string AllScheduleCounts = "0";
        int AllLateScheduleCounts = 0;
        int DuePaymentCounts = 0;
        RadzenDataGrid<VSchedule> grid = default!;
        RadzenDataGrid<VSchedule> grid1 = default!;
        RadzenDataGrid<VSchedule> lateScheduleGrid = default!;
        IList<VSchedule> PaymentSchedules = [];
        IList<VSchedule> PaymentSchedulesOnDate = [];
        IEnumerable<VSchedule> LateSchedules = [];
        IList<VSchedule> SelectedPaymentSchedules = [];
        IEnumerable<VBranchResponse> Branches = [];
        DateTime paymentDate = DateTime.Now;
        VSchedule? SelectedSchedule = new();
        string? filterText = "";
        bool includeLateSchedule = true;
        bool isShowLateSchedule = false;
        bool isShowScheduleHistory = false;
        int selectedTabIndex = 0;
        List<int> tabcounts = [0];
        int currencyId = 0;
        string? loanId = string.Empty;
        int installmentNo = 0;


        void GetCount(string count)
        {
            AllScheduleCounts = count;
        }
        void GetDueDateScheduleCount(int count)
        {
            DuePaymentCounts = count;
        }

        void GetLateScheduleCount(int count)
        {
            AllLateScheduleCounts = count;
        }


        void CloseTab()
        {

            if (tabcounts.Count == 1) return;

            if (selectedTabIndex == 1)
            {
                tabcounts.Remove(1);
                isShowLateSchedule = false;
            }
            if (selectedTabIndex == 2)
            {
                tabcounts.Remove(2);
                isShowScheduleHistory = false;
            }

            selectedTabIndex = tabcounts.Count - 1;
        }

        void SelectedTabChanged(int tabIndex)
        {
            selectedTabIndex = tabIndex;
            Console.WriteLine(tabIndex);
        }


        void GetLoanDetails(VSchedule p)
        {
            NavigationManager.NavigateTo($"/Loans/Details/{p.LoanId}?BranchId={BranchId}&CreditAgentId={p.CreditAgentId}&CurrencyId={p.CurrencyId}&UserId={UserId}");
            return;
        }

        void GetCustomerLateSchedules(VSchedule p)
        {
            ContextMenuService.Close();
            if (p is null) return;
            if (!isShowLateSchedule)
            {
                isShowLateSchedule = true;
                tabcounts.Add(1);
            }
            selectedTabIndex = 1;
            loanId = p.LoanId!;
            StateHasChanged();

        }

        void CustomerPayOff(VSchedule p)
        {
            ContextMenuService.Close();
            if (p is null) return;
            if (!isShowLateSchedule)
            {
                isShowLateSchedule = true;
                tabcounts.Add(1);
            }

            selectedTabIndex = 1;
            loanId = p.LoanId!;
            StateHasChanged();

        }


        async void GetPaymentDialogue(VSchedule payment)
        {
            var options = new DialogOptions
            {
                Width = "720px",
                Height = "460px",
                Draggable = true,
                Resizable = false
            };
            var parameters = new Dictionary<string, object>{
                        { "LoanPayment",payment},
                        { "CurrencyId",payment.CurrencyId!.Value },
                        { "LoanId",payment.LoanId! },
                        { "UserId",UserId },
                        { "CreditAgentId",payment.CreditAgentId!
                        }};
            var response = await DialogService.OpenAsync<ConfirmPaymentDialogue>("Recieve Payment", parameters, options);

            if (((bool)response) == true)
            {

            }
        }






        void ShowCustomerPayment(VSchedule p)
        {

            var parameters = new Dictionary<string, object>{
                        { "LoanPayment",p},
                        { "CurrencyId",p.CurrencyId!.Value },
                        { "LoanId",p.LoanId!},
                        { "UserId",UserId },
                        { "CreditAgentId",p.CreditAgentId!
                        }};
            NavigationManager.NavigateTo($"/Loans/Repayment/{p.LoanId}/Multiple?BranchId={p.BranchId}&CreditAgentId={p.CreditAgentId}&CurrencyId={p.CurrencyId}&UserId={UserId}");
        }



        //async Task GetBranches(long userId)
        //{
        //    var resultResponse = await _branchService.GetBranchesAsync(userId);

        //    if (resultResponse.IsSuccess)
        //    {
        //        Branches = resultResponse.Data!;
        //    }
        //    else
        //    {
        //        var error = resultResponse.Errors!.FirstOrDefault();

        //        NotificationService.Notify(NotificationSeverity.Error, error);
        //    }

        //}
        async Task selectedPageSizeChanged()
        {
            await GetCustomerSchedules();
        }
        async void GetLoanDataFilter()
        {
            await GetCustomerSchedules();
        }
        async void SelectedDateChanged()
        {
            if (selectedTabIndex == 1)
            {

            }
            else
            {
                includeLateSchedule = true;
                //await GetCustomerSchedules();
                await grid.Reload();

            }
        }

        void GetSchedulePaymentLines(VSchedule v)
        {
            ContextMenuService.Close();

            if (v is not null)
            {
                currencyId = v.CurrencyId!.Value;
                loanId = v.LoanId;
                installmentNo = v.InstallmentNo!.Value;
                tabcounts.Add(2);
                selectedTabIndex = 2;
                isShowScheduleHistory = true;
            }


        }

        async Task GetCustomerSchedules()
        {
            try
            {
                isLoading = true;

                PaymentSchedules = [];

                //PaymentSchedulesOnDate = [];

                //var responseData = await _scheduleService.GetCustomerPaymentSchedules(BranchId, paymentDate, includeLateSchedule, filterText);

                //if (responseData.IsSuccess)
                //{
                //    if (includeLateSchedule)
                //    {
                //        PaymentSchedules = responseData.Data!;
                //    }
                //    else
                //    {
                //        PaymentSchedulesOnDate = responseData.Data!;
                //    }
                //    AllScheduleCounts = responseData.Data!.Count();
                //}
                //else
                //{
                //    //var error = responseData.Errors!.FirstOrDefault();
                //    PaymentSchedules = [];
                //    PaymentSchedulesOnDate = [];
                //    //NotificationService.Notify(NotificationSeverity.Error, error);
                //}

                await Task.CompletedTask;

                isLoading = false;
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
            StateHasChanged();
        }




        void ShowLoanDetails(VSchedule payment)
        {
            NavigationManager.NavigateTo($"/Loans/Details/{payment.LoanId}?BranchId={payment.BranchId}&CurrencyId={payment.CurrencyId}");
        }
        void ShowScheulePayment(VSchedule payment)
        {
            NavigationManager.NavigateTo($"/Loans/Repayment/{payment.LoanId}?BranchId={payment.BranchId}&CreditAgentId={payment.CreditAgentId}&CurrencyId={payment.CurrencyId}");
        }
        async void ShowPaymentDialogue(VSchedule payment)
        {
            var parameters = new Dictionary<string, object>{
                        {"LoanPayment",payment},
                        {"CurrencyId",payment.CurrencyId!.Value},
                        {"LoanId",payment.LoanId!},
                        {"UserId",UserId},
                        {"CreditAgentId",payment.CreditAgentId!}
                    };

            var options = new DialogOptions
            {
                Width = "720px",
                Height = "460px",
                Draggable = true,
                Resizable = false
            };
            var response = await DialogService.OpenAsync<ConfirmPaymentDialogue>("Confirm Payment", parameters, options);

            if (((bool)response) == true)
            {
                await GetCustomerSchedules();
            }
        }

    }
}