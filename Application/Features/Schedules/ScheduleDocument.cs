using Application.Extensions;
using Application.Extensions.Attrs;
using Application.Extensions.Maths;
using Application.Features.Reports.Dtos;
using Application.Responses.ViewModels.Reports;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
namespace Application.Features.Schedules;

public sealed class ScheduleDocument : IDocument
{
    public RptScheduleDetails Value { get; }
    public ScheduleDocument(RptScheduleDetails value)
    {
        Value = value;
    }
    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
    public DocumentSettings GetSettings() => DocumentSettings.Default;
    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Khmer Os Siemreap"));

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
                r.RelativeItem().AlignCenter().Text("ស្នាមមេដៃអតិថិជន").FontFamily("Khmer Os Siemreap").SemiBold();
                r.RelativeItem().AlignCenter().Text("ហត្ថលេខាអ្នករៀបចំ").FontFamily("Khmer Os Siemreap").SemiBold();
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
                r.RelativeItem().AlignCenter().Text(DateTime.Now.ToLongDateString()).FontFamily("Khmer Os Siemreap").SemiBold();
                r.RelativeItem().AlignCenter().Text(DateTime.Now.ToLongDateString()).FontFamily("Khmer Os Siemreap").SemiBold();
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
        header.Cell().Element(HeaderCellStyle).Text("ថ្អៃបង់ប្រាក់").FontFamily("Khmer Os Siemreap").SemiBold().AlignCenter();
        header.Cell().Element(HeaderCellStyle).Text("ចំនួនថ្ងៃ").FontFamily("Khmer Os Siemreap").SemiBold().AlignCenter();
        header.Cell().Element(HeaderCellStyle).AlignRight().Text("ប្រាក់ដើមត្រូវបង់").FontFamily("Khmer Os Siemreap").SemiBold();
        header.Cell().Element(HeaderCellStyle).AlignRight().Text("ការប្រាក់").FontFamily("Khmer Os Siemreap").SemiBold();
        header.Cell().Element(HeaderCellStyle).AlignRight().Text("ប្រាក់បង់សរុប").FontFamily("Khmer Os Siemreap").SemiBold();
        header.Cell().Element(HeaderCellStyle).AlignRight().Text("ប្រាក់ដើមនៅសល់").FontFamily("Khmer Os Siemreap").SemiBold();
    }

    private void InsertTableRow(TableDescriptor table, RptScheduleDetailsLine item)
    {
        table.Cell().Element(CellStyle).Text($"{item.InstallmentNo}").AlignCenter();
        table.Cell().Element(CellStyle).Text($"{item.PaymentDate.DayFirst()}").AlignCenter();
        table.Cell().Element(CellStyle).Text($"{item.TotalDay}").AlignCenter();
        table.Cell().Element(CellStyle).Text($"{item.Principle.As(Value.CurrencyId)}").AlignRight();
        table.Cell().Element(CellStyle).Text($"{item.Interest.As(Value.CurrencyId)}").AlignRight();
        table.Cell().Element(CellStyle).Text($"{item.TotalPay.As(Value.CurrencyId)}").SemiBold().AlignRight();
        table.Cell().Element(CellStyle).Text($"{item.LoanOutstanding.As(Value.CurrencyId)}").AlignRight();
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
                        .Text(ScheduleHelpers.COMPANY_NAME).FontFamily("Khmer Os Siemreap");
                        col.Item().AlignCenter()
                        .Text(ScheduleHelpers.SCHEDULE_TITLE_KH).FontFamily("Khmer Os Siemreap");
                    });

                    r.RelativeItem().Text("");
                });

                col.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Black);

                col.Item().Row(ro =>
                {
                    CreateHeaderRow(ro);
                });

            });

        });

    }

    private void CreateHeaderRow(RowDescriptor row)
    {

        row.ConstantItem(250).PaddingTop(10).Column(col =>
        {
            CreateColumn(col, ScheduleHelpers.NAME, $"{Value.AltName}", 10);
            CreateColumn(col, ScheduleHelpers.PERIOD, $"{Value.PeriodName}", 10);
            CreateColumn(col, ScheduleHelpers.RATE, $"{Value.InterestRate.AsPercent()}", 10);
            CreateColumn(col, ScheduleHelpers.PRINCIPLE, $"{Value.Principle.As(Value.CurrencyId)}", 10);
            CreateColumn(col, ScheduleHelpers.BRANCH, $"{Value.BranchName}", 10);
        });
        row.RelativeItem().PaddingTop(10).Column(col =>
        {
            CreateColumn(col, ScheduleHelpers.LAST_DUE_DATE, $"{Value.LastPaymentDate.DayFirst()}");
            CreateColumn(col, ScheduleHelpers.ADDRESS, $"{Value.Address}");
            CreateColumn(col, ScheduleHelpers.AGENT, $"{Value.CreditAgentKhmer}");
            CreateColumn(col, ScheduleHelpers.PHONE, $"{Value.AgentPhone}");
            CreateColumn(col, "", "");
        });
    }

    private void CreateColumn(ColumnDescriptor col, string label, object? value, float width = 10)
    {
        col.Item().Row(row =>
        {
            row.ConstantItem(30, Unit.Millimetre).Column(c =>
            {
                c.Item().Text(label).FontSize(10).FontFamily("Khmer Os Siemreap");
            });
            row.ConstantItem(2, Unit.Millimetre).Column(c =>
            {
                c.Item().Text(":").FontSize(10).FontFamily("Khmer Os Siemreap");
            });
            row.RelativeItem()
                    .ScaleToFit()
                    .Column(c =>
            {
                c.Item()
                    .ScaleToFit()
                    .Text($"{value}")
                    .FontSize(10).FontFamily("Khmer Os Siemreap");
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
