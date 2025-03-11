using Applications.Interfaces.Employees;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Employees;
using Domains.Models.Lists.Employees;

namespace Applications.Services.Employees
{
    public class EmployeeService : IEmployeeService
    {

        private readonly IEmployeeService _emp;

        public EmployeeService(IEmployeeService employeeService)
        {
            _emp = employeeService;
        }

        public async Task<Result<string>> CreateOrUpdateAsync(Employee entityName)
        {
            return await _emp.CreateOrUpdateAsync(entityName);
        }

        public async Task<Result<string>> DeleteAsync(long id)
        {
            return await _emp.DeleteAsync(id);
        }

        public async Task<Result<Employee>> GetByIdAsync(long id)
        {
            return await _emp.GetByIdAsync(id);
        }

        public async Task<Result<IReadOnlyList<VEmployees>>> GetEmployeesAsync(long employeeId = 0, int branchId = 0, bool isActive = false, string? q = null)
        {
            return await _emp.GetEmployeesAsync(employeeId, branchId, isActive, q);
        }

        public async Task<Pager<VEmployees>> GetEmployeesAsync(int pageIndex, int pageSize, string sortColumn, bool isDecending = false, bool includeInActive = false, string? q = null)
        {
            return await _emp.GetEmployeesAsync(pageIndex, pageSize, sortColumn, isDecending, includeInActive, q);
        }

        public async Task<Result<string>> UpdateIsActiveAsync(long id)
        {
            return await _emp.UpdateIsActiveAsync(id);
        }
    }
}
