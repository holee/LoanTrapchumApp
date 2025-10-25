using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Customers.Commands.NotesCommand.EditNoteCommand
{
    public sealed class EditUpdateCommand:IRequest<ErrorOr<int>>
    {
        public int NoteId { get; set; }
        public DateTime NoteDate { get; set; } = DateTime.Now;
        public string? CustomerRefId { get; set; }
        public string? CreditAgentRefId { get; set; }
        public string? Description { get; set; }
    }
}
