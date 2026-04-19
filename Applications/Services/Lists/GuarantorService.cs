using Applications.Commons.Mappers;
using Applications.Features.Guarantor.Dtos;
using Applications.Features.OtherNames.Dto;
using Applications.Interfaces.Lists;
using Applications.Responses;
using Applications.Responses.Paginations;
using Domains.Models.Lists;
using Mapster;

namespace Applications.Services.Lists;

public sealed class GuarantorService(IGuarantorService gService) : IGuarantorServiceImpl
{
    public async Task<Result<string>> CreateAsync(EntityNamesCreate othername)
    {

        var req = othername.ToGuarantorCreate();

        return await gService.CreateAsync(req);
    }

    public async Task<Result<string>> CreateAsync(EntityGuarantorCreate othername)
    {

        var req = othername.MapGuarantorCreate();

        return await gService.CreateAsync(req);
    }

    public async Task<Result<string>> UpdateAsync(EntityNamesCreate othername)
    {

        var req = othername.ToGuarantorEdit();

        return await gService.UpdateAsync(req);
    }

    public async Task<Result<string>> UpdateAsync(EntityGuarantorCreate othername)
    {

        var req = othername.MapGuarantorEdit();

        return await gService.UpdateAsync(req);
    }

    public async Task<Result<string>> DeleteAsync(string? id)
    {
        return await gService.DeleteAsync(id);
    }

    public async Task<Result<EntityNames>> GetGuarantorByIdAsync(string? id)
    {
        return await gService.GetGuarantorByIdAsync(id);
    }

    public async Task<Result<VGuarantor>> GetGuarantorByIdAsync(string? id, bool allGuarantors = false, bool include = true)
    {
        return await gService.GetGuarantorByIdAsync(id, allGuarantors, include);
    }

    public async Task<Result<VGuarantor>> GetGuarantorWithByIdAsync(string? id) 
    {
        return await gService.GetGuarantorWithByIdAsync(id);
    }

    public async Task<Result<VGuarantor>> GetGuarantorByIdAsync(string? id,CancellationToken cancellation=default)
    {
        return await gService.GetGuarantorByIdAsync(id,cancellation); 
    }


    public async Task<Pager<VGuarantor>> GetGuarantorsAsync(QueryParam param,CancellationToken cancellation=default)
    {
        return await gService.GetGuarantorsAsync(param,cancellation);
    }
    public async Task<Result<IEnumerable<VGuarantor>>> GetGuarantorsAsync(string? id, bool allGuarantors = true, string? q = null)
    {
        return await gService.GetGuarantorsAsync(id, allGuarantors, q);
    }
    public async Task<Pager<VGuarantor>> GetGuarantorsAsync(string? id, QueryParam q)
    {
        return await gService.GetGuarantorsAsync(id, q);
    }
    public async Task<Result<EntityGuarantorCreate>> GetGuarantorEditAsync(string? id)
    {

        var rsp = await gService.GetGuarantorByIdAsync(id);

        if (rsp.IsSuccess)
        {

            var result = rsp.Data!.MapGuarantor();

            return Result.Success(result);

        }
        return Result.Failure<EntityGuarantorCreate>(ErrorResponse.NotFound(rsp.ErrMessage));
    }
}