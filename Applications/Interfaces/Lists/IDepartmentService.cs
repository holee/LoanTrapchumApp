using Applications.Features.Departments.Dto;
using Applications.Responses;
using Applications.Responses.Paginations;
using Domains.Models.Lists;

namespace Applications.Interfaces.Lists;

public interface IDepartmentService
{
    Task<Result<Department>> GetDepartmentById(int? departmentId);
    Task<Result<IEnumerable<Department>>> GetDepartments(int? departmentId = 0, int take = 0,bool includeInactive = false,string? condition = null);
    Task<Pager<VDepartment>> GetDepartments(QueryParam param);
    Task<Result<string>> DeleteAsync(int? id, CancellationToken cancellation = default);
    Task<Result<string>> ActivateOrDeactivate(int? id, bool state);
    Task<Result<VDepartment>> GetDepartmentDetails(int? departmentId);
    Task<Result<IEnumerable<T>>> GetDepartments<T>(int? departmentId, int take = 10, string? condition = null) where T : class;
    Task<Result<string>> CreateAsync(Department req, CancellationToken cancellation = default);
    Task<Result<string>> UpdateAsync(Department req, CancellationToken cancellation = default);
}
