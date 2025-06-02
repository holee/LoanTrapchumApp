using Applications.Interfaces.Addresses;
using Applications.Services.Addresses;
using Applications.Services.Banks;
using Applications.Services.Branches;
using Applications.Services.Currencies;
using Applications.Services.Customers;
using Applications.Services.Employees;
using Applications.Services.Files;
using Applications.Services.Identity;
using Applications.Services.Lists;
using Applications.Services.LIsts;
using Applications.Services.Loans;
using Applications.Services.Settings;
using Applications.Services.Transactions.Branches;
using Applications.Services.Transactions.Currencies;
using Applications.Services.Transactions.Journals;
using Microsoft.Extensions.DependencyInjection;
namespace Applications.DependencyInjections
{
    public static class ApplicationServiceInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IAddressServiceImpl, AddressService>();
            services.AddTransient<IBranchServiceImpl, BranchService>();
            services.AddTransient<IBankServiceImpl, BankService>();
            services.AddTransient<IOpenBranchServiceImpl, OpenBranchService>();
            services.AddTransient<ICurrencyServiceImpl, CurrencyService>();
            services.AddTransient<ICashVaultServiceImpl, CashVaultService>();
            services.AddTransient<ICustomerServiceImpl, CustomerService>();
            services.AddTransient<IIncomeExpendServiceImpl, CustomerIncomeExpendService>();
            services.AddTransient<IHolidayServiceImpl, HolidayService>();
            services.AddTransient<ICreditAgentServiceImpl, CreditAgentService>();
            services.AddTransient<ICollateralsServiceImpl, CollateralsService>();
            services.AddTransient<IEmployeeServiceImpl, EmployeeService>();
            services.AddTransient<IFileUploadServiceImp, FileUploadService>();
            services.AddTransient<IDepartmentServiceImpl, DepartmentService>();
            services.AddTransient<IEntityNameServiceImpl, EntityNameService>();
            services.AddTransient<ICreditAgentServiceImpl, CreditAgentService>();
            services.AddTransient<ILoginServiceImpl, LoginService>();
            services.AddTransient<IUserServiceImpl, UserService>();
            services.AddTransient<IForiegnCurrencyExchangeImpl, ForiegnCurrencyExchange>();
            services.AddTransient<IJournalServiceImpl, JournalService>();
            services.AddTransient<ILedgerServiceImpl, LedgerService>();
            services.AddTransient<ILoanRequestServiceImpl, LoanRequestService>();
            services.AddTransient<ILoanServiceImpl, LoanService>();
            services.AddTransient<ILoanScheduleServiceImpl, LoanScheduleService>();
            services.AddTransient<ILoanCategoryServiceImpl, LoanCategoryService>();
            services.AddTransient<ILoanProductServiceImpl, LoanProductService>();
            services.AddTransient<ILoanPaymentServiceImpl, LoanPaymentService>();
            services.AddTransient<ILoanSettingServiceImpl, LoanSettingService>();
            services.AddTransient<INationalityServiceImpl, NationalityService>();
            services.AddTransient<IOccupationServiceImpl, OccupationService>();
            services.AddTransient<IOtherNameServiceImpl, OtherNameService>();
            services.AddTransient<IPrintPdfServiceImpl, PrintPdfServiceImpl>();
            services.AddTransient<IGuarantorServiceImpl, GuarantorService>();
            services.AddTransient<IRoleServiceImpl, RoleService>();
            services.AddTransient<ISpotRateServiceImpl, SpotRateService>();
            services.AddTransient<ISettingServiceImpl, SettingService>();
            services.AddTransient<ISalutationServiceImpl, SalutationService>();
            services.AddTransient<IVendorServiceImpl, VendorService>();
            services.AddTransient<IFileUploadServiceImp, FileUploadService>();
            services.AddTransient<IPrintExelServiceImpl, PrintExelServiceImpl>();
            return services;

        }
    }
}
