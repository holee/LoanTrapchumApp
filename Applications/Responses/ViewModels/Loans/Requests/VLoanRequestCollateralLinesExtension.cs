using Domains.Models.Collaterals;

namespace Applications.Responses.ViewModels.Loans.Requests;

public static class VLoanRequestCollateralLinesExtension
{
    public static List<CollateralLinkDtoCreate> CollateralLinkDtoCreate(this IEnumerable<VLoanRequestCollateralLines> cols, int branchId, long loanRequestId)
    {
        return cols.Select(c => new CollateralLinkDtoCreate
        {
            Id = c.Id,
            BranchId = branchId,
            LoanRequestRefId = loanRequestId,
            CollateralRefId = c.CollateralId,
            CollateralLineRefId = c.CollateralLineId,
            IsActive = true
        }).ToList() ?? [];
    }

    public static List<CollateralLinkDtoCreate> CollateralLinkDtoCreate(this IEnumerable<VLoanRequestCollateralLines> cols)
    {
        return cols.Select(c => new CollateralLinkDtoCreate
        {
            Id = c.Id,
            BranchId = c.BranchId,
            LoanRequestRefId = c.LoanRequestId,
            CollateralRefId = c.CollateralId,
            CollateralLineRefId = c.CollateralLineId,
            IsActive = true
        }).ToList() ?? [];
    }

}


