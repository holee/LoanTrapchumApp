
using Application.Extensions;

namespace Application;

public static class ApplicationServiceInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var assembly = typeof(ApplicationServiceInjection).Assembly;

        services.AddValidatorsFromAssembly(assembly);
        services.AddScoped<IMediator, SimpleMediator>();
        services.AddRequestHandlersFromAssembly(assembly);
        services.AddScoped<ScheduleService>();
        services.AddScoped<SchedulePrintService>();
        services.AddScoped<IAddRecieptNumber, AddRecieptNumberUseCase>();
        services.AddScoped<IGetCustomersByUseCase, GetCustomersByUseCase>();
        services.AddScoped<IAddressServiceImpl, AddressService>();
        services.AddScoped<IBankServiceImpl, BankService>();
        services.AddScoped<IOpenBranchServiceImpl, OpenBranchService>();
        //services.AddScoped<ICurrencyServiceImpl, CurrencyService>();
        services.AddScoped<ICashVaultServiceImpl, CashVaultService>();
        services.AddScoped<IHolidayServiceImpl, HolidayService>();
        services.AddScoped<ICreditAgentServiceImpl, CreditAgentService>();
        services.AddScoped<ICollateralsServiceImpl, CollateralsService>();
        services.AddScoped<IFileUploadServiceImp, FileUploadService>();
        services.AddScoped<IEntityNameServiceImpl, EntityNameService>();
        services.AddScoped<ILoginServiceImpl, LoginService>();
        services.AddScoped<IUserServiceImpl, UserService>();
        services.AddScoped<IForiegnCurrencyExchangeImpl, ForiegnCurrencyExchange>();
        //services.AddScoped<IJournalServiceImpl, JournalService>();
        services.AddScoped<ILoanCategoryServiceImpl, LoanCategoryService>();
        services.AddScoped<ILoanProductServiceImpl, LoanProductService>();
        services.AddScoped<ILoanReportServiceImpl, LoanReportServiceImpl>();
        services.AddScoped<INationalityServiceImpl, NationalityService>();
        services.AddScoped<IOtherNameServiceImpl, OtherNameService>();
        services.AddScoped<IGuarantorServiceImpl, GuarantorService>();
        services.AddScoped<ISettingServiceImpl, SettingService>();
        services.AddScoped<IVendorServiceImpl, VendorService>();
        services.AddScoped<IPrintExelServiceImpl, PrintExelServiceImpl>();

        return services;
    }
}
