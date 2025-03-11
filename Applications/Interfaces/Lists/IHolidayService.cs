using Applications.Responses;
using Applications.Responses.Paginations;
using Domains.Models.Lists;

namespace Applications.Interfaces.Lists
{
    public interface  IHolidayService
    {
        Task<Result<string>> CreateHoliday(Holiday enity);
        Task<Result<string>> BulkCreateHoliday(List<Holiday> enity);
        Task<Result<string>> DeleteHoliday(long id);
        Task<Result<string>> BulkDeleteHoliday(List<long> id);
        Task<Holiday?> GetHoliday(long id);
        Task<Result<Holiday>> GetByDate(DateTime? dt); 
        Task<bool> Existdate(DateTime setDate);
        Task<IEnumerable<Holiday>> GetDate(DateTime q);
        Task<Pager<Holiday>> GetHolidays(int pageNumber, int pageSize, string sortColumn= "HolidayName",bool isSelectedPrevious = false,bool sortDescending=false, string? q = null);
        Task<IEnumerable<Holiday>> GetHolidays(bool isSelectedPrevious=false, DateTime? startFrom=null,DateTime? end=null);
        Task<IEnumerable<Holiday>> GetHolidays(DateTime selectedDate);
    }
}
