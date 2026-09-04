using Application.Responses.ViewModels.Loans.Requests;

namespace Application.Features.Collaterals.Dtos;

public static class VCollateralLinesExtensions
{

    public static VCollateralLines Map(this VCollateralLines lines)
    {
        return new VCollateralLines
        {
            Id = lines.Id,
            CollateralId = lines.CollateralId,
            CategoryRefId = lines.CategoryRefId,
            TypeName = lines.TypeName,
            SubCategoryRefId = lines.SubCategoryRefId,
            SubTypeName = lines.SubTypeName,
            ReferenceNo = lines.ReferenceNo,
            CollateralNo = lines.CollateralNo,
            IsAvailable = lines.IsAvailable,
            IsReturn = lines.IsReturn,
            ReturnDate = lines.ReturnDate,
            OwnerName = lines.OwnerName,
            Location = lines.Location,
            WareHouse = lines.WareHouse,
            Quantity = lines.Quantity,
            RegisterDate = lines.RegisterDate,
            Size = lines.Size,
            MarketPrice = lines.MarketPrice,
            HotPrice = lines.HotPrice,
            PawnPrice = lines.PawnPrice,
            Description = lines.Description,
            IsUsed = lines.IsUsed
        };
    }

    public static VCollateralLines MapToVCollateralLine(this VLoanRequestCollateralLines loanCollateral)
    {
        return new VCollateralLines
        {
            Id = loanCollateral.CollateralLineId ?? 0, // Use CollateralLineId as Id, default to 0 if null
            CollateralId = loanCollateral.CollateralId,
            CategoryRefId = null, // No direct mapping
            TypeName = loanCollateral.TypeName,
            SubCategoryRefId = null, // No direct mapping
            SubTypeName = loanCollateral.SubTypeName,
            ReferenceNo = loanCollateral.ReferenceNo,
            CollateralNo = loanCollateral.CollateralNo,
            IsAvailable = !(loanCollateral.InUsed ?? true), // Inverse of InUsed, default to false if null
            IsReturn = false, // No direct mapping, default to false
            ReturnDate = null, // No direct mapping,
            RegisterDate = null, // No direct mapping
            OwnerName = loanCollateral.OwnerName,
            Location = loanCollateral.Location,
            WareHouse = null, // No direct mapping
            Quantity = loanCollateral.Quantity,
            Size = null, // No direct mapping
            MarketPrice = loanCollateral.MarketPrice,
            HotPrice = loanCollateral.HotPrice,
            PawnPrice = loanCollateral.PawnPrice,
            Description = null, // No direct mapping
            IsUsed = loanCollateral.InUsed ?? false // Map InUsed to IsUsed, default to false if null
        };
    }

    public static IEnumerable<VCollateralLines> MapToVCollateralLines(this IEnumerable<VLoanRequestCollateralLines> lines)
    {
        return lines.Select(x => x.MapToVCollateralLine()); 
    }

    public static IEnumerable<VCollateralLines> MapToVCollateralLines(this List<VLoanRequestCollateralLines> lines)
    {
        return lines.Select(x => x.MapToVCollateralLine());
    }


    public static VLoanRequestCollateralLines MapToVLoanRequestCollateralLine(this VCollateralLines collateral)
    {
        return new VLoanRequestCollateralLines
        {
            Id = 0, // No direct mapping, default to 0
            BranchId = null, // No direct mapping
            LoanRequestId = null, // No direct mapping
            CollateralLineId = collateral.Id, // Map Id to CollateralLineId
            CollateralId = collateral.CollateralId,
            CollateralNo = collateral.CollateralNo,
            ReferenceNo = collateral.ReferenceNo,
            Location = collateral.Location,
            OwnerName = collateral.OwnerName,
            Quantity = collateral.Quantity,
            MarketPrice = collateral.MarketPrice,
            HotPrice = collateral.HotPrice,
            PawnPrice = collateral.PawnPrice,
            TypeName = collateral.TypeName,
            SubTypeName = collateral.SubTypeName,
            InUsed = collateral.IsUsed // Map IsUsed to InUsed
        };
    }

    public static IEnumerable<VLoanRequestCollateralLines> MapToVLoanRequestCollateralLine(this IEnumerable<VCollateralLines> lines)
    {
        return lines.Select(x => x.MapToVLoanRequestCollateralLine()); 
    }

    public static IEnumerable<VLoanRequestCollateralLines> MapToVLoanRequestCollateralLine(this List<VCollateralLines> lines)
    {
        return lines.Select(x => x.MapToVLoanRequestCollateralLine());
    }

}