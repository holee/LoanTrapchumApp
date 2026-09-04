using Applications.Commons;
using Applications.Exceptions;
using Applications.Extensions;
using Applications.Features.Addresses.Dto;
using Applications.Interfaces.Addresses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Addresses;
using Domains.Models;
using Domains.Models.Addresses;
using ErrorOr;
using Infrastructures.DataAccess;
using Infrastructures.Repository.Extensions;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructures.Repository;

public sealed class AddressRepository : IAddressService
{
    private readonly DataContext _dbContextFactory;
    private readonly ILogger<AddressRepository> _logger;

    public AddressRepository(
        DataContext dbContextFactory,
        ILogger<AddressRepository> logger)
    {
        _dbContextFactory = dbContextFactory;
        _logger = logger;
    }

    public async Task<IEnumerable<ProvinceDto>> GetsProvinces(string? expression)
    {
        var context = _dbContextFactory;

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
        var context = _dbContextFactory;

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
        var context = _dbContextFactory;

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
        var context = _dbContextFactory;
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
        var context = _dbContextFactory;
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
        var context = _dbContextFactory;

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
        var context = _dbContextFactory;

        var province = await context.Provinces.AsNoTracking().FirstOrDefaultAsync(x => x.ProvinceId == id);
        return province.Adapt<T?>();
    }


    public async Task<Province?> GetsProvinceById(int? id)
    {
        var context = _dbContextFactory;

        IQueryable<Province> query = context.Provinces;

        return await query.AsNoTracking().FirstOrDefaultAsync(x => x.ProvinceId == id);
    }

    public async Task<IEnumerable<DistrictDto>> GetDistrictsByProvinceId(int? provinceId)
    {
        var context = _dbContextFactory;
        var queries = await context.Districts
            .Where(x => x.ProvinceRefId == provinceId)
            .ProjectToType<DistrictDto>()
            .AsNoTracking()
            .ToListAsync();
        return queries;
    }

    public async Task<PageMetaData<CountryDto>> GetCountries(int? countryId, QueryParam param, CancellationToken cancellation = default)
    {

        var context = _dbContextFactory;

        var queries = context.Countries.WhereIf(countryId.HasValue && countryId > 0, x => x.CountryId == countryId);

        queries = ApplyFilter(param, queries);

        var page = await queries.AsNoTracking()
                                .ProjectToType<CountryDto>()
                                .ToPagedAsync(param.Page, param.Take, cancellation);
        return page;
    }

    public async Task<PageMetaData<ProvinceDto>> GetProvinces(int? provinceId, QueryParam param, CancellationToken cancellation = default)
    {
        var context = _dbContextFactory;
        var queries = context.Provinces
                                    .WhereIf(provinceId.HasValue && provinceId > 0, x => x.ProvinceId == provinceId);
        queries = ApplyFilter(queries, param);
        queries = ApplySort(param, queries);
        var pages = await queries
                .ProjectToType<ProvinceDto>()
                .AsNoTracking()
                .ToPagedAsync(param.Page, param.Take, cancellation);
        return pages;

    }

    public async Task<PageMetaData<DistrictDto>> GetDistrictsByProvinceId(int? provinceId, int? districtId, QueryParam param,
            CancellationToken cancellation = default)
    {
        var context = _dbContextFactory;

        var queries = context.Districts.Where(x => x.ProvinceRefId == provinceId);

        queries = ApplyFilter(districtId, param, ref queries);

        var page = queries.ProjectToType<DistrictDto>()
                          .AsNoTracking()
                          .ToPagedAsync(param.Page, param.Take, cancellation);
        return await page;
    }

    public async Task<PageMetaData<CommuneDto>> GetCommunesByDistrictId(int? districtId, int? communeId, QueryParam param,
            CancellationToken cancellation = default)
    {
        var context = _dbContextFactory;

        var queries = context.Communes.Where(x => x.DistrictRefId == districtId)
                                      .WhereIf(communeId.HasValue && communeId > 0, x => x.CommuneId == communeId);

        queries = ApplyFilter(param, queries);

        var page = await queries
                        .ProjectToType<CommuneDto>()
                        .AsNoTracking()
                        .ToPagedAsync(param.Page, param.Take, cancellation);

        return page;
    }

