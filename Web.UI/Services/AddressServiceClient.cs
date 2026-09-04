using Applications.Features.Addresses.Dto;
using Applications.Features.Addresses.Interfaces;
using Domains.Models.Addresses;
using ErrorOr;

namespace Web.UI.Services;

public sealed class AddressServiceClient(HttpClient httpClient) : IAddressServiceImpl
{
    public Task<ErrorOr<string>> AddCity(CityDtoCreate city) => Task.FromResult<ErrorOr<string>>(Error.Unexpected());
    public Task<IEnumerable<string>> GetCities(string? filter = null) => Task.FromResult<IEnumerable<string>>([]);
    public Task<IEnumerable<CommuneDto>> GetCommunesByDistrictId(int? districtId) => Task.FromResult<IEnumerable<CommuneDto>>([]);
    public Task<ErrorOr<IEnumerable<Country>>> GetCountries(int? countryId = 105) => Task.FromResult<ErrorOr<IEnumerable<Country>>>(new List<Country>());
    public Task<IEnumerable<Country>> GetCountries(string? filter = null) => Task.FromResult<IEnumerable<Country>>([]);
    public Task<IEnumerable<DistrictDto>> GetDistrictsByProvinceId(int? provinceId) => Task.FromResult<IEnumerable<DistrictDto>>([]);
    public Task<Province?> GetsProvinceById(int? id) => Task.FromResult<Province?>(null);
    public Task<IEnumerable<ProvinceDto>> GetsProvinces(string? filter = null) => Task.FromResult<IEnumerable<ProvinceDto>>([]);
    public Task<IEnumerable<VillageDto>> GetVillagesByCommuneId(int? communeId) => Task.FromResult<IEnumerable<VillageDto>>([]);
    public Task<ErrorOr<string>> UpdateCity(CityDtoUpdate city) => Task.FromResult<ErrorOr<string>>(Error.Unexpected());
    public Task<IEnumerable<T>> GetsProvinces<T>(string? expression) where T : class => Task.FromResult<IEnumerable<T>>([]);
    public Task<IEnumerable<T>> GetDistrictsByProvinceId<T>(int? provinceId) where T : class => Task.FromResult<IEnumerable<T>>([]);
    public Task<IEnumerable<T>> GetCommunesByDistrictId<T>(int? districtId) where T : class => Task.FromResult<IEnumerable<T>>([]);
    public Task<IEnumerable<T>> GetVillagesByCommuneId<T>(int? communeId) where T : class => Task.FromResult<IEnumerable<T>>([]);
    public Task<T?> GetsProvinceById<T>(int? id) where T : class => Task.FromResult<T?>(null);
}
