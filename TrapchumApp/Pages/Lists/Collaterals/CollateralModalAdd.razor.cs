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
            DialogService.Close(true);
        }

        protected override async Task OnInitializedAsync()
        {

            CollateralTypes = await _collateralService.GetCollateralTypes(0);

            CollateralProperties = await _collateralService.GetCollateralSubTypes((int)CollateralTypeEnum.Property);

            CollateralPersonals = await _collateralService.GetCollateralSubTypes((int)CollateralTypeEnum.Personal);

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


        private Dictionary<long, VCollateralLines> _originalsProperties = new();
        private Dictionary<long, VCollateralLines> _originalsPersonals = new();

        async Task EditRow(VCollateralLines row, CollateralTypeEnum type)
        {

            DeepCopy(row, type);

            if (type == CollateralTypeEnum.Property && propertiesGrid.IsValid)
            {
                await propertiesGrid.EditRow(row);
            }
            if (type == CollateralTypeEnum.Personal && personalGrid.IsValid)
            {
                await personalGrid.EditRow(row);
            }
            StateHasChanged();
        }


        void Reset(VCollateralLines e, CollateralTypeEnum type)
        {
            switch (type)
            {
                case CollateralTypeEnum.Property:
                    _properties.Remove(e);
                    break;
                case CollateralTypeEnum.Personal:
                    _personals.Remove(e);
                    break;
                default:
                    break;
            }

        }
        async Task InsertNewRow(CollateralTypeEnum type)
        {

            if (type == CollateralTypeEnum.Property)
            {

                if (!propertiesGrid.IsValid) return;

                var prop = new VCollateralLines();

                prop.CategoryRefId = (int)CollateralTypeEnum.Property;

                prop.TypeName = CollateralTypeEnum.Property.ToString();

                await propertiesGrid.InsertRow(prop);
            }
            else
            {
                if (!personalGrid.IsValid) return;

                var prop = new VCollateralLines();

                prop.CategoryRefId = (int)CollateralTypeEnum.Personal;

                prop.TypeName = CollateralTypeEnum.Property.ToString();

                await personalGrid.InsertRow(prop);
            }
        }


        async Task DeleteRow(VCollateralLines e, CollateralTypeEnum type)
        {

            var confirm = await DialogService.Confirm("Are you sure?", "Delete");


            if (confirm == false )
            {
                CancelEdit(e, type);    

                return;
            }

            Reset(e, type);


            if (type == CollateralTypeEnum.Property)
            {

                if (_properties.Contains(e))
                {
                    //dbContext.Remove<Order>(order);

                    //dbContext.SaveChanges();

                    //await ordersGrid.Reload();

                    propertiesGrid.CancelEditRow(e);

                    await propertiesGrid.Reload();
                }
                else
                {
                    propertiesGrid.CancelEditRow(e);

                    await propertiesGrid.Reload();
                }


            }
            else
            {
                if (_properties.Contains(e))
                {
                    //dbContext.Remove<Order>(order);

                    //dbContext.SaveChanges();

                    //await ordersGrid.Reload();

                    propertiesGrid.CancelEditRow(e);

                    await propertiesGrid.Reload();
                }
                else
                {
                    propertiesGrid.CancelEditRow(e);
                    await propertiesGrid.Reload();
                }

            }

        }


        async Task SaveRow(VCollateralLines e, CollateralTypeEnum type)
        {
            switch (type)
            {
                case CollateralTypeEnum.Property:
                    await propertiesGrid.UpdateRow(e);
                    break;
                case CollateralTypeEnum.Personal:
                    await personalGrid.UpdateRow(e);
                    break;
                default:
                    break;
            }

            StateHasChanged();
        }

        async Task OnRowUpdate(VCollateralLines arg, CollateralTypeEnum type)
        {
            Reset(arg, type);

            var prop = arg.MapCollateralLineCreate();

            model.CollateralLines.Add(prop);

            var rsp = await _collateralService.CreateAsync(model, false);

            if (rsp.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Success, rsp.Data);
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, rsp.Data);
            }
        }



        void CancelEdit(VCollateralLines row, CollateralTypeEnum type)
        {

            Reset(row, type);

            ReadCopy(ref row, type);

            switch (type)
            {
                case CollateralTypeEnum.Property:
                    propertiesGrid.CancelEditRow(row);
                    break;
                case CollateralTypeEnum.Personal:
                    personalGrid.CancelEditRow(row);
                    break;
                default:
                    break;
            }


        }


        async Task OnRowCreate(VCollateralLines arg, CollateralTypeEnum type)
        {
            var prop = arg.MapCollateralLineCreate();

            model.CollateralLines.Add(prop);

            var rsp = await _collateralService.CreateAsync(model, false);

            if (rsp.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Success, rsp.Data);
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, rsp.Data);
            }

            if (type == CollateralTypeEnum.Property)
            {
                _personalsToInsert.Remove(arg);
            }
            else
            {
                _personalsToInsert.Remove(arg);
            }



        }



        private void ReadCopy(ref VCollateralLines row, CollateralTypeEnum type)
        {

            if (type == CollateralTypeEnum.Property && _originalsProperties.TryGetValue(row.Id, out var original))
            {
                row.Id = original.Id;
                row.CollateralRefId = original.CollateralRefId;
                row.CategoryRefId = original.CategoryRefId;
                row.CollateralNo = original.CollateralNo;
                row.TypeName = original.TypeName;
                row.CollateralSubTypeName = original.CollateralSubTypeName;
                row.SubCategoryRefId = original.SubCategoryRefId;
                row.OwnerName = original.OwnerName;
                row.Size = original.Size;
                row.Quantity = original.Quantity;
                row.MarketPrice = original.MarketPrice;
                row.HotPrice = original.HotPrice;
                row.PawnPrice = original.PawnPrice;
                row.TypeName = original.TypeName;
                row.ReferenceNo = original.ReferenceNo;
                row.WareHouse = original.WareHouse;
            }

            if (type == CollateralTypeEnum.Personal && _originalsPersonals.TryGetValue(row.Id, out var origin))
            {
                row.Id = origin.Id;
                row.CollateralRefId = origin.CollateralRefId;
                row.CollateralNo = origin.CollateralNo;
                row.CategoryRefId = origin.CategoryRefId;
                row.TypeName = origin.TypeName;
                row.SubCategoryRefId = origin.SubCategoryRefId;
                row.CollateralSubTypeName = origin.CollateralSubTypeName;
                row.OwnerName = origin.OwnerName;
                row.Size = origin.Size;
                row.Quantity = origin.Quantity;
                row.MarketPrice = origin.MarketPrice;
                row.HotPrice = origin.HotPrice;
                row.PawnPrice = origin.PawnPrice;
                row.TypeName = origin.TypeName;
                row.ReferenceNo = origin.ReferenceNo;
                row.WareHouse = origin.WareHouse;
            }


        }




        private void DeepCopy(VCollateralLines row, CollateralTypeEnum type)
        {
            if (type == CollateralTypeEnum.Property)
            {
                _originalsProperties[row.Id] = new VCollateralLines
                {
                    Id = row.Id,
                    CollateralRefId = row.CollateralRefId,
                    CollateralNo = row.CollateralNo,
                    SubCategoryRefId = row.SubCategoryRefId,
                    CategoryRefId = row.CategoryRefId,
                    CollateralSubTypeName = row.CollateralSubTypeName,
                    OwnerName = row.OwnerName,
                    Size = row.Size,
                    Quantity = row.Quantity,
                    MarketPrice = row.MarketPrice,
                    HotPrice = row.HotPrice,
                    PawnPrice = row.PawnPrice,
                    ReferenceNo = row.ReferenceNo,
                    Location = row.Location,
                    TypeName = row.TypeName,
                    WareHouse = row.WareHouse
                };
            }
            if (type == CollateralTypeEnum.Personal)
            {
                _originalsPersonals[row.Id] = new VCollateralLines
                {
                    Id = row.Id,
                    CollateralRefId = row.CollateralRefId,
                    CollateralNo = row.CollateralNo,
                    SubCategoryRefId = row.SubCategoryRefId,
                    CategoryRefId = row.CategoryRefId,
                    CollateralSubTypeName = row.CollateralSubTypeName,
                    TypeName = row.TypeName,
                    OwnerName = row.OwnerName,
                    Size = row.Size,
                    Quantity = row.Quantity,
                    MarketPrice = row.MarketPrice,
                    HotPrice = row.HotPrice,
                    PawnPrice = row.PawnPrice,
                    ReferenceNo = row.ReferenceNo,
                    Location = row.Location,
                    WareHouse = row.WareHouse
                };
            }

        }


    }





}