    public async Task<PageMetaData<VillageDto>> GetVillagesByCommuneId(int? communeId, int? villageId, QueryParam param,
    CancellationToken cancellation = default)
    {

        var context = _dbContextFactory;

        var queries = context.Villages
                            .Where(x => x.CommuneRefId == communeId)
                            .WhereIf(villageId.HasValue && villageId > 0, x => x.VillageId == villageId);
        queries = ApplyFilter(param, queries);

        var page = await queries
                           .ProjectToType<VillageDto>()
                           .AsNoTracking()
                           .ToPagedAsync(param.Page, param.Take, cancellation);
        return page;
    }



    public async Task<PageMetaData<CityDto>> GetCities(int? cityId, QueryParam param, CancellationToken cancellation = default)
    {

        var context = _dbContextFactory;

        var queries = context.Cities.WhereIf(cityId.HasValue && cityId > 0, x => x.Id == cityId);

        queries = ApplyFilter(param, queries);

        var page = await queries
                                .ProjectToType<CityDto>()
                                .AsNoTracking()
                                .ToPagedAsync(param.Page, param.Take, cancellation);

        return page;
    }


    public async Task<IEnumerable<CommuneDto>> GetCommunesByDistrictId(int? districtId)
    {
        var context = _dbContextFactory;

        var queries = context.Communes.Where(x => x.DistrictRefId == districtId);

        var filter = await queries
                        .ProjectToType<CommuneDto>()
                        .AsNoTracking()
                        .ToListAsync();
        return filter;
    }
    public async Task<IEnumerable<VillageDto>> GetVillagesByCommuneId(int? communeId)
    {

        var context = _dbContextFactory;

        var queries = await context.Villages
            .Where(x => x.CommuneRefId == communeId)
            .ProjectToType<VillageDto>()
            .AsNoTracking()
            .ToListAsync();
        return queries;
    }

