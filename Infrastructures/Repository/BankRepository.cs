using Applications.Commons;
using Applications.Exceptions;
using Applications.Extensions;
using Applications.Features.Banks.Dtos;
using Applications.Interfaces.Banks;
using Applications.Responses.Paginations;
using Domains.Models.Currencies;
using Infrastructures.DataAccess;
using Infrastructures.Repository.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repository;

public sealed class BankRepository : IBankService
{
    private readonly DataContext _contextFactory;
    public BankRepository(DataContext contextFactory)
    {
        _contextFactory = contextFactory;
    }
    public async Task<BankDto> CreateAsync(Bank bank, CancellationToken cancellation = default)
    {

        var _context = _contextFactory;

        _context.Banks.Add(bank);

        await _context.SaveChangesAsync();

        return new BankDto
        {
            AltName = bank.AltName,
            BankCode = bank.BankCode,
            BankName = bank.BankName,
            AltPhone = bank.AltPhone,
            BankId = bank.BankId,
            CommuneName = bank.CommuneName,
            Country = bank.Country,
            DistrictName = bank.DistrictName,
            Email = bank.Email,
            IsClosed = bank.IsClosed ?? false,
            IsHead = bank.IsHead ?? false,
            ProvinceName = bank.ProvinceName,
            Remark = bank.Remark,
            VillageName = bank.VillageName,
            WebSite = bank.WebSite,
        };
    }

    public async Task<bool> DeleteAsync(int? id, CancellationToken cancellation = default)
    {

        ArgumentNullException.ThrowIfNull(id, nameof(id));

        var _context = _contextFactory;

        var result = await _context.Banks.FirstOrDefaultAsync(x => x.BankId == id);

        if (result is null)
        {
            throw new NotFoundExceptions("មិនបានរកឃើញ");
        }

        _context.Banks.Remove(result);

        await _context.SaveChangesAsync(cancellation);

        return true;
    }

    public async Task<PageMetaData<BankDto>> GetAllAsync(int? bankId, QueryParam query, CancellationToken cancellation = default)
    {
        var _context = _contextFactory;

        IQueryable<Bank> queries = _context.Banks.AsNoTracking()
                                            .WhereIf(bankId.HasValue && bankId > 0, x => x.BankId == bankId.Value);

        string filter = query.Filter?.Trim() ?? string.Empty;

        if (string.IsNullOrEmpty(filter) is false)
        {
            if (string.IsNullOrEmpty(query.FilterColumn))
            {
                string[] filterColumns = [nameof(Bank.BankCode), nameof(Bank.BankName), nameof(Bank.AltPhone), nameof(Bank.Email)];

                queries = queries.StartWiths(filter, filterColumns);
            }
            else
            {
                string? filterColumn = typeof(Bank).MapField(query.FilterColumn);

                if (string.IsNullOrEmpty(filterColumn) is false)
                {
                    queries = queries.StartWiths(filter, filterColumn);
                }
            }
        }

        queries = queries.OrderByColumn(query.SortColumn, query.SortDescending, x => x.BankId);

        var paginate = queries.Select(x => new BankDto
        {
            AltName = x.AltName,
            BankCode = x.BankCode,
            BankName = x.BankName,
            AltPhone = x.AltPhone,
            BankId = x.BankId,
            CommuneName = x.CommuneName,
            Country = x.Country,
            DistrictName = x.DistrictName,
            Email = x.Email,
            IsClosed = x.IsClosed ?? false,
            IsHead = x.IsHead ?? false,
            ProvinceName = x.ProvinceName,
            Remark = x.Remark,
            VillageName = x.VillageName,
            WebSite = x.WebSite,
        });

        return await paginate.ToPagedAsync(query.Page, query.Take, cancellation);
    }

    public async Task<Bank?> GetByIdAsync(int? id, CancellationToken cancellation = default)
    {

        ArgumentNullException.ThrowIfNull(id, nameof(id));

        var _context = _contextFactory;

        var found = await _context.Banks.AsNoTracking().FirstOrDefaultAsync(x => x.BankId == id);

        if (found is null)
        {
            throw new NotFoundExceptions("ស្វែងរកធនាគារមិនឃើញ");
        }

        return found;
    }

    public async Task<BankDto?> UpdateAsync(int? id, Bank bank, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id, nameof(id));

        var _context = _contextFactory;

        var exists = await _context.Banks.AsNoTracking().AnyAsync(x => x.BankId == id, cancellationToken);

        if (exists is false)
        {
            throw new NotFoundExceptions("មិនបានរកឃើញ");
        }

        bank.BankId = id.Value;

        _context.Banks.Update(bank);

        await _context.SaveChangesAsync(cancellationToken);

        return new BankDto
        {
            AltName = bank.AltName,
            BankCode = bank.BankCode,
            BankName = bank.BankName,
            AltPhone = bank.AltPhone,
            BankId = bank.BankId,
            CommuneName = bank.CommuneName,
            Country = bank.Country,
            DistrictName = bank.DistrictName,
            Email = bank.Email,
            IsClosed = bank.IsClosed ?? false,
            IsHead = bank.IsHead ?? false,
            ProvinceName = bank.ProvinceName,
            Remark = bank.Remark,
            VillageName = bank.VillageName,
            WebSite = bank.WebSite,
        };
    }


}
