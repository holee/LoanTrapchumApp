using Applications.Interfaces.Addresses;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Addresses;
using Domains.Models.Addresses;
using Infrastructures.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Implements
{
    public class DistrictService : IDistrictService
    {
        private readonly DataContext _context;

        public DistrictService(DataContext context) {
            _context = context;
        }
        public async Task<Result<string>> CreateAsync(District model)
        {
            try
            {
                
                await _context.Districts.AddAsync(model);

                await _context.SaveChangesAsync();

                return Result.Success<string>(new("New District added."));
            }
            catch (DbUpdateException ex)
            {
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }
        }

        public async Task<Result<string>> DeleteAsync(int? Id)
        {
            try
            {
                var result = await _context.Districts.FirstOrDefaultAsync(dp => dp.DistrictId == Id);

                if (result is null) return Result.Failure<string>(ErrorResponse.NotFound("No District Found."));

                _context.Districts.Remove(result);

                await _context.SaveChangesAsync();

                return Result.Success<string>(new("District was deleted."));

            }
            catch (Exception ex)
            {
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }
        }

        public Task<Result<IEnumerable<VDistricts>>> GetAllAsync(int? proId = 0, int take = 0, string? condition = null)
        {
            throw new NotImplementedException();
        }

        public async Task<Pager<VDistricts>> GetAllPagerAsync(QueryParam param)
        {
            IQueryable<VDistricts> query = _context.VDistricts;

            if (!string.IsNullOrEmpty(param.Filter))
            {
                query = query.Where(x => EF.Functions.Like(x.DisInKhmer!, $"%{param.Filter}%")
                || EF.Functions.Like(x.DisDescription!, $"%{param.Filter}%")
                || EF.Functions.Like(x.ProDescription!, $"%{param.Filter}%")
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
            

            return new Pager<VDistricts>(data, counts);
        }

        public async Task<Result<District>> GetByIdAsync(int? Id)
        {
            try
            {
                var result = await _context.Districts.AsNoTracking().FirstOrDefaultAsync(dp => dp.DistrictId == Id);

                if (result is null) return Result.Failure<District>(ErrorResponse.NotFound("No District found."));

                return Result.Success<District>(result);

            }
            catch (Exception ex)
            {
                return Result.Failure<District>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }
        }

        public async Task<Result<string>> UpdateAsync(District model)
        {
            try
            {
                var result = await _context.Districts
                    .Where(x => x.DistrictId == model.DistrictId).AsNoTracking().FirstOrDefaultAsync();

                if (result is null) return Result.Failure<string>(ErrorResponse.Conflict("District doesn't exist!"));

                _context.Districts.Update(model);

                await _context.SaveChangesAsync();

                return Result.Success(new string("District was updated"));
            }
            catch (DbUpdateException ex)
            {
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }
        }
    }
}
