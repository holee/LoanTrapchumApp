using Applications.Interfaces.Lists;
using Applications.Requests.Lists.Entities;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans;
using Applications.Responses.ViewModels.OtherNames;
using Domains.Models.Lists;
using FluentValidation;
using Infrastructures.DataAccess;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Implements;
public class GuarantorService : IGuarantorService
{
    private readonly ApplicationDbContext _context;
    private readonly IValidator<EntityNameDtoCreate> _validator;
    public GuarantorService(ApplicationDbContext context, IValidator<EntityNameDtoCreate> validator)
    {
        _context = context;
        _validator = validator;
    }
    public async Task<Result<string>> CreateGaurantorAsync(EntityName othername)
    {
        try
        {
            var req = othername.Adapt<EntityNameDtoCreate>();

            var validatorResult = _validator.Validate(req);

            if (!validatorResult.IsValid)
            {
                var errors = validatorResult.Errors.Select(er => er.ErrorMessage).ToList();
                var errString = string.Join(Environment.NewLine, errors);
                return Result.Failure<string>(ErrorResponse.ServerError(errString));
            }

            _context.OtherNames.Add(othername);

            await _context.SaveChangesAsync();

            return Result.Success(new string("A new guarantor was created"));

        }
        catch (Exception ex)
        {
            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));
        }
    }
    public async Task<Result<string>> UpdateGaurantorAsync(EntityName othername)
    {
        try
        {
            _context.OtherNames.Update(othername);
            await _context.SaveChangesAsync();
            return Result.Success(new string("A new guarantor was created"));
        }
        catch (Exception ex)
        {
            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));
        }
    }
    public async Task<Result<string>> DeleteGaurantorAsync(long id)
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

            var result = _context.OtherNames.SingleOrDefault(er => er.EntId == id);

            if (result is null)
            {
                return Result.Failure<string>(ErrorResponse.NotFound("no data found."));
            }

            _context.OtherNames.Remove(result);

            await _context.SaveChangesAsync();

            return Result.Success(new string("A new guarantor was created"));

        }
        catch (Exception ex)
        {
            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));
        }
    }
    public async Task<Result<EntityName>> GetGuarantorByIdAsync(long id)
    {
        var guarantors = _context.OtherNames.Where(x => x.EntId == id);

        if (!guarantors.Any())
        {
            return Result.Failure<EntityName>(ErrorResponse.NotFound("No data found"));
        }

        var item = await guarantors.AsNoTracking().FirstOrDefaultAsync();

        return Result.Success(item!);

    }
    public async Task<Result<Pager<VGuarantor>>> GetGuarantorsAsync(QueryParam param)
    {
        var guarantors = _context.Database.SqlQuery<VGuarantor>($"SELECT * FROM VGuarantors");

        if (!string.IsNullOrEmpty(param.Filter))
        {
            guarantors = guarantors.Where(x => EF.Functions.Like(x.GuarantorName, $"%{param.Filter}%") ||
                                                EF.Functions.Like(x.Phone!, $"%{param.Filter}%"));
        }

        if (!param.IsIncludeInActive)
        {
            guarantors = guarantors.Where(x => x.IsActive == false);
        }

        if (param.SortDescending)
        {
            guarantors = guarantors.OrderByDescending(x => EF.Property<object>(x, param.SortColumn));
        }

        if (!guarantors.Any())
        {
            return Result.Failure<Pager<VGuarantor>>(ErrorResponse.NotFound("No data found"));
        }

        var counts = await guarantors.CountAsync();

        var items = await guarantors.Skip(param.PageIndex)
                                    .Take(param.PageSize)
                                    .ToListAsync();

        return Result.Success(new Pager<VGuarantor>(items, counts));

    }
    public async Task<Result<IEnumerable<VGuarantor>>> GetGuarantorsAsync(long id, bool allGuarantors = true, string? q = null)
    {

        var guarantors = _context.Database.SqlQuery<VGuarantor>($"SELECT * FROM VGuarantors");

        if (!string.IsNullOrEmpty(q))
        {
            guarantors = guarantors.Where(x => EF.Functions.Like(x.GuarantorName!, $"%{q}%") ||
                                                EF.Functions.Like(x.Phone!, $"%{q}%"));
        }
        if (!allGuarantors)
        {
            guarantors = guarantors.Where(x => x.IsActive == false);
        }
        if (id > 0)
        {
            guarantors = guarantors.Where(x => x.Id == id);
        }
        if (!guarantors.Any())
        {
            return Result.Failure<IEnumerable<VGuarantor>>(ErrorResponse.NotFound("There is no guarantor avalable."));
        }

        var result = await guarantors.AsNoTracking().ToListAsync();

        return Result.Success<IEnumerable<VGuarantor>>(result);

    }

    public async Task<Result<VGuarantor>> GetGuarantorByIdAsync(long id, bool allGuarantors = false, bool include = true)
    {
        var guarantors = _context.Database.SqlQuery<VGuarantor>($"SELECT * FROM VGuarantors");

        if (!allGuarantors)
        {
            guarantors = guarantors.Where(x => x.IsActive == false);
        }
        if (id > 0)
        {
            guarantors = guarantors.Where(x => x.Id == id);
        }
        if (!guarantors.Any())
        {
            return Result.Failure<VGuarantor>(ErrorResponse.NotFound("There is no guarantor avalable."));
        }

        var result = await guarantors.AsNoTracking().FirstAsync();

        if (include)
        {

            var loans = await _context.Database
                                .SqlQuery<VLoanGuarantors>($"SELECT * FROM VLoanGuarators WHERE GuarantorId={id}")
                                .AsNoTracking()
                                .ToListAsync();
            result.LoanGuarantors = loans ?? [];
        }
        return Result.Success<VGuarantor>(result!);
    }


}