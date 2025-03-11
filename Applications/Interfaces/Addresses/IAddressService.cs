using Applications.Responses;
using Domains.Models.Addresses;

namespace Applications.Interfaces.Addresses
{
    public interface IAddressService
    {
        Task<IEnumerable<Province>> GetsProvinces(string? filter); 
        Task<Province?> GetsProvinceById(int id);
        Task<IEnumerable<District>> GetDistrictsByProvince(int provinceId);
        Task<IEnumerable<Commune>> GetCommunesByDistrict(int districtId);
        Task<IEnumerable<Village>> GetVillagesByCommune(int communeId);
        Task<IEnumerable<Country>> GetCountries(string? filter = null);
        Task<IEnumerable<Country>> GetCountries(int countryId = 105);
        Task<IEnumerable<string>> GetCities(string? filter = null); 
        Task<Result> AddCity(City city);
        Task<Result> UpdateCity(City city);
    }
}
