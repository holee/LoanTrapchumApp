using Applications.Interfaces.Lists;
using Applications.Responses.Paginations;
using Domains.Models.Lists;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Applications.Services.Lists
{

    public sealed class OccupationService(IOccupationService oService) : IOccupationServiceImpl
    {
        public async Task<IEnumerable<Occupation>> GetOccupationsAsync(string? expression = null)
        {
            return await oService.GetOccupationsAsync(expression);
        }

        public async Task<Occupation?> GetOccupationByIdAsync(int? id)
        {
            return await oService.GetOccupationByIdAsync(id);
        }

        public async Task<Pager<T>> GetOccupations<T>(int? id, string? term = null)
        {
            return await oService.GetOccupations<T>(id, term);
        }


    }
}
