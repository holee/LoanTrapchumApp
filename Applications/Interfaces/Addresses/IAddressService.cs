using Applications.Requests.Lists.Addreses;
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
        Task<IEnumerable<Country>> GetCountries(int? countryId = 105);
        Task<IEnumerable<string>> GetCities(string? filter = null); 
        Task<Result> AddCity(City city);
        Task<Result> UpdateCity(City city);
    }
}
