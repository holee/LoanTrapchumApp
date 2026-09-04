using Application.Bases;
using Application.Utils;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Holidays.Command.DeleteHoliday;

public record DeleteHolidayCommand(long? Id) : IRequest<ErrorOr<DeleteResult>>;
