using Applications.Bases;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Holidays;
using Domains.Models.Lists;
using ErrorOr;

namespace Applications.Interfaces.Lists
{
    public interface  IHolidayService
    {
        Task<Result<string>> CreateOrUpdateHoliday(Holiday enity);

        Task<Result<string>> BulkCreateHoliday(List<Holiday> enity);
        Task<Result<string>> DeleteHoliday(long id);
        Task<Result<string>> BulkDeleteHoliday(List<long> id);
        Task<Result<Holiday>> GetHoliday(long id);
        Task<Result<Holiday>> GetByDate(DateTime? dt); 
        Task<bool> ExistHolidays(DateTime? setDate);
        Task<IEnumerable<Holiday>> GetDate(DateTime q);
        Task<Pager<Holiday>> GetHolidays(int pageNumber, int pageSize, string sortColumn= "HolidayName",bool isSelectedPrevious = false,bool sortDescending=false, string? q = null);
        Task<IEnumerable<Holiday>> GetHolidays(bool isSelectedPrevious=false, DateTime? startFrom=null,DateTime? end=null);
        Task<IEnumerable<HolidayVm>> GetHolidays(DateTime? selectedDate);
        Task<Pager<HolidayVm>> GetHolidays(QueryParam q, CancellationToken cancellation = default);
        Task<IEnumerable<HolidayVm>> GetActiveHolidays(DateTime? selectedDate,int? selectedYear= 0);
        Task<Result<IEnumerable<HolidayVm>>> CreateHolidays(IEnumerable<Holiday> entities);
        Task<Result<string>> ActivateOrDeactivate(int id, Holiday req);
        Task<ErrorOr<DeleteResult>> DeleteHoliday(long? id, CancellationToken cancellation = default);
        Task<ErrorOr<IEnumerable<HolidayVm>>> CreateHolidays(List<Holiday> entities);
        Task<ErrorOr<long>> CreateOrUpdate(Holiday entities, CancellationToken cancellation = default);
    }
}
