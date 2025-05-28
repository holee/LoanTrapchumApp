using Applications.Requests.Loans.Categories;
using Applications.Services.Loans;
using Radzen;

namespace TrapchumApp.Components.Pages.Transactions.Loans.LoanCategoriess
{
    public partial class LoanCategoryModalCreate
    {
        private readonly ILoanCategoryServiceImpl service;

        public LoanCategoryModalCreate(ILoanCategoryServiceImpl service)
        {
            this.service = service;
        }

        LoanCategoryDtoCreate model = new LoanCategoryDtoCreate();

        protected bool IsActive = false;
        //protected void Cancel() => DialogService.Close(false);

        private string? messageName = "";

        private string? messageCode = "";

        protected async Task OnSaveNew()
        {
            try
            {
                var result = await service!.CreateAsync(model);

                if (result.IsSuccess)
                {
                    NotificationService.Notify(NotificationSeverity.Success, "Created", "Loan Category created successfully!");
                    model = new();
                    messageName = "";
                    messageCode = "";
                }
                else
                {
                    //if (result.Status == 2)
                    //{
                    //    messageName = result.Title;

                    //}
                    //else if (result.Status == 3)
                    //{
                    //    messageName = "";
                    //    messageCode = result.Title;
                    //}
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Exception", ex.Message);
            }
        }
    }
}