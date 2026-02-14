using Applications.Features.Transactions.Journals;
using Domains.Models.Journals;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Text;

namespace Applications.Features.Journals.Command.CreateJournal;

public record CreateJournalCommand(JournalTransactionDtoCreate req) : IRequest<ErrorOr<Domains.Models.Journals.JournalEntry?>>;
