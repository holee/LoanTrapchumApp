using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Employees;
using Domains.Models.Lists;

namespace Applications.Interfaces.Employees
{


    public interface IEmployeeService 
    {
        Task<Result<string>> DeleteAsync(string? id, CancellationToken cancellation = default);
        Task<Result<EntityNames>> GetByIdAsync(string? id);
        Task<Result<IReadOnlyList<VEmployeeList>>> GetEmployeesAsync(string? employeeId,int branchId=0, bool isActive = false, string? q = null);
        Task<Pager<VEmployeeList>> GetEmployeesAsync(int? branchId, string? employeeId, QueryParam q); 
        Task<Result<string>> UpdateIsActiveAsync(string? id, bool state, CancellationToken cancellation = default);
        Task<Result<string>> CreateAsync(EntityNames employee, CancellationToken cancellation = default);
        Task<Result<string>> UpdateAsync(EntityNames employee, CancellationToken cancellation = default);
        Task<Result<T>> GetByIdAsync<T>(string? id) where T : class;
        Task<Result<VEmployeeList>> GetByIdAsync(string? id, int? branchId, bool include);
        Task<Result<VEmployeeList>> GetByIdAsync(string? employeeId, CancellationToken cancellation = default);
        Task<Result<VEmployeeList>> GetEmployeeByIdAsync(string? employeeId, CancellationToken cancellation = default);
    }
}
