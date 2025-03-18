using Applications.Interfaces.Lists;
using Applications.Responses;
using Domains.Models.Lists;

namespace Applications.Services.Lists
{
    public sealed class SalutationService(ISalutationService sService) : ISalutationService
    {
        public async Task<Result<string>> AddSalution(Salutation salutation)
        {
           return await sService.AddSalution(salutation);
        }

        public async Task<Result<string>> DeleteSalution(int id)
        {
            return await sService.DeleteSalution(id);
        }

        public async Task<Result<IEnumerable<Salutation>>> GetSalutions(string? q = null)
        {
            return await sService.GetSalutions(q);    
        }
    }
}
