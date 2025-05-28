using Applications.Extensions;
using Applications.Interfaces.Lists;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Customers;
using Applications.Responses.ViewModels.Employees;
using Domains.Models.Loans;
using Infrastructures.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Implements
{
    public class CreditAgentService : ICreditAgentService
    {
        private readonly DataContext _context;
        private readonly IDapperConnectionFactory _connectionFactory;
        private readonly IDbContextFactory<DataContext> _dbContextFactory;
        public CreditAgentService(DataContext context, IDapperConnectionFactory connectionFactory, IDbContextFactory<DataContext> dbContextFactory)
        {
            _context = context;
            _connectionFactory = connectionFactory;
            _dbContextFactory = dbContextFactory;
        }

        public async Task<Result<string>> AddCreditAgent(string? employeeRefId, LoanCreditAgent req)
        {
            try
            {
                await using var context = await _dbContextFactory.CreateDbContextAsync();

                if (IsAlreadyCreditAgent(employeeRefId))
                {
                    var existingCo = await context.LoanCreditAgents
                                                    .Where(x => x.EmployeeRefId == employeeRefId)
                                                    .FirstOrDefaultAsync();

                    existingCo!.IsRemove = false;

                    await _context.SaveChangesAsync();

                    return Result.Success("Credit Office was Added");

                }
                var emp = await context.Employees
                                        .Include(x => x.LoanCreditAgent)
                                        .FirstOrDefaultAsync(x => x.EmployeeId == employeeRefId);
                if (emp is null)
                    return Result.Failure<string>(ErrorResponse.NotFound("There is no employee selected yet!."));

                emp.LoanCreditAgent = req;

                await _context.SaveChangesAsync();

                return Result.Success("Credit Office was Added");
            }
            catch (Exception ex)
            {
                return Result.Failure<string>(ErrorResponse.ServerError(ex.Message));
            }
        }
        public async Task<Result> UpdateCreditAgent(string? id, LoanCreditAgent req)
        {
            try
            {
                await using var context = await _dbContextFactory.CreateDbContextAsync();


                var emp = await context.LoanCreditAgents.FirstOrDefaultAsync(x => x.EmployeeRefId == id);

                if (emp is null) return Result.Failure(ErrorResponse.NotFound("There is no employee yet!."));

                context.Update(req);

                context.SaveChanges();

                return Result.Success("Credit Agent was Updated");
            }
            catch (Exception ex)
            {
                return Result.Failure(ErrorResponse.ServerError(ex.Message));
            }
        }
        public async Task<Result<string>> ActivateOrDeactivateCreditAgent(LoanCreditAgent req)
        {
            try
            {
                await using var context = await _dbContextFactory.CreateDbContextAsync();

                var emp = await context.LoanCreditAgents
                                .AsNoTracking()
                                .FirstOrDefaultAsync(x => x.EmployeeRefId == req.EmployeeRefId && x.EditSeq == req.EditSeq);
                if (emp is null) return Result.Failure<string>(ErrorResponse.NotFound("There is no employee yet Or Someone was changed this data!."));
               
                context.Entry(req).Property(x => x.IsRemove).IsModified = true;

                context.Entry(req).Property(x => x.EditSeq).IsModified = true;

                await _context.SaveChangesAsync();

                return Result.Success("Credit Agent was Updated");
            }
            catch (Exception ex)
            {
                return Result.Failure<string>(ErrorResponse.ServerError(ex.Message));
            }
        }
        public async Task<Result<IEnumerable<VCreditAgent>>> GetCreditAgents(bool includeIsRemove = false, string? query = null)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();

            var queries = await context.Database
                                                      .SqlQuery<VCreditAgent>($@"SELECT * FROM VCreditAgents")
                                                      .WhereIf(!includeIsRemove, x => x.IsRemove == false)
                                                      .WhereIf(!string.IsNullOrEmpty(query), x => EF.Functions.Like(x.Name, $"%{query}%") ||
                                                                                                  EF.Functions.Like(x.FirstName, $"%{query}%") ||
                                                                                                  EF.Functions.Like(x.LastName, $"%{query}%") ||
                                                                                                  EF.Functions.Like(x.AltName, $"%{query}%"))
                                                      .OrderByDescending(x => x.FirstName)
                                                            .ThenByDescending(x => x.LastName)
                                                      .AsNoTracking()
                                                      .ToArrayAsync();


            return Result.Success<IEnumerable<VCreditAgent>>(queries);


        }

        public async Task<Pager<VCreditAgent>> GetCreditAgents(int? branchId, QueryParam param)
        {

            await using var context = await _dbContextFactory.CreateDbContextAsync();

            IQueryable<VCreditAgent> queries = context.VCreditAgents
                                                        .WhereIf(branchId > 0, x => x.BranchId == branchId);

            if (param.IsIncludeInActive)
            {
                queries = queries.IgnoreQueryFilters();
            }

            if (!string.IsNullOrWhiteSpace(param.Filter))
            {
                queries = queries.Where(x => EF.Functions.Like(x.Name!, $"%{param.Filter}%") ||
                                             EF.Functions.Like(x.FirstName!, $"%{param.Filter}%") ||
                                             EF.Functions.Like(x.LastName!, $"%{param.Filter}%") ||
                                             EF.Functions.Like(x.BranchName!, $"%{param.Filter}%") ||
                                             EF.Functions.Like(x.AltName!, $"%{param.Filter}%"));
            }

            if (param.SortDescending)
            {
                queries = queries.OrderByDescending(x => EF.Property<object>(x, param.SortColumn));
            }
            var counts = queries.Count();

            if (param.PageSize > 0)
            {
                queries = queries.Skip(param.Skip).Take(param.PageSize);

            }

            var items = await queries.AsNoTracking().ToListAsync();


            return new Pager<VCreditAgent>
            {
                Count = counts,
                Items = items
            };
        }



        public async Task<Result<IEnumerable<VEmployeeCreateCreditAgent>>> GetEmployeeCreateCreditAgents(string? filter = null)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();

            var queries = await context.Database
                                .SqlQuery<VEmployeeCreateCreditAgent>($"SELECT * FROM VEmployeeCreateCreditAgents")
                                .WhereIf(!string.IsNullOrEmpty(filter),
                                                                       x => EF.Functions.Like(x.FirstName!, $"%{filter}%") ||
                                                                            EF.Functions.Like(x.LastName!, $"%{filter}%") ||
                                                                            EF.Functions.Like(x.AltName!, $"%{filter}%") ||
                                                                            EF.Functions.Like(x.Name!, $"%{filter}%"))
                                .OrderByDescending(x => x.FirstName)
                                    .ThenByDescending(x => x.LastName)
                                .AsNoTracking()
                                .ToListAsync();

            return Result.Success<IEnumerable<VEmployeeCreateCreditAgent>>(queries);
        }
        private bool IsAlreadyCreditAgent(string? empId)
        {
            return _context.LoanCreditAgents
                            .Where(x => x.EmployeeRefId == empId)
                                .Any();
        }

    }
}

//var sql = @"
//            SELECT * FROM VEmployeeCreateCreditAgents WHERE (FirstName LIKE N'%'+@filter+'%' OR 
//            LastName LIKE N'%'+@filter+'%' OR 
//            AltName LIKE N'%'+@filter+'%' OR 
//            Name LIKE N'%'+@filter+'%') OR (@filter is NULL OR @filter=N'')
//            ORDER BY FirstName DESC, LastName DESC
//           ";
//using var db = _connectionFactory.CreateConnection();

//var items = await db.QueryAsync<VEmployeeCreateCreditAgent>(sql, new { filter });

//return items;