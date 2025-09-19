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



    }
}
