using Applications.Features.Activities.Command.CreateActivity;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Activities.Command.EditActivity;

public sealed record EditActivityCommand(ActivityDtoCreate Request) : IRequest<ErrorOr<int>>;
//{
//    public int Id { get; set; }
//    public string? CustomerRefId { get; set; }
//    public DateTime? ActivityDate { get; set; }
//    public string? Description { get; set; } = string.Empty;
//    public bool Status { get; set; } = true;
//}
