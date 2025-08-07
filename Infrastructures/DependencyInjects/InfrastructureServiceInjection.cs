using ActualLab.DependencyInjection;
using Applications.Interfaces;
using Applications.Interfaces.Addresses;
using Applications.Interfaces.Audits;
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
using Domains.Models.Identity;
using Infrastructures.DataAccess;
using Infrastructures.DataAccess.Factories;
using Infrastructures.Implements;
using Infrastructures.Implements.Audits;
using Infrastructures.Implements.Identity;
using Infrastructures.Persistence.Interceptors;
using Infrastructures.Repository;
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

            var connectionString = configuration.GetConnectionString("MyConnection") ?? throw new InvalidOperationException(nameof(configuration));

            services.AddDbContextFactory<DataContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddDbContext<UserDataContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });


            services.AddScoped<Seeds.DatabaseSeeder>();


            services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ApplicationScheme;

            }).AddIdentityCookies();

            services.AddIdentityCore<ApplicationUser>()
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<UserDataContext>()
                .AddRoleManager<RoleManager<ApplicationRole>>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromDays(15);
                options.SlidingExpiration = true;
            });
            services.AddSingleton<AuditingSaveChangesInterceptor>();
            services.AddSingleton<ICurrentUserService, CurrentUserService>();
            services.AddSingleton<ICustomerCollateralService, CustomerCollateralRepository>();
            services.AddScoped<IAuditReadService, AuditReadService>();
            services.AddScoped<IAppSettingService, AppSettingService>();
            services.AddScoped<ILoanSettingService, LoanSettingService>();
            services.AddScoped<IForiegnCurrencyExchange, ForiegnCurrencyExchange>();
            services.AddScoped<IJournalService, JournalService>();
            services.AddScoped<ISpotRateService, SpotRateService>();
            services.AddScoped<ILedgerService, LedgerService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleRepository>();
            services.AddScoped<IOpenBranchService, OpenBranchService>();
            services.AddScoped<IOccupationService, OccupationService>();
            services.AddScoped<IOtherNameService, OtherNameService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IBranchService, BranchService>();
            services.AddScoped<IBankService, BankRepository>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IEntityNameService, EntityNameService>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<ICashVaultService, CashVaultService>();
            services.AddScoped<ICashVaultService, CashVaultService>();
            services.AddScoped<ICollateralsService, CollateralsRepository>();
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<ICreditAgentService, CreditAgentService>();
            services.AddScoped<ICustomerService, CustomerRepository>();
            services.AddScoped<ICustomerIncomeExpendService, IncomeExpendRepository>();
            services.AddScoped<ICustomerNoteService, CustomerNoteRepository>();
            services.AddScoped<ICustomerActivityService, CustomerActivityRepository>();
            services.AddScoped<IHolidayService, HolidayService>();
            services.AddScoped<IGuarantorService, GuarantorService>();
            services.AddScoped<ILoanSettingService, LoanSettingService>();
            services.AddScoped<ILoanProductService, LoanProductService>();
            services.AddScoped<ILoanPaymentService, LoanPaymentService>();
            services.AddScoped<ILoanScheduleService, LoanScheduleService>();
            services.AddScoped<ILoanService, LoanDisbursementRepository>();
            services.AddScoped<ILoanCategoryService, LoanCategoryService>();
            services.AddScoped<ILoanRequestService, LoanRequestService>();
            services.AddScoped<ILoanReportService, LoanReportService>();
            services.AddScoped<ISalutationService, SalutaionService>();
            services.AddScoped<INationalityService, NationalityService>();
            services.AddScoped<IFileUploadService, FileUploadService>();
            services.AddScoped<IVendorService, VendorService>();
            services.AddScoped<IProvinceService, ProvinceService>();
            services.AddScoped<IDistrictService, DistrictService>();
            services.AddScoped<ICommuneService, CommuneService>();
            services.AddScoped<IVillageServer, VillageService>();
            services.AddScoped<ILoanScheduleRepo, LoanScheduleRepository>();
            services.AddSingleton<IDapperConnectionFactory, DapperConnectionFactory>();
            return services;

        }

    }
}
