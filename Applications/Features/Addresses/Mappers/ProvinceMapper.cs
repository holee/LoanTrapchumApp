using Applications.Features.Addresses.Responses;
using Domains.Models.Addresses;

namespace Applications.Commons.Mappers
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
