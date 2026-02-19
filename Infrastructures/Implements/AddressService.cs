using Applications.Commons;
using Applications.Extensions;
using Applications.Features.Addresses.Dto;
using Applications.Interfaces.Addresses;
using Applications.Responses;
using Applications.Responses.Lists.Addreses;
using Applications.Responses.Paginations;
using Domains.Models.Addresses;
using Infrastructures.DataAccess;
using Infrastructures.Implements.Extensions;
using Mapster;
using Mapster.Utils;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructures.Implements;

public class AddressService : IAddressService
{
    private readonly DataContext _context;
    private readonly IDbContextFactory<DataContext> _dbContextFactory;
    public AddressService(DataContext context, IDbContextFactory<DataContext> dbContextFactory)
    {
        _context = context;
        _dbContextFactory = dbContextFactory;
    }

    public async Task<IEnumerable<ProvinceDto>> GetsProvinces(string? expression)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var items = await context.Provinces
                                        .WhereIf(expression.IsNotNullOrEmpty(), x => EF.Functions.Like(x.ProInKhmer!, $"%{expression}%") ||
                                                                                   EF.Functions.Like(x.ProDescription!, $"%{expression}%"))
                                        .ProjectToType<ProvinceDto>()
                                        .AsNoTracking()
                                        .ToListAsync();

