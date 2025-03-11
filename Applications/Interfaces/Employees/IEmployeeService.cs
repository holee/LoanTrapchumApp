using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Employees;
using Domains.Models.Lists.Employees;

namespace Applications.Interfaces.Employees
{
    public interface IEmployeeService
    {
        Task<Result<string>> CreateOrUpdateAsync(Employee entityName); 
        Task<Result<string>> DeleteAsync(long id);
        Task<Result<Employee>> GetByIdAsync(long id);
        Task<Result<IReadOnlyList<VEmployees>>> GetEmployeesAsync(long employeeId=0,int branchId=0, bool isActive = false, string? q = null);
        Task<Pager<VEmployees>> GetEmployeesAsync(int pageIndex, int pageSize, string sortColumn, bool isDecending = false, bool includeInActive = false, string? q = null);
        Task<Result<string>> UpdateIsActiveAsync(long id);
    }
}
