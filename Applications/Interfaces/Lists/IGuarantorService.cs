using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.OtherNames;
using Domains.Models.Lists;

namespace Applications.Interfaces.Lists;
public interface IGuarantorService
{

    Task<Result<string>> CreateGaurantorAsync(EntityNames othername);
    Task<Result<string>> UpdateGaurantorAsync(EntityNames othername);
    Task<Result<string>> DeleteGaurantorAsync(string? id);
    Task<Result<IEnumerable<VGuarantor>>> GetGuarantorsAsync(string? id, bool allGuarantors = true, string? q = null);
    Task<Result<EntityNames>> GetGuarantorByIdAsync(string? id);
    Task<Result<VGuarantor>> GetGuarantorByIdAsync(string? id, bool allGuarantors = false, bool include = true);
    Task<Pager<VGuarantor>> GetGuarantorsAsync(string? id, QueryParam q);
    Task<Pager<VGuarantor>> GetGuarantorsAsync(QueryParam param, CancellationToken cancellation = default);
}