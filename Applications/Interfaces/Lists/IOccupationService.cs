using Domains.Models.Lists;

namespace Applications.Interfaces.Lists
{
    public interface IOccupationService
    {
        Task<Occupation?> FindOccupationAsync(int id);
        Task<IEnumerable<Occupation>> FindAllOccupationsAsync(string? expression = null);
    }
}
