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
using Applications.Interfaces.Settings;
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
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructures;

public static class InfrastructureServiceInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {

        var connectionString = configuration.GetConnectionString("MyConnection") ?? throw new InvalidOperationException(nameof(configuration));

        services.AddDbContextFactory<DataContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddDbContextFactory<UserDataContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });


        services.AddScoped<Seeds.DatabaseSeeder>();


        services.AddAuthentication(options =>
        {
            options.DefaultScheme = IdentityConstants.ApplicationScheme;
            options.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
        }).AddIdentityCookies();

        services.AddIdentityCore<ApplicationUser>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 5;
        })
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<UserDataContext>()
            .AddRoleManager<RoleManager<ApplicationRole>>()
            .AddSignInManager<SignInManager<ApplicationUser>>()
            .AddDefaultTokenProviders();

        services.ConfigureApplicationCookie(options =>
        {
            options.ExpireTimeSpan = TimeSpan.FromHours(24);
            options.SlidingExpiration = true;
            //options.AccessDeniedPath = new PathString("/app/access-denied");
        });

        services.AddHttpContextAccessor();
        services.AddSingleton<AuditingSaveChangesInterceptor>();
        services.AddSingleton<ICurrentUserService, CurrentUserReposity>();
        services.AddSingleton<ICurrentUserServices, CurrentUserRepository>();
        services.AddScoped<IAuditReadService, AuditReadService>();
        services.AddScoped<Applications.Interfaces.Settings.IAppSettingService, AppSettingService>();
        services.AddScoped<Applications.Interfaces.setting.IAppSettingService, AppSettingRepository>();
        services.AddScoped<ILoanSettingService, LoanSettingRepository>();
        services.AddScoped<IForiegnCurrencyExchange, ForiegnCurrencyExchange>();
        services.AddScoped<IJournalService, JournalRepository>();
        services.AddScoped<ISpotRateService, SpotRateService>();
        services.AddScoped<ILedgerService, LedgerRepository>();
        services.AddScoped<IUserService, UserRepository>();
        services.AddScoped<IRoleService, RoleRepository>();
        services.AddScoped<IOpenBranchService, OpenBranchService>();
        services.AddScoped<IOccupationService, OccupationRepository>();
        services.AddScoped<IPeriodService, PeriodRepository>();
        services.AddScoped<ILoanPurposeService, LoanPurposeRepository>();
        services.AddScoped<IOtherNameService, OtherNameRepository>();
        services.AddScoped<ILoginService, LoginService>();
        services.AddScoped<IBranchService, BranchRepository>();
        services.AddScoped<IBankService, BankRepository>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IItemService, ItemRepository>();
        services.AddScoped<IEmployeeService, EmployeeRepository>();
        services.AddScoped<IEntityNameService, EntityNameService>();
        services.AddScoped<IAddressService, AddressRepository>();
        services.AddScoped<ICashVaultService, CashVaultService>();
        services.AddScoped<ICashVaultService, CashVaultService>();
        services.AddScoped<ICurrencyService, CurrencyRepository>();
        services.AddScoped<ICreditAgentService, CreditAgentService>();
        services.AddScoped<ICustomerService, CustomerRepository>();
        services.AddScoped<ICustomerIncomeExpendService, IncomeExpendRepository>();
        services.AddScoped<ICustomerNoteService, CustomerNoteRepository>();
        services.AddScoped<ICollateralsService, CollateralsRepository>();
        services.AddScoped<ICustomerActivityService, CustomerActivityRepository>();
        services.AddScoped<ICollateralService, CollateralRepository>();
        services.AddScoped<IHolidayService, HolidayRepository>();
        services.AddScoped<ILocationService, LocationRepository>();
        services.AddScoped<IGuarantorService, GuarantorRepository>();
        services.AddScoped<ILoanSettingService, LoanSettingRepository>();
        services.AddScoped<ILoanProductService, LoanProductService>();
        services.AddScoped<ILoanPaymentService, LoanPaymentRepository>();
        services.AddScoped<ILoanScheduleService, LoanScheduleService>();
        services.AddScoped<ILoanService, LoanRepository>();
        services.AddScoped<ILoanCategoryService, LoanCategoryService>();
        services.AddScoped<ILoanRequestService, LoanRequestRepository>();
        services.AddScoped<ILoanReportService, LoanReportService>();
        services.AddScoped<ISalutationService, SalutaionRepository>();
        services.AddScoped<INationalityService, NationalityRepository>();
        services.AddScoped<IFileUploadService, FileUploadRepository>();
        services.AddScoped<IVendorService, VendorRepository>();
        services.AddScoped<IScheduleService, LoanScheduleRepository>();
        services.AddScoped<ICollateralValuationService, CollateralValuationRepository>();
        services.AddScoped<INotificationService, NotificationRepository>();
        services.AddSingleton<IDapperConnectionFactory, DapperConnectionFactory>();
        services.AddScoped<IDashboardService, DashboardRepository>();
        services.AddScoped<IDocumentServices, DocumentRepository>();
        return services;

    }

}
