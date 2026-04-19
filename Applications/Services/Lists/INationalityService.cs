using Applications.Interfaces.Lists;
using Domains.Models.Lists;

namespace Applications.Services.Lists
{
    public interface INationalityServiceImpl
    {
        Task<IEnumerable<Nationality>> GetNationalities(string? searchText = null);
    }

    public sealed class NationalityService(INationalityService nService) : INationalityServiceImpl
    {

        public async Task<IEnumerable<Nationality>> GetNationalities(string? searchText = null)
        {
            return await nService.GetNationalities(searchText);
        }

    }
}