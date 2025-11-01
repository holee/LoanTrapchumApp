using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Activities.Command.CreateActivity
{
    public sealed record CreateActivityCommand(ActivityDtoCreate Request) : IRequest<ErrorOr<int>>;
    //{
    //    public string? CustomerRefId { get; set; }
    //    public DateTime? ActivityDate { get; set; } = DateTime.Now;
    //    public string? Description { get; set; } = string.Empty;
    //    public bool Status { get; set; } = true;
    //}
}
