
using Applications.Features.Holidays.Queries.GetHolidaysAsPaging;
using Applications.Features.LoanPayments.Command.RecievePayment;
using Applications.Features.LoanPayments.Queries.GetLoanSchedulesByLoanIdAsPaging;
using Applications.Features.LoanRequests.Command.SaveEditLoanRequest;
using Applications.Features.LoanRequests.Queryies.GetLoanRequestsDocuments;
using Applications.Features.Loans.Command.CloseLoanDisbursment;
using Applications.Features.Loans.Queries.GetLoanById;
using Applications.Responses.ViewModels.Loans.Schedules;
using ErrorOr;

namespace Applications;

public static class ApplicationServiceInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
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
        ///
        services.AddScoped<IRequestHandler<SaveEditLoanRequestCommand, ErrorOr<RequestResult>>, SaveEditLoanRequestCommandHandler>();
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
        services.AddScoped<IRequestHandler<GetLoanRequestDocumentQuery,ErrorOr<IEnumerable<VLoanRequestsDocuments>>>, GetLoanRequestDocumentQueryHandler>();


        //Commands for Loans disubursed
        services.AddScoped<IRequestHandler<GetLoanRelatedGuarantorsQuery, IEnumerable<VLoanGuarantors>>, GetLoanRelatedGuarantorsQueryHandler>();
        services.AddScoped<IRequestHandler<GetLoanRelatedDocumentsQuery, IEnumerable<VLoanRequestsDocuments>>, GetLoanRelatedDocumentsQueryHandler>();
        services.AddScoped<IRequestHandler<GetLoanRelatedCollateralsQuery, IEnumerable<VCollateralLines>>, GetLoanRelatedCollateralsQueryHandler>();




        services.AddScoped<IRequestHandler<CreateHolidaysCommand, ErrorOr<IEnumerable<HolidayVm>>>, CreateHolidaysCommandHandler>();
        services.AddScoped<IRequestHandler<DeleteHolidayCommand, ErrorOr<DeleteResult>>, DeleteHolidayCommandHandler>();
        services.AddScoped<IRequestHandler<GetHolidaysQuery, IEnumerable<HolidayVm>>, GetHolidaysQueryHandler>();
        services.AddScoped<IRequestHandler<GetHolidaysAsPagingQuery, Pager<HolidayVm>>, GetHolidaysAsPagingQueryHandler>();
        services.AddScoped<IRequestHandler<CloseLoanDisbursementCommand, ErrorOr<string>>, CloseLoanDisbursementCommandHandler>();



        services.AddScoped<IRequestHandler<RecievePaymentCommand, ErrorOr<string>>, RecievePaymentCommandHandler>();



        services.AddScoped<IRequestHandler<GetLoanSchedulesByLoanIdQuery, ErrorOr<IEnumerable<VSchedule>>>, GetLoanSchedulesByLoanIdQueryHandler>();
        services.AddScoped<IRequestHandler<GetLoanByIdQuery, ErrorOr<VLoanDisbursments>>, GetLoanByIdQueryHandler>();

        //
        //
        return services;

    }
}
