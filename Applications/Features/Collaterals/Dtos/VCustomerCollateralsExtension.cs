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

}