using Applications.Interfaces.Lists;
using Domains.Models.Lists;

namespace Applications.Services.Lists
{
    public interface INationalityServiceImpl
    {
        Task<IEnumerable<Nationality>> GetNationalities(int id);
        Task<IEnumerable<Nationality>> GetNationalities(string? searchText = null);
    }

    public sealed class NationalityService(INationalityService nService) : INationalityServiceImpl
    {
        public async Task<IEnumerable<Nationality>> GetNationalities(int id)
        {
            return await nService.GetNationalities(id);
        }

        public async Task<IEnumerable<Nationality>> GetNationalities(string? searchText = null)
        {
            return await nService.GetNationalities(searchText);
        }

        public async Task<IEnumerable<Nationality>> GetNationalities(int id, string? searchText = null)
        {
            return await nService.GetNationalities<Nationality>(id, searchText);
        }


    }
}