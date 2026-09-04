using Application.Utils;
using ErrorOr;

namespace Application.Features.Notes.Command.EditNote
{
    public sealed class EditNoteCommand:IRequest<ErrorOr<int>>
    {
        public int NoteId { get; set; }
        public DateTime NoteDate { get; set; } = DateTime.Now;
        public string? CustomerRefId { get; set; }
        public string? CreditAgentRefId { get; set; }
        public string? Description { get; set; }
    }
}
