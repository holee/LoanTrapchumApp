using Applications.Features.Identity.Users;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Text;

namespace Applications.Features.Identity.Command.ChangePassword;

public record ChangePasswordCommand(ChangePasswordDto Request) : IRequest<ErrorOr<string>>;
