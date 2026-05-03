using Applications.Features.Currencies.Dto;
using Applications.Features.Currencies.Queries.GetCurrencies;
using Applications.Features.Ledgers.Commands.CreateOrUpdateLedger;
using Applications.Features.Ledgers.Dto;
using Applications.Features.Ledgers.Queries.GetLedgers;
using Applications.Features.Ledgers.Queries.GetLedgerTypes;
using Applications.Responses.ViewModels.Ledgers;
using Domains.Enumerations;
using Domains.Models.Journals;
using Mapster;

namespace TrapchumApp.Pages.Transactions.Ledgers
{
    public partial class CreateLedgerModal
    {

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

            var command = new CreateOrUpdateLedgerCommand(req);

            var response = await mediator.Send(command); //_ledgerService.CreateOrUpdateLedger(req);

            if (response.IsSuccess())
            {
                NotificationService.Notify(NotificationSeverity.Success, response.Value);

                model = new();

                Close(true);
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, response.GetMessage());
            }
        }
        void Close(bool IsSuccess = false) => DialogService.Close(IsSuccess);

        protected override async Task OnInitializedAsync()
        {
            await GetLedgerTypes();

            await GetCurrencies();

            await GetLedgers(selectedCurrencyId);

            await base.OnInitializedAsync();
        }

        private async Task GetCurrencies()
        {
            var command = new GetCurrenciesQuery(0, new QueryParam(0, 0));

            var rsp = await mediator.Send(command); //_currencyService.GetCurrencies(0);

            model.CurrencyRefId = (int)CurrencyEnum.KHR;

            Currencies = rsp.Items.Adapt<IEnumerable<CurrencyDto>>();
        }

        private async Task GetLedgerTypes()
        {
            var command = new GetLedgerTypesQuery(null, string.Empty);

            var resp = await mediator.Send(command);  //_ledgerService.FindLedgerTypes();

            if (resp.IsSuccess())
            {
                LedgerTypes = resp.Value.Adapt<IEnumerable<LedgerTypeDto>>();

                counts = resp.Value.Count();
            }
            else
            {
                counts = 0;

                NotificationService.Notify(NotificationSeverity.Error, resp.GetMessage());
            }

        }

        async Task GetLedgers(int? currencyId = 1, string? searchText = null)
        {

            var command = new GetLedgersQuery(null, currencyId, searchText);

            var resp = await mediator.Send(command);  //_ledgerService.GetLedgersAsync(0, currencyId, searchText);

            if (resp.IsSuccess())
            {
                FilterLedgers = resp.Value!;
            }
            else
            {
                FilterLedgers = [];

                NotificationService.Notify(NotificationSeverity.Error, resp.GetMessage());
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