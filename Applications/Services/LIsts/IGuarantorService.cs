using Applications.Interfaces.Lists;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.OtherNames;
using Domains.Models.Lists;

namespace Applications.Services.Lists;
public sealed class GuarantorService(IGuarantorService gService) : IGuarantorService
{
    public async Task<Result<string>> CreateGaurantorAsync(EntityName othername)
    {
        return await gService.CreateGaurantorAsync(othername);
    }

    public async Task<Result<string>> DeleteGaurantorAsync(long id)
    {
        return await gService.DeleteGaurantorAsync(id);
    }

    public async Task<Result<EntityName>> GetGuarantorByIdAsync(long id)
    {
        return await gService.GetGuarantorByIdAsync(id);
    }

    public async Task<Result<VGuarantor>> GetGuarantorByIdAsync(long id, bool allGuarantors = false, bool include = true)
    {
        return await gService.GetGuarantorByIdAsync(id, allGuarantors, include);
    }

    public async Task<Result<Pager<VGuarantor>>> GetGuarantorsAsync(QueryParam param)
    {
       return await gService.GetGuarantorsAsync(param);
    }

    public async Task<Result<IEnumerable<VGuarantor>>> GetGuarantorsAsync(long id = 0, bool allGuarantors = true, string? q = null)
    {
        return await gService.GetGuarantorsAsync(id, allGuarantors, q);
    }

    public async Task<Result<string>> UpdateGaurantorAsync(EntityName othername)
    {
        return await gService.UpdateGaurantorAsync(othername);
    }
}