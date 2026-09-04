using Application.Features.Activities.Command.CreateActivity;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Activities.Command.EditActivity;

public sealed record EditActivityCommand(ActivityDtoCreate Request) : IRequest<ErrorOr<int>>;
//{
//    public int Id { get; set; }
//    public string? CustomerRefId { get; set; }
//    public DateTime? ActivityDate { get; set; }
//    public string? Description { get; set; } = string.Empty;
//    public bool Status { get; set; } = true;
//}
