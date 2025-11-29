using Applications.Bases;
using Applications.Features.Activities.ActitvityQuery.GetActivitiesQuery;
using Applications.Features.Activities.ActitvityQuery.GetActivityByCustomer;
using Applications.Features.Activities.ActitvityQuery.GetActivityById;
using Applications.Features.Activities.Command.CreateActivity;
using Applications.Features.Activities.Command.DeleteActivity;
using Applications.Features.Activities.Command.EditActivity;
using Applications.Features.Addresses.Interfaces;
using Applications.Features.Banks;
using Applications.Features.Branches;
using Applications.Features.Branches.Interfaces;
using Applications.Features.CashManagements.Interfaces;
using Applications.Features.CashManagements.UseCases;
using Applications.Features.Collaterals.Commands.AddOrUpdateCollaterals;
using Applications.Features.Collaterals.Commands.CreateCollateralLines;
using Applications.Features.Collaterals.Commands.DeleteCollateralLines;
using Applications.Features.Collaterals.Dtos;
using Applications.Features.Collaterals.Queries.GetCollateralDetails;
using Applications.Features.Collaterals.Queries.GetCollateralLineByCollateralId;
using Applications.Features.Collaterals.Queries.GetCollateralLineById;
using Applications.Features.Collaterals.Queries.GetCollateralLineDetails;
using Applications.Features.Collaterals.Queries.GetCollateralLinesAsPaging;
using Applications.Features.Currencies;
using Applications.Features.Customers.Commands.Customers.Queries.GetCustomerAsPaging;
using Applications.Features.Customers.Commands.Customers.Queries.GetCustomerById;
using Applications.Features.Customers.Interfaces;
using Applications.Features.Customers.Queries.GetCustomerAsPaging;
using Applications.Features.Customers.Response;
using Applications.Features.Customers.Services;
using Applications.Features.Customers.UseCases;
using Applications.Features.Employees.Interfaces;
using Applications.Features.Employees.UseCases;
using Applications.Features.Expenses.CreateExpenseCommands;
using Applications.Features.Expenses.CreateExpensesCommands;
using Applications.Features.Expenses.DeleteExpenseCommand;
using Applications.Features.Expenses.Dto;
using Applications.Features.Expenses.EditExpenseCommands;
using Applications.Features.Expenses.EditExpensesCommands;
using Applications.Features.Expenses.Queries.GetExpenseByCustomerId;
using Applications.Features.Expenses.Queries.GetExpenseById;
using Applications.Features.Incomes.Command.CreateIncome;
using Applications.Features.Incomes.Command.CreateIncomeExpenseSource;
using Applications.Features.Incomes.Command.CreateIncomes;
using Applications.Features.Incomes.Command.DeleteIncome;
using Applications.Features.Incomes.Command.EditIncome;
using Applications.Features.Incomes.Command.EditIncomeExpenseSource;
using Applications.Features.Incomes.Command.GetEditIncomeExpenseSource;
using Applications.Features.Incomes.Dto;
using Applications.Features.Incomes.Queries.GetEditIncome;
using Applications.Features.Incomes.Queries.GetEditIncomeExpenseSource;
using Applications.Features.Incomes.Queries.GetIncomeByCustomerId;
using Applications.Features.Incomes.Queries.GetIncomeById;
using Applications.Features.Incomes.Queries.GetIncomeExpense;
using Applications.Features.LoanPayments.Interfaces;
using Applications.Features.LoanPayments.UseCases;
using Applications.Features.LoanRequests.Command.AddDescription;
using Applications.Features.LoanRequests.Command.AddLoanRequestCollateral;
using Applications.Features.LoanRequests.Command.AddLoanRequestDescription;
using Applications.Features.LoanRequests.Command.AddLoanRequestDocument;
using Applications.Features.LoanRequests.Command.AddLoanRequestGuarantor;
using Applications.Features.LoanRequests.Command.ApplyLoanRequest;
using Applications.Features.LoanRequests.Command.ApprovedLoanRequest;
using Applications.Features.LoanRequests.Command.CreateLoanRequest;
using Applications.Features.LoanRequests.Command.DeleteLoanRequestCollateral;
using Applications.Features.LoanRequests.Command.DeleteLoanRequestDocument;
using Applications.Features.LoanRequests.Command.DeleteLoanRequestGuarantor;
using Applications.Features.LoanRequests.Command.DenyLoanRequest;
using Applications.Features.LoanRequests.Command.IsCustomerBusy;
using Applications.Features.LoanRequests.Command.RecheckedLoanRequest;
using Applications.Features.LoanRequests.Command.SaveLoanRequest;
using Applications.Features.LoanRequests.Command.VerifiedLoanRequest;
using Applications.Features.LoanRequests.Queryies.GetLoanRequestAsPaging;
using Applications.Features.LoanRequests.Queryies.GetLoanRequestById;
using Applications.Features.LoanRequests.Queryies.GetLoanRequestDetailById;
using Applications.Features.LoanRequests.Queryies.GetLoanRequestsWithCanDisburse;
using Applications.Features.Loans.Queries.GetLoanRelatedCollaterals;
using Applications.Features.Loans.Queries.GetLoanRelatedDocuments;
using Applications.Features.Loans.Queries.GetLoanRelatedGuarantors;
using Applications.Features.Loans.Responses;
using Applications.Features.Loans.Services;
using Applications.Features.Notes.Command.CreateNote;
using Applications.Features.Notes.Command.DeleteNote;
using Applications.Features.Notes.Command.EditNote;
using Applications.Features.Notes.Queries.GetNoteByCustomerId;
using Applications.Features.Notes.Queries.GetNoteById;
using Applications.Features.Settings.UseCases;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Customers;
using Applications.Responses.ViewModels.Loans.Requests;
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
using ErrorOr;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
namespace Applications
{
    public static class ApplicationServiceInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {

