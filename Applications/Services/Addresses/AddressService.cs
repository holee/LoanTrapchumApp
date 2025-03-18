using Applications.Interfaces.Addresses;
using Applications.Requests.Lists.Addreses;
using Applications.Responses;
using Domains.Models.Addresses;

namespace Applications.Services.Addresses
{
    public sealed class AddressService : IAddressService
    {
        private readonly IAddressService _address;
        public AddressService(IAddressService addressService)
        {
            _address = addressService;
        }

        public async Task<Result> AddCity(City city)
        {
            return await _address.AddCity(city);
        }

        public async Task<IEnumerable<string>> GetCities(string? filter = null)
        {
            return await _address.GetCities(filter);
        }

        public async Task<IEnumerable<CommuneDto>> GetCommunesByDistrictId(int? districtId) 
        {
            return await _address.GetCommunesByDistrictId(districtId);
        }

        public async Task<IEnumerable<Country>> GetCountries(string? filter = null)
        {
            return await _address.GetCountries(filter);
        }

        public async Task<IEnumerable<Country>> GetCountries(int? countryId = 105)
        {
            return await _address.GetCountries(countryId);
        }

        public async Task<IEnumerable<DistrictDto>> GetDistrictsByProvinceId(int? provinceId)
        {
            return await _address.GetDistrictsByProvinceId(provinceId); 
        }

        public async Task<Province?> GetsProvinceById(int? id)
        {
            return await _address.GetsProvinceById(id);
        }

        public async Task<IEnumerable<ProvinceDto>> GetsProvinces(string? filter)
        {
            return await _address.GetsProvinces(filter);
        }

        public async Task<IEnumerable<VillageDto>> GetVillagesByCommuneId(int? communeId)
        {
            return await _address.GetVillagesByCommuneId(communeId); 
        }

        public async Task<Result> UpdateCity(City city)
        {
            return await _address.UpdateCity(city);
        }
    }
}
