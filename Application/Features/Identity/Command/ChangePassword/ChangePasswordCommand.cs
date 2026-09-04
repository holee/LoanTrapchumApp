using Application.Features.Identity.Users;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Identity.Command.ChangePassword;

public record ChangePasswordCommand(ChangePasswordDto Request) : IRequest<ErrorOr<string>>;
