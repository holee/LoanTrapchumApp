using Applications.Interfaces.Loans;
using Applications.Responses.Loans.Disbursements;
using Microsoft.AspNetCore.Components;

namespace TrapchumApp.Components.Pages.Transactions.Loans.LoanProducts
{
    public partial class LPModalDetail
    {
        [Parameter] public int Id { get; set; }
        [Inject] private ILoanProductService? Service { get; set; }

        [Parameter] public LoanProductResponse? model { get; set; }

        protected bool IsActive = false;

        protected override async Task OnInitializedAsync()
        {
           
            await base.OnInitializedAsync();
        }
    }
}