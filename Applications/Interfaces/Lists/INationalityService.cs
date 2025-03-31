using Domains.Models.Lists;

namespace Applications.Interfaces.Lists
{
    public interface INationalityService
    {
        Task<IEnumerable<Nationality>> GetNationalities(int id);
        Task<IEnumerable<Nationality>> GetNationalities(string? searchText = null);
        Task<IEnumerable<T>> GetNationalities<T>(int? id, string? searchText = null) where T:class;
    }
}