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
    public interface ICommuneService
    {
        Task<Result<Commune>> GetByIdAsync(int? Id);
        Task<Result<IEnumerable<Commune>>> GetAllAsync(int? proId = 0, int take = 0, string? condition = null);
        Task<Pager<VCommunes>> GetAllPagerAsync(QueryParam param);
        Task<Result<string>> CreateAsync(CommuneDtoCreate model);
        Task<Result<string>> UpdateAsync(CommuneDtoUpdate model);
        Task<Result<string>> DeleteAsync(int? Id);
    }
}
