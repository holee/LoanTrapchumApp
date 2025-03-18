using Applications.Interfaces.Currencies;
using Applications.Interfaces.Transactions;
using Applications.Requests.Journals;
using Applications.Requests.Lists.Currency;
using Applications.Responses.ViewModels.Ledgers;
using Applications.Services.Transactions.Journals;
using Domains.Models.Journals;
using Mapster;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;

namespace TrapchumApp.Components.Pages.Transactions.Ledgers
{
    public partial class CreateLedgerModal
    {
        private readonly ILedgerService _ledgerService;
        private readonly ICurrencyService _currencyService;
        public CreateLedgerModal(ILedgerService ledgerService, ICurrencyService currencyService)
        {
            _ledgerService = ledgerService;
            _currencyService = currencyService;
        }

        EditForm editForm = default!;
        IEnumerable<LedgerTypeDto>? LedgerTypes = [];
        IEnumerable<CurrencyDto>? Currencies = [];
        IEnumerable<VLedger>? FilterLedgers = [];
        bool IsSubTypeOf { get; set; }
        LedgerCreateDto model = new();
        //LedgerList ledgerList = new LedgerList();
        LedgerTypeDto? SelectedLedgerType;
        VLedger? SelectedLedger;
        bool isValid = true;
        int selectedCurrencyId = 1;
        int counts = 0;
        string ledgerSearchText = "";
        string ledgerTypeSearchText = "";
        void SelectCheckedChanged(bool isMain)
        {
            if (isMain)
            {
                model.IsSubTypeOf = false;
                model.ParentRefId = null;
            }
        }
        async void SubTypeChanged(bool isSubTypeOf)
        {
            if (!isSubTypeOf)
            {
                model.ParentRefId = null;
                return;
            }
            await GetLedgers(model.CurrencyRefId, ledgerSearchText);
        }
        //async Task LoadDataLedgerTypeChanged(LoadDataArgs e)
        //{
        //    //ledgerTypeSearchText = e.Filter ?? string.Empty;
        //    //var resp = await _ledgerService.FindLedgerTypes(0, ledgerTypeSearchText);
        //    //if (resp.IsSuccess)
        //    //{
        //    //    LedgerTypes = resp.Data!.Adapt<IEnumerable<LedgerTypeDto>>();
        //    //    counts = resp.Data!.Count();
        //    //}
        //    //else
        //    //{
        //    //    LedgerTypes = [];
        //    //    counts = 0;
        //    //    NotificationService.Notify(Radzen.NotificationSeverity.Error, resp.ErrMessage);
        //    //}
        //}
       
        async Task LoadDataTextChanged(LoadDataArgs e)
        {
            ledgerSearchText = e.Filter ?? string.Empty;
            await GetLedgers(selectedCurrencyId, ledgerSearchText);
        }
        async void SelectedCurrencyChanged(object o)
        {
            if (o is not null)
            {
                selectedCurrencyId = ((CurrencyDto)o).CurrencyId;
            }
            else
            {
                FilterLedgers = [];
            }

            if (IsSubTypeOf)
            {
                await GetLedgers(selectedCurrencyId, ledgerSearchText);
            }
        }
        async Task SelectedValueChanged()
        {
            if (!model.IsMainGl)
            {
                await GetLedgers(selectedCurrencyId);
            }

        }
        async Task FormValidSubmit()
        {
            if (editForm.EditContext?.Validate() == false) return;

            if (model.IsSubTypeOf && model.ParentRefId <= 0)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Please select ledger name.");
                return;
            }

            if (model.LedgerTypeRefId == 1 && string.IsNullOrEmpty(model.BankNumber))
            {
                NotificationService.Notify(NotificationSeverity.Error, "please input bank acoount number.");
                return;
            }

            var req = model.Adapt<Ledger>();

            var response = await _ledgerService.CreateOrUpdateLedger(req);

            if (response.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Success, response.Data);
                model = new();
                Close(true);
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, response.ErrMessage);
            }
        }
        void Close(bool IsSuccess = false) => DialogService.Close(IsSuccess);

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var resp = await _ledgerService.FindLedgerTypes();

                if (resp.IsSuccess)
                {
                    LedgerTypes = resp.Data!.Adapt<IEnumerable<LedgerTypeDto>>();
                    counts = resp.Data!.Count();
                }
                else
                {
                    counts = 0;
                    NotificationService.Notify(Radzen.NotificationSeverity.Error, resp.ErrMessage);
                }

                var currenciesRsponse = await _currencyService.GetCurrencies(0);

                model.CurrencyRefId = 1;

                if (currenciesRsponse.IsSuccess)
                {
                    Currencies = currenciesRsponse.Data!.Adapt<IEnumerable<CurrencyDto>>();
                }
                await GetLedgers(selectedCurrencyId);
            }
            catch (Exception ex)
            {
                NotificationService.Notify(Radzen.NotificationSeverity.Success, $"{ex.Message}");
            }

            await base.OnInitializedAsync();
        }

        async Task GetLedgers(int? currencyId = 1, string? searchText = null)
        {
            var resp = await _ledgerService.GetLedgersAsync(0, currencyId, searchText);

            if (resp.IsSuccess)
            {
                FilterLedgers = resp.Data!;
            }
            else
            {
                FilterLedgers = [];
                NotificationService.Notify(Radzen.NotificationSeverity.Error, resp.ErrMessage);
            }
        }
        void SelectedChangedLedgerType(object p)
        {
            if (p is not null)
            {

            }
        }

    }
}