            // services.AddScoped<ICreateNewLoanRequestUseCase, CreateNewLoanRequestUseCase>();

            //services.AddScoped<ISaveLoanRequestUseCase, SaveLoanRequestUseCase>();

            //services.AddScoped<IApprovedLoanRequestUseCase, ApprovedLoanRequestUseCase>();

            //services.AddScoped<IDeniedLoanRequestUseCase, DeniedLoanRequestUseCase>();

            //services.AddScoped<IRecheckedLoanRequestUseCase, RecheckedLoanRequestUseCase>();

            //services.AddScoped<IEditLoanRequestUseCase, EditLoanRequestUseCase>();

            services.AddScoped<IAddRecieptNumberUseCase, AddRecieptNumberUseCase>();

            services.AddScoped<IStartVaultInUseCase, StartVaultInUseCase>();

            services.AddScoped<IStartVaultOutUseCase, StartVaultOutUseCase>();

            services.AddScoped<IGetCashVaultUseCase, GetCashVaultUseCase>();


            services.AddScoped<IGetCustomersByUseCase, GetCustomersByUseCase>();





            services.AddScoped<ScheduleService>();
            services.AddScoped<GetCollateralDetailQueryHandler>();
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
            //services.AddScoped<ILoanRequestServiceImpl, LoanRequestService>();
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





            services.AddValidatorsFromAssemblyContaining<GetCollateralLineDetailQueryValidator>();
            services.AddScoped<IMediator, SimpleMediator>();


