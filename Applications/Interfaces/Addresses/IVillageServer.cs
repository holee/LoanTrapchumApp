using Applications.Features.Addresses.Dto;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Addresses;
using Domains.Models.Addresses;
namespace Applications.Interfaces.Addresses
{
    public interface IVillageServer 
    {
        Task<Result<Village>> GetByIdAsync(int? Id);
        Task<Result<VVillageDetails>> GetDettailsAsync(int? Id);
        Task<Result<IEnumerable<Village>>> GetAllAsync(int? proId = 0, int take = 0, string? condition = null);
        Task<Pager<VVillages>> GetAllPagerAsync(QueryParam param);
        Task<Result<string>> CreateAsync(VillageDtoCreate model);
        Task<Result<string>> UpdateAsync(VillageDtoUpdate model);
        Task<Result<string>> DeleteAsync(int? Id);
    }
}
