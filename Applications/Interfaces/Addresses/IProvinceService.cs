using Applications.Responses;
using Applications.Requests.Lists.Addreses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Addresses;
using Applications.Responses.ViewModels.Departments;
using Domains.Models.Addresses;
using Domains.Models.Lists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interfaces.Addresses
{
    public interface IProvinceService
    {
        Task<Result<Province>> GetById(int? proId);
        Task<Result<IEnumerable<Province>>> GetAllAsync(int? proId = 0, int take = 0, string? condition = null);
        Task<Pager<ProvinceViewModel>> GetAllPager(QueryParam param);
        Task<Result<string>> CreateAsync(Province province);
        Task<Result<string>> UpdateAsync(Province province);
        Task<Result<string>> DeleteAsync(int? proId);
       
    }
}
