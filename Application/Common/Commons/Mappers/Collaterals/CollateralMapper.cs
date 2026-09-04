using Application.Features.Collaterals.Dtos;
using Domains.Models.Collaterals;

namespace Application.Commons.Mappers.Collaterals;

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
