using Applications.Interfaces.Lists;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.OtherNames;
using Domains.Models.Lists;
using Domains.Models.Lists.Customers;
using Domains.Models.Lists.Employees;
using Domains.Models.Lists.Vendors;

namespace Applications.Services.Lists 
{
    public sealed class OtherNameService(IOtherNameService oName) : IOtherNameService
    {
        public async Task<Result<string>> ConvertToCustomerAsync(long id, Customer customer)
        {
            return await oName.ConvertToCustomerAsync(id, customer);
        }

        public async Task<Result<string>> ConvertToEmployeeAsync(long id, Employee employee)
        {
           return await oName.ConvertToEmployeeAsync(id, employee);
        }

        public async Task<Result<string>> ConvertToOtherNameAsync(long id, EntityName othername)
        {
            return await oName.ConvertToOtherNameAsync(id, othername);
        }

        public async Task<Result<string>> ConvertToVendorAsync(long id, Vendor vendor)
        {
           return await oName.ConvertToVendorAsync(id, vendor);
        }

        public async Task<Result<string>> CreateAsync(EntityName othername)
        {
            return await oName.CreateAsync(othername);    
        }

        public async Task<Result<string>> CreateGaurantorAsync(EntityName othername)
        {
            return await oName.CreateGaurantorAsync(othername);
        }

        public async Task<Result<string>> DeleteAsync(long othernameId)
        {
            return await oName.DeleteAsync(othernameId);  
        }

        public async Task<IEnumerable<EntityName>> GetAllAsync(string? searchTerm, string sortField, string sortOrder, bool isActive = false)
        {
            return await oName.GetAllAsync(searchTerm, sortField, sortOrder, isActive);
        }

        public async Task<EntityName?> GetByIdAsync(long id)
        {
            return await oName.GetByIdAsync(id);
        }

        public async Task<EntityName?> GetDetailAsync(long id)
        {
            return await oName.GetDetailAsync(id);
        }

        public async Task<Pager<VOtherNames>> GetNamesAsync(QueryParam q, string type)
        {
           return await oName.GetNamesAsync(q, type);
        }
        public async Task<Result<string>> UpdateStatusAsync(long othernameId)
        {
            return await oName.UpdateStatusAsync(othernameId);
        }
    }
}
