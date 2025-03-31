using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.OtherNames;
using Domains.Models.Lists;

namespace Applications.Interfaces.Lists;
public interface IGuarantorService
{

    Task<Result<string>> CreateGaurantorAsync(EntityName othername);
    Task<Result<string>> UpdateGaurantorAsync(EntityName othername);
    Task<Result<string>> DeleteGaurantorAsync(Guid? id);
    Task<Result<Pager<VGuarantor>>> GetGuarantorsAsync(QueryParam param);
    Task<Result<IEnumerable<VGuarantor>>> GetGuarantorsAsync(Guid? id, bool allGuarantors = true, string? q = null);
    Task<Result<EntityName>> GetGuarantorByIdAsync(Guid? id);
    Task<Result<VGuarantor>> GetGuarantorByIdAsync(Guid? id, bool allGuarantors = false, bool include = true);
}