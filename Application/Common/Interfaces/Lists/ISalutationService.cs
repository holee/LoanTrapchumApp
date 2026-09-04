using Application.Features.AdditionalInformations.Dto;
using Application.Responses;
using Domains.Models.Lists;

namespace Application.Interfaces.Lists
{
    public interface ISalutationService
    {
        Task<Result<bool>> DeleteAsync(int? id, CancellationToken cancellation = default);
        Task<Pager<SalutationDto>> GetsSalutions(int? id, QueryParam? param, CancellationToken ct = default);
        Task<SalutationDto?> AddOrUpdateAsync(Salutation salutation, CancellationToken ct = default);
    }
}
