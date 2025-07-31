using Applications.Features.Customers.Services;
using Applications.Features.Customers.Validations;
using Applications.Features.Identity.Roles.Validation;
using Applications.Features.LoanRequests.Services;
using Applications.Features.LoanRequests.Validations;
using Applications.Features.Loans.Services;
using Applications.Interfaces.Addresses;
using Applications.Requests.Identity;
using Applications.Requests.Lists.Collaterals;
using Applications.Requests.Loans.LoanRequests;
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
using Applications.Services.Reports;
using Applications.Services.Settings;
using Applications.Services.Transactions.Branches;
using Applications.Services.Transactions.Currencies;
using Applications.Services.Transactions.Journals;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
namespace Applications.DependencyInjections
{
    public static class ApplicationServiceInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ScheduleService>();
            services.AddScoped<CustomerCollateralServices>();
            services.AddScoped<CustomerNoteService>();
            services.AddScoped<SchedulePrintService>();
            services.AddScoped<IAddressServiceImpl, AddressService>();
            services.AddScoped<IBranchServiceImpl, BranchService>();
            services.AddScoped<IBankServiceImpl, BankService>();
            services.AddScoped<IOpenBranchServiceImpl, OpenBranchService>();
            services.AddScoped<ICurrencyServiceImpl, CurrencyService>();
            services.AddScoped<ICashVaultServiceImpl, CashVaultService>();
            services.AddScoped<ICustomerServiceImpl, CustomerService>();
            services.AddScoped<IIncomeExpendServiceImpl, CustomerIncomeExpendService>();
            services.AddScoped<IHolidayServiceImpl, HolidayService>();
            services.AddScoped<ICreditAgentServiceImpl, CreditAgentService>();
            services.AddScoped<ICollateralsServiceImpl, CollateralsService>();
            services.AddScoped<IEmployeeServiceImpl, EmployeeService>();
            services.AddScoped<IFileUploadServiceImp, FileUploadService>();
            services.AddScoped<IDepartmentServiceImpl, DepartmentService>();
            services.AddScoped<IEntityNameServiceImpl, EntityNameService>();
            services.AddScoped<ICreditAgentServiceImpl, CreditAgentService>();
            services.AddScoped<ILoginServiceImpl, LoginService>();
            services.AddScoped<IUserServiceImpl, UserService>();
            services.AddScoped<IForiegnCurrencyExchangeImpl, ForiegnCurrencyExchange>();
            services.AddScoped<IJournalServiceImpl, JournalService>();
            services.AddScoped<ILedgerServiceImpl, LedgerService>();
            services.AddScoped<ILoanRequestServiceImpl, LoanRequestService>();
            services.AddScoped<ILoanServiceImpl, LoanService>();
            services.AddScoped<ILoanScheduleServiceImpl, LoanScheduleService>();
            services.AddScoped<ILoanCategoryServiceImpl, LoanCategoryService>();
            services.AddScoped<ILoanProductServiceImpl, LoanProductService>();
            services.AddScoped<ILoanPaymentServiceImpl, LoanPaymentService>();
            services.AddScoped<ILoanReportServiceImpl, LoanReportServiceImpl>();
            services.AddScoped<ILoanSettingServiceImpl, LoanSettingService>();
            services.AddScoped<INationalityServiceImpl, NationalityService>();
            services.AddScoped<IOccupationServiceImpl, OccupationService>();
            services.AddScoped<IOtherNameServiceImpl, OtherNameService>();
            services.AddScoped<IPrintPdfServiceImpl, PrintPdfServiceImpl>();
            services.AddScoped<IGuarantorServiceImpl, GuarantorService>();
            services.AddScoped<IRoleServiceImpl, RoleService>();
            services.AddScoped<ISpotRateServiceImpl, SpotRateService>();
            services.AddScoped<ISettingServiceImpl, SettingService>();
            services.AddScoped<ISalutationServiceImpl, SalutationService>();
            services.AddScoped<IVendorServiceImpl, VendorService>();
            services.AddScoped<IFileUploadServiceImp, FileUploadService>();
            services.AddScoped<IPrintExelServiceImpl, PrintExelServiceImpl>();
            return services;

        }

        public static IServiceCollection AddValidationServices(this IServiceCollection services)
        {

            services.AddScoped<IValidator<RoleDtoUpdate>, RoleDtoUpdateValidator>();
            services.AddScoped<IValidator<AproveLoanDtoCreate>, AprovedLoanDtoCreateValidator>();
            services.AddScoped<IValidator<CollateralDtoCreate>, CollateralCreateValidator>();
            services.AddScoped<IValidator<CollateralLineDtoCreate>, CollateralLineCreateValidator>();
            
            return services;

        }
    }
}
