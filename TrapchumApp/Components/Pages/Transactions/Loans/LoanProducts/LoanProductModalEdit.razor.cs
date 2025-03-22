using AppilcationServices.Interfaces;
using Microsoft.AspNetCore.Components;
using TCLMS.Shared.Entities.Dtos.Loans;

namespace TCLMS.Front.Pages.Transactions.Loans.LoanProducts
{
    public partial class LoanProductModalEdit
    {
        [Parameter] public int Id { get; set; }
        [Inject] private ILoanProductService? service { get; set; }

        protected LoanProductDTOUpdate model = new LoanProductDTOUpdate();
        protected bool IsActive = false;

        protected async Task OnSaveChanges()
        {
            await Task.CompletedTask;
        }
    }
}