            services.AddScoped<IRequestHandler<AddOrUpdateCollateralLineCommand, ErrorOr<string>>, AddOrUpdateCollateralLineCommandHandler>();
            services.AddScoped<IRequestHandler<AddOrUpdateCollateralCommand, ErrorOr<string>>, AddOrUpdateCollateralCommandHandler>();
            services.AddScoped<IRequestHandler<DeleteCollateralLineCommand, ErrorOr<DeleteResult>>, DeleteCollateralLineCommandHandler>();
            services.AddScoped<IRequestHandler<GetCollateralLineByCollateralIdQuery, ErrorOr<IEnumerable<VCollateralLines>>>, GetCustomerCollateralLineByIdHandler>();
            services.AddScoped<IRequestHandler<GetCollateralLineDetailQuery, ErrorOr<VCollateralLines>>, GetCollateralLineDetailQueryHandler>();
            services.AddScoped<IRequestHandler<GetCollateralDetailQuery, ErrorOr<VCustomerCollaterals>>, GetCollateralDetailQueryHandler>();
            services.AddScoped<IRequestHandler<GetCollateralLineByIdEditQuery, ErrorOr<CollateralLineDtoCreate>>, GetCollateralLineByIdQueryEditHandler>();
            services.AddScoped<IRequestHandler<GetCollateralLineAsPagingQuery, Pager<VCollateralLines>>, GetColleralLineAsPagingQueryHandler>();

            ///Activities
            services.AddScoped<IRequestHandler<DeleteActivityCommand, ErrorOr<bool>>, DeleteActivityCommandHandler>();
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
            services.AddScoped<IRequestHandler<GetIncomeExpenseQuery, Pager<IncomeExpenseDto>>, GetIncomeExpenseAsPagingQuery>();






            services.AddScoped<IRequestHandler<GetEditIncomeCommand, ErrorOr<EditIncomeDto>>, GetEditIncomeCommandHandler>();
            services.AddScoped<IRequestHandler<GetExpenseByCustomerQuery, ErrorOr<IEnumerable<VCustomerExpenses>>>, GetCustomerExpenseQueryHandler>();
            services.AddScoped<IRequestHandler<GetExpenseByIdQuery, ErrorOr<VCustomerExpenses>>, GetExpenseByIdQueryHandler>();
            services.AddScoped<IRequestHandler<EditExpenseCommand, ErrorOr<long>>, EditExpenseCommandHandler>();
            services.AddScoped<IRequestHandler<EditExpensesCommand, ErrorOr<string>>, EditExpensesCommandHandler>();
            services.AddScoped<IRequestHandler<AddExpensesCommand, ErrorOr<string>>, AddExpensesCommandHandler>();
            services.AddScoped<IRequestHandler<AddExpenseCommand, ErrorOr<long>>, AddExpenseCommandHandler>();
            services.AddScoped<IRequestHandler<DeleteExpenseCommand, ErrorOr<string>>, DeleteExpenseCommandHandler>();
            services.AddScoped<IRequestHandler<GetExpenseByIdUpdateQuery, ErrorOr<ExpenseDtoUpdate>>, GetExpenseByIdUpdateQueryHandler>();




            services.AddScoped<IRequestHandler<GetCustomerByIdQuery, ErrorOr<EntityNames>>, GetCustomerByIdQueryHandler>();
            services.AddScoped<IRequestHandler<GetCustomersQuery, Pager<VCustomerList>>, GetCustomersQueryHandler>();




            services.AddScoped<IRequestHandler<GetCustomerAsPagingQuery, Pager<VCustomerList>>, GetCustomerAsPagingQueryHandler>();


            services.AddScoped<IRequestHandler<CreateIncomeExpenseCommand, ErrorOr<int>>, CreateIncomeExpenseCommandHandler>();
            services.AddScoped<IRequestHandler<EditIncomeExpenseCommand, ErrorOr<int>>, EditIncomeExpenseCommandHandler>();
            services.AddScoped<IRequestHandler<GetEditIncomeExpenseQuery, ErrorOr<IncomeExpenseDto>>, GetEditIncomeExpenseQueryHandler>();


