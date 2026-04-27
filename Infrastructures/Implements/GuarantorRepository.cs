using Applications.Extensions;
using Applications.Features.Guarantor.Dtos;
using Applications.Interfaces.Lists;
using Applications.Responses.Paginations;
using Dapper;
using Domains.Models.Lists;
using ErrorOr;
using FluentValidation;
using Infrastructures.DataAccess;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Infrastructures.Implements;

public sealed class GuarantorRepository(IDbContextFactory<DataContext> contextFactory,
    IDapperConnectionFactory dapperConnection,
    ISalutationService salu,
    IOccupationService oc,
    ILogger<GuarantorRepository> logger) : IGuarantorService
{
    public async Task<ErrorOr<VGuarantor>> CreateAsync(EntityNames othername, CancellationToken cancellation = default)
    {
        try
        {
            logger.LogInformation("Creating guarantor with ID {GuarantorId}", othername.AltName);

            await using var context = await contextFactory.CreateDbContextAsync(cancellation);

            await AddOccupationIfNotExist(othername, context, cancellation);

            await AddSalutionIfNotExist(othername, context);

            context.EntityNames.Add(othername);

            await context.SaveChangesAsync(cancellation);

            var guarantor = await GetGuarantorDetailByIdAsync(othername.Id, cancellation);

            if (guarantor is null)
            {
                return Error.NotFound(description: "Guarantor was created but could not be retrieved.");
            }

            return guarantor;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating guarantor with ID {GuarantorId}", othername.Id);

            return Error.Unexpected(description: $"Error creating guarantor: {ex.Message}");
        }
    }
    public async Task<ErrorOr<Updated>> UpdateAsync(EntityNames othername, CancellationToken cancellation = default)
    {
        try
        {
            logger.LogInformation("Updating guarantor with ID {GuarantorId}", othername.Id);

            await using var context = await contextFactory.CreateDbContextAsync(cancellation);

            await AddOccupationIfNotExist(othername, context, cancellation);

            context.EntityNames.Update(othername);

            await context.SaveChangesAsync(cancellation);

            return Result.Updated;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            logger.LogError(ex, "Concurrency error updating guarantor with ID {GuarantorId}", othername.Id);

            return Error.Unexpected(description: ex.Message);

        }
    }
    public async Task<ErrorOr<Deleted>> DeleteAsync(string? id, CancellationToken cancellation = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellation);

            var existingLoan = context.LoanRequests
                                    .Where(x => x.GuarantorsLink!.Any(x => x.EntityNameRefId == id))
                                    .AsNoTracking();
            if (await existingLoan.AnyAsync(cancellation))
            {
                return Error.Conflict(description: "this guarantor is in used.");
            }

            var result = await context.EntityNames.FindAsync([id], cancellation);

            if (result is null)
            {
                return Error.NotFound(description: "no data found.");
            }

            context.EntityNames.Remove(result);

            await context.SaveChangesAsync(cancellation);

            return Result.Deleted;

        }
        catch (Exception ex)
        {
            return Error.Unexpected(description: ex.Message);
        }
    }
    public async Task<ErrorOr<EntityNames>> GetByIdAsync(string? id, CancellationToken cancellation = default)
    {

        await using var context = await contextFactory.CreateDbContextAsync(cancellation);

        var guarantors = await context.EntityNames
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(x => x.Id == id, cancellation);
        if (guarantors is null)
        {
            return Error.NotFound(description: "No data found");
        }
        return guarantors;

    }
    public async Task<Pager<VGuarantor>> GetGuarantorsAsync(string? id, QueryParam param, CancellationToken cancellation = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellation);

        var baseQuery = context.VGuarantors.AsQueryable();

        if (!string.IsNullOrEmpty(param.Filter))
        {

            baseQuery = baseQuery.Where(x =>
                                            EF.Functions.Like(x.GuarantorName!, $"%{param.Filter}") ||
                                            EF.Functions.Like(x.FullName!, $"%{param.Filter}") ||
                                            EF.Functions.Like(x.Gender!, $"%{param.Filter}") ||
                                            EF.Functions.Like(x.Phone!, $"%{param.Filter}"));
        }

        if (!param.IncludeInActive)
        {
            baseQuery = baseQuery.IgnoreQueryFilters();
        }

        if (!string.IsNullOrEmpty(id))
        {
            baseQuery = baseQuery.Where(x => x.Id == id);
        }

        if (param.SortDescending && !string.IsNullOrEmpty(param.SortColumn))
        {
            baseQuery = baseQuery.OrderByDescending(x => EF.Property<object>(x, param.SortColumn));
        }

        var counts = await baseQuery.CountAsync(cancellation);

        if (param.PageSize > 0)
        {
            baseQuery = baseQuery.Skip(param.Page).Take(param.PageSize);
        }

        var items = await baseQuery.AsNoTracking()
                                    .ToListAsync(cancellation);

        return new Pager<VGuarantor>(items, counts);

    }
    public async Task<ErrorOr<VGuarantor>> GetGuarantorByIdAsync(string? id, CancellationToken cancellation = default)
    {

        await using var context = await contextFactory.CreateDbContextAsync(cancellation);

        var guarantor = await context.VGuarantors
                                .Include(x => x.GuarantorLoans)
                                .Include(x => x.LoanRequests)
                                .AsSplitQuery()
                                .AsNoTracking()
                                .FirstOrDefaultAsync(x => x.Id == id, cancellation);

        if (guarantor is null)
        {
            return Error.NotFound(description: "There is no guarantor avalable.");
        }
        return guarantor;

    }
    private async Task<VGuarantor?> GetGuarantorDetailByIdAsync(string? id, CancellationToken cancellation = default)
    {

        var parameters = new DynamicParameters();

        parameters.Add("@Id", id);

        using var db = await dapperConnection.CreateConnectionAsync(cancellation);

        var guarantor = await db.QueryFirstOrDefaultAsync<VGuarantor>("SELECT * FROM VGuarantors WHERE Id = @Id", new { id });
        return guarantor;

    }
    private async Task AddSalutionIfNotExist(EntityNames othername, DataContext context)
    {
        if (string.IsNullOrEmpty(othername.Gender)) return;

        var duplicates = await context.Salutations
                                      .AnyAsync(x => x.Title == othername.Gender);
        if (duplicates) return;

        var sal = new Salutation
        {
            Title = othername.Gender
        };

        await salu.AddOrUpdateAsync(sal);
    }
    private async Task AddOccupationIfNotExist(EntityNames othername, DataContext context, CancellationToken cancellation)
    {
        if (!string.IsNullOrEmpty(othername.Occupation))
        {
            var occupation = await
                                context
                                .Occupations
                                .AsNoTracking()
                                .AnyAsync(x => x.OccupationTitle == othername.Occupation, cancellation);

            if (!occupation)
            {
                var newOccupation = new Occupation
                {
                    OccupationTitle = othername.Occupation,

                    Khmer = othername.Occupation,
                };

                await oc.CreateAsync(newOccupation);
            }

        }
    }

}


