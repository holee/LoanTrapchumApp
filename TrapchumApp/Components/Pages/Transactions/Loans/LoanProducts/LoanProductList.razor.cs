using Applications.Interfaces.Loans;
using Applications.Responses.Loans.Disbursements;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans.Products;
using Applications.Services.Loans;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace TrapchumApp.Components.Pages.Transactions.Loans.LoanProducts
{
    public partial class LoanProductList
    {

        private readonly ILoanProductServiceImpl _loanProductService;

        public LoanProductList(ILoanProductServiceImpl loanProductService)
        {
            _loanProductService = loanProductService;
        }

        int pageSize = 10;
        int pageIndex = 0;
        bool sortDescending = false;
        string sortColumn = "Id";
        bool isLoading = false;
        int counts = 0;
        protected RadzenTabs? tabs;
        [Parameter] public int SelectIndex { get; set; }
        private bool IsNew = false;
        private bool IsEdit = false;
        private bool IsDetails = false;
        protected LoanProductResponse? DetailModel = new();
        bool IsIncludeInActive = false;
        RadzenDataGrid<VlProductList> dataGridRef = default!;
        protected string searchText = string.Empty;
        protected bool InActive = false;
        List<VlProductList> VLProducts = [];
        int Id = 0;
        void LPDetails(int id)
        {
            try
            {
                ContextMenuService.Close();
                IsDetails = true;
                SelectIndex = 1;
                Id = id;
                tabs!.Reload();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Exception", ex.Message);
                ContextMenuService.Close();

            }
        }

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
        protected async void Reload()
        {
            await GetLoanProducts();
        }

        protected async Task OnIsActive(bool args)
        {
            IsIncludeInActive = args;
            await GetLoanProducts();

        }
        protected async Task DeactiveOrActive(bool arg, VlProductList para)
        {
            var rsp = await _loanProductService.UpdateAsync(para.LoanProductId, arg);

            if (rsp.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Success, rsp.Data);
                para.IsActive = arg;
                await GetLoanProducts();

            }
            else
                NotificationService.Notify(NotificationSeverity.Success, rsp.ErrMessage);

        }
        async Task LoadData(LoadDataArgs e)
        {

            searchText = e.Filter!.ToString()!;

            pageIndex = e.Skip!.Value;

            if (!string.IsNullOrEmpty(e.OrderBy))
            {
                sortColumn = e.OrderBy.Split(" ")[0];
                string dir = e.OrderBy.Split(" ")[1];
                sortDescending = dir.Contains("asc") ? false : true;
            }

            await GetLoanProducts();
        }
        protected override async Task OnInitializedAsync()
        {
            await GetLoanProducts();

            await base.OnInitializedAsync();
        }

        async Task GetLoanProducts()
        {
            try
            {
                isLoading = true;


                var param = new QueryParam
                {
                    PageSize = pageSize,
                    PageNumber = pageIndex,
                    IsIncludeInActive = IsIncludeInActive,
                    SortColumn = sortColumn,
                    SortDescending = sortDescending,
                    Filter = searchText
                };

                var results = await _loanProductService!.GetLoanProductsAsync(param);

                counts = results.Count;

                VLProducts = results!.Items;
                isLoading = false;
                StateHasChanged();

            }
            catch (Exception ex)
            {
                await DialogService.Alert(ex.InnerException!.Message);
                isLoading = false;
            }
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
                        var isDe = await _loanProductService!.DeleteAsync(productDTO!.LoanProductId);
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