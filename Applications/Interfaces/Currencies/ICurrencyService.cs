using Applications.Features.Currencies.Dto;
using Applications.Features.Currencies.Responses;
using Applications.Responses;
using Applications.Responses.Paginations;
using Domains.Models.Currencies;
using ErrorOr;

namespace Applications.Interfaces.Currencies
{
    public interface ICurrencyService
    {
        Task<ErrorOr<VCurrenciesList>> SaveOrUpdate(Currency currency, CancellationToken cancellation = default);
        Task<ErrorOr<VCurrencyCounter>> AddNote(CurrencyCounter model, CancellationToken cancellation = default);
        Task<ErrorOr<VCurrencyCounter>> AddNote(int? id, CurrencyCounter model, CancellationToken cancellation = default);
        Task<ErrorOr<string>> ActivateOrDeactivateNote(int? noteId, NoteActivateDto model, CancellationToken cancellation = default);
        Task<ErrorOr<Currency>> GetCurrencyWithSpotRates(int? id, CancellationToken cancellation = default);
        Task<ErrorOr<Currency>> GetCurrencyById(int? id, CancellationToken cancellation = default);
        Task<Pager<Currency>> GetCurrencies(int? id, QueryParam param, CancellationToken cancellation = default);
       
        Task<Pager<VCurrenciesList>> GetCurrencies(QueryParam param, CancellationToken cancellation = default);
        Task<IEnumerable<VCurrenciesList>> GetCurrencies(string? q);
        Task<ErrorOr<VCurrenciesList>> GetCurrencyDetailById(int? id, CancellationToken cancellation = default);

        Task<ErrorOr<IEnumerable<VCurrencyCounter>>> GetCurrencyNotes(int? branchId, int? currencyId, CancellationToken cancellation = default);
        Task<Pager<VCurrencyCounter>> GetCurrencyCounter(int? currencyId, QueryParam param, CancellationToken cancellation = default);

        Task<Pager<CurrencyNoteViewDto>> GetCurrencyNotes(int? branchId, int? currencyId, QueryParam param, CancellationToken cancellation = default);
        Task<Pager<VCurrencyNotes>> GetCurrenciesNotes(int? branchId, int? currencyId, QueryParam param, CancellationToken ca = default);
    } 
}

//Task<ErrorOr<IEnumerable<Currency>>> GetCurrencies(int? currencyId, string? q = null);

//Task<IEnumerable<VCurrencyCounter>> GetCurrecyNotesss(int? branchRefId);

//Task<Result<IEnumerable<T>>> GetCurrencies<T>(int? currencyId, string? q = null) where T : class, new();
