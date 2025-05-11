using Applications.Responses;
using Domains.Models.Lists;

namespace Applications.Services.Lists;

public interface ISalutationServiceImpl
{
    Task<Result<string>> AddSalution(Salutation salutation);
    Task<Result<string>> DeleteSalution(int id);
    Task<Result<IEnumerable<Salutation>>> GetSalutions(string? q = null);
    Task<Result<IEnumerable<string>>> GetsSalutions(string? q = null); 
}