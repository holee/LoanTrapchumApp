using Applications.Features.Categories.Requests;
using Applications.Services.Loans;

namespace Web.UI.Pages.Transactions.Loans.LoanCategoriess
{
    public partial class LoanCategoryModalEdit
    {
        [Parameter] public int Id { get; set; }
        LoanCategoryDtoUpdate model { get; set; } = new();

        private readonly ILoanCategoryServiceImpl service;
        public LoanCategoryModalEdit(ILoanCategoryServiceImpl service)
        {
            this.service = service;
        }
        protected bool IsActive = false;
        protected void Cancel() => DialogService.Close(false);

        private bool isValid = true;
        protected override async Task OnInitializedAsync()
        {
            await GetLoanCategoryById();
            await base.OnInitializedAsync();
        }
        protected async Task OnSaveChanges()
        {
            try
            {
                var req = await service.UpdateAsync(model!);

                if (!req.IsSuccess)
                {

                }

            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Exception", ex.Message);
            }
        }


        async Task GetLoanCategoryById()
        {
            var result = await service.GetForEditAsync(Id);

            if (result.IsSuccess)
            {
                model = result.Data!;
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, "Error", result.ErrMessage);
            }
        }
    }
}