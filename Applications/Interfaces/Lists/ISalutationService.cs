using Applications.Features.AdditionalInformations.Dto;
using Applications.Responses;
using Domains.Models.Lists;

namespace Applications.Interfaces.Lists
{
    public interface ISalutationService
    {
        Task<Result<bool>> DeleteAsync(int? id, CancellationToken cancellation = default);
        Task<Pager<SalutationDto>> GetsSalutions(int? id, QueryParam? param, CancellationToken ct = default);
        Task<SalutationDto?> AddOrUpdateAsync(Salutation salutation, CancellationToken ct = default);
    }
}
