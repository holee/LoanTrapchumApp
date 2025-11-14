using Applications.Features.Employees.Dto;
using Applications.Features.OtherNames;
using Domains.Models.Lists;
using Mapster;

namespace Applications.Commons.Mappers;

public static class EntityNameMapper 
{
    //public static EntityNamesCreate ToCustomerCreate(this EntityNames c)
    //{
    //    var entityNamesCreate = c.Adapt<EntityNamesCreate>();
    //    entityNamesCreate.CustomerId = c.Customer?.CustomerId;
    //    entityNamesCreate.Dependents = c.Customer?.Dependents;
    //    entityNamesCreate.NoOfChildren = c.Customer?.NoOfChildren;
    //    entityNamesCreate.Income = c.Customer?.Income;
    //    entityNamesCreate.Expenditure = c.Customer?.Expenditure;
    //    entityNamesCreate.SpouseName = c.Customer?.SpouseName;
    //    entityNamesCreate.SpouseDateOfBirth = c.Customer?.SpouseDateOfBirth;
    //    entityNamesCreate.SpouseNationality = c.Customer?.SpouseNationality;
    //    entityNamesCreate.SpousePassport = c.Customer?.SpousePassport;
    //    entityNamesCreate.SpousePhoneNo = c.Customer?.SpousePhoneNo;
    //    entityNamesCreate.SpouseOccupation = c.Customer?.SpouseOccupation;
    //    entityNamesCreate.Photo = c.Customer?.Photo;
    //    entityNamesCreate.Occupation = c.Customer?.Occupation;
    //    return entityNamesCreate;
    //}

    //public static EntityNamesCreate ToVendorCreate(this EntityNames c)
    //{
    //    var entityNamesCreate = c.Adapt<EntityNamesCreate>(); 
    //    entityNamesCreate.VendorId = c.Vendor?.VendorId;
    //    entityNamesCreate.CompanyName = c.Vendor?.CompanyName;
    //    entityNamesCreate.CompanyAddress = c.Vendor?.CompanyAddress;
    //    return entityNamesCreate;
    //}
 

    public static EntityEmployeeCreate ToEmployeeCreate(this EntityNames c)  
    {
        var entityNamesCreate = c.Adapt<EntityEmployeeCreate>();
        entityNamesCreate.EmployeeId = c.Employee?.EmployeeId;
        entityNamesCreate.BranchRefId = c.Employee?.BranchRefId;
        entityNamesCreate.DepartmentRefId = c.Employee?.DepartmentRefId;
        entityNamesCreate.EndDate = c.Employee?.EndDate;
        entityNamesCreate.HireDate = c.Employee?.HireDate;
        entityNamesCreate.UserRefId = c.Employee?.UserRefId;
        entityNamesCreate.Photo = c.Employee?.Photo;
        entityNamesCreate.StaffId = c.Employee?.StaffId;
        return entityNamesCreate;
    }

    public static EntityNamesCreate ToNamesCreate(this EntityNames c)
    {
        return c.Adapt<EntityNamesCreate>();
    }

}