using Applications.Extensions;
using Applications.Interfaces.Lists;
using Applications.Requests.Lists.Entities;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans;
using Applications.Responses.ViewModels.OtherNames;
using Dapper;
using FluentValidation;
using Infrastructures.DataAccess;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Infrastructures.Implements;
public class GuarantorService : IGuarantorService
{
    private readonly DataContext _context;
    private readonly IDbContextFactory<DataContext> _contextFactory;
    private readonly IValidator<EntityNameDtoCreate> _validator;
    private readonly IDapperConnectionFactory dapperConnection;
    public GuarantorService(DataContext context, IValidator<EntityNameDtoCreate> validator, IDbContextFactory<DataContext> contextFactory, IDapperConnectionFactory dapperConnection)
    {
        _context = context;
        _validator = validator;
        _contextFactory = contextFactory;
        this.dapperConnection = dapperConnection;
    }
    public async Task<Result<string>> CreateGaurantorAsync(EntityNames othername)
    {
        try
        {

            _context.EntityNames.Add(othername);

            await _context.SaveChangesAsync();

            return Result.Success(new string("A new guarantor was created"));

        }
        catch (Exception ex)
        {
            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));
        }
    }
    public async Task<Result<string>> UpdateGaurantorAsync(EntityNames othername)
    {
        try
        {
            _context.EntityNames.Update(othername);

            await _context.SaveChangesAsync();

            return Result.Success(new string("A Gaurantor was changed"));
        }
        catch (Exception ex)
        {
            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));
        }
    }
    public async Task<Result<string>> DeleteGaurantorAsync(string? id)
    {
        try
        {

            var existingLoan = _context.LoanRequests
                                    .Where(x => x.GuarantorsLink!.Any(x => x.EntityNameRefId == id))
                                    .AsNoTracking();
            if (existingLoan.Any())
            {
                return Result.Failure<string>(ErrorResponse.ServerError("this guarantor is in used."));
            }

            var result = await _context.EntityNames.FindAsync(id);

            if (result is null)
            {
                return Result.Failure<string>(ErrorResponse.NotFound("no data found."));
            }

            _context.EntityNames.Remove(result);

            await _context.SaveChangesAsync();

            return Result.Success(new string("A guarantor was removed"));

        }
        catch (Exception ex)
        {
            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));
        }
    }
    public async Task<Result<EntityNames>> GetGuarantorByIdAsync(string? id)
    {
        var guarantors = await _context.EntityNames
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(x => x.Id == id);

        if (guarantors is null)
        {
            return Result.Failure<EntityNames>(ErrorResponse.NotFound("No data found"));
        }
        return Result.Success(guarantors);

    }
    public async Task<Result<Pager<VGuarantor>>> GetGuarantorsAsync(QueryParam param)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var guarantors = context.Database.SqlQuery<VGuarantor>($"SELECT * FROM VGuarantors")
                                .WhereIf(!string.IsNullOrEmpty(param.Filter), x => EF.Functions.Like(x.GuarantorName, $"%{param.Filter}%") ||
                                                EF.Functions.Like(x.Phone!, $"%{param.Filter}%"))
                                .WhereIf(!param.IsIncludeInActive, x => x.IsActive == false);

        if (param.SortDescending)
        {
            guarantors = guarantors.OrderByDescending(x => EF.Property<object>(x, param.SortColumn));
        }

        if (!guarantors.Any())
        {
            return Result.Failure<Pager<VGuarantor>>(ErrorResponse.NotFound("No data found"));
        }

        var counts = guarantors.Count();

        if (param.PageSize > 0)
        {
            guarantors = guarantors.Skip(param.PageNumber).Take(param.PageSize);
        }

        var items = await guarantors.AsNoTracking()
                                    .ToListAsync();

        return Result.Success(new Pager<VGuarantor>(items, counts));

    }
    public async Task<Result<IEnumerable<VGuarantor>>> GetGuarantorsAsync(string? id, bool allGuarantors = true, string? q = null)
    {

        await using var context = await _contextFactory.CreateDbContextAsync();

        var guarantors = context.Database
                                .SqlQuery<VGuarantor>($"SELECT * FROM VGuarantors")
                                .WhereIf(!string.IsNullOrEmpty(q), x => EF.Functions.Like(x.GuarantorName!, $"%{q}%") ||
                                                                       EF.Functions.Like(x.Phone!, $"%{q}%"))
                                .WhereIf(!string.IsNullOrEmpty(id), x => x.Id == id)
                                .WhereIf(!allGuarantors, x => x.IsActive == false);
        if (!guarantors.Any())
        {
            return Result.Failure<IEnumerable<VGuarantor>>(ErrorResponse.NotFound("There is no guarantor avalable."));
        }

        var result = await guarantors.AsNoTracking().ToListAsync();

        return Result.Success<IEnumerable<VGuarantor>>(result);

    }

    public async Task<Pager<VGuarantor>> GetGuarantorsAsync(string? id, QueryParam q)
    {

        await using var context = await _contextFactory.CreateDbContextAsync();

        var guarantors = context.Database
                                .SqlQuery<VGuarantor>($"SELECT * FROM VGuarantors")
                                .WhereIf(!string.IsNullOrEmpty(q.Filter), x => EF.Functions.Like(x.GuarantorName!, $"%{q.Filter}%") ||
                                                                       EF.Functions.Like(x.FullName!, $"%{q.Filter}%") ||
                                                                       EF.Functions.Like(x.Gender!, $"%{q.Filter}%") ||
                                                                       EF.Functions.Like(x.Phone!, $"%{q.Filter}%"))
                                .WhereIf(!string.IsNullOrEmpty(id), x => x.Id == id)
                                .WhereIf(!q.IsIncludeInActive, x => x.IsActive == false);
        if (!guarantors.Any())
        {
            return new Pager<VGuarantor>([], 0);
        }

        if (q.PageSize > 0)
        {
            guarantors = guarantors.Skip(q.PageNumber).Take(q.PageSize);
        }

        var counts = await guarantors.CountAsync();

        var items = await guarantors.AsNoTracking().ToListAsync();

        return new Pager<VGuarantor>(items, counts);

    }


    public async Task<Result<VGuarantor>> GetGuarantorByIdAsync(string? id, bool allGuarantors = false, bool includeRequest = true)
    {

        if (id.IsNullOrEmpty())
        {
            return Result.Failure<VGuarantor>(ErrorResponse.NotFound("There is no guarantor available."));
        }

        var parameters = new DynamicParameters();

        parameters.Add("@Id", id);

        parameters.Add("@AllGuarantors", allGuarantors);

        parameters.Add("@IncludeRequest", includeRequest);

        using var db = dapperConnection.CreateConnection();


        if (!includeRequest)
        {

            var existingG = await db.QueryFirstOrDefaultAsync<VGuarantor>("spGetGaurantorWithLoans", parameters, commandType: CommandType.StoredProcedure);

            if (existingG is null)
            {
                return Result.Failure<VGuarantor>(ErrorResponse.NotFound("There is no guarantor avalable."));
            }

            return Result.Success(existingG);
        }

        var lookup = new Dictionary<string, VGuarantor>();

        var items = await db.QueryAsync<VGuarantor, VLoanGuarantors, VGuarantor>("spGetGaurantorWithLoans",
            (g, gl) =>
            {
                if (lookup.TryGetValue(g.Id!, out var existingGaurantor))
                {
                    g = existingGaurantor;

                    g.LoanGuarantors = [];
                }
                else
                {
                    lookup.Add(g.Id!, g);
                }

                if (gl is not null)
                {
                    g.LoanGuarantors.Add(gl);
                }

                return g;

            }, commandType: CommandType.StoredProcedure,
            param: parameters,
            splitOn: "LoanId");

        var guarantor = items.FirstOrDefault(x => x.Id == id);

        if (guarantor is null)
        {
            return Result.Failure<VGuarantor>(ErrorResponse.NotFound("There is no guarantor avalable."));
        }
        return Result.Success(guarantor);

    }


}