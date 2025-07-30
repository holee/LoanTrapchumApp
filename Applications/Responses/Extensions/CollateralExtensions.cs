using Applications.Responses.Lists.Collaterals;
using Applications.Responses.ViewModels.Collaterals;
using Domains.Models.Collaterals;
using Domains.Models.Lists.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Responses.Extensions
{
    public static class CollateralExtensions
    {

        public static VCustomerCollaterals ToCollateralResponse(this EntityNames cust)
        {
            return new VCustomerCollaterals
            {
                CollateralId = cust.Customer!.CustomerId,
                CustomerName = $"{cust.FirstName} {cust.LastName}",
                DocumentKeeping = cust.Customer!.Collateral?.DocumentKeeping ?? "",
                RegisterDate = cust.Customer.Collateral?.RegisterDate ?? DateTime.UtcNow,
                Remark = cust.Customer!.Collateral!.Remark,
                CollateralLines = cust.Customer!.Collateral!.CollateralLines.ToCollateralLineResponse().ToList()
            };
        }

        public static IEnumerable<VCollateralLines> ToCollateralLineResponse(this IEnumerable<CollateralLine> collateralLines)
        {
            return collateralLines!.Select(x => new VCollateralLines
            {
                Id = x.Id,
                CollateralRefId = x.CollateralRefId,
                TypeName = x.TypeName,
                SubCategoryRefId = x.SubCategoryRefId,
                CategoryRefId = x.CategoryRefId,
                CollateralNo = x.CollateralNo,
                MarketPrice = x.MarketPrice,
                PawnPrice = x.PawnPrice,
                HotPrice = x.HotPrice,
                Size = x.Size,
                Description = x.Description,
                OwnerName = x.OwnerName,
                WareHouse = x.WareHouse,
            });

        }

        public static List<VCollateralLines> ToPersonals(this VCustomerCollaterals cust)
        {

            if (!cust.CollateralLines.Any())
            {
                return Enumerable.Empty<VCollateralLines>().ToList();
            }

            return cust.CollateralLines
                .Where(x => x.CategoryRefId == (int)CollateralTypeEnum.Personal)
                .Select(x => new VCollateralLines
                {
                    Id = x.Id,
                    CollateralRefId = x.CollateralRefId,
                    TypeName = x.TypeName,
                    SubCategoryRefId = x.SubCategoryRefId,
                    CategoryRefId = x.CategoryRefId,
                    CollateralNo = x.CollateralNo,
                    MarketPrice = x.MarketPrice,
                    PawnPrice = x.PawnPrice,
                    HotPrice = x.HotPrice,
                    Size = x.Size,
                    Description = x.Description,
                    OwnerName = x.OwnerName,
                    WareHouse = x.WareHouse,
                }).ToList();

        }

        public static List<VCollateralLines> ToProperties(this VCustomerCollaterals cust)
        {

            if (!cust.CollateralLines.Any())
            {
                return Enumerable.Empty<VCollateralLines>().ToList();
            }

            return cust.CollateralLines
                .Where(x => x.CategoryRefId == (int)CollateralTypeEnum.Property)
                .Select(x => new VCollateralLines
                {
                    Id = x.Id,
                    CollateralRefId = x.CollateralRefId,
                    TypeName = x.TypeName,
                    SubCategoryRefId = x.SubCategoryRefId,
                    CategoryRefId = x.CategoryRefId,
                    CollateralNo = x.CollateralNo,
                    MarketPrice = x.MarketPrice,
                    PawnPrice = x.PawnPrice,
                    HotPrice = x.HotPrice,
                    Size = x.Size,
                    Description = x.Description,
                    OwnerName = x.OwnerName,
                    WareHouse = x.WareHouse,
                }).ToList();

        }


        public static List<VCollateralLines> ToPersonals(this List<VCollateralLines> c)
        {
            if (!c.Any())
            {
                return Enumerable.Empty<VCollateralLines>().ToList();
            }

            return c.Where(x => x.CategoryRefId == (int)CollateralTypeEnum.Personal)
                    .ToList();
        }

        public static List<VCollateralLines> ToProperties(this List<VCollateralLines> c)
        {
            if (!c.Any())
            {
                return Enumerable.Empty<VCollateralLines>().ToList();
            }

            return c.Where(x => x.CategoryRefId == (int)CollateralTypeEnum.Property)
                    .ToList();
        }

    }
}
