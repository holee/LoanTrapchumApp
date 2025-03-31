using Applications.Requests.Lists.Entities;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Departments;
using Domains.Models.Lists;

namespace Applications.Services.Lists;

public interface IDepartmentServiceImpl
{
    Task<Result<string>> ActivateOrDeactivate(int id, Department req);
    Task<Result<string>> CreateAsync(DepartmentDtoCreate department);
    Task<Result<string>> DeleteAsync(int departmentId);
    Task<Result<Department>> GetDepartmentById(int departmentId);
    Task<Result<VDepartment>> GetDepartmentDetails(int departmentId);
    Task<Result<IEnumerable<Department>>> GetDepartments(int departmentId = 0, int take = 0, bool includeInactive = false, string? condition = null);
    Task<Pager<VDepartment>> GetDepartments(QueryParam param);
    Task<Result<string>> UpdateAsync(DepartmentDtoUpdate department);
}
