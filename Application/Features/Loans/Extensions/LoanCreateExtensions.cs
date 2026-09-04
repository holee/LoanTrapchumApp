using Application.Calculations;

namespace Application.Features.Loans.Extensions;

public static class LoanCreateExtensions
{
    public static LoanInfo ToLoanInfo(this LoanDtoCreate arg)
    {
        return new LoanInfo(arg);
    }


}