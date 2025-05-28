using Applications.Responses.ViewModels.Loans.Payments;
using Applications.Responses.ViewModels.Loans.Schedules;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace TrapchumApp.Components.Pages.Transactions.Loans.Disbursements
{
    public partial class PaidSchedules
    {
        [Parameter] public int? CurrencyId { get; set; } = 1;
        [Parameter] public bool ShowCloseButton { get; set; } = false;
        [Parameter] public EventCallback CloseButton { get; set; }

        private IEnumerable<VSchedule> _schedules = [];
        async void RowSelectedChanged(VSchedule s)
        {
            var para = new Dictionary<string, object?>
        {
            {"LoanId",s.LoanId},
            {"BranchId",s.BranchId},
            {"InstallmentNo",s.InstallmentNo}
        };

            var options = new SideDialogOptions
            {
                CloseDialogOnOverlayClick = true,
                Position = DialogPosition.Right,
                ShowMask = true,
                Width = "40rem",
                CloseTabIndex = 1,
                WrapperCssClass = "rz-background-color-primary"
            };


            await DialogService.OpenSideAsync<PaymentLineModal>("Payments", para, options);
        }
        async Task closePage()
        {
            await CloseButton.InvokeAsync();
        }
    }
}