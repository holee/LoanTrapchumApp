using Applications.Features.Collaterals.Dtos;
using Domains.Models.Collaterals;

namespace Applications.Features.Collaterals.Mapper;

public static class CollateralSubTypeMapper
{
    public static CollateralSubType MapCollateralSubTypeCreate(this CollateralSubTypeDtoCreate dto)
    {
        return new CollateralSubType
        {
            CollateralTypeRefId = dto.CollateralTypeRefId,
            CollateralSubTypeName = dto.SubTypeName,
            LoanSizeAmuntMin = dto.LoanSizeAmuntMin,
            LoanSizeAmuntMax = dto.LoanSizeAmuntMax,
            IsActive = dto.IsActive ?? true,
            ValueRate = dto.ValueRate,
            CollateralSubTypeNameInKhmer = dto.SubTypeNameInKhmer,
        };
    }
    public static CollateralSubType MapCollateralSubTypeEdit(this CollateralSubTypeDtoCreate dto)
    {
        return new CollateralSubType
        {
            Id = dto.Id,
            CollateralTypeRefId = dto.CollateralTypeRefId,
            CollateralSubTypeName = dto.SubTypeName,
            LoanSizeAmuntMin = dto.LoanSizeAmuntMin,
            LoanSizeAmuntMax = dto.LoanSizeAmuntMax,
            IsActive = dto.IsActive,
            ValueRate = dto.ValueRate,
            CollateralSubTypeNameInKhmer = dto.SubTypeNameInKhmer,
        };
    }


}