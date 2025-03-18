using Applications.Interfaces.Lists;
using Applications.Requests.Lists.Entities;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.OtherNames;
using Domains.Models.Lists;

namespace Infrastructures.Implements
{
    public class EntityNameService : IEntityNameService
    {
        public Task<string> AddMarriedStatus(MarritalStatus marrital)
        {
            throw new NotImplementedException();
        }

        public Task<string> ChangeNameStatus(EntityNameDtoActivate req)
        {
            throw new NotImplementedException();
        }

        public Task<Pager<VGuarantor>> FindGuarantorsAsync(int pageIndex, int pageSize, string sortColumn, bool sortDescending, bool includeInActive = false, string? filter = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<VGuarantor>> FindGuarantorsAsync(long id, bool includeInActive = false, string? filter = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MarritalStatus>> GetMarriedStatuses(string? q = null)
        {
            throw new NotImplementedException();
        }
    }
}
