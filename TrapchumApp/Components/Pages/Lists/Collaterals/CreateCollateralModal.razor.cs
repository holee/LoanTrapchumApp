using AppilcationServices.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using TCLMS.Shared.Entities.Dtos.Collaterals;
using TCLMS.Shared.Entities.Models.Lists.Customers;

namespace TCLMS.Front.Pages.Lists.Collaterals
{
    public partial class CreateCollateralModal
    {
        [Inject] ICollateralServiceImpl _service { get; set; } = default!;
        private IEnumerable<VCustomerList> custlist = [];
        protected IEnumerable<CustomerCollateral>? cusName = [];
        private IEnumerable<CollateralTypeDto> collateralTypes = [];
        private IEnumerable<CollateralSubTypeDto> collateralProperties = [];
        private IEnumerable<CollateralSubTypeDto> collateralPersonals = [];
        private IEnumerable<CollateralSubTypeDto>? allSubTypes = [];
        protected EditForm editForm { get; set; } = default!;
        protected CollateralTypeDto? SelectCollateralType;
        protected CustomerCollateral? SelectedCustomer = new();
        protected CollateralDto model = new();
        /// <summary>
        /// end model include
        /// </summary>
        /// <returns></returns>
        private void Close() => DialogService.Close(false);
        [Parameter] public Func<bool, Task<bool>> onVildated { get; set; } = default!;
        [Parameter] public Func<bool, Task> OnSuccess { get; set; } = default!;
        [Parameter] public long CollateralId { get; set; } = 0;
        //model varr
        //Onstartup
        protected async override Task OnInitializedAsync()
        {
            try
            {
                var response = await _customerService.GetCustomersAsync(0,25, includeisactive, false, q);
                if (response.IsSuccess)
                {
                    custlist = response.Data!;
                    cusName = custlist.Select(x => new CustomerCollateral
                    {
                        CustomerId = x.CustomerId,
                        Name = x.AltName,
                    });
                }
                collateralTypes = await _service.GetCollateralType();
                collateralProperties = await _service.GetCollateralSubTypes(1);
                collateralPersonals = await _service.GetCollateralSubTypes(3);
                model.RegisterDate = DateTime.UtcNow;
                allSubTypes = await _service.GetCollateralSubTypes();

            }
            catch (Exception ex)
            {
                NotificationService.Notify(Radzen.NotificationSeverity.Error, ex.Message);
            }
            await base.OnInitializedAsync();
        }
        //===================================

        /// <summary>
        /// customer control
        /// </summary>
        string? q = string.Empty;
        protected string? strText = "";
        protected void SearchCustomer(ChangeEventArgs e)
        {
            if (e.Value is not null)
            {
                this.strText = e?.Value?.ToString();
            }
        }
        bool includeisactive = false;
        protected async Task<IEnumerable<CustomerCollateral?>> SearchCustomer(string searchText = "")
        {
            var response = await _customerService.GetCustomersAsync(0,25, includeisactive, false, searchText);
            if (response.IsSuccess)
            {
                custlist = response.Data!;
                cusName = custlist!.Select(x => new CustomerCollateral
                {
                    CustomerId = x.CustomerId,
                    Name = x.AltName,
                }).Take(50).ToList();
            }
            return await Task.FromResult(cusName!);
        }
        protected async Task SelectCustomerChanged()
        {
            var collateral = await _service.GetCollateralById(SelectedCustomer!.CustomerId);
            if (collateral is not null)
            {
                CollateralId = collateral!.CollateralId!.Value;
                model.CollateralId = collateral!.CollateralId;
                model.CustomerName = SelectedCustomer.Name;
                model.CollateralId = SelectedCustomer.CustomerId;
                model.Remark = collateral!.Remark;
                model.DocumentKeeping = collateral!.DocumentKeeping;
                model.RegisterDate = collateral!.RegisterDate;
                model.CollateralLines = collateral!.CollateralLines;
            }
            else
            {
                CollateralId = 0;
                model.CustomerName = SelectedCustomer.Name;
                model.CollateralId = SelectedCustomer.CustomerId;
                model.Remark = "";
                model.DocumentKeeping = "";
                model.RegisterDate = DateTime.Now;
                model.CollateralLines!.Clear();
            }
        }

        string selectedTab = "property";
        private Task OnSelectedTabChanged(string name)
        {
            selectedTab = name;
            return Task.CompletedTask;
        }

        private void AddNewRow(int collateralTypeId)
        {
            model.CollateralLines!.Add(new CollateralLineDto
            {
                CategoryRefId = collateralTypeId,
                TypeName = collateralTypeId == 3 ? "Personal" : "Property"
            });
        }
        private void Remove(CollateralLineDto row)
        {
            model?.CollateralLines!.Remove(row);
        }

        ////
        ///create method
        protected async Task FormValidate()
        {

            model.CollateralLines!.ForEach(x =>
            {
                var subType = allSubTypes!.Where(a => a.Id == x.SubCategoryRefId).FirstOrDefault();
                if (subType is not null)
                {
                    x.SubTypeName = subType.CollateralSubTypeName;
                }
            });

            editForm.EditContext?.Validate();
            try
            {
                var data = await _service.CreateCollateral(CollateralId, model);
                if (data.IsSuccess)
                {
                    NotificationService.Notify(Radzen.NotificationSeverity.Success, data.Data!.Message);
                    Close();
                    await OnSuccess(true);
                    model = new();
                }
                else
                {
                    NotificationService.Notify(Radzen.NotificationSeverity.Error, data.Data!.Message);
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(Radzen.NotificationSeverity.Error, ex.Message);
            }
        }
    }
}