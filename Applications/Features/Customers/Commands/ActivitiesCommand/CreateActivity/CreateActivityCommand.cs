using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Customers.Commands.ActivitiesCommand.CreateActivity
{
    public sealed class CreateActivityCommand : IRequest<ErrorOr<int>>
    {
        public string? CustomerRefId { get; set; }
        public DateTime? ActivityDate { get; set; } = DateTime.Now;
        public string? Description { get; set; } = string.Empty;
        public bool Status { get; set; } = true;
    }
}
