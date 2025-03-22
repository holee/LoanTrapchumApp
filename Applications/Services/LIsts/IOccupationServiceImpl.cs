using Domains.Models.Lists;

namespace Applications.Services.Lists
{
    public interface IOccupationServiceImpl
    {
        Task<IEnumerable<Occupation>> FindAllOccupationsAsync(string? expression = null);
        Task<Occupation?> FindOccupationAsync(int id);
    }
}
