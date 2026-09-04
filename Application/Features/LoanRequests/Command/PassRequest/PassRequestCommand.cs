using Application.Features.LoanRequests.Requests;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.LoanRequests.Command.PassRequest;

public record PassRequestCommand(PassRequestDtoCreate Request) : IRequest<ErrorOr<RequestResult>>;

