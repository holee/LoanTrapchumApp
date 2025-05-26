using Applications.Helpers.Mappers;
using Applications.Interfaces.Lists;
using Applications.Requests.Lists.Customers;
using Applications.Requests.Lists.Employees;
using Applications.Requests.Lists.Entities;
using Applications.Requests.Lists.OtherNames;
using Applications.Requests.Lists.Vendors;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.OtherNames;
using Domains.Models.Bases;
using Domains.Models.Lists;
using Domains.Models.Lists.Customers;
using Domains.Models.Lists.Employees;
using Domains.Models.Lists.Vendors;
using Mapster;

namespace Applications.Services.Lists
{
    public interface IOtherNameServiceImpl
    {
        Task<Result<string>> ConvertToCustomerAsync(string? id, CustomerConvertCreate customer);
        Task<Result<string>> ConvertToEmployeeAsync(string? id, EmployeeConvertCreate employee);
        Task<Result<string>> ConvertToVendorAsync(string? id, VenderConvertCreate vendor);
        Task<Result<string>> CreateAsync(EntityNamesCreate othername);
        Task<Result<string>> UpdateAsync(EntityNamesCreate othername);
        Task<Result<string>> CreateGaurantorAsync(EntityNamesCreate othername);
        Task<Result<string>> DeleteAsync(string? othernameId);
        Task<Result<EntityNames>> GetByIdAsync(string? id);
        Task<Result<T>> GetDetailAsync<T>(string? id) where T : class;
        Task<Pager<VOtherNames>> GetNamesAsync(QueryParam q, string type);
        Task<Result<string>> UpdateStatusAsync(string? othernameId);
        Task<Pager<VOtherNames>> GetNamesAsync(QueryParam q, EntType type);
        Task<Result<EntityNamesCreate>> GetEditNameByIdAsync(string? id);

        Task<IEnumerable<T>> GetNameLists<T>(string? nameId, QueryParam q) 
            where T : class, new();
    }


    public sealed class OtherNameService(IOtherNameService oName) : IOtherNameServiceImpl
    {
        public async Task<Result<string>> ConvertToCustomerAsync(string? id, CustomerConvertCreate customer)
        {
            var req = customer.Adapt<Customer>();

            return await oName.ConvertToCustomerAsync(id, req);
        }

        public async Task<Result<string>> ConvertToEmployeeAsync(string? id, EmployeeConvertCreate employee)
        {
            var req = employee.Adapt<Employee>();

            return await oName.ConvertToEmployeeAsync(id, req);
        }

        public async Task<Result<string>> ConvertToVendorAsync(string? id, VenderConvertCreate vendor)
        {
            var req = vendor.Adapt<Vendor>();

            return await oName.ConvertToVendorAsync(id, req);
        }

        public async Task<Result<string>> CreateAsync(EntityNamesCreate othername)
        {
            var req = othername.Adapt<EntityNames>();

            return await oName.CreateAsync(req);
        }


        public async Task<Result<string>> CreateGaurantorAsync(EntityNamesCreate othername)
        {
            var req = othername.Adapt<EntityNames>();

            return await oName.CreateGaurantorAsync(req);
        }

        public async Task<Result<string>> DeleteAsync(string? id)
        {
            return await oName.DeleteAsync(id);
        }

        public async Task<Result<EntityNames>> GetByIdAsync(string? id)
        {
            return await oName.GetByIdAsync(id);
        }

        public async Task<Result<EntityNamesCreate>> GetEditNameByIdAsync(string? id)
        {
            var rsp = await oName.GetByIdAsync(id);

            if (rsp.IsSuccess)
            {
                var model = rsp.Data!.ToNamesCreate();

                return Result.Success(model);
            }

            return Result.Failure<EntityNamesCreate>(ErrorResponse.NotFound(rsp.ErrMessage));
        }


        public async Task<Result<T>> GetDetailAsync<T>(string? id) where T : class
        {
            return await oName.GetDetailAsync<T>(id);
        }

        public async Task<Pager<VOtherNames>> GetNamesAsync(QueryParam q, string type)
        {
            return await oName.GetNamesAsync(q, type);
        }

        public async Task<IEnumerable<T>> GetNameLists<T>(string? nameId, QueryParam q) 
            where T : class, new()
        {
            return await oName.GetNameLists<T>(nameId, q);
        }

        public async Task<Pager<VOtherNames>> GetNamesAsync(QueryParam q, EntType type)
        {
            return await oName.GetNamesAsync(q, type);
        }

        public async Task<Result<string>> UpdateAsync(EntityNamesCreate othername)
        {
            var model = othername.Adapt<EntityNames>();

            return await oName.UpdateAsync(model);
        }

        public async Task<Result<string>> UpdateStatusAsync(string? id)
        {
            return await oName.UpdateStatusAsync(id);
        }
    }
}