using Applications.Interfaces.Lists;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Customers;
using Applications.Responses.ViewModels.Employees;
using Domains.Models.Loans;
using Infrastructures.Data;
using Microsoft.EntityFrameworkCore;

namespace CoreBussiness.Implements
{
    public class CreditAgentService : ICreditAgentService
    {
        private readonly ApplicationDbContext _context;

        public CreditAgentService(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<Result> AddCreditAgent(long employeeId, LoanCreditAgent req)
        {
            try
            {
                if (IsAlreadyCreditAgent(employeeId))
                {
                    var existingCo = await _context.LoanCreditAgents
                                                    .Where(x => x.EmployeeRefId == employeeId)
                                                        .FirstOrDefaultAsync();

                    existingCo!.IsRemove = false;

                    await _context.SaveChangesAsync();

                    return Result.Success("Credit Office was Added");

                }
                var emp = await _context.Employees
                                        .Include(x => x.LoanCreditAgent)
                                        .FirstOrDefaultAsync(x => x.EmployeeId == employeeId);
                if (emp is null)
                    return Result.Failure(ErrorResponse.NotFound("There is no employee selected yet!."));

                emp.AddCreditAgent(req);

                await _context.SaveChangesAsync();

                return Result.Success("Credit Office was Added");
            }
            catch (Exception ex)
            {
                return Result.Failure(ErrorResponse.ServerError(ex.Message));
            }
        }
        public async Task<Result> UpdateCreditAgent(int id, LoanCreditAgent req)
        {
            try
            {
                var emp = await _context.LoanCreditAgents.FirstOrDefaultAsync(x => x.Id == id);
                if (emp is null) return Result.Failure(ErrorResponse.NotFound("There is no employee yet!."));
                _context.Update(req);
                _context.SaveChanges();
                return Result.Success("Credit Agent was Updated");
            }
            catch (Exception ex)
            {
                return Result.Failure(ErrorResponse.ServerError(ex.Message));
            }
        }
        public async Task<Result> ActivateOrDeactivateCreditAgent(LoanCreditAgent req)
        {
            try
            {
                var emp = await _context.LoanCreditAgents.AsNoTracking().FirstOrDefaultAsync(x => x.EmployeeRefId == req.EmployeeRefId && x.EditSeq == req.EditSeq);
                if (emp is null) return Result.Failure(ErrorResponse.NotFound("There is no employee yet Or Someone was changed this data!."));
                _context.Entry(req).Property(x => x.IsRemove).IsModified = true;
                _context.Entry(req).Property(x => x.EditSeq).IsModified = true;
                await _context.SaveChangesAsync();
                return Result.Success("Credit Agent was Updated");
            }
            catch (Exception ex)
            {
                return Result.Failure(ErrorResponse.ServerError(ex.Message));
            }
        }
        public async Task<IEnumerable<VCreditAgent>> GetCreditAgents(bool includeIsRemove = false, string? query = null)
        {
            IQueryable<VCreditAgent> queries = _context.Database.SqlQuery<VCreditAgent>($@"SELECT * FROM VCreditAgents");

            if (!includeIsRemove)
            {
                queries = queries.Where(x => x.IsRemove == false);
            }

            if (!string.IsNullOrWhiteSpace(query))
            {
                queries = queries.Where(x => EF.Functions.Like(x.Name, $"%{query}%") ||
                                             EF.Functions.Like(x.FirstName, $"%{query}%") ||
                                             EF.Functions.Like(x.LastName, $"%{query}%") ||
                                             EF.Functions.Like(x.AltName, $"%{query}%"));
            }
            return await queries.AsNoTracking().ToListAsync();
        }
        public async Task<Pager<VCreditAgent>> GetCreditAgents(int pageIndex,
            int pageSize,
            string sortColumn,
            bool includeIsRemove = false,
            bool sortDescending = false,
            string? query = null)
        {
            IQueryable<VCreditAgent> queries = _context.VCreditAgents
                                                      .OrderBy(x => EF.Property<object>(x, sortColumn));
            if (includeIsRemove)
            {
                queries = queries.IgnoreQueryFilters();
            }

            if (!string.IsNullOrWhiteSpace(query))
            {
                queries = queries.Where(x => EF.Functions.Like(x.Name, $"%{query}%") ||
                                             EF.Functions.Like(x.FirstName, $"%{query}%") ||
                                             EF.Functions.Like(x.LastName, $"%{query}%") ||
                                             EF.Functions.Like(x.BranchId.ToString(), $"%{query}%") ||
                                             EF.Functions.Like(x.AltName, $"%{query}%"));
            }

            if (sortDescending)
            {
                queries = queries.OrderByDescending(x => EF.Property<object>(x, sortColumn));
            }
            var counts = await queries.CountAsync();
            queries = queries.Skip(pageIndex).Take(pageSize);
            return new Pager<VCreditAgent>
            {
                Count = counts,
                Items = await queries.AsNoTracking().ToListAsync()
            };
        }
        public async Task<IEnumerable<VEmployeeCreateCreditAgent>> GetEmployeeCreateCreditAgents(string? query = null)
        {
            IQueryable<VEmployeeCreateCreditAgent> queries = _context.Database.SqlQuery<VEmployeeCreateCreditAgent>(@$"SELECT * FROM VEmployeeCreateCreditAgents")
                                                                                                        .AsQueryable();
            if (!string.IsNullOrWhiteSpace(query))
            {
                queries = queries.Where(x => EF.Functions.Like(x.FirstName, $"${query}%") ||
                                             EF.Functions.Like(x.LastName, $"%{query}%") ||
                                             EF.Functions.Like(x.AltName, $"%{query}%") ||
                                             EF.Functions.Like(x.Name, $"%{query}%"))
                                  .OrderByDescending(x => x.FirstName)
                                  .ThenByDescending(x => x.FirstName);
            }
            return await queries.AsNoTracking().ToListAsync();
        }
        private bool IsAlreadyCreditAgent(long empId)
        {
            return _context.LoanCreditAgents
                            .Where(x => x.EmployeeRefId == empId)
                                .Any();
        }

    }
}

