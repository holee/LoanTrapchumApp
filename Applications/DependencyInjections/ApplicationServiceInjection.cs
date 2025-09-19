using Applications.Features.Addresses.Interfaces;
using Applications.Features.Banks;
using Applications.Features.Branches;
using Applications.Features.Branches.Interfaces;
using Applications.Features.CashManagements.Interfaces;
using Applications.Features.CashManagements.UseCases;
using Applications.Features.Currencies;
using Applications.Features.Customers.Services;
using Applications.Features.Customers.UseCases;
using Applications.Features.Employees.Interfaces;
using Applications.Features.Employees.UseCases;
using Applications.Features.LoanPayments.Interfaces;
using Applications.Features.LoanPayments.UseCases;
using Applications.Features.LoanRequests.Interfaces;
using Applications.Features.LoanRequests.UseCases;
using Applications.Features.Loans.Services;
using Applications.Features.Settings.UseCases;
using Applications.Services.Files;
using Applications.Services.Identity;
using Applications.Services.Lists;
using Applications.Services.Loans;
using Applications.Services.Reports;
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

            services.AddScoped<ICreateNewLoanRequestUseCase,CreateNewLoanRequestUseCase>();

            services.AddScoped<ISaveLoanRequestUseCase, SaveLoanRequestUseCase>();

            services.AddScoped<IApprovedLoanRequestUseCase, ApprovedLoanRequestUseCase>();

            services.AddScoped<IDeniedLoanRequestUseCase, DeniedLoanRequestUseCase>();

            services.AddScoped<IRecheckedLoanRequestUseCase, RecheckedLoanRequestUseCase>();

            services.AddScoped<IEditLoanRequestUseCase, EditLoanRequestUseCase>();

            services.AddScoped<IAddRecieptNumberUseCase, AddRecieptNumberUseCase>();

            services.AddScoped<IStartVaultInUseCase, StartVaultInUseCase>();

            services.AddScoped<IStartVaultOutUseCase, StartVaultOutUseCase>();

            services.AddScoped<IGetCashVaultUseCase, GetCashVaultUseCase>();








            services.AddScoped<ScheduleService>();
            services.AddScoped<CustomerCollateralServices>();
            services.AddScoped<AddCustomerActivityUseCase>();
            services.AddScoped<AddCustomerNotesUseCase>();
            services.AddScoped<GetCustomerActivityUseCase>();
            services.AddScoped<GetCustomerNotesUseCase>();
            services.AddScoped<GetCustomersCommand>();
            services.AddScoped<GetCustomerDetailByIdCommand>();
            services.AddScoped<GetCustomerByIdCommand>();
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
    }
}
