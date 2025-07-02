using Applications.Requests.Settings;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels;
using Domains.Models.Lists;
using Domains.Shared.Enums;

namespace Applications.Services.Lists
{
    public interface IOccupationServiceImpl
    {
        Task<IEnumerable<Occupation>> GetOccupationsAsync(string? expression = null);
        Task<Occupation?> GetOccupationByIdAsync(int? id);
        Task<Pager<T>> GetOccupations<T>(int? id, string? term = null);
        Task<Pager<OccupationVm>> GetOccupationsAsync(int? id, QueryParam param, CancellationToken cancellation = default);
        Task<Result<OccupationVm>> CreateAsync(OccupationDtoCreate req);
        Task<Pager<T>> GetOccupations<T>(int? id, TypeOfEnum typeOf, string? term = null);
    }
}
