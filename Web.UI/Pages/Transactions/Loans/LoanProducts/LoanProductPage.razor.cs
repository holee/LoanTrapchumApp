using Applications.Responses.Loans.Disbursements;
using Applications.Responses.ViewModels.Loans.Products;
using Applications.Services.Loans;
using Radzen.Blazor;

namespace Web.UI.Pages.Transactions.Loans.LoanProducts
{
    public partial class LoanProductPage
    {

        private readonly ILoanProductServiceImpl _loanProductService;

        public LoanProductPage(ILoanProductServiceImpl loanProductService)
        {
            _loanProductService = loanProductService;
        }

        int pageSize = 15;
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

            await GetLoanProducts(searchText);

        }

        async Task PageSizeChanged(int page)
        {
            pageSize = page;

            pageIndex = 0;

            await GetLoanProducts(searchText);
        }

        protected async Task ChangeStatus(bool arg, int id)
        {
            var confirm = await DialogService.Confirm("Do you disable or enable this Product?");

            if (confirm == false) return;

            var rsp = await _loanProductService.UpdateAsync(id, arg);

            if (rsp.IsSuccess)
            {
                await GetLoanProducts();
            }
            else
                NotificationService.Notify(NotificationSeverity.Success, rsp.ErrMessage);

        }
        async Task LoadData(LoadDataArgs e)
        {

            searchText = e.Filter!.ToString()!;

            pageIndex = e.Skip!.Value;

            (sortColumn, sortDescending) = e.PageInfo();

            var filterColumn = e.Filters?.FirstOrDefault(p =>
                                p.Property == nameof(VlProductList.LoanSize) ||
                                p.Property == nameof(VlProductList.LoanProductCode) ||
                                p.Property == nameof(VlProductList.LoanProductName)
                                )?.FilterValue.ToString() ?? string.Empty;

            searchText = filterColumn ?? string.Empty;

            await GetLoanProducts(searchText);
        }
        protected override async Task OnInitializedAsync()
        {
            await GetLoanProducts();

            await base.OnInitializedAsync();
        }

        async Task GetLoanProducts(string? filter = null)
        {
            try
            {
                isLoading = true;

                var param = new QueryParam(pageIndex, pageSize, sortColumn, IsIncludeInActive, sortDescending, filter);

                var results = await _loanProductService.GetLoanProductsAsync(param);

                counts = results.Count;

                VLProducts = results.Items;

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

            ContextMenuService.Close();

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
                       

                    }
                    else
                    {
                        var isDe = await _loanProductService!.DeleteAsync(productDTO!.LoanProductId);
                        if (isDe.IsSuccess)
                        {
                            NotificationService.Notify(NotificationSeverity.Success, "Delete Loan Product", "Loan product was deleted successfully.");
                            await OnInitializedAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    NotificationService.Notify(NotificationSeverity.Success, "Exception", ex.InnerException!.Message);
                }

            }

        }
    }
}