using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using TCLMS.Shared.Entities.Dtos.Collaterals;
using TCLMS.Shared.Entities.Models.Lists.Customers;

namespace TCLMS.Front.Pages.Lists.Collaterals
{
    public partial class CollateralCreateTab 
    {
        [Parameter] public long? CustomerId { get; set; } = 0;
        [Parameter] public string? CustomerName { get; set; } = string.Empty;
        IEnumerable<VCustomerList>? custlist = new List<VCustomerList>();
        IEnumerable<CustomerCollateral> cusName = [];
        IEnumerable<CollateralTypeDto> CollateralTypes = [];
        IEnumerable<CollateralSubTypeDto> CollateralProperties = [];
        IEnumerable<CollateralSubTypeDto> CollateralPersonals = [];
        IEnumerable<CollateralSubTypeDto> allSubTypes = [];
        List<CollateralLineDto> Properties = [];
        List<CollateralLineDto> Personals = [];
        EditForm editForm { get; set; } = default!;
        CollateralTypeDto? SelectCollateralType;
        CollateralDto model { get; set; } = new();

        long CollateralId = 0;
        protected override async Task OnInitializedAsync()
        {
            CollateralTypes = await _collateralService.GetCollateralType(0);
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
            var customerResponse = await _customerService.GetCustomerAsync(CustomerId);

            if (customerResponse.IsSuccess)
            {
                CustomerName = customerResponse!.Data!.AltName;
            }

            await base.OnParametersSetAsync();
        }


        protected async Task GetCollateralLineByCustomer()
        {
            if(CustomerId == 0)
            {
                return;
            }
            var customerResult = await _collateralService.GetCustomerCollateralById(CustomerId);

            if (customerResult.IsSuccess)
            {
                var collateral = customerResult.Data!;
                CollateralId = CustomerId!.Value;
                model!.CollateralId = CustomerId;
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
                model.CollateralId = CustomerId;
                model.Remark = "";
                model.DocumentKeeping = "";
                model.RegisterDate = DateTime.Now;
            }
        }

        private void AddNewRow(int collateralTypeId)
        {
            if (collateralTypeId == 3)
            {
                Personals.Add(new CollateralLineDto
                {
                    CategoryRefId = collateralTypeId,
                    TypeName = "Personal"
                });

                personalGrid.Reload();
            }
            else
            {
                Properties.Add(new CollateralLineDto
                {
                    CategoryRefId = collateralTypeId,
                    TypeName = "Property"
                });
                propertiesGrid.Reload();
            }

        }
        private void Remove(CollateralLineDto row, int type = 3)
        {
            if ((type == 3))
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

            List<CollateralLineDto> lines = new List<CollateralLineDto>
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

            model.CollateralLines = lines;

            if (CustomerId == 0)
            {
                NotificationService.Notify(Radzen.NotificationSeverity.Warning, "Empty Selected", "Please select customer first");
                return;
            }
            editForm.EditContext?.Validate();
            try
            {
                var response = await _collateralService.CreateCollateral(CollateralId, model);
                if (response.IsSuccess)
                {

                    NotificationService.Notify(Radzen.NotificationSeverity.Success, response.Data!.Message);
                    model = new();
                    await SaveChanged.InvokeAsync();
                }
                else
                {
                    NotificationService.Notify(Radzen.NotificationSeverity.Error, response.Data!.Message);
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