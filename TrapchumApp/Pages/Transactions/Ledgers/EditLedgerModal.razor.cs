using Applications.Extensions.mappers;
using Applications.Features.Currencies.Dto;
using Applications.Features.Currencies.Queries.GetCurrencies;
using Applications.Features.Ledgers.Commands.UpdateLedger;
using Applications.Features.Ledgers.Dto;
using Applications.Features.Ledgers.Queries.GetLedgerByCurrency;
using Applications.Features.Ledgers.Queries.GetLedgerForUpdateById;
using Applications.Features.Ledgers.Queries.GetLedgerTypes;
using Mapster;

namespace TrapchumApp.Pages.Transactions.Ledgers;

public partial class EditLedgerModal
{
    [Parameter] public int Id { get; set; }
    IEnumerable<LedgerTypeDto>? LedgerTypes = [];
    IEnumerable<CurrencyDto>? Currencies = [];
    IEnumerable<VLedger>? FilterLedgers = [];
    LedgerUpdateDto model = new LedgerUpdateDto();
    EditForm? editForm;
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

        var resp = await mediator.Send(new GetLedgerTypesQuery(0, new QueryParam(0, 0, string.Empty, filterTypeText)));

        LedgerTypes = resp.Items.Adapt<IEnumerable<LedgerTypeDto>>();

    }

    protected async Task GetLedgers(int? currencyId = 1, string? searchText = null)
    {
        var resp = await mediator.Send(new GetLedgerByCurrencyQuery(0, currencyId, new QueryParam(0, 0, string.Empty, searchText)));

        FilterLedgers = resp.Items;

    }

    void Close(bool IsSuccess = false) => DialogService.Close(IsSuccess);

    protected async Task FormValidSubmit()
    {
        if (editForm?.EditContext?.Validate() == false) return;

        if (model.IsSubTypeOf && model.ParentRefId <= 0)
        {
            _toastService.ShowError("Please select ledger name.", "Error");

            return;
        }

        if (model.LedgerTypeRefId == 1 && string.IsNullOrEmpty(model.BankNumber))
        {
            _toastService.ShowError("please input bank acoount number.", "Error");

            return;
        }

        var result = await mediator.Send(new UpdateLedgerCommand(model));

        if (result.IsSuccess())
        {
            _toastService.ShowSuccess("", "Success");

            model = new();

            Close(true);
        }
        else
        {
            _toastService.ShowError(result.GetMessage(), "Error");
        }
    }
    protected override async Task OnParametersSetAsync()
    {
        var resp = await mediator.Send(new GetLedgerTypesQuery(0, new QueryParam(0, 0)));

        LedgerTypes = resp.Items.Adapt<IEnumerable<LedgerTypeDto>>();

        var command = new GetCurrenciesQuery(null, new QueryParam(0, 0));

        var rsp = await mediator.Send(command);

        Currencies = rsp.Items.Maps<CurrencyDto>();

        await base.OnParametersSetAsync();
    }
    protected override async Task OnInitializedAsync()
    {

        var res = await mediator.Send(new GetLedgerForUpdateByIdQuery(Id));

        if (res.IsSuccess())
        {
            model = res.Value;

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
        var currencyId = ((CurrencyDto)param).CurrencyId;

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