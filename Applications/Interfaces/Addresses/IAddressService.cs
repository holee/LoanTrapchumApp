using Applications.Features.Addresses.Responses;
using Applications.Responses;
using Domains.Models.Addresses;

namespace Applications.Interfaces.Addresses
{
    public interface IAddressService
    {
        Task<IEnumerable<ProvinceDto>> GetsProvinces(string? filter=null); 
        Task<Province?> GetsProvinceById(int? id);
        Task<IEnumerable<DistrictDto>> GetDistrictsByProvinceId(int? provinceId);
        Task<IEnumerable<CommuneDto>> GetCommunesByDistrictId(int? districtId);
        Task<IEnumerable<VillageDto>> GetVillagesByCommuneId(int? communeId);
        Task<IEnumerable<Country>> GetCountries(string? filter = null);
        Task<Result<IEnumerable<Country>>> GetCountries(int? countryId = 105);
        Task<IEnumerable<string>> GetCities(string? filter = null); 
        Task<Result> AddCity(City city);
        Task<Result> UpdateCity(City city);

        Task<IEnumerable<T>> GetsProvinces<T>(string? expression)
            where T : class;

        Task<IEnumerable<T>> GetDistrictsByProvinceId<T>(int? provinceId)
            where T : class;

        Task<IEnumerable<T>> GetCommunesByDistrictId<T>(int? districtId)
            where T : class;

        Task<IEnumerable<T>> GetVillagesByCommuneId<T>(int? communeId)
            where T : class;

        Task<T?> GetsProvinceById<T>(int? id) where T : class;
        Task<IEnumerable<ProvinceDto>> GetsProvinces(int? provinceId, string? expression);
    }
}
