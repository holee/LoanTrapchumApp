using Applications.Commons;
using Applications.Features.Addresses.Dto;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Addresses;
using Domains.Models.Addresses;
using ErrorOr;

namespace Applications.Interfaces.Addresses;

public interface IAddressService
{
    // Province CRUD
    Task<ErrorOr<Province>> GetProvinceByIdAsync(int? proId, CancellationToken cancellation = default);
    Task<Pager<ProvinceViewModel>> GetAllProvincesAsync(QueryParam param, CancellationToken cancellation = default);
    Task<ErrorOr<string>> CreateProvinceAsync(Province province, CancellationToken cancellation = default);
    Task<ErrorOr<string>> UpdateProvinceAsync(Province province, CancellationToken cancellation = default);
    Task<ErrorOr<string>> DeleteProvinceAsync(int? proId, CancellationToken cancellation = default);

    // District CRUD
    Task<ErrorOr<District>> GetDistrictByIdAsync(int? id);
    Task<ErrorOr<IEnumerable<District>>> GetAllDistrictsAsync(int? proId = 0, int take = 0, string? condition = null);
    Task<Pager<VDistricts>> GetAllDistrictsPagerAsync(QueryParam param);
    Task<ErrorOr<string>> CreateDistrictAsync(District model);
    Task<ErrorOr<string>> UpdateDistrictAsync(District model);
    Task<ErrorOr<string>> DeleteDistrictAsync(int? id);

    // Commune CRUD
    Task<ErrorOr<Commune>> GetCommuneByIdAsync(int? id);
    Task<ErrorOr<IEnumerable<Commune>>> GetAllCommunesAsync(int? proId = 0, int take = 0, string? condition = null);
    Task<Pager<VCommunes>> GetAllCommunesPagerAsync(QueryParam param);
    Task<ErrorOr<string>> CreateCommuneAsync(CommuneDtoCreate model);
    Task<ErrorOr<string>> UpdateCommuneAsync(CommuneDtoUpdate model);
    Task<ErrorOr<string>> DeleteCommuneAsync(int? id);

    // Village CRUD
    Task<ErrorOr<Village>> GetVillageByIdAsync(int? id);
    Task<ErrorOr<VVillageDetails>> GetVillageDetailsAsync(int? id);
    Task<ErrorOr<IEnumerable<Village>>> GetAllVillagesAsync(int? proId = 0, int take = 0, string? condition = null);
    Task<Pager<VVillages>> GetAllVillagesPagerAsync(QueryParam param);
    Task<ErrorOr<string>> CreateVillageAsync(VillageDtoCreate model);
    Task<ErrorOr<string>> UpdateVillageAsync(VillageDtoUpdate model);
    Task<ErrorOr<string>> DeleteVillageAsync(int? id);

    Task<IEnumerable<ProvinceDto>> GetsProvinces(string? filter=null); 
    Task<Province?> GetsProvinceById(int? id);
    Task<IEnumerable<DistrictDto>> GetDistrictsByProvinceId(int? provinceId);
    Task<IEnumerable<CommuneDto>> GetCommunesByDistrictId(int? districtId);
    Task<IEnumerable<VillageDto>> GetVillagesByCommuneId(int? communeId);
    Task<IEnumerable<Country>> GetCountries(string? filter = null);
    Task<ErrorOr<IEnumerable<Country>>> GetCountries(int? countryId = 105);
    Task<IEnumerable<string>> GetCities(string? filter = null); 
    Task<ErrorOr<string>> AddCity(City city);
    Task<ErrorOr<string>> UpdateCity(City city);

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
    Task<PageMetaData<ProvinceDto>> GetProvinces(int? provinceId, QueryParam param, CancellationToken cancellation = default);
    Task<PageMetaData<DistrictDto>> GetDistrictsByProvinceId(int? provinceId, int? districtId, QueryParam param, CancellationToken cancellation = default);
    Task<PageMetaData<CommuneDto>> GetCommunesByDistrictId(int? districtId, int? communeId, QueryParam param, CancellationToken cancellation = default);
    Task<PageMetaData<VillageDto>> GetVillagesByCommuneId(int? communeId, int? villageId, QueryParam param, CancellationToken cancellation = default);
    Task<ErrorOr<CityDto>> AddCity(City city, CancellationToken cancellation = default);
    Task<ErrorOr<CityDto>> UpdateCity(City city, CancellationToken cancellation = default);
    Task<PageMetaData<CountryDto>> GetCountries(int? countryId, QueryParam param, CancellationToken cancellation = default);
    Task<PageMetaData<CityDto>> GetCities(int? cityId, QueryParam param, CancellationToken cancellation = default);
}
