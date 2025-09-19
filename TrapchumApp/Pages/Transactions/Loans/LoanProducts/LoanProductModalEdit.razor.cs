using Applications.Features.Products.Requests;
using Applications.Interfaces.Loans;
using Microsoft.AspNetCore.Components;

namespace TrapchumApp.Pages.Transactions.Loans.LoanProducts
{
    public partial class LoanProductModalEdit
    {
        [Parameter] public int Id { get; set; }
        [Inject] private ILoanProductService? service { get; set; }

        protected LoanProductDtoUpdate model = new LoanProductDtoUpdate();

        protected bool IsActive = false;

        protected async Task OnSaveChanges()
        {
            await Task.CompletedTask;
        }
    }
}