using Applications.Interfaces.Addresses;
using Applications.Requests.Lists.Addreses;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Addresses;
using Applications.Responses.ViewModels.Departments;
using Domains.Models.Addresses;
using Domains.Models.Lists;
using Infrastructures.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;


namespace Infrastructures.Implements
{
    public class ProvinceService : IProvinceService
    {
        private readonly DataContext _context;
        private readonly IDapperConnectionFactory _dapper;
        public ProvinceService(DataContext context,IDapperConnectionFactory dapper) 
        {
            _context = context;
            _dapper = dapper;
        }

        public async Task<Result<string>> CreateAsync(Province province)
        {
            try
            {
                var pro = await _context.Provinces.Where(x => x.ProvinceCode != null & x.ProvinceCode == province.ProvinceCode)
                                       .FirstOrDefaultAsync();
                if (pro is not null) return Result.Failure<string>(ErrorResponse.Conflict("Province code already exist."));

                await _context.Provinces.AddAsync(province);

                await _context.SaveChangesAsync();

                return Result.Success<string>(new("New province added."));
            }
            catch (DbUpdateException ex)
            {
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }
        }

        public async Task<Result<string>> DeleteAsync(int? proId)
        {
            try
            {
                var pro = await _context.Provinces.FirstOrDefaultAsync(dp => dp.ProvinceId == proId);

                if (pro is null) return Result.Failure<string>(ErrorResponse.NotFound("No Province Found."));

                _context.Provinces.Remove(pro);

                await _context.SaveChangesAsync();

                return Result.Success<string>(new("Province was deleted."));

            }
            catch (Exception ex)
            {
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }
        }

        public async Task<Result<IEnumerable<Province>>> GetAllAsync(int? proId = 0, int take = 0, string? condition = null)
        {
            try
            {
                IQueryable<Province> provinces = _context.Provinces;

                if (condition is not null)
                {
                    provinces = provinces.Where(x => x.ProvinceCode != null
                                                && x.ProvinceCode.Contains(condition));
                }

                if (proId > 0)
                {

                    provinces = provinces.Where(x => x.ProvinceId == proId);
                }

                if (take > 0)
                {
                    provinces = provinces.Take(take).OrderByDescending(x => x.ProvinceCode);
                }

                var pros = await provinces.AsNoTracking().ToListAsync();

                return Result.Success < IEnumerable <Province>>(pros);

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Pager<ProvinceViewModel>> GetAllPager(QueryParam param)
        {
            IQueryable<Province> query = _context.Provinces;

            if (param.IsIncludeInActive == true)
            {
                query = query.IgnoreQueryFilters();
            }

            if (!string.IsNullOrEmpty(param.Filter))
            {
                query = query.Where(x => EF.Functions.Like(x.ProDescription!, $"%{param.Filter}%")
                || EF.Functions.Like(x.ProInKhmer!, $"%{param.Filter}%"));
            }

            if (param.SortDescending)
            {
                query = query.OrderByDescending(x => EF.Property<object>(x, param.SortColumn));
            }

            var counts = await query.CountAsync();

            var data = await query.Skip(param.Skip)
                           .Take(param.PageSize)
                           .AsNoTracking()
                           .ToListAsync();
            var result = data.Select(x => new ProvinceViewModel
            {
                ProvinceId = x.ProvinceId,
                ProvinceCode = x.ProvinceCode,
                ProDescription = x.ProDescription,
                ProInKhmer = x.ProInKhmer,
                // Add more properties as needed
            }).ToList();


            return new Pager<ProvinceViewModel>(result, counts);
        }

        public async Task<Result<Province>> GetById(int? proId)
        {
            try
            {
                var pro = await _context.Provinces.AsNoTracking().FirstOrDefaultAsync(dp => dp.ProvinceId == proId);

                if (pro is null) return Result.Failure<Province>(ErrorResponse.NotFound("No Deapartment found."));

                return Result.Success<Province>(pro);

            }
            catch (Exception ex)
            {
                return Result.Failure<Province>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }
        }

        public async Task<Result<string>> UpdateAsync(Province province)
        {
            try
            {
                var result = await _context.Provinces
                    .Where(x => x.ProvinceId==province.ProvinceId).AsNoTracking().FirstOrDefaultAsync();

                if (result is null) return Result.Failure<string>(ErrorResponse.Conflict("Province doesn't exist!"));

                _context.Provinces.Update(province);

                await _context.SaveChangesAsync();

                return Result.Success(new string("Province was updated"));
            }
            catch (DbUpdateException ex)
            {
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }
        }
    }
}
