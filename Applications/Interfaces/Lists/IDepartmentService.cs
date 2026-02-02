using Applications.Features.Departments.Dto;
using Applications.Responses;
using Applications.Responses.Paginations;
using Domains.Models.Lists;
using ErrorOr;

namespace Applications.Interfaces.Lists;

public interface IDepartmentService
{

    Task<ErrorOr<string>> DeleteAsync(int? id, CancellationToken cancellation = default);
    Task<ErrorOr<VDepartment>> ActivateOrDeactivate(int? id, bool state, CancellationToken cancellation = default);
    Task<ErrorOr<VDepartment>> CreateAsync(Department req, CancellationToken cancellation = default);
    Task<ErrorOr<VDepartment>> UpdateAsync(Department req, CancellationToken cancellation = default);
    Task<Pager<VDepartment>> GetDepartments(int? departmentId, QueryParam param, CancellationToken cancellation = default);
    Task<ErrorOr<Department>> GetDepartmentById(int? departmentId, int? branchId, CancellationToken cancellation = default);
    Task<ErrorOr<VDepartment>> GetDepartmentDetailsById(int? departmentId, int? branchId, CancellationToken cancellation = default, params string[] columns);
    Task<bool> ExistsByNameAsync(string name, int branchId, CancellationToken ct);
}
//Task<Result<VDepartment>> GetDepartmentDetailsById(int? departmentId);
//Task<Result<Department>> GetDepartmentById(int? departmentId, CancellationToken cancellation = default);
//Task<Pager<VDepartment>> GetDepartments(QueryParam param);
//Task<Result<IEnumerable<Department>>> GetDepartments(int? departmentId = 0, int take = 0, bool includeInactive = false, string? condition = null);
