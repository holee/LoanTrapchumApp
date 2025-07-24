using Applications.Extensions;
using Applications.Extensions.DateTimes;
using Applications.Extensions.Maths;
using Applications.Features.Loans.Responses;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Applications.Features.Loans.Services
{
    public sealed class SchedulePdfPrintService
    {
        public static byte[] CreatePdf(IEnumerable<LoanScheduleResponse> args)
        {
            List<IDocument> documents = new List<IDocument>();

            foreach (LoanScheduleResponse response in args ?? [])
            {
                documents.Add(GenerateDocument(response));
            }

            return Document.Merge(documents).GeneratePdf();
        }

        public static byte[] CreatePdf(LoanScheduleResponse arg)
        {

            var doc = GenerateDocument(arg);

            return Document.Merge(doc).GeneratePdf();
        }


        public static string CreatePdf(LoanScheduleResponse arg, string path)
        {

            var doc = GenerateDocument(arg);

            Document.Merge(doc).GeneratePdf(path);

            return path;
        }


        public static Document GenerateDocument(LoanScheduleResponse Item)
        {
            return Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Size(PageSizes.A4);

                    page.Margin(0.5f, Unit.Inch);

                    page.Header()
                        .Element(header =>
                        {
                            HeaderCompose(Item, header);
                        });

                    page.Content()
                        .Element(container =>
                        {
                            ContentCompose(Item, container);
                        });

                    page.Footer()
                        .AlignCenter()
                        .PaddingVertical(20)
                        .Text(text =>
                        {
                            text.DefaultTextStyle(TextStyle.Default.FontSize(16));
                            text.CurrentPageNumber();
                            text.Span(" / ");
                            text.TotalPages();
                        });
                });
            });
        }

        private static void ContentCompose(LoanScheduleResponse Item, IContainer container)
        {
            container.Table(table =>
            {
                //step 1 defining columns

                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(30);
                    columns.ConstantColumn(35);
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });


                //step 2 defining header

                table.Header(header =>
                {
                    header.Cell().Element(SetHeaderCellStyle).Text("#").AlignCenter();
                    header.Cell().Element(SetHeaderCellStyle).Text("Days").SemiBold().AlignCenter();
                    header.Cell().Element(SetHeaderCellStyle).AlignRight().Text("Principle").SemiBold();
                    header.Cell().Element(SetHeaderCellStyle).AlignRight().Text("Interest").SemiBold();
                    header.Cell().Element(SetHeaderCellStyle).AlignRight().Text("Total").SemiBold();
                    header.Cell().Element(SetHeaderCellStyle).AlignRight().Text("Outstanding").SemiBold();
                });

                //step 3 defining data

                foreach (var item in Item.Schedules ?? [])
                {
                    table.Cell().Element(SetCellStyle).Text($"{item.ScheduleNum}").AlignCenter();
                    table.Cell().Element(SetCellStyle).Text($"{item.TotalDay}").AlignCenter();
                    table.Cell().Element(SetCellStyle).Text($"{item.Principle.Formats(item.CurrencyId)}").AlignRight();
                    table.Cell().Element(SetCellStyle).Text($"{item.Interest.Formats(item.CurrencyId)}").AlignRight();
                    table.Cell().Element(SetCellStyle).Text($"{item.TotalPay.Formats(item.CurrencyId)}").AlignRight();
                    table.Cell().Element(SetCellStyle).Text($"{item.LoanOutstanding.Formats(item.CurrencyId)}").AlignRight();
                }
            });
        }

        private static void HeaderCompose(LoanScheduleResponse Item, IContainer header)
        {
            header.Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    var font = "Khmer Os Siemreap";
                    var scale = 0.9f;
                    col.Item().Row(r1 =>
                    {
                        r1.RelativeItem().Column(c1 =>
                        {
                            c1.Item().Scale(scale).Text($"Name").FontFamily(font);
                            c1.Item().Scale(scale).Text($"Period").FontFamily(font);
                            c1.Item().Scale(scale).Text($"Rate").FontFamily(font);
                            c1.Item().Scale(scale).Text($"Principle").FontFamily(font);
                            c1.Item().Scale(scale).Text($"Branch").FontFamily(font);
                        });
                        r1.RelativeItem().Column(c1 =>
                        {
                            c1.Item().Scale(scale).Text($"{Item.AltName}").FontFamily(font);
                            c1.Item().Scale(scale).Text($"{Item.PeriodName}").FontFamily(font);
                            c1.Item().Scale(scale).Text($"{Item.InterestRate}").FontFamily(font);
                            c1.Item().Scale(scale).Text($"{Item.OutStanding.Formats(Item.CurrencyId)}");
                            c1.Item().Scale(scale).Text($"{Item.BranchName}").FontFamily(font);
                        });

                    });

                });

                row.RelativeItem().Column(col =>
                {
                    const string font = "Khmer Os Siemreap";
                    const float scale = 0.9f;
                    const float size = 2.8F;
                    col.Item().Row(r1 =>
                    {
                        r1.ConstantItem(size, Unit.Centimetre).Column(c1 =>
                        {
                            c1.Label("ថ្ងៃបង់ចុងក្រោយ");
                            c1.Label("អស៍យដ្ឋាន");
                            c1.Item().Scale(scale).Text($"ភ្ងាក់ងារ").FontFamily(font);
                            c1.Item().Scale(scale).Text($"ទូរស័ព្ទ").FontFamily(font);
                        });
                        r1.RelativeItem().Column(c1 =>
                        {
                            c1.Value(Item.LastPaymentDate.DayFirst());
                            c1.Value(Item.Address);
                            c1.Item().Scale(scale).Text($"{Item.CreditAgentKhmer}").FontFamily(font);
                            c1.Item().Scale(scale).Text($"{Item.AgentPhone}");
                        });
                    });
                });
            });
        }

        private static IContainer SetCellStyle(IContainer container)
        {
            return container
               .Border(0.5F)
                .BorderColor(Colors.Black)
                .PaddingHorizontal(3)
                .PaddingVertical(5);
        }

        private static IContainer SetHeaderCellStyle(IContainer container)
        {
            return container
                .Border(0.5F)
                .BorderColor(Colors.Black)
                .Background(Colors.Grey.Medium)
                .BorderColor(Colors.Black)
                .PaddingHorizontal(3)
                .PaddingVertical(5);
        }
    }
}





//public void Compose(IDocumentContainer container,LoanScheduleResponse model)
//{

//        container.Page(page =>
//        {
//            page.Size(PageSizes.A4);

//            page.Margin(1F, Unit.Centimetre);

//            page.Header().Element(config =>
//            {
//                ComposeHeader(config, model);
//            });

//            page.Content().Element(config =>
//            {
//                ComposeContent(config,model);
//            });

//            page.Footer().Element(ComposeFooter);
//        });
//    }