        return items;
    }


    public async Task<IEnumerable<ProvinceDto>> GetsProvinces(int? provinceId, string? expression)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var items = await context.Provinces
                                        .WhereIf(expression.IsNotNullOrEmpty(), x => EF.Functions.Like(x.ProInKhmer!, $"%{expression}%") ||
                                                                                   EF.Functions.Like(x.ProDescription!, $"%{expression}%"))
                                        .WhereIf(provinceId > 0, x => x.ProvinceId == provinceId)
                                        .ProjectToType<ProvinceDto>()
                                        .AsNoTracking()
                                        .ToListAsync();

        return items;
    }

    public async Task<IEnumerable<T>> GetsProvinces<T>(string? expression)
        where T : class
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        IQueryable<Province> query = context.Provinces;

        if (!string.IsNullOrEmpty(expression))
        {
            query = query.Where(x =>
                x.ProInKhmer != null && x.ProInKhmer.Contains(expression) ||
                x.ProDescription != null && x.ProDescription.Contains(expression));
        }


        var items = await query
            .ProjectToType<T>()
            .AsNoTracking()
            .ToListAsync();

        return items;
    }

    public async Task<IEnumerable<T>> GetDistrictsByProvinceId<T>(int? provinceId)
        where T : class
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();
        var queries = await context.Districts
            .Where(x => x.ProvinceRefId == provinceId)
            .ProjectToType<T>()
            .AsNoTracking()
            .ToListAsync();
        return queries;
    }

    public async Task<IEnumerable<T>> GetCommunesByDistrictId<T>(int? districtId)
        where T : class
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();
        var queries = await context.Communes
            .Where(x => x.DistrictRefId == districtId)
            .ProjectToType<T>()
            .AsNoTracking()
            .ToListAsync();
        return queries;
    }

    public async Task<IEnumerable<T>> GetVillagesByCommuneId<T>(int? communeId)
        where T : class
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var queries = await context.Villages
            .Where(x => x.CommuneRefId == communeId)
            .ProjectToType<T>()
            .AsNoTracking()
            .ToListAsync();
        return queries;
    }

    public async Task<T?> GetsProvinceById<T>(int? id)
        where T : class
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var province = await context.Provinces.AsNoTracking().FirstOrDefaultAsync(x => x.ProvinceId == id);
        return province.Adapt<T?>();
    }


    public async Task<Province?> GetsProvinceById(int? id)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        IQueryable<Province> query = context.Provinces;

        return await query.AsNoTracking().FirstOrDefaultAsync(x => x.ProvinceId == id);
    }

    public async Task<IEnumerable<DistrictDto>> GetDistrictsByProvinceId(int? provinceId)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();
        var queries = await context.Districts
            .Where(x => x.ProvinceRefId == provinceId)
            .ProjectToType<DistrictDto>()
            .AsNoTracking()
            .ToListAsync();
        return queries;
    }

    public async Task<PageMetaData<CountryDto>> GetCountries(int? countryId, QueryParam param, CancellationToken cancellation = default)
    {

        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellation);

        var queries = context.Countries.WhereIf(countryId.HasValue && countryId > 0, x => x.CountryId == countryId);

        queries = ApplyFilter(param, queries);

        var page = await queries.AsNoTracking()
                                .ProjectToType<CountryDto>()
                                .ToPagedAsync(param.Page, param.PageSize, cancellation);
        return page;
    }

    public async Task<PageMetaData<ProvinceDto>> GetProvinces(int? provinceId, QueryParam param, CancellationToken cancellation = default)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellation);
        var queries = context.Provinces
                                    .WhereIf(provinceId.HasValue && provinceId > 0, x => x.ProvinceId == provinceId);
        queries = ApplyFilter(queries, param);
        queries = ApplySort(param, queries);
        var pages = await queries
                .ProjectToType<ProvinceDto>()
                .AsNoTracking()
                .ToPagedAsync(param.Page, param.PageSize, cancellation);
        return pages;

    }

    public async Task<PageMetaData<DistrictDto>> GetDistrictsByProvinceId(int? provinceId, int? districtId, QueryParam param,
            CancellationToken cancellation = default)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellation);

        var queries = context.Districts.Where(x => x.ProvinceRefId == provinceId);

        queries = ApplyFilter(districtId, param, ref queries);

        var page = queries.ProjectToType<DistrictDto>()
                          .AsNoTracking()
                          .ToPagedAsync(param.Page, param.PageSize, cancellation);
        return await page;
    }

    public async Task<PageMetaData<CommuneDto>> GetCommunesByDistrictId(int? districtId, int? communeId, QueryParam param,
            CancellationToken cancellation = default)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellation);

        var queries = context.Communes.Where(x => x.DistrictRefId == districtId)
                                      .WhereIf(communeId.HasValue && communeId > 0, x => x.CommuneId == communeId);

        queries = ApplyFilter(param, queries);

        var page = await queries
                        .ProjectToType<CommuneDto>()
                        .AsNoTracking()
                        .ToPagedAsync(param.Page, param.PageSize, cancellation);

        return page;
    }

    public async Task<PageMetaData<VillageDto>> GetVillagesByCommuneId(int? communeId, int? villageId, QueryParam param,
    CancellationToken cancellation = default)
    {

        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellation);

        var queries = context.Villages
                            .Where(x => x.CommuneRefId == communeId)
                            .WhereIf(villageId.HasValue && villageId > 0, x => x.VillageId == villageId);
        queries = ApplyFilter(param, queries);

        var page = await queries
                           .ProjectToType<VillageDto>()
                           .AsNoTracking()
                           .ToPagedAsync(param.Page, param.PageSize, cancellation);
        return page;
    }



    public async Task<PageMetaData<CityDto>> GetCities(int? cityId, QueryParam param, CancellationToken cancellation = default)
    {

        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellation);

        var queries = context.Cities.WhereIf(cityId.HasValue && cityId > 0, x => x.Id == cityId);

        queries = ApplyFilter(param, queries);

        var page = await queries
                                .ProjectToType<CityDto>()
                                .AsNoTracking()
                                .ToPagedAsync(param.Page, param.PageSize, cancellation);

        return page;
    }


    public async Task<IEnumerable<CommuneDto>> GetCommunesByDistrictId(int? districtId)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var queries = context.Communes.Where(x => x.DistrictRefId == districtId);

        var filter = await queries
                        .ProjectToType<CommuneDto>()
                        .AsNoTracking()
                        .ToListAsync();
        return filter;
    }
    public async Task<IEnumerable<VillageDto>> GetVillagesByCommuneId(int? communeId)
    {

        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var queries = await context.Villages
            .Where(x => x.CommuneRefId == communeId)
            .ProjectToType<VillageDto>()
            .AsNoTracking()
            .ToListAsync();
        return queries;
    }

    public async Task<IEnumerable<Country>> GetCountries(string? searchText = null)
    {

        await using var context = await _dbContextFactory.CreateDbContextAsync();

        IQueryable<Country> query = context.Countries;

        if (!string.IsNullOrEmpty(searchText))
        {
            query = query.Where(x =>
                x.DesInKhmer != null &&
                x.DesInKhmer.StartsWith(searchText, StringComparison.InvariantCultureIgnoreCase) ||
                x.CountryName != null &&
                x.CountryName.StartsWith(searchText, StringComparison.InvariantCultureIgnoreCase));
        }

        return await query.AsNoTracking().ToListAsync();
    }


    public async Task<IEnumerable<string>> GetCities(string? searchText = null)
    {

        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var cities = await context.Cities
                                        .Where(x => !x.CityName.Equals(""))
                                        .WhereIf(!string.IsNullOrEmpty(searchText), x =>
                                                EF.Functions.Like(x.CityName, $"%{searchText}%"))
                                        .AsNoTracking()
                                        .ToListAsync();

        if (cities.Any())
        {
            return cities.Select(x => x.CityName).ToList();
        }

        return Enumerable.Empty<string>();
    }

    public async Task<Result> AddCity(City city)
    {
        try
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();

            var c = context.Cities.Where(x => x.CityName == city.CityName);

            if (c.Any())
            {
                return Result.Failure(ErrorResponse.Conflict("City Already Exist."));
            }

            context.Cities.Add(city);

            await context.SaveChangesAsync();

            return Result.Success("New City was Added.");
        }
        catch (Exception ex)
        {
            return Result.Failure(ErrorResponse.ServerError($"{ex.Message}"));
        }
    }


    public async Task<ErrorOr.ErrorOr<CityDto>> AddCity(City city, CancellationToken cancellation = default)
    {
        try
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync(cancellation);

            var c = await context.Cities.AnyAsync(x => x.CityName == city.CityName, cancellation);

            if (c)
            {
                return ErrorOr.Error.Conflict("City Already Exist.");
            }

            context.Cities.Add(city);

            await context.SaveChangesAsync(cancellation);

            return new CityDto { Id = city.Id, CityName = city.CityName };
        }
        catch (Exception ex)
        {
            return ErrorOr.Error.Unexpected(ex.Message);
        }
    }


    public async Task<Result> UpdateCity(City city)
    {
        try
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();

            var c = context.Cities.Where(x => x.CityName == city.CityName && x.Id != city.Id);

            if (c.Any())
            {
                return Result.Failure(ErrorResponse.Conflict("City Already Exist."));
            }

            context.Cities.Update(city);

            await context.SaveChangesAsync();

            return Result.Success("City was Updated.");
        }
        catch (Exception ex)
        {
            return Result.Failure(ErrorResponse.ServerError($"{ex.Message}"));
        }
    }


    public async Task<ErrorOr.ErrorOr<CityDto>> UpdateCity(City city, CancellationToken cancellation = default)
    {
        try
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync(cancellation);

            var c = await context.Cities.AnyAsync(x => x.CityName == city.CityName && x.Id != city.Id, cancellation);

            if (c)
            {
                return ErrorOr.Error.Conflict("City Already Exist.");
            }

            context.Cities.Update(city);

            await context.SaveChangesAsync(cancellation);

            return new CityDto { Id = city.Id, CityName = city.CityName };
        }
        catch (Exception ex)
        {
            return ErrorOr.Error.Unexpected(ex.Message);
        }
    }


    public async Task<Result<IEnumerable<Country>>> GetCountries(int? countryId)
    {

        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var countries = await context.Countries
                                            .WhereIf(countryId > 0, x => x.CountryId == countryId)
                                            .AsNoTracking()
                                            .ToListAsync();
        return Result.Success<IEnumerable<Country>>(countries);
    }




    private static IQueryable<City> ApplyFilter(QueryParam param, IQueryable<City> queries)
    {
        var filter = param.Filter?.Trim();

        if (string.IsNullOrEmpty(filter))
        {
            return queries;
        }


        if (string.IsNullOrEmpty(param.FilterColumn))
        {
            queries = queries.WhereMatchesFilter(filter, AllowedCitiesFilter.ToArray());
        }
        else if (AllowedCitiesFilter.Contains(param.FilterColumn))
        {
            queries = queries.WhereMatchesFilter(filter, param.FilterColumn.Trim());
        }

        return queries;
    }

    private static readonly HashSet<string> AllowedProvincesFilter = new(StringComparer.OrdinalIgnoreCase)
    {
        nameof(ProvinceDto.ProvinceCode),
        nameof(ProvinceDto.ProInKhmer),
        nameof(ProvinceDto.ProDescription)
    };

    private static readonly HashSet<string> AllowedDistrcitsFilter = new(StringComparer.OrdinalIgnoreCase)
    {
        nameof(DistrictDto.DisInKhmer),
        nameof(DistrictDto.DisDescription),
        nameof(DistrictDto.DistrictId)
    };

    private static readonly HashSet<string> AllowedCommunesFilter = new(StringComparer.OrdinalIgnoreCase)
    {
        nameof(CommuneDto.CommuneId),
        nameof(CommuneDto.ComInKhmer),
        nameof(CommuneDto.ComDescription)
    };

    private static readonly HashSet<string> AllowedVillagesFilter = new(StringComparer.OrdinalIgnoreCase)
    {
        nameof(VillageDto.VilInKhmer),
        nameof(VillageDto.VillDescription)
    };

    private static readonly HashSet<string> AllowedCountriesFilter = new(StringComparer.OrdinalIgnoreCase)
    {
        nameof(CountryDto.CountryId),
        nameof(CountryDto.CountryName),
        nameof(CountryDto.CountryCode),
        nameof(CountryDto.DesInKhmer)
    };

    private static readonly HashSet<string> AllowedCitiesFilter = new(StringComparer.OrdinalIgnoreCase)
    {
        nameof(CityDto.CityName),
        nameof(CityDto.Id)
    };


    private static IQueryable<Country> ApplyFilter(QueryParam param, IQueryable<Country> queries)
    {
        var filter = param.Filter?.Trim();

        if (string.IsNullOrEmpty(param.FilterColumn))
        {
            queries = queries.WhereMatchesFilter(filter, AllowedCountriesFilter.ToArray());
        }
        else if (AllowedCountriesFilter.Contains(param.FilterColumn))
        {
            queries = queries.WhereMatchesFilter(filter, param.FilterColumn.Trim());
        }

        return queries;
    }

    private static IQueryable<Village> ApplyFilter(QueryParam param, IQueryable<Village> queries)
    {
        var filter = param.Filter?.Trim();

        if (string.IsNullOrEmpty(param.FilterColumn))
        {
            queries = queries.WhereMatchesFilter(filter, AllowedVillagesFilter.ToArray());
        }
        else if (AllowedVillagesFilter.Contains(param.FilterColumn))
        {
            queries = queries.WhereMatchesFilter(filter, param.FilterColumn.Trim());
        }

        return queries;
    }

    private static IQueryable<Commune> ApplyFilter(QueryParam param, IQueryable<Commune> queries)
    {
        var filter = param.Filter?.Trim();

        if (string.IsNullOrEmpty(filter))
        {
            return queries;
        }

        if (string.IsNullOrEmpty(param.FilterColumn))
        {
            queries = queries.WhereMatchesFilter(filter, AllowedCommunesFilter.ToArray());
        }
        else if (AllowedCommunesFilter.Contains(param.FilterColumn))
        {
            queries = queries.WhereMatchesFilter(filter, param.FilterColumn.Trim());
        }

        return queries;
    }

    private static IQueryable<District> ApplyFilter(int? districtId, QueryParam param, ref IQueryable<District> queries)
    {
        var filter = param.Filter?.Trim();

        if (!string.IsNullOrWhiteSpace(filter)) return queries;

        if (districtId.HasValue && districtId > 0)

            queries = queries.Where(x => x.DistrictId == districtId);

        if (!string.IsNullOrEmpty(param.FilterColumn) &&
            AllowedDistrcitsFilter.Contains(param.FilterColumn))
        {
            queries = queries.WhereMatchesFilter(filter, param.FilterColumn.Trim());
        }

        queries = queries.WhereMatchesFilter(filter, AllowedDistrcitsFilter.ToArray());

        return queries;
    }



    private static IQueryable<Province> ApplySort(QueryParam param, IQueryable<Province> queries)
    {
        if (string.IsNullOrEmpty(param.SortColumn))
        {
            var sortDescending = param.SortDescending;

            queries = sortDescending ?
                    queries.OrderByDescending(x => param.SortColumn) :
                    queries.OrderBy(x => param.SortColumn);

        }

        return queries;
    }


    private static IQueryable<Province> ApplyFilter(IQueryable<Province> queries, QueryParam param)
    {
        var filter = param.Filter?.Trim();

        if (string.IsNullOrEmpty(filter))
        {
            return queries;
        }

        if (string.IsNullOrEmpty(param.FilterColumn))
        {
            queries = queries.WhereMatchesFilter(filter, AllowedProvincesFilter.ToArray());
        }
        else if (AllowedProvincesFilter.Contains(param.FilterColumn))
        {
            queries = queries.WhereMatchesFilter(filter, param.FilterColumn.Trim());

        }

        return queries;
    }

}