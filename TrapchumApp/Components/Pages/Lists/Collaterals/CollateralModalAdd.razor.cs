using AppilcationServices.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;
using Radzen.Blazor;
using TCLMS.Shared.Entities.Dtos.Collaterals;
using TCLMS.Shared.Entities.Models.Lists.Customers;
using TCLMS.Shared.Entities.ViewModels.Loans;

namespace TCLMS.Front.Pages.Lists.Collaterals
{
    public partial class CollateralModalAdd
    {
        [Parameter] public long? CustomerId { get; set; } = 0;
        [Parameter] public string? CustomerName { get; set; } = string.Empty;
        IEnumerable<CollateralTypeDto> CollateralTypes = [];
        IEnumerable<CollateralSubTypeDto> CollateralProperties = [];
        IEnumerable<CollateralSubTypeDto> CollateralPersonals = [];
        IEnumerable<CollateralSubTypeDto> allSubTypes = [];
        List<CollateralLineDto> Properties = [];
        List<CollateralLineDto> Personals = [];
        EditForm editForm { get; set; } = default!;
        CollateralTypeDto? SelectCollateralType;
        CollateralDto model { get; set; } = new();
        VCustomerList Customer = new();
        long CollateralId = 0;
        RadzenDataGrid<CollateralLineDto> personalGrid = default!;
        RadzenDataGrid<CollateralLineDto> propertiesGrid = default!;
        void CloseDialogue()
        {
            DialogService.Close(false);
        }

        protected override async Task OnInitializedAsync()
        {

            var response = await _customerService.GetCustomerDetailsAsync(CustomerId);

            if (response.IsSuccess)
            {
                Customer = response.Data!;
                CustomerName = response.Data!.AltName;
            }
            else
            {
                var error = response.Errors!.FirstOrDefault();

                NotificationService.Notify(Radzen.NotificationSeverity.Error, error);

            }
            CollateralTypes = await _collateralService.GetCollateralType(0);

            CollateralProperties = await _collateralService.GetCollateralSubTypes(1);

            CollateralPersonals = await _collateralService.GetCollateralSubTypes(3);

            await GetCollateralLineByCustomer();

            allSubTypes = await _collateralService.GetCollateralSubTypes();

            await base.OnInitializedAsync();
        }

        protected async Task GetCollateralLineByCustomer()
        {
            if (CustomerId == 0)
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
                              .Where(x => x.CategoryRefId == 3)
                              .ToList();
                Properties = collateral.CollateralLines
                                .Where(x => x.CategoryRefId == 1)
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

        ////
        ///create method
        protected async Task FormValidate()
        {
            allSubTypes = await _collateralService.GetCollateralSubTypes();

            List<CollateralLineDto> lines = [];

            var insertedProperties = Properties.Where(x => x.SubCategoryRefId > 0).ToList();

            var insertedPersanals = Personals.Where(x => x.SubCategoryRefId > 0).ToList();

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
            
            if (CustomerId == 0)
            {
                NotificationService.Notify(Radzen.NotificationSeverity.Info, "Please select customer first");
                return;
            }
            editForm.EditContext?.Validate();
            try
            {
                var response = await _collateralService.CreateCollateral(CustomerId, model);

                if (response.IsSuccess)
                {

                    NotificationService.Notify(Radzen.NotificationSeverity.Success, response.Data!.Message);
                    DialogService.Close(true);
                    model = new();
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


        void ResetPerson()
        {
            Personals.Clear();
        }

        void ResetPerson(CollateralLineDto e)
        {
            if (!Personals.Contains(e))
            {
                return;
            }
            Personals.Remove(e);
        }
        async Task InsertPersonRow()
        {
            var e = new CollateralLineDto();
            e.CategoryRefId = 3;
            e.TypeName = "Personal";
            await personalGrid.InsertRow(e);
        }
        void CancelPersonEdit(CollateralLineDto e)
        {
            ResetPerson(e);
            personalGrid.CancelEditRow(e);
        }

        async Task DeletePersonRow(CollateralLineDto e)
        {
            ResetPerson(e);
            personalGrid.CancelEditRow(e);
            await personalGrid.Reload();

        }

        async Task SavePersonRow(CollateralLineDto e)
        {
            Personals.Add(e);
            await personalGrid.UpdateRow(e);
        }


        async Task EditRow(CollateralLineDto e, int type = 1)
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




        /// <summary>
        /// Protperies
        /// </summary>

        void ResetProperty()
        {
            Properties.Clear();
        }

        void ResetProperty(CollateralLineDto e)
        {
            if (!Properties.Contains(e))
            {
                return;
            }
            Properties.Remove(e);
        }

        async Task InsertPropertyRow()
        {
            var e = new CollateralLineDto();
            e.CategoryRefId = 1;
            e.TypeName = "Propery";
            await propertiesGrid.InsertRow(e);
        }

        void CancelPropertyEdit(CollateralLineDto e)
        {
            ResetPerson(e);
            propertiesGrid.CancelEditRow(e);
        }

        async Task DeletePropertyRow(CollateralLineDto e)
        {
            ResetProperty(e);
            propertiesGrid.CancelEditRow(e);
            await propertiesGrid.Reload();

        }

        async Task SavePropertyRow(CollateralLineDto e)
        {
            Properties.Add(e);
            await propertiesGrid.UpdateRow(e);
        }

    }





}

//private void AddNewRow(int collateralTypeId)
//{
//    if (collateralTypeId == 3)
//    {
//        Personals.Add(new CollateralLineDTO
//        {
//            CategoryRefId = collateralTypeId,
//            TypeName = "Personal"
//        });

//        personalGrid.Reload();
//    }
//    else
//    {
//        Properties.Add(new CollateralLineDTO
//        {
//            CategoryRefId = collateralTypeId,
//            TypeName = "Property"
//        });
//        propertiesGrid.Reload();
//    }

//}
//private void Remove(CollateralLineDTO row, int type = 3)
//{
//    if ((type == 3))
//    {
//        Personals.Remove(row);
//        personalGrid.Reload();
//    }
//    else
//    {
//        Properties.Remove(row);
//        propertiesGrid.Reload();
//    }

//}