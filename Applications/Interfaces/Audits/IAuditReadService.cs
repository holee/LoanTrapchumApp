using Applications.Responses.Audits;
using Applications.Responses.Paginations;

namespace Applications.Interfaces.Audits;

public interface IAuditReadService
{
    Task<Pager<AuditVm>> GetAllAsync(string? userId, DateOnly? from, DateOnly? to, QueryParam param, CancellationToken ct = default);
}
