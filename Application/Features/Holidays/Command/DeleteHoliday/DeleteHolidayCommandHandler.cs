using Application.Bases;
using Application.Interfaces.Lists;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Holidays.Command.DeleteHoliday;

public sealed class DeleteHolidayCommandHandler : IRequestHandler<DeleteHolidayCommand, ErrorOr<DeleteResult>>
{
    private readonly IHolidayService _holidayRepository;
    public DeleteHolidayCommandHandler(IHolidayService holidayRepository)
    {
        _holidayRepository = holidayRepository;
    }
    public async Task<ErrorOr<DeleteResult>> Handle(DeleteHolidayCommand request, CancellationToken cancellationToken)
    {
        return await _holidayRepository.DeleteHoliday(request.Id, cancellationToken);
    }
}