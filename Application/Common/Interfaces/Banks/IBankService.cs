using Application.Features.Banks.Dtos;
using Domains.Models.Currencies;

namespace Application.Interfaces.Banks
{
    public interface IBankService
    {
        Task<PageMetaData<BankDto>> GetAllAsync(int? bankId, QueryParam query, CancellationToken cancellation = default);
        Task<Bank?> GetByIdAsync(int? id, CancellationToken cancellation = default);
        Task<BankDto?> UpdateAsync(int? id, Bank bank, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int? id, CancellationToken cancellation = default);
        Task<BankDto> CreateAsync(Bank bank, CancellationToken cancellation = default);
    }
}
