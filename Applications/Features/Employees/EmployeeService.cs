using Applications.Commons.Mappers;
using Applications.Interfaces.Employees;
using Applications.Interfaces.Lists;
using Applications.Requests.Lists.OtherNames;
using Applications.Responses;
using Applications.Responses.Lists.Entities;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Employees;

namespace Applications.Features.Employees
{
    public class EmployeeService(IEmployeeService empService, IOccupationService ocService) : IEmployeeServiceImpl
    {
        public async Task<Result<string>> CreateAsync(EntityEmployeeCreate entityName)
        {
            var emp = entityName.ToEmployeeCreate();
            return await empService.CreateAsync(emp);
        }
        public async Task<Result<string>> UpdateAsync(EntityEmployeeCreate entityName)
        {
            var emp = entityName.ToEmployeeEdit();

            return await empService.UpdateAsync(emp);
        }

        public async Task<Result<string>> DeleteAsync(string? employeeId)
        {
            return await empService.DeleteAsync(employeeId);
        }

        public async Task<Result<EntityNames>> GetByIdAsync(string? employeeId)
        {
            return await empService.GetByIdAsync(employeeId);
        }

        public async Task<Result<IReadOnlyList<VEmployeeList>>> GetEmployeesByBranchIdAsync(string? employeeId, int branchId = 0, bool isActive = false, string? q = null)
        {
            return await empService.GetEmployeesAsync(employeeId, branchId, isActive, q);
        }

        public async Task<Pager<VEmployeeList>> GetEmployeesAsync(int? branchId, string? employeeId, QueryParam param)
        {
            return await empService.GetEmployeesAsync(branchId, employeeId, param);
        }
        public async Task<Result<string>> UpdateIsActiveAsync(string? employeeId, bool state)
        {
            return await empService.UpdateIsActiveAsync(employeeId, state);
        }

        public async Task<IEnumerable<OcupationResponse>> GetOccupationsAsync(string? filter)
        {
            var rsp = await ocService.GetOccupations<OcupationResponse>(null, filter);
            return rsp.Items;
        }

        public async Task<Result<EntityEmployeeCreate>> GetEmployeeForUpdate(string? employeeId)
        {
            var emp = await empService.GetByIdAsync(employeeId);

            var employee = emp.Data!.ToEmployeeCreate();

            if (employee is not null)
            {
                return Result.Success(employee);
            }

            return Result.Failure<EntityEmployeeCreate>(ErrorResponse.NotFound("There is no employee"));
        }

        public async Task<Result<T>> GetEmployeeById<T>(string? employeeId)
            where T : class
        {
            return await empService.GetByIdAsync<T>(employeeId);
        }


        public async Task<Result<VEmployeeList>> GetEmployeeById(string? employeeId, int? branchId, bool include = false)
        {
            return await empService.GetByIdAsync(employeeId, branchId, include);
        }

        //public async Task<Result<VEmployeeList>> GetEmployeeById(string? employeeId,CancellationToken cancellation=default)
        //{
        //    return await empService.GetByIdAsync(employeeId,cancellation);
        //}

        public async Task<Result<VEmployeeList>> GetEmployeeById(string? employeeId, CancellationToken cancellation = default)
        {
            if (string.IsNullOrEmpty(employeeId))
            {
                return Result.Failure<VEmployeeList>(ErrorResponse.NotFound("Employee ID cannot be null or empty"));
            }

            return await empService.GetEmployeeByIdAsync(employeeId, cancellation);
        }
    }
}
