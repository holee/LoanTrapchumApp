using Applications.Extensions;
using Applications.Interfaces.Employees;
using Applications.Interfaces.Lists;
using Applications.Requests.Lists.Employees;
using Applications.Requests.Lists.OtherNames;
using Applications.Responses;
using Applications.Responses.Lists.Entities;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Employees;

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

        public async Task<Result<string>> DeleteAsync(string? employeeId)
        {
            return await empService.DeleteAsync(employeeId);
        }

        public async Task<Result<EntityNames>> GetByIdAsync(string? employeeId)
        {
            return await empService.GetByIdAsync(employeeId);
        }

        public async Task<Result<IReadOnlyList<VEmployeeList>>> GetEmployeesAsync(string? employeeId, int branchId = 0, bool isActive = false, string? q = null)
        {
            return await empService.GetEmployeesAsync(employeeId, branchId, isActive, q);
        }

        public async Task<Pager<VEmployeeList>> GetEmployeesAsync(QueryParam param)
        {
            return await empService.GetEmployeesAsync(param);
        }
        public async Task<Pager<VEmployeeList>> GetEmployeesAsync(int? branchId,QueryParam param) 
        {
            return await empService.GetEmployeesAsync(branchId,param);
        }
        public async Task<Result<string>> UpdateIsActiveAsync(string? employeeId)
        {
            return await empService.UpdateIsActiveAsync(employeeId);
        }

        public async Task<IEnumerable<OcupationResponse>> GetOccupationsAsync(string? filter)
        {
            var rsp = await ocService.GetOccupations<OcupationResponse>(null, filter);
            return rsp.Items;
        }

        public async Task<Result<EntityNamesCreate>> GetEmployeeForUpdate(string? employeeId)
        {
            var emp = await empService.GetByIdAsync(employeeId);

            var employee = emp.Data!.ToEmployeeCreate();

            if (employee is not null)
            {
                return Result.Success(employee);
            }

            return Result.Failure<EntityNamesCreate>(ErrorResponse.NotFound("There is no employee"));
        }

        public async Task<Result<T>> GetEmployeeById<T>(string? employeeId)
            where T:class
        {
            return await empService.GetByIdAsync<T>(employeeId);
        }


        public async Task<Result<VEmployeeList>> GetEmployeeById(string? employeeId,int? branchId,bool include=false)
        {
            return await empService.GetByIdAsync(employeeId,branchId,include);
        }


    }
}
