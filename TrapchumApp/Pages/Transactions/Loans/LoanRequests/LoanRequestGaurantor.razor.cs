using Applications.Responses.ViewModels.Loans.Requests;
using Applications.Services.Lists;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;

namespace TrapchumApp.Components.Pages.Transactions.Loans.LoanRequests
{
    public partial class LoanRequestGaurantor
    {
        [Parameter] public IEnumerable<VLoanRequestGuarantors> Guarantors { get; set; } = [];
        [Parameter] public bool ShowAdd { get; set; } = false;
        [Parameter] public bool ShowCommand { get; set; } = false; 
        [Parameter] public bool ShowRemove { get; set; } = false;
        [Parameter] public EventCallback Add { get; set; }
        [Parameter] public EventCallback Reload { get; set; }
        [Parameter] public EventCallback<VLoanRequestGuarantors> Remove { get; set; }

        RadzenDataGrid<VLoanRequestGuarantors> gridGuarantors = default!;

        private readonly IGuarantorServiceImpl _gService;

        public LoanRequestGaurantor(IGuarantorServiceImpl gService)
        {
            _gService = gService;
        }

        async void ClickedAdd()
        {
            await Add.InvokeAsync();
            //await Reload.InvokeAsync();
            //await gridGuarantors.Reload();
        }
        async void ClickedRevmove(VLoanRequestGuarantors g)
        {
            await Remove.InvokeAsync(g);
        }

        public async Task GuarantorReload()
        {
            await gridGuarantors.Reload();
        }

    }
}