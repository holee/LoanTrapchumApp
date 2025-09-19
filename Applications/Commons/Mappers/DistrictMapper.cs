using Domains.Models.Addresses;
using Applications.Features.Addresses.Responses;


namespace Applications.Commons.Mappers
{
    public static class DistrictMapper
    {

        public static DistrictDto MapFrom(this District source)
        {
            return new DistrictDto
            {
                DisDescription = source.DisDescription,
                ProvinceRefId = source.ProvinceRefId,
                DisInKhmer = source.DisDescription,
            };
        }

        public static District MapCreate(this DistrictDtoCreate source) 
        {
            return new District
            {
                DistrictId = source.DistrictId,
                DisDescription = source.DisDescription,
                ProvinceRefId = source.ProvinceRefId,
                DisInKhmer = source.DisDescription,
            };
        }

        public static District MapEdit(this DistrictDtoUpdate source)
        {
            return new District
            {
                DistrictId = source.DistrictId,
                DisDescription = source.DisDescription,
                ProvinceRefId = source.ProvinceRefId,
                DisInKhmer = source.DisDescription,
            };
        }
    }
}
