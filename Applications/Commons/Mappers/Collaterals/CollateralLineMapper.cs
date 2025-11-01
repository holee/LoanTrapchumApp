using Applications.Features.Collaterals.Dtos;
using Domains.Models.Collaterals;

namespace Applications.Commons.Mappers.Collaterals;

public static class CollateralLineMapper
{
    public static CollateralLine MapCollateralLine(this CollateralLineDtoCreate dto)
    {
        return new CollateralLine
        {
            Id = dto.Id,
            CollateralRefId = dto.CollateralRefId,
            CategoryRefId = dto.CategoryRefId,
            TypeName = dto.TypeName,
            SubCategoryRefId = dto.SubCategoryRefId,
            SubTypeName = dto.CollateralSubTypeName,
            CollateralNo = dto.CollateralNo,
            OwnerName = dto.OwnerName,
            WareHouse = dto.WareHouse,
            Location = dto.Location,
            Size = dto.Size,
            Quantity = dto.Quantity,
            MarketPrice = dto.MarketPrice,
            HotPrice = dto.HotPrice,
            PawnPrice = dto.PawnPrice,
            Description = dto.Description,
        };

    }

    public static CollateralLineDtoCreate MapCollateralLineEdit(this CollateralLine dto)
    {
        return new CollateralLineDtoCreate
        {
            Id = dto.Id,
            CollateralRefId = dto.CollateralRefId,
            CategoryRefId = dto.CategoryRefId,
            TypeName = dto.TypeName,
            SubCategoryRefId = dto.SubCategoryRefId,
            //SubTypeName = dto.CollateralSubTypeName,
            CollateralNo = dto.CollateralNo,
            OwnerName = dto.OwnerName,
            WareHouse = dto.WareHouse,
            Location = dto.Location,
            Size = dto.Size,
            Quantity = dto.Quantity,
            MarketPrice = dto.MarketPrice,
            HotPrice = dto.HotPrice,
            PawnPrice = dto.PawnPrice,
            Description = dto.Description,
        };

    }



    public static CollateralLine MapCollateralLine(this CollateralLineDtoUpdate dto)
    {
        return new CollateralLine
        {
            Id = dto.Id,
            CollateralRefId = dto.CollateralRefId,
            CategoryRefId = dto.CategoryRefId,
            TypeName = dto.TypeName,
            SubCategoryRefId = dto.SubCategoryRefId,
            SubTypeName = dto.SubTypeName,
            CollateralNo = dto.CollateralNo,
            OwnerName = dto.OwnerName,
            WareHouse = dto.WareHouse,
            Location = dto.Location,
            Size = dto.Size,
            Quantity = dto.Quantity,
            MarketPrice = dto.MarketPrice,
            HotPrice = dto.HotPrice,
            PawnPrice = dto.PawnPrice,
            Description = dto.Description,
        };

    }


    public static CollateralLineDtoCreate MapCollateralLineCreate(this VCollateralLines? source)
    {

        if (source is null) return new();

        return new CollateralLineDtoCreate
        {
            Id = source.Id,
            CollateralRefId = source.CollateralId,
            CategoryRefId = source.CategoryRefId,
            TypeName = source.TypeName,
            SubCategoryRefId = source.SubCategoryRefId!.Value,
            CollateralSubTypeName = source.CollateralSubTypeName,
            CollateralNo = source.CollateralNo,
            OwnerName = source.OwnerName,
            WareHouse = source.WareHouse,
            Location = source.Location,
            Size = source.Size,
            Quantity = source.Quantity,
            MarketPrice = source.MarketPrice,
            HotPrice = source.HotPrice,
            PawnPrice = source.PawnPrice,
            Description = source.Description,
        };

    }


    public static CollateralLineDtoCreate MapCollateralLineEdit(this VCollateralLines sourse)
    {

        if (sourse is null) return new();

        return new CollateralLineDtoCreate
        {
            Id = sourse.Id,
            CollateralRefId = sourse.CollateralId,
            CategoryRefId = sourse.CategoryRefId,
            TypeName = sourse.TypeName,
            SubCategoryRefId = sourse.SubCategoryRefId,
            SubTypeName = sourse.CollateralSubTypeName,
            CollateralNo = sourse.CollateralNo,
            OwnerName = sourse.OwnerName,
            WareHouse = sourse.WareHouse,
            Location = sourse.Location,
            Size = sourse.Size,
            MarketPrice = sourse.MarketPrice,
            HotPrice = sourse.HotPrice,
            PawnPrice = sourse.PawnPrice,
            Description = sourse.Description,
            IsAvailable = sourse.IsAvailable,
            Quantity = sourse.Quantity,
            IsReturn = sourse.IsReturn,
            ReferenceNo = sourse.ReferenceNo,
            ReturnDate = sourse.ReturnDate,
        };

    }

}
