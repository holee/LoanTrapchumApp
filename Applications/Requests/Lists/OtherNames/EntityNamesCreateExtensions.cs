using Domains.Models.Lists.Customers;
using Domains.Models.Lists.Employees;
using Domains.Models.Lists.Vendors;
using Mapster;

namespace Applications.Requests.Lists.OtherNames;

public static class EntityNamesCreateExtensions
{
    public static Employee ToEmployee(this EntityNamesCreate entityNameCreate)
    {
        return entityNameCreate.Adapt<Employee>();
    }

    public static Customer ToCustomer(this EntityNamesCreate entityNameCreate)
    {
        return entityNameCreate.Adapt<Customer>();
    }

    public static Vendor ToVendor(this EntityNamesCreate entityNameCreate)
    {
        return entityNameCreate.Adapt<Vendor>(); 
    }
    public static EntityNames ToEntityNames(this EntityNamesCreate entityNameCreate)
    {
        return entityNameCreate.Adapt<EntityNames>();
    }


}
