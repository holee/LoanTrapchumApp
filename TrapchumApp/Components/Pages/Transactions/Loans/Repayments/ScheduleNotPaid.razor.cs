using Applications.Responses.ViewModels.Loans;
using Applications.Responses.ViewModels.Loans.Schedules;
using Radzen;

namespace TCLMS.Front.Pages.Transactions.Loans.Repayments
{
    public partial class ScheduleNotPaid
    {
        IList<VSchedule> SelectedSchedules = [];
        bool ShowUnPaindOnly = false;
        private VLoanDisbursments? loanSchedule = new();
        int CurrencyId = 1;


        void CellRender(DataGridCellRenderEventArgs<VSchedule> args)
        {
            if (args.Data.IsPaid)
            {
                args.Attributes.Add("style", "background-color:var(--rz-danger-lighter)");
            }

        }
    }
}