using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Customers.Commands.ActivitiesCommand.EditActivity
{
    public sealed class EditActivityCommand : IRequest<ErrorOr<int>>
    {
        public int Id { get; set; }
        public string? CustomerRefId { get; set; }
        public DateTime? ActivityDate { get; set; }
        public string? Description { get; set; } = string.Empty;
        public bool Status { get; set; } = true;
    }
}
