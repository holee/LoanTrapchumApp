using Domains.Models.Currencies;

namespace Applications.Interfaces.Banks
{
    public interface IBankService
    {
        Task<IEnumerable<Bank?>> GetAllAsync(string? q=null);
        Task<Bank?> GetByIdAsync(int id);
        Task<bool> CreateAsync(Bank bank);
        Task<bool> UpdateAsync(int id, Bank bank);
        Task<bool> DeleteAsync(int id);
    }
}
