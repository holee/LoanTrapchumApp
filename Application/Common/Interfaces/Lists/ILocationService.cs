using Application.Features.Locations.Dtos;
using Application.Responses.Paginations;
using Domains.Models.Lists;
using ErrorOr;

namespace Application.Interfaces.Lists;

public interface ILocationService
{
    Task<ErrorOr<LocationDto>> GetByIdAsync(long id, CancellationToken cancellation = default);
    Task<IEnumerable<LocationDto>> GetAllAsync(CancellationToken cancellation = default);
    Task<Pager<LocationDto>> GetPagedAsync(QueryParam q, CancellationToken cancellation = default);
    Task<ErrorOr<LocationDto>> CreateAsync(LtLocation entity, CancellationToken cancellation = default);
    Task<ErrorOr<LocationDto>> UpdateAsync(LtLocation entity, CancellationToken cancellation = default);
    Task<ErrorOr<string>> DeleteAsync(long id, CancellationToken cancellation = default);
}
