using Applications.Extensions.Maths;
using Applications.Responses.ViewModels.Loans.Schedules;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Applications.Schedules
{
    public sealed class SchedulePdfService : IDocument
    {
        public LoanScheduleVm Item { get; }

        public string? ImageUrl { get; set; } = string.Empty;


        public SchedulePdfService(LoanScheduleVm loan)
        {
            Item = loan;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);

                page.Margin(1F,Unit.Centimetre);

                page.Header().Element(ComposeHeader);

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
                x.Span($"Page {x.CurrentPageNumber} of {x.TotalPages}");
            });
        }

        private void ComposeContent(IContainer container)
        {
            container.PaddingVertical(10F).Column(col =>
            {
                col.Item().Element(CompostTable);
            });
        }

        private void CompostTable(IContainer container)
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
                    header.Cell().Element(HeaderCellStyle).Text("#").AlignCenter();
                    header.Cell().Element(HeaderCellStyle).Text("Days").SemiBold().AlignCenter();
                    header.Cell().Element(HeaderCellStyle).AlignRight().Text("Principle").SemiBold();
                    header.Cell().Element(HeaderCellStyle).AlignRight().Text("Interest").SemiBold();
                    header.Cell().Element(HeaderCellStyle).AlignRight().Text("Total").SemiBold();
                    header.Cell().Element(HeaderCellStyle).AlignRight().Text("Outstanding").SemiBold();
                    //header.Cell().ColumnSpan(6).PaddingTop(5).BorderBottom(1).BorderColor(Colors.Black);
                });

                //step 3 defining data

                foreach (var item in Item.LoanSchedules)
                {
                    table.Cell().Element(CellStyle).Text($"{item.InstallmentNo}").AlignCenter();
                    table.Cell().Element(CellStyle).Text($"{item.TotalDay}").AlignCenter();
                    table.Cell().Element(CellStyle).Text($"{item.Principal.Formats(Item.CurrencyId)}").AlignRight();
                    table.Cell().Element(CellStyle).Text($"{item.Interest.Formats(Item.CurrencyId)}").AlignRight();
                    table.Cell().Element(CellStyle).Text($"{item.TotalPay.Formats(Item.CurrencyId)}").AlignRight();
                    table.Cell().Element(CellStyle).Text($"{item.LoanOutstanding.Formats(Item.CurrencyId)}").AlignRight();
                }



            });
        }

        private void ComposeHeader(IContainer header)
        {

            header.Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    var scale = 0.9f;
                    col.Item().Scale(scale).Text($"Name      :{Item.FullName}");
                    col.Item().Scale(scale).Text($"Period    : {Item.PeriodName}");
                    col.Item().Scale(scale).Text($"Rate      :{Item.InterestRate}");
                    col.Item().Scale(scale).Text($"Principle :{Item.Principle.Formats(Item.CurrencyId)}");
                    col.Item().Scale(scale).Text($"Branch    :{Item.BranchName}");
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
                .Background(Colors.Grey.Medium)
                .BorderColor(Colors.Black)
                .PaddingHorizontal(3)
                .PaddingVertical(5);
        }

    }
}
