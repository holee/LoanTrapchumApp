using Applications.Features.Transactions.CashManagements;
using MiniExcelLibs;
namespace TrapchumApp.Services
{
    public sealed class ExcelImportService
    {

        public IEnumerable<CashManagementDetailDtoCreate> GetCashValaultIn(Stream stream)
        {
            if (stream is null || stream.Length == 0)
            {
                return Enumerable.Empty<CashManagementDetailDtoCreate>();
            }

            var items = stream.Query<CashManagementDetailDtoCreate>();

            return items;
        }

        public async Task<IEnumerable<CashManagementDetailDtoCreate>> GetCashValaultInFromExcel(Stream stream)
        {
            if (stream is null)
                return Enumerable.Empty<CashManagementDetailDtoCreate>(); 

            using var memoryStream = new MemoryStream();

            await stream.CopyToAsync(memoryStream);

            memoryStream.Position = 0;

            var items = memoryStream.Query<CashManagementDetailDtoCreate>();

            return items.ToList();
        }



    }
}
