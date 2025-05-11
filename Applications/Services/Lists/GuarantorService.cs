using Applications.Interfaces.Lists;
using Applications.Requests.Lists.OtherNames;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.OtherNames;
using Mapster;

namespace Applications.Services.Lists;

public sealed class GuarantorService(IGuarantorService gService) : IGuarantorServiceImpl
{
    public async Task<Result<string>> CreateAsync(EntityNamesCreate othername)
    {

        var req = othername.Adapt<EntityNames>();

        return await gService.CreateGaurantorAsync(req);
    }

    public async Task<Result<string>> UpdateAsync(EntityNamesCreate othername)
    {

        var req = othername.Adapt<EntityNames>();

        return await gService.UpdateGaurantorAsync(req);
    }

    public async Task<Result<string>> DeleteAsync(string? id)
    {
        return await gService.DeleteGaurantorAsync(id);
    }

    public async Task<Result<EntityNames>> GetGuarantorByIdAsync(string? id)
    {
        return await gService.GetGuarantorByIdAsync(id);
    }

    public async Task<Result<VGuarantor>> GetGuarantorByIdAsync(string? id, bool allGuarantors = false, bool include = true)
    {
        return await gService.GetGuarantorByIdAsync(id, allGuarantors, include);
    }

    public async Task<Result<Pager<VGuarantor>>> GetGuarantorsAsync(QueryParam param)
    {
        return await gService.GetGuarantorsAsync(param);
    }

    public async Task<Result<IEnumerable<VGuarantor>>> GetGuarantorsAsync(string? id, bool allGuarantors = true, string? q = null)
    {
        return await gService.GetGuarantorsAsync(id, allGuarantors, q);
    }
    public async Task<Pager<VGuarantor>> GetGuarantorsAsync(string? id, QueryParam q)
    {
        return await gService.GetGuarantorsAsync(id, q);
    }
    public async Task<Result<EntityNamesCreate>> GetGuarantorEditAsync(string? id)
    {

        var rsp = await gService.GetGuarantorByIdAsync(id);

        if (rsp.IsSuccess)
        {

            var result = rsp.Data!.Adapt<EntityNamesCreate>();

            return Result.Success(result);

        }
        return Result.Failure<EntityNamesCreate>(ErrorResponse.NotFound(rsp.ErrMessage));
    }
}