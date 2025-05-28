using Applications.Interfaces.Loans;
using Applications.Requests.Loans.Categories;
using Applications.Responses.Loans.Disbursements;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans.Categories;
using Applications.Services.Loans;
using Mapster;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace TrapchumApp.Components.Pages.Transactions.Loans.LoanCategoriess
{
    public partial class LoanCategoryView
    {
        [Parameter] public int SelectIndex { get; set; } = 0;

        private readonly ILoanCategoryServiceImpl _catService;

        public LoanCategoryView(ILoanCategoryServiceImpl service)
        {
            _catService = service;
        }

        List<VlCategoryList?> loanCategories = [];

        protected string? searchText = string.Empty;

        protected bool InActive = false;

        List<VlCategoryList> vLCategories = [];

        int pageSize = 10;
        int pageIndex = 0;
        int CategoryId = 0;
        bool sortDescending = false;
        string sortColumn = "Id";
        bool isLoading = false;
        int counts = 0;
        RadzenTabs? tabs;
        private bool IsNew = false;
        private bool IsEdit = false;
        private bool IsDetails = false;
        protected LoanCategoryDtoUpdate? modelEdit = new();
        protected LoanCategoryResponse? detailsModel = new();

        void TabChanged(int e)
        {
            SelectIndex = e;
            StateHasChanged();

        }
        void AddNewCategory()
        {
            IsNew = true;
            SelectIndex = 1;
            tabs!.Reload();
        }
         void EditCategory(int id)
        {
            ContextMenuService.Close();

            try
            {

                CategoryId = id;

                //var rsp = await _catService!.GetForEditAsync(id);

                //if (rsp.IsSuccess)
                //{
                //    modelEdit = rsp.Data!;

                IsEdit = true;
                SelectIndex = 2;
                //}
                //else
                //{
                //    NotificationService.Notify(NotificationSeverity.Error, "Loan Category", rsp.ErrMessage);
                //    return;
                //}

                tabs!.Reload();

                StateHasChanged();


            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Exception", ex.Message);
            }
        }
        async Task Details(int Id)
        {
            ContextMenuService.Close();

            try
            {
                var result = await _catService!.GetLoanCategory(Id);

                if (result is not null)
                {
                    detailsModel = result.Data.Adapt<LoanCategoryResponse>();

                    IsDetails = true;

                    SelectIndex = 3;

                    tabs!.Reload();
                }

                ContextMenuService.Close();

                StateHasChanged();
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Exception", ex.Message);
              
            }
        }
        void CloseTabs(int e)
        {
            if (SelectIndex == 1)
            {
                IsNew = false;
            }
            else if (SelectIndex == 2)
            {
                IsEdit = false;
            }
            else if (SelectIndex == 3)
            {
                IsDetails = false;
            }
            SelectIndex = 0;
            StateHasChanged();
        }
        protected async void Reload()
        {
            await GetLoanCategories();
        }
        async void LoadData(LoadDataArgs e)
        {
            pageIndex = e.Skip!.Value;

            searchText = e.Filter!.ToString()!;

            if (!string.IsNullOrEmpty(e.OrderBy))
            {
                sortColumn = e.OrderBy.Split(" ")[0];
                string dir = e.OrderBy.Split(" ")[1];
                sortDescending = dir.Contains("asc") ? false : true;
            }

            await GetLoanCategories();
        }
        protected async void PerformSearch(string filter)
        {
            searchText = filter;

            await GetLoanCategories();
        }


        protected async Task OnIsActive()
        {
            await GetLoanCategories();
        }

        protected async Task DeactiveOrActive(VlCategoryList category)
        {
            try
            {
                var rsp = await _catService!.UpdateStatus(category.LoanCategoryId);

                if (rsp.IsSuccess)
                {
                    NotificationService.Notify(NotificationSeverity.Success, "Loan Category", rsp.Data);
                    vLCategories!.ToList().Remove(category);
                    await OnInitializedAsync();
                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, rsp.ErrMessage);
                    return;
                }

            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Exception", ex.InnerException!.Message);
                await OnInitializedAsync();
            }
            await Task.CompletedTask;
        }
        protected override async Task OnInitializedAsync()
        {
            await GetLoanCategories();
            await base.OnInitializedAsync();
        }


        async Task PageChanged(int page)
        {
            pageSize = page;
            await GetLoanCategories();
        }



        async Task GetLoanCategories()
        {
            try
            {
                var param = new QueryParam
                {
                    Skip = pageIndex,
                    PageSize = pageSize,
                    Filter = searchText,
                    SortColumn = sortColumn,
                    IsIncludeInActive = InActive,
                    SortDescending = sortDescending
                };

                var results = await _catService.GetCategoriesAsync(param);
                vLCategories = results.Items;
                counts = vLCategories.Count();
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Exception", ex.InnerException!.Message);
            }
        }



        protected async Task OnDetail(VlCategoryList? loanCategory)
        {

            await Task.CompletedTask;
        }
        protected async Task OnEdit(VlCategoryList? loanCategory)
        {

            await Task.CompletedTask;
        }
        protected async void OnCreate()
        {

            await Task.CompletedTask;

        }

        async Task OnDelete(VlCategoryList? loanCategory)
        {

            if (await DialogService!.Confirm("Loan Category deleted will not be able to recover, would you like to continue?", "Delete Loan Category",
                new ConfirmOptions
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
                        new ConfirmOptions
                        {
                            OkButtonText = "OK",
                            CancelButtonText = "Cancel",
                        }) == true)
                        {
                            var message = await _catService!.UpdateStatus(loanCategory.LoanCategoryId);
                            if (message.IsSuccess)
                            {
                                NotificationService.Notify(NotificationSeverity.Success, "Update Loan Category", "Loan Category was made inactive!");

                                await OnInitializedAsync();
                            }
                            ;
                            ContextMenuService.Close();

                            return;
                        }
                    }
                    else
                    {
                        var isDe = await _catService!.DeleteAsync(loanCategory.LoanCategoryId);

                        if (isDe.IsSuccess)
                        {
                            NotificationService.Notify(NotificationSeverity.Success, "Delete Loan Category", "Loan category was deleted successfully.");

                            ContextMenuService.Close();

                            await OnInitializedAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    NotificationService.Notify(NotificationSeverity.Error, "Exception", ex.InnerException!.Message);
                    ContextMenuService.Close();

                }

            }

        }
    }
}