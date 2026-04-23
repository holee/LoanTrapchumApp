using Applications.Features.Guarantor.Dtos;
using Applications.Features.OtherNames.Dto;
using Applications.Responses.Paginations;
using Domains.Models.Lists;
using ErrorOr;

namespace Applications.Services.Lists;

public interface IGuarantorServiceImpl
{
    Task<ErrorOr<Created>> CreateAsync(EntityNamesCreate othername);
    Task<ErrorOr<VGuarantor>> CreateAsync(EntityGuarantorCreate othername);
    Task<ErrorOr<Deleted>> DeleteAsync(string? id);
    Task<ErrorOr<Updated>> UpdateAsync(EntityNamesCreate othername);
    Task<ErrorOr<EntityNames>> GetGuarantorByIdAsync(string? id);
    Task<ErrorOr<EntityGuarantorCreate>> GetGuarantorEditAsync(string? id);
    Task<ErrorOr<VGuarantor>> GetGuarantorByIdAsync(string? id, bool allGuarantors = false, bool include = true);
    Task<ErrorOr<IEnumerable<VGuarantor>>> GetGuarantorsAsync(string? id, bool allGuarantors = true, string? q = null);
    Task<Pager<VGuarantor>> GetGuarantorsAsync(string? id, QueryParam q);
    Task<Pager<VGuarantor>> GetGuarantorsAsync(QueryParam param, CancellationToken cancellation = default);
    Task<ErrorOr<VGuarantor>> UpdateAsync(EntityGuarantorCreate othername);
   // Task<ErrorOr<VGuarantor>> GetGuarantorWithByIdAsync(string? id);
    //Task<ErrorOr<VGuarantor>> GetGuarantorByIdAsync(string? id, CancellationToken cancellation = default);
}
