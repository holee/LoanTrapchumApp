using Applications.Extensions;
using Applications.Requests.Collaterals;
using Applications.Requests.Lists.Collaterals;
using Applications.Responses;
using Applications.Responses.Collaterals;
using Applications.Responses.ViewModels.Customers;
using Applications.Services.LIsts;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace TrapchumApp.Components.Pages.Lists.Collaterals
{
    public partial class CollateralCreate
    {

        readonly ICollateralsServiceImpl _cService;
        public CollateralCreate(ICollateralsServiceImpl collateralService)
        {
            _cService = collateralService;
        }

        [Parameter] public Guid? CustomerId { get; set; }
        [Parameter] public string? CustomerName { get; set; } = string.Empty;
        IEnumerable<VCustomerList> custlist = [];
        IEnumerable<CustomerCollateral> cusName = [];
        IEnumerable<CollateralTypeResponse> CollateralTypes = [];
        IEnumerable<CollateralSubTypeResponse> CollateralProperties = [];
        IEnumerable<CollateralSubTypeResponse> CollateralPersonals = [];
        IEnumerable<CollateralSubTypeResponse> allSubTypes = [];
        List<CollateralLineResponse> Properties = [];
        List<CollateralLineResponse> Personals = [];
        EditForm editForm { get; set; } = default!;
        CollateralTypeResponse? SelectCollateralType;
        CollateralDtoCreate model { get; set; } = new();

        Guid? CollateralId = Guid.Empty;
        protected override async Task OnInitializedAsync()
        {
            CollateralTypes = await _cService.GetCollateralTypes(0);
            CollateralProperties = await _cService.GetCollateralSubTypes(1);
            CollateralPersonals = await _cService.GetCollateralSubTypes(3);
            await GetCollateralLineByCustomer();
            allSubTypes = await _cService.GetCollateralSubTypes();
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
                CustomerName = customerResponse!.Data!.EntityNames.AltName;
            }

            await base.OnParametersSetAsync();
        }


        protected async Task GetCollateralLineByCustomer()
        {
            var customerResult = await _cService.GetCollateralByCustomerAsync(CustomerId);

            if (customerResult.IsSuccess)
            {
                var collateral = customerResult.Data!;
                CollateralId = CustomerId;
                model!.CollateralId = CustomerId;
                model.CustomerName = CustomerName;
                model.Remark = collateral!.Remark;
                model.DocumentKeeping = collateral!.DocumentKeeping;
                model.RegisterDate = collateral!.RegisterDate;
                //model.CollateralLines = collateral!.CollateralLines;
                Personals = collateral.CollateralLines
                              .Where(x => x.TypeName == "Personal")
                              .ToList();
                Properties = collateral.CollateralLines
                                .Where(x => x.TypeName == "Property")
                                .ToList();
            }
            else
            {
                CollateralId = Guid.Empty;
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
            allSubTypes = await _cService.GetCollateralSubTypes();

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

            if (CustomerId.IsNullOrEmpty())
            {
                NotificationService.Notify(Radzen.NotificationSeverity.Warning, "Empty Selected", "Please select customer first");
                return;
            }
            editForm.EditContext?.Validate();
            try
            {
                var response = await _cService.CreateAsync(model);
                if (response.IsSuccess)
                {

                    NotificationService.Notify(Radzen.NotificationSeverity.Success, response.Data);
                    model = new();
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
  
    }
}