using Applications.Constants.Others;
using Applications.Responses.ViewModels.Options;
using Domains.Models.Identity;

namespace Applications.Constants
{
    public class StorageConstant
    {
        public const long UploadMaxSize = 30_000_000;
        public const string AuthToken = "bearerToken";
        public const string RefreshToken = "refreshToken";
        public static IReadOnlyList<Salutations> Salutations = [
             new("mr","Mr."),
             new("ms","Ms"),
             new("miss","Miss"),
             new("female","Female"),
             new("male","Male"),
             new("Other","Other")
            ];
        public static IList<Items> PageSizes = [
             new(10,"10"),
             new(25,"25"),
             new(50,"50"),
             new(100,"100")
            ];

        public static IList<Items> Currencies = [
             new(0,"All"),
             new(1,"KHR"),
             new(2,"USD"),
           ];

        public static IReadOnlyList<Items> CurrencyTpes = [
                    new(1,"KHR"),
                    new(2,"USD"),
            ];
        public static IReadOnlyList<Items> CollectionFeeModes = [
                    new(1,"IRR"),
                    new(2,"Decline"),
            ];
        public static IReadOnlyList<Items> PeriodNames = [
                new (1,"Months"),
                new (2,"Weeks"),
                new (3,"Days"),
                new (4,"Years"),
             ];
        public static IReadOnlyList<AprovedLevel> AprovedLevels = [
                new (1,"CO"),
                new (2,"Branch Manager"),
                new (3,"General Manager"),
                new (4,"Counsil"),
             ];
        public static IReadOnlyList<Items> RepaymentModes = [
                new (1,"Balloon"),
                new (2,"Decline"),
                new (3,"Flat"),
                new (4,"Annuarity"),
             ];
        public static IReadOnlyList<Items> InstallmentTypes = [
                new (1,"Monthly"),
                new (2,"2 Weekly"),
                new (3,"Weekly"),
                new (4,"Daily"),
             ];

        public static IEnumerable<Items> ExchangedTypes = [
            new (1, "Buy Currency"),
            new (2, "Sell Currency")
            ];

        public static int[] InstallemntOptions = [12, 18, 24, 36, 42, 48, 60, 72];

        public static int[] PageSizeOptions = [15, 25, 50, 100];

        public static string[] MarritalOptions = ["Single", "Married", "Divorced", "Widow"];

        public static IReadOnlyList<Items> RepaymentModeOptions = [
                new (1,"Balloon"),
                new (2,"Decline"),
                new (3,"Flat"),
                new (4,"Annuarity"),
             ];
        public static IReadOnlyList<Items> CurrencyTypeOptions = [
                    new(1,"KHR"),
                    new(2,"USD"),
            ];

        public static IReadOnlyList<string> TypeOfOptions = ["Job", "Position"];
    }
}
