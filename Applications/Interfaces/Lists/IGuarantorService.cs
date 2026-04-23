using Applications.Features.Guarantor.Dtos;
using Applications.Responses.Paginations;
using Domains.Models.Lists;
using ErrorOr;

namespace Applications.Interfaces.Lists;
public interface IGuarantorService
{

    Task<ErrorOr<Created>> CreateAsync(EntityNames othername, CancellationToken cancellation = default);
    Task<ErrorOr<Updated>> UpdateAsync(EntityNames othername, CancellationToken cancellation = default);
    Task<ErrorOr<Deleted>> DeleteAsync(string? id, CancellationToken cancellation = default);
    Task<ErrorOr<IEnumerable<VGuarantor>>> GetGuarantorsAsync(string? id, bool allGuarantors = true, string? q = null);
    Task<ErrorOr<EntityNames>> GetGuarantorByIdAsync(string? id);
    Task<ErrorOr<VGuarantor>> GetGuarantorByIdAsync(string? id, bool allGuarantors = false, bool include = true);
    Task<Pager<VGuarantor>> GetGuarantorsAsync(string? id, QueryParam q);
    Task<Pager<VGuarantor>> GetGuarantorsAsync(QueryParam param, CancellationToken cancellation = default);
    Task<ErrorOr<VGuarantor>> GetGuarantorWithByIdAsync(string? id);
    Task<ErrorOr<VGuarantor>> GetGuarantorByIdAsync(string? id, CancellationToken cancellation = default);
    Task<ErrorOr<VGuarantor>> CreateGuarantorAsync(EntityNames othername, CancellationToken cancellation = default);
    Task<Pager<VGuarantor>> GetGuarantorsAsync(string? id, QueryParam param, CancellationToken cancellation = default);
}