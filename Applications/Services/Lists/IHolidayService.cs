using Applications.Extensions.mappers;
using Applications.Features.Holidays.Dtos;
using Applications.Interfaces.Lists;
using Applications.Responses;
using Applications.Responses.Paginations;
using Domains.Models.Lists;
using Mapster;

namespace Applications.Services.Lists
{
    public interface IHolidayServiceImpl
    {
        // Task<Result<string>> BulkCreateHoliday(List<HolidayCreateDto> enity);
        //Task<Result<string>> BulkDeleteHoliday(List<long> id);
        //Task<Result<string>> CreateOrUpdateHoliday(HolidayCreateDto enity);
        //Task<Result<IEnumerable<HolidayVm>>> CreateHolidays(IEnumerable<HolidayCreateDto> enities);
        //Task<Result<string>> DeleteHoliday(long id);
        Task<bool> Existdate(DateTime setDate);
       // Task<Result<Holiday>> GetByDate(DateTime? dt);
        //Task<IEnumerable<Holiday>> GetDate(DateTime q);
        //Task<Result<Holiday>> GetHoliday(long id);
        Task<IEnumerable<Holiday>> GetHolidays(bool isSelectedPrevious, DateTime? startFrom = null, DateTime? end = null);
        //Task<IEnumerable<HolidayVm>> GetHolidays(DateTime? selectedDate);
        //Task<Pager<Holiday>> GetHolidays(int pageNumber, int pageSize, string sortColumn = "HolidayName", bool isSelectedPrevious = false, bool sortDescending = false, string? q = null);
        Task<IEnumerable<HolidayDto>> GetActiveHolidays(DateTime? startFrom, int? selectedYear);
        //Task<Result<string>> ModifyHolidate(long id, HolidayCreateDto model);
    }

    public sealed class HolidayService(IHolidayService hService) : IHolidayServiceImpl
    {
        //public async Task<Result<string>> BulkCreateHoliday(List<HolidayCreateDto> enities)
        //{

        //    var req = enities.MapList<Holiday>();

        //    return await hService.BulkCreateHoliday(req);
        //}

        //public async Task<Result<string>> BulkDeleteHoliday(List<long> id)
        //{
        //    return await hService.BulkDeleteHoliday(id);
        //}

        //public async Task<Result<string>> CreateOrUpdateHoliday(HolidayCreateDto enity)
        //{
        //    var req = enity.ToHoliday();

        //    return await hService.CreateOrUpdateHoliday(req);
        //}

        //public async Task<Result<IEnumerable<HolidayVm>>> CreateHolidays(IEnumerable<HolidayCreateDto> enities)
        //{
        //    var req = enities.ToHolidays();

        //    return await hService.CreateHolidays(req);
        //}



        //public async Task<Result<string>> DeleteHoliday(long id)
        //{
        //    return await hService.DeleteHoliday(id);
        //}

        public async Task<bool> Existdate(DateTime setDate)
        {
            return await hService.ExistHolidays(setDate);
        }

        //public async Task<Result<Holiday>> GetByDate(DateTime? dt)
        //{
        //    return await hService.GetByDate(dt);
        //}

        //public async Task<IEnumerable<Holiday>> GetDate(DateTime q)
        //{
        //    return await hService.GetDate(q);
        //}

        //public async Task<Result<Holiday>> GetHoliday(long id)
        //{
        //    return await hService.GetHoliday(id);
        //}

        //public async Task<Pager<Holiday>> GetHolidays(int pageNumber, int pageSize, string sortColumn = "HolidayName", bool isSelectedPrevious = false, bool sortDescending = false, string? q = null)
        //{
        //    return await hService.GetHolidays(pageNumber, pageSize, sortColumn, isSelectedPrevious, sortDescending, q);

        //}

        public async Task<IEnumerable<Holiday>> GetHolidays(bool isSelectedPrevious = false, DateTime? startFrom = null, DateTime? end = null)
        {


            return await hService.GetHolidays(isSelectedPrevious, startFrom, end);
        }


        public async Task<IEnumerable<HolidayDto>> GetActiveHolidays(DateTime? startFrom, int? selectedYear)
        {
            var startDate = startFrom < DateTime.Now ? DateTime.Now : startFrom;

            return await hService.GetActiveHolidays(startDate, selectedYear);
        }

        //public async Task<IEnumerable<HolidayVm>> GetHolidays(DateTime? selectedDate)
        //{
        //    return await hService.GetHolidays(selectedDate);
        //}

        //public Task<Result<string>> ModifyHolidate(long id, HolidayCreateDto model)
        //{
        //    return await hService.ExistHolidays (selectedDate);
        //}
    }
}
