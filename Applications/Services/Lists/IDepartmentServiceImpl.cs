using Applications.Features.Entities;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Departments;
using Domains.Models.Lists;

namespace Applications.Services.Lists;

public interface IDepartmentServiceImpl
{
    Task<Result<string>> ActivateOrDeactivate(int? id, bool req);
    Task<Result<string>> CreateAsync(DepartmentDtoCreate department); 
    Task<Result<string>> DeleteAsync(int? departmentId);
    Task<Result<string>> UpdateAsync(DepartmentDtoUpdate department);
    Task<Result<Department>> GetDepartmentById(int? departmentId);
    Task<Result<VDepartment>> GetDepartmentDetails(int? departmentId);
    Task<Result<IEnumerable<Department>>> GetDepartments(int? departmentId, int take = 0, string? condition = null);
    Task<Result<IEnumerable<T>>> GetDepartments<T>(int? departmentId, int take = 0, string? condition = null) where T:class;

    Task<Pager<VDepartment>> GetDepartments(QueryParam param);
}
