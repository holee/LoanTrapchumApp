using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Customers.Commands.NotesCommand.CreateNoteCommand;

public sealed class AddNoteCommand : IRequest<ErrorOr<int>>
{

    public int NoteId { get; set; } = 0; 
    public DateTime? NoteDate { get; set; } = DateTime.Now;
    public string? CustomerRefId { get; set; }
    
    public string? CreditAgentRefId { get; set; }

    public string? Description { get; set; }   
}