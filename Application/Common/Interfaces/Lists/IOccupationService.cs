using Application.Features.AdditionalInformations.Dto;
using Application.Responses;
using Application.Responses.Paginations;
using Domains.Models.Lists;
using Domains.Shared.Enums;
using ErrorOr;

namespace Application.Interfaces.Lists
{
    public interface IOccupationService
    {
        Task<Occupation?> GetOccupationByIdAsync(int? id, CancellationToken cancellation = default);
        Task<OccupationDto?> GetOccupationDetailByIdAsync(int? id,CancellationToken cancellation = default); 
        Task<Pager<OccupationDto>> GetOccupationsAsync(int? id, QueryParam? param, TypeOfEnum ofOption = TypeOfEnum.Position, CancellationToken cancellation = default);
        Task<ErrorOr<OccupationDto>> CreateAsync(Occupation occupation, CancellationToken cancellation = default);
        Task<ErrorOr<OccupationDto>> UpdateAsync(OccupationDtoUpate request, CancellationToken cancellation = default);
    }
}
