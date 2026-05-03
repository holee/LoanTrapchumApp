using Applications.Features.Currencies.Dto;
using Applications.Features.Currencies.Queries.GetCurrencies;
using Applications.Extensions.mappers;
using Applications.Features.Products.Requests;
using Applications.Interfaces.Loans;

namespace TrapchumApp.Pages.Transactions.Loans.LoanProducts
{
    public partial class LoanProductModalEdit
    {
        [Parameter] public int Id { get; set; }
        [Inject] private ILoanProductService? service { get; set; }

        protected LoanProductDtoUpdate model = new LoanProductDtoUpdate();

        protected IEnumerable<CurrencyDto> _currencies = [];

        protected bool IsActive = false;

        protected override async Task OnInitializedAsync()
        {
            var command = new GetCurrenciesQuery(null, new QueryParam(0, 0));
            var rsp = await mediator.Send(command);
            _currencies = rsp.Items.Maps<CurrencyDto>();
            await base.OnInitializedAsync();
        }

        protected async Task OnSaveChanges()
        {
            await Task.CompletedTask;
        }
    }
}