using Application.Features.Periods.Dto;
using Application.Responses.Paginations;
using ErrorOr;

namespace Application.Interfaces.Lists;

public interface IPeriodService
{
    Task<Pager<PeriodDto>> GetAllAsync(QueryParam? param, CancellationToken cancellation = default);
    Task<PeriodDto?> GetByIdAsync(int id, CancellationToken cancellation = default);
    Task<ErrorOr<PeriodDto>> CreateAsync(PeriodDtoCreate request, CancellationToken cancellation = default);
    Task<ErrorOr<PeriodDto>> UpdateAsync(PeriodDtoUpdate request, CancellationToken cancellation = default);
    Task<ErrorOr<bool>> DeleteAsync(int id, CancellationToken cancellation = default);
}
