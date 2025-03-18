using Applications.Interfaces.Currencies;
using Applications.Requests.Lists.Currency;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Currencies;
using Domains.Models.Currencies;
using System.Diagnostics.CodeAnalysis;

namespace Applications.Services.Currencies
{
    public sealed class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyService _cService;

        public CurrencyService(ICurrencyService currencyService)
        {
            _cService = currencyService;
        }

        public async Task<Result> ActivateOrDeactivateNote(int? noteId, NoteActivateDto model)
        {
            return await _cService.ActivateOrDeactivateNote(noteId, model);
        }

        public async Task<Result> AddNote(CurrencyCounter model)
        {
            return await _cService.AddNote(model);
        }

        public async Task<Result> AddNote(int? id, CurrencyCounter model)
        {
            return await _cService.AddNote(id, model);
        }

        public async Task<Result<IEnumerable<VCurrencyNotes>>> GetCurrencyNotes(int? branchId)
        {
            return await _cService.GetCurrencyNotes(branchId);
        }

        public async Task<Result<IEnumerable<VCurrencyNotes>>> GetCurrencyNotes(int? currencyId, int? branchId)
        {
            return await _cService.GetCurrencyNotes(branchId, currencyId);
        }

        public async Task<IEnumerable<VCurrencyNotes>> GetCurrecyNotesss(int? branchRefId)
        {
            return await _cService.GetCurrecyNotesss(branchRefId);
        }

        public async Task<Pager<VCurrenciesList>> GetCurrencies(QueryParam param)
        {
            return await _cService.GetCurrencies(param);
        }

        public async Task<IEnumerable<VCurrenciesList>> GetCurrencies(string? q)
        {
            return await _cService.GetCurrencies(q);
        }

        public async Task<Result<IEnumerable<Currency>>> GetCurrencies(int? currencyId, string? q = null)
        {
            return await _cService.GetCurrencies(currencyId, q);
        }

        public async Task<Result<VCurrenciesList>> GetCurrencyAsync(int? currencyId)
        {
            return await _cService.GetCurrencyAsync(currencyId);

        }

        public async Task<Result<Currency>> GetCurrencyById(int? id)
        {
            return await _cService.GetCurrencyById(id);
        }

        public async Task<Result<Pager<VCurrencyNotes>>> GetCurrencyNotes(QueryParam param)
        {
            return await _cService.GetCurrencyNotes(param);
        }

        public async Task<Result<Pager<VCurrencyNotes>>> GetCurrencyNotes(int? currencyId, QueryParam param)
        {
            return await _cService.GetCurrencyNotes(currencyId, param);
        }

        public async Task<Currency> GetCurrencyWithSpotRates(int? id)
        {
            return await GetCurrencyWithSpotRates(id);
        }

        public async Task<Result> SaveOrUpdate(Currency currency)
        {
            return await _cService.SaveOrUpdate(currency);
        }


    }

    //public interface ICurrencyService
    //{
    //    Task<Result> SaveOrUpdate(Currency currency);
    //    Task<Result> AddNote(CurrencyCounter model);
    //    Task<Result> AddNote(int id, CurrencyCounter model);
    //    Task<Result> ActivateOrDeactivateNote(int noteId, NoteActivateDto model);
    //    Task<Currency> GetCurrencyWithSpotRates(int id);
    //    Task<Currency> GetCurrencyById(int id);
    //    Task<Pager<VCurrenciesList>> GetCurrencies(QueryParam param);
    //    Task<Result<Pager<VCurrencyNotes>>> GetCurrencyNotes(QueryParam param);
    //    Task<Result<Pager<VCurrencyNotes>>> GetCurrencyNotes(int currencyId, QueryParam param);
    //    Task<Result<IEnumerable<VCurrencyNotes>>> GetCurrecyNotes(int branchId); 
    //    Task<IEnumerable<VCurrenciesList>> GetCurrencies(string? q);
    //    Task<Result<IEnumerable<Currency>>> GetCurrencies(int currencyId, string? q=null);
    //    Task<Result<IEnumerable<VCurrencyNotes>>> GetCurrecyNotes(int currencyId, int branchId);
    //    Task<IEnumerable<VCurrencyNotes>> GetCurrecyNotesss(int branchRefId);
    //    Task<Result<VCurrenciesList>> GetCurrencyAsync(int currencyId);
    //}

}
