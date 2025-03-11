using Domains.Models.Lists;

namespace Applications.Interfaces.Lists
{
    public interface INationalityService
    {
        Task<IEnumerable<Nationality>> GetNationalities(int id = 12);
        Task<IEnumerable<Nationality>> GetNationalities(string? searchText = null);
    }
}