using Applications.Commons.Mappers;
using Applications.Features.OtherNames;
using Applications.Features.OtherNames.Dto;
using Applications.Interfaces.Lists;
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
        Task<Result<string>> ConvertToCustomerAsync(string? id, ConvertCreateCustomer customer);
        Task<Result<string>> ConvertToEmployeeAsync(string? id, ConvertCreateEmployee employee);
        Task<Result<string>> ConvertToVendorAsync(string? id, ConvertCreateVendor vendor);
        Task<Result<string>> CreateAsync(EntityNamesCreate othername);
        Task<Result<string>> UpdateAsync(EntityNamesCreate othername);
        Task<Result<string>> CreateGaurantorAsync(EntityNamesCreate othername);
        Task<Result<string>> DeleteAsync(string? othernameId);
        Task<Result<EntityNames>> GetByIdAsync(string? id); 
        Task<Result<VOtherNames>> GetDetailAsync(string? id);
        Task<Result<EntityNamesCreate>> GetEditNameByIdAsync(string? id);
        Task<IEnumerable<VOtherNames>> GetNameLists(string? nameId, QueryParam q);
        Task<Pager<VOtherNames>> GetNamesAsync(QueryParam q, CancellationToken cancellation = default);
        Task<Result<VOtherNames>> UpdateStatusAsync(string? id, bool state);
        Task<Pager<VOtherNames>> GetNameAsync(string? nameId, QueryParam q, CancellationToken cancellation = default);
    }

    public sealed class OtherNameService(IOtherNameService oName) : IOtherNameServiceImpl
    {
        public async Task<Result<string>> ConvertToCustomerAsync(string? id, ConvertCreateCustomer customer)
        {
            var req = customer.Adapt<Customer>();
            var result = await oName.ConvertToCustomerAsync(id, req);
            return result.IsError
                ? Result.Failure<string>(ErrorResponse.Conflict(result.FirstError.Description))
                : Result.Success("Name was converted to customer.");
        }

        public async Task<Result<string>> ConvertToEmployeeAsync(string? id, ConvertCreateEmployee employee)
        {
            var req = employee.Adapt<Employee>();
            var result = await oName.ConvertToEmployeeAsync(id, req);
            return result.IsError
                ? Result.Failure<string>(ErrorResponse.Conflict(result.FirstError.Description))
                : Result.Success("Name was converted to employee.");
        }

        public async Task<Result<string>> ConvertToVendorAsync(string? id, ConvertCreateVendor vendor)
        {
            var req = vendor.Adapt<Vendor>();
            var result = await oName.ConvertToVendorAsync(id, req);
            return result.IsError
                ? Result.Failure<string>(ErrorResponse.Conflict(result.FirstError.Description))
                : Result.Success("Name was converted to vendor.");
        }

        public async Task<Result<string>> CreateAsync(EntityNamesCreate othername)
        {
            var req = othername.Adapt<EntityNames>();
            var result = await oName.CreateAsync(req);
            return result.IsError
                ? Result.Failure<string>(ErrorResponse.ServerError(result.FirstError.Description))
                : Result.Success("A new name was created.");
        }

        public async Task<Result<string>> CreateGaurantorAsync(EntityNamesCreate othername)
        {
            var req = othername.Adapt<EntityNames>();
            var result = await oName.CreateGaurantorAsync(req);
            return result.IsError
                ? Result.Failure<string>(ErrorResponse.ServerError(result.FirstError.Description))
                : Result.Success("A new guarantor was created.");
        }

        public async Task<Result<string>> DeleteAsync(string? id)
        {
            var result = await oName.DeleteAsync(id);
            return result.IsError
                ? Result.Failure<string>(ErrorResponse.NotFound(result.FirstError.Description))
                : Result.Success("Name was deleted.");
        }

        public async Task<Result<EntityNames>> GetByIdAsync(string? id)
        {
            var result = await oName.GetByIdAsync(id);
            return result.IsError
                ? Result.Failure<EntityNames>(ErrorResponse.NotFound(result.FirstError.Description))
                : Result.Success(result.Value);
        }

        public async Task<Result<EntityNamesCreate>> GetEditNameByIdAsync(string? id)
        {
            var result = await oName.GetByIdAsync(id);
            if (result.IsError)
                return Result.Failure<EntityNamesCreate>(ErrorResponse.NotFound(result.FirstError.Description));

            var model = result.Value.ToNamesCreate();
            return Result.Success(model);
        }

        public async Task<Result<VOtherNames>> GetDetailAsync(string? id)
        {
            var result = await oName.GetDetailAsync(id);
            return result.IsError
                ? Result.Failure<VOtherNames>(ErrorResponse.NotFound(result.FirstError.Description))
                : Result.Success(result.Value);
        }

        public async Task<Pager<VOtherNames>> GetNamesAsync(QueryParam q, CancellationToken cancellation = default)
            => await oName.GetNamesAsync(q, cancellation);

        public async Task<IEnumerable<VOtherNames>> GetNameLists(string? nameId, QueryParam q)
            => await oName.GetNameListsAsync(nameId, q);

        public async Task<Pager<VOtherNames>> GetNameAsync(string? nameId, QueryParam q, CancellationToken cancellation = default)
            => await oName.GetNameAsync(nameId, q, cancellation);

        public async Task<Result<string>> UpdateAsync(EntityNamesCreate othername)
        {
            var model = othername.MapNamesEdit();
            var result = await oName.UpdateAsync(model);
            return result.IsError
                ? Result.Failure<string>(ErrorResponse.ServerError(result.FirstError.Description))
                : Result.Success("Name was updated.");
        }

        public async Task<Result<string>> UpdateStatusAsync(string? id)
        {
            var result = await oName.UpdateStatusAsync(id);
            return result.IsError
                ? Result.Failure<string>(ErrorResponse.ServerError(result.FirstError.Description))
                : Result.Success("Status was updated.");
        }

        public async Task<Result<VOtherNames>> UpdateStatusAsync(string? id, bool state)
        {
            var result = await oName.UpdateStatusAsync(id, state);
            return result.IsError
                ? Result.Failure<VOtherNames>(ErrorResponse.ServerError(result.FirstError.Description))
                : Result.Success(result.Value);
        }
    }
}
