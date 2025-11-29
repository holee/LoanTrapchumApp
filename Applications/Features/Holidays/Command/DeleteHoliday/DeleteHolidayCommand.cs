using Applications.Bases;
using Applications.Utils;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.Holidays.Command.DeleteHoliday;

public record DeleteHolidayCommand(long? Id) : IRequest<ErrorOr<DeleteResult>>;
