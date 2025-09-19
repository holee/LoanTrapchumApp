using Applications.Features.Entities;
using Applications.Features.Entities;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.OtherNames;
using Domains.Models.Lists;

namespace Applications.Interfaces.Lists
{
    public interface IEntityNameService
    {
        Task<string> ChangeNameStatus(EntityNameDtoActivate req);
        Task<IEnumerable<MarritalStatus>> GetMarriedStatuses(string? q = null);
        Task<string> AddMarriedStatus(MarritalStatus marrital);
        Task<Pager<VGuarantor>> FindGuarantorsAsync(int pageIndex,int pageSize,string sortColumn,
                                                    bool sortDescending, bool includeInActive = false, 
                                                    string? filter = null);
        Task<IEnumerable<VGuarantor>> FindGuarantorsAsync(long id, bool includeInActive = false, string? filter = null);
    }
}
