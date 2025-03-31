using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Employees;
using Domains.Models.Lists.Employees;

namespace Applications.Interfaces.Employees
{
    public interface IEmployeeService
    {
        Task<Result<string>> DeleteAsync(Guid? id);
        Task<Result<Employee>> GetByIdAsync(Guid? id);
        Task<Result<IReadOnlyList<VEmployees>>> GetEmployeesAsync(Guid? employeeId,int branchId=0, bool isActive = false, string? q = null);
        Task<Pager<VEmployees>> GetEmployeesAsync(int pageIndex, int pageSize, string sortColumn, bool isDecending = false, bool includeInActive = false, string? q = null);
        Task<Result<string>> UpdateIsActiveAsync(Guid? id);
        Task<Result<string>> CreateAsync(EntityNames employee);
        Task<Result<string>> UpdateAsync(EntityNames employee);
    }
}
