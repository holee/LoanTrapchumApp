using Applications.Interfaces.Banks;
using Domains.Models.Currencies;

namespace Applications.Services.Banks
{
    public interface IBankServiceImpl
    {
        Task<bool> CreateAsync(Bank bank);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Bank?>> GetAllAsync(string? q = null);
        Task<Bank?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, Bank bank);
    }


    public sealed class BankService(IBankService bankService) : IBankServiceImpl 
    {
        public async Task<bool> CreateAsync(Bank bank)
        {
            return await bankService.CreateAsync(bank);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await bankService.DeleteAsync(id);
        }

        public async Task<IEnumerable<Bank?>> GetAllAsync(string? q = null)
        {
            return await bankService.GetAllAsync(q);
        }

        public async Task<Bank?> GetByIdAsync(int id)
        {
            return await bankService.GetByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(int id, Bank bank)
        {
            return await bankService.UpdateAsync(id, bank);
        }
    }
}
