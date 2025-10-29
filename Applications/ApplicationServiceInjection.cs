using Applications.Bases;
using Applications.Features.Addresses.Interfaces;
using Applications.Features.Banks;
using Applications.Features.Branches;
using Applications.Features.Branches.Interfaces;
using Applications.Features.CashManagements.Interfaces;
using Applications.Features.CashManagements.UseCases;
using Applications.Features.Collaterals.Commands.CollateralLineQuery;
using Applications.Features.Collaterals.Commands.DeleteCOmmand;
using Applications.Features.Collaterals.Commands.Queries.GetCollateralLineByCollateralId;
using Applications.Features.Collaterals.Commands.Queries.GetCollateralLineById;
using Applications.Features.Currencies;
using Applications.Features.Customers.Commands.ActivitiesCommand.ActitvityQuery.GetActivityByCustomer;
using Applications.Features.Customers.Commands.ActivitiesCommand.ActitvityQuery.GetActivityById;
using Applications.Features.Customers.Commands.ActivitiesCommand.CreateActivity;
using Applications.Features.Customers.Commands.ActivitiesCommand.EditActivity;
using Applications.Features.Customers.Commands.Customers.Queries.GetCustomerAsPaging;
using Applications.Features.Customers.Commands.Customers.Queries.GetCustomerById;
using Applications.Features.Customers.Commands.ExpenseCommands.CreateExpenseCommands;
using Applications.Features.Customers.Commands.ExpenseCommands.Queries.GetExpenseByCustomerId;
using Applications.Features.Customers.Commands.Expenses.CreateExpensesCommands;
using Applications.Features.Customers.Commands.Expenses.DeleteExpenseCommand;
using Applications.Features.Customers.Commands.Expenses.EditExpenseCommands;
using Applications.Features.Customers.Commands.GetCustomerActivityQuery;
using Applications.Features.Customers.Commands.IncomeCommands.Queries.GetIncomeByCustomerId;
using Applications.Features.Customers.Commands.IncomeCommands.Queries.GetIncomeById;
using Applications.Features.Customers.Commands.Incomes.CreateIncomeCommands;
using Applications.Features.Customers.Commands.Incomes.CreateIncomes;
using Applications.Features.Customers.Commands.Incomes.DeleteIncome;
using Applications.Features.Customers.Commands.Incomes.EditIncome;
using Applications.Features.Customers.Commands.Incomes.Queries.GetEditIncome;
using Applications.Features.Customers.Commands.Incomes.Queries.GetIncomeByCustomerId;
using Applications.Features.Customers.Commands.Incomes.Queries.GetIncomeById;
using Applications.Features.Customers.Commands.NotesCommand.CreateNoteCommand;
using Applications.Features.Customers.Commands.NotesCommand.DeleteNoteCommand;
using Applications.Features.Customers.Commands.NotesCommand.EditNoteCommand;
using Applications.Features.Customers.Commands.NotesCommand.Queries.GetNoteByCustomerId;
using Applications.Features.Customers.Commands.NotesCommand.Queries.GetNoteById;
using Applications.Features.Customers.Interfaces;
using Applications.Features.Customers.Response;
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
using Applications.Responses.Lists.Collaterals;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Customers;
using Applications.Services.Files;
using Applications.Services.Identity;
using Applications.Services.Lists;
using Applications.Services.Loans;
using Applications.Services.Reports;
using Applications.Services.Transactions.Branches;
using Applications.Services.Transactions.Currencies;
using Applications.Services.Transactions.Journals;
using Applications.Utils;
using Domains.Models.Lists;
using Domains.Models.Lists.Customers;
using ErrorOr;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
namespace Applications
{
    public static class ApplicationServiceInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {

            services.AddScoped<ICreateNewLoanRequestUseCase, CreateNewLoanRequestUseCase>();

            services.AddScoped<ISaveLoanRequestUseCase, SaveLoanRequestUseCase>();

            services.AddScoped<IApprovedLoanRequestUseCase, ApprovedLoanRequestUseCase>();

            services.AddScoped<IDeniedLoanRequestUseCase, DeniedLoanRequestUseCase>();

            services.AddScoped<IRecheckedLoanRequestUseCase, RecheckedLoanRequestUseCase>();

            services.AddScoped<IEditLoanRequestUseCase, EditLoanRequestUseCase>();

            services.AddScoped<IAddRecieptNumberUseCase, AddRecieptNumberUseCase>();

            services.AddScoped<IStartVaultInUseCase, StartVaultInUseCase>();

            services.AddScoped<IStartVaultOutUseCase, StartVaultOutUseCase>();

            services.AddScoped<IGetCashVaultUseCase, GetCashVaultUseCase>();


            services.AddScoped<IGetCustomersByUseCase, GetCustomersByUseCase>();





