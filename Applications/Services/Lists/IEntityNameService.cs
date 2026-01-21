using Applications.Features.Entities;
using Applications.Features.Guarantor.Dtos;
using Applications.Interfaces.Lists;
using Applications.Responses.Paginations;
using Domains.Models.Lists;

namespace Applications.Services.Lists
{

    public interface IEntityNameServiceImpl
    {
        Task<string> AddMarriedStatus(MarritalStatus marrital);
        Task<string> ChangeNameStatus(EntityNameDtoActivate req);
        Task<Pager<VGuarantor>> FindGuarantorsAsync(int pageIndex, int pageSize, string sortColumn, bool sortDescending, bool includeInActive = false, string? filter = null);
        Task<IEnumerable<VGuarantor>> FindGuarantorsAsync(long id, bool includeInActive = false, string? filter = null);
        Task<IEnumerable<MarritalStatus>> GetMarriedStatuses(string? q = null);
    }
    public sealed class EntityNameService(IEntityNameService entService) : IEntityNameServiceImpl
    {
        public async Task<string> AddMarriedStatus(MarritalStatus marrital)
        {
            return await entService.AddMarriedStatus(marrital);
        }

        public async Task<string> ChangeNameStatus(EntityNameDtoActivate req)
        {
            return await entService.ChangeNameStatus(req);
        }

        public async Task<Pager<VGuarantor>> FindGuarantorsAsync(int pageIndex, int pageSize, string sortColumn, bool sortDescending, bool includeInActive = false, string? filter = null)
        {
            return await entService.FindGuarantorsAsync(pageIndex, pageSize, sortColumn, sortDescending, includeInActive, filter);
        }

        public async Task<IEnumerable<VGuarantor>> FindGuarantorsAsync(long id, bool includeInActive = false, string? filter = null)
        {
            return await entService.FindGuarantorsAsync(id, includeInActive, filter);
        }

        public async Task<IEnumerable<MarritalStatus>> GetMarriedStatuses(string? q = null)
        {
            return await entService.GetMarriedStatuses(q);
        }
    }
}
