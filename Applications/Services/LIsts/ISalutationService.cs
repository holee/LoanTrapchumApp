using Applications.Interfaces.Lists;
using Applications.Responses;
using Domains.Models.Lists;

namespace Applications.Services.Lists
{
    public sealed class SalutationService : ISalutationService
    {
        public Task<Result<string>> AddSalution(Salutation salutation)
        {
            throw new NotImplementedException();
        }

        public Task<Result<string>> DeleteSalution(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<IEnumerable<Salutation>>> GetSalutions(string? q = null)
        {
            throw new NotImplementedException();
        }
    }
}