            services.AddScoped<ScheduleService>();
            services.AddScoped<CustomerCollateralServices>();
            services.AddScoped<AddCustomerActivityUseCase>();
            services.AddScoped<GetCustomerActivityUseCase>();
            services.AddScoped<GetCustomersUseCase>();
            services.AddScoped<GetIncomeByCustomerQueryHandler>();
            services.AddScoped<GetCustomerDetailByIdUseCase>();
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





            services.AddValidatorsFromAssemblyContaining<GetCollateralLineQueryValiator>();
            services.AddScoped<IMediator, SimpleMediator>();

            services.AddScoped<IRequestHandler<DeleteCollateralLineCommand, ErrorOr<DeleteResult>>, DeleteCollateralLineCommandHandler>();
            services.AddScoped<IRequestHandler<GetCollateralLineByCollateralIdQuery, ErrorOr<IEnumerable<VCollateralLines>>>, GetCustomerCollateralLineByIdHandler>();
            services.AddScoped<IRequestHandler<GetCollateralLineDetailQuery, ErrorOr<VCollateralLines>>, GetCollateralLineDetailQueryHandler>();


            ///Activities

            services.AddScoped<IRequestHandler<CreateActivityCommand, ErrorOr<int>>, CreateActivityCommandHandler>();
            services.AddScoped<IRequestHandler<EditActivityCommand, ErrorOr<int>>, EditActivityCommandHandler>();
            services.AddScoped<IRequestHandler<GetCustomerActivityQuery, IEnumerable<VCustomerActivities>>, GetCustomerActivityQueryHandler>();
            services.AddScoped<IRequestHandler<GetActivityByCustomerIdQuery, ErrorOr<IEnumerable<VCustomerActivities>>>, GetActivityByCustomerIdQueryHandler>();
            services.AddScoped<IRequestHandler<GetActivityByIdQuery, ErrorOr<VCustomerActivities>>, GetActivityByIdQueryHandler>();


            services.AddScoped<IRequestHandler<GetNoteByIdQuery, ErrorOr<VCustomerNotes>>, GetNoteByIdQueryHandler>();
            services.AddScoped<IRequestHandler<GetNotesByCustomerIdQuery, ErrorOr<IEnumerable<VCustomerNotes>>>, GetNotesByCustomerIdQueryHandler>();
            services.AddScoped<IRequestHandler<AddNoteCommand, ErrorOr<int>>, CreateNoteCommandHandler>();
            services.AddScoped<IRequestHandler<EditNoteCommand, ErrorOr<int>>, EditNoteCommandHandler>();
            services.AddScoped<IRequestHandler<DeleteNoteCommand, ErrorOr<bool>>, DeleteNoteCommandHandler>();

            services.AddScoped<IRequestHandler<GetIncomeByCustomerQuery, ErrorOr<IEnumerable<VCustomerIncomes>>>, GetIncomeByCustomerQueryHandler>();
            services.AddScoped<IRequestHandler<GetIncomeByIdQuery, ErrorOr<VCustomerIncomes>>, GetIncomeByIdQueryHandler>();
            services.AddScoped<IRequestHandler<CreateIncomeCommand, ErrorOr<long>>, CreateIncomeCommandHandler>();
            services.AddScoped<IRequestHandler<CreateIncomesCommand, ErrorOr<string>>, CreateIncomesCommandHandler>();
            services.AddScoped<IRequestHandler<DeleteIncomeCommand, ErrorOr<bool>>, DeleteIncomeCommandHandler>();
            services.AddScoped<IRequestHandler<EditIncomeCommand, ErrorOr<long>>, EditIncomeCommandHandler>();
      


            services.AddScoped<IRequestHandler<GetEditIncomeCommand, ErrorOr<CustomerIncome>>, GetEditIncomeCommandHandler>();
            services.AddScoped<IRequestHandler<GetExpenseByCustomerQuery, ErrorOr<IEnumerable<VCustomerExpenses>>>, GetCustomerExpenseQueryHandler>();
            services.AddScoped<IRequestHandler<GetExpenseByIdQuery, ErrorOr<VCustomerExpenses>>, GetExpenseByIdQueryHandler>();
            services.AddScoped<IRequestHandler<EditExpenseCommand, ErrorOr<long>>, EditExpenseCommandHandler>();
            services.AddScoped<IRequestHandler<EditExpensesCommand, ErrorOr<string>>, EditExpensesCommandHandler>();
            services.AddScoped<IRequestHandler<AddExpensesCommand, ErrorOr<string>>, AddExpensesCommandHandler>();
            services.AddScoped<IRequestHandler<AddExpenseCommand, ErrorOr<long>>, AddExpenseCommandHandler>();
            services.AddScoped<IRequestHandler<DeleteExpenseCommand, ErrorOr<string>>, DeleteExpenseCommandHandler>();




            services.AddScoped<IRequestHandler<GetCustomerByIdQuery, ErrorOr<EntityNames>>, GetCustomerByIdQueryHandler>();
            services.AddScoped<IRequestHandler<GetCustomersQuery, Pager<VCustomerList>>, GetCustomersQueryHandler>();






            return services;

        }
    }
}
