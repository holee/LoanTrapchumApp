using Applications.Features.Currencies.Dto;
using Applications.Features.Currencies.Queries.GetCurrencies;
using Applications.Features.Ledgers.Commands.CreateLedger;
using Applications.Features.Ledgers.Dto;
using Applications.Features.Ledgers.Queries.GetLedgers;
using Applications.Features.Ledgers.Queries.GetLedgerTypes;
using Domains.Enumerations;
using Mapster;

namespace Web.UI.Pages.Transactions.Ledgers
{
    public partial class CreateLedgerModal
    {

        EditForm editForm = default!;
        IEnumerable<LedgerTypeDto>? LedgerTypes = [];
        IEnumerable<CurrencyDto>? Currencies = [];
        IEnumerable<VLedger>? FilterLedgers = [];
        bool IsSubTypeOf { get; set; }
        LedgerCreateDto model = new();
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
                _toastService.ShowError("Please select ledger name.", "Error");

                return;
            }

            if (model.LedgerTypeRefId == (int)AccountTypes.CashAndBank && string.IsNullOrEmpty(model.BankNumber))
            {
                _toastService.ShowError("Please input bank acoount number.", "Error");

                return;
            }

            var command = new CreateLedgerCommand(model);

            var response = await mediator.Send(command); 

            if (response.IsSuccess())
            {
                
                _toastService.ShowSuccess("Ledger created successfully.", "Success");

                model = new();

                Close(true);
            }
            else
            {
                _toastService.ShowError(response.GetMessage(), "Error");
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
            var command = new GetLedgerTypesQuery(null, new QueryParam(0, 0));

            var resp = await mediator.Send(command);

            LedgerTypes = resp.Items.Adapt<IEnumerable<LedgerTypeDto>>();

            counts = resp.TotalCount;

        }

        async Task GetLedgers(int? currencyId = 1, string? searchText = null)
        {

            var param = new QueryParam(0, 0, string.Empty, false, false, searchText);

            var command = new GetLedgersPaginatedQuery(currencyId, param);

            var resp = await mediator.Send(command);

            FilterLedgers = resp.Items;

            StateHasChanged();

        }
        void SelectedChangedLedgerType(object p)
        {
            if (p is not null)
            {

            }
        }

    }
}