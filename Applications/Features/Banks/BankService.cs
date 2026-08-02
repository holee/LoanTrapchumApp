using Applications.Interfaces.Banks;
using Domains.Models.Currencies;

namespace Applications.Features.Banks
{
    public interface IBankServiceImpl
    {
        Task<bool> DeleteAsync(int id);
        Task<Bank?> GetByIdAsync(int id);
    }


    public sealed class BankService(IBankService bankService) : IBankServiceImpl 
    {
        public async Task<bool> DeleteAsync(int id)
        {
            return await bankService.DeleteAsync(id);
        }


        public async Task<Bank?> GetByIdAsync(int id)
        {
            return await bankService.GetByIdAsync(id);
        }

    }
}
