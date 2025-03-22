using AppilcationServices.Interfaces;
using Microsoft.AspNetCore.Components;
using TCLMS.Shared.Entities.Dtos.Loans;

namespace TCLMS.Front.Pages.Transactions.Loans.LoanProducts
{
    public partial class LPModalDetail
    {
        [Parameter] public int Id { get; set; }
        [Inject] private ILoanProductService? Service { get; set; }

        [Parameter] public LoanProductDTO? model { get; set; }
        protected bool IsActive = false;

        protected override async Task OnInitializedAsync()
        {
           
            await base.OnInitializedAsync();
        }
    }
}