            //// Loan Requests Commands and Queries
            services.AddScoped<IRequestHandler<SaveLoanRequestCommand, ErrorOr<RequestResult>>, SaveLoanRequestCommandHandler>();
            services.AddScoped<IRequestHandler<CreateLoanRequestCommand, ErrorOr<RequestResult>>, CreateLoanRequestCommandHandler>();
            services.AddScoped<IRequestHandler<DeleteLoanRequestCollateralCommand, ErrorOr<string>>, DeleteLoanRequestCollateralCommandHandler>();
            services.AddScoped<IRequestHandler<DeleteLoanRequestDocumentCommand, ErrorOr<string>>, DeleteLoanRequestDocumentCommandHandler>();
            services.AddScoped<IRequestHandler<DeleteLoanRequestGuarantorCommand, ErrorOr<string>>, DeleteLoanRequestGuarantorCommandHandler>();
            services.AddScoped<IRequestHandler<ApprovedLoanRequestCommand, ErrorOr<RequestResult>>, ApprovedLoanRequestCommandHandler>();
            services.AddScoped<IRequestHandler<AppliedLoanRequestCommand, ErrorOr<RequestResult>>, AppliedLoanRequestCommandHandler>();
            services.AddScoped<IRequestHandler<AddLoanRequestGuarantorCommand, ErrorOr<string>>, AddLoanRequestGuarantorCommandHandler>();
            services.AddScoped<IRequestHandler<AddLoanRequestDocumentsCommand, ErrorOr<string>>, AddLoanRequestDocumentsCommandHandler>();
            services.AddScoped<IRequestHandler<AddLoanRequestDescriptionCommand, ErrorOr<string>>, AddLoanRequestDescriptionCommandHandler>();
            services.AddScoped<IRequestHandler<AddLoanRequestCollateralCommand, ErrorOr<string>>, AddLoanRequestCollateralCommandHandler>();
            services.AddScoped<IRequestHandler<AddDescriptionCommand, ErrorOr<string>>, AddDescriptionCommandHandler>();
            services.AddScoped<IRequestHandler<DeniedLoanRequestCommand, ErrorOr<RequestResult>>, DeniedLoanRequestCommandHandler>();
            services.AddScoped<IRequestHandler<RecheckedLoanRequestCommand, ErrorOr<RequestResult>>, RecheckedLoanRequestCommandHandler>();
            services.AddScoped<IRequestHandler<VerifiedLoanRequestCommand, ErrorOr<RequestResult>>, VerifiedLoanRequestCommandHandler>();
            services.AddScoped<IRequestHandler<IsCustomerBusyQuery, ErrorOr<bool>>, IsCustomerBusyQueryHandler>();
            services.AddScoped<IRequestHandler<GetLoanRequestAsPagingQuery, Pager<VLoanRequests>>, GetLoanRequestAsPagingQueryHandler>();
            services.AddScoped<IRequestHandler<GetLoanRequestDetailByIdQuery, ErrorOr<VLoanRequests>>, GetLoanRequestDetailByIdQueryHandler>();
            services.AddScoped<IRequestHandler<GetLoanRequestByIdQuery, ErrorOr<VLoanRequests>>, GetLoanRequestByIdQueryHandler>();
            services.AddScoped<IRequestHandler<GetLoanRequestsWithCanDisburseQuery, Pager<VLoanRequests>>, GetLoanRequestsWithCanDisburseQueryHandler>();


            //Commands for Loans disubursed
            services.AddScoped<IRequestHandler<GetLoanRelatedGuarantorsQuery, IEnumerable<VLoanGuarantors>>, GetLoanRelatedGuarantorsQueryHandler>();
            services.AddScoped<IRequestHandler<GetLoanRelatedDocumentsQuery, IEnumerable<VLoanRequestsDocuments>>, GetLoanRelatedDocumentsQueryHandler>();
            services.AddScoped<IRequestHandler<GetLoanRelatedCollateralsQuery, IEnumerable<VCollateralLines>>, GetLoanRelatedCollateralsQueryHandler>();



            return services;

        }
    }
}
