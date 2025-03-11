using Applications.Interfaces.Lists;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Departments;
using Domains.Models.Lists;

namespace Applications.Services.Lists;

public sealed class DepartmentService(IDepartmentService deService) : IDepartmentService
{
    public async Task<Result<string>> ActivateOrDeactivate(int id, Department req)
    {
        return await deService.ActivateOrDeactivate(id, req);
    }

    public async Task<Result<string>> CreateAsync(Department department)
    {
        return await deService.CreateAsync(department);
    }

    public async Task<Result<string>> DeleteAsync(int departmentId)
    {
        return await deService.DeleteAsync(departmentId);
    }

    public async Task<Result<Department>> GetDepartmentById(int departmentId)
    {
        return await deService.GetDepartmentById(departmentId);
    }

    public async Task<Result<VDepartment>> GetDepartmentDetails(int departmentId)
    {
        return await deService.GetDepartmentDetails(departmentId);
    }

    public async Task<Result<IEnumerable<Department>>> GetDepartments(int departmentId = 0, int take = 0, bool includeInactive = false, string? condition = null)
    {
        return await deService.GetDepartments(departmentId, take, includeInactive, condition);
    }

    public async Task<Pager<VDepartment>> GetDepartments(QueryParam param)
    {
        return await deService.GetDepartments(param);
    }

    public async Task<Result<string>> UpdateAsync(Department department)
    {
        return await deService.UpdateAsync(department);
    }
}
