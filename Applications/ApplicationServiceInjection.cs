using Applications.Features.Holidays.Command.ActivateOrDeactivate;
using Applications.Features.Holidays.Command.CreateHoliday;
using Applications.Features.Holidays.Queries.GetActiveHolidays;
using Applications.Features.Holidays.Queries.GetHolidayByDate;
using Applications.Features.Holidays.Queries.GetHolidayById;
using Applications.Features.Schedules.Queries.GetDueScheduleByLoanId;
using ErrorOr;
namespace Applications;

public static class ApplicationServiceInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IRequestHandler<GetDueScheduleByLoanIdQuery, Pager<VSchedule>>, GetDueScheduleByLoanIdQueryHandler>();
        services.AddScoped<IRequestHandler<ActivateOrDeactivateHolidayCommand, ErrorOr<HolidayVm>>, ActivateOrDeactivateHolidayCommandHandler>();
        services.AddScoped<IRequestHandler<GetHolidayByDateQuery, IEnumerable<HolidayVm>>, GetHolidayByDateQueryHandler>();
        services.AddScoped<IRequestHandler<GetHolidayByIdQuery, ErrorOr<Holiday>>, GetHolidayByIdQueryHandler>();
        services.AddScoped<IRequestHandler<GetActiveHolidaysQuery, Pager<HolidayVm>>, GetActiveHolidaysQueryHandler>();
        services.AddScoped<IRequestHandler<CreateHolidaysCommand, ErrorOr<IEnumerable<HolidayVm>>>, CreateHolidaysCommandHandler>();
        services.AddScoped<IRequestHandler<DeleteHolidayCommand, ErrorOr<DeleteResult>>, DeleteHolidayCommandHandler>();
        services.AddScoped<IRequestHandler<GetHolidaysQuery, IEnumerable<HolidayVm>>, GetHolidaysQueryHandler>();
        services.AddScoped<IRequestHandler<GetHolidaysAsPagingQuery, Pager<HolidayVm>>, GetHolidaysAsPagingQueryHandler>();
        services.AddScoped<IRequestHandler<CreateHolidaysCommand, ErrorOr<IEnumerable<HolidayVm>>>, CreateHolidaysCommandHandler>();
        services.AddScoped<IRequestHandler<CreateHolidayCommand, ErrorOr<HolidayVm>>, CreateHolidayCommandHandler>();
        services.AddScoped<IRequestHandler<GetUserDetailsByIdQuery, ErrorOr<VUser>>, GetUserDetailsByIdQueryHandler>();
        services.AddScoped<IRequestHandler<GetBranchByUserQuery, ErrorOr<VBranch>>, GetBranchByUserQueryHandler>();
        services.AddScoped<IRequestHandler<GetDbManagerLoanSummaryQuery, ErrorOr<DbManagerLoanSummary>>, GetDbManagerLoanSummaryQueryHandler>();
        services.AddScoped<IRequestHandler<GetNumberLoanRequestQuery, ErrorOr<NumerLoanRequest>>, GetNumberLoanRequestQueryHandler>();
        services.AddScoped<IRequestHandler<GetDbCreditOfficeLoanSummaryQuery, Pager<DbCoLoanSummary>>, GetDbCreditOfficeLoanSummaryQueryHandler>();
        services.AddScoped<IRequestHandler<GetDbApprovedRequestQuery, ErrorOr<int>>, GetDbApprovedRequestHandler>();
        services.AddScoped<IRequestHandler<GetDbAccountLoanSummaryQuery, Pager<AccountantLoanSummary>>, GetDbAccountLoanSummaryQueryHandler>();
        services.AddScoped<IRequestHandler<GetRolesQuery, IEnumerable<string>>, GetRolesQueryHandler>();
        services.AddScoped<IRequestHandler<GetLoanScheduleByScheduleQuery, ErrorOr<LoanResponse>>, GetLoanScheduleByScheduleQueryHandler>();
        services.AddScoped<IRequestHandler<GetLoanWithScheduleQuery, ErrorOr<LoanResponse>>, GetLoanWithScheduleQueryHandler>();
        services.AddScoped<IRequestHandler<GetCitiesQuery, PageMetaData<CityDto>>, GetCitiesQueryHandler>();
        services.AddScoped<IRequestHandler<GetCommunesQuery, PageMetaData<CommuneDto>>, GetCommunesQueryHandler>();
        services.AddScoped<IRequestHandler<GetDistrictsQuery, PageMetaData<DistrictDto>>, GetDistrictsQueryHandler>();
        services.AddScoped<IRequestHandler<GetProvincesQuery, PageMetaData<ProvinceDto>>, GetProvincesQueryHandler>();
        services.AddScoped<IRequestHandler<GetVillagesQuery, PageMetaData<VillageDto>>, GetVillagesQueryHandler>();
        services.AddScoped<IRequestHandler<CreateCityCommand, ErrorOr<CityDto>>, CreateCityCommandHandler>();
        services.AddScoped<IRequestHandler<UpdateCityCommand, ErrorOr<CityDto>>, UpdateCityCommandHandler>();
        services.AddScoped<IRequestHandler<CreateJournalCommand, ErrorOr<JournalEntry?>>, CreateJournalCommandHandler>();
        services.AddScoped<IRequestHandler<GetJournalByIdQuery, ErrorOr<JournalEntry>>, GetJournalByIdQueryHandler>();
        services.AddScoped<IRequestHandler<GetJournalEntryDetailByIdQuery, ErrorOr<JournalEntryDto>>, GetJournalEntryDetailByIdHandler>();
        services.AddScoped<IRequestHandler<GetJournalTypesQuery, Pager<JournalTypeDto>>, GetJournalTypesQueryHandler>();
        services.AddScoped<IRequestHandler<GetBranchStateQuery, ErrorOr<BranchStatus>>, GetBranchStateQueryHandler>();
        services.AddScoped<IRequestHandler<GetCurrenciesDtoQuery, Pager<CurrencyDto>>, GetCurrenciesDtoQueryHandler>();
        services.AddScoped<IRequestHandler<GetBranchForVaultQuery, ErrorOr<VBranch>>, GetBranchForVaultQueryHandler>();
        services.AddScoped<IRequestHandler<GetBranchDetailsByIdQuery, ErrorOr<VBranch?>>, GetBranchDetailsByIdQueryHandler>();
        services.AddScoped<IRequestHandler<GetBranchByIdQuery, ErrorOr<Branch>>, GetBranchByIdQueryHandler>();
        services.AddScoped<IRequestHandler<GetBranchAsPagingQuery, Pager<VBranch>>, GetBranchAsPagingQueryHandler>();
        services.AddScoped<IRequestHandler<DeleteBranchCommand, ErrorOr<DeleteResult>>, DeleteBranchCommandHandler>();
        services.AddScoped<IRequestHandler<GetUpdateBranchQuery, ErrorOr<BranchDtoUpdate>>, GetUpdateBranchQueryHandler>();
        services.AddScoped<IRequestHandler<CreateBranchCommand, ErrorOr<VBranch>>, CreateBranchCommandHandler>();
        services.AddScoped<IRequestHandler<EditBranchCommand, ErrorOr<VBranch>>, EditBranchCommandHandler>();
        services.AddScoped<IRequestHandler<GetBranchsByUserQuery, ErrorOr<IEnumerable<VBranch>>>, GetBranchsByUserQueryHandler>();
        services.AddScoped<IRequestHandler<OpenBranchCommand, ErrorOr<BranchStatus>>, OpenBranchCommandHandler>();
        services.AddScoped<IRequestHandler<CloseBranchCommand, ErrorOr<BranchStatus>>, CloseBranchCommandHandler>();
        services.AddScoped<IRequestHandler<GetCashVaultBalanceQuery, ErrorOr<IEnumerable<VCashVaultBalance>>>, GetCashVaultBalanceQueryHandler>();
        services.AddScoped<IRequestHandler<GetCurrencyNotesQuery, Pager<VCurrencyNotes>>, GetCurrencyNotesQueryHandler>();
        services.AddScoped<IRequestHandler<GetCurrencyCounterQuery, Pager<VCurrencyCounter>>, GetCurrencyCounterQueryHandler>();
        services.AddScoped<IRequestHandler<GetCurrencyDetailsByIdQuery, ErrorOr<VCurrenciesList>>, GetCurrencyDetailsByIdQueryHandler>();
        services.AddScoped<IRequestHandler<GetCurrenciesQuery, Pager<Currency>>, GetCurrenciesQueryHandler>();
        services.AddScoped<IRequestHandler<GetCashVaultQuery, Pager<VCashVaults>>, GetCashVaultQueryHandler>();
        services.AddScoped<IRequestHandler<AddCurrencyNoteCommand, ErrorOr<VCurrencyCounter>>, AddCurrencyNoteCommandHandler>();
        services.AddScoped<IRequestHandler<StartCashVaultIn, ErrorOr<string>>, CreateCashVaultInHandler>();
        services.AddScoped<IRequestHandler<StartCashVaultOut, ErrorOr<string>>, StartCashVaultOutHandler>();
        services.AddScoped<IRequestHandler<GetDepartmentAsPagingQuery, Pager<VDepartment>>, GetDepartmentAsPagingQueryHandler>();
        services.AddScoped<IRequestHandler<GetDepartmentDetailsById, ErrorOr<VDepartment>>, GetDepartmentDetailsByIdHandler>();
        services.AddScoped<IRequestHandler<GetDepartmentById, ErrorOr<Department>>, GetDepartmentByIdHandler>();
        services.AddScoped<IRequestHandler<CreateDepartmentCommand, ErrorOr<VDepartment>>, CreateDepartmentCommandHandler>();
        services.AddScoped<IRequestHandler<EditDepartmentCommand, ErrorOr<VDepartment>>, EditDepartmentCommandHandler>();
        services.AddScoped<IRequestHandler<DeleteDepartmentCommand, ErrorOr<string>>, DeleteDepartmentCommandHandler>();
        services.AddScoped<IRequestHandler<ChangeDepartmentStateCommand, ErrorOr<VDepartment>>, ChangeDepartmentStateCommandHandler>();
        services.AddScoped<IRequestHandler<CreateGuarantorCommand, ErrorOr<VGuarantor>>, CreateGuarantorCommandHandler>();
        services.AddScoped<IRequestHandler<GetLoanRequestApprovedQuery, ErrorOr<ApprovedRequest>>, GetLoanRequestApprovedQueryHandler>();
        services.AddScoped<IRequestHandler<GetApprovedLoanRequestQuery, ErrorOr<VLoanRequests>>, GetApprovedLoanRequestQueryHandler>();
        services.AddScoped<IRequestHandler<CreateLoanCommand, ErrorOr<DisburseResult>>, CreateLoanCommandHandler>();
        services.AddScoped<IRequestHandler<GetLoanWithScedulesQuery, ErrorOr<IEnumerable<LoanResponse>>>, GetLoanWithScedulesQueryHandler>();
        services.AddScoped<IRequestHandler<GetEmployeeAsPagingQuery, Pager<VEmployeeList>>, GetEmployeeAsPagingQueryHandler>();
        services.AddScoped<IRequestHandler<ChangeEmployeeStateCommand, ErrorOr<VEmployeeList>>, ChangeEmployeeStateCommandHandler>();
        services.AddScoped<IRequestHandler<GetEmployeeDetailsByIdQuery, ErrorOr<VEmployeeList>>, GetEmployeeDetailsByIdQueryHandler>();
        services.AddScoped<IRequestHandler<EditEmployeeCommand, ErrorOr<VEmployeeList>>, EditEmployeeCommandHandler>();
        services.AddScoped<IRequestHandler<CreateEmployeeCommand, ErrorOr<VEmployeeList>>, CreateEmployeeCommandHandler>();
        services.AddScoped<IRequestHandler<GetEmployeeByIdQuery, ErrorOr<EntityNames>>, GetEmployeeByIdQueryHandler>();
        services.AddScoped<IRequestHandler<GetEditEmployeeQuery, ErrorOr<EntityEmployeeCreate>>, GetEditEmployeeQueryHandler>();

        //GetLoanRequestApprovedQuery

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
        //services.AddScoped<IBranchServiceImpl, BranchService>();
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


        services.AddScoped<IRequestHandler<GetCurrencyDetailsByIdQuery, ErrorOr<VCurrenciesList>>, GetCurrencyDetailsByIdQueryHandler>();
        services.AddScoped<IRequestHandler<GetCurrencyByIdQuery, ErrorOr<CurrencyDto>>, GetCurrencyByIdQueryHandler>();
        services.AddScoped<IRequestHandler<CreateMoneyExchangeCommand, ErrorOr<VttCurrencyExchanges>>, CreateMoneyExchangeCommandHandler>();
        services.AddScoped<IRequestHandler<EditMoneyExchangeCommand, ErrorOr<VttCurrencyExchanges>>, EditMoneyExchangeCommandHandler>();



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
        services.AddScoped<IRequestHandler<CreateCustomerCommand, ErrorOr<VCustomerList>>, CreateCustomerCommandHandler>();
        services.AddScoped<IRequestHandler<EditCustomerCommand, ErrorOr<VCustomerList>>, EditCustomerCommandHandler>();


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
        services.AddScoped<IRequestHandler<GetLoanRequestDocumentQuery, ErrorOr<IEnumerable<VLoanRequestsDocuments>>>, GetLoanRequestDocumentQueryHandler>();


        //Commands for Loans disubursed
        services.AddScoped<IRequestHandler<GetLoanRelatedGuarantorsQuery, IEnumerable<VLoanDisbursementGuarantors>>, GetLoanRelatedGuarantorsQueryHandler>();
        services.AddScoped<IRequestHandler<GetLoanRelatedDocumentsQuery, IEnumerable<VLoanDisbursementDocuments>>, GetLoanRelatedDocumentsQueryHandler>();
        services.AddScoped<IRequestHandler<GetLoanRelatedCollateralsQuery, IEnumerable<VLoanDisbursementCollateral>>, GetLoanRelatedCollateralsQueryHandler>();





        services.AddScoped<IRequestHandler<CloseLoanDisbursementCommand, ErrorOr<string>>, CloseLoanDisbursementCommandHandler>();



        services.AddScoped<IRequestHandler<RecievePaymentCommand, ErrorOr<string>>, RecievePaymentCommandHandler>();



        services.AddScoped<IRequestHandler<GetLoanSchedulesByLoanIdQuery, ErrorOr<IEnumerable<VSchedule>>>, GetLoanSchedulesByLoanIdQueryHandler>();
        services.AddScoped<IRequestHandler<GetLoanByIdQuery, ErrorOr<VLoanDisbursments>>, GetLoanByIdQueryHandler>();
        services.AddScoped<IRequestHandler<GetLoanByBranchIdQuery, Pager<VLoanDisbursments>>, GetLoanByBranchIdQueryHandler>();

        //
        services.AddScoped<IRequestHandler<GetUnreadNotificationQuery, int>, GetUnreadNotificationQueryHandler>();
        services.AddScoped<IRequestHandler<GetNotificationAsPagingQuery, Pager<NotificationDto>>, GetNotificationAsPagingQueryHandler>();
        services.AddScoped<IRequestHandler<MarkUnreadNotificationCommand, ErrorOr<NotificationDto>>, MarkUnreadNotificationCommandHandler>();
        services.AddScoped<IRequestHandler<GetUnreadNotificationSummaryQuery, NotificationSummaryDto>, GetUnreadNotificationQuerySummaryHandler>();
        services.AddScoped<IRequestHandler<GetUnreadNotificationsQuery, Pager<NotificationDto>>, GetUnreadNotificationsQueryHandler>();
        services.AddScoped<IRequestHandler<CreateNotificationCommand, ErrorOr<Guid>>, CreateNotificationCommandHandler>();




        ///Schedules
        services.AddScoped<IRequestHandler<GetLoanSchedulesMonthToDateQuery, Pager<VSchedule>>, GetLoanSchedulesMonthToDateQueryHandler>();

        services.AddScoped<IRequestHandler<GetLoanSchedulesByPaymentDateQuery, Pager<VSchedule>>, GetLoanSchedulesByPaymentDateQueryHandler>();



        //Exchanged
        services.AddScoped<IRequestHandler<GetEditMoneyExchangeQuery, ErrorOr<ExchangeDtoEdit>>, GetEditMoneyExchangeQueryHandler>();
        services.AddScoped<IRequestHandler<GetMoneyExchangeAsPagingQuery, Pager<VttCurrencyExchanges>>, GetMoneyExchangeQueryAsPagingHandler>();

        return services;

    }
}
