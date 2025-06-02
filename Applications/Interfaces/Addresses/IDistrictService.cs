using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Addresses;
using Domains.Models.Addresses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interfaces.Addresses
{
    public interface IDistrictService
    {
        Task<Result<District>> GetByIdAsync(int? Id);
        Task<Result<IEnumerable<VDistricts>>> GetAllAsync(int? proId = 0, int take = 0, string? condition = null);
        Task<Pager<VDistricts>> GetAllPagerAsync(QueryParam param);
        Task<Result<string>> CreateAsync(District model);
        Task<Result<string>> UpdateAsync(District model);
        Task<Result<string>> DeleteAsync(int? Id);
    }
}
