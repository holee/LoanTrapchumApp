using Applications.Calculations;
using Applications.Features.Loans.Requests;

namespace Applications.Features.Loans.Extensions;

public static class LoanCreateExtensions
{
    public static LoanInfo ToLoanInfo(this LoanDtoCreate arg)
    {
        return new LoanInfo(arg);
    }


}