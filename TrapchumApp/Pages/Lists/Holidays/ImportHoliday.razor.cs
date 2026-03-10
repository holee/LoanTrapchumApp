//using MiniExcelLibs;

using Applications.Features.Holidays.Dtos;

namespace TrapchumApp.Pages.Lists.Holidays
{
    public partial class ImportHoliday
    {
        private List<HolidayCreateDto> model = new List<HolidayCreateDto>();
        private HolidayCreateDto modelDTO = new();
        private async Task setDate()
        {
            //string setdate = modelDTO.HolidayName.ToString("yyyy-MM-dd");
            //Console.WriteLine(setdate);
            //var existdate = await service.ExistSetDate(setdate);
            //Console.WriteLine(existdate);
            //if (existdate == true)
            //{
            //    bool checkDT = checkdt(modelDTO.HolidayName);
            //    var a = Model.Find(x => x.HolidayName == modelDTO.HolidayName);
            //    if (checkDT == true && a == null)
            //    {

            //    }
            //    else
            //    {
            //        toaster?.Warning("Please check date!", "Message", config =>
            //        {
            //            config.ShowProgressBar = false;
            //            config.ShowCloseIcon = true;
            //        });

            //    }
            //}
            //else
            //{
            //    toaster?.Warning("This date already register", "Message", config =>
            //    {
            //        config.ShowCloseIcon = true;
            //        config.ShowProgressBar = false;
            //    });
            //}
            model.Add(new HolidayCreateDto
            {
                Id = modelDTO.Id,
                HolidayName = modelDTO.HolidayName,
                Description = modelDTO.Description
            });
            await Task.CompletedTask;
        }
        private void DeleteEvent(HolidayCreateDto modelV)
        {
            model?.Remove(modelV);
        }
        //private void readexcel() => Model = MiniExcel.Query<HolidayCreateDtos>("readexcel.xlsx").ToList();

    }
}