    public async Task<IEnumerable<Country>> GetCountries(string? searchText = null)
    {

        var context = _dbContextFactory;

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





    //public async Task<PageMetaData<CountryDto>> GetCountries(int? countryId, QueryParam query, CancellationToken cancellation = default)
    //{

    //    var context = _dbContextFactory;

    //    var queries = context.Countries.WhereIf(countryId.HasValue && countryId > 0, x => x.CountryId == countryId);

    //    queries = ApplyFilter(query, queries);


    //    if (!string.IsNullOrEmpty(searchText))
    //    {
    //        queries = queries.Where(x =>
    //            x.DesInKhmer != null &&
    //            x.DesInKhmer.StartsWith(searchText, StringComparison.InvariantCultureIgnoreCase) ||
    //            x.CountryName != null &&
    //            x.CountryName.StartsWith(searchText, StringComparison.InvariantCultureIgnoreCase));
    //    }

    //    return await queries.AsNoTracking().ToListAsync();
    //}


    public async Task<IEnumerable<string>> GetCities(string? searchText = null)
    {

        var context = _dbContextFactory;

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

    public async Task<ErrorOr<string>> AddCity(City city)
    {
        try
        {
            var context = _dbContextFactory;

            var c = context.Cities.Where(x => x.CityName == city.CityName);

            if (c.Any())
            {
                return Error.Conflict(description: "City Already Exist.");
            }

            context.Cities.Add(city);

            await context.SaveChangesAsync();

            return "New City was Added.";
        }
        catch (Exception ex)
        {
            return Error.Unexpected(description: ex.Message);
        }
    }


    public async Task<ErrorOr<CityDto>> AddCity(City city, CancellationToken cancellation = default)
    {
        try
        {
            var context = _dbContextFactory;

            var c = await context.Cities.AnyAsync(x => x.CityName == city.CityName, cancellation);

            if (c)
            {
                return Error.Conflict(description: "City Already Exist.");
            }

            context.Cities.Add(city);

            await context.SaveChangesAsync(cancellation);

            return new CityDto { Id = city.Id, CityName = city.CityName };
        }
        catch (Exception ex)
        {
            return Error.Unexpected(description: ex.Message);
        }
    }


    public async Task<ErrorOr<string>> UpdateCity(City city)
    {
        try
        {
            var context = _dbContextFactory;

            var c = context.Cities.Where(x => x.CityName == city.CityName && x.Id != city.Id);

            if (c.Any())
            {
                return Error.Conflict(description: "City Already Exist.");
            }

            context.Cities.Update(city);

            await context.SaveChangesAsync();

            return "City was Updated.";
        }
        catch (Exception ex)
        {
            return Error.Unexpected(description: ex.Message);
        }
    }


    public async Task<ErrorOr<CityDto>> UpdateCity(City city, CancellationToken cancellation = default)
    {
        try
        {
            var context = _dbContextFactory;

            var c = await context.Cities.AnyAsync(x => x.CityName == city.CityName && x.Id != city.Id, cancellation);

            if (c)
            {
                return Error.Conflict(description: "City Already Exist.");
            }

            context.Cities.Update(city);

            await context.SaveChangesAsync(cancellation);

            return new CityDto { Id = city.Id, CityName = city.CityName };
        }
        catch (Exception ex)
        {
            return Error.Unexpected(description: ex.Message);
        }
    }


    public async Task<ErrorOr<IEnumerable<Country>>> GetCountries(int? countryId)
    {

        var context = _dbContextFactory;

        var countries = await context.Countries
                                            .WhereIf(countryId > 0, x => x.CountryId == countryId)
                                            .AsNoTracking()
                                            .ToListAsync();
        return ErrorOrFactory.From<IEnumerable<Country>>(countries);
    }

    public async Task<ErrorOr<Province>> GetProvinceByIdAsync(int? proId, CancellationToken cancellation = default)
    {
        try
        {
            var context = _dbContextFactory;
            var pro = await context.Provinces.AsNoTracking().FirstOrDefaultAsync(x => x.ProvinceId == proId, cancellation);
            if (pro is null) return Error.NotFound(description: "No Province found.");
            return pro;
        }
        catch (Exception ex)
        {
            return Error.Unexpected(description: ex.Message);
        }
    }

    public async Task<Pager<ProvinceViewModel>> GetAllProvincesAsync(QueryParam param, CancellationToken cancellation = default)
    {
        var context = _dbContextFactory;
        IQueryable<Province> query = context.Provinces;

        if (param.IncludeInActive == true)
            query = query.IgnoreQueryFilters();

        if (!string.IsNullOrEmpty(param.Filter))
            query = query.Where(x => EF.Functions.Like(x.ProDescription!, $"%{param.Filter}%") ||
                                     EF.Functions.Like(x.ProInKhmer!, $"%{param.Filter}%"));

        if (param.SortDescending)
            query = query.OrderByDescending(x => EF.Property<object>(x, param.SortColumn!));

        var counts = await query.CountAsync(cancellation);
        var data = await query.Skip(param.Page).Take(param.Take)
                        .Select(x => new ProvinceViewModel
                        {
                            ProvinceId = x.ProvinceId,
                            ProvinceCode = x.ProvinceCode,
                            ProDescription = x.ProDescription,
                            ProInKhmer = x.ProInKhmer,
                        })
                        .AsNoTracking()
                        .ToListAsync(cancellation);

        return new Pager<ProvinceViewModel>(data, counts);
    }

    public async Task<ErrorOr<string>> CreateProvinceAsync(Province province, CancellationToken cancellation = default)
    {
        _logger.LogInformation("Creating new province with code: {ProvinceCode}", province.ProvinceCode);
        try
        {
            var context = _dbContextFactory;
            var exists = await context.Provinces.AnyAsync(x => x.ProvinceCode != null && x.ProvinceCode == province.ProvinceCode, cancellation);
            if (exists) return Error.Conflict(description: "Province code already exist.");

            await context.Provinces.AddAsync(province, cancellation);
            await context.SaveChangesAsync(cancellation);
            return "New province added.";
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error creating province with code: {ProvinceCode}", province.ProvinceCode);
            return Error.Unexpected(description: ex.InnerException!.Message);
        }
    }

    public async Task<ErrorOr<string>> UpdateProvinceAsync(Province province, CancellationToken cancellation = default)
    {
        try
        {
            var context = _dbContextFactory;
            var exists = await context.Provinces.AsNoTracking().AnyAsync(x => x.ProvinceId == province.ProvinceId, cancellation);
            if (!exists) return Error.Conflict(description: "Province doesn't exist!");

            context.Provinces.Update(province);
            await context.SaveChangesAsync(cancellation);
            return "Province was updated.";
        }
        catch (DbUpdateException ex)
        {
            return Error.Unexpected(description: ex.InnerException!.Message);
        }
    }

    public async Task<ErrorOr<string>> DeleteProvinceAsync(int? proId, CancellationToken cancellation = default)
    {
        _logger.LogInformation("Deleting province with ID: {ProvinceId}", proId);
        try
        {
            var context = _dbContextFactory;
            var pro = await context.Provinces.AsNoTracking().FirstOrDefaultAsync(x => x.ProvinceId == proId, cancellation);
            if (pro is null) return Error.NotFound(description: "No Province Found.");

            context.Provinces.Remove(pro);
            await context.SaveChangesAsync(cancellation);
            return "Province was deleted.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting province with ID: {ProvinceId}", proId);
            return Error.Unexpected(description: ex.Message);
        }
    }

    // ── District CRUD ────────────────────────────────────────────────────────

    public async Task<ErrorOr<District>> GetDistrictByIdAsync(int? id)
    {
        try
        {
            var context = _dbContextFactory;
            var result = await context.Districts.AsNoTracking().FirstOrDefaultAsync(x => x.DistrictId == id);
            if (result is null) return Error.NotFound(description: "No District found.");
            return result;
        }
        catch (Exception ex)
        {
            return Error.Unexpected(description: ex.Message);
        }
    }

    public async Task<ErrorOr<IEnumerable<District>>> GetAllDistrictsAsync(int? proId = 0, int take = 0, string? condition = null)
    {
        _logger.LogInformation("Retrieving district records.");
        var context = _dbContextFactory;
        IQueryable<District> query = context.Districts;

        if (!string.IsNullOrEmpty(condition))
            query = query.Where(x => EF.Functions.Like(x.DisInKhmer!, $"%{condition}%") ||
                                     EF.Functions.Like(x.DisDescription!, $"%{condition}%"));

        var items = await query.AsNoTracking().ToListAsync();
        return ErrorOrFactory.From<IEnumerable<District>>(items);
    }

    public async Task<Pager<VDistricts>> GetAllDistrictsPagerAsync(QueryParam param)
    {
        _logger.LogInformation("Retrieving district paged records.");
        var context = _dbContextFactory;
        IQueryable<VDistricts> query = context.VDistricts;

        if (!string.IsNullOrEmpty(param.Filter))
            query = query.Where(x => EF.Functions.Like(x.DisInKhmer!, $"%{param.Filter}%") ||
                                     EF.Functions.Like(x.DisDescription!, $"%{param.Filter}%") ||
                                     EF.Functions.Like(x.ProDescription!, $"%{param.Filter}%") ||
                                     EF.Functions.Like(x.ProInKhmer!, $"%{param.Filter}%"));

        if (param.SortDescending)
            query = query.OrderByDescending(x => EF.Property<object>(x, param.SortColumn!));

        var counts = await query.CountAsync();
        var data = await query.Skip(param.Page).Take(param.Take).AsNoTracking().ToListAsync();
        return new Pager<VDistricts>(data, counts);
    }

    public async Task<ErrorOr<string>> CreateDistrictAsync(District model)
    {
        try
        {
            _logger.LogInformation("Creating district record.");
            var context = _dbContextFactory;
            await context.Districts.AddAsync(model);
            await context.SaveChangesAsync();
            return "New District added.";
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error creating district.");
            return Error.Unexpected(description: ex.InnerException!.Message);
        }
    }

    public async Task<ErrorOr<string>> UpdateDistrictAsync(District model)
    {
        try
        {
            _logger.LogInformation("Updating district record.");
            var context = _dbContextFactory;
            var exists = await context.Districts.AsNoTracking().AnyAsync(x => x.DistrictId == model.DistrictId);
            if (!exists) return Error.Conflict(description: "District doesn't exist!");

            context.Districts.Update(model);
            await context.SaveChangesAsync();
            return "District was updated.";
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error updating district.");
            return Error.Unexpected(description: ex.InnerException!.Message);
        }
    }

    public async Task<ErrorOr<string>> DeleteDistrictAsync(int? id)
    {
        try
        {
            _logger.LogInformation("Deleting district record.");
            var context = _dbContextFactory;
            var result = await context.Districts.FirstOrDefaultAsync(x => x.DistrictId == id);
            if (result is null) return Error.NotFound(description: "No District Found.");

            context.Districts.Remove(result);
            await context.SaveChangesAsync();
            return "District was deleted.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting district.");
            return Error.Unexpected(description: ex.Message);
        }
    }

