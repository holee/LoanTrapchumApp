using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace TrapchumApp.Components.Pages.Lists.OtherNames
{
    public partial class Index
    {
        





        // async void MenuClicked(RadzenSplitButtonItem args, EntNameDto e)
        // {
        //     if (args is not null)
        //     {
        //         if (args.Value == "1")
        //         {
        //             //await DisplayDetail(e.EntId);
        //             return;
        //         }
        //         if (args.Value == "2")
        //         {
        //             await ShowEdit(e.EntId);
        //             return;
        //         }
        //         if (args.Value == "3")
        //         {
        //             // await OnDelete(e.EntId);
        //             return;
        //         }
        //         if (args.Value == "4")
        //         {
        //             await ConvertToEmployee(e);
        //             return;
        //         }
        //         if (args.Value == "5")
        //         {
        //             await ConvertToCustomer(e);
        //             return;
        //         }
        //         if (args.Value == "6")
        //         {
        //             await ConvertToVedor(e);
        //             return;
        //         }
        //     }
        //     return;
        // }

        // protected async Task ShowEdit(long id)
        // {
        //     if (id != 0)
        //     {
        //         try
        //         {

        //             modelEdit = await _otherNameService!.GetToEditAsync(id);
        //             IsEnableEdit = true;
        //             selectedIndex = 2;
        //             ProvinceList = await AddressService!.GetAllProvinceAsync();
        //             StateHasChanged();
        //             await GetSalutations();

        //             if (modelEdit is not null)
        //             {
        //                 if (modelEdit!.ProvinceRefId.HasValue)
        //                 {
        //                     SelectedProvince = ProvinceList!.FirstOrDefault(d => d.ProvinceID == modelEdit?.ProvinceRefId);
        //                 }
        //                 if (modelEdit.DistrictRefId.HasValue)
        //                 {
        //                     this.DistrictList = await _addressService!.GetDistrictByProvinceAsync(modelEdit?.ProvinceRefId);
        //                     SelectedDistrict = DistrictList.FirstOrDefault(d => d.DistrictId == modelEdit?.DistrictRefId);

        //                 }
        //                 if (modelEdit!.DistrictRefId.HasValue)
        //                 {
        //                     this.CommuneList = await _addressService!.GetCommuneByDistricteAsync(modelEdit?.DistrictRefId);
        //                     SelectedCommune = this.CommuneList.FirstOrDefault(c => c.CommuneId == modelEdit?.CommuneRefId);
        //                 }
        //                 if (modelEdit!.DistrictRefId.HasValue)
        //                 {
        //                     this.VillageList = await _addressService!.GetVillageByCommuneAsync(modelEdit?.CommuneRefId);
        //                     SelectedVillage = this.VillageList.FirstOrDefault(x => x.VillageID == modelEdit?.VillageRefId);
        //                     Console.WriteLine(SelectedVillage!.VilInKhmer + " Selected");

        //                 }
        //             }
        //             ContextMenuService.Close();

        //             StateHasChanged();

        //         }
        //         catch (Exception ex)
        //         {
        //             NotificationService.Notify(NotificationSeverity.Error, ex.Message);
        //         }

        //     }
        // }
        // protected async Task ConvertToCustomer(EntNameDto e)
        // {
        //     var confirm = (bool)await DialogService.Confirm("Are you sure!, you want to this other name to customer?", "Changing Entity",
        // new Radzen.ConfirmOptions { OkButtonText = "Yes", CancelButtonText = "No" });
        //     if (confirm)
        //     {
        //         if (e is null)
        //         {
        //             NotificationService.Notify(Radzen.NotificationSeverity.Success, "Changing Entity", "Getting other name information is empty!");
        //             return;
        //         }
        //         var customer = new CustomerDtoCreate();
        //         customer.FirstName = e.FirstName;
        //         customer.LastName = e.LastName == "" ? "N/A" : e.LastName;
        //         customer.MiddleName = e.MiddleName;
        //         customer.AltName = e.AltName;
        //         customer.JobTitle = e.JobTitle;
        //         customer.Gender = e.Sex;
        //         customer.Phone = e.Phone;
        //         customer.AltPhone = e.AltPhone;
        //         customer.EmailAddress = e.EmailAddress;
        //         customer.CompanyName = "N/A";
        //         customer.EnType = "Customer";
        //         customer.IDCard = e.IDCard;
        //         customer.Description = e.Description;
        //         customer.CountryName = e.CountryName;
        //         customer.City = e.City;
        //         customer.ProvinceRefId = e.ProvinceRefId;
        //         customer.ProDescription = e.ProDescription;
        //         customer.ProInKhmer = e.ProInKhmer;
        //         customer.DistrictRefId = e.DistrictRefId;
        //         customer.DisDescription = e.DisDescription;
        //         customer.DisInKhmer = e.DisInKhmer;
        //         customer.CommuneRefId = e.CommuneRefId;
        //         customer.ComDescription = e.ComDescription;
        //         customer.ComInKhmer = e.ComInKhmer;
        //         customer.VillageRefId = e.VillageRefId;
        //         customer.VillDescription = e.VillDescription;
        //         customer.VilInKhmer = e.VilInKhmer;
        //         try
        //         {
        //             var resutl = await _otherNameService!.UpdateToCustomerAsync(e.EntId, customer);
        //             if (resutl == true)
        //             {
        //                 NotificationService.Notify(Radzen.NotificationSeverity.Success, "Changing Entity", "Changing other name to customer was successful!");
        //                 customer = new();
        //                 await OnInitializedAsync();
        //                 await InvokeAsync(StateHasChanged);
        //                 return;
        //             }
        //             else
        //             {
        //                 NotificationService.Notify(Radzen.NotificationSeverity.Success, "Changing Entity", "Changing was failed! ");
        //             }
        //             ContextMenuService.Close();

        //         }
        //         catch (Exception ex)
        //         {
        //             NotificationService.Notify(Radzen.NotificationSeverity.Success, "Changing Entity", "Changing was failed! ");

        //         }

        //     }
        //     else
        //     {
        //         Console.WriteLine("Canceled");
        //         ContextMenuService.Close();

        //         return;
        //     }
        // }
        // protected async Task ConvertToVedor(EntNameDto e)
        // {
        //     var confirm = (bool)await DialogService.Confirm("Are you sure!, you want to this other name to model?", "Changing Entity",
        // new Radzen.ConfirmOptions { OkButtonText = "Yes", CancelButtonText = "No" });
        //     if (confirm)
        //     {
        //         var vendor = new VendorDTOCreate();
        //         if (e is null)
        //         {
        //             NotificationService.Notify(Radzen.NotificationSeverity.Success, "Changing Entity", "Getting other name information is empty!");

        //             return;
        //         }
        //         vendor.FirstName = e.FirstName;
        //         vendor.LastName = e.LastName;
        //         vendor.MiddleName = e.MiddleName;
        //         vendor.Name = e.Name;
        //         vendor.AltName = e.AltName;
        //         vendor.JobTitle = e.JobTitle;
        //         vendor.Salution = e.Sex;
        //         vendor.Phone = e.Phone;
        //         vendor.AltPhone = e.AltPhone;
        //         vendor.EmailAddress = e.EmailAddress;
        //         vendor.CompanyName = "N/A";
        //         vendor.EnType = "Vendor";
        //         vendor.IDCard = e.IDCard;
        //         vendor.IsActive = e.IsActive;
        //         vendor.Description = e.Description;
        //         vendor.CountryName = e.CountryName;
        //         vendor.City = e.City;
        //         vendor.ProvinceRefId = e.ProvinceRefId;
        //         vendor.ProDescription = e.ProDescription;
        //         vendor.ProInKhmer = e.ProInKhmer;
        //         vendor.DistrictRefId = e.DistrictRefId;
        //         vendor.DisDescription = e.DisDescription;
        //         vendor.DisInKhmer = e.DisInKhmer;
        //         vendor.CommuneRefId = e.CommuneRefId;
        //         vendor.ComDescription = e.ComDescription;
        //         vendor.ComInKhmer = e.ComInKhmer;
        //         vendor.VillageRefId = e.VillageRefId;
        //         vendor.VillDescription = e.VillDescription;
        //         vendor.VilInKhmer = e.VilInKhmer;
        //         var resutl = await _otherNameService!.UpdateToVendorAsync(e.EntId, vendor);
        //         if (resutl == true)
        //         {
        //             NotificationService.Notify(Radzen.NotificationSeverity.Success, "Changing Entity", "Changing other name to model was successful!");
        //             vendor = new();
        //             await OnInitializedAsync();
        //             StateHasChanged();
        //             return;
        //         }
        //         else
        //         {
        //             NotificationService.Notify(Radzen.NotificationSeverity.Success, "Changing Entity", "Changing was failed!");

        //         }
        //         ContextMenuService.Close();

        //     }
        //     else
        //     {
        //         Console.WriteLine("Canceled");
        //         ContextMenuService.Close();

        //     }
        // }
        // protected async Task ConvertToEmployee(EntNameDto e)
        // {
        //     var confirm = (bool)await DialogService.Confirm("Are you sure!, you want to this other name to employee?", "Changing Entity",
        // new Radzen.ConfirmOptions { OkButtonText = "Yes", CancelButtonText = "No" });
        //     if (confirm)
        //     {
        //         var model = new EmployeeDtoCreate();
        //         if (e is null)
        //         {
        //             NotificationService.Notify(Radzen.NotificationSeverity.Success, "Changing Entity", "Getting other name information is empty!");
        //             return;
        //         }
        //         model.FirstName = e.FirstName;
        //         model.LastName = e.LastName == "" ? "N/A" : e.LastName;
        //         model.MiddleName = e.MiddleName;
        //         model.Name = e.Name;
        //         model.AltName = e.AltName;
        //         model.DateOfBirth = DateTime.Now;
        //         model.JobTitle = e.JobTitle;
        //         model.Salutation = e.Sex;
        //         model.Gender = "N/A";
        //         model.Phone = e.Phone;
        //         model.AltPhone = e.AltPhone;
        //         model.EmailAddress = e.EmailAddress;
        //         model.EntType = "Employee";
        //         model.IDCard = e.IDCard;
        //         model.IsActive = e.IsActive;
        //         model.Description = e.Description;
        //         model.CountryName = e.CountryName;
        //         model.City = e.City;
        //         model.ProvinceRefId = e.ProvinceRefId;
        //         model.ProDescription = e.ProDescription;
        //         model.ProInKhmer = e.ProInKhmer;
        //         model.DistrictRefId = e.DistrictRefId;
        //         model.DisDescription = e.DisDescription;
        //         model.DisInKhmer = e.DisInKhmer;
        //         model.CommuneRefId = e.CommuneRefId;
        //         model.ComDescription = e.ComDescription;
        //         model.ComInKhmer = e.ComInKhmer;
        //         model.VillageRefId = e.VillageRefId;
        //         model.VillDescription = e.VillDescription;
        //         model.VilInKhmer = e.VilInKhmer;
        //         var resutl = await _otherNameService!.UpdateToEmployeeAsync(e.EntId, model);
        //         if (resutl == true)
        //         {
        //             NotificationService.Notify(Radzen.NotificationSeverity.Success, "Changing Entity", "Changing other name to model was successful!");
        //             model = new();
        //             await OnInitializedAsync();
        //             StateHasChanged();
        //             return;
        //         }
        //         else
        //         {
        //             NotificationService.Notify(Radzen.NotificationSeverity.Success, "Changing Entity", "Changing was failed!");

        //         }
        //         ContextMenuService.Close();

        //     }
        //     else
        //     {
        //         Console.WriteLine("Canceled");
        //         ContextMenuService.Close();

        //     }
        // }
        // protected EntNameDto? OtherNames = new();

        // private async Task GetSalutations()
        // {
        //     var salutations = await _salutationService.GetSalutions();
        //     if (salutations.IsSuccess)
        //     {
        //         Salutations = salutations!.Data!.Select(x => x.Title!).ToList();
        //     }
        //     else
        //     {
        //         Salutations = [];
        //     }

        // }

        // async void MenuClicked(RadzenSplitButtonItem args, EntNameDto e)
        // {
        //     if (args is not null)
        //     {
        //         if (args.Value == "1")
        //         {
        //             await DisplayDetail(e.EntId);
        //             return;
        //         }
        //         if (args.Value == "2")
        //         {
        //             await ShowEdit(e.EntId);
        //             return;
        //         }
        //         if (args.Value == "3")
        //         {
        //             await OnDelete(e.EntId);
        //             return;
        //         }
        //         if (args.Value == "4")
        //         {
        //             await ConvertToEmployee(e);
        //             return;
        //         }
        //         if (args.Value == "5")
        //         {
        //             await ConvertToCustomer(e);
        //             return;
        //         }
        //         if (args.Value == "6")
        //         {
        //             await ConvertToVedor(e);
        //             return;
        //         }
        //     }
        //     return;
        // }
        // void AddTabOtherName()
        // {
        //     IsEnableNew = true;
        //     selectedIndex = 1;
        //     StateHasChanged();
        // }
        // async void CloseTabs(int e)
        // {
        //     if (selectedIndex == 1)
        //     {
        //         IsEnableNew = false;
        //         selectedIndex = 0;

        //         await OnInitializedAsync();
        //     }
        //     else if (selectedIndex == 2)
        //     {
        //         IsEnableEdit = false;
        //         selectedIndex = 0;

        //         await OnInitializedAsync();
        //     }
        //     else if (selectedIndex == 3)
        //     {
        //         IsEnableDetail = false;
        //         selectedIndex = 0;

        //     }
        //     StateHasChanged();
        // }
        // protected async Task DeactiveOrActive(EntNameDto e)
        // {

        //     var response = await _otherNameService!.UpdateStatusAsync(e.EntId);
        //     if (response.IsSuccess)
        //     {
        //         // names!.ToList().Remove(e);
        //         // NotificationService.Notify(NotificationSeverity.Success, "", response.Message);
        //         // await IncludeInActiveChanged(false);
        //         // await InvokeAsync(StateHasChanged);

        //     }
        //     else
        //     {
        //         await IncludeInActiveChanged(false);
        //         NotificationService.Notify(NotificationSeverity.Warning, "", "Changing status failded!");
        //     }
        // }


        // protected async Task DisplayDetail(long e)
        // {
        //     if (e != 0)
        //     {
        //         try
        //         {
        //             OtherNames = await _otherNameService!.GetByIdAsync(e);
        //             IsEnableDetail = true;
        //             selectedIndex = 3;
        //             await InvokeAsync(StateHasChanged);
        //             ContextMenuService.Close();

        //         }
        //         catch (Exception ex)
        //         {
        //             NotificationService.Notify(NotificationSeverity.Error, "Exception", ex.Message);
        //         }

        //     }
        // }
        // async Task OnDelete(long id)
        // {
        //     var confirm = (bool)await DialogService.Confirm("Other Name deleted will not be able to recover, would you like to continue?", "Delete Other Name",
        //     new Radzen.ConfirmOptions { OkButtonText = "Yes", CancelButtonText = "No" });
        //     if (confirm)
        //     {
        //         var result = await _otherNameService!.DeleteAsync(id);
        //         if (result == true)
        //         {
        //             NotificationService.Notify(Radzen.NotificationSeverity.Success, "Delete Other Name", "Other Name was deleted successful!");
        //             await OnInitializedAsync();
        //             ContextMenuService.Close();
        //             return;

        //         }
        //     }
        //     else
        //     {
        //         Console.WriteLine("Canceled");
        //         ContextMenuService.Close();

        //     }
        // }



    }
}