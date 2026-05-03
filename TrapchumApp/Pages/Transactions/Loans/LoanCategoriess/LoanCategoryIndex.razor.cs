using Applications.Responses.Loans.Disbursements;
using Applications.Responses.ViewModels.Loans.Categories;
using Applications.Services.Loans;
using Domains.Models.Bases;
using Mapster;
using Radzen.Blazor;
using Applications.Features.Categories.Requests;

namespace TrapchumApp.Pages.Transactions.Loans.LoanCategoriess
{
    public partial class LoanCategoryIndex
    {
        [Parameter] public int SelectIndex { get; set; } = 0;

        private readonly ILoanCategoryServiceImpl _catService;

        public LoanCategoryIndex(ILoanCategoryServiceImpl service)
        {
            _catService = service;
        }

        List<VlCategoryList?> loanCategories = [];

        protected string? searchText = string.Empty;

        protected bool InActive = false;

        List<VlCategoryList> vLCategories = [];

        int pageSize = 15;
        int pageIndex = 0;
        int CategoryId = 0;
        bool sortDescending = false;
        string sortColumn = "LoanCategoryId";
        bool isLoading = false;
        int counts = 0;
        RadzenTabs? tabs;
        private bool IsNew = false;
        private bool IsEdit = false;
        private bool IsDetails = false;
        protected LoanCategoryDtoUpdate? modelEdit = new();
        protected LoanCategoryDto? detailsModel = new();

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
        async Task EditCategory(int? id)
        {
            ContextMenuService.Close();

            try
            {
                var options = new DialogOptions
                {
                    Draggable = true,
                };

                var parameters = new Dictionary<string, object?>()
                {
                    { "Id",id }
                };


                var dia = await DialogService.OpenAsync<LoanCategoryModalEdit>("Edit", parameters, options);

                if (dia is bool result && result)
                {
                    await GetLoanCategories(searchText);
                }

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
                    detailsModel = result.Data.Adapt<LoanCategoryDto>();

                    var parameters = new Dictionary<string, object?>
                    {
                        { "category", detailsModel }
                    };

                    var options = new DialogOptions { Draggable = true, Width = "600px" };

                    await DialogService.OpenAsync<LCDetailModal>("Category Details", parameters, options);
                }

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

            (sortColumn, sortDescending) = e.PageInfo();

            var filterField = e.Filters?.FirstOrDefault()?.Property;
            var filterValue = e.Filters?.FirstOrDefault()?.FilterValue.ToString();


            var filterColumn = e.Filters?.FirstOrDefault(p =>
                                        p.Property == nameof(VlCategoryList.Description) ||
                                        p.Property == nameof(VlCategoryList.DesInKhmer) ||
                                        p.Property == nameof(VlCategoryList.Code) ||
                                        p.Property == nameof(VlCategoryList.LoancategoryCode))
                                       ?.FilterValue.ToString() ?? string.Empty;

            searchText = filterColumn ?? string.Empty;

            await GetLoanCategories(searchText);
        }
        protected async void PerformSearch(string filter)
        {
            searchText = filter;

            await GetLoanCategories(filter);
        }


        protected async Task OnIsActive()
        {
            await GetLoanCategories();
        }

        protected async Task DeactiveOrActive(bool? arg, VlCategoryList cat)
        {

            var confirm = await DialogService.Confirm("Do you want to disable or enable Category?.");

            if (confirm == false) return;

            var state = new ChangeState(arg, cat.EditSeq);

            var rsp = await _catService.UpdateStatusAsync(cat.LoanCategoryId, state);

            if (rsp.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Success, "Loan Category", rsp.Data);

                await GetLoanCategories(searchText);
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, rsp.ErrMessage);
            }

        }
        protected override async Task OnInitializedAsync()
        {
            await GetLoanCategories(searchText);

            await base.OnInitializedAsync();
        }


        async Task PageChanged(int page)
        {
            pageSize = page;

            pageIndex = 0;

            await GetLoanCategories(searchText);
        }



        async Task GetLoanCategories(string? filter = null)
        {
            try
            {
                var param = new QueryParam
                {
                    Page = pageIndex,
                    Take = pageSize,
                    Filter = filter,
                    SortColumn = sortColumn,
                    IncludeInActive = InActive,
                    SortDescending = sortDescending
                };

                var results = await _catService.GetCategoriesAsync(param);

                vLCategories = results.Items;

                counts = results.Count;

                StateHasChanged();
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


            var options = new DialogOptions
            {
                Draggable = true,
            };

            var dia = await DialogService.OpenAsync<LoanCategoryModalCreate>("Categogry", options: options);


            if (dia is bool result && result == true)
            {
                await GetLoanCategories(searchText);
            }

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