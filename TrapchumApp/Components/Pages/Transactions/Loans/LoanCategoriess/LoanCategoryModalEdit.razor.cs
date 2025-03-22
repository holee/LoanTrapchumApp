using AppilcationServices.Interfaces;
using Microsoft.AspNetCore.Components;
using TCLMS.Shared.Entities.Dtos.Loans;

namespace TCLMS.Front.Pages.Transactions.Loans.LoanCategoriess
{
    public partial class LoanCategoryModalEdit
    {
        [Parameter] public int Id { get; set; }
        [Inject] private ILoanCategoryService? service { get; set; }

        [Parameter] public LoanCategoryDTOUpdate? model { get; set; }
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
                if (result != null && model!.EditSeq == result.EditSeq)
                {
                    if (await service.UpdateAsync(Id, model))
                    {
                        NotificationService.Notify(Radzen.NotificationSeverity.Success, "Updated", "Loan Category created successfully!");


                    }
                }
                else
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