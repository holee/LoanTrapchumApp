using AppilcationServices.Interfaces;
using Microsoft.AspNetCore.Components;
using Radzen;
using TCLMS.Shared.Entities.Paginations;
using TCLMS.Shared.Entities.ViewModels.Loans.Categories;

namespace TCLMS.Front.Pages.Transactions.Loans.LoanCategoriess
{
    public partial class LoanCategoryView
    {
        [Inject] private ILoanCategoryService? service { get; set; }
        protected List<VLCategoryList?> loanCategories { get; set; } = null!;
        protected string? searchText { get; set; } = string.Empty;
        protected bool InActive = false;
        IQueryable<VLCategoryList>? vLCategories;
        List<int> SelectedRow = [10, 20, 50, 100];



        protected async void Reload()
        {
            await OnInitializedAsync();
        }
        async void LoadData(LoadDataArgs e)
        {
            //pageIndex = e.Skip!.Value;
            //if (!string.IsNullOrEmpty(e.OrderBy))
            //{
            //    sortColumn = e.OrderBy.Split(" ")[0];
            //    string dir = e.OrderBy.Split(" ")[1];
            //    sortDescending = dir.Contains("asc") ? false : true;
            //}
            searchText = e.Filter!.ToString()!;

            vLCategories = vLCategories!.Where(x => x.LoancategoryCode!.ToLower().Contains(searchText.ToLower(), StringComparison.CurrentCultureIgnoreCase) ||
            x.DesInKhmer!.ToLower().Contains(searchText.ToLower(), StringComparison.CurrentCultureIgnoreCase) ||
            x.Code!.ToLower().Contains(searchText.ToLower(), StringComparison.CurrentCultureIgnoreCase) ||
            x.Description!.ToLower().Contains(searchText.ToLower(), StringComparison.CurrentCultureIgnoreCase));
            await InvokeAsync(StateHasChanged);
        }
        protected async void PerformSearch(ChangeEventArgs e)
        {
            searchText = e.Value!.ToString()!;
            vLCategories = vLCategories!.Where(x => x.LoancategoryCode!.ToLower().Contains(searchText.ToLower(), StringComparison.CurrentCultureIgnoreCase) ||
            x.DesInKhmer!.ToLower().Contains(searchText.ToLower(), StringComparison.CurrentCultureIgnoreCase) ||
            x.Code!.ToLower().Contains(searchText.ToLower(), StringComparison.CurrentCultureIgnoreCase) ||
            x.Description!.ToLower().Contains(searchText.ToLower(), StringComparison.CurrentCultureIgnoreCase));
            await InvokeAsync(StateHasChanged);

        }


        protected async Task OnIsActive()
        {
            await OnInitializedAsync();
            await InvokeAsync(StateHasChanged);
        }

        protected async Task DeactiveOrActive(VLCategoryList category)
        {
            try
            {
                var message = await service!.UpdateStatus(category.LoanCategoryID);
                if (message.IsSuccess)
                {
                    NotificationService.Notify(Radzen.NotificationSeverity.Success, "Loan Category",message.Data!.Message);
                    vLCategories!.ToList().Remove(category);
                    await OnInitializedAsync();
                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    var error = message.Errors!.FirstOrDefault();
                    NotificationService.Notify(NotificationSeverity.Error, error);
                    return;
                }
                    
            }
            catch (Exception ex)
            {
                NotificationService.Notify(Radzen.NotificationSeverity.Error, "Exception", ex.InnerException!.Message);
                await OnInitializedAsync();
            }
            await Task.CompletedTask;
        }
        protected override async Task OnInitializedAsync()
        {
            try
            {
                var results = await service!.GetAllListAsync(InActive, null);
                if (results.IsSuccess)
                {
                    vLCategories = results!.Data!.AsQueryable()!;
                    counts=vLCategories.Count();
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(Radzen.NotificationSeverity.Error, "Exception", ex.InnerException!.Message);
            }
            await base.OnInitializedAsync();
        }
        protected async Task OnDetail(VLCategoryList? loanCategory)
        {

            await Task.CompletedTask;
        }
        protected async Task OnEdit(VLCategoryList? loanCategory)
        {

            await Task.CompletedTask;
        }
        protected async void OnCreate()
        {

            await Task.CompletedTask;

        }
        
        async Task OnDelete(VLCategoryList? loanCategory)
        {

            if (await DialogService!.Confirm("Loan Category deleted will not be able to recover, would you like to continue?", "Delete Loan Category",
                new Radzen.ConfirmOptions
                {
                    OkButtonText = "OK",
                    CancelButtonText = "Cancel",
                }) == true)
            {

                try
                {


                    if (loanCategory!.IsInUse == "true")
                    {
                        if (await DialogService!.Confirm("Loan Category you are trying to delete has started its operation, so you cannot delete it. Make inactive is strongly recommmended!", "Delete Loan Category",
                        new Radzen.ConfirmOptions
                        {
                            OkButtonText = "OK",
                            CancelButtonText = "Cancel",
                        }) == true)
                        {
                            var message = await service!.UpdateStatus(loanCategory.LoanCategoryID);
                            if (message.IsSuccess)
                            {
                                NotificationService.Notify(Radzen.NotificationSeverity.Success, "Update Loan Category", "Loan Category was made inactive!");

                                await OnInitializedAsync();
                            };
                            ContextMenuService.Close();

                            return;
                        }
                    }
                    else
                    {
                        var isDe = await service!.DeleteAsync(loanCategory.LoanCategoryID);
                        if (isDe.IsSuccess)
                        {
                            NotificationService.Notify(Radzen.NotificationSeverity.Success, "Delete Loan Category", "Loan category was deleted successfully.");

            ContextMenuService.Close();

                            await OnInitializedAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    NotificationService.Notify(Radzen.NotificationSeverity.Error, "Exception", ex.InnerException!.Message);
                    ContextMenuService.Close();

                }

            }

        }
    }
}