//public async Task<Pager<VGuarantor>> GetGuarantorsAsync(string? id, QueryParam q)
//{

//    await using var context = await contextFactory.CreateDbContextAsync();

//    var guarantors = context.VGuarantors
//                            .Where(x => EF.Functions.Like(x.GuarantorName!, $"%{q.Filter}%") ||
//                                       EF.Functions.Like(x.FullName!, $"%{q.Filter}%") ||
//                                       EF.Functions.Like(x.Gender!, $"%{q.Filter}%") ||
//                                       EF.Functions.Like(x.Phone!, $"%{q.Filter}%"));
//    if (q.PageSize > 0)
//    {
//        guarantors = guarantors.Skip(q.Page).Take(q.PageSize);
//    }

//    var counts = await guarantors.CountAsync();

//    var items = await guarantors.AsNoTracking().ToListAsync();

//    return new Pager<VGuarantor>(items, counts);

//}

//public async Task<ErrorOr<IEnumerable<VGuarantor>>> GetGuarantorsAsync(string? id, bool allGuarantors = true, string? q = null)
//{

//    await using var context = await contextFactory.CreateDbContextAsync();

//    var guarantors = context.VGuarantors
//                            .WhereIf(!string.IsNullOrEmpty(q), x => EF.Functions.Like(x.GuarantorName!, $"%{q}%") ||
//                                                                    EF.Functions.Like(x.FullName!, $"%{q}%") ||
//                                                                    EF.Functions.Like(x.Gender!, $"%{q}%") ||
//                                                                   EF.Functions.Like(x.Phone!, $"%{q}%"))
//                            .WhereIf(!string.IsNullOrEmpty(id), x => x.Id == id)
//                            .WhereIf(!allGuarantors, x => x.IsActive == false);
//    if (!await guarantors.AnyAsync())
//    {
//        return Error.NotFound(description: "There is no guarantor avalable.");
//    }

