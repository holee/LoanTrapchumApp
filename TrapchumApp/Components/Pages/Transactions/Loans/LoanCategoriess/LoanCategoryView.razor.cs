using Applications.Interfaces.Loans;
using Applications.Requests.Loans;
using Applications.Responses.Loans.Disbursements;
using Applications.Responses.ViewModels.Loans.Categories;
using Mapster;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace TrapchumApp.Components.Pages.Transactions.Loans.LoanCategoriess
{
    public partial class LoanCategoryView
    {
        private readonly ILoanCategoryService? _catService;

        public LoanCategoryView(ILoanCategoryService? service)
        {
            this._catService = service;
        }

        List<VlCategoryList?> loanCategories = [];
        protected string? searchText = string.Empty;

        protected bool InActive = false;

        List<VlCategoryList>? vLCategories = [];

        List<int> SelectedRow = [10, 20, 50, 100];


        int pageSize = 10;
        bool sortDescending = false;
        string sortColumn = "Id";
        bool isLoading = false;
        int counts = 0;
        RadzenTabs? tabs;
        private bool IsNew = false;
        private bool IsEdit = false;
        private bool IsDetails = false;
        [Parameter] public int SelectIndex { get; set; } = 0;
        protected LoanCategoryDtoUpdate? modelEdit = new();
        protected LoanCategoryResponse? detailsModel = new();

        async void MenuClicked(RadzenSplitButtonItem args, VlCategoryList? loanCategory)
        {
            if (args is not null)
            {
                if (args.Value == "1")
                {
                    await Details(loanCategory!.LoanCategoryId);
                    return;
                }
                if (args.Value == "2")
                {
                    EditCategory(loanCategory!.LoanCategoryId);
                    return;
                }
                if (args.Value == "3")
                {
                    await OnDelete(loanCategory);
                    return;
                }

            }
            return;
        }
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
        async void EditCategory(int id)
        {
            try
            {

                modelEdit = await _catService!.GetForEditAsync(id);

                IsEdit = true;

                SelectIndex = 2;

                StateHasChanged();

                tabs!.Reload();

                ContextMenuService.Close();

            }
            catch (Exception ex)
            {
                NotificationService.Notify(Radzen.NotificationSeverity.Error, "Exception", ex.Message);
                ContextMenuService.Close();

            }
        }
        async Task Details(int Id)
        {
            try
            {
                var result = await _catService!.GetLoanCategory(Id);

                if (result is not null)
                {
                    detailsModel = result.Adapt<LoanCategoryResponse>();

                    IsDetails = true;

                    SelectIndex = 3;

                    tabs!.Reload();
                }

                ContextMenuService.Close();

                StateHasChanged();
            }
            catch (Exception ex)
            {
                NotificationService.Notify(Radzen.NotificationSeverity.Error, "Exception", ex.Message);
                ContextMenuService.Close();

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

            //vLCategories = vLCategories!.Where(x => x.LoancategoryCode!.ToLower().Contains(searchText.ToLower(), StringComparison.CurrentCultureIgnoreCase) ||
            //x.DesInKhmer!.ToLower().Contains(searchText.ToLower(), StringComparison.CurrentCultureIgnoreCase) ||
            //x.Code!.ToLower().Contains(searchText.ToLower(), StringComparison.CurrentCultureIgnoreCase) ||
            //x.Description!.ToLower().Contains(searchText.ToLower(), StringComparison.CurrentCultureIgnoreCase));
            await InvokeAsync(StateHasChanged);
        }
        protected async void PerformSearch(ChangeEventArgs e)
        {
            searchText = e.Value!.ToString()!;

            //vLCategories = vLCategories!.Where(x => x.LoancategoryCode!.ToLower().Contains(searchText.ToLower(), StringComparison.CurrentCultureIgnoreCase) ||
            //                            x.DesInKhmer!.ToLower().Contains(searchText.ToLower(), StringComparison.CurrentCultureIgnoreCase) ||
            //                            x.Code!.ToLower().Contains(searchText.ToLower(), StringComparison.CurrentCultureIgnoreCase) ||
            //                            x.Description!.ToLower().Contains(searchText.ToLower(), StringComparison.CurrentCultureIgnoreCase));

            await InvokeAsync(StateHasChanged);

        }


        protected async Task OnIsActive()
        {
            await OnInitializedAsync();
            await InvokeAsync(StateHasChanged);
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
            try
            {
                var results = await _catService!.GetListCategories(InActive, null);

                if (results.Any())
                {
                    vLCategories = results.ToList();

                    counts = vLCategories.Count();
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Exception", ex.InnerException!.Message);
            }
            await base.OnInitializedAsync();
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
                        var isDe = await _catService!.Delete(loanCategory.LoanCategoryId);

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