
using Applications.Features.Collaterals.Queries.GetCollateralSubTypesById;
using Applications.Features.Collaterals.Queries.GetCollateralTypes;
using Applications.Features.Holidays.Dtos;
using Applications.Features.LoanRequests.Command.AddCoPawnCustomer;
using Applications.Features.LoanRequests.Command.AddCoPawnCustomers;
using Applications.Features.LoanRequests.Command.DeleteCoPawnCustomer;
using Applications.Features.LoanRequests.Dtos;
using Applications.Features.LoanRequests.Queryies.IsCoPawnExist;
using Applications.Features.Locations.Command.CreateLocation;
using Applications.Features.Locations.Command.DeleteLocation;
using Applications.Features.Locations.Command.UpdateLocation;
using Applications.Features.Locations.Dtos;
using Applications.Features.Locations.Queries.GetAllLocations;
using Applications.Features.Locations.Queries.GetLocationById;
using Applications.Features.Locations.Queries.GetLocationsAsPaging;
using Applications.Features.OtherNames.Dto;
using Applications.Features.Settings.Queries.GetApprovedUsers;
using Applications.Features.Settings.Queries.GetLoanCategories;
using Applications.Features.Vendors.Commands.ChangeVendorStatus;
using Applications.Features.Vendors.Commands.CreateVendor;
using Applications.Features.Vendors.Commands.DeleteVendor;
using Applications.Features.Vendors.Commands.UpdateVendor;
using Applications.Features.Vendors.Dto.Responses;
using Applications.Features.Vendors.Queries.GetUpdateVendorById;
using Applications.Features.Vendors.Queries.GetVendorById;
using Applications.Features.Vendors.Queries.GetVendorsAsPaging;
using Applications.Responses.Loans.Disbursements;
using Applications.Responses.ViewModels.Ledgers;
using Domains.Models.Collaterals;
using ErrorOr;
namespace Applications;

