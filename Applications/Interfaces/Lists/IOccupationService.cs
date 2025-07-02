using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels;
using Domains.Models.Lists;
using Domains.Shared.Enums;

namespace Applications.Interfaces.Lists
{
    public interface IOccupationService
    {
        Task<Occupation?> GetOccupationByIdAsync(int? id);
        Task<IEnumerable<Occupation>> GetOccupationsAsync(string? expression = null);
        Task<Pager<T>> GetOccupations<T>(int? id, string? term = null);
        Task<Pager<OccupationVm>> GetOccupationsAsync(int? id, QueryParam param, CancellationToken cancellation = default);
        Task<Result<OccupationVm>> CreateAsync(Occupation occupation, CancellationToken cancellation = default);
        Task<Pager<T>> GetOccupations<T>(int? id, TypeOfEnum ofOption, string? term = null, CancellationToken cancellation = default);
    }
}
