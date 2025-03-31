using Applications.Requests.Lists.OtherNames;
using Domains.Models.Lists.Customers;
using Mapster;

namespace Applications.Extensions;

public static class CustomerExtensions
{
    public static EntityNamesCreate ToEntityNamesCreate(this EntityNames c)
    {
        var entityNamesCreate = c.Adapt<EntityNamesCreate>();
        entityNamesCreate.CustomerId = c.Customer?.CustomerId;
        entityNamesCreate.Dependents = c.Customer?.Dependents;
        entityNamesCreate.NoOfChildren = c.Customer?.NoOfChildren;
        entityNamesCreate.Income = c.Customer?.Income;
        entityNamesCreate.Expenditure = c.Customer?.Expenditure;
        entityNamesCreate.SpouseName = c.Customer?.SpouseName;
        entityNamesCreate.SpouseDateOfBirth = c.Customer?.SpouseDateOfBirth;
        entityNamesCreate.SpouseNationality = c.Customer?.SpouseNationality;
        entityNamesCreate.SpousePassport = c.Customer?.SpousePassport;
        entityNamesCreate.SpousePhoneNo = c.Customer?.SpousePhoneNo;
        entityNamesCreate.SpouseOccupation = c.Customer?.SpouseOccupation;
        entityNamesCreate.Photo = c.Customer?.Photo;
        entityNamesCreate.Occupation = c.Customer?.Occupation;
        return entityNamesCreate;
    }
}