using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Departments;
using Domains.Models.Lists;

namespace Applications.Interfaces.Lists;

public interface IDepartmentService
{
    Task<Result<Department>> GetDepartmentById(int departmentId);
    Task<Result<IEnumerable<Department>>> GetDepartments(int departmentId = 0, int take = 0,bool includeInactive = false,string? condition = null);
    Task<Pager<VDepartment>> GetDepartments(QueryParam param);
    Task<Result<string>> CreateAsync(Department department);
    Task<Result<string>> UpdateAsync(Department department);
    Task<Result<string>> DeleteAsync(int departmentId);
    Task<Result<string>> ActivateOrDeactivate(int id, Department req);
    Task<Result<VDepartment>> GetDepartmentDetails(int departmentId);
}
