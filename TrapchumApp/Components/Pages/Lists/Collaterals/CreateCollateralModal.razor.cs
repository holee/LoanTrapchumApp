using Applications.Extensions.mappers;
using Applications.Requests.Lists.Collaterals;
using Applications.Responses.Lists.Collaterals;
using Applications.Responses.ViewModels.Customers;
using Applications.Services.LIsts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;

namespace TrapchumApp.Components.Pages.Lists.Collaterals
{
    public partial class CreateCollateralModal
    {
        private readonly ICollateralsServiceImpl _service;
        private readonly ILogger<CreateCollateralModal> _logger;

        private IEnumerable<VCustomerList> custlist = [];
        protected IEnumerable<CustomerCollateral>? cusName = [];
        private IEnumerable<CollateralTypeResponse> collateralTypes = [];
        private IEnumerable<CollateralSubTypeResponse> collateralProperties = [];
        private IEnumerable<CollateralSubTypeResponse> collateralPersonals = [];
        private IEnumerable<CollateralSubTypeResponse> allSubTypes = [];
        protected EditForm editForm { get; set; } = default!;
        protected CollateralTypeResponse? SelectCollateralType;
        protected CustomerCollateral? SelectedCustomer = new();
        protected CollateralDtoCreate model = new();
        /// <summary>
        /// end model include
        /// </summary>
        /// <returns></returns>
        private void Close() => DialogService.Close(false);
        [Parameter] public string? CollateralId { get; set; } = string.Empty;
        //model varr
        //Onstartup
        protected async override Task OnInitializedAsync()
        {
            try
            {
                var response = await _customerService.GetCustomersAsync(null, 25, includeisactive, false, q);
                if (response.IsSuccess)
                {
                    custlist = response.Data!;
                    cusName = custlist.Select(x => new CustomerCollateral
                    {
                        CustomerId = x.CustomerId,
                        Name = x.AltName,
                    });
                }
                collateralTypes = await _service.GetCollateralTypes();
                collateralProperties = await _service.GetCollateralSubTypes(1);
                collateralPersonals = await _service.GetCollateralSubTypes(3);
                model.RegisterDate = DateTime.UtcNow;
                allSubTypes = await _service.GetCollateralSubTypes();

            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.Message);
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
                strText = e?.Value?.ToString();
            }
        }
        bool includeisactive = false;
        protected async Task<IEnumerable<CustomerCollateral?>> SearchCustomer(string searchText = "")
        {
            var response = await _customerService.GetCustomersAsync(null, 25, includeisactive, false, searchText);
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
            var rsp = await _service.GetCollateralByIdAsync(SelectedCustomer!.CustomerId);
            if (rsp.IsSuccess)
            {
                CollateralId = rsp.Data!.CollateralId;
                model.CollateralId = rsp.Data!.CollateralId;
                model.CustomerName = SelectedCustomer.Name;
                model.CollateralId = SelectedCustomer.CustomerId;
                model.Remark = rsp.Data!.Remark;
                model.DocumentKeeping = rsp.Data!.DocumentKeeping;
                model.RegisterDate = rsp.Data!.RegisterDate;
                model.CollateralLines = rsp.Data!.CollateralLines.MapList<CollateralLineDtoCreate>();
            }
            else
            {
                CollateralId = string.Empty;
                model.CustomerName = SelectedCustomer.Name;
                model.CollateralId = SelectedCustomer.CustomerId;
                model.Remark = "";
                model.DocumentKeeping = "";
                model.RegisterDate = DateTime.Now;
                model.CollateralLines!.Clear();
            }
        }

        string selectedTab = "property";

        public CreateCollateralModal(ICollateralsServiceImpl service, ILogger<CreateCollateralModal> logger)
        {
            _service = service;
            _logger = logger;
        }

        private Task OnSelectedTabChanged(string name)
        {
            selectedTab = name;
            return Task.CompletedTask;
        }

        private void AddNewRow(int collateralTypeId)
        {
            model.CollateralLines!.Add(new CollateralLineDtoCreate
            {
                SubCategoryRefId = collateralTypeId,
                TypeName = collateralTypeId == 3 ? "Personal" : "Property"
            });
        }
        private void Remove(CollateralLineDtoCreate row)
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
                var rsp = await _service.CreateAsync(model);

                if (rsp.IsSuccess)
                {
                    NotificationService.Notify(NotificationSeverity.Success, rsp.Data);
                    Close();
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, rsp.ErrMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
        }
    }
}