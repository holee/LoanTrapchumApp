using Applications.Responses.Paginations;
using Domains.Models.Lists;

namespace Applications.Interfaces.Lists
{
    public interface IOccupationService
    {
        Task<Occupation?> GetOccupationByIdAsync(int? id);
        Task<IEnumerable<Occupation>> GetOccupationsAsync(string? expression = null);
        Task<Pager<T>> GetOccupations<T>(int? id, string? term = null);

    }
}
