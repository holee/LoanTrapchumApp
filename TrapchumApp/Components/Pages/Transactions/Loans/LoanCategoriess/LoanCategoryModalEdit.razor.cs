using Applications.Interfaces.Loans;
using Applications.Requests.Loans;
using Applications.Services.Loans;
using Microsoft.AspNetCore.Components;

namespace TrapchumApp.Components.Pages.Transactions.Loans.LoanCategoriess
{
    public partial class LoanCategoryModalEdit
    {
        [Parameter] public int Id { get; set; }

        private readonly ILoanCategoryServiceImpl service;

        public LoanCategoryModalEdit(ILoanCategoryServiceImpl service)
        {
            this.service = service;
        }

        [Parameter] public LoanCategoryDtoUpdate model { get; set; } = new();
        protected bool IsActive = false;
        protected void Cancel() => DialogService.Close(false);
        private bool isValid = true;
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }
        protected async Task OnSaveChanges()
        {
            try
            {
                var result = await service!.GetForEditAsync(Id);

                if (result is not null && model!.EditSeq != result.EditSeq)
                {

                }

                var req = await service.UpdateAsync(model);

                if (!req.IsSuccess)
                {

                }

            }
            catch (Exception ex)
            {
                NotificationService.Notify(Radzen.NotificationSeverity.Error, "Exception", ex.Message);

            }
        }
    }
}