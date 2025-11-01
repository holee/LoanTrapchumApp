using System.Diagnostics;
using Domains.Models.Lists.Customers;

namespace Applications.Features.Activities.Command.CreateActivity;

public static class CreateActivityMapper
{
    public static CustomerActivity ToCreateActivity(this CreateActivityCommand source)
    {
        return new CustomerActivity
        {
            CustomerRefId = source.Request.CustomerRefId,
            Description = source.Request.Description,
            ActivityDate = source.Request.ActivityDate,
            Status = source.Request.Status
        };
    }
}