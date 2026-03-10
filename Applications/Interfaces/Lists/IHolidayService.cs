using Applications.Bases;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Holidays;
using Domains.Models.Lists;
using ErrorOr;

namespace Applications.Interfaces.Lists
{
    public interface IHolidayService
    {
        Task<bool> ExistHolidays(DateTime? setDate, CancellationToken cancellation = default);
        Task<IEnumerable<Holiday>> GetHolidays(bool isSelectedPrevious = false, DateTime? startFrom = null, DateTime? end = null);
        Task<IEnumerable<HolidayVm>> GetHolidays(DateTime? selectedDate);
        Task<Pager<HolidayVm>> GetHolidays(QueryParam q, CancellationToken cancellation = default);
        Task<ErrorOr<DeleteResult>> DeleteHoliday(long? id, CancellationToken cancellation = default);
        Task<ErrorOr<IEnumerable<HolidayVm>>> CreateHolidays(List<Holiday> entities);
        Task<Pager<HolidayVm>> GetActiveHolidays(int? selectedYear, CancellationToken cancellation = default);
        Task<IEnumerable<HolidayVm>> GetActiveHolidays(DateTime? selectedDate, int? selectedYear = 0);
        Task<ErrorOr<HolidayVm>> CreateOrUpdate(Holiday entities, CancellationToken cancellation = default);
        Task<ErrorOr<Holiday>> GetHoliday(long? id, CancellationToken cancellation = default);
        Task<ErrorOr<string>> ActivateOrDeactivate(int id, Holiday req, CancellationToken cancellation = default);
        Task<Pager<Holiday>> BulkCreateHoliday(List<Holiday> enities, CancellationToken cancellation = default);
        Task<ErrorOr<Holiday>> GetByDate(DateTime? dt, CancellationToken cancellation = default);
        Task<Pager<HolidayVm>> GetHolidays(QueryParam q, DateTime? selectedDate = null, CancellationToken cancellation = default);
        Task<ErrorOr<HolidayVm>> ActivateOrDeactivate(long? id, CancellationToken cancellation = default);
    }
}
