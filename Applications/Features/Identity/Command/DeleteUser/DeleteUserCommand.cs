using ErrorOr;
using System;
using System.Collections.Generic;
using System.Text;

namespace Applications.Features.Identity.Command.DeleteUser;

public record DeleteUserCommand(string Id) : IRequest<ErrorOr<DeleteResult>>
{
}