    // ── Commune CRUD ─────────────────────────────────────────────────────────

    public async Task<ErrorOr<Commune>> GetCommuneByIdAsync(int? id)
    {
        try
        {
            var context = _dbContextFactory;
            var result = await context.Communes.AsNoTracking().FirstOrDefaultAsync(x => x.CommuneId == id);
            if (result is null) return Error.NotFound(description: "No Commune found.");
            return result;
        }
        catch (Exception ex)
        {
            return Error.Unexpected(description: ex.Message);
        }
    }

    public async Task<ErrorOr<IEnumerable<Commune>>> GetAllCommunesAsync(int? proId = 0, int take = 0, string? condition = null)
    {
        _logger.LogInformation("Retrieving commune records.");
        var context = _dbContextFactory;
        IQueryable<Commune> query = context.Communes;

        if (!string.IsNullOrEmpty(condition))
            query = query.Where(x => EF.Functions.Like(x.ComInKhmer!, $"%{condition}%") ||
                                     EF.Functions.Like(x.ComDescription!, $"%{condition}%"));

        var items = await query.AsNoTracking().ToListAsync();
        return ErrorOrFactory.From<IEnumerable<Commune>>(items);
    }

    public async Task<Pager<VCommunes>> GetAllCommunesPagerAsync(QueryParam param)
    {
        _logger.LogInformation("Retrieving commune paged records.");
        var context = _dbContextFactory;
        IQueryable<VCommunes> query = context.VCommunes;

        if (!string.IsNullOrEmpty(param.Filter))
            query = query.Where(x => EF.Functions.Like(x.DisInKhmer!, $"%{param.Filter}%") ||
                                     EF.Functions.Like(x.DisDescription!, $"%{param.Filter}%") ||
                                     EF.Functions.Like(x.ComInKhmer!, $"%{param.Filter}%") ||
                                     EF.Functions.Like(x.ComDescription!, $"%{param.Filter}%"));

        if (param.SortDescending)
            query = query.OrderByDescending(x => EF.Property<object>(x, param.SortColumn!));

        var counts = await query.CountAsync();
        var data = await query.Skip(param.Page).Take(param.Take).AsNoTracking().ToListAsync();
        return new Pager<VCommunes>(data, counts);
    }

