using Applications.Responses.ViewModels.Loans.Requests;
using Applications.Services.Lists;
using Radzen.Blazor;

namespace TrapchumApp.Pages.Transactions.LoanRequests
{
    public partial class LoanRequestGaurantor
    {
        [Parameter] public IEnumerable<VLoanRequestGuarantor> Guarantors { get; set; } = [];
        [Parameter] public bool ShowAdd { get; set; } = false;
        [Parameter] public bool ShowCommand { get; set; } = false; 
        [Parameter] public bool ShowRemove { get; set; } = false;
        [Parameter] public EventCallback Add { get; set; }
        
        [Parameter] public EventCallback Reload { get; set; }
        [Parameter] public EventCallback<VLoanRequestGuarantor> Remove { get; set; }

        RadzenDataGrid<VLoanRequestGuarantor> gridGuarantors = default!;

        [Inject] private IGuarantorServiceImpl _gService { get; set; } = default!;

        private async Task ClickedAdd()
        {
            await Add.InvokeAsync();
        }

        private async Task ClickedRemove(VLoanRequestGuarantor g)
        {
            await Remove.InvokeAsync(g);
        }

        public async Task GuarantorReload()
        {
            await gridGuarantors.Reload();
        }

    }
}