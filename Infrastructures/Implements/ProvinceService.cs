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
    public sealed class ProvinceService(IDapperConnectionFactory dapper,
            IDbContextFactory<DataContext> contextFactory,
            ILogger<ProvinceService> logger) : IProvinceService
    {
        public async Task<Result<string>> CreateAsync(Province province, CancellationToken cancellation = default)
        {
            logger.LogInformation("Creating new province with code: {ProvinceCode}", province.ProvinceCode);
            try
            {
                await using var context = await contextFactory.CreateDbContextAsync(cancellation);

                var pro = await context.Provinces.Where(x => x.ProvinceCode != null & x.ProvinceCode == province.ProvinceCode)
                                       .FirstOrDefaultAsync(cancellation);
                if (pro is not null) return Result.Failure<string>(ErrorResponse.Conflict("Province code already exist."));

                await context.Provinces.AddAsync(province, cancellation);

                await context.SaveChangesAsync(cancellation);

                return Result.Success<string>(new("New province added."));
            }
            catch (DbUpdateException ex)
            {
                logger.LogError(ex, "Error occurred while creating province with code: {ProvinceCode}", province.ProvinceCode);
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }
        }

        public async Task<Result<string>> DeleteAsync(int? proId, CancellationToken cancellation = default)
        {
            logger.LogInformation("Deleting province with ID: {ProvinceId}", proId);

            try
            {
                await using var context = await contextFactory.CreateDbContextAsync(cancellation);

                var pro = await context.Provinces.FirstOrDefaultAsync(dp => dp.ProvinceId == proId, cancellation);

                if (pro is null) return Result.Failure<string>(ErrorResponse.NotFound("No Province Found."));

                context.Provinces.Remove(pro);

                await context.SaveChangesAsync();

                return Result.Success<string>(new("Province was deleted."));

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while deleting province with ID: {ProvinceId}", proId);
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }
        }


        public async Task<Pager<ProvinceViewModel>> GetAllAsync(QueryParam param, CancellationToken cancellation = default)
        {

            await using var context = await contextFactory.CreateDbContextAsync(cancellation);

            IQueryable<Province> query = context.Provinces;

            if (param.IsIncludeInActive == true)
            {
                query = query.IgnoreQueryFilters();
            }

            if (!string.IsNullOrEmpty(param.Filter))
            {
                query = query.Where(x => EF.Functions.Like(x.ProDescription!, $"%{param.Filter}%") ||
                                         EF.Functions.Like(x.ProInKhmer!, $"%{param.Filter}%"));
            }

            if (param.SortDescending)
            {
                query = query.OrderByDescending(x => EF.Property<object>(x, param.SortColumn));
            }

            var counts = await query.CountAsync();

            var data = await query.Skip(param.Skip)
                           .Take(param.PageSize)
                           .Select(x => new ProvinceViewModel
                           {
                               ProvinceId = x.ProvinceId,
                               ProvinceCode = x.ProvinceCode,
                               ProDescription = x.ProDescription,
                               ProInKhmer = x.ProInKhmer,
                               // Add more properties as needed
                           })
                          .AsNoTracking()
                     .ToListAsync(cancellation);

            return new Pager<ProvinceViewModel>(data, counts);
        }

        public async Task<Result<Province>> GetById(int? proId, CancellationToken cancellation = default)
        {
            try
            {
                await using var context = await contextFactory.CreateDbContextAsync(cancellation);

                var pro = await context.Provinces.AsNoTracking().FirstOrDefaultAsync(dp => dp.ProvinceId == proId);

                if (pro is null) return Result.Failure<Province>(ErrorResponse.NotFound("No Deapartment found."));

                return Result.Success<Province>(pro);

            }
            catch (Exception ex)
            {
                return Result.Failure<Province>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }
        }

        public async Task<Result<string>> UpdateAsync(Province province, CancellationToken cancellation = default)
        {
            try
            {
                await using var context = await contextFactory.CreateDbContextAsync(cancellation);

                var result = await context.Provinces
                                            .FindAsync(province.ProvinceId);

                if (result is null) return Result.Failure<string>(ErrorResponse.Conflict("Province doesn't exist!"));

                context.Provinces.Update(province);

                await context.SaveChangesAsync();

                return Result.Success(new string("Province was updated"));
            }
            catch (DbUpdateException ex)
            {
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }
        }
    }
}
