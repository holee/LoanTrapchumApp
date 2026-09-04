using Application.Features.AdditionalInformations.Dto;
using Domains.Models.Lists;

namespace Application.Interfaces.Lists
{
    public interface INationalityService
    {
        Task<IEnumerable<Nationality>> GetNationalities(string? searchText = null);
        Task<Pager<NationalityDto>> GetNationalities(int? id, QueryParam param, CancellationToken cancellation = default);
    }
}