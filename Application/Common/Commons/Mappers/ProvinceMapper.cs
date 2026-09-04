using Application.Features.Addresses.Dto;
using Domains.Models.Addresses;

namespace Application.Commons.Mappers
{
    public static class ProvinceMapper
    {
        public static Province MapTo(this ProvinceDtoCreate dto)
        {
            return new Province
            {
                ProDescription = dto.ProDescription,
                CountryId = dto.CountryId,
                ProInKhmer = dto.ProInKhmer,
                ProvinceCode = dto.ProvinceCode,
            };
        }
    }
}
