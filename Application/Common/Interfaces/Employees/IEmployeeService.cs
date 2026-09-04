using Application.Bases;
using Application.Responses;
using Application.Responses.Paginations;
using Application.Responses.ViewModels.Employees;
using Domains.Models.Lists;
using ErrorOr;

namespace Application.Interfaces.Employees;


public interface IEmployeeService
{
    //Task<Result<string>> DeleteAsync(string? id, CancellationToken cancellation = default);
    Task<ErrorOr<DeleteResult>> DeleteAsync(string? id, int? branchId, CancellationToken cancellation = default);
    Task<ErrorOr<EntityNames>> GetByIdAsync(string? id, int? branchId, CancellationToken cancellation = default);  
    Task<Pager<VEmployeeList>> GetEmployeesAsync(int? branchId, string? employeeId, QueryParam q, CancellationToken cancellation = default);
    Task<ErrorOr<VEmployeeList>> CreateAsync(EntityNames employee, CancellationToken cancellation = default);
    Task<ErrorOr<VEmployeeList>> UpdateAsync(EntityNames employee, CancellationToken cancellation = default);
    Task<VEmployeeList> GetEmployeeDetailsByIdAsync(string? id, int? branchId, bool include);
    Task<ErrorOr<VEmployeeList>> UpdateEmployeeStateAsync(string? id, bool state, CancellationToken ct = default);
    Task<VEmployeeList> CreateOrUpdateAsync(EntityNames employee, CancellationToken cancellation = default);
}
//Task<Result<string>> UpdateIsActiveAsync(string? id, bool state, CancellationToken cancellation = default);
//Task<Result<VEmployeeList>> GetByIdAsync(string? employeeId, CancellationToken cancellation = default);
//Task<Result<VEmployeeList>> GetEmployeeByIdAsync(string? employeeId, CancellationToken cancellation = default);
//Task<Result<IReadOnlyList<VEmployeeList>>> GetEmployeesAsync(string? employeeId, int branchId = 0, bool isActive = false, string? q = null);
//Task<Result<T>> GetByIdAsync<T>(string? id) where T : 
//Task<Pager<VEmployeeList>> GetEmployeesAsync(int? branchId, string? employeeId, QueryParam q, CancellationToken cancellation = default, params string[] columns);