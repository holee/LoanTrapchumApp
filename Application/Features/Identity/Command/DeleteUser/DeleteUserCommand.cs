using ErrorOr;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Identity.Command.DeleteUser;

public record DeleteUserCommand(string Id) : IRequest<ErrorOr<DeleteResult>>
{
}
