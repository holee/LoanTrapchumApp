namespace Applications.Features.Loans.Dtos.Requests;

public static class LoanDtoCreateExtension
{
    public static LoanDtoCreate ToLoanDtoCreate(this LoanDtoCreate loanDtoCreate,
        int? branchId,
        string? userId,
        decimal? penaltyRate,
        int? loanStatusRefId,
        int? loanProductRefId,
        int? loanSubTypeRefId,
        int? bankId = 11,
        int? termTypeId = 11)
    {
        return new LoanDtoCreate(
            branchId,
            userId,
            penaltyRate,
            loanStatusRefId,
            loanProductRefId,
            loanSubTypeRefId,
            bankId,
            termTypeId);
    }
}


