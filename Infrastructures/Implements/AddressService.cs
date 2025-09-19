using Applications.Extensions;
using Applications.Features.Addresses.Responses;
using Applications.Interfaces.Addresses;
using Applications.Responses;
using Domains.Models.Addresses;
using Infrastructures.DataAccess;
using Mapster;
using Mapster.Utils;
using Microsoft.EntityFrameworkCore;

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


    public async Task<IEnumerable<ProvinceDto>> GetsProvinces(int? provinceId,string? expression)
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

    public async Task<IEnumerable<CommuneDto>> GetCommunesByDistrictId(int? districtId)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var queries = await context.Communes
            .Where(x => x.DistrictRefId == districtId)
            .ProjectToType<CommuneDto>()
            .AsNoTracking()
            .ToListAsync();
        return queries;
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


    public async Task<Result<IEnumerable<Country>>> GetCountries(int? countryId)
    {

        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var countries = await context.Countries
                                            .WhereIf(countryId > 0, x => x.CountryId == countryId)
                                            .AsNoTracking()
                                            .ToListAsync();
        return Result.Success<IEnumerable<Country>>(countries);
    }
}