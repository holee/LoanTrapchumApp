using Applications.Requests.Lists.Addreses;
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
