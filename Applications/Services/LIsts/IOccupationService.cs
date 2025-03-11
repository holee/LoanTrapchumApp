using Applications.Interfaces.Lists;
using Domains.Models.Lists;

namespace Applications.Services.Lists
{
    public sealed class OccupationService(IOccupationService oService) : IOccupationService
    {
        public async Task<IEnumerable<Occupation>> FindAllOccupationsAsync(string? expression = null)
        {
           return await oService.FindAllOccupationsAsync(expression);
        }

        public async Task<Occupation?> FindOccupationAsync(int id)
        {
            return await oService.FindOccupationAsync(id);
        }
    }
}
