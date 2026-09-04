using ErrorOr;

namespace Application.Interfaces.Lists;

public interface IGuarantorService
{

    Task<ErrorOr<Updated>> UpdateAsync(EntityNames othername, CancellationToken cancellation = default);
    Task<ErrorOr<Deleted>> DeleteAsync(string? id, CancellationToken cancellation = default);
    Task<ErrorOr<EntityNames>> GetByIdAsync(string? id, CancellationToken cancellation = default);
    Task<ErrorOr<VGuarantor>> GetGuarantorByIdAsync(string? id, CancellationToken cancellation = default);
    Task<ErrorOr<VGuarantor>> CreateAsync(EntityNames othername, CancellationToken cancellation = default);
    Task<Pager<VGuarantor>> GetGuarantorsAsync(string? id, QueryParam param, CancellationToken cancellation = default);
}
//Task<Pager<VGuarantor>> GetGuarantorsAsync(string? id, QueryParam q);
//Task<ErrorOr<IEnumerable<VGuarantor>>> GetGuarantorsAsync(string? id, bool allGuarantors = true, string? q = null);
//Task<Pager<VGuarantor>> GetGuarantorsAsync(QueryParam param, CancellationToken cancellation = default);
//Task<ErrorOr<VGuarantor>> GetGuarantorByIdAsync(string? id, bool allGuarantors = false, bool include = true);