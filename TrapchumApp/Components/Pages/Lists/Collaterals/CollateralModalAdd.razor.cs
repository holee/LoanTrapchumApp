using Applications.Extensions;
using Applications.Requests.Lists.Collaterals;
using Applications.Responses;
using Applications.Responses.Collaterals;
using Applications.Responses.ViewModels.Customers;
using Applications.Services.LIsts;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;
using Radzen.Blazor;

namespace TrapchumApp.Components.Pages.Lists.Collaterals
{
    public partial class CollateralModalAdd
    {

        private readonly ICollateralsServiceImpl _collateralService;
        [Parameter] public Guid? CustomerId { get; set; } = Guid.Empty;
        [Parameter] public string? CustomerName { get; set; } = string.Empty;
        IEnumerable<CollateralTypeResponse> CollateralTypes = [];
        IEnumerable<CollateralSubTypeResponse> CollateralProperties = [];
        IEnumerable<CollateralSubTypeResponse> CollateralPersonals = [];
        IEnumerable<CollateralSubTypeResponse> allSubTypes = [];
        List<CollateralLineResponse> Properties = [];
        List<CollateralLineResponse> Personals = [];
        EditForm editForm { get; set; } = default!;
        CollateralTypeResponse? SelectCollateralType;
        CollateralDtoCreate model { get; set; } = new();
        VCustomerList Customer = new();
        Guid? CollateralId = Guid.Empty;
        RadzenDataGrid<CollateralLineResponse> personalGrid = default!;
        RadzenDataGrid<CollateralLineResponse> propertiesGrid = default!;

        public CollateralModalAdd(ICollateralsServiceImpl collateralService)
        {
            _collateralService = collateralService;
        }

        void CloseDialogue()
        {
            DialogService.Close(false);
        }

        protected override async Task OnInitializedAsync()
        {

            var response = await _customerService.GetCustomerDetailsByIdAsync(CustomerId,false);

            if (response.IsSuccess)
            {
                Customer = response.Data!;
                CustomerName = response.Data!.AltName;
            }
            else
            {

                NotificationService.Notify(NotificationSeverity.Error, response.ErrMessage);

            }

            CollateralTypes = await _collateralService.GetCollateralTypes(0);

            CollateralProperties = await _collateralService.GetCollateralSubTypes(1);

            CollateralPersonals = await _collateralService.GetCollateralSubTypes(3);

            await GetCollateralLineByCustomer();

            allSubTypes = await _collateralService.GetCollateralSubTypes();

            await base.OnInitializedAsync();
        }

        protected async Task GetCollateralLineByCustomer()
        {
            if (CustomerId.IsNullOrEmpty())
            {
                return;
            }
            var customerResult = await _collateralService.GetCollateralByCustomerAsync(CustomerId);

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
                              .Where(x => x.CategoryRefId == 3)
                              .ToList();
                Properties = collateral.CollateralLines
                                .Where(x => x.CategoryRefId == 1)
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

        ////
        ///create method
        protected async Task FormValidate()
        {
            allSubTypes = await _collateralService.GetCollateralSubTypes();

            List<CollateralLineDtoCreate> lines = [];

            var insertedProperties = Properties
                                    .Where(x => x.SubCategoryRefId > 0)
                                    .Select(x => x.Adapt<CollateralLineDtoCreate>())
                                    .ToList();

            var insertedPersanals = Personals
                                    .Where(x => x.SubCategoryRefId > 0)
                                    .Select(x => x.Adapt<CollateralLineDtoCreate>())
                                    .ToList();

            lines = lines.Union(insertedProperties).Union(insertedPersanals).ToList();

            lines!.ForEach(x =>
            {
                var subType = allSubTypes!.Where(a => a.Id == x.SubCategoryRefId).FirstOrDefault();

                if (subType is not null)
                {
                    x.SubTypeName = subType.CollateralSubTypeName;
                }
            });

            model.CollateralLines = lines;
            model.CollateralId = CustomerId;

            if (CustomerId.IsNullOrEmpty())
            {
                NotificationService.Notify(NotificationSeverity.Info, "Please select customer first");
                return;
            }
            editForm.EditContext?.Validate();
            try
            {
                var response = await _collateralService.CreateAsync(model);

                if (response.IsSuccess)
                {

                    NotificationService.Notify(NotificationSeverity.Success, response.Data);
                    DialogService.Close(true);
                    model = new();
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, response.Data);
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.Message);

            }
        }


        void ResetPerson()
        {
            Personals.Clear();
        }

        void ResetPerson(CollateralLineResponse e)
        {
            if (!Personals.Contains(e))
            {
                return;
            }
            Personals.Remove(e);
        }
        async Task InsertPersonRow()
        {
            var e = new CollateralLineResponse();
            e.CategoryRefId = 3;
            e.TypeName = "Personal";
            await personalGrid.InsertRow(e);
        }
        void CancelPersonEdit(CollateralLineResponse e)
        {
            ResetPerson(e);
            personalGrid.CancelEditRow(e);
        }

        async Task DeletePersonRow(CollateralLineResponse e)
        {
            ResetPerson(e);
            personalGrid.CancelEditRow(e);
            await personalGrid.Reload();

        }

        async Task SavePersonRow(CollateralLineResponse e)
        {
            Personals.Add(e);

            await personalGrid.UpdateRow(e);
        }


        async Task EditRow(CollateralLineResponse e, int type = 1)
        {
            if (type == 1)
            {
                await propertiesGrid.EditRow(e);
            }
            else
            {
                await personalGrid.EditRow(e);
            }
        }

        void ResetProperty()
        {
            Properties.Clear();
        }

        void ResetProperty(CollateralLineResponse e)
        {
            if (!Properties.Contains(e))
            {
                return;
            }
            Properties.Remove(e);
        }

        async Task InsertPropertyRow()
        {
            var e = new CollateralLineResponse();
            e.CategoryRefId = 1;
            e.TypeName = "Propery";
            await propertiesGrid.InsertRow(e);
        }

        void CancelPropertyEdit(CollateralLineResponse e)
        {
            ResetPerson(e);

            propertiesGrid.CancelEditRow(e);
        }

        async Task DeletePropertyRow(CollateralLineResponse e)
        {
            ResetProperty(e);
            propertiesGrid.CancelEditRow(e);
            await propertiesGrid.Reload();

        }

        async Task SavePropertyRow(CollateralLineResponse e)
        {
            Properties.Add(e);
            await propertiesGrid.UpdateRow(e);
        }

    }





}

