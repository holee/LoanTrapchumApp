using Application.Responses.Audits;
using Application.Responses.Paginations;

namespace Application.Interfaces.Audits;

public interface IAuditReadService
{
    Task<Pager<AuditVm>> GetAllAsync(string? userId, DateOnly? from, DateOnly? to, QueryParam param, CancellationToken ct = default);
}
