using Applications.Extensions.mappers;
using Applications.Interfaces.Currencies;
using Applications.Interfaces.Transactions;
using Applications.Requests.Journals;
using Applications.Responses.Lists.Currency;
using Applications.Responses.ViewModels.Ledgers;
using Domains.Models.Journals;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;

namespace TrapchumApp.Pages.Transactions.Ledgers
{
    public partial class EditLedgerModal
    {
        private readonly ILedgerService _ledgService;
        private readonly ICurrencyService _currencyService;
        public EditLedgerModal(ILedgerService ledgerService, ICurrencyService currencyService)
        {
            _ledgService = ledgerService;
            _currencyService = currencyService;
        }

        [Parameter] public int Id { get; set; }
        IEnumerable<LedgerTypeDto>? LedgerTypes = [];
        IEnumerable<CurrencyResponse>? Currencies = [];
        IEnumerable<VLedger>? FilterLedgers = [];
        LedgerUpdateDto model = new LedgerUpdateDto();
        EditForm? editForm;
        private bool isValid = true;
        int? tempParentRefIdId = 0;
        int? tempCurrencyId = 0;
        string filterText = "";
        string filterTypeText = "";
        bool tempIsSubTypeOf = false;
        async Task LoadDataTextChanged(LoadDataArgs e)
        {
            filterText = e.Filter ?? string.Empty;
            await GetLedgers(model.CurrencyRefId, filterText);
        }

        async Task LoadDataLedgerTypeChanged(LoadDataArgs e)
        {
            filterTypeText = e.Filter ?? string.Empty;
            var resp = await _ledgService.FindLedgerTypes(0, filterTypeText);
            if (resp.IsSuccess)
            {
                LedgerTypes = resp.Data!.Adapt<IEnumerable<LedgerTypeDto>>();
            }
            else
            {
                LedgerTypes = [];
                NotificationService.Notify(NotificationSeverity.Error, resp.ErrMessage);
            }
        }


        protected async Task GetLedgers(int? currencyId = 1, string? searchText = null)
        {
            var resp = await _ledgService.GetLedgersAsync(0, currencyId, null);
            if (resp.IsSuccess)
            {
                FilterLedgers = resp.Data!;
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, resp.ErrMessage);
            }
        }

        void Close(bool IsSuccess = false) => DialogService.Close(IsSuccess);

        protected async Task FormValidSubmit()
        {
            if (editForm?.EditContext?.Validate() == false) return;

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

            var result = await _ledgService.CreateOrUpdateLedger(req);

            if (result.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Success, result.Data);
                model = new();
                Close(true);
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, result.ErrMessage);
            }
        }
        protected override async Task OnParametersSetAsync()
        {
            var resp = await _ledgService.FindLedgerTypes();

            if ((resp.IsSuccess))
            {
                LedgerTypes = resp.Data!.Maps<LedgerTypeDto>();
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, resp.ErrMessage);
            }

            var currenciesRsponse = await _currencyService.GetCurrencies(0);

            if (currenciesRsponse.IsSuccess)
            {
                Currencies = currenciesRsponse.Data?.Maps<CurrencyResponse>();
            }

            await base.OnParametersSetAsync();
        }
        protected override async Task OnInitializedAsync()
        {

            var res = await _ledgService.FindLedgerByIdAsync(Id);

            if (res.IsSuccess)
            {
                model = res.Data!.Adapt<LedgerUpdateDto>();

                if (model is not null && model!.ParentRefId > 0)
                {
                    model.IsSubTypeOf = true;
                    tempIsSubTypeOf = true;
                    tempParentRefIdId = model!.ParentRefId;
                }
                tempCurrencyId = model?.CurrencyRefId;
                await GetLedgers(model?.CurrencyRefId);
            }



            await base.OnInitializedAsync();
        }

        protected async Task CurrencySelectedChanged(object param)
        {
            var currencyId = ((CurrencyResponse)param).CurrencyId;

            if (!model.IsMainGl && currencyId == tempCurrencyId)
            {
                model.IsSubTypeOf = tempIsSubTypeOf;
                model.ParentRefId = tempParentRefIdId;
                await GetLedgers(currencyId, filterText);

            }

            if (!model.IsMainGl && currencyId != tempCurrencyId)
            {
                model.IsSubTypeOf = tempIsSubTypeOf;
                model.ParentRefId = null;
                await GetLedgers(currencyId, filterText);

            }

            Console.WriteLine(tempCurrencyId);


            // if (Model.IsSubTypeOf && Model.ParentRefId != 0)
            // {
            //     Model.ParentRefId = null;
            // }
            // if (tempCurrencyId != 0 && tempCurrencyId == currencyId)
            // {
            //     Model.ParentRefId = tempParentRefIdId;
            // }


            StateHasChanged();
        }

        protected void SelectedChangedLedgerType(object p)
        {
            //Model.LedgerTypeRefId = SelectedLedgerType!.LedgerTypeId;
        }
        private void SelectCheckedChanged(bool isMain)
        {
            if (isMain)
            {
                model.IsSubTypeOf = false;
                model.ParentRefId = null;
                return;
            }
            if (!isMain && Id > 0)
            {
                model.IsSubTypeOf = tempIsSubTypeOf;
                model.ParentRefId = tempParentRefIdId;
                StateHasChanged();
            }

        }

        async void SubTypeChanged(bool isSubTypeOf)
        {
            if (!isSubTypeOf)
            {
                model.ParentRefId = null;
                return;
            }
            else
            {
                model.ParentRefId = tempParentRefIdId;
                // StateHasChanged();
            }
            await GetLedgers(model.CurrencyRefId, filterText);
        }

    }
}