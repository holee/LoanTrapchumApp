using Applications.Extensions.mappers;
using Applications.Interfaces.Lists;
using Applications.Requests.Lists.Holidays;
using Applications.Responses;
using Applications.Responses.Paginations;
using Domains.Models.Lists;
using Mapster;

namespace Applications.Services.Lists
{
    public interface IHolidayServiceImpl
    {
        Task<Result<string>> BulkCreateHoliday(List<HolidayCreateDto> enity);
        Task<Result<string>> BulkDeleteHoliday(List<long> id);
        Task<Result<string>> CreateHoliday(HolidayCreateDto enity);
        Task<Result<string>> DeleteHoliday(long id);
        Task<bool> Existdate(DateTime setDate);
        Task<Result<Holiday>> GetByDate(DateTime? dt);
        Task<IEnumerable<Holiday>> GetDate(DateTime q);
        Task<Holiday?> GetHoliday(long id);
        Task<IEnumerable<Holiday>> GetHolidays(bool isSelectedPrevious, DateTime? startFrom = null, DateTime? end = null);
        Task<IEnumerable<Holiday>> GetHolidays(DateTime? selectedDate);
        Task<Pager<Holiday>> GetHolidays(int pageNumber, int pageSize, string sortColumn = "HolidayName", bool isSelectedPrevious = false, bool sortDescending = false, string? q = null);
        Task<Result<string>> ModifyHolidate(long id, HolidayUpdatesDto model);
    }

    public sealed class HolidayService(IHolidayService hService) : IHolidayServiceImpl
    {
        public async Task<Result<string>> BulkCreateHoliday(List<HolidayCreateDto> enities)
        {

            var req = enities.MapList<Holiday>();

            return await hService.BulkCreateHoliday(req);
        }

        public async Task<Result<string>> BulkDeleteHoliday(List<long> id)
        {
            return await hService.BulkDeleteHoliday(id);
        }

        public async Task<Result<string>> CreateHoliday(HolidayCreateDto enity)
        {
            var req = enity.Adapt<Holiday>();

            return await hService.CreateHoliday(req);
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

        public async Task<IEnumerable<Holiday>> GetHolidays(DateTime? selectedDate)
        {
            return await hService.GetHolidays(selectedDate!.Value);
        }

        public Task<Result<string>> ModifyHolidate(long id, HolidayUpdatesDto model)
        {
            throw new NotImplementedException();
        }
    }
}
