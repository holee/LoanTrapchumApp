using Applications.Interfaces.Lists;
using Applications.Requests.Lists.Entities;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Departments;
using Domains.Models.Lists;
using Mapster;

namespace Applications.Services.Lists;
public sealed class DepartmentService(IDepartmentService deService) : IDepartmentServiceImpl
{
    public async Task<Result<string>> ActivateOrDeactivate(int id, Department req)
    {
        return await deService.ActivateOrDeactivate(id, req);
    }

    public async Task<Result<string>> CreateAsync(DepartmentDtoCreate department)
    {
        var req = department.Adapt<Department>();

        return await deService.CreateAsync(req);
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

    public async Task<Result<string>> UpdateAsync(DepartmentDtoUpdate department)
    {
        var req = department.Adapt<Department>();

        return await deService.UpdateAsync(req);
    }
}
