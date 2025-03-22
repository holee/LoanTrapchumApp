using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Responses.Lists.Customers
{
    public sealed class CustomerNoteResponse
    {
        public int NoteId { get; set; }
        public long? CustomerRefId { get; set; }
        public long? CreditAgentRefId { get; set; }
        public string? Description { get; set; }
    }
}
