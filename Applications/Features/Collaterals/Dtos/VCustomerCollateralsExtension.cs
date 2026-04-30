using Domains.Models.Collaterals;

namespace Applications.Features.Collaterals.Dtos;

public static class VCustomerCollateralsExtension
{

    public static CollateralDtoCreate ToCollateralDtoCreate(this VCustomerCollaterals c)
    {
        return new CollateralDtoCreate
        {
            CollateralId = c.CollateralId,
            CustomerName = c.CustomerName,
            DocumentKeeping = c.DocumentKeeping ?? string.Empty,
            Remark = c.Remark ?? string.Empty,
            RegisterDate = c.RegisterDate ?? DateTime.UtcNow,
        };

    }

    public static CollateralLineDtoCreate ToCollateralLineDtoCreate(this CollateralLine collateralLine)
    {
        return new CollateralLineDtoCreate
        {
            Id = collateralLine.Id,
            CategoryRefId = collateralLine.CategoryRefId,
            TypeName = collateralLine.TypeName,
            CollateralNo = collateralLine.CollateralNo,
            SubCategoryRefId = collateralLine.SubCategoryRefId,
            MarketPrice = collateralLine.MarketPrice,
            HotPrice = collateralLine.HotPrice,
            PawnPrice = collateralLine.PawnPrice,
            ReferenceNo = collateralLine.ReferenceNo,
            CollateralRefId = collateralLine.CollateralRefId,
            Description = collateralLine.Description,
        };
    }



}