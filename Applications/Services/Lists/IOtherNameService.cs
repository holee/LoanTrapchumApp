using Applications.Interfaces.Lists;
using Applications.Requests.Lists.Customers;
using Applications.Requests.Lists.Employees;
using Applications.Requests.Lists.Entities;
using Applications.Requests.Lists.OtherNames;
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
        Task<Result<string>> ConvertToVendorAsync(Guid? id, VendorDtoCreate vendor);
        Task<Result<string>> CreateAsync(EntityNameDtoCreate othername);
        Task<Result<string>> CreateAsync(EntNameDtoCreate othername);
        Task<Result<string>> UpdateAsync(EntNameDtoUpdate othername); 
        Task<Result<string>> CreateGaurantorAsync(EntityNameDtoCreate othername);
        Task<Result<string>> DeleteAsync(Guid? othernameId);
        Task<IEnumerable<EntityName>> GetAllAsync(string? searchTerm, string sortField, string sortOrder, bool isActive = false);
        Task<EntityName?> GetByIdAsync(Guid? id);
        Task<EntityName?> GetDetailAsync(Guid? id);
        Task<Pager<VOtherNames>> GetNamesAsync(QueryParam q, string type);
        Task<Result<string>> UpdateStatusAsync(Guid? othernameId);
    }


    public sealed class OtherNameService(IOtherNameService oName) : IOtherNameServiceImpl
    {
        public async Task<Result<string>> ConvertToCustomerAsync(Guid? id, CustomerDtoCreate customer)
        {
            var req = customer.Adapt<Customer>();

            return await oName.ConvertToCustomerAsync(id, req);
        }

        public async Task<Result<string>> ConvertToEmployeeAsync(Guid? id, EmployeeDtoCreate employee)
        {
            var req = employee.Adapt<Employee>();

            return await oName.ConvertToEmployeeAsync(id, req);
        }

        public async Task<Result<string>> ConvertToOtherNameAsync(Guid? id, EntityName othername)
        {
            return await oName.ConvertToOtherNameAsync(id, othername);
        }

        public async Task<Result<string>> ConvertToVendorAsync(Guid? id, VendorDtoCreate vendor)
        {
            var req = vendor.Adapt<Vendor>();

            return await oName.ConvertToVendorAsync(id, req);
        }

        public async Task<Result<string>> CreateAsync(EntityNameDtoCreate othername)
        {

            var req = othername.Adapt<EntityName>();

            return await oName.CreateAsync(req);
        }

        public async Task<Result<string>> CreateAsync(EntNameDtoCreate othername)
        {
            var req = othername.Adapt<EntityName>();

            return await oName.CreateAsync(req);
        }

        public async Task<Result<string>> CreateGaurantorAsync(EntityNameDtoCreate othername)
        {
            var req = othername.Adapt<EntityName>();

            return await oName.CreateGaurantorAsync(req);
        }

        public async Task<Result<string>> DeleteAsync(Guid? id)
        {
            return await oName.DeleteAsync(id);
        }

        public async Task<IEnumerable<EntityName>> GetAllAsync(string? searchTerm, string sortField, string sortOrder, bool isActive = false)
        {
            return await oName.GetAllAsync(searchTerm, sortField, sortOrder, isActive);
        }

        public async Task<EntityName?> GetByIdAsync(Guid? id)
        {
            return await oName.GetByIdAsync(id);
        }

        public async Task<EntityName?> GetDetailAsync(Guid? id)
        {
            return await oName.GetDetailAsync(id);
        }

        public async Task<Pager<VOtherNames>> GetNamesAsync(QueryParam q, string type)
        {
            return await oName.GetNamesAsync(q, type);
        }

        public Task<Result<string>> UpdateAsync(EntNameDtoUpdate othername)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<string>> UpdateStatusAsync(Guid? id)
        {
            return await oName.UpdateStatusAsync(id);
        }
    }
}
