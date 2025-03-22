using AppilcationServices.Interfaces;
using Microsoft.AspNetCore.Components;
using TCLMS.Shared.Entities.Dtos.Loans;

namespace TCLMS.Front.Pages.Transactions.Loans.LoanCategoriess
{
    public partial class LoanCategoryModalCreate
    {
        [Inject] private ILoanCategoryService? service { get; set; }

        protected LoanCategoryDTOCreate model = new LoanCategoryDTOCreate();
        protected bool IsActive = false;
        protected void Cancel() => DialogService.Close(false);
        private string? messageName = "";
        private string? messageCode = "";

        protected async Task OnSaveNew()
        {
            try
            {
                var result = await service!.CreateAsync(model);
                if (result.IsSuccess)
                {
                    NotificationService.Notify(Radzen.NotificationSeverity.Success, "Created", "Loan Category created successfully!");
                    model = new();
                    messageName = "";
                    messageCode = "";
                }
                else
                {
                    if (result.Status == 2)
                    {
                        messageName = result.Title;

                    }
                    else if (result.Status == 3)
                    {
                        messageName = "";
                        messageCode = result.Title;
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(Radzen.NotificationSeverity.Error, "Exception", ex.Message);
            }
        }
    }
}