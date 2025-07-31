using Applications.Requests.Lists.Collaterals;
using Applications.Responses.Lists.Collaterals;
using Domains.Models.Collaterals;

namespace Applications.Features.Customers.Mappers
{
    public static class CollateralMapper
    {
        public static Collateral MapCollateral(this CollateralDtoCreate dto)
        {
            return new Collateral
            {
                CollateralId = dto.CollateralId,
                CustomerName = dto.CustomerName,
                DocumentKeeping = dto.DocumentKeeping,
                RegisterDate = dto.RegisterDate,
                Remark = dto.Remark,
                CollateralLines = dto.CollateralLines.Select(x => x.MapCollateralLine()).ToList(),
            };
        }
    }
    public static class CollateralLineMapper
    {
        public static CollateralLine MapCollateralLine(this CollateralLineDtoCreate dto)
        {
            return new CollateralLine
            {
                Id= dto.Id,
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


        public static CollateralLineDtoCreate MapCollateralLineCreate(this VCollateralLines sourse)
        {
            return new CollateralLineDtoCreate
            {
                Id = sourse.Id,
                CollateralRefId = sourse.CollateralId,
                CategoryRefId = sourse.CategoryRefId,
                TypeName = sourse.TypeName,
                SubCategoryRefId = sourse.SubCategoryRefId!.Value,
                CollateralSubTypeName = sourse.CollateralSubTypeName,
                CollateralNo = sourse.CollateralNo,
                OwnerName = sourse.OwnerName,
                WareHouse = sourse.WareHouse,
                Location = sourse.Location,
                Size = sourse.Size,
                Quantity = sourse.Quantity,
                MarketPrice = sourse.MarketPrice,
                HotPrice = sourse.HotPrice,
                PawnPrice = sourse.PawnPrice,
                Description = sourse.Description,
            };

        }


        public static CollateralLineDtoCreate MapCollateralLineEdit(this VCollateralLines sourse)
        {
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
}
