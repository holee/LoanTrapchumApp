using Applications.Features.LoanRequests.Requests;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Text;

namespace Applications.Features.LoanRequests.Command.PassRequest;

public record PassRequestCommand(PassRequestDtoCreate Request) : IRequest<ErrorOr<RequestResult>>;

