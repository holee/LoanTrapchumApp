using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.OtherNames;
using Domains.Models.Lists;

namespace Applications.Interfaces.Lists;
public interface IGuarantorService
{

    Task<Result<string>> CreateAsync(EntityNames othername, CancellationToken cancellation = default);
    Task<Result<string>> UpdateAsync(EntityNames othername,CancellationToken cancellation=default);
    Task<Result<string>> DeleteAsync(string? id,CancellationToken cancellation = default);
    Task<Result<IEnumerable<VGuarantor>>> GetGuarantorsAsync(string? id, bool allGuarantors = true, string? q = null);
    Task<Result<EntityNames>> GetGuarantorByIdAsync(string? id);
    Task<Result<VGuarantor>> GetGuarantorByIdAsync(string? id, bool allGuarantors = false, bool include = true);
    Task<Pager<VGuarantor>> GetGuarantorsAsync(string? id, QueryParam q);
    Task<Pager<VGuarantor>> GetGuarantorsAsync(QueryParam param, CancellationToken cancellation = default);
    Task<Result<VGuarantor>> GetGuarantorWithByIdAsync(string? id);
}