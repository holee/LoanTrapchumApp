using Applications.Interfaces.Lists;
using Domains.Models.Lists;

namespace Applications.Services.Lists
{
    public sealed class NationalityService(INationalityService nService) : INationalityService
    {
        public async Task<IEnumerable<Nationality>> GetNationalities(int id = 12)
        {
           return await nService.GetNationalities(id);
        }

        public async Task<IEnumerable<Nationality>> GetNationalities(string? searchText = null)
        {
            return await nService.GetNationalities(searchText);
        }
    }
}