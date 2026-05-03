
using Applications.Extensions;
using Applications.Interfaces.Lists;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Customers;
using Applications.Responses.ViewModels.Employees;
using Domains.Models.Lists.CreditAgents;
using Domains.Models.Loans;
using Infrastructures.DataAccess;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

                    await context.SaveChangesAsync();

                    return Result.Success("Credit Office was Added");

                }
                var emp = await context.Employees
                                        .Include(x => x.LoanCreditAgent)
                                        .FirstOrDefaultAsync(x => x.EmployeeId == employeeRefId);
                if (emp is null)
                    return Result.Failure<string>(ErrorResponse.NotFound("There is no employee selected yet!."));

                emp.LoanCreditAgent = req;

                await context.SaveChangesAsync();

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
        public async Task<Result<string>> ChangeCreditAgentState(LoanCreditAgent req)
        {
            try
            {
                await using var context = await _dbContextFactory.CreateDbContextAsync();

                var emp = await context.LoanCreditAgents
                                .AsNoTracking()
                                .FirstOrDefaultAsync(x => x.EmployeeRefId == req.EmployeeRefId &&
                                                          x.EditSeq == req.EditSeq);

                if (emp is null) return Result.Failure<string>(ErrorResponse.NotFound("There is no employee yet Or Someone was changed this data!."));

                context.Entry(req).Property(x => x.IsRemove).IsModified = true;

                context.Entry(req).Property(x => x.EditSeq).IsModified = true;

                await context.SaveChangesAsync();

                return Result.Success("Credit Agent was Updated");
            }
            catch (DbUpdateException ex)
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
                                                      .WhereIf(!string.IsNullOrEmpty(query), x =>
                                                                                                  EF.Functions.Like(x.FirstName, $"%{query}%") ||
                                                                                                  EF.Functions.Like(x.LastName, $"%{query}%") ||
                                                                                                  EF.Functions.Like(x.AltName, $"%{query}%"))
                                                      .OrderByDescending(x => x.FirstName)
                                                            .ThenByDescending(x => x.LastName)
                                                      .AsNoTracking()
                                                      .ToArrayAsync();


            return Result.Success<IEnumerable<VCreditAgent>>(queries);


        }

        public async Task<Pager<VCreditAgent>> GetCreditAgents(int? branchId, QueryParam param, CancellationToken cancellation = default)
        {

            await using var context = await _dbContextFactory.CreateDbContextAsync(cancellation);

            var queries = context.VCreditAgents
                                                        .WhereIf(branchId > 0, x => x.BranchId == branchId)
                                                        .OrderByDescending(x => x.FirstName)
                                                        .ThenByDescending(x => x.LastName)
                                                        .AsNoTracking();

            if (param.IncludeInActive)
            {
                queries = queries.IgnoreQueryFilters();
            }

            if (!string.IsNullOrWhiteSpace(param.Filter))
            {
                queries = queries.Where(x => EF.Functions.Like(x.FirstName!, $"%{param.Filter}%"))
                                 .OrWhere(x => EF.Functions.Like(x.BranchName!, $"%{param.Filter}%"))
                                 .OrWhere(x => EF.Functions.Like(x.AltName!, $"%{param.Filter}%"))
                                 .OrWhere(x => EF.Functions.Like(x.LastName!, $"%{param.Filter}%"));
            }

            if (param.SortDescending)
            {
                queries = queries.OrderByDescending(x => EF.Property<object>(x, param.SortColumn));
            }

            var counts = await queries.CountAsync(cancellation);

            if (param.Take > 0)
            {
                queries = queries.Skip(param.Page).Take(param.Take);

            }

            var items = await queries.AsNoTracking().ToListAsync(cancellation);


            return new Pager<VCreditAgent>
            {
                Count = counts,
                Items = items
            };
        }

        public async Task<Pager<VEmployeeCreditAgent>> GetEmployeeCreditAgents(int? branchId, string? employeeId, QueryParam param, CancellationToken cancellation = default)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();

            var queries = context.EmployeeCreditAgent
                               .WhereIf(branchId.HasValue && branchId > 0, x => x.BranchId == branchId)
                               .WhereIf(employeeId.IsNotNullOrEmpty(), x => x.EmployeeId == employeeId);

            if (param.FilterColumn.IsNotNullOrEmpty())
            {
                queries = param.FilterColumn switch
                {
                    nameof(VEmployeeCreditAgent.AltName) => queries.Where(x => EF.Functions.Like(x.AltName!, $"%{param.Filter}%")),
                    nameof(VEmployeeCreditAgent.BranchName) => queries.Where(x => EF.Functions.Like(x.BranchName!, $"%{param.Filter}%")),
                    nameof(VEmployeeCreditAgent.Phone) => queries.Where(x => EF.Functions.Like(x.Phone!, $"%{param.Filter}%")),
                    nameof(VEmployeeCreditAgent.FirstName) => queries.Where(x => EF.Functions.Like(x.FirstName!, $"%{param.Filter}%")),
                    nameof(VEmployeeCreditAgent.LastName) => queries.Where(x => EF.Functions.Like(x.LastName!, $"%{param.Filter}%")),
                    _ => queries // No filter applied if column is unrecognized
                };
            }
            if (param.Filter.IsNotNullOrEmpty() && param.FilterColumn.IsNullOrEmpty())
            {

                queries = queries.Where(x => EF.Functions.Like(x.FirstName!, $"%{param.Filter}%") ||
                                           EF.Functions.Like(x.LastName!, $"%{param.Filter}%") ||
                                           EF.Functions.Like(x.AltName!, $"%{param.Filter}%") ||
                                           EF.Functions.Like(x.AltName!, $"%{param.Filter}%"));

            }
            if (param.SortDescending)
            {
                queries = queries.OrderByDescending(x => EF.Property<string>(x, param.SortColumn));
            }


            var counts = await queries.CountAsync(cancellation);

            var items = await queries
                            .Skip(param.Page)
                            .Take(param.Take)
                            .AsNoTracking().ToListAsync(cancellation);

            return new Pager<VEmployeeCreditAgent>(items, counts);
        }

        public async Task<Result<IEnumerable<VEmployeeCreditAgent>>> GetEmployeeCreateCreditAgents(string? filter = null)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();

            var queries = await context.Database
                                .SqlQuery<VEmployeeCreditAgent>($"SELECT * FROM VEmployeeCreateCreditAgents")
                                .WhereIf(!string.IsNullOrEmpty(filter),
                                                                       x => EF.Functions.Like(x.FirstName!, $"%{filter}%") ||
                                                                            EF.Functions.Like(x.LastName!, $"%{filter}%") ||
                                                                            EF.Functions.Like(x.AltName!, $"%{filter}%") ||
                                                                            EF.Functions.Like(x.Name!, $"%{filter}%"))
                                .OrderByDescending(x => x.FirstName)
                                    .ThenByDescending(x => x.LastName)
                                .AsNoTracking()
                                .ToListAsync();

            return Result.Success<IEnumerable<VEmployeeCreditAgent>>(queries);
        }



        public async Task<Result<string>> ChangedCreditAgents(int? branchId, List<LoanChangedCa> req, CancellationToken cancellation = default)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync(cancellation);

            var trx = await context.Database.BeginTransactionAsync(cancellation);

            try
            {

                context.LoanChangedCa.AddRange(req);

                await context.SaveChangesAsync(cancellation);

                var firstRequest = req[0];

                var oldCreditAgentId = firstRequest.OldCreditAgentRefId;

                var newCreditAgentRefId = firstRequest.NewCreditAgentRefId;

                var currentDateTime = DateTime.Now;

                // Build query based on count
                var disbursmentsQuery = context.LoanDisbursments
                                                .Where(x => x.BranchRefId == branchId &&
                                                            x.CreditAgentRefId == oldCreditAgentId);

                // Get relevant LoanIds from request
                var loanIds = req.Select(r => r.LoanRefId).Distinct().ToList();

                // Build query for affected disbursements
                var disbursementsQuery = context.LoanDisbursments
                                                .Where(x => x.BranchRefId == branchId &&
                                                            x.CreditAgentRefId == oldCreditAgentId &&
                                                            loanIds.Contains(x.LoanId));
                // Batch update
                await disbursementsQuery.ExecuteUpdateAsync(prop => prop
                    .SetProperty(p => p.CreditAgentRefId, newCreditAgentRefId)
                    .SetProperty(p => p.ModifiedDt, currentDateTime), cancellation);

                await trx.CommitAsync(cancellation);

                return Result.Success<string>(new("Loan has changed new credit agent."));

            }
            catch (DbUpdateException ex)
            {
                await trx.RollbackAsync(cancellation);

                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }


        }





        private bool IsAlreadyCreditAgent(string? empId)
        {
            return _context.LoanCreditAgents
                                .IgnoreQueryFilters()
                                .Any(x => x.EmployeeRefId == empId);
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