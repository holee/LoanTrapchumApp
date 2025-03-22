using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor.Rendering;
using Radzen.Blazor;
using Applications.Responses.ViewModels.Loans.Schedules;
using Applications.Responses.Paginations;
using Applications.Enumerations;

namespace TrapchumApp.Components.Pages.Transactions.Loans.Repayments
{
    public partial class SchedulesPaid
    {
        [Parameter] public int BranchId { get; set; } = 0;
        [Parameter] public int CurrencyId { get; set; } = 1;
        [Parameter] public long UserId { get; set; } = 0;
        [Parameter] public long LoanId { get; set; } = 0;

        IList<VSchedule> PaymentSchedules = [];
        bool isLoading = false;
        int count = 0;
        int pageSize = 25;
        string? filterText = "";
        Popup popup = default!;
        RadzenMenuItem buttonItem = default!;
        long ScheduleId = 0;

        protected override async Task OnInitializedAsync()
        {
            await GetCustomerSchedules();
            await base.OnInitializedAsync();
        }

        void ShowCustomerPayment(VSchedule p)
        {

            var parameters = new Dictionary<string, object>{
                        { "LoanPayment",p},
                        { "CurrencyId",p.CurrencyId!.Value },
                        { "LoanId",p.LoanId!.Value },
                        { "UserId",UserId },
                        { "CreditAgentId",p.CreditAgentId!.Value
                        }};
            NavigationManager.NavigateTo($"/Loans/{p.LoanId}/Schedules?BranchId={p.BranchId}&CreditAgentId={p.CreditAgentId}&CurrencyId={p.CurrencyId}&UserId={UserId}");
        }

        void RowSelecting(VSchedule s) { }



        async Task ShowScheduleDetails(VSchedule p)
        {
            //await popup.ToggleAsync(buttonItem.Element);
            ContextMenuService.Close();
            var param = new Dictionary<string, object?>
            {
                {"LoanId",LoanId },
                {"CurrencyId",CurrencyId },
                {"BranchId",BranchId },
                {"ScheduleId",p.LoanScheduleId },
            };
            var options = new SideDialogOptions
            {
                Position = DialogPosition.Right,
                ShowMask = true,
                CloseDialogOnOverlayClick = true,
                Width = "70vw"
            };
            await DialogService.OpenSideAsync<ScheduleDetails>($"Schedule-{p.LoanScheduleId}", parameters: param, options);


        }

        void ChangedPageSize(int page)
        {
            pageSize = page;
        }

        async Task GetCustomerSchedules()
        {
            try
            {
                isLoading = true;

                PaymentSchedules = [];

                var param = new QueryParam(1, pageSize, "LoanId");

                //var responseData = await _scheduleService.GetLoanScheduleByIdAsync(LoanId, BranchId, ScheduleStatus.Paid, param);

                //if (responseData.IsSuccess)
                //{

                //    PaymentSchedules = responseData.Data!.Items;

                //    count = responseData.Data!.Count;
                //}
                //else
                //{
                //    count = 0;
                //    PaymentSchedules = [];
                //    var error = responseData.Errors!.FirstOrDefault();
                //    NotificationService.Notify(NotificationSeverity.Error, error);
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
    }
}