public static class ApplicationServiceInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<GetCollateralLineDetailQueryValidator>();
        services.AddScoped<IMediator, SimpleMediator>();

        //Locations Services
        services.AddScoped<IRequestHandler<GetLocationsAsPagingQuery, Pager<LocationDto>>, GetLocationsAsPagingQueryHandler>();
        services.AddScoped<IRequestHandler<GetLocationByIdQuery, ErrorOr<LocationDto>>, GetLocationByIdQueryHandler>();
        services.AddScoped<IRequestHandler<GetAllLocationsQuery, IEnumerable<LocationDto>>, GetAllLocationsQueryHandler>();
        services.AddScoped<IRequestHandler<CreateLocationCommand, ErrorOr<LocationDto>>, CreateLocationCommandHandler>();
        services.AddScoped<IRequestHandler<UpdateLocationCommand, ErrorOr<LocationDto>>, UpdateLocationCommandHandler>();
        services.AddScoped<IRequestHandler<DeleteLocationCommand, ErrorOr<string>>, DeleteLocationCommandHandler>();
        services.AddScoped<IRequestHandler<GetCollateralTypeQuery, IEnumerable<CollateralTypeDto>>, GetCollateralTypeQueryHandler>();



        //Vendor Services
        services.AddScoped<IRequestHandler<GetUpdateVendorByIdQuery, ErrorOr<EntityVendorCreate>>, GetUpdateVendorByIdQueryHandler>();
        services.AddScoped<IRequestHandler<CreateVendorCommand, ErrorOr<VendorDto>>, CreateVendorCommandHandler>();
        services.AddScoped<IRequestHandler<UpdateVendorCommand, ErrorOr<VendorDto>>, UpdateVendorCommandHandler>();
        services.AddScoped<IRequestHandler<ChangeVendorStatusCommand, ErrorOr<Updated>>, ChangeVendorStatusCommandHandler>();
        services.AddScoped<IRequestHandler<DeleteVendorCommand, ErrorOr<Deleted>>, DeleteVendorCommandHandler>();
        services.AddScoped<IRequestHandler<GetVendorsAsPagingQuery, Pager<VendorDto>>, GetVendorsAsPagingQueryHandler>();
        services.AddScoped<IRequestHandler<GetVendorByIdQuery, ErrorOr<VendorDto>>, GetVendorByIdQueryHandler>();


        //Guarantors
        services.AddScoped<IRequestHandler<GetGuarantorsAsPagingQuery, Pager<VGuarantor>>, GetGuarantorsAsPagingQueryHandler>();
        services.AddScoped<IRequestHandler<GetGuarantorDetailByIdQuery, ErrorOr<VGuarantor>>, GetGuarantorDetailByIdQueryHandler>();
        services.AddScoped<IRequestHandler<GetGuarantorByIdQuery, ErrorOr<VGuarantor>>, GetGuarantorByIdQueryHandler>();
        services.AddScoped<IRequestHandler<GetUpdateGuarantorByIdQuery, ErrorOr<EntityGuarantorCreate>>, GetUpdateGuarantorByIdQueryHandler>();
        services.AddScoped<IRequestHandler<UpdateGuarantorCommand, ErrorOr<VGuarantor>>, UpdateGuarantorCommandHandler>();


        services.AddScoped<IRequestHandler<GetOccupationsQuery, Pager<OccupationDto>>, GetOccupationsQueryHandler>();
        services.AddScoped<IRequestHandler<CreateOccupationCommand, ErrorOr<OccupationDto>>, CreateOccupationCommandHandler>();
        services.AddScoped<IRequestHandler<UpdateOccupationCommand, ErrorOr<OccupationDto>>, UpdateOccupationCommandHandler>();

        // Periods
        services.AddScoped<IRequestHandler<GetPeriodsQuery, Pager<PeriodDto>>, GetPeriodsQueryHandler>();
        services.AddScoped<IRequestHandler<CreatePeriodCommand, ErrorOr<PeriodDto>>, CreatePeriodCommandHandler>();
        services.AddScoped<IRequestHandler<UpdatePeriodCommand, ErrorOr<PeriodDto>>, UpdatePeriodCommandHandler>();
        services.AddScoped<IRequestHandler<DeletePeriodCommand, ErrorOr<bool>>, DeletePeriodCommandHandler>();
        services.AddScoped<IRequestHandler<GetLoanPurposesQuery, Pager<LoanPurposeDto>>, GetLoanPurposesQueryHandler>();
        services.AddScoped<IRequestHandler<GetLoanPurposeByIdQuery, LoanPurposeDto?>, GetLoanPurposeByIdQueryHandler>();
        services.AddScoped<IRequestHandler<CreateLoanPurposeCommand, ErrorOr<LoanPurposeDto>>, CreateLoanPurposeCommandHandler>();
        services.AddScoped<IRequestHandler<UpdateLoanPurposeCommand, ErrorOr<LoanPurposeDto>>, UpdateLoanPurposeCommandHandler>();
        services.AddScoped<IRequestHandler<DeleteLoanPurposeCommand, ErrorOr<bool>>, DeleteLoanPurposeCommandHandler>();
        services.AddScoped<IRequestHandler<GetNationalitiesQuery, Pager<NationalityDto>>, GetNationalitiesQueryHandler>();
        services.AddScoped<IRequestHandler<GetSalutationsQuery, Pager<SalutationDto>>, GetSalutationsQueryHandler>();
        services.AddScoped<IRequestHandler<CreateSalutationCommand, ErrorOr<SalutationDto>>, CreateSalutationCommandHandler>();

        // Documents Injection
        services.AddScoped<IRequestHandler<AddDocumentCommand, ErrorOr<DocumentDto>>, AddDocumentCommandHandler>();
        services.AddScoped<IRequestHandler<AddDocumentsCommand, IEnumerable<DocumentDto>>, AddDocumentsCommandHandler>();
        services.AddScoped<IRequestHandler<DeleteDocumentCommand, bool>, DeleteDocumentCommandHandler>();
        services.AddScoped<IRequestHandler<GetDocumentByIdQuery, ErrorOr<DocumentDto>>, GetDocumentByIdQueryHandler>();
        services.AddScoped<IRequestHandler<GetDocumentsQuery, IEnumerable<DocumentDto>>, GetDocumentsQueryHandler>();



        services.AddScoped<IRequestHandler<GetPaidScheduleQuery, ErrorOr<IEnumerable<VSchedule>>>, GetPaidScheduleQueryHandler>();
        services.AddScoped<IRequestHandler<GetScheduleByIdQuery, ErrorOr<VSchedule>>, GetScheduleByIdQueryHandler>();
        services.AddScoped<IRequestHandler<PayOffCommand, ErrorOr<PayOffResult>>, PayOffCommandHandler>();

        services.AddScoped<IRequestHandler<SaveLoanRequestByManagerCommand, ErrorOr<RequestResult>>, SaveLoanRequestByManagerCommandHandler>();
        services.AddScoped<IRequestHandler<GetScheduleDetailByIdQuery, ErrorOr<ScheduleDetailsDto>>, GetScheduleDetailByIdQueryHandler>();
        services.AddScoped<IRequestHandler<GetScheduleByIdQuery, ErrorOr<VSchedule>>, GetScheduleByIdQueryHandler>();
        services.AddScoped<IRequestHandler<GetDueScheduleByLoanIdQuery, Pager<VSchedule>>, GetDueScheduleByLoanIdQueryHandler>();
        services.AddScoped<IRequestHandler<ActivateOrDeactivateHolidayCommand, ErrorOr<HolidayDto>>, ActivateOrDeactivateHolidayCommandHandler>();
        services.AddScoped<IRequestHandler<GetHolidayByDateQuery, IEnumerable<HolidayDto>>, GetHolidayByDateQueryHandler>();
        services.AddScoped<IRequestHandler<GetHolidayByIdQuery, ErrorOr<Holiday>>, GetHolidayByIdQueryHandler>();
        services.AddScoped<IRequestHandler<GetActiveHolidaysQuery, Pager<HolidayDto>>, GetActiveHolidaysQueryHandler>();
        services.AddScoped<IRequestHandler<CreateHolidaysCommand, ErrorOr<IEnumerable<HolidayDto>>>, CreateHolidaysCommandHandler>();
        services.AddScoped<IRequestHandler<DeleteHolidayCommand, ErrorOr<DeleteResult>>, DeleteHolidayCommandHandler>();
        services.AddScoped<IRequestHandler<GetHolidaysQuery, IEnumerable<HolidayDto>>, GetHolidaysQueryHandler>();
        services.AddScoped<IRequestHandler<GetHolidaysAsPagingQuery, Pager<HolidayDto>>, GetHolidaysAsPagingQueryHandler>();
        services.AddScoped<IRequestHandler<CreateHolidaysCommand, ErrorOr<IEnumerable<HolidayDto>>>, CreateHolidaysCommandHandler>();
        services.AddScoped<IRequestHandler<CreateHolidayCommand, ErrorOr<HolidayDto>>, CreateHolidayCommandHandler>();
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

        services.AddScoped<IAddRecieptNumber, AddRecieptNumberUseCase>();
        //services.AddScoped<IStartVaultInUseCase, StartVaultInUseCase>();
        //services.AddScoped<IStartVaultOutUseCase, StartVaultOutUseCase>();
        //services.AddScoped<IGetCashVaultUseCase, GetCashVaultUseCase>();
        services.AddScoped<IGetCustomersByUseCase, GetCustomersByUseCase>();
        services.AddScoped<ScheduleService>();
        //services.AddScoped<GetCollateralDetailQueryHandler>();
        //services.AddScoped<AddCustomerActivityUseCase>();
        //services.AddScoped<GetCustomerActivityUseCase>();
        //services.AddScoped<GetIncomeByCustomerQueryHandler>();
        //services.AddScoped<GetCustomerDetailByIdUseCase>();
        //services.AddScoped<GetCustomerByIdCommand>();
        services.AddScoped<SchedulePrintService>();
        services.AddScoped<IAddressServiceImpl, AddressService>();
        services.AddScoped<IBankServiceImpl, BankService>();
        services.AddScoped<IOpenBranchServiceImpl, OpenBranchService>();
        services.AddScoped<ICurrencyServiceImpl, CurrencyService>();
        services.AddScoped<ICashVaultServiceImpl, CashVaultService>();
        services.AddScoped<IHolidayServiceImpl, HolidayService>();
        services.AddScoped<ICreditAgentServiceImpl, CreditAgentService>();
        services.AddScoped<ICollateralsServiceImpl, CollateralsService>();
        services.AddScoped<IFileUploadServiceImp, FileUploadService>();
        services.AddScoped<IEntityNameServiceImpl, EntityNameService>();
        services.AddScoped<ICreditAgentServiceImpl, CreditAgentService>();
        services.AddScoped<ILoginServiceImpl, LoginService>();
        services.AddScoped<IUserServiceImpl, UserService>();
        services.AddScoped<IForiegnCurrencyExchangeImpl, ForiegnCurrencyExchange>();
        services.AddScoped<IJournalServiceImpl, JournalService>();
        services.AddScoped<ILedgerServiceImpl, LedgerService>();
        services.AddScoped<ILoanServiceImpl, LoanService>();
        services.AddScoped<ILoanScheduleServiceImpl, LoanScheduleService>();
        services.AddScoped<ILoanCategoryServiceImpl, LoanCategoryService>();
        services.AddScoped<ILoanProductServiceImpl, LoanProductService>();
        services.AddScoped<ILoanReportServiceImpl, LoanReportServiceImpl>();
        //services.AddScoped<ILoanSettingServiceImpl, LoanSettingService>();
        services.AddScoped<INationalityServiceImpl, NationalityService>();
        services.AddScoped<IOtherNameServiceImpl, OtherNameService>();
        services.AddScoped<IPrintPdfServiceImpl, PrintPdfServiceImpl>();
        services.AddScoped<IGuarantorServiceImpl, GuarantorService>();
        services.AddScoped<IRoleServiceImpl, RoleService>();
        services.AddScoped<ISpotRateServiceImpl, SpotRateService>();
        services.AddScoped<ISettingServiceImpl, SettingService>();
        services.AddScoped<IVendorServiceImpl, VendorService>();
        services.AddScoped<IFileUploadServiceImp, FileUploadService>();
        services.AddScoped<IPrintExelServiceImpl, PrintExelServiceImpl>();








        services.AddScoped<IRequestHandler<GetCurrencyDetailsByIdQuery, ErrorOr<VCurrenciesList>>, GetCurrencyDetailsByIdQueryHandler>();
        services.AddScoped<IRequestHandler<GetCurrencyByIdQuery, ErrorOr<CurrencyDto>>, GetCurrencyByIdQueryHandler>();
        services.AddScoped<IRequestHandler<CreateMoneyExchangeCommand, ErrorOr<VttCurrencyExchanges>>, CreateMoneyExchangeCommandHandler>();
        services.AddScoped<IRequestHandler<EditMoneyExchangeCommand, ErrorOr<VttCurrencyExchanges>>, EditMoneyExchangeCommandHandler>();


        ///Collateral Services
        services.AddScoped<IRequestHandler<AddOrUpdateCollateralLineCommand, ErrorOr<string>>, AddOrUpdateCollateralLineCommandHandler>();
        services.AddScoped<IRequestHandler<AddOrUpdateCollateralCommand, ErrorOr<string>>, AddOrUpdateCollateralCommandHandler>();
        services.AddScoped<IRequestHandler<DeleteCollateralLineCommand, ErrorOr<DeleteResult>>, DeleteCollateralLineCommandHandler>();
        services.AddScoped<IRequestHandler<GetCollateralLineByCollateralIdQuery, ErrorOr<IEnumerable<VCollateralLines>>>, GetCustomerCollateralLineByIdHandler>();
        services.AddScoped<IRequestHandler<GetCollateralLineDetailQuery, ErrorOr<VCollateralLines>>, GetCollateralLineDetailQueryHandler>();
        services.AddScoped<IRequestHandler<GetCollateralDetailQuery, ErrorOr<VCustomerCollaterals>>, GetCollateralDetailQueryHandler>();
        services.AddScoped<IRequestHandler<GetCollateralLineByIdEditQuery, ErrorOr<CollateralLineDtoCreate>>, GetCollateralLineByIdQueryEditHandler>();
        services.AddScoped<IRequestHandler<GetCollateralLineAsPagingQuery, Pager<VCollateralLines>>, GetColleralLineAsPagingQueryHandler>();
        services.AddScoped<IRequestHandler<GetCustomerCollateralLinesQuery, Pager<VCollateralLines>>, GetCustomerCollateralLinesQueryHandler>();
        services.AddScoped<IRequestHandler<GetCollateralLineByIdQuery, ErrorOr<CollateralLine>>, GetCollateralLineByIdQueryHandler>();
        services.AddScoped<IRequestHandler<GetCollateralSubTypesByIdQuery, IEnumerable<CollateralSubTypeDto>>, GetCollateralSubTypesByIdQueryHandler>();




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
        services.AddScoped<IRequestHandler<DeleteIncomeExpenseCommand, ErrorOr<string>>, DeleteIncomeExpenseCommandHandler>();







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
        services.AddScoped<IRequestHandler<GetCustomerAsPagingQuery, Pager<VCustomerList>>, GetCustomerAsPagingQueryHandler>();
        services.AddScoped<IRequestHandler<GetActiveLoanCustomerQuery, Pager<VCustomerList>>, GetActiveLoanCustomerQueryHandler>();
        services.AddScoped<IRequestHandler<CreateCustomerCommand, ErrorOr<VCustomerList>>, CreateCustomerCommandHandler>();
        services.AddScoped<IRequestHandler<EditCustomerCommand, ErrorOr<VCustomerList>>, EditCustomerCommandHandler>();
        services.AddScoped<IRequestHandler<DeleteCustomerCommand, DeleteResult>, DeleteCustomerCommandHandler>();
        services.AddScoped<IRequestHandler<DeactivateCustomerCommand, ErrorOr<bool>>, DeactivateCustomerCommandHandler>();
        services.AddScoped<IRequestHandler<GetCustomerDetailByIdQuery, ErrorOr<VCustomerDetails>>, GetCustomerDetailByIdQueryHandler>();
        services.AddScoped<IRequestHandler<GetLoanByCustomerIdQuery, Pager<VLoanDisbursments>>, GetLoanByCustomerIdQueryHandler>();
        services.AddScoped<IRequestHandler<GetUpdateCustomerByIdQuery, ErrorOr<EntityCustomerCreate>>, GetUpdateCustomerByIdQueryHandler>();
        services.AddScoped<IRequestHandler<GetRequestByCustomerIdQuery, Pager<VLoanRequests>>, GetRequestByCustomerIdQueryHandler>();


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
        services.AddScoped<IRequestHandler<ApprovedRequestCommand, ErrorOr<RequestResult>>, ApprovedLoanRequestCommandHandler>();
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
        services.AddScoped<IRequestHandler<SaveLoanRequestsCommand, ErrorOr<RequestResult>>, SaveLoanRequestsCommandHandler>();
        services.AddScoped<IRequestHandler<AddCoPawnCustomerCommand, ErrorOr<CoPawnCustomerDto?>>, AddCoPawnCustomerCommandHandler>();
        services.AddScoped<IRequestHandler<AddCoPawnCustomersCommand, ErrorOr<IEnumerable<CoPawnCustomerDto?>>>, AddCoPawnCustomersCommandHandler>();
        services.AddScoped<IRequestHandler<DeleteCoPawnCustomerCommand, ErrorOr<bool>>, DeleteCoPawnCustomerCommandHandler>();
        services.AddScoped<IRequestHandler<IsCoPawnExistQuery, ErrorOr<bool>>, IsCoPawnExistQueryHandler>();


        //Commands for Loans disubursed
        services.AddScoped<IRequestHandler<GetLoanRelatedGuarantorsQuery, IEnumerable<VLoanDisbursementGuarantors>>, GetLoanRelatedGuarantorsQueryHandler>();
        services.AddScoped<IRequestHandler<GetLoanRelatedDocumentsQuery, IEnumerable<VLoanDisbursementDocuments>>, GetLoanRelatedDocumentsQueryHandler>();
        services.AddScoped<IRequestHandler<GetLoanRelatedCollateralsQuery, IEnumerable<VLoanDisbursementCollateral>>, GetLoanRelatedCollateralsQueryHandler>();
        services.AddScoped<IRequestHandler<GetLoanCollateralsQuery, Pager<VLoanCollateral>>, GetLoanCollateralsQueryHandler>();





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


        //User and Identity
        services.AddScoped<IRequestHandler<CreateNewUserCommand, ErrorOr<string>>, CreateNewUserCommandHandler>();

        // Settings — AppSettings
        services.AddScoped<IRequestHandler<GetAppSettingsQuery, ErrorOr<IEnumerable<AppSettingDto>>>, GetAppSettingsQueryHandler>();
        services.AddScoped<IRequestHandler<GetAppSettingByIdQuery, ErrorOr<AppSettingDto>>, GetAppSettingByIdQueryHandler>();
        services.AddScoped<IRequestHandler<CreateAppSettingCommand, ErrorOr<AppSettingDto>>, CreateAppSettingCommandHandler>();
        services.AddScoped<IRequestHandler<UpdateAppSettingCommand, ErrorOr<AppSettingDto>>, UpdateAppSettingCommandHandler>();
        services.AddScoped<IRequestHandler<DeleteAppSettingCommand, ErrorOr<bool>>, DeleteAppSettingCommandHandler>();

        services.AddScoped<IRequestHandler<GetLoanCategoriesQuery, Pager<LoanCategoryDto>>, GetLoanCategoriesQueryHandler>();



        // Settings — UserLoanSizes
        services.AddScoped<IRequestHandler<GetUserLoanSizesQuery, Pager<VUserLoanSizes>>, GetUserLoanSizesQueryHandler>();
        services.AddScoped<IRequestHandler<GetUserLoanSizeByIdQuery, ErrorOr<UserLoanSizeDto>>, GetUserLoanSizeByIdQueryHandler>();
        services.AddScoped<IRequestHandler<CreateUserLoanSizeCommand, ErrorOr<UserLoanSizeDto>>, CreateUserLoanSizeCommandHandler>();
        services.AddScoped<IRequestHandler<UpdateUserLoanSizeCommand, ErrorOr<UserLoanSizeDto>>, UpdateUserLoanSizeCommandHandler>();
        services.AddScoped<IRequestHandler<DeleteUserLoanSizeCommand, ErrorOr<bool>>, DeleteUserLoanSizeCommandHandler>();
        services.AddScoped<IRequestHandler<ToggleUserLoanSizeCommand, ErrorOr<bool>>, ToggleUserLoanSizeCommandHandler>();
        services.AddScoped<IRequestHandler<GetApprovedUserQuery, ErrorOr<IEnumerable<VUserLoanSizes>>>, GetApprovedUserQueryHandler>();

        // Settings — CollateralSubTypes
        services.AddScoped<IRequestHandler<GetCollateralSubTypesQuery, Pager<CollateralSubTypeDto>>, GetCollateralSubTypesQueryHandler>();

        // Ledger Commands
        services.AddScoped<IRequestHandler<CreateOrUpdateLedgerCommand, ErrorOr<string>>, CreateOrUpdateLedgerCommandHandler>();
        services.AddScoped<IRequestHandler<DeleteLedgerCommand, ErrorOr<string>>, DeleteLedgerCommandHandler>();
        services.AddScoped<IRequestHandler<ChangeLedgerStatusCommand, ErrorOr<string>>, ChangeLedgerStatusCommandHandler>();

        // Ledger Queries
        services.AddScoped<IRequestHandler<GetLedgerByIdQuery, ErrorOr<Ledger>>, GetLedgerByIdQueryHandler>();
        services.AddScoped<IRequestHandler<GetLedgersQuery, ErrorOr<IEnumerable<VLedger>>>, GetLedgersQueryHandler>();
        services.AddScoped<IRequestHandler<GetLedgersPaginatedQuery, Pager<VLedger>>, GetLedgersPaginatedQueryHandler>();
        services.AddScoped<IRequestHandler<GetLedgerTypesQuery, ErrorOr<IEnumerable<LedgerType>>>, GetLedgerTypesQueryHandler>();
        services.AddScoped<IRequestHandler<GetLedgerTypeByIdQuery, ErrorOr<LedgerType>>, GetLedgerTypeByIdQueryHandler>();

        // OtherName Commands
        services.AddScoped<IRequestHandler<CreateOtherNameCommand, ErrorOr<VOtherNames>>, CreateOtherNameCommandHandler>();
        services.AddScoped<IRequestHandler<CreateOtherNameGuarantorCommand, ErrorOr<VGuarantor>>, CreateOtherNameGuarantorCommandHandler>();
        services.AddScoped<IRequestHandler<UpdateOtherNameCommand, ErrorOr<VOtherNames>>, UpdateOtherNameCommandHandler>();
        services.AddScoped<IRequestHandler<DeleteOtherNameCommand, ErrorOr<bool>>, DeleteOtherNameCommandHandler>();
        services.AddScoped<IRequestHandler<UpdateOtherNameStatusCommand, ErrorOr<VOtherNames>>, UpdateOtherNameStatusCommandHandler>();
        services.AddScoped<IRequestHandler<ConvertToCustomerCommand, ErrorOr<bool>>, ConvertToCustomerCommandHandler>();
        services.AddScoped<IRequestHandler<ConvertToVendorCommand, ErrorOr<bool>>, ConvertToVendorCommandHandler>();
        services.AddScoped<IRequestHandler<ConvertToEmployeeCommand, ErrorOr<bool>>, ConvertToEmployeeCommandHandler>();

        // OtherName Queries
        services.AddScoped<IRequestHandler<GetOtherNameByIdQuery, ErrorOr<EntityNames>>, GetOtherNameByIdQueryHandler>();
        services.AddScoped<IRequestHandler<GetOtherNamesAsPagingQuery, Pager<VOtherNames>>, GetOtherNamesAsPagingQueryHandler>();

        return services;

    }
}
