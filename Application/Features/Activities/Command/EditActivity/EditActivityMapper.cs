using Domains.Models.Lists.Customers;

namespace Application.Features.Activities.Command.EditActivity;

public static class EditActivityMapper
{
    public static CustomerActivity ToActivity(this EditActivityCommand source)
    {
        return new CustomerActivity
        {
            Id = source.Request.Id,
            CustomerRefId = source.Request.CustomerRefId,
            Description = source.Request.Description,
            ActivityDate = source.Request.ActivityDate,
            Status = source.Request.Status
        };
    }
}