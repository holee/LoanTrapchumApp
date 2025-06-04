using Applications.Commons.Mappers;
using Applications.Interfaces.Audits;
using Applications.Responses.Audits;
using Applications.Responses.Paginations;
using Infrastructures.DataAccess;
using Microsoft.EntityFrameworkCore;
namespace Infrastructures.Implements.Audits;

public sealed class AuditReadService : IAuditReadService
{
    private readonly IDbContextFactory<DataContext> _contextFactory;

    public AuditReadService(IDbContextFactory<DataContext> context)
    {
        _contextFactory = context;
    }

    public async Task<Pager<AuditVm>> GetAllAsync(string? userId, DateOnly? from, DateOnly? to, QueryParam param, CancellationToken ct = default)
    {

        await using var _context = await _contextFactory.CreateDbContextAsync(ct);

        var query = _context
                            .Database
                            .SqlQuery<AuditVm>($"SELECT * FROM VAuditTrails")
                            .AsNoTracking();

        if (userId is not null) query = query.Where(a => a.UserId == userId);

        if (from.HasValue) query = query.Where(a => a.CreatedAt >= from.Value.ToDateTime(TimeOnly.MinValue));

        if (to.HasValue) query = query.Where(a => a.CreatedAt <= to.Value.ToDateTime(TimeOnly.MaxValue));

        query = query.OrderByDescending(a => a.CreatedAt);


        var counts = await query.CountAsync(ct);

        if (param.PageSize > 0)
        {
            query = query.Skip(param.Skip).Take(param.PageSize);
        }

        var projection = await query
                              .AsNoTracking()
                              .ToListAsync(ct);

        return new Pager<AuditVm>(projection, counts);

    }

}
