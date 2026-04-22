using Applications.Features.AdditionalInformations.Dto;
using Applications.Responses;
using Applications.Responses.Paginations;
using Domains.Models.Lists;
using Domains.Shared.Enums;
using ErrorOr;

namespace Applications.Interfaces.Lists
{
    public interface IOccupationService
    {
        Task<Occupation?> GetOccupationByIdAsync(int? id, CancellationToken cancellation = default);
        Task<IEnumerable<Occupation>> GetOccupationsAsync(string? expression = null);
        Task<Pager<OccupationDto>> GetOccupationsAsync(int? id, QueryParam? param, TypeOfEnum ofOption = TypeOfEnum.Position, CancellationToken cancellation = default);
        Task<ErrorOr<OccupationDto>> CreateAsync(Occupation occupation, CancellationToken cancellation = default);
        Task<ErrorOr<OccupationDto>> UpdateAsync(OccupationDtoUpate request, CancellationToken cancellation = default);
    }
}
