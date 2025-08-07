using Applications.Interfaces.Currencies;
using Applications.Requests.Lists.Currency;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Currencies;
using Domains.Models.Currencies;
using System.Diagnostics.CodeAnalysis;

namespace Applications.Features.Currencies
{
    public interface ICurrencyServiceImpl
    {
        Task<Result> ActivateOrDeactivateNote(int? noteId, NoteActivateDto model);
        Task<Result> AddNote(CurrencyCounter model);
        Task<Result> AddNote(int? id, CurrencyCounter model);
        Task<IEnumerable<VCurrencyNotes>> GetCurrecyNotesss(int? branchRefId);
        Task<Result<IEnumerable<Currency>>> GetCurrencies(int? currencyId, string? q = null);
        Task<Pager<VCurrenciesList>> GetCurrencies(QueryParam param);
        Task<IEnumerable<VCurrenciesList>> GetCurrencies(string? q);
        Task<Result<VCurrenciesList>> GetCurrencyAsync(int? currencyId);
        Task<Result<Currency>> GetCurrencyById(int? id);
        Task<Result<IEnumerable<VCurrencyNotes>>> GetCurrencyNotes(int? branchId);
        Task<Result<IEnumerable<VCurrencyNotes>>> GetCurrencyNotes(int? currencyId, int? branchId);
        Task<Result<Pager<VCurrencyNotes>>> GetCurrencyNotes(int? currencyId, QueryParam param);
        Task<Result<Pager<VCurrencyNotes>>> GetCurrencyNotes(QueryParam param);
        Task<Currency> GetCurrencyWithSpotRates(int? id);
        Task<Result> SaveOrUpdate(Currency currency);

        Task<Result<IEnumerable<T>>> GetCurrencies<T>(int? currencyId, string? q = null)
            where T : class, new();
    }

    public sealed class CurrencyService(ICurrencyService currencyService) : ICurrencyServiceImpl
    {
        public async Task<Result> ActivateOrDeactivateNote(int? noteId, NoteActivateDto model)
        {
            return await currencyService.ActivateOrDeactivateNote(noteId, model);
        }

        public async Task<Result> AddNote(CurrencyCounter model)
        {
            return await currencyService.AddNote(model);
        }

        public async Task<Result> AddNote(int? id, CurrencyCounter model)
        {
            return await currencyService.AddNote(id, model);
        }

        public async Task<Result<IEnumerable<VCurrencyNotes>>> GetCurrencyNotes(int? branchId)
        {
            return await currencyService.GetCurrencyNotes(branchId);
        }

        public async Task<Result<IEnumerable<VCurrencyNotes>>> GetCurrencyNotes(int? currencyId, int? branchId)
        {
            return await currencyService.GetCurrencyNotes(currencyId, branchId);
        }

        public async Task<IEnumerable<VCurrencyNotes>> GetCurrecyNotesss(int? branchRefId)
        {
            return await currencyService.GetCurrecyNotesss(branchRefId);
        }

        public async Task<Pager<VCurrenciesList>> GetCurrencies(QueryParam param)
        {
            return await currencyService.GetCurrencies(param);
        }

        public async Task<IEnumerable<VCurrenciesList>> GetCurrencies(string? q)
        {
            return await currencyService.GetCurrencies(q);
        }

        public async Task<Result<IEnumerable<Currency>>> GetCurrencies(int? currencyId, string? q = null)
        {
            return await currencyService.GetCurrencies(currencyId, q);
        }

        public async Task<Result<IEnumerable<T>>> GetCurrencies<T>(int? currencyId, string? q = null)
            where T : class, new()
        {
            return await currencyService.GetCurrencies<T>(currencyId, q);
        }

        public async Task<Result<VCurrenciesList>> GetCurrencyAsync(int? currencyId)
        {
            return await currencyService.GetCurrencyAsync(currencyId);
        }

        public async Task<Result<Currency>> GetCurrencyById(int? id)
        {
            return await currencyService.GetCurrencyById(id);
        }

        public async Task<Result<Pager<VCurrencyNotes>>> GetCurrencyNotes(QueryParam param)
        {
            return await currencyService.GetCurrencyNotes(param);
        }

        public async Task<Result<Pager<VCurrencyNotes>>> GetCurrencyNotes(int? currencyId, QueryParam param)
        {
            return await currencyService.GetCurrencyNotes(currencyId, param);
        }

        public async Task<Currency> GetCurrencyWithSpotRates(int? id)
        {
            return await GetCurrencyWithSpotRates(id);
        }

        public async Task<Result> SaveOrUpdate(Currency currency)
        {
            return await currencyService.SaveOrUpdate(currency);
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