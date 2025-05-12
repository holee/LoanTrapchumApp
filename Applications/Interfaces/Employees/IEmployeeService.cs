using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Employees;
using Domains.Models.Lists.Employees;

namespace Applications.Interfaces.Employees
{
    public interface IEmployeeService
    {
        Task<Result<string>> DeleteAsync(string? id);
        Task<Result<EntityNames>> GetByIdAsync(string? id);
        Task<Result<IReadOnlyList<VEmployeeList>>> GetEmployeesAsync(string? employeeId,int branchId=0, bool isActive = false, string? q = null);
        Task<Pager<VEmployeeList>> GetEmployeesAsync(QueryParam q);
        Task<Pager<VEmployeeList>> GetEmployeesAsync(int? branchId,QueryParam q); 
        Task<Result<string>> UpdateIsActiveAsync(string? id);
        Task<Result<string>> CreateAsync(EntityNames employee);
        Task<Result<string>> UpdateAsync(EntityNames employee);
        Task<Result<T>> GetByIdAsync<T>(string? id) where T : class;
        Task<Result<VEmployeeList>> GetByIdAsync(string? id, int? branchId, bool include);
    }
}
