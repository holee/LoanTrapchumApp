using Applications.Interfaces.Banks;
using Domains.Models.Currencies;
using Infrastructures.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Implements
{
    public class BankService : IBankService
    {
        private readonly ApplicationDbContext _context;

        public BankService(ApplicationDbContext context)
        {
            _context = context;
        }
        StringComparison Sc { get; set; } = StringComparison.InvariantCultureIgnoreCase;
        public async Task<bool> CreateAsync(Bank bank)
        {
            _context.Entry(bank).State = EntityState.Added;
            await Save();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var result = await _context.Banks.AsNoTracking().FirstOrDefaultAsync(x => x.BankId == id);
            if (result is null) return false;
            _context.Banks.Remove(result!);
            await Save();
            return true;
        }

        public async Task<IEnumerable<Bank?>> GetAllAsync(string? q = null)
        {
            IQueryable<Bank> results = _context.Banks.AsNoTracking();
            if (q != null)
            {
                results = results.Where(x => x.BankCode.ToLower().Contains(q.ToLower(), Sc)
                || x.BankName.ToLower().StartsWith(q.ToLower(), Sc)
                || x.AltPhone!.ToLower().Contains(q.ToLower(), Sc)
                || x.Email!.ToLower().Contains(q, Sc)
                );
            }
            return await results.ToListAsync() ?? Enumerable.Empty<Bank>();
        }

        public async Task<Bank?> GetByIdAsync(int id)
        {
            return await _context.Banks.AsNoTracking().FirstOrDefaultAsync(x => x.BankId.Equals(id));
        }

        private async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(int id, Bank bank)
        {
            var result = await _context.Banks.AsNoTracking().FirstOrDefaultAsync(x => x.BankId == id);
            if (result is null) return false;
            _context.Banks.Remove(result!);
            await Save();
            return true;
        }
    }
}
