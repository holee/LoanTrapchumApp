using Applications.Requests.Lists.Entities;
using Applications.Requests.Lists.OtherNames;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.OtherNames;
using Domains.Models.Lists;

namespace Applications.Services.Lists;

public interface IGuarantorServiceImpl
{
    Task<Result<string>> CreateAsync(EntityNamesCreate othername);
    Task<Result<string>> DeleteAsync(string? id);
    Task<Result<EntityNames>> GetGuarantorByIdAsync(string? id);
    Task<Result<EntityNamesCreate>> GetGuarantorEditAsync(string? id);
    Task<Result<VGuarantor>> GetGuarantorByIdAsync(string? id, bool allGuarantors = false, bool include = true);
    Task<Result<IEnumerable<VGuarantor>>> GetGuarantorsAsync(string? id, bool allGuarantors = true, string? q = null);
    Task<Result<Pager<VGuarantor>>> GetGuarantorsAsync(QueryParam param);
    Task<Result<string>> UpdateAsync(EntityNamesCreate othername);
    Task<Pager<VGuarantor>> GetGuarantorsAsync(string? id, QueryParam q);
}
