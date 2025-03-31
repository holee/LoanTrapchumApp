using Applications.Responses.ViewModels.Loans;
using Microsoft.AspNetCore.Components;

namespace TrapchumApp.Components.Pages.Transactions.Loans.Disbursements
{
    public partial class LoanInfos
    {
        [Parameter]
        public VLoanDisbursments LoanSchedule { get; set; } = new();
    }
}