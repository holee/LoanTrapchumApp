using Applications.Features.Loans.Responses;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace TrapchumApp.Pages.Transactions.Loans.Repayments
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