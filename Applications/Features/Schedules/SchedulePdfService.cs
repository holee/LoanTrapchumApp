using Applications.Extensions.DateTimes;
using Applications.Extensions.Maths;
using Applications.Responses.ViewModels.Loans;
using Applications.Responses.ViewModels.Reports;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Applications.Features.Schedules
{
    public sealed class SchedulePdfService : IDocument
    {
        public DisbursmentsRpt Item { get; }
        public VLoanDisbursments Value { get; }

        public SchedulePdfService(VLoanDisbursments value)
        {
            Item = new();
            Value = value;
        }

        public string? ImageUrl { get; set; } = string.Empty;


        public SchedulePdfService(DisbursmentsRpt loan)
        {
            Item = loan;
            Value = new();
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);

                page.Margin(1F, Unit.Centimetre);

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
                });

                //step 3 defining data

                foreach (var item in Value.Schedules ?? [])
                {
                    table.Cell().Element(CellStyle).Text($"{item.InstallmentNo}").AlignCenter();
                    table.Cell().Element(CellStyle).Text($"{item.TotalDay}").AlignCenter();
                    table.Cell().Element(CellStyle).Text($"{item.Principle.Formats(Value.CurrencyId)}").AlignRight();
                    table.Cell().Element(CellStyle).Text($"{item.Interest.Formats(Value.CurrencyId)}").AlignRight();
                    table.Cell().Element(CellStyle).Text($"{item.TotalPay.Formats(Value.CurrencyId)}").AlignRight();
                    table.Cell().Element(CellStyle).Text($"{item.LoanOutstanding.Formats(Value.CurrencyId)}").AlignRight();
                }



            });
        }

        private void ComposeHeader(IContainer header)
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
                            c1.Item().Scale(scale).Text($"{Value.AltName}").FontFamily(font);
                            c1.Item().Scale(scale).Text($"{Value.PeriodName}").FontFamily(font);
                            c1.Item().Scale(scale).Text($"{Value.PaidInterest.Formats(Value.CurrencyId)}");
                            c1.Item().Scale(scale).Text($"{Value.Principle.Formats(Value.CurrencyId)}");
                            c1.Item().Scale(scale).Text($"{Value.BranchName}").FontFamily(font);
                        });

                    });                

                });

                row.RelativeItem().Column(col =>
                {
                    var font = "Khmer Os Siemreap";
                    var scale = 0.9f;
                    col.Item().Row(r1 =>
                    {
                        r1.ConstantItem(2.8F,Unit.Centimetre).Column(c1 =>
                        {
                            c1.Item().Scale(scale).Text($"ថ្ងៃបង់ចុងក្រោយ").FontFamily(font);
                            c1.Item().Scale(scale).Text($"អស៍យដ្ឋាន").FontFamily(font);
                            c1.Item().Scale(scale).Text($"ភ្ងាក់ងារ").FontFamily(font);
                            c1.Item().Scale(scale).Text($"ទូរស័ព្ទ").FontFamily(font);
                        });
                        r1.RelativeItem().Column(c1 =>
                        {
                            c1.Item().Scale(scale).Text($"{Value.LastPaymentDate.DayFirst()}");
                            c1.Item().Scale(scale).Text($"{Value.Address}").FontFamily(font);
                            c1.Item().Scale(scale).Text($"{Value.AgentName}").FontFamily(font);
                            c1.Item().Scale(scale).Text($"{Value.AgentPhone}");
                        });
                    });
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

        //private static IContainer LabelRow(string label, string value, string font, float scale = 0.9f)
        //{
        //    return IContainer().Scale(scale).Row(row =>
        //    {
        //        row.ConstantItem(100).Text($"{label} :").FontFamily(font);
        //        row.RelativeItem().Text(value).FontFamily(font);
        //    });
        //}

    }
}
