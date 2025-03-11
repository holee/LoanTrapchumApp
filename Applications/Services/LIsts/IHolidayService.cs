using Applications.Interfaces.Lists;
using Applications.Responses;
using Applications.Responses.Paginations;
using Domains.Models.Lists;

namespace Applications.Services.Lists
{
    public sealed class HolidayService(IHolidayService hService) : IHolidayService
    {
        public async Task<Result<string>> BulkCreateHoliday(List<Holiday> enity)
        {
            return await hService.BulkCreateHoliday(enity);
        }

        public async Task<Result<string>> BulkDeleteHoliday(List<long> id)
        {
            return await hService.BulkDeleteHoliday(id);
        }

        public async Task<Result<string>> CreateHoliday(Holiday enity)
        {
            return await hService.CreateHoliday(enity);
        }

        public async Task<Result<string>> DeleteHoliday(long id)
        {
            return await hService.DeleteHoliday(id);
        }

        public async Task<bool> Existdate(DateTime setDate)
        {
            return await hService.Existdate(setDate);
        }

        public async Task<Result<Holiday>> GetByDate(DateTime? dt)
        {
            return await hService.GetByDate(dt);
        }

        public async Task<IEnumerable<Holiday>> GetDate(DateTime q)
        {
            return await hService.GetDate(q);
        }

        public async Task<Holiday?> GetHoliday(long id)
        {
            return await hService.GetHoliday(id);
        }

        public async Task<Pager<Holiday>> GetHolidays(int pageNumber, int pageSize, string sortColumn = "HolidayName", bool isSelectedPrevious = false, bool sortDescending = false, string? q = null)
        {
            return await hService.GetHolidays(pageNumber, pageSize, sortColumn, isSelectedPrevious, sortDescending, q);

        }

        public async Task<IEnumerable<Holiday>> GetHolidays(bool isSelectedPrevious = false, DateTime? startFrom = null, DateTime? end = null)
        {
            return await hService.GetHolidays(isSelectedPrevious, startFrom, end);
        }

        public async Task<IEnumerable<Holiday>> GetHolidays(DateTime selectedDate)
        {
            return await hService.GetHolidays(selectedDate);
        }
    }
}
