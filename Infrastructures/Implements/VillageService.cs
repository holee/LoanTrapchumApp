using Applications.Interfaces.Addresses;
using Applications.Requests.Lists.Addreses;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Addresses;
using Domains.Models.Addresses;
using Infrastructures.DataAccess;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Implements
{
    public class VillageService : IVillageServer
    {
        private readonly IDbContextFactory<DataContext> contextFactory;
        private readonly ILogger<VillageService> logger;

        public VillageService(IDbContextFactory<DataContext> contextFactory,ILogger<VillageService> logger) {
            this.contextFactory = contextFactory;
            this.logger = logger;
        }
        public async Task<Result<string>> CreateAsync(VillageDtoCreate model)
        {
            try
            {
                logger.LogInformation("Creating village record.");
                await using var cxt = await contextFactory.CreateDbContextAsync();

                var data = model.Adapt<Village>();
                await cxt.Villages.AddAsync(data);

                await cxt.SaveChangesAsync();

                return Result.Success<string>(new("New village added."));
            }
            catch (DbUpdateException ex)
            {
                logger.LogError("Village", ex.Message);
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }
        }

        public async Task<Result<string>> DeleteAsync(int? Id)
        {
            try
            {
                logger.LogInformation("Deleting village record.");
                await using var cxt = await contextFactory.CreateDbContextAsync();
                var result = await cxt.Villages.FirstOrDefaultAsync(dp => dp.VillageId == Id);

                if (result is null) return Result.Failure<string>(ErrorResponse.NotFound("No village Found."));

                cxt.Villages.Remove(result);

                await cxt.SaveChangesAsync();

                return Result.Success<string>(new("Village was deleted."));

            }
            catch (Exception ex)
            {
                logger.LogError($"{ex.Message}", ex);
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }
        }

        public async Task<Result<IEnumerable<Village>>> GetAllAsync(int? proId = 0, int take = 0, string? condition = null)
        {
            logger.LogInformation("Retrieving village record.");
            await using var cxt = await contextFactory.CreateDbContextAsync();
            IQueryable<Village> query = cxt.Villages;

            if (!string.IsNullOrEmpty(condition))
            {
                query = query.Where(x => EF.Functions.Like(x.VilInKhmer!, $"%{condition}%")
                || EF.Functions.Like(x.VillDescription!, $"%{condition}%")
               );
            }
            var dis = await query.AsNoTracking().ToListAsync();

            return Result.Success<IEnumerable<Village>>(dis);
        }

        public async Task<Pager<VVillages>> GetAllPagerAsync(QueryParam param)
        {
            logger.LogInformation("Retrieving commune record.");
            await using var cxt = await contextFactory.CreateDbContextAsync();
            IQueryable<VVillages> query = cxt.VVillages;

            if (!string.IsNullOrEmpty(param.Filter))
            {
                query = query.Where(x => EF.Functions.Like(x.VillDescription!, $"%{param.Filter}%")
                || EF.Functions.Like(x.VilInKhmer!, $"%{param.Filter}%")
                || EF.Functions.Like(x.ComInKhmer!, $"%{param.Filter}%")
                || EF.Functions.Like(x.ComDescription!, $"%{param.Filter}%")
                || EF.Functions.Like(x.VillageId.ToString(), $"%{param.Filter}%"));
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


            return new Pager<VVillages>(data, counts);
        }
        

        public async Task<Result<Village>> GetByIdAsync(int? Id)
        {
            try
            {
                logger.LogInformation("Retrieving village record.");
                await using var cxt = await contextFactory.CreateDbContextAsync();
                var result = await cxt.Villages.AsNoTracking().FirstOrDefaultAsync(dp => dp.VillageId == Id);

                if (result is null) return Result.Failure<Village>(ErrorResponse.NotFound("No village found."));

                return Result.Success<Village>(result);

            }
            catch (Exception ex)
            {
                logger.LogError($"{ex.Message}", ex);
                return Result.Failure<Village>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }
        }

        public async Task<Result<string>> UpdateAsync(VillageDtoUpdate model)
        {
            try
            {
                logger.LogInformation("Updating village record.");
                await using var cxt = await contextFactory.CreateDbContextAsync();
                var result = await cxt.Villages
                    .Where(x => x.VillageId == model.VillageId).AsNoTracking().FirstOrDefaultAsync();

                if (result is null) return Result.Failure<string>(ErrorResponse.Conflict("Village doesn't exist!"));
                var data = model.Adapt<Village>();
                cxt.Villages.Update(data);

                await cxt.SaveChangesAsync();

                return Result.Success(new string("Village was updated"));
            }
            catch (DbUpdateException ex)
            {
                logger.LogError(ex.Message, ex);
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }
        }
    }
}
