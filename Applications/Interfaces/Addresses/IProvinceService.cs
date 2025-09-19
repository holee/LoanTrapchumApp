using Applications.Responses;
using Applications.Responses.Paginations; 
using Applications.Responses.ViewModels.Addresses;
using Domains.Models.Addresses;

namespace Applications.Interfaces.Addresses
{
    public interface IProvinceService
    {
        Task<Result<Province>> GetById(int? proId,CancellationToken cancellation = default);
        Task<Pager<ProvinceViewModel>> GetAllAsync(QueryParam param,CancellationToken cancellation = default);
        Task<Result<string>> CreateAsync(Province province,CancellationToken cancellation = default);
        Task<Result<string>> UpdateAsync(Province province, CancellationToken cancellation = default);
        Task<Result<string>> DeleteAsync(int? proId, CancellationToken cancellation = default);
       
    }
}
