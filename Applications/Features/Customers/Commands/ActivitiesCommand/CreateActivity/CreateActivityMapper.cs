using System.Diagnostics;
using Domains.Models.Lists.Customers;

namespace Applications.Features.Customers.Commands.ActivitiesCommand.CreateActivity;

public static class CreateActivityMapper
{
    public static CustomerActivity ToCreateActivity(this CreateActivityCommand source)
    {
        return new CustomerActivity
        {
            CustomerRefId = source.CustomerRefId,
            Description = source.Description,
            ActivityDate = source.ActivityDate,
            Status = source.Status
        };
    } 
}