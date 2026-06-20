using Applications.Extensions.DateTimes;
using Applications.Extensions.Maths;
using Applications.Features.Loans.Dtos.Responses;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
namespace Applications.Features.Schedules;

public sealed class ScheduleByScheduleDocument(LoanResponse item) : IDocument
{
    private LoanResponse Item => item ?? new();
    public string ImageUrl { get; set; } = string.Empty;
    public void Compose(IDocumentContainer document)
    {
        document.Page(page =>
        {
            page.DefaultTextStyle(x => x.FontSize(10).FontFamily(ScheduleHelpers.FONT));
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
                    text.DefaultTextStyle(TextStyle.Default.FontSize(12));
                    text.CurrentPageNumber();
                    text.Span("/");
                    text.TotalPages();
                });
        });
    }

    private void ContentCompose(IContainer container)
    {

        container.PaddingTop(10).Column(cols =>
        {
            cols.Item().Element(ComposeTable);

            cols.Item().Row(r =>
            {
                r.RelativeItem().Column(col =>
                {
                    col.Item().PaddingTop(20).Row(r =>
                    {
                        r.RelativeItem()
                                .AlignCenter()
                                .Text(ScheduleHelpers.CUSTOMER_THUMB)
                                .FontFamily(ScheduleHelpers.FONT).SemiBold();
                        r.RelativeItem()
                                .AlignCenter()
                                .Text(ScheduleHelpers.AGENT_SIGNATURE)
                                .FontFamily(ScheduleHelpers.FONT).SemiBold();
                    });
                    col.Item().PaddingTop(100).Row(r =>
                    {
                        r.RelativeItem()
                          .AlignCenter()
                          .Column(c =>
                          {
                              c.Item()
                              .Width(120)
                              .LineHorizontal(1)
                              .LineDashPattern([1, 2])
                              .LineColor(Colors.Black);
                          });

                        r.RelativeItem()
                          .AlignCenter()
                          .Column(c =>
                          {
                              c.Item()
                              .Width(120)
                              .LineHorizontal(1)
                              .LineDashPattern([1, 2])
                              .LineColor(Colors.Black);
                          });
                    });
                    col.Item().PaddingTop(10).Row(r =>
                    {
                        r.RelativeItem().AlignCenter()
                                .Text(DateTime.Now.ToLongDateString())
                                .FontFamily(ScheduleHelpers.FONT).SemiBold();
                        r.RelativeItem().AlignCenter()
                                .Text(DateTime.Now.ToLongDateString())
                                .FontFamily(ScheduleHelpers.FONT).SemiBold();
                    });
                });

            });
        });


    }

    private void ComposeTable(IContainer container)
    {
        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.ConstantColumn(30);
                columns.RelativeColumn();
                columns.ConstantColumn(40);
                columns.RelativeColumn();
                columns.RelativeColumn();
                columns.RelativeColumn();
                columns.RelativeColumn();
            });

            table.Header(header =>
            {
                header.Cell().Element(SetHeaderCellStyle).Text("#").AlignCenter();
                header.Cell().ColumnSpan(2)
                            .Element(SetHeaderCellStyle)
                            .Text("Due Date")
                            .SemiBold()
                            .AlignCenter();
                header.Cell().Element(SetHeaderCellStyle).AlignRight().Text("ប្រាក់ដើមត្រូវបង់").SemiBold();
                header.Cell().Element(SetHeaderCellStyle).AlignRight().Text("ការប្រាក់").SemiBold();
                header.Cell().Element(SetHeaderCellStyle).AlignRight().Text("ប្រាក់បង់សរុប").SemiBold();
                header.Cell().Element(SetHeaderCellStyle).AlignRight().Text("ប្រាក់ដើមគ្រា").SemiBold();
            });

            foreach (var item in Item!.Schedules ?? [])
            {
                table.Cell().Element(SetCellStyle).Text($"{item.InstallmentNo}").AlignCenter();
                table.Cell().Element(SetCellStyle).Text($"{item.PaymentDate.DayFirst()}").AlignCenter();
                table.Cell().Element(SetCellStyle).Text($"{item.TotalDay}").AlignCenter();
                table.Cell().Element(SetCellStyle).Text($"{item.Principle.As(item.CurrencyId)}").AlignRight();
                table.Cell().Element(SetCellStyle).Text($"{item.TotalFee.As(item.CurrencyId)}").AlignRight();
                table.Cell().Element(SetCellStyle).Text($"{item.TotalPay.As(item.CurrencyId)}").AlignRight();
                table.Cell().Element(SetCellStyle).Text($"{item.LoanOutstanding.As(item.CurrencyId)}").AlignRight();
            }
        });
    }

    private void HeaderCompose(IContainer header)
    {
        header.Row(row =>
        {

            row.RelativeItem().Column(col =>
            {
                col.Item().Row(r =>
                {
                    r.RelativeItem()
                     .Width(80)
                     .Image("wwwroot/assets/logo/trapchum.jpg");
                    r.RelativeItem().AlignCenter().Column(col =>
                    {
                        col.Item().Text(ScheduleHelpers.COMPANY_NAME)
                        .AlignCenter()
                            .FontFamily(ScheduleHelpers.TITLE_FONT);
                        col.Item().AlignCenter()
                        .Text(ScheduleHelpers.SCHEDULE_TITLE).FontFamily(ScheduleHelpers.FONT);
                        col.Item().AlignCenter()
                        .Text(ScheduleHelpers.SCHEDULE_TITLE_KH).FontFamily(ScheduleHelpers.FONT);
                    });

                    r.RelativeItem().Text("");
                });

                col.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Black);
                CreateHeaderRow(col);
            });

        });
    }

    private void CreateHeaderRow(ColumnDescriptor col)
    {
        col.Item().Row(ro =>
        {
            ro.RelativeItem().Column(co =>
            {
                AddColumnHeader(co, ScheduleHelpers.FONT, ScheduleHelpers.NAME, Item.AltName);
                AddColumnHeader(co, ScheduleHelpers.FONT, ScheduleHelpers.PERIOD, Item.PeriodName);
                AddColumnHeader(co, ScheduleHelpers.FONT, ScheduleHelpers.RATE, $"{Item.InterestRate.AsPercent()}");
                AddColumnHeader(co, ScheduleHelpers.FONT, ScheduleHelpers.PRINCIPLE, $"{Item.OutStanding.Formats(Item.CurrencyId)}");
                AddColumnHeader(co, ScheduleHelpers.FONT, ScheduleHelpers.BRANCH, Item.BranchName);
            });
            ro.RelativeItem().PaddingTop(5).Column(co =>
            {
                AddColumnHeader(co, ScheduleHelpers.FONT, ScheduleHelpers.LAST_DUE_DATE, Item.LastPaymentDate.DayFirst());
                AddColumnHeader(co, ScheduleHelpers.FONT, ScheduleHelpers.ADDRESS, Item.Address);
                AddColumnHeader(co, ScheduleHelpers.FONT, ScheduleHelpers.AGENT, Item.CreditAgentKhmer);
                AddColumnHeader(co, ScheduleHelpers.FONT, ScheduleHelpers.PHONE, Item.AgentPhone);
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


