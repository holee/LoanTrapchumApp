using Application.Bases;
using Application.Features.Holidays.Dtos;
using Application.Responses;
using Application.Responses.Paginations;
using Domains.Models.Lists;
using ErrorOr;

namespace Application.Interfaces.Lists
{
    public interface IHolidayService
    {
        Task<bool> ExistHolidays(DateTime? setDate, CancellationToken cancellation = default);
        Task<IEnumerable<Holiday>> GetHolidays(bool isSelectedPrevious = false, DateTime? startFrom = null, DateTime? end = null);
        Task<IEnumerable<HolidayDto>> GetHolidays(DateTime? selectedDate);
        Task<Pager<HolidayDto>> GetHolidays(QueryParam q, CancellationToken cancellation = default);
        Task<ErrorOr<DeleteResult>> DeleteHoliday(long? id, CancellationToken cancellation = default);
        Task<ErrorOr<IEnumerable<HolidayDto>>> CreateHolidays(List<Holiday> entities);
        Task<Pager<HolidayDto>> GetActiveHolidays(int? selectedYear, CancellationToken cancellation = default);
        Task<IEnumerable<HolidayDto>> GetActiveHolidays(DateTime? selectedDate, int? selectedYear = 0);
        Task<ErrorOr<HolidayDto>> CreateOrUpdate(Holiday entities, CancellationToken cancellation = default);
        Task<ErrorOr<Holiday>> GetHoliday(long? id, CancellationToken cancellation = default);
        Task<ErrorOr<string>> ActivateOrDeactivate(int id, Holiday req, CancellationToken cancellation = default);
        Task<Pager<Holiday>> BulkCreateHoliday(List<Holiday> enities, CancellationToken cancellation = default);
        Task<ErrorOr<Holiday>> GetByDate(DateTime? dt, CancellationToken cancellation = default);
        Task<Pager<HolidayDto>> GetHolidays(QueryParam q, DateTime? selectedDate = null, CancellationToken cancellation = default);
        Task<ErrorOr<HolidayDto>> ActivateOrDeactivate(long? id, CancellationToken cancellation = default);
    }
}
