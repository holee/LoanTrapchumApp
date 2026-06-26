using Applications.Features.Loans.Dtos;
using Radzen.Blazor;

namespace TrapchumApp.Pages.Transactions.Loans.Disbursements;

public partial class LoanGaurantors 
{
    [Parameter] public IEnumerable<LoanGuarantorDtoEdit> Guarantors { get; set; } = [];
    
    RadzenDataGrid<LoanGuarantorDtoEdit> gridGuarantors = default!;

}