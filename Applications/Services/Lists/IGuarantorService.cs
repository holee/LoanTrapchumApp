using Applications.Interfaces.Lists;
using Applications.Requests.Lists.Entities;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.OtherNames;
using Domains.Models.Lists;
using Mapster;

namespace Applications.Services.Lists;

public interface IGuarantorServiceImpl
{
    Task<Result<string>> CreateAsync(EntityNameDtoCreate othername);
    Task<Result<string>> DeleteAsync(Guid? id);
    Task<Result<EntityName>> GetGuarantorByIdAsync(Guid? id);
    Task<Result<EntityNameDtoUpdate>> GetGuarantorEditAsync(Guid? id);
    Task<Result<VGuarantor>> GetGuarantorByIdAsync(Guid? id, bool allGuarantors = false, bool include = true);
    Task<Result<IEnumerable<VGuarantor>>> GetGuarantorsAsync(Guid? id, bool allGuarantors = true, string? q = null);
    Task<Result<Pager<VGuarantor>>> GetGuarantorsAsync(QueryParam param);
    Task<Result<string>> UpdateAsync(EntityNameDtoUpdate othername);
}


public sealed class GuarantorService(IGuarantorService gService) : IGuarantorServiceImpl
{
    public async Task<Result<string>> CreateAsync(EntityNameDtoCreate othername)
    {
        var req = othername.Adapt<EntityName>();

        return await gService.CreateGaurantorAsync(req);
    }

    public async Task<Result<string>> UpdateAsync(EntityNameDtoUpdate othername)
    {

        var req = othername.Adapt<EntityName>();

        return await gService.UpdateGaurantorAsync(req);
    }

    public async Task<Result<string>> DeleteAsync(Guid? id)
    {
        return await gService.DeleteGaurantorAsync(id);
    }

    public async Task<Result<EntityName>> GetGuarantorByIdAsync(Guid? id)
    {
        return await gService.GetGuarantorByIdAsync(id);
    }

    public async Task<Result<VGuarantor>> GetGuarantorByIdAsync(Guid? id, bool allGuarantors = false, bool include = true)
    {
        return await gService.GetGuarantorByIdAsync(id, allGuarantors, include);
    }

    public async Task<Result<Pager<VGuarantor>>> GetGuarantorsAsync(QueryParam param)
    {
        return await gService.GetGuarantorsAsync(param);
    }

    public async Task<Result<IEnumerable<VGuarantor>>> GetGuarantorsAsync(Guid? id, bool allGuarantors = true, string? q = null)
    {
        return await gService.GetGuarantorsAsync(id, allGuarantors, q);
    }

    public Task<Result<EntityNameDtoUpdate>> GetGuarantorEditAsync(Guid? id)
    {
        throw new NotImplementedException();
    }
}