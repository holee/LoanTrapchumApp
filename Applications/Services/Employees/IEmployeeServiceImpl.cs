using Applications.Requests.Lists.Employees;
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
        Task<Result<string>> DeleteAsync(string? employeeId);
        Task<Result<EntityNames>> GetByIdAsync(string? employeeId);

        Task<Result<IReadOnlyList<VEmployeeList>>> GetEmployeesAsync(string? employeeId, int branchId, bool isActive = false,string? q = null);

        Task<Pager<VEmployeeList>> GetEmployeesAsync(QueryParam param);
        Task<Result<string>> UpdateIsActiveAsync(string? employeeId);
        Task<IEnumerable<OcupationResponse>> GetOccupationsAsync(string? filter);
        Task<Result<EntityNamesCreate>> GetEmployeeForUpdate(string? employeeId);
        Task<Result<T>> GetEmployeeById<T>(string? employeeId) where T:class;
        Task<Result<VEmployeeList>> GetEmployeeById(string? employeeId, int? branchId, bool include = false);
    }
}