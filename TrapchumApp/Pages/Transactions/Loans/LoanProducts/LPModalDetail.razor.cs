using Applications.Responses.ViewModels.Loans.Products;
using Applications.Services.Loans;
using Microsoft.AspNetCore.Components;

namespace TrapchumApp.Pages.Transactions.Loans.LoanProducts
{
    public partial class LPModalDetail
    {
        [Parameter] public int? Id { get; set; }
        VlProductList model = new();
        private readonly ILoanProductServiceImpl _loanProductService;
        public LPModalDetail(ILoanProductServiceImpl service)
        {
            _loanProductService = service;
        }
        protected bool IsActive = false;
        protected override async Task OnInitializedAsync()
        {
            await GetLoanProductById(Id);

            await base.OnInitializedAsync();
        }

        async Task GetLoanProductById(int? id)
        {
            if (Id.HasValue && Id > 0)
            {
                var rsp = await _loanProductService.GetByIdAsync<VlProductList>(id);

                if (rsp.IsSuccess)
                {
                    model = rsp.Data!;
                }
            }
        }
    }
}