using Applications.Responses.ViewModels.Loans.Requests;

namespace Applications.Responses.Lists.Collaterals;

public sealed class VCollateralLines
{
    public long Id { get; set; }
    public string? CollateralId { get; set; }
    public int? CategoryRefId { get; set; }
    public string? TypeName { get; set; }
    public int? SubCategoryRefId { get; set; }
    public string? CollateralSubTypeName { get; set; }
    public string? ReferenceNo { get; set; }
    public string? CollateralNo { get; set; } 
    public bool IsAvailable { get; set; } = true;  
    public bool? IsReturn { get; set; }=false;    
    public DateTime? ReturnDate { get; set; } 
    public DateTime? RegisterDate { get; set; } 
    public string? OwnerName { get; set; }
    public string? Location { get; set; }
    public string? WareHouse { get; set; }
    public int? Quantity { get; set; } = 1;
    public string? Size { get; set; } 
    public decimal? MarketPrice { get; set; } = 0.00M;
    public decimal? HotPrice { get; set; } = 0.00M;
    public decimal? PawnPrice { get; set; } = 0.00M;
    public string? Description { get; set; }
    public bool IsUsed { get; set; } = false;

}


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
            CollateralSubTypeName = lines.CollateralSubTypeName,
            ReferenceNo = lines.ReferenceNo,
            CollateralNo = lines.CollateralNo,
            IsAvailable = lines.IsAvailable,
            IsReturn = lines.IsReturn,
            ReturnDate = lines.ReturnDate,
            RegisterDate = lines.RegisterDate,
            OwnerName = lines.OwnerName,
            Location = lines.Location,
            WareHouse = lines.WareHouse,
            Quantity = lines.Quantity,
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
            CollateralSubTypeName = loanCollateral.SubTypeName,
            ReferenceNo = loanCollateral.ReferenceNo,
            CollateralNo = loanCollateral.CollateralNo,
            IsAvailable = !(loanCollateral.InUsed ?? true), // Inverse of InUsed, default to false if null
            IsReturn = false, // No direct mapping, default to false
            ReturnDate = null, // No direct mapping
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
            SubTypeName = collateral.CollateralSubTypeName,
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