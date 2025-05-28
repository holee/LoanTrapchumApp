using Applications.Interfaces.Loans;
using Applications.Requests.Loans.Products;
using Microsoft.AspNetCore.Components;

namespace TrapchumApp.Components.Pages.Transactions.Loans.LoanProducts
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