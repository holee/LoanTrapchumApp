using Applications.Interfaces.Addresses;
using Applications.Interfaces.Branches;
using Applications.Interfaces.Customers;
using Applications.Interfaces.Employees;
using Applications.Interfaces.Identity;
using Applications.Interfaces.Lists;
using Applications.Services;
using Applications.Services.Addresses;
using Applications.Services.Branches;
using Applications.Services.Currencies;
using Applications.Services.Customers;
using Applications.Services.Employees;
using Applications.Services.Identity;
using Applications.Services.Lists;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
namespace Applications.DependencyInjections
{
    public static class ApplicationServiceInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.TryAddScoped<IAddressService, AddressService>();
            services.TryAddScoped<IBranchService, BranchService>();
            services.TryAddScoped<ICurrencyService, CurrencyService>();
            services.TryAddScoped<ICustomerService, CustomerService>();
            services.TryAddScoped<ICustomerIncomeExpendService, CustomerIncomeExpendService>();
            services.TryAddScoped<ICreditAgentService, CreditAgentService>();
            services.TryAddScoped<IEmployeeService, EmployeeService>();
            services.TryAddScoped<IEntityNameService, EntityNameService>();
            services.TryAddScoped<ILoginServiceImpl, LoginServiceImpl>();
            services.TryAddScoped<IUserServiceImpl, UserServiceImpl>();
            return services;

        }
    }
}
