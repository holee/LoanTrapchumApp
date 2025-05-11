using Applications.Requests.Lists.Currency;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Currencies;
using Domains.Models.Currencies;

namespace Applications.Interfaces.Currencies
{
    public interface ICurrencyService 
    {
        Task<Result> SaveOrUpdate(Currency currency);
        Task<Result> AddNote(CurrencyCounter model);
        Task<Result> AddNote(int? id, CurrencyCounter model);
        Task<Result> ActivateOrDeactivateNote(int? noteId, NoteActivateDto model);
        Task<Currency> GetCurrencyWithSpotRates(int? id);
        Task<Result<Currency>> GetCurrencyById(int? id);
        Task<Pager<VCurrenciesList>> GetCurrencies(QueryParam param);
        Task<Result<Pager<VCurrencyNotes>>> GetCurrencyNotes(QueryParam param);
        Task<Result<Pager<VCurrencyNotes>>> GetCurrencyNotes(int? currencyId, QueryParam param);
        Task<Result<IEnumerable<VCurrencyNotes>>> GetCurrencyNotes(int? branchId);
        Task<Result<IEnumerable<VCurrencyNotes>>> GetCurrencyNotes(int? currencyId, int? branchId);
        Task<IEnumerable<VCurrenciesList>> GetCurrencies(string? q);
        Task<Result<IEnumerable<Currency>>> GetCurrencies(int? currencyId, string? q=null);
       
        Task<IEnumerable<VCurrencyNotes>> GetCurrecyNotesss(int? branchRefId);
        Task<Result<VCurrenciesList>> GetCurrencyAsync(int? currencyId);

        Task<Result<IEnumerable<T>>> GetCurrencies<T>(int? currencyId, string? q = null)
            where T : class, new();
    }
}
