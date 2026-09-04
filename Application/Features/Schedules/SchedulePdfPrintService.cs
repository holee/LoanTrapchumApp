using Application.Extensions;
using Application.Extensions.Attrs;
using Application.Extensions.Maths;
using Application.Features.Loans.Dtos.Responses;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
namespace Application.Features.Schedules;

public sealed class SchedulePdfPrintService
{
    public static byte[] CreatePdf(IEnumerable<LoanResponse> args)
    {
        List<IDocument> documents = new List<IDocument>();

        foreach (LoanResponse response in args ?? [])
        {
            documents.Add(GenerateDocument(response));
        }

        return QuestPDF.Fluent.Document.Merge(documents).GeneratePdf();
    }

    public static byte[] CreatePdf(LoanResponse arg)
    {

        var doc = GenerateDocument(arg);

        return Document.Merge(doc).GeneratePdf();
    }


    public static string CreatePdf(LoanResponse arg, string path)
    {

        var doc = GenerateDocument(arg);

        Document.Merge(doc).GeneratePdf(path);

        return path;
    }


    public static Document GenerateDocument(LoanResponse Item)
    {
        return Document.Create(document =>
        {
            document.Page(page =>
            {
                page.Size(PageSizes.A4);

                page.Margin(0.5f, Unit.Millimetre);

                page.Header()
                    .Background(Colors.Grey.Lighten2)
                    .Padding(10)
                    .Element(header => HeaderCompose(Item, header));
                page.Content()
                    .PaddingTop(5)
                    .Element(container => ContentCompose(Item, container));
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

    private static void ContentCompose(LoanResponse Item, IContainer container)
    {
        container.Table(table =>
        {

            table.ColumnsDefinition(columns =>
            {
                columns.ConstantColumn(30);
                columns.ConstantColumn(35);
                columns.RelativeColumn();
                columns.RelativeColumn();
                columns.RelativeColumn();
                columns.RelativeColumn();
            });

            table.Header(header =>
            {
                header.Cell().Element(SetHeaderCellStyle).Text("#").AlignCenter();
                header.Cell().Element(SetHeaderCellStyle).Text("Days").SemiBold().AlignCenter();
                header.Cell().Element(SetHeaderCellStyle).AlignRight().Text("Principle").SemiBold();
                header.Cell().Element(SetHeaderCellStyle).AlignRight().Text("Interest").SemiBold();
                header.Cell().Element(SetHeaderCellStyle).AlignRight().Text("Total").SemiBold();
                header.Cell().Element(SetHeaderCellStyle).AlignRight().Text("Outstanding").SemiBold();
            });

            foreach (var item in Item.Schedules ?? [])
            {
                table.Cell().Element(SetCellStyle).Text($"{item.ScheduleNum}").AlignCenter();
                table.Cell().Element(SetCellStyle).Text($"{item.TotalDay}").AlignCenter();
                table.Cell().Element(SetCellStyle).Text($"{item.Principle.Formats(item.CurrencyId)}").AlignRight();
                table.Cell().Element(SetCellStyle).Text($"{item.TotalFee.Formats(item.CurrencyId)}").AlignRight();
                table.Cell().Element(SetCellStyle).Text($"{item.TotalPay.Formats(item.CurrencyId)}").AlignRight();
                table.Cell().Element(SetCellStyle).Text($"{item.LoanOutstanding.Formats(item.CurrencyId)}").AlignRight();
            }
        });
    }

    private static void HeaderCompose(LoanResponse Item, IContainer header)
    {
        const string font = "Khmer Os Siemreap";
        const float scale = 0.9f;
        header.Row(row =>
        {
            row.RelativeItem().Column(col =>
            {
                AddColumnHeader(col, font, scale, "Name", Item.AltName);
                AddColumnHeader(col, font, scale, "Period", Item.PeriodName);
                AddColumnHeader(col, font, scale, "Rate", $"{Item.InterestRate}");
                AddColumnHeader(col, font, scale, "Principle", $"{Item.OutStanding.Formats(Item.CurrencyId)}");
                AddColumnHeader(col, font, scale, "Branch", Item.BranchName);
            });
            row.RelativeItem().PaddingTop(5).Column(col =>
            {
                AddColumnHeader(col, font, scale, "ថ្ងៃបង់ចុងក្រោយ", Item.LastPaymentDate.DayFirst());
                AddColumnHeader(col, font, scale, "អស៍យដ្ឋាន", Item.Address);
                AddColumnHeader(col, font, scale, "ភ្ងាក់ងារ", Item.CreditAgentKhmer);
                AddColumnHeader(col, font, scale, "ទូរស័ព្ទ", Item.AgentPhone);
            });
        });
    }

    private static void AddColumnHeader(ColumnDescriptor col, string font, float scale, string? label, string? value)
    {
        col.Item().Row(row =>
        {
            row.ConstantItem(30, Unit.Millimetre).Column(c1 =>
            {
                c1.Item().Text(label).FontFamily(font);
            });
            row.ConstantItem(2, Unit.Millimetre).Column(c1 =>
            {
                c1.Item().Text(":").FontFamily(font);
            });
            row.RelativeItem().Column(c1 =>
            {
                c1.Item().Text($"{value}").FontFamily(font);
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
            .Background(Colors.Grey.Lighten1)
            .BorderColor(Colors.Black)
            .PaddingHorizontal(3)
            .PaddingVertical(5);
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
