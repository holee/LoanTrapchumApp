using Applications.Interfaces.Employees;
using Applications.Interfaces.Lists;
using Applications.Requests.Lists.Employees;
using Applications.Requests.Lists.Entities;
using Applications.Requests.Lists.OtherNames;
using Applications.Responses;
using Applications.Responses.Lists.Entities;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Employees;
using Domains.Models.Lists.Employees;
using Mapster;

namespace Applications.Services.Employees
{
    public class EmployeeService(IEmployeeService empService, IOccupationService ocService) : IEmployeeServiceImpl
    {
        public async Task<Result<string>> CreateAsync(EntityNamesCreate entityName)
        {
            var emp = entityName.ToEmployee();
            var req = entityName.ToEntityNames();
            req.Employee = emp;
            return await empService.CreateAsync(req);
        }
        public async Task<Result<string>> UpdateAsync(EntityNamesCreate entityName)
        {
            var emp = entityName.ToEmployee();
            var req = entityName.ToEntityNames();
            req.Employee = emp;
            return await empService.UpdateAsync(req);
        }

        public async Task<Result<string>> DeleteAsync(Guid? employeeId)
        {
            return await empService.DeleteAsync(employeeId);
        }

        public async Task<Result<Employee>> GetByIdAsync(Guid? employeeId)
        {
            return await empService.GetByIdAsync(employeeId);
        }

        public async Task<Result<IReadOnlyList<VEmployees>>> GetEmployeesAsync(Guid? employeeId, int branchId = 0, bool isActive = false, string? q = null)
        {
            return await empService.GetEmployeesAsync(employeeId, branchId, isActive, q);
        }

        public Task<Pager<VEmployees>> GetEmployeesAsync(QueryParam param)
        {
            throw new NotImplementedException();
        }

        public async Task<Pager<VEmployees>> GetEmployeesAsync(int pageIndex, int pageSize, string sortColumn, bool isDecending = false, bool includeInActive = false, string? q = null)
        {
            return await empService.GetEmployeesAsync(pageIndex, pageSize, sortColumn, isDecending, includeInActive, q);
        }

        public async Task<Result<string>> UpdateIsActiveAsync(Guid? employeeId)
        {
            return await empService.UpdateIsActiveAsync(employeeId);
        }

        public async Task<IEnumerable<OcupationResponse>> GetOccupationsAsync(string? filter) 
        {
            var rsp= await ocService.GetOccupations<OcupationResponse>(null, filter);
            return rsp.Items;
        }

        public Task<IEnumerable<EmployeeDtoUpdate>> GetEmployeeForUpdate(Guid? employeeId)
        {
            throw new NotImplementedException();
        }

        Task<Result<EmployeeDtoUpdate>> IEmployeeServiceImpl.GetEmployeeForUpdate(Guid? employeeId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<EmployeeDto>> GetEmployeeById(Guid? employeeId)
        {
            throw new NotImplementedException();
        }
    }
}
