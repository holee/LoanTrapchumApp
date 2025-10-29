using Applications.Features.Collaterals.Dtos;

namespace Applications.Responses.Lists.Collaterals;

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

}