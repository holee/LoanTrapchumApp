using Applications.Interfaces.Addresses;
using Applications.Responses;
using Domains.Models.Addresses;
using Infrastructures.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Implements;

public class AddressService : IAddressService
{
    private readonly ApplicationDbContext _context;

    public AddressService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Province>> GetsProvinces(string? expression)
    {
        IQueryable<Province> query = _context.Provinces;

        if (!string.IsNullOrEmpty(expression))
        {
            query = query.Where(x =>
            x.ProInKhmer != null && x.ProInKhmer.Contains(expression) ||
            x.ProDescription != null && x.ProDescription.Contains(expression));
        }
        return await query.AsNoTracking().ToListAsync();
    }
    public async Task<Province?> GetsProvinceById(int id)
    {
        IQueryable<Province> query = _context.Provinces;
        return await query.AsNoTracking().FirstOrDefaultAsync(x => x.ProvinceId == id);
    }

    public async Task<IEnumerable<District>> GetDistrictsByProvince(int provinceId)
    {
        IQueryable<District> query = _context.Districts.Where(x => x.ProvinceRefId == provinceId);
        return await query.AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<Commune>> GetCommunesByDistrict(int districtId)
    {
        IQueryable<Commune> query = _context.Communes.Where(x => x.DistrictRefId == districtId);
        return await query.AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<Village>> GetVillagesByCommune(int communeId)
    {
        IQueryable<Village> query = _context.Villages.Where(x => x.CommuneRefId == communeId);
        return await query.AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<Country>> GetCountries(string? searchText = null)
    {
        IQueryable<Country> query = _context.Countries;

        if (!string.IsNullOrEmpty(searchText))
        {
            query = query.Where(x => x.DesInKhmer != null && x.DesInKhmer.StartsWith(searchText, StringComparison.InvariantCultureIgnoreCase) ||
                                x.CountryName != null && x.CountryName.StartsWith(searchText, StringComparison.InvariantCultureIgnoreCase));
        }
        return await query.AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<string>> GetCities(string? searchText = null)
    {
        IQueryable<City> query = _context.Cities;

        if (!string.IsNullOrEmpty(searchText))
        {
            query = query.Where(x => x.CityName != null && x.CityName.StartsWith(searchText, StringComparison.InvariantCultureIgnoreCase));
        }
        return await query.AsNoTracking().Select(x => x.CityName).ToListAsync();
    }

    public async Task<Result> AddCity(City city)
    {
        try
        {
            var c = _context.Cities.Where(x => x.CityName == city.CityName);
            if (c.Any())
            {
                return Result.Failure(ErrorResponse.Conflict("City Already Exist."));
            }
            _context.Cities.Add(city);
            await _context.SaveChangesAsync();
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
            var c = _context.Cities.Where(x => x.CityName == city.CityName && x.Id != city.Id);
            if (c.Any())
            {
                return Result.Failure(ErrorResponse.Conflict("City Already Exist."));
            }
            _context.Cities.Update(city);
            await _context.SaveChangesAsync();
            return Result.Success("City was Updated.");
        }
        catch (Exception ex)
        {
            return Result.Failure(ErrorResponse.ServerError($"{ex.Message}"));
        }

    }


    public async Task<IEnumerable<Country>> GetCountries(int countryId)
    {
        IQueryable<Country> query = _context.Countries;

        if (countryId > 0)
        {
            query = query.Where(x => x.CountryId == countryId);
        }
        return await query.AsNoTracking().ToListAsync();
    }
}


