using Applications.Extensions.Identity;
using Applications.Interfaces.Addresses;
using Applications.Interfaces.Banks;
using Applications.Interfaces.Branches;
using Applications.Interfaces.Currencies;
using Applications.Interfaces.Customers;
using Applications.Interfaces.Employees;
using Applications.Interfaces.Files;
using Applications.Interfaces.Identity;
using Applications.Interfaces.Lists;
using Applications.Interfaces.Loans;
using Applications.Interfaces.setting;
using Applications.Interfaces.Transactions;
using Applications.Interfaces.Vendors;
using Infrastructures.DataAccess;
using Infrastructures.DataAccess.Factories;
using Infrastructures.Implements;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Infrastructures.DependencyInjects
{
    public static class InfrastructureServiceInjection 
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            },ServiceLifetime.Transient);

            services.AddDbContextFactory<UserDataContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddDbContextFactory<DataContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddScoped<Seeds.DatabaseSeeder>();
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            }).AddIdentityCookies();
            services.AddIdentityCore<ApplicationUser>()
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<UserDataContext>()
                .AddRoleManager<RoleManager<ApplicationRole>>()
                .AddSignInManager()
                .AddDefaultTokenProviders();
           
            services.AddScoped<IAppSettingService, AppSettingService>();
            services.TryAddTransient<ILoanSettingService, LoanSettingService>();
            services.TryAddTransient<IForiegnCurrencyExchange, ForiegnCurrencyExchange>();
            services.TryAddTransient<IJournalService, JournalService>();
            services.TryAddTransient<ISpotRateService, SpotRateService>();
            services.TryAddTransient<ILedgerService, LedgerService>();
            services.TryAddTransient<IUserService, UserService>();
            services.TryAddTransient<IRoleService, RoleService>();
            services.TryAddTransient<IOpenBranchService, OpenBranchService>();
            services.TryAddTransient<IOccupationService, OccupationService>();
            services.TryAddTransient<IOtherNameService, OtherNameService>();
            services.TryAddTransient<ILoginService, LoginService>();
            services.TryAddTransient<IBranchService, BranchService>();
            services.TryAddTransient<IBankService, BankService>();
            services.TryAddTransient<IDepartmentService, DepartmentService>();
            services.TryAddTransient<IEmployeeService, EmployeeService>();
            services.TryAddTransient<IEntityNameService, EntityNameService>();
            services.TryAddTransient<IAddressService, AddressService>();
            services.TryAddTransient<ICashVaultService, CashVaultService>();
            services.TryAddTransient<ICashVaultService, CashVaultService>();
            services.TryAddTransient<ICustomerService, CustomerService>();
            services.TryAddTransient<ICollateralsService, CollateralsService>();
            services.TryAddTransient<ICurrencyService, CurrencyService>();
            services.TryAddTransient<ICreditAgentService, CreditAgentService>();
            services.TryAddTransient<ICustomerIncomeExpendService, CustomerIncomeExpendService>();
            services.TryAddTransient<IHolidayService, HolidayService>();
            services.TryAddTransient<IGuarantorService, GuarantorService>();
            services.TryAddTransient<ILoanSettingService, LoanSettingService>();
            services.TryAddTransient<ILoanProductService, LoanProductService>();
            services.TryAddTransient<ILoanPaymentService, LoanPaymentService>();
            services.TryAddTransient<ILoanScheduleService, LoanScheduleService>();
            services.TryAddTransient<ILoanService, LoanService>();
            services.TryAddTransient<ILoanCategoryService, LoanCategoryService>();
            services.TryAddTransient<ILoanRequestService, LoanRequestService>();
            services.TryAddTransient<ISalutationService, SalutaionService>();
            services.TryAddTransient<INationalityService, NationalityService>();
            services.TryAddTransient<IFileUploadService, FileUploadService>();
            services.AddScoped<IDapperConnectionFactory, DapperConnectionFactory>();
            services.AddScoped<IVendorService, VendorService>();
            return services;

        }
        
    }
}
