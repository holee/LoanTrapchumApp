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
        Task<Result<Province>> GetById(int? proId,CancellationToken cancellation = default);
        Task<Pager<ProvinceViewModel>> GetAllAsync(QueryParam param,CancellationToken cancellation = default);
        Task<Result<string>> CreateAsync(Province province,CancellationToken cancellation = default);
        Task<Result<string>> UpdateAsync(Province province, CancellationToken cancellation = default);
        Task<Result<string>> DeleteAsync(int? proId, CancellationToken cancellation = default);
       
    }
}
