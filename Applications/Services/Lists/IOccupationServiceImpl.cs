using Applications.Responses.Paginations;
using Domains.Models.Lists;

namespace Applications.Services.Lists
{
    public interface IOccupationServiceImpl
    {
        Task<IEnumerable<Occupation>> GetOccupationsAsync(string? expression = null);
        Task<Occupation?> GetOccupationByIdAsync(int? id);
        Task<Pager<T>> GetOccupations<T>(int? id, string? term = null);
    }
}