    public async Task<ErrorOr<string>> CreateCommuneAsync(CommuneDtoCreate model)
    {
        try
        {
            _logger.LogInformation("Creating commune record.");
            var context = _dbContextFactory;
            var data = model.Adapt<Commune>();
            await context.Communes.AddAsync(data);
            await context.SaveChangesAsync();
            return "New commune added.";
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error creating commune.");
            return Error.Unexpected(description: ex.InnerException!.Message);
        }
    }

    public async Task<ErrorOr<string>> UpdateCommuneAsync(CommuneDtoUpdate model)
    {
        try
        {
            _logger.LogInformation("Updating Commune record.");
            var context = _dbContextFactory;
            var exists = await context.Communes.AsNoTracking().AnyAsync(x => x.CommuneId == model.CommuneId);
            if (!exists) return Error.Conflict(description: "Commune doesn't exist!");

            var data = model.Adapt<Commune>();
            context.Communes.Update(data);
            await context.SaveChangesAsync();
            return "Commune was updated.";
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error updating commune.");
            return Error.Unexpected(description: ex.InnerException!.Message);
        }
    }

    public async Task<ErrorOr<string>> DeleteCommuneAsync(int? id)
    {
        try
        {
            _logger.LogInformation("Deleting commune record.");
            var context = _dbContextFactory;
            var result = await context.Communes.FirstOrDefaultAsync(x => x.CommuneId == id);
            if (result is null) return Error.NotFound(description: "No Commune Found.");

            context.Communes.Remove(result);
            await context.SaveChangesAsync();
            return "Commune was deleted.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting commune.");
            return Error.Unexpected(description: ex.Message);
        }
    }

    // ── Village CRUD ─────────────────────────────────────────────────────────

    public async Task<ErrorOr<Village>> GetVillageByIdAsync(int? id)
    {
        try
        {
            var context = _dbContextFactory;
            var result = await context.Villages.AsNoTracking().FirstOrDefaultAsync(x => x.VillageId == id);
            if (result is null) return Error.NotFound(description: "No Village found.");
            return result;
        }
        catch (Exception ex)
        {
            return Error.Unexpected(description: ex.Message);
        }
    }

    public async Task<ErrorOr<VVillageDetails>> GetVillageDetailsAsync(int? id)
    {
        try
        {
            var context = _dbContextFactory;
            var result = context.Database.SqlQuery<VVillageDetails>($"SELECT * FROM fGetVillageDetails({id})");
            if (result is null) return Error.NotFound(description: "No Village found.");
            var data = await result.AsNoTracking().FirstOrDefaultAsync();
            return data!;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving village details for ID: {VillageId}", id);
            return Error.Unexpected(description: ex.Message);
        }
    }

    public async Task<ErrorOr<IEnumerable<Village>>> GetAllVillagesAsync(int? proId = 0, int take = 0, string? condition = null)
    {
        _logger.LogInformation("Retrieving village records.");
        var context = _dbContextFactory;
        IQueryable<Village> query = context.Villages;

        if (!string.IsNullOrEmpty(condition))
            query = query.Where(x => EF.Functions.Like(x.VilInKhmer!, $"%{condition}%") ||
                                     EF.Functions.Like(x.VillDescription!, $"%{condition}%"));

        var items = await query.AsNoTracking().ToListAsync();
        return ErrorOrFactory.From<IEnumerable<Village>>(items);
    }

    public async Task<Pager<VVillages>> GetAllVillagesPagerAsync(QueryParam param)
    {
        _logger.LogInformation("Retrieving village paged records.");
        var context = _dbContextFactory;
        IQueryable<VVillages> query = context.VVillages;

        if (!string.IsNullOrEmpty(param.Filter))
            query = query.Where(x => EF.Functions.Like(x.VillDescription!, $"%{param.Filter}%") ||
                                     EF.Functions.Like(x.VilInKhmer!, $"%{param.Filter}%") ||
                                     EF.Functions.Like(x.ComInKhmer!, $"%{param.Filter}%") ||
                                     EF.Functions.Like(x.ComDescription!, $"%{param.Filter}%") ||
                                     EF.Functions.Like(x.VillageId.ToString(), $"%{param.Filter}%"));

        if (param.SortDescending)
            query = query.OrderByDescending(x => EF.Property<object>(x, param.SortColumn!));

        var counts = await query.CountAsync();
        var data = await query.Skip(param.Page).Take(param.Take).AsNoTracking().ToListAsync();
        return new Pager<VVillages>(data, counts);
    }

    public async Task<ErrorOr<string>> CreateVillageAsync(VillageDtoCreate model)
    {
        try
        {
            _logger.LogInformation("Creating village record.");
            var context = _dbContextFactory;
            var data = model.Adapt<Village>();
            await context.Villages.AddAsync(data);
            await context.SaveChangesAsync();
            return "New village added.";
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error creating village.");
            return Error.Unexpected(description: ex.InnerException!.Message);
        }
    }

    public async Task<ErrorOr<string>> UpdateVillageAsync(VillageDtoUpdate model)
    {
        try
        {
            _logger.LogInformation("Updating village record.");
            var context = _dbContextFactory;
            var exists = await context.Villages.AsNoTracking().AnyAsync(x => x.VillageId == model.VillageId);
            if (!exists) return Error.Conflict(description: "Village doesn't exist!");

            var data = model.Adapt<Village>();
            context.Villages.Update(data);
            await context.SaveChangesAsync();
            return "Village was updated.";
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error updating village.");
            return Error.Unexpected(description: ex.InnerException!.Message);
        }
    }

    public async Task<ErrorOr<string>> DeleteVillageAsync(int? id)
    {
        try
        {
            _logger.LogInformation("Deleting village record.");
            var context = _dbContextFactory;
            var result = await context.Villages.FirstOrDefaultAsync(x => x.VillageId == id);
            if (result is null) return Error.NotFound(description: "No Village Found.");

            context.Villages.Remove(result);
            await context.SaveChangesAsync();
            return "Village was deleted.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting village.");
            return Error.Unexpected(description: ex.Message);
        }
    }

    public async Task<ErrorOr<string>> DeleteCityAsync(int? id, CancellationToken cancellation = default)
    {
        try
        {
            _logger.LogInformation("Deleting city record.");
            var context = _dbContextFactory;
            var result = await context.Cities.FirstOrDefaultAsync(x => x.Id == id, cancellation);
            if (result is null) return Error.NotFound(description: "No City Found.");
            context.Cities.Remove(result);
            await context.SaveChangesAsync(cancellation);
            return "City was deleted.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting city.");
            return Error.Unexpected(description: ex.Message);
        }
    }


    /// <summary>
    /// Get Areas with optional filtering and pagination.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="param"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    public async Task<PageMetaData<AreaDto>> GetAreasAsync(long? id, QueryParam param, CancellationToken cancellation = default)
    {
        var context = _dbContextFactory;

        var queries = context.Set<AreaDto>().WhereIf(id.HasValue && id > 0, x => x.Id == id);

        var filter = param.Filter?.Trim();

        if (string.IsNullOrEmpty(filter))
        {
            return await queries.AsNoTracking()
                                .ToPagedAsync(param.Page, param.Take, cancellation);
        }


        if (!string.IsNullOrEmpty(param.FilterColumn))
        {
            queries = queries.StartWith(filter, param.FilterColumn.Trim());
        }
        else if (AllowedAreaFilter.Contains(param.FilterColumn))
        {
            queries = queries.StartWiths(filter, AllowedAreaFilter.ToArray());
        }

        return await queries.AsNoTracking()
                            .ToPagedAsync(param.Page, param.Take, cancellation);
    }

    public async Task<AreaDto> GetAreaAsync(long? id, CancellationToken cancellation = default)
    {
        var context = _dbContextFactory;

        var area = await context.Set<AreaDto>()
                             .AsNoTracking()
                             .FirstOrDefaultAsync(x => x.Id == id, cancellation);

        if (area == null)
        {
            throw new NotFoundExceptions("Area not found.");
        }

        return area;

    }


    public async Task<AreaDto?> AddOrUpdateAreasAsync(Area area, CancellationToken cancellation = default)
    {
        ArgumentNullException.ThrowIfNull(area);

        var context = _dbContextFactory;

        if (area.Id <= 0)
        {
            await context.Areas.AddAsync(area, cancellation);
        }
        else
        {
            context.Areas.Update(area);
        }

        await context.SaveChangesAsync(cancellation);

        var savedArea = await context.Set<AreaDto>()
                                     .AsNoTracking()
                                     .FirstOrDefaultAsync(x => x.Id == area.Id, cancellation);

        return savedArea ?? throw new NotFoundExceptions("Area not found after save.");
    }


    public async Task<bool> DeleteAreasAsync(long? id, CancellationToken cancellation = default)
    {
        ArgumentNullException.ThrowIfNull(id);

        var context = _dbContextFactory;

        var area = await context.Areas.FirstOrDefaultAsync(x => x.Id == id, cancellation);

        if (area == null)
        {
            throw new NotFoundExceptions("Area not found.");
        }

        context.Areas.Remove(area);

        var affectedRows = await context.SaveChangesAsync(cancellation);

        return affectedRows > 0;
    }




    /// <summary>
    /// private methods
    /// </summary>
    /// <param name="param"></param>
    /// <param name="queries"></param>
    /// <returns></returns>
    private static IQueryable<City> ApplyFilter(QueryParam param, IQueryable<City> queries)
    {
        var filter = param.Filter?.Trim();

        if (string.IsNullOrEmpty(filter))
        {
            return queries;
        }


        if (string.IsNullOrEmpty(param.FilterColumn))
        {
            queries = queries.StartWiths(filter, AllowedCitiesFilter.ToArray());
        }
        else if (AllowedCitiesFilter.Contains(param.FilterColumn))
        {
            queries = queries.StartWiths(filter, param.FilterColumn.Trim());
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

    private static readonly HashSet<string> AllowedAreaFilter = new(StringComparer.OrdinalIgnoreCase)
    {
        nameof(AreaDto.Code),
        nameof(AreaDto.Srok),
        nameof(AreaDto.Khum),
        nameof(AreaDto.Phum),
        nameof(AreaDto.Khet)
    };

    private static IQueryable<Country> ApplyFilter(QueryParam param, IQueryable<Country> queries)
    {
        var filter = param.Filter?.Trim();

        if (string.IsNullOrEmpty(filter))
            return queries;

        if (string.IsNullOrEmpty(param.FilterColumn))
        {
            queries = queries.StartWiths(filter, AllowedCountriesFilter.ToArray());
        }
        else if (AllowedCountriesFilter.Contains(param.FilterColumn))
        {
            queries = queries.StartWiths(filter, param.FilterColumn.Trim());
        }

        return queries;
    }

    private static IQueryable<Village> ApplyFilter(QueryParam param, IQueryable<Village> queries)
    {
        var filter = param.Filter?.Trim();

        if (string.IsNullOrEmpty(filter))
            return queries;

        if (string.IsNullOrEmpty(param.FilterColumn))
        {
            queries = queries.StartWiths(filter, AllowedVillagesFilter.ToArray());
        }
        else if (AllowedVillagesFilter.Contains(param.FilterColumn))
        {
            queries = queries.StartWiths(filter, param.FilterColumn.Trim());
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
            queries = queries.StartWiths(filter, AllowedCommunesFilter.ToArray());
        }
        else if (AllowedCommunesFilter.Contains(param.FilterColumn))
        {
            queries = queries.StartWiths(filter, param.FilterColumn.Trim());
        }

        return queries;
    }

    private static IQueryable<District> ApplyFilter(int? districtId, QueryParam param, ref IQueryable<District> queries)
    {
        if (districtId.HasValue && districtId > 0)
            queries = queries.Where(x => x.DistrictId == districtId);

        var filter = param.Filter?.Trim();

        if (string.IsNullOrWhiteSpace(filter))
            return queries;

        if (!string.IsNullOrEmpty(param.FilterColumn) && AllowedDistrcitsFilter.Contains(param.FilterColumn))
        {
            queries = queries.StartWiths(filter, param.FilterColumn.Trim());
        }
        else
        {
            queries = queries.StartWiths(filter, AllowedDistrcitsFilter.ToArray());
        }

        return queries;
    }



    private static IQueryable<Province> ApplySort(QueryParam param, IQueryable<Province> queries)
    {
        if (!string.IsNullOrEmpty(param.SortColumn))
        {
            queries = param.SortDescending
                ? queries.OrderByDescending(x => EF.Property<object>(x, param.SortColumn))
                : queries.OrderBy(x => EF.Property<object>(x, param.SortColumn));
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
            queries = queries.StartWiths(filter, AllowedProvincesFilter.ToArray());
        }
        else if (AllowedProvincesFilter.Contains(param.FilterColumn))
        {
            queries = queries.StartWiths(filter, param.FilterColumn.Trim());

        }

        return queries;
    }

}