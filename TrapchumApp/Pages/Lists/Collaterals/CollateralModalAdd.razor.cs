using Applications.Commons.Mappers;
using Applications.Extensions;
using Applications.Requests.Lists.Collaterals;
using Applications.Responses.Extensions;
using Applications.Responses.Lists.Collaterals;
using Applications.Responses.ViewModels.Customers;
using Applications.Services.LIsts;
using Domains.Models.Collaterals;
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
        EditForm editForm { get; set; } = default!;

        CollateralTypeResponse? SelectCollateralType;

        CollateralDtoCreate model { get; set; } = new();

        VCustomerList Customer = new();

        string? CollateralId = string.Empty;

        RadzenDataGrid<VCollateralLines> personalGrid = default!;
        List<VCollateralLines> _personalsToUpdate = [];
        List<VCollateralLines> _personals = [];
        List<VCollateralLines> _personalsToInsert = [];
        RadzenDataGrid<VCollateralLines> propertiesGrid = default!;
        List<VCollateralLines> _propertiesToUpdate = [];
        List<VCollateralLines> _propertiesToInert = [];
        List<VCollateralLines> _properties = [];
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

                _personals = collateral.ToPersonals();

                _properties = collateral.ToProperties();
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

            var insertedProperties = _properties
                                    .Where(x => x.SubCategoryRefId > 0)
                                    .Select(x => x.MapCollateralLineCreate())
                                    .ToList();

            var insertedPersanals = _personals
                                    .Where(x => x.SubCategoryRefId > 0)
                                    .Select(x => x.MapCollateralLineCreate())
                                    .ToList();

            lines.AddRange(insertedProperties);
            lines.AddRange(insertedPersanals);

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

                    model = new();

                    DialogService.Close(true);


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



        /// <summary>
        /// Personal Prop
        /// </summary>

        void ResetPerson(VCollateralLines e)
        {
            _personals.Remove(e);
        }
        async Task InsertPersonRow()
        {
            var personal = new VCollateralLines();

            personal.CategoryRefId = (int)CollateralTypeEnum.Personal;

            personal.TypeName = CollateralTypeEnum.Personal.ToString();

            await personalGrid.InsertRow(personal);
        }
        void CancelPersonEdit(VCollateralLines e)
        {
            ResetPerson(e);
            personalGrid.CancelEditRow(e);
        }

        async Task EditRowPerson(VCollateralLines arg)
        {
            if (!propertiesGrid.IsValid) return;

            await personalGrid.EditRow(arg);
        }

        async Task SavePersonRow(VCollateralLines e)
        {
            await personalGrid.UpdateRow(e);
        }


        async Task EditRow(VCollateralLines e, int type = 1)
        {
            if (type == (int)CollateralTypeEnum.Property)
            {
                await EditRowProperty(e);
            }
            if (type == (int)CollateralTypeEnum.Personal)
            {
                await EditRowPerson(e);
            }
        }

        void PersonalRowUpdate(VCollateralLines arg)
        {
            ResetPerson(arg);
            _personals.Add(arg);
        }

        void PersonalRowCreate(VCollateralLines arg)
        {
            //ResetPerson(arg);
            _personals.Add(arg);
        }

        /// <summary>
        /// Properties
        /// </summary>
        void ResetProperty(VCollateralLines e)
        {
            _properties.Remove(e);
        }

        async Task InsertPropertyRow()
        {
            if (!propertiesGrid.IsValid) return;

            var prop = new VCollateralLines();

            prop.CategoryRefId = (int)CollateralTypeEnum.Property;

            prop.TypeName = CollateralTypeEnum.Property.ToString();

            await propertiesGrid.InsertRow(prop);
        }


        async Task EditRowProperty(VCollateralLines arg)
        {
            if (!propertiesGrid.IsValid) return;
            await propertiesGrid.EditRow(arg);
        }

        async Task SavePropertyRow(VCollateralLines e)
        {
            await propertiesGrid.UpdateRow(e);
        }

        void CancelPropertyEdit(VCollateralLines e)
        {
            ResetProperty(e);

            propertiesGrid.CancelEditRow(e);

        }

        async Task DeletePropertyRow(VCollateralLines e)
        {
            ResetProperty(e);
            propertiesGrid.CancelEditRow(e);
            await propertiesGrid.Reload();
        }

        void PropertyRowUpdate(VCollateralLines arg)
        {
            ResetProperty(arg);

            _properties.Add(arg);
            //dbContext.Update(order);

            //dbContext.SaveChanges();

            //var prop = arg.MapCollateralLineCreate();

            //Model.CollateralLines.Add(prop);

            //var rsp = await _collateralService.UpsertAsync(Model);

            //if (rsp.IsSuccess)
            //{
            //    NotificationService.Notify(NotificationSeverity.Success,rsp.Data);
            //}
            //else
            //{
            //    NotificationService.Notify(NotificationSeverity.Error, rsp.Data);
            //}

        }

        void PropertyRowCreate(VCollateralLines arg)
        {
            //var prop = arg.MapCollateralLineCreate();

            //Model.CollateralLines.Add(prop);

            //var rsp = await _collateralService.UpsertAsync(Model);

            //if (rsp.IsSuccess)
            //{
            //    NotificationService.Notify(NotificationSeverity.Success, rsp.Data);
            //}
            //else
            //{
            //    NotificationService.Notify(NotificationSeverity.Error, rsp.Data);
            //}

            //_personalsToInsert.Remove(arg);

            _properties.Add(arg);

        }
    }





}

