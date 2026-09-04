using Applications.Extensions;
using Domains.Models.Collaterals;
using Radzen.Blazor;
using Applications.Services.Lists;
using Web.UI.Helpers.Validations;
using Applications.Features.Customers.Response;
using Applications.Features.Collaterals.Dtos;
using Applications.Commons.Mappers.Collaterals;
using Applications.Features.Collaterals.Queries.GetCollateralDetails;
using Applications.Features.Collaterals.Commands.AddOrUpdateCollaterals;
using Applications.Commons.Messages.Collaterals;
namespace Web.UI.Pages.Lists.Collaterals
{
    public partial class CollateralModalAdd
    {

        private readonly ICollateralsServiceImpl _collateralService;

        private readonly GetCollateralDetailQueryHandler _cusCollateralServices;
        [Parameter] public string CustomerId { get; set; } = string.Empty;
        [Parameter] public string? CustomerName { get; set; } = string.Empty;

        IEnumerable<CollateralTypeDto> CollateralTypes = [];

        IEnumerable<CollateralSubTypeDto> CollateralProperties = [];

        IEnumerable<CollateralSubTypeDto> CollateralPersonals = [];

        IEnumerable<CollateralSubTypeDto> allSubTypes = [];
        EditForm editForm { get; set; } = default!;

        CollateralTypeDto? SelectCollateralType;

        CollateralDtoCreate model { get; set; } = new();

        VCustomerDetails Customer = new();

        string? CollateralId = string.Empty;

        RadzenDataGrid<VCollateralLines> personalGrid = default!;
        List<VCollateralLines> _personalsToUpdate = [];
        List<VCollateralLines> _personals = [];
        List<VCollateralLines> _personalsToInsert = [];
        RadzenDataGrid<VCollateralLines> propertiesGrid = default!;
        List<VCollateralLines> _propertiesToUpdate = [];
        List<VCollateralLines> _propertiesToInert = [];
        List<VCollateralLines> _properties = [];
        public CollateralModalAdd(ICollateralsServiceImpl collateralService, GetCollateralDetailQueryHandler cusCollateralServices)
        {
            _collateralService = collateralService;
            _cusCollateralServices = cusCollateralServices;
        }

        void CloseDialogue()
        {
            DialogService.Close(true);
        }

        private EditContext editContext = default!;

        private ValidationMessageStore messageStore = default!;
        protected override async Task OnInitializedAsync()
        {

            editContext = new EditContext(model);

            messageStore = new ValidationMessageStore(editContext);

            await GetCollateralSubTypes();

            allSubTypes = await _collateralService.GetCollateralSubTypes();

            await GetCustomerCollaterals();

            await base.OnInitializedAsync();
        }

        private async Task GetCollateralSubTypes()
        {

            CollateralTypes = await _collateralService.GetCollateralTypes(0);

            CollateralProperties = await _collateralService.GetCollateralSubTypes((int)CollateralTypeEnum.Property);

            CollateralPersonals = await _collateralService.GetCollateralSubTypes((int)CollateralTypeEnum.Personal);
        }

        void PropSelectedItemChanged(object? arg, VCollateralLines line)
        {
            if (arg is not null)
            {
                line.ValueRate = ((CollateralSubTypeDto)arg).ValueRate ?? 0;
            }
        }
        void OnMarketPriceChanged(decimal? arg, VCollateralLines line)
        {
            var maxPrice = allSubTypes.Where(x => x.Id == line.SubCategoryRefId)
                                      .Select(x => x.LoanSizeAmuntMax)
                                      .FirstOrDefault();
            var valueRate = allSubTypes.Where(x => x.Id == line.SubCategoryRefId)
                                      .Select(x => x.ValueRate)
                                      .FirstOrDefault();
            if (maxPrice > 0 && arg > maxPrice)
            {
                NotificationService.Notify(NotificationSeverity.Warning, $"Market Price must be less than {maxPrice}");

                line.MarketPrice = maxPrice ?? 0;

                line.HotPrice = maxPrice * valueRate;

                line.PawnPrice = line.HotPrice * valueRate;

                return;
            }

            if (arg.HasValue && valueRate > 0)
            {
                line.HotPrice = arg.Value * valueRate;

                line.PawnPrice = line.HotPrice * valueRate;
            }
            else
            {
                line.PawnPrice = arg;

                line.HotPrice = arg;
            }

            StateHasChanged();
        }

        protected async Task GetCustomerCollaterals()
        {
            if (CustomerId.IsNullOrEmpty())
            {
                return;
            }

            var query = new GetCollateralDetailQuery(CustomerId);

            var rsp = await mediator.Send(query);


            if (!rsp.IsError)
            {
                var collateral = rsp.Value;

                CollateralId = CustomerId;

                model = collateral.ToCollateralDtoCreate();

                model.CollateralLines = [];

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

            if (type == CollateralTypeEnum.Personal)
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


            if (confirm == false)
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
            var validator = new VCollateralLineCreateValidator();

            var validatorResult = validator.Validate(e);

            if (!validatorResult.IsValid)
            {

                var errors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList();

                messageStore.ApplyValidationSummary(editContext, errors);

                return;
            }

            messageStore.ClearMessages(editContext);

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

        }

        async Task OnRowUpdate(VCollateralLines arg, CollateralTypeEnum type)
        {
            Reset(arg, type);


            var command = new AddOrUpdateCollateralCommand(model);

            var rsp = await mediator.Send(command);

            //await _collateralService.CreateAsync(model, false);

            if (!rsp.IsError)
            {
                NotificationService.Notify(NotificationSeverity.Success, CollateralMessages.Created);
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, rsp.FirstError.Description);
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

            model.CollateralLines.Clear();

            var prop = arg.MapCollateralLineCreate();

            model.CollateralLines.Add(prop);

            //var rsp = await _collateralService.CreateAsync(model, false);

            var command = new AddOrUpdateCollateralCommand(model);

            var rsp = await mediator.Send(command);

            if (!rsp.IsError)
            {
                NotificationService.Notify(NotificationSeverity.Success,CollateralMessages.Created);
            }
            else
            {

                editContext.ApplyValidationSummaries<CollateralDtoCreate>(rsp.Errors);

                return;
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
                row.CollateralId = original.CollateralId;
                row.CategoryRefId = original.CategoryRefId;
                row.CollateralNo = original.CollateralNo;
                row.TypeName = original.TypeName;
                row.SubTypeName = original.SubTypeName;
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
                row.CollateralId = origin.CollateralId;
                row.CollateralNo = origin.CollateralNo;
                row.CategoryRefId = origin.CategoryRefId;
                row.TypeName = origin.TypeName;
                row.SubCategoryRefId = origin.SubCategoryRefId;
                row.SubTypeName = origin.SubTypeName;
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
                    CollateralId = row.CollateralId,
                    CollateralNo = row.CollateralNo,
                    SubCategoryRefId = row.SubCategoryRefId,
                    CategoryRefId = row.CategoryRefId,
                    SubTypeName = row.SubTypeName,
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
                    CollateralId = row.CollateralId,
                    CollateralNo = row.CollateralNo,
                    SubCategoryRefId = row.SubCategoryRefId,
                    CategoryRefId = row.CategoryRefId,
                    SubTypeName = row.SubTypeName,
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


