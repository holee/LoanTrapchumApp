using Applications.Interfaces.Addresses;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Addresses;
using Domains.Models.Addresses;
using Infrastructures.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructures.Implements
{
    public class DistrictService : IDistrictService
    {
        private readonly IDbContextFactory<DataContext> contextFactory;
        private readonly ILogger<DistrictService> logger;
        private readonly DataContext dataContext;

        public DistrictService( IDbContextFactory<DataContext> context,
            ILogger<DistrictService> logger,
            DataContext dataContext) {
            contextFactory = context;
            this.logger = logger;
            this.dataContext = dataContext;
        }
        public async Task<Result<string>> CreateAsync(District model)
        {
            try
            {
                logger.LogInformation("Creating district record.");
                await using var cxt = await contextFactory.CreateDbContextAsync();

                await cxt.Districts.AddAsync(model);

                await cxt.SaveChangesAsync();

                return Result.Success<string>(new("New District added."));
            }
            catch (DbUpdateException ex)
            {
                logger.LogError("District",ex.Message);
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }
        }

        public async Task<Result<string>> DeleteAsync(int? Id)
        {
            try
            {
                logger.LogInformation("Deleting district record.");
                await using var cxt = await contextFactory.CreateDbContextAsync();
                var result = await cxt.Districts.FirstOrDefaultAsync(dp => dp.DistrictId == Id);

                if (result is null) return Result.Failure<string>(ErrorResponse.NotFound("No District Found."));

                cxt.Districts.Remove(result);

                await cxt.SaveChangesAsync();

                return Result.Success<string>(new("District was deleted."));

            }
            catch (Exception ex)
            {
                logger.LogError($"{ex.Message}", ex);
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }
        }

        public async Task<Result<IEnumerable<District>>> GetAllAsync(int? proId = 0, int take = 0, string? condition = null)
        {
            logger.LogInformation("Retrieving district record.");
            await using var cxt = await contextFactory.CreateDbContextAsync();
            IQueryable<District> query = cxt.Districts;

            if (!string.IsNullOrEmpty(condition))
            {
                query = query.Where(x => EF.Functions.Like(x.DisInKhmer!, $"%{condition}%")
                || EF.Functions.Like(x.DisDescription!, $"%{condition}%")
               );
            }
            var dis = await query.AsNoTracking().ToListAsync();

            return Result.Success<IEnumerable<District>>(dis);
        }

        public async Task<Pager<VDistricts>> GetAllPagerAsync(QueryParam param)
        {
            logger.LogInformation("Retrieving district record.");
            await using var cxt = await contextFactory.CreateDbContextAsync();
            IQueryable<VDistricts> query = cxt.VDistricts;

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

            var data = await query.Skip(param.Page)
                           .Take(param.PageSize)
                           .AsNoTracking()
                           .ToListAsync();
            

            return new Pager<VDistricts>(data, counts);
        }

        public async Task<Result<District>> GetByIdAsync(int? Id)
        {
            try
            {
                logger.LogInformation("Retrieving district record.");
                await using var cxt = await contextFactory.CreateDbContextAsync();
                var result = await cxt.Districts.AsNoTracking().FirstOrDefaultAsync(dp => dp.DistrictId == Id);

                if (result is null) return Result.Failure<District>(ErrorResponse.NotFound("No District found."));

                return Result.Success<District>(result);

            }
            catch (Exception ex)
            {
                logger.LogError($"{ex.Message}",ex);
                return Result.Failure<District>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }
        }

        public async Task<Result<string>> UpdateAsync(District model)
        {
            try
            {
                logger.LogInformation("Updating district record.");
                await using var cxt = await contextFactory.CreateDbContextAsync();
                var result = await cxt.Districts
                    .Where(x => x.DistrictId == model.DistrictId).AsNoTracking().FirstOrDefaultAsync();

                if (result is null) return Result.Failure<string>(ErrorResponse.Conflict("District doesn't exist!"));

                cxt.Districts.Update(model);

                await cxt.SaveChangesAsync();

                return Result.Success(new string("District was updated"));
            }
            catch (DbUpdateException ex)
            {
                logger.LogError(ex.Message, ex);
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }
        }
    }
}
