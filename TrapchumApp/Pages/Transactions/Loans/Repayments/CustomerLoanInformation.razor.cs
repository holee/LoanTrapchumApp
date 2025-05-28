using Applications.Responses.ViewModels.Loans;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace TrapchumApp.Components.Pages.Transactions.Loans.Repayments
{
    public partial class CustomerLoanInformation
    {
        [CascadingParameter]
        Task<AuthenticationState> AuthenticationState { get; set; } = default!;
        [Parameter] public VLoanDisbursments LoanSchedule { get; set; } = new();
        [Parameter] public Guid? UserId { get; set; }
        bool collapse = false;
        string collapseIcon = "expand_more";
        void CollapseButton()
        {
            collapse = !collapse;
            collapseIcon = collapse ? "expand_more" : "expand_less";
        }
    }
}