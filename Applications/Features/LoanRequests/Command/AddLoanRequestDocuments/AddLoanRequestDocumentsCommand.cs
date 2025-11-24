using Applications.Features.LoanRequests.Requests;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.LoanRequests.Command.AddLoanRequestDocument;

public record AddLoanRequestDocumentsCommand(List<LoanDocumentDtoCreate> Requests) :IRequest<ErrorOr<string>>;
