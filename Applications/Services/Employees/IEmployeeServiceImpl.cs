using Applications.Requests.Lists.OtherNames;
using Applications.Responses;
using Applications.Responses.Lists.Entities;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Employees;

namespace Applications.Services.Employees
{
    public interface IEmployeeServiceImpl
    {
        Task<Result<string>> CreateAsync(EntityNamesCreate entityName);
        Task<Result<string>> UpdateAsync(EntityNamesCreate entityName);
        Task<Result<string>> DeleteAsync(string? employeeId);
        Task<Result<string>> UpdateIsActiveAsync(string? employeeId);
        Task<IEnumerable<OcupationResponse>> GetOccupationsAsync(string? filter);
        Task<Result<EntityNamesCreate>> GetEmployeeForUpdate(string? employeeId);
        Task<Result<EntityNames>> GetByIdAsync(string? employeeId);
        Task<Result<VEmployeeList>> GetEmployeeById(string? employeeId, int? branchId, bool include = false);
        Task<Pager<VEmployeeList>> GetEmployeesAsync(int? branchId,string? employeeId,QueryParam param);
    }
}