//    var result = await guarantors.AsNoTracking().ToListAsync();

//    return result;

//}

//public async Task<ErrorOr<Created>> CreateAsync(EntityNames othername, CancellationToken cancellation = default)
//{
//    try
//    {
//        await using var context = await contextFactory.CreateDbContextAsync(cancellation);

//        context.EntityNames.Add(othername);

//        await context.SaveChangesAsync(cancellation);

//        return Result.Created;

//    }
//    catch (Exception ex)
//    {
//        return Error.Unexpected(description: ex.Message);
//    }
//}

//public async Task<ErrorOr<VGuarantor>> GetGuarantorByIdAsync(string? id, bool allGuarantors = false, bool includeRequest = true)
//{

//    if (id.IsNullOrEmpty())
//    {
//        return Error.NotFound(description: "There is no guarantor available.");
//    }

//    var parameters = new DynamicParameters();

//    parameters.Add("@Id", id);

//    parameters.Add("@AllGuarantors", allGuarantors);

//    //parameters.Add("@IncludeRequest", includeRequest);

//    using var db = dapperConnection.CreateConnection();


//    if (!includeRequest)
//    {

//        var existingG = await db.QueryFirstOrDefaultAsync<VGuarantor>("spGetGaurantorWithLoans", parameters, commandType: CommandType.StoredProcedure);

//        if (existingG is null)
//        {
//            return Error.NotFound(description: "There is no guarantor avalable.");
//        }

//        return existingG;
//    }

//    var lookup = new Dictionary<string, VGuarantor>();

//    var items = await db.QueryAsync<VGuarantor, VLoanGuarantors, VGuarantor>(ProcedureConstant.SpGetGaurantorWithLoans,
//        (g, gl) =>
//        {
//            if (lookup.TryGetValue(g.Id!, out var existingGaurantor))
//            {
//                g = existingGaurantor;

//                g.LoanGuarantors = [];
//            }
//            else
//            {
//                lookup.Add(g.Id!, g);
//            }

//            if (gl is not null)
//            {
//                g.LoanGuarantors.Add(gl);
//            }

//            return g;

//        }, commandType: CommandType.StoredProcedure,
//        param: parameters,
//        splitOn: "LoanId");

//    var guarantor = items.FirstOrDefault(x => x.Id == id);

//    if (guarantor is null)
//    {
//        return Error.NotFound(description: "There is no guarantor avalable.");
//    }
//    return guarantor;

//}


//public async Task<ErrorOr<VGuarantor>> GetGuarantorWithByIdAsync(string? id)
//{

//    if (id.IsNullOrEmpty())
//    {
//        return Error.NotFound(description: "There is no guarantor available.");
//    }

//    var parameters = new DynamicParameters();

//    parameters.Add("@Id", id);

//    using var db = dapperConnection.CreateConnection();

//    var lookup = new Dictionary<string, VGuarantor>();

//    var items = await db.QueryAsync<VGuarantor, VGuarantorsLoanRequest, VGuarantorLoan, VGuarantor>(ProcedureConstant.SpGetGaurantorWithLoanRequests,
//        (g, gl, d) =>
//        {
//            if (!lookup.TryGetValue(g.Id!, out var existingGaurantor))
//            {
//                existingGaurantor = g;

//                existingGaurantor.LoanGuarantors = [];

//                lookup.Add(g.Id!, existingGaurantor);
//            }
//            if (gl is not null && gl.LoanRequestId.IsNotNullOrEmpty())
//            {
//                g.LoanRequests.Add(gl);
//            }
//            if (d is not null && d.LoanId.IsNotNullOrEmpty())
//            {
//                g.GuarantorLoans.Add(d);
//            }
//            return g;

//        }, commandType: CommandType.StoredProcedure,
//        param: parameters,
//        splitOn: "LoanRequestId,LoanId");

//    var guarantor = items.FirstOrDefault(x => x.Id == id);

//    if (guarantor is null)
//    {
//        return Error.NotFound(description: "There is no guarantor avalable.");
//    }
//    return guarantor;

//}
