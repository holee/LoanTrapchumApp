using Domains.Models.Lists.Customers;
using Domains.Models.Lists.Employees;
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
    public static EntityNames ToEntityNames(this EntityNamesCreate entityNameCreate)
    {
        return entityNameCreate.Adapt<EntityNames>();
    }


}
