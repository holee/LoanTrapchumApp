using Applications.Extensions.DateTimes;
using Applications.Extensions.Maths;
using Applications.Responses.ViewModels.Reports;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
namespace Applications.Features.Schedules;

public sealed class ScheduleDocument : IDocument
{
    public DisbursmentsRpt Item { get; }
    public VLoanDisbursments Value { get; }
    public ScheduleDocument(VLoanDisbursments value)
    {
        Item = new();

        Value = value;
    }

    public ScheduleDocument(DisbursmentsRpt loan)
    {
        Item = loan;
        Value = new();
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
    public DocumentSettings GetSettings() => DocumentSettings.Default;
    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Size(PageSizes.A4);

            page.Margin(10);

            page.Header().ShowOnce().Element(ComposeHeader);

            page.Content().Element(ComposeContent);

            page.Footer().Element(ComposeFooter);
        });
    }

    private void ComposeFooter(IContainer container)
    {
        container
        .AlignCenter()
        .Text(x =>
        {
            x.Span("Thank you for your business! ");
            x.Span("Page");
            x.CurrentPageNumber();
            x.Span(" / ");
            x.TotalPages();
        });
    }

    private void ComposeContent(IContainer container)
    {
        container.PaddingVertical(10F).Column(col =>
        {
            col.Item().Element(CompostTable);
         
            col.Item().PaddingTop(20).Row(r =>
            {
                r.RelativeItem().AlignCenter().Text("ស្នាមមេដៃអតិថិជន").FontFamily(ScheduleHelpers.FONT).SemiBold();
                r.RelativeItem().AlignCenter().Text("ហត្ថលេខាអ្នករៀបចំ").FontFamily(ScheduleHelpers.FONT).SemiBold();
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
                r.RelativeItem().AlignCenter().Text(DateTime.Now.ToLongDateString()).FontFamily(ScheduleHelpers.FONT).SemiBold();
                r.RelativeItem().AlignCenter().Text(DateTime.Now.ToLongDateString()).FontFamily(ScheduleHelpers.FONT).SemiBold();
            });
        });
    }

    private void CompostTable(IContainer container)
    {
        container.Table(table =>
        {
            table.ColumnsDefinition(columns => AddTableHeader(columns));
            table.Header(header => AddTableHeaderCell(header));
            AddTableData(table);
        });
    }

    private void AddTableData(TableDescriptor table)
    {
        foreach (var item in Value.Schedules ?? [])
        {
            InsertTableRow(table, item);
        }
    }

    private static void AddTableHeader(TableColumnsDefinitionDescriptor columns)
    {
        columns.ConstantColumn(30);
        columns.RelativeColumn();
        columns.ConstantColumn(40);
        columns.RelativeColumn();
        columns.RelativeColumn();
        columns.RelativeColumn();
        columns.RelativeColumn();
    }

    private void AddTableHeaderCell(TableCellDescriptor header)
    {
        header.Cell().Element(HeaderCellStyle).Text("#").AlignCenter();
        header.Cell().Element(HeaderCellStyle).Text("Payment Date").SemiBold().AlignCenter();
        header.Cell().Element(HeaderCellStyle).Text("Days").SemiBold().AlignCenter();
        header.Cell().Element(HeaderCellStyle).AlignRight().Text("Principle").SemiBold();
        header.Cell().Element(HeaderCellStyle).AlignRight().Text("Interest").SemiBold();
        header.Cell().Element(HeaderCellStyle).AlignRight().Text("Total").SemiBold();
        header.Cell().Element(HeaderCellStyle).AlignRight().Text("Outstanding").SemiBold();
    }

    private void InsertTableRow(TableDescriptor table, VSchedule item)
    {
        table.Cell().Element(CellStyle).Text($"{item.InstallmentNo}").AlignCenter();
        table.Cell().Element(CellStyle).Text($"{item.PaymentDate.DayFirst()}").AlignCenter();
        table.Cell().Element(CellStyle).Text($"{item.TotalDay}").AlignCenter();
        table.Cell().Element(CellStyle).Text($"{item.Principle.Formats(Value.CurrencyId)}").AlignRight();
        table.Cell().Element(CellStyle).Text($"{item.TotalFee.Formats(Value.CurrencyId)}").AlignRight();
        table.Cell().Element(CellStyle).Text($"{item.TotalPay.Formats(Value.CurrencyId)}").SemiBold().AlignRight();
        table.Cell().Element(CellStyle).Text($"{item.LoanOutstanding.Formats(Value.CurrencyId)}").AlignRight();
    }

    private void ComposeHeader(IContainer header)
    {
        //float scale = 0.9f;
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
                        col.Item().Text(ScheduleHelpers.SCHEDULE_TITLE)
                        .AlignCenter()
                            .FontFamily(ScheduleHelpers.TITLE_FONT);
                        col.Item().AlignCenter()
                        .Text(ScheduleHelpers.COMPANY_NAME).FontFamily(ScheduleHelpers.FONT);
                        col.Item().AlignCenter()
                        .Text(ScheduleHelpers.SCHEDULE_TITLE_KH).FontFamily(ScheduleHelpers.FONT);
                    });

                    r.RelativeItem().Text("");
                });

                col.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Black);

                col.Item().Row(ro =>
                {
                    CreateHeaderRow(ro, ScheduleHelpers.FONT);
                });

            });

        });

    }

    private void CreateHeaderRow(RowDescriptor row, string font)
    {

        row.ConstantItem(250).PaddingTop(10).Column(col =>
        {
            CreateColumn(col, ScheduleHelpers.NAME, $"{Value.AltName}", font, 10);
            CreateColumn(col, ScheduleHelpers.PERIOD, $"{Value.PeriodName}", font, 10);
            CreateColumn(col, ScheduleHelpers.RATE, $"{Value.PaidInterest.Formats(Value.CurrencyId)}", font, 10);
            CreateColumn(col, ScheduleHelpers.PRINCIPLE, $"{Value.Principle.Formats(Value.CurrencyId)}", font, 10);
            CreateColumn(col, ScheduleHelpers.BRANCH, $"{Value.BranchName}", font, 10);
        });
        row.RelativeItem().PaddingTop(10).Column(col =>
        {
            CreateColumn(col, ScheduleHelpers.LAST_DUE_DATE, $"{Value.LastPaymentDate.DayFirst()}", font);
            CreateColumn(col, ScheduleHelpers.ADDRESS, $"{Value.Address}", font);
            CreateColumn(col, ScheduleHelpers.AGENT, $"{Value.AgentName}", font);
            CreateColumn(col, ScheduleHelpers.PHONE, $"{Value.AgentPhone}", font);
            CreateColumn(col, "", "", font);
        });
    }

    private void CreateColumn(ColumnDescriptor col, string label, object? value, string font, float width = 1)
    {
        col.Item().Row(row =>
        {
            row.ConstantItem(30, Unit.Millimetre).Column(c =>
            {
                c.Item().Text(label).FontFamily(font);
            });
            row.ConstantItem(2, Unit.Millimetre).Column(c =>
            {
                c.Item().Text(":").FontFamily(font);
            });
            row.RelativeItem()
                    .ScaleToFit()
                    .Column(c =>
            {
                c.Item()
                    .ScaleToFit()
                    .Text($"{value}")
                    .FontFamily(font);
            });
        });

    }


    private IContainer CellStyle(IContainer container)
    {
        return container
           .Border(0.5F)
            .BorderColor(Colors.Black)
            .PaddingHorizontal(3)
            .PaddingVertical(5);
    }

    private IContainer HeaderCellStyle(IContainer container)
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
