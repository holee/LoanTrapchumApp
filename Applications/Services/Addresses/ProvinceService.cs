using Applications.Interfaces.Addresses;
using Applications.Requests.Lists.Addreses;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Addresses;
using Domains.Models.Addresses;
using Domains.Models.Lists;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Services.Addresses
{
    //public class ProvinceService(IProvinceService proService) : IProvinceService
    //{
    //    public async Task<Result<string>> UpsertAsync(ProvinceDtoCreate province)
    //    {
    //        var pro = province.Adapt<ProvinceDtoCreate>();
    //        return await proService.UpsertAsync(pro);
    //    }

    //    public async Task<Result<string>> DeleteAsync(int? proId)
    //    {
    //        return await proService.DeleteAsync(proId);
    //    }

    //    public async Task<Result<IEnumerable<Province>>> GetAllAsync(int? proId = 0, int take = 0, string? condition = null)
    //    {
    //        return await proService.GetAllAsync(proId, take, condition);
    //    }

    //    public async Task<Pager<ProvinceViewModel>> GetAllPager(QueryParam param)
    //    {
    //        return await proService.GetAllPager(param);
    //    }

    //    public async Task<Result<Province>> GetById(int? proId)
    //    {
    //        return await proService.GetById(proId);
    //    }

    //    public async Task<Result<string>> UpdateAsync(Province province)
    //    {
    //        return await proService.UpdateAsync(province);
    //    }
    //}
}
