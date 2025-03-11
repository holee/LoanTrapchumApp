using Applications.Interfaces.Banks;
using Domains.Models.Currencies;

namespace Applications.Services.Banks
{
    public sealed class BankService : IBankService
    {
        private readonly IBankService _bankService;

        public BankService(IBankService bankService)
        {
            _bankService = bankService;
        }

        public async Task<bool> CreateAsync(Bank bank)
        {
            return await _bankService.CreateAsync(bank);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _bankService.DeleteAsync(id);
        }

        public async Task<IEnumerable<Bank?>> GetAllAsync(string? q = null)
        {
            return await _bankService.GetAllAsync(q);
        }

        public async Task<Bank?> GetByIdAsync(int id)
        {
            return await _bankService.GetByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(int id, Bank bank)
        {
            return await _bankService.UpdateAsync(id, bank);
        }
    }
}
