using Applications.Interfaces.Loans;
using Applications.Requests.Loans.Products;
using Applications.Responses.ViewModels.Loans.Products;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace TrapchumApp.Components.Pages.Transactions.Loans.LoanProducts
{
    public partial class LoanProductList
    {

        int pageSize = 10;
        bool sortDescending = false;
        string sortColumn = "Id";
        bool isLoading = false;
        int counts = 0;
        protected RadzenTabs? tabs;
        [Parameter]
        public int SelectIndex { get; set; }
        private bool IsNew = false;
        private bool IsEdit = false;
        private bool IsDetails = false;
        protected LoanProductDto? DetailModel = new();

        async Task LPDetails(int Id)
        {
            try
            {
                //DetailModel = await service!.GetByIdAsync(Id);
                StateHasChanged();
                IsDetails = true;
                SelectIndex = 1;
                ContextMenuService.Close();
                tabs!.Reload();

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Exception", ex.Message);
                ContextMenuService.Close();

            }
        }
        // async void MenuClicked(RadzenSplitButtonItem args, VLProductList e)
        // {
        //     if (args is not null)
        //     {
        //         if (args.Value == "1")
        //         {
        //             await LPDetails(e.LoanProductId);
        //             return;
        //         }
        //         if (args.Value == "2")
        //         {
        //             await OnEdit(e);

        //             return;
        //         }
        //         if (args.Value == "3")
        //         {
        //             await OnDelete(e);
        //             return;
        //         }

        //     }
        //     return;
        // }
        void CloseTabs(int e)
        {
            if (SelectIndex == 1)
            {
                IsDetails = false;
            }
            // else if (SelectIndex == 2)
            // {
            //     IsEdit = false;
            // }
            // else if (SelectIndex == 3)
            // {
            //     IsDetails = false;
            //}
            SelectIndex = 0;
            StateHasChanged();
        }

        [Inject] private ILoanProductService? service { get; set; }
        protected string searchText { get; set; } = string.Empty;
        protected bool InActive = false;
        IQueryable<VlProductList>? VLProducts;
        List<int> SelectedRow = [10, 20, 50, 100];

        protected async void Reload()
        {
            await OnInitializedAsync();
        }

        protected async Task OnIsActive()
        {
            await OnInitializedAsync();
            await InvokeAsync(StateHasChanged);

        }
        protected async Task DeactiveOrActive(bool arg, VlProductList para)
        {
            //var message = await service!.IsActiveAsync(para.LoanProductId, arg);
            //if (message)
            //{
            //    NotificationService.Notify(NotificationSeverity.Success, "", "Loan product was updated");
            //    VLProducts!.ToList().Remove(para);
            //    await OnInitializedAsync();
            //    await InvokeAsync(StateHasChanged);
            //    return;
            //}
            //else
            //    NotificationService.Notify(NotificationSeverity.Success, "", "Loan product cannot update");
            await Task.CompletedTask;

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

            VLProducts = VLProducts!.Where(x => x.LoanProductName!.ToLower().Contains(searchText, StringComparison.InvariantCultureIgnoreCase)
            || x.LoanProductCode!.ToLower().Contains(searchText, StringComparison.InvariantCultureIgnoreCase));
            await InvokeAsync(StateHasChanged);
        }
        protected async Task PerformSearch(ChangeEventArgs e)
        {
            searchText = e.Value!.ToString()!;

            VLProducts = VLProducts!.Where(x => x.LoanProductName!.ToLower().Contains(searchText, StringComparison.InvariantCultureIgnoreCase)
            || x.LoanProductCode!.ToLower().Contains(searchText, StringComparison.InvariantCultureIgnoreCase));
            await InvokeAsync(StateHasChanged);
        }
        protected override async Task OnInitializedAsync()
        {
            try
            {
                var results = await service!.GetAllListAsyc(InActive, null);

                if (results.IsSuccess)
                {
                    VLProducts = results!.Data!.AsQueryable()!;
                }
            }
            catch (Exception ex)
            {
                await DialogService.Alert(ex.InnerException!.Message);
            }
            await base.OnInitializedAsync();
        }
        protected async Task OnEdit(VlProductList? loanProduct)
        {
            await Task.CompletedTask;
        }

        protected async void OnCreate()
        {
            await InvokeAsync(StateHasChanged);

        }

        async Task OnDelete(VlProductList? productDTO)
        {
            var confirm = (bool)await DialogService!.Confirm("Loan Product deleted will not be able to recover, would you like to continue?", "Delete Loan Product",
                new ConfirmOptions
                {
                    OkButtonText = "OK",
                    CancelButtonText = "Cancel",
                });
            if (confirm)
            {
                try
                {

                    if (productDTO?.IsInUse == "true")
                    {
                        var confirm2 = (bool)await DialogService!.Confirm("Loan Product you are trying to delete has started its operation, so you cannot delete it. Make inactive is strongly recommmended!", "Update Loan Product",
                            new ConfirmOptions
                            {
                                OkButtonText = "OK",
                                CancelButtonText = "Cancel",
                            });
                        if (confirm2)
                        {
                            //if (await service!.IsActiveAsync(productDTO.LoanProductId, false))
                            //{
                            //    NotificationService.Notify(NotificationSeverity.Success, "Update Loan Product", "Loan Product was made inactive!");

                            //    await OnInitializedAsync();
                            //}
                            return;
                        }
                        ContextMenuService.Close();

                    }
                    else
                    {
                        var isDe = await service!.DeleteAsync(productDTO!.LoanProductId);
                        if (isDe.IsSuccess)
                        {
                            NotificationService.Notify(NotificationSeverity.Success, "Delete Loan Product", "Loan product was deleted successfully.");
                            await OnInitializedAsync();
                        }
                        ContextMenuService.Close();

                    }
                }
                catch (Exception ex)
                {
                    NotificationService.Notify(NotificationSeverity.Success, "Exception", ex.InnerException!.Message);
                    ContextMenuService.Close();

                }

            }

        }
    }
}