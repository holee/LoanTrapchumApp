using Application.Commons.Mappers;
using Application.Features.Guarantor.Dtos;
using Application.Features.OtherNames.Dto;
using Application.Interfaces.Lists;
using Application.Responses.Paginations;
using Domains.Models.Lists;
using ErrorOr;
using Mapster;

namespace Application.Services.Lists;

public sealed class GuarantorService(IGuarantorService gService) : IGuarantorServiceImpl
{
    //public async Task<ErrorOr<Created>> CreateAsync(EntityNamesCreate othername)
    //{

    //    var req = othername.ToGuarantorCreate();

    //    return await gService.CreateAsync(req);
    //}

    //public async Task<ErrorOr<VGuarantor>> CreateAsync(EntityGuarantorCreate othername)
    //{

    //    var req = othername.MapGuarantorCreate();

    //    return await gService.CreateGuarantorAsync(req);
    //}

    //public async Task<ErrorOr<Updated>> UpdateAsync(EntityNamesCreate othername)
    //{

    //    var req = othername.ToGuarantorEdit();

    //    return await gService.UpdateAsync(req);
    //}

    //public async Task<ErrorOr<VGuarantor>> UpdateAsync(EntityGuarantorCreate othername)
    //{

    //    var req = othername.MapGuarantorEdit();

    //    var updateResult = await gService.UpdateAsync(req);

    //    if (updateResult.IsError)
    //    {
    //        return updateResult.Errors;
    //    }

    //    return await gService.GetGuarantorByIdAsync(req.Id, allGuarantors: false, include: true);
    //}

    public async Task<ErrorOr<Deleted>> DeleteAsync(string? id)
    {
        return await gService.DeleteAsync(id);
    }

    //public async Task<ErrorOr<EntityNames>> GetGuarantorByIdAsync(string? id)
    //{
    //    return await gService.GetGuarantorByIdAsync(id);
    //}

    //public async Task<ErrorOr<VGuarantor>> GetGuarantorByIdAsync(string? id, bool allGuarantors = false, bool include = true)
    //{
    //    return await gService.GetGuarantorByIdAsync(id, allGuarantors, include);
    //}

    //public async Task<ErrorOr<VGuarantor>> GetGuarantorWithByIdAsync(string? id)
    //{
    //    return await gService.GetGuarantorWithByIdAsync(id);
    //}

    //public async Task<ErrorOr<VGuarantor>> GetGuarantorByIdAsync(string? id, CancellationToken cancellation = default)
    //{
    //    return await gService.GetGuarantorByIdAsync(id, cancellation);
    //}


    //public async Task<Pager<VGuarantor>> GetGuarantorsAsync(QueryParam param,CancellationToken cancellation=default)
    //{
    //    return await gService.GetGuarantorsAsync(param,cancellation);
    //}
    //public async Task<ErrorOr<IEnumerable<VGuarantor>>> GetGuarantorsAsync(string? id, bool allGuarantors = true, string? q = null)
    //{
    //    return await gService.GetGuarantorsAsync(id, allGuarantors, q);
    //}
    //public async Task<Pager<VGuarantor>> GetGuarantorsAsync(string? id, QueryParam q)
    //{
    //    return await gService.GetGuarantorsAsync(id, q);
    //}
    public async Task<ErrorOr<EntityGuarantorCreate>> GetGuarantorEditAsync(string? id)
    {

        var rsp = await gService.GetByIdAsync(id);

        if (!rsp.IsError)
        {

            var result = rsp.Value.MapGuarantor();

            return result;

        }
        return rsp.Errors;
    }
}