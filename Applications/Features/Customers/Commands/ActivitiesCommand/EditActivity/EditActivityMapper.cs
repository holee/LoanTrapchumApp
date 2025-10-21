using Applications.Features.Customers.Commands.ActivitiesCommand.CreateActivity;
using Domains.Models.Lists.Customers;

namespace Applications.Features.Customers.Commands.ActivitiesCommand.EditActivity;

public static class EditActivityMapper
{
    public static CustomerActivity ToActivity(this EditActivityCommand source)
    {
        return new CustomerActivity
        {
            Id = source.Id,
            CustomerRefId = source.CustomerRefId,
            Description = source.Description,
            ActivityDate = source.ActivityDate,
            Status = source.Status
        };
    } 
}