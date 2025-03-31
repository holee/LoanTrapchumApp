using Applications.Requests.Lists.Employees;
using Applications.Requests.Lists.Entities;
using Applications.Requests.Lists.OtherNames;
using Applications.Responses;
using Applications.Responses.Lists.Entities;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Employees;
using Domains.Models.Lists.Employees;

namespace Applications.Services.Employees
{
    public interface IEmployeeServiceImpl
    {
        Task<Result<string>> CreateAsync(EntityNamesCreate entityName);
        Task<Result<string>> UpdateAsync(EntityNamesCreate entityName);
        Task<Result<string>> DeleteAsync(Guid? employeeId);
        Task<Result<Employee>> GetByIdAsync(Guid? employeeId);

        Task<Result<IReadOnlyList<VEmployees>>> GetEmployeesAsync(Guid? employeeId, int branchId, bool isActive = false,
            string? q = null);

        Task<Pager<VEmployees>> GetEmployeesAsync(QueryParam param);
        Task<Result<string>> UpdateIsActiveAsync(Guid? employeeId);
        Task<IEnumerable<OcupationResponse>> GetOccupationsAsync(string? filter);
        Task<Result<EmployeeDtoUpdate>> GetEmployeeForUpdate(Guid? employeeId);
        Task<Result<EmployeeDto>> GetEmployeeById(Guid? employeeId);
    }
}