using Applications.Extensions.DateTimes;
using Applications.Extensions.Maths;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
namespace Applications.Features.Schedules;

public sealed class ScheduleByScheduleDocument(LoanResponse item) : IDocument
{
    const string font = "Khmer Os Siemreap";
    private LoanResponse Item => item ?? new();
    public string ImageUrl { get; set; } = string.Empty;
    public void Compose(IDocumentContainer document)
    {
        document.Page(page =>
        {
            page.Size(PageSizes.A4);

            page.Margin(0.5f, Unit.Millimetre);

            page.Header()
                .ShowOnce()
                .Element(header => HeaderCompose(header));
            page.Content()
                .Element(container => ContentCompose(container));
            page.Footer()
                .AlignCenter()
                .PaddingVertical(20)
                .Text(text =>
                {
                    text.DefaultTextStyle(TextStyle.Default.FontSize(16));
                    text.CurrentPageNumber();
                    text.Span("/");
                    text.TotalPages();
                });
        });
    }

    private void ContentCompose(IContainer container)
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
                columns.RelativeColumn();
            });

            table.Header(header =>
            {
                header.Cell().Element(SetHeaderCellStyle).Text("#").AlignCenter();
                header.Cell().Element(SetHeaderCellStyle).Text("Days").SemiBold().AlignCenter();
                header.Cell().Element(SetHeaderCellStyle).Text("Payment Date").SemiBold().AlignCenter();
                header.Cell().Element(SetHeaderCellStyle).AlignRight().Text("Principle").SemiBold();
                header.Cell().Element(SetHeaderCellStyle).AlignRight().Text("Interest").SemiBold();
                header.Cell().Element(SetHeaderCellStyle).AlignRight().Text("Total").SemiBold();
                header.Cell().Element(SetHeaderCellStyle).AlignRight().Text("Outstanding").SemiBold();
            });

            foreach (var item in Item!.Schedules ?? [])
            {
                table.Cell().Element(SetCellStyle).Text($"{item.InstallmentNo}").AlignCenter();
                table.Cell().Element(SetCellStyle).Text($"{item.TotalDay}").AlignCenter();
                table.Cell().Element(SetCellStyle).Text($"{item.PaymentDate.DayFirst()}").AlignCenter();
                table.Cell().Element(SetCellStyle).Text($"{item.Principle.Formats(item.CurrencyId)}").AlignRight();
                table.Cell().Element(SetCellStyle).Text($"{item.TotalFee.Formats(item.CurrencyId)}").AlignRight();
                table.Cell().Element(SetCellStyle).Text($"{item.TotalPay.Formats(item.CurrencyId)}").AlignRight();
                table.Cell().Element(SetCellStyle).Text($"{item.LoanOutstanding.Formats(item.CurrencyId)}").AlignRight();
            }
        });
    }

    private void HeaderCompose(IContainer header)
    {
        header.Row(row =>
        {

            row.RelativeItem().Column(col =>
            {

                col.Item()
                        .PaddingTop(10)
                         .Height(80)
                         .AlignCenter()
                         .Width(80)
                         .Image("wwwroot/assets/logo/tch_Logo.png");

                col.Item()
                        .PaddingBottom(5)
                        .PaddingTop(5)
                        .LineHorizontal(1)
                        .LineColor(Colors.Black);

                col.Item().Row(ro =>
                {
                    ro.RelativeItem().Column(co =>
                    {
                        AddColumnHeader(co, font, "ឈ្មោះ", Item.AltName);
                        AddColumnHeader(co, font, "រយៈពេល", Item.PeriodName);
                        AddColumnHeader(co, font, "អត្រា", $"{Item.InterestRate}");
                        AddColumnHeader(co, font, "មូលដ្ឋាន", $"{Item.OutStanding.Formats(Item.CurrencyId)}");
                        AddColumnHeader(co, font, "សាខា", Item.BranchName);
                    });
                    ro.RelativeItem().PaddingTop(5).Column(co =>
                    {
                        AddColumnHeader(co, font, "ថ្ងៃបង់ចុងក្រោយ", Item.LastPaymentDate.DayFirst());
                        AddColumnHeader(co, font, "អស៍យដ្ឋាន", Item.Address);
                        AddColumnHeader(co, font, "ភ្ងាក់ងារ", Item.CreditAgentKhmer);
                        AddColumnHeader(co, font, "ទូរស័ព្ទ", Item.AgentPhone);
                    });

                });

            });


        });
    }

    private static void AddColumnHeader(ColumnDescriptor col, string font, string? label, string? value)
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


