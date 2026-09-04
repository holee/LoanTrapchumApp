using Applications.Commons;
using Applications.Features.Currencies;
using Applications.Features.Currencies.Dto;
using Applications.Features.Currencies.Responses;
using Applications.Responses.Paginations;
using Refit;

namespace Web.UI.Services;

/// <summary>
/// Typed HTTP client for the /api/currencies endpoints exposed by the server app.
/// </summary>
public interface ICurrencyApi
{
    [Get("/api/currencies")]
    Task<Pager<VCurrenciesList>> GetCurrenciesAsync(
        [Query] int page,
        [Query] int take,
        [Query] string? sortColumn = "CurrencyCode",
        [Query] bool sortDescending = false,
        [Query] bool includeInactive = false,
        [Query] string? filter = null,
        CancellationToken cancellationToken = default);

    [Get("/api/currencies/list")]
    Task<PageMetaData<CurrencyDto>> GetCurrencyListAsync(
        [Query] int? currencyId = 0,
        [Query] int page = 0,
        [Query] int take = 0,
        CancellationToken cancellationToken = default);

    [Get("/api/currencies/{id}")]
    Task<CurrencyDto> GetCurrencyByIdAsync(int id, CancellationToken cancellationToken = default);

    [Post("/api/currencies")]
    Task<VCurrenciesList> CreateCurrencyAsync(
        [Body] CurrencyDtoCreate currency,
        CancellationToken cancellationToken = default);

    [Get("/api/currencies/{id}/spot-rates")]
    Task<PageMetaData<CurrencyDtoSpotRate>> GetSpotRatesAsync(
        int id,
        [Query] int page,
        [Query] int take,
        CancellationToken cancellationToken = default);

    [Post("/api/currencies/{id}/spot-rate")]
    Task<SpotRateDtoCreate> SetSpotRateAsync(
        int id,
        [Body] CurrencySpotRateDtoCreate spotRate,
        CancellationToken cancellationToken = default);

    [Get("/api/currencies/notes")]
    Task<Pager<VCurrencyCounter>> GetNotesAsync(
        [Query] int? currencyId = 0,
        [Query] int? branchId = 0,
        CancellationToken cancellationToken = default);

    [Post("/api/currencies/notes")]
    Task<VCurrencyCounter> CreateNoteAsync(
        [Body] CurrencyNoteDtoCreate note,
        CancellationToken cancellationToken = default);

    [Patch("/api/currencies/notes/{id}/toggle")]
    Task<MessageResponse> ToggleNoteAsync(
        int id,
        [Body] NoteToggleRequest request,
        CancellationToken cancellationToken = default);
}
