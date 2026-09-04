using Application.Features.LoanRequests.Requests;
using Application.Utils;
using ErrorOr;

namespace Application.Features.LoanRequests.Command.AddLoanRequestDocument;

public record AddLoanRequestDocumentsCommand(List<LoanDocumentDtoCreate> Requests) :IRequest<ErrorOr<string>>;
