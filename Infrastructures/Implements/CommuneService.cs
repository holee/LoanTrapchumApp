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
    public class CommuneService : ICommuneService
    {
        private readonly IDbContextFactory<DataContext> contextFactory;
        private readonly ILogger<CommuneService> logger;

        public CommuneService(IDbContextFactory<DataContext> contextFactory, ILogger<CommuneService> logger) {
            this.contextFactory = contextFactory;
            this.logger = logger;
        } 
        public async Task<Result<string>> CreateAsync(CommuneDtoCreate model)
        {
            try
            {
                logger.LogInformation("Creating commune record.");
                await using var cxt = await contextFactory.CreateDbContextAsync();

                var data = model.Adapt<Commune>();
                await cxt.Communes.AddAsync(data);

                await cxt.SaveChangesAsync();

                return Result.Success<string>(new("New commune added."));
            }
            catch (DbUpdateException ex)
            {
                logger.LogError("Commune", ex.Message);
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }
        }

        public async Task<Result<string>> DeleteAsync(int? Id)
        {
            try
            {
                logger.LogInformation("Deleting commune record.");
                await using var cxt = await contextFactory.CreateDbContextAsync();
                var result = await cxt.Communes.FirstOrDefaultAsync(dp => dp.CommuneId == Id);

                if (result is null) return Result.Failure<string>(ErrorResponse.NotFound("No commune Found."));

                cxt.Communes.Remove(result);

                await cxt.SaveChangesAsync();

                return Result.Success<string>(new("Commune was deleted."));

            }
            catch (Exception ex)
            {
                logger.LogError($"{ex.Message}", ex);
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }
        }

        public async Task<Result<IEnumerable<Commune>>> GetAllAsync(int? proId = 0, int take = 0, string? condition = null)
        {
            logger.LogInformation("Retrieving commune record.");
            await using var cxt = await contextFactory.CreateDbContextAsync();
            IQueryable<Commune> query = cxt.Communes;

            if (!string.IsNullOrEmpty(condition))
            {
                query = query.Where(x => EF.Functions.Like(x.ComInKhmer!, $"%{condition}%")
                || EF.Functions.Like(x.ComDescription!, $"%{condition}%")
               );
            }
            var dis = await query.AsNoTracking().ToListAsync();

            return Result.Success<IEnumerable<Commune>>(dis);
        }

        public async Task<Pager<VCommunes>> GetAllPagerAsync(QueryParam param)
        {
            logger.LogInformation("Retrieving commune record.");
            await using var cxt = await contextFactory.CreateDbContextAsync();
            IQueryable<VCommunes> query = cxt.VCommunes;

            if (!string.IsNullOrEmpty(param.Filter))
            {
                query = query.Where(x => EF.Functions.Like(x.DisInKhmer!, $"%{param.Filter}%")
                || EF.Functions.Like(x.DisDescription!, $"%{param.Filter}%")
                || EF.Functions.Like(x.ComInKhmer!, $"%{param.Filter}%")
                || EF.Functions.Like(x.ComDescription!, $"%{param.Filter}%"));
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


            return new Pager<VCommunes>(data, counts);
        }

        public async Task<Result<Commune>> GetByIdAsync(int? Id)
        {
            try
            {
                logger.LogInformation("Retrieving commune record.");
                await using var cxt = await contextFactory.CreateDbContextAsync();
                var result = await cxt.Communes.AsNoTracking().FirstOrDefaultAsync(dp => dp.CommuneId == Id);

                if (result is null) return Result.Failure<Commune>(ErrorResponse.NotFound("No District found."));

                return Result.Success<Commune>(result);

            }
            catch (Exception ex)
            {
                logger.LogError($"{ex.Message}", ex);
                return Result.Failure<Commune>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }
        }

        public async Task<Result<string>> UpdateAsync(CommuneDtoUpdate model)
        {
            try
            {
                logger.LogInformation("Updating Commune record.");
                await using var cxt = await contextFactory.CreateDbContextAsync();
                var result = await cxt.Communes
                    .Where(x => x.CommuneId == model.CommuneId).AsNoTracking().FirstOrDefaultAsync();

                if (result is null) return Result.Failure<string>(ErrorResponse.Conflict("Commune doesn't exist!"));
                var data = model.Adapt<Commune>();
                cxt.Communes.Update(data);

                await cxt.SaveChangesAsync();

                return Result.Success(new string("Commune was updated"));
            }
            catch (DbUpdateException ex)
            {
                logger.LogError(ex.Message, ex);
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }
        }
    }
}
