using Applications.Extensions;
using Applications.Requests.Lists.Collaterals;
using Applications.Responses.Extensions;
using Applications.Responses.Lists.Collaterals;
using Applications.Responses.ViewModels.Customers;
using Applications.Services.LIsts;
using Domains.Models.Collaterals;
using Mapster;
using Microsoft.AspNetCore.Components;
using Radzen;
using Microsoft.AspNetCore.Components.Forms;
using Radzen.Blazor;

namespace TrapchumApp.Pages.Lists.Collaterals
{
    public partial class CollateralModalAdd
    {

        private readonly ICollateralsServiceImpl _collateralService;
        [Parameter] public string? CustomerId { get; set; } = string.Empty;
        [Parameter] public string? CustomerName { get; set; } = string.Empty;

        IEnumerable<CollateralTypeResponse> CollateralTypes = [];

        IEnumerable<CollateralSubTypeResponse> CollateralProperties = [];

        IEnumerable<CollateralSubTypeResponse> CollateralPersonals = [];

        IEnumerable<CollateralSubTypeResponse> allSubTypes = [];

        List<VCollateralLines> Properties = [];

        List<VCollateralLines> Personals = [];
        EditForm editForm { get; set; } = default!;

        CollateralTypeResponse? SelectCollateralType;

        CollateralDtoCreate model { get; set; } = new();

        VCustomerList Customer = new();

        string? CollateralId = string.Empty;

        RadzenDataGrid<VCollateralLines> personalGrid = default!;

        RadzenDataGrid<VCollateralLines> propertiesGrid = default!;
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

            CollateralTypes = await _collateralService.GetCollateralTypes(0);

            CollateralProperties = await _collateralService.GetCollateralSubTypes(1);

            CollateralPersonals = await _collateralService.GetCollateralSubTypes(3);

            await GetCustomerCollaterals();

            allSubTypes = await _collateralService.GetCollateralSubTypes();

            await base.OnInitializedAsync();
        }

        protected async Task GetCustomerCollaterals()
        {
            if (CustomerId.IsNullOrEmpty())
            {
                return;
            }
            var customerResult = await _collateralService.GetCustomerCollateralsAsync(CustomerId, true);

            if (customerResult.IsSuccess)
            {
                var collateral = customerResult.Data!;

                CollateralId = CustomerId;

                model = collateral.ToCollateralDtoCreate();

                Personals = collateral.ToPersonals();

                Properties = collateral.ToProperties();
            }
            else
            {
                CollateralId = string.Empty;

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

            model.CollateralLines?.AddRange(lines);

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

        void ResetPerson(VCollateralLines e)
        {
            if (!Personals.Contains(e))
            {
                return;
            }
            Personals.Remove(e);
        }
        async Task InsertPersonRow()
        {
            var e = new VCollateralLines();

            e.CategoryRefId = 3;

            e.TypeName = CollateralTypeEnum.Personal.ToString();

            await personalGrid.InsertRow(e);
        }
        void CancelPersonEdit(VCollateralLines e)
        {
            ResetPerson(e);

            personalGrid.CancelEditRow(e);
        }

        async Task DeletePersonRow(VCollateralLines e)
        {
            ResetPerson(e);

            personalGrid.CancelEditRow(e);

            await personalGrid.Reload();

        }

        async Task SavePersonRow(VCollateralLines e)
        {
            Personals.Add(e);

            await personalGrid.UpdateRow(e);
        }


        async Task EditRow(VCollateralLines e, int type = 1)
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

        void ResetProperty(VCollateralLines e)
        {
            if (!Properties.Contains(e))
            {
                return;
            }
            Properties.Remove(e);
        }

        async Task InsertPropertyRow()
        {
            var e = new VCollateralLines();

            e.CategoryRefId = 1;

            e.TypeName = CollateralTypeEnum.Property.ToString();

            await propertiesGrid.InsertRow(e);
        }

        void CancelPropertyEdit(VCollateralLines e)
        {
            ResetPerson(e);

            propertiesGrid.CancelEditRow(e);
        }

        async Task DeletePropertyRow(VCollateralLines e)
        {
            ResetProperty(e);

            propertiesGrid.CancelEditRow(e);

            await propertiesGrid.Reload();

        }

        async Task SavePropertyRow(VCollateralLines e)
        {
            Properties.Add(e);

            await propertiesGrid.UpdateRow(e);
        }

    }





}

