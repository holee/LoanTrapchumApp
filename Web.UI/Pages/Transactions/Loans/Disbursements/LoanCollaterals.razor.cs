using Applications.Features.Loans.Dtos;

namespace Web.UI.Pages.Transactions.Loans.Disbursements;

public partial class LoanCollaterals 
{

    [Parameter] 
    public IEnumerable<LoanCollateralLinesDtoEdit> Collaterals { get; set; } = []; 
   
}

