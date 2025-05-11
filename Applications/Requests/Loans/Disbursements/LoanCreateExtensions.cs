using Applications.Calculations;
using Domains.Models.Loans;

namespace Applications.Requests.Loans.Disbursements;

public static class LoanCreateExtensions
{
    public static LoanInfo ToLoanInfo(this LoanDtoCreate arg)
    {
        return new LoanInfo(arg);
    }


}