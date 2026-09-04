using Applications.Features.AdditionalInformations.Dto;
using Applications.Features.Collaterals.Dtos;
using Applications.Features.Incomes.Dto;
using Applications.Features.LoanPurposes;
using Applications.Features.Periods.Dto;
using Applications.Features.Settings.Dto;
using Applications.Responses.Audits;
using Applications.Responses.Paginations;
using Domains.Enumerations;
using Domains.Shared.Enums;
using Refit;

namespace Web.UI.Services;

/// <summary>
/// Typed HTTP client for the /api/settings endpoints exposed by the server app.
/// </summary>
public interface ISettingApi
{
    // ----------------------------------------------------------------- app settings

    [Get("/api/settings")]
    Task<List<AppSettingDto>> GetAppSettingsAsync(
        [Query] string? filter = null,
        CancellationToken cancellationToken = default);

    [Post("/api/settings")]
    Task<AppSettingDto> CreateAppSettingAsync(
        [Body] AppSettingDtoCreate setting,
        CancellationToken cancellationToken = default);

    [Put("/api/settings/{id}")]
    Task<AppSettingDto> UpdateAppSettingAsync(
        int id,
        [Body] AppSettingDtoUpdate setting,
        CancellationToken cancellationToken = default);

    [Delete("/api/settings/{id}")]
    Task<SettingMessageResponse> DeleteAppSettingAsync(
        int id,
        CancellationToken cancellationToken = default);

    // ---------------------------------------------------------- collateral settings

    [Get("/api/settings/collateral-types")]
    Task<List<CollateralTypeDto>> GetCollateralTypesAsync(
        [Query] int? id = null,
        [Query] string? q = null,
        CancellationToken cancellationToken = default);

    [Get("/api/settings/collateral-subtypes")]
    Task<Pager<CollateralSubTypeDto>> GetCollateralSubTypesAsync(
        [Query] int page = 0,
        [Query] int take = 15,
        [Query] string? sortColumn = "Id",
        [Query] bool sortDescending = false,
        [Query] bool includeInactive = true,
        [Query] string? filter = null,
        CancellationToken cancellationToken = default);

    [Post("/api/settings/collateral-subtypes")]
    Task<SettingMessageResponse> UpsertCollateralSubTypeAsync(
        [Body] CollateralSubTypeDtoCreate subType,
        CancellationToken cancellationToken = default);

    [Put("/api/settings/collateral-subtypes/{id}")]
    Task<SettingMessageResponse> UpdateCollateralSubTypeAsync(
        int id,
        [Body] CollateralSubTypeDto subType,
        CancellationToken cancellationToken = default);

    // --------------------------------------------------------------------- periods

    [Get("/api/settings/periods")]
    Task<Pager<PeriodDto>> GetPeriodsAsync(
        [Query] int page = 0,
        [Query] int take = 15,
        [Query] string? sortColumn = "Id",
        [Query] bool sortDescending = false,
        [Query] string? filter = null,
        CancellationToken cancellationToken = default);

    [Post("/api/settings/periods")]
    Task<PeriodDto> CreatePeriodAsync(
        [Body] PeriodDtoCreate period,
        CancellationToken cancellationToken = default);

    [Put("/api/settings/periods/{id}")]
    Task<PeriodDto> UpdatePeriodAsync(
        int id,
        [Body] PeriodDtoUpdate period,
        CancellationToken cancellationToken = default);

    [Delete("/api/settings/periods/{id}")]
    Task<SettingMessageResponse> DeletePeriodAsync(
        int id,
        CancellationToken cancellationToken = default);

    // --------------------------------------------------------------- loan purposes

    [Get("/api/settings/loan-purposes")]
    Task<Pager<LoanPurposeDto>> GetLoanPurposesAsync(
        [Query] int page = 0,
        [Query] int take = 15,
        [Query] string? sortColumn = "Id",
        [Query] bool sortDescending = false,
        [Query] string? filter = null,
        CancellationToken cancellationToken = default);

    [Get("/api/settings/loan-purposes/{id}")]
    Task<LoanPurposeDto> GetLoanPurposeByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    [Post("/api/settings/loan-purposes")]
    Task<LoanPurposeDto> CreateLoanPurposeAsync(
        [Body] LoanPurposeDtoCreate loanPurpose,
        CancellationToken cancellationToken = default);

    [Put("/api/settings/loan-purposes/{id}")]
    Task<LoanPurposeDto> UpdateLoanPurposeAsync(
        int id,
        [Body] LoanPurposeDtoUpdate loanPurpose,
        CancellationToken cancellationToken = default);

    [Delete("/api/settings/loan-purposes/{id}")]
    Task<SettingMessageResponse> DeleteLoanPurposeAsync(
        int id,
        CancellationToken cancellationToken = default);

    // ------------------------------------------------------------------ occupations

    [Get("/api/settings/occupations")]
    Task<Pager<OccupationDto>> GetOccupationsAsync(
        [Query] int? id = 0,
        [Query] int page = 0,
        [Query] int take = 15,
        [Query] string? sortColumn = "OccupationId",
        [Query] bool sortDescending = false,
        [Query] bool includeInactive = false,
        [Query] string? filter = null,
        [Query] TypeOfEnum type = TypeOfEnum.All,
        CancellationToken cancellationToken = default);

    [Get("/api/settings/occupations/{id}")]
    Task<OccupationDto> GetOccupationByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    [Post("/api/settings/occupations")]
    Task<OccupationDto> CreateOccupationAsync(
        [Body] OccupationDtoCreate occupation,
        CancellationToken cancellationToken = default);

    [Put("/api/settings/occupations/{id}")]
    Task<OccupationDto> UpdateOccupationAsync(
        int id,
        [Body] OccupationDtoUpate occupation,
        CancellationToken cancellationToken = default);

    // ------------------------------------------------------------- income / expenses

    [Get("/api/settings/income-expenses")]
    Task<Pager<IncomeExpenseDto>> GetIncomeExpensesAsync(
        [Query] int? id = null,
        [Query] int page = 0,
        [Query] int take = 15,
        [Query] string? sortColumn = "Types",
        [Query] bool sortDescending = false,
        [Query] string? filter = null,
        [Query] IncomeExpenseTypes type = IncomeExpenseTypes.All,
        CancellationToken cancellationToken = default);

    [Get("/api/settings/income-expenses/{id}")]
    Task<IncomeExpenseDto> GetIncomeExpenseByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    [Post("/api/settings/income-expenses")]
    Task<SettingMessageResponse> CreateIncomeExpenseAsync(
        [Body] CreateIncomeExpenseDto incomeExpense,
        CancellationToken cancellationToken = default);

    [Put("/api/settings/income-expenses/{id}")]
    Task<SettingMessageResponse> UpdateIncomeExpenseAsync(
        int id,
        [Body] CreateIncomeExpenseDto incomeExpense,
        CancellationToken cancellationToken = default);

    [Delete("/api/settings/income-expenses/{id}")]
    Task<SettingMessageResponse> DeleteIncomeExpenseAsync(
        int id,
        CancellationToken cancellationToken = default);

    // ------------------------------------------------------------------ audit trails

    [Get("/api/settings/audits")]
    Task<Pager<AuditVm>> GetAuditTrailsAsync(
        [Query] string? userId = null,
        [Query] DateOnly? from = null,
        [Query] DateOnly? to = null,
        [Query] int page = 0,
        [Query] int take = 15,
        CancellationToken cancellationToken = default);
}
