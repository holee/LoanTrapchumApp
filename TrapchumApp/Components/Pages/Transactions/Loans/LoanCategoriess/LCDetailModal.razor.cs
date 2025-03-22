using AppilcationServices.Interfaces;
using Microsoft.AspNetCore.Components;
using TCLMS.Shared.Entities.Dtos.Loans;

namespace TCLMS.Front.Pages.Transactions.Loans.LoanCategoriess
{
    public partial class LCDetailModal
    {
        [Parameter] public int Id { get; set; }
        //[Inject] private ILoanCategoryService? service { get; set; }
        [Parameter] public LoanCategoryDto? category { get; set; }
   
    }
}