using Applications.Interfaces.Addresses;
using Applications.Requests.Lists.Addreses;
using Applications.Responses;
using Domains.Models.Addresses;
using Mapster;

namespace Applications.Services.Addresses
{
    public interface IAddressServiceImpl
    {
        Task<Result> AddCity(CityDtoCreate city);
        Task<IEnumerable<string>> GetCities(string? filter = null);
        Task<IEnumerable<CommuneDto>> GetCommunesByDistrictId(int? districtId);
        Task<IEnumerable<Country>> GetCountries(int? countryId = 105);
        Task<IEnumerable<Country>> GetCountries(string? filter = null);
        Task<IEnumerable<DistrictDto>> GetDistrictsByProvinceId(int? provinceId);
        Task<Province?> GetsProvinceById(int? id);
        Task<IEnumerable<ProvinceDto>> GetsProvinces(string? filter = null);
        Task<IEnumerable<VillageDto>> GetVillagesByCommuneId(int? communeId);
        Task<Result> UpdateCity(CityDtoUpdate city);

        Task<IEnumerable<T>> GetsProvinces<T>(string? expression)
            where T : class;

        Task<IEnumerable<T>> GetDistrictsByProvinceId<T>(int? provinceId)
            where T : class;

        Task<IEnumerable<T>> GetCommunesByDistrictId<T>(int? districtId)
            where T : class;

        Task<IEnumerable<T>> GetVillagesByCommuneId<T>(int? communeId)
            where T : class;

        Task<T?> GetsProvinceById<T>(int? id)
            where T : class;
    }


    public sealed class AddressService(IAddressService addressService) : IAddressServiceImpl
    {
        public async Task<Result> AddCity(CityDtoCreate city)
        {
            var req = city.Adapt<City>();

            return await addressService.AddCity(req);
        }

        public async Task<IEnumerable<string>> GetCities(string? filter = null)
        {
            return await addressService.GetCities(filter);
        }

        public async Task<IEnumerable<CommuneDto>> GetCommunesByDistrictId(int? districtId)
        {
            return await addressService.GetCommunesByDistrictId(districtId);
        }

        public async Task<IEnumerable<Country>> GetCountries(string? filter = null)
        {
            return await addressService.GetCountries(filter);
        }

        public async Task<IEnumerable<Country>> GetCountries(int? countryId = 105)
        {
            return await addressService.GetCountries(countryId);
        }

        public async Task<IEnumerable<DistrictDto>> GetDistrictsByProvinceId(int? provinceId)
        {
            return await addressService.GetDistrictsByProvinceId(provinceId);
        }

        public async Task<Province?> GetsProvinceById(int? id)
        {
            return await addressService.GetsProvinceById(id);
        }

        public async Task<IEnumerable<ProvinceDto>> GetsProvinces(string? filter)
        {
            return await addressService.GetsProvinces(filter);
        }

        public async Task<IEnumerable<VillageDto>> GetVillagesByCommuneId(int? communeId)
        {
            return await addressService.GetVillagesByCommuneId(communeId);
        }

        public async Task<Result> UpdateCity(CityDtoUpdate city)
        {
            var req = city.Adapt<City>();

            return await addressService.UpdateCity(req);
        }

        public async Task<IEnumerable<T>> GetsProvinces<T>(string? expression) where T : class
        {
            return await addressService.GetsProvinces<T>(expression);
        }

        public async Task<IEnumerable<T>> GetDistrictsByProvinceId<T>(int? provinceId) where T : class
        {
            return await addressService.GetDistrictsByProvinceId<T>(provinceId);
        }

        public async Task<IEnumerable<T>> GetCommunesByDistrictId<T>(int? districtId) where T : class
        {
            return await addressService.GetCommunesByDistrictId<T>(districtId);
        }

        public async Task<IEnumerable<T>> GetVillagesByCommuneId<T>(int? communeId) where T : class
        {
            return await addressService.GetVillagesByCommuneId<T>(communeId);
        }

        public async Task<T?> GetsProvinceById<T>(int? id) where T : class
        {
            return await addressService.GetsProvinceById<T>(id);
        }
    }
}