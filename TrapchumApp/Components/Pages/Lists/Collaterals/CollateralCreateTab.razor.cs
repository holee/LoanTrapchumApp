using Applications.Interfaces.Customers;
using Applications.Requests.Collaterals;
using Applications.Responses;
using Applications.Responses.Collaterals;
using Applications.Services.LIsts;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace TrapchumApp.Components.Pages.Lists.Collaterals
{
    public partial class CollateralCreateTab
    {
        private readonly ICollateralsServiceImpl _collateralService;
        private readonly ICustomerService _customerService;
        public CollateralCreateTab(ICollateralsServiceImpl collateralsService, ICustomerService cuCustomerService)
        {
            _collateralService = collateralsService;
            _customerService = cuCustomerService;
        }

        [Parameter] public Guid? CustomerId { get; set; }
        [Parameter] public string? CustomerName { get; set; } = string.Empty;
        IEnumerable<VCustomerList>? custlist = new List<VCustomerList>();
        IEnumerable<CustomerCollateral> cusName = [];
        IEnumerable<CollateralTypeResponse> CollateralTypes = [];
        IEnumerable<CollateralSubTypeResponse> CollateralProperties = [];
        IEnumerable<CollateralSubTypeResponse> CollateralPersonals = [];
        IEnumerable<CollateralSubTypeResponse> allSubTypes = [];
        List<CollateralLineResponse> Properties = [];
        List<CollateralLineResponse> Personals = [];
        EditForm editForm { get; set; } = default!;
        CollateralTypeDto? SelectCollateralType;
        CollateralDtoCreate model { get; set; } = new();

        Guid? CollateralId;
        protected override async Task OnInitializedAsync()
        {
            CollateralTypes = await _collateralService.GetCollateralTypes(0);
            CollateralProperties = await _collateralService.GetCollateralSubTypes(1);
            CollateralPersonals = await _collateralService.GetCollateralSubTypes(3);
            await GetCollateralLineByCustomer();
            allSubTypes = await _collateralService.GetCollateralSubTypes();
            await base.OnInitializedAsync();
        }
        string? q = string.Empty;
        protected string? strText = "";

        bool includeisactive = false;


        void SelectedPeopleChanged()
        {

        }

        protected override async Task OnParametersSetAsync()
        {
            var customerResponse = await _customerService.GetCustomerByIdAsync(CustomerId);

            if (customerResponse.IsSuccess)
            {
                CustomerName = customerResponse!.Data!.AltName;
            }

            await base.OnParametersSetAsync();
        }


        protected async Task GetCollateralLineByCustomer()
        {
            if (CustomerId.HasValue)
            {
                return;
            }
            var customerResult = await _collateralService.GetCollateralByCustomerAsync(CustomerId);

            if (customerResult.IsSuccess)
            {
                var collateral = customerResult.Data!;
                CollateralId = CustomerId!.Value;
                model!.CollateralId = CustomerId.Value;
                model.CustomerName = CustomerName;
                model.Remark = collateral!.Remark;
                model.DocumentKeeping = collateral!.DocumentKeeping;
                model.RegisterDate = collateral!.RegisterDate;
                Personals = collateral.CollateralLines
                              .Where(x => x.TypeName == "Personal")
                              .ToList();
                Properties = collateral.CollateralLines
                                .Where(x => x.TypeName == "Property")
                                .ToList();
            }
            else
            {
                CollateralId = 0;
                model!.CustomerName = CustomerName;
                model.CollateralId = CustomerId.Value;
                model.Remark = "";
                model.DocumentKeeping = "";
                model.RegisterDate = DateTime.Now;
            }
        }

        private void AddNewRow(int collateralTypeId)
        {
            if (collateralTypeId == 3)
            {
                Personals.Add(new CollateralLineResponse
                {
                    CategoryRefId = collateralTypeId,
                    TypeName = "Personal"
                });

                personalGrid.Reload();
            }
            else
            {
                Properties.Add(new CollateralLineResponse
                {
                    CategoryRefId = collateralTypeId,
                    TypeName = "Property"
                });
                propertiesGrid.Reload();
            }

        }
        private void Remove(CollateralLineResponse row, int type = 3)
        {
            if (type == 3)
            {
                Personals.Remove(row);
                personalGrid.Reload();
            }
            else
            {
                Properties.Remove(row);
                propertiesGrid.Reload();
            }

        }

        ////
        ///create method
        protected async Task FormValidate()
        {
            allSubTypes = await _collateralService.GetCollateralSubTypes();

            List<CollateralLineResponse> lines = new List<CollateralLineResponse>
            {

            }.Union(Properties.Where(x => x.SubCategoryRefId > 0))
            .Union(Personals.Where(x => x.SubCategoryRefId > 0))
            .ToList();

            lines!.ForEach(x =>
            {
                var subType = allSubTypes!.Where(a => a.Id == x.SubCategoryRefId).FirstOrDefault();

                if (subType is not null)
                {
                    x.SubTypeName = subType.CollateralSubTypeName;
                }
            });

            model.CollateralLines = lines.Adapt<List<CollateralLineDtoCreate>>();

            if (!CustomerId.HasValue)
            {
                NotificationService.Notify(Radzen.NotificationSeverity.Warning, "Empty Selected", "Please select customer first");
                return;
            }
            editForm.EditContext?.Validate();
            try
            {
                var response = await _collateralService.InsertOrUpdate(model);

                if (response.IsSuccess)
                {

                    NotificationService.Notify(Radzen.NotificationSeverity.Success, response.Data);
                    model = new();
                    await SaveChanged.InvokeAsync();
                }
                else
                {
                    NotificationService.Notify(Radzen.NotificationSeverity.Error, response.ErrMessage);
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(Radzen.NotificationSeverity.Error, ex.Message);

            }
        }

        #region codes


        //model!.CollateralLines!.Add(new CollateralLineDTO
        //{
        //    CategoryRefId = collateralTypeId,
        //    TypeName = collateralTypeId == 3 ? "Personal" : "Property"
        //});


        //model!.CollateralLines!.ForEach(x =>
        //{
        //    var subType = allSubTypes!.Where(a => a.Id == x.SubCategoryRefId).FirstOrDefault();
        //    if (subType is not null)
        //    {
        //        x.SubTypeName = subType.CollateralSubTypeName;
        //    }
        //});
        //protected async Task SelectCustomerChanged()
        //{
        //    var collateral = await _collateralService.GetCollateralById(CustomerId);
        //    if (collateral is not null)
        //    {
        //        CollateralId = CustomerId;
        //        model!.CustomerRefId = CustomerId;
        //        model.CustomerName = CustomerName;
        //        model.CollateralLines = collateral!.CollateralLines;
        //        model.Remark = collateral!.Remark;
        //        model.DocumentKeeping = collateral!.DocumentKeeping;
        //        model.RegisterDate = collateral!.RegisterDate;
        //    }
        //    else
        //    {
        //        CollateralId = 0;
        //        model!.CustomerName = CustomerName;
        //        model.CustomerRefId = CustomerId;
        //        model.Remark = "";
        //        model.DocumentKeeping = "";
        //        model.RegisterDate = DateTime.Now;
        //        model.CollateralLines!.Clear();
        //    }
        //    await InvokeAsync(StateHasChanged);
        //}
        //protected async Task SearchCustomer(string searchText = "")
        //{
        //    strText = searchText;
        //    custlist = await _customerService.GetCustomersAsync(25, includeisactive, false, searchText);
        //    cusName = custlist!.Select(x => new CustomerCollateral
        //    {
        //        CustomerId = x.CustomerId,
        //        Name = x.AltName,
        //    }).Take(50).ToList();
        //    StateHasChanged();
        //}
        //[Parameter]
        //public LoanCustomerQuery? SelectedCustomer { get; set; }
        //[Parameter]
        //public long CollateralId { get; set; } = 0;
        //[Inject] IToaster Toaster { get; set; } = default!;
        //[Inject] ICollateralNew _service { get; set; } = default!;
        //[Inject] ICustomerServiceImpl _cusService { get; set; } = default!;

        //protected IEnumerable<LoanCustomerQuery>? cusName = Enumerable.Empty<LoanCustomerQuery>();
        //protected EditForm editForm { get; set; } = default!;

        //protected CollateralDto model = new CollateralDto();

        //private IEnumerable<CollateralTypeDto> CollateralTypeDtos = [];
        //private IEnumerable<CollateralSubTypeDto> collateralProperties = [];
        //private IEnumerable<CollateralSubTypeDto> collateralPersonals = [];
        //protected CollateralTypeDto? SelectCollateralType;
        //private IEnumerable<CollateralSubTypeDto>? allSubTypes = [];
        //private void Close() => DialogService.Close(false);
        //[Parameter] public Func<bool, Task<bool>> onVildated { get; set; } = default!;
        //[Parameter] public Func<bool, Task> OnSuccess { get; set; } = default!;
        ////Onstartup
        //protected async override Task OnInitializedAsync()
        //{
        //    try
        //    {
        //        CollateralTypeDtos = await _service.GetCollateralType();
        //        collateralProperties = await _service.GetCollateralSubTypes(1);
        //        collateralPersonals = await _service.GetCollateralSubTypes(3);
        //        await GetCollateralLineByCustomer(SelectedCustomer!.CustomerId);
        //        allSubTypes = await _service.GetCollateralSubTypes();
        //        model.RegisterDate = DateTime.UtcNow;

        //    }
        //    catch (Exception ex)
        //    {
        //        Toaster.Warning(ex.Message, "Collateral", config =>
        //        {
        //            config.ShowProgressBar = false;
        //            config.ShowCloseIcon = true;
        //        });
        //    }
        //    await base.OnInitializedAsync();
        //}
        ////===================================

        ///// <summary>
        ///// customer control
        ///// </summary>
        //string? q = string.Empty;
        //protected string? strText = "";
        //bool includeisactive = false;

        //string selectedTab = "property";
        //private Task OnSelectedTabChanged(string name)
        //{
        //    selectedTab = name;
        //    return Task.CompletedTask;
        //}
        //private void AddNewRow(int collateralTypeId)
        //{
        //    model.CollateralLines!.Add(new CollateralLineDTO
        //    {
        //        CategoryRefId = collateralTypeId,
        //        TypeName = collateralTypeId == 3 ? "Personal" : "Property"
        //    });
        //}
        //protected async Task GetCollateralLineByCustomer(long customerId)
        //{
        //    var customerResult = await _service.GetCollateralById(customerId);
        //    if (customerResult is not null)
        //    {
        //        CollateralId = customerResult!.CustomerRefId!.Value;
        //        model.CustomerRefId = customerResult!.CustomerRefId;
        //        model.CustomerName = SelectedCustomer!.AltName;
        //        model.CustomerRefId = SelectedCustomer.CustomerId;
        //        model.Remark = customerResult!.Remark;
        //        model.DocumentKeeping = customerResult!.DocumentKeeping;
        //        model.RegisterDate = customerResult!.RegisterDate;
        //        model.CollateralLines = customerResult!.CollateralLines;
        //    }
        //    else
        //    {
        //        CollateralId = 0;
        //        model.CustomerName = SelectedCustomer!.AltName;
        //        model.CustomerRefId = SelectedCustomer.CustomerId;
        //        model.Remark = "";
        //        model.DocumentKeeping = "";
        //        model.RegisterDate = DateTime.Now;
        //    }
        //}
        //private void Remove(CollateralLineDTO row)
        //{
        //    model?.CollateralLines!.Remove(row);
        //}

        //////
        /////create method
        //protected async Task FormValidate()
        //{
        //    model.CollateralLines!.ForEach(x =>
        //    {
        //        var subType = allSubTypes!.Where(a => a.Id == x.SubCategoryRefId).FirstOrDefault();
        //        if (subType is not null)
        //        {
        //            x.SubTypeName = subType.CollateralSubTypeName;
        //        }
        //    });

        //    editForm.EditContext?.Validate();
        //    try
        //    {
        //        var data = await _service.CreateCollateral(CollateralId, model);
        //        if (data.IsSuccess)
        //        {
        //            NotificationService.Notify(Radzen.NotificationSeverity.Success, data.Message);
        //            DialogService.Close(true);
        //        }
        //        else
        //        {
        //            NotificationService.Notify(Radzen.NotificationSeverity.Error, data.Message);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        NotificationService.Notify(Radzen.NotificationSeverity.Error, ex.Message);
        //    }
        //}
        #endregion
    }
}