using Applications.Interfaces.Lists;
using Applications.Requests.Lists.Customers;
using Applications.Requests.Lists.Employees;
using Applications.Requests.Lists.Entities;
using Applications.Requests.Lists.Vendors;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.OtherNames;
using Domains.Models.Lists;
using Domains.Models.Lists.Customers;
using Domains.Models.Lists.Employees;
using Domains.Models.Lists.Vendors;
using Mapster;

namespace Applications.Services.Lists
{
    public interface IOtherNameServiceImpl
    {
        Task<Result<string>> ConvertToCustomerAsync(Guid? id, CustomerDtoCreate customer);
        Task<Result<string>> ConvertToEmployeeAsync(Guid? id, EmployeeDtoCreate employee);
        Task<Result<string>> ConvertToOtherNameAsync(Guid? id, EntityName othername);
        Task<Result<string>> ConvertToVendorAsync(Guid id, VendorDtoCreate vendor);
        Task<Result<string>> CreateAsync(EntityNameDtoCreate othername);
        Task<Result<string>> CreateGaurantorAsync(EntityNameDtoCreate othername);
        Task<Result<string>> DeleteAsync(Guid? othernameId);
        Task<IEnumerable<EntityName>> GetAllAsync(string? searchTerm, string sortField, string sortOrder, bool isActive = false);
        Task<EntityName?> GetByIdAsync(Guid? id);
        Task<EntityName?> GetByIdAsync(long id);
        Task<EntityName?> GetDetailAsync(Guid? id);
        Task<Pager<VOtherNames>> GetNamesAsync(QueryParam q, string type);
        Task<Result<string>> UpdateStatusAsync(Guid? othernameId);
    }


    public sealed class OtherNameService(IOtherNameService oName) : IOtherNameServiceImpl
    {
        public async Task<Result<string>> ConvertToCustomerAsync(Guid? id, CustomerDtoCreate customer)
        {
            var req = customer.Adapt<Customer>();

            return await oName.ConvertToCustomerAsync(0, req);
        }

        public async Task<Result<string>> ConvertToEmployeeAsync(Guid? id, EmployeeDtoCreate employee)
        {
            var req = employee.Adapt<Employee>();

            return await oName.ConvertToEmployeeAsync(0, req);
        }

        public async Task<Result<string>> ConvertToOtherNameAsync(Guid? id, EntityName othername)
        {
            return await oName.ConvertToOtherNameAsync(0, othername);
        }

        public async Task<Result<string>> ConvertToVendorAsync(Guid id, VendorDtoCreate vendor)
        {
            var req = vendor.Adapt<Vendor>();

            return await oName.ConvertToVendorAsync(0, req);
        }

        public async Task<Result<string>> CreateAsync(EntityNameDtoCreate othername)
        {

            var req = othername.Adapt<EntityName>();

            return await oName.CreateAsync(req);
        }

        public async Task<Result<string>> CreateGaurantorAsync(EntityNameDtoCreate othername)
        {
            var req = othername.Adapt<EntityName>();

            return await oName.CreateGaurantorAsync(req);
        }

        public async Task<Result<string>> DeleteAsync(Guid? othernameId)
        {
            return await oName.DeleteAsync(0);
        }

        public async Task<IEnumerable<EntityName>> GetAllAsync(string? searchTerm, string sortField, string sortOrder, bool isActive = false)
        {
            return await oName.GetAllAsync(searchTerm, sortField, sortOrder, isActive);
        }

        public async Task<EntityName?> GetByIdAsync(long id)
        {
            return await oName.GetByIdAsync(id);
        }

        public Task<EntityName?> GetByIdAsync(Guid? id)
        {
            throw new NotImplementedException();
        }

        public async Task<EntityName?> GetDetailAsync(Guid? id)
        {
            return await oName.GetDetailAsync(0);
        }

        public async Task<Pager<VOtherNames>> GetNamesAsync(QueryParam q, string type)
        {
            return await oName.GetNamesAsync(q, type);
        }
        public async Task<Result<string>> UpdateStatusAsync(Guid? othernameId)
        {
            return await oName.UpdateStatusAsync(0);
        }
    }
}
