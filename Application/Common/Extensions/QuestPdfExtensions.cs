using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using static System.Net.Mime.MediaTypeNames;

namespace Application.Extensions
{
    // Extension methods to format cells
    public static class QuestPdfExtensions
    {
        public static TextBlockDescriptor LabelCell(this IContainer container, string text) =>
            container.PaddingVertical(2).Text(text).SemiBold().FontSize(10);

        public static TextBlockDescriptor ValueCell(this IContainer container, string? text) =>
            container.PaddingVertical(2).Text(text ?? "-").FontSize(10);

        public static TextBlockDescriptor HeaderCell(this IContainer container, string text) =>
            container.Padding(2).Background(Colors.Grey.Lighten2).Border(1)
                     .Text(text).SemiBold().FontSize(10).AlignCenter();

        public static TextBlockDescriptor Label(this ColumnDescriptor container, string text, string font = "Khmer Os Siemreap", float scale = 0.9f)
        {
            return container.Item().Scale(scale).Text($"{text}").FontFamily(font);
        }

        public static TextBlockDescriptor Value(this ColumnDescriptor container, object? value, string font = "Khmer Os Siemreap", float scale = 0.9f)
        {
            return container.Item().Scale(scale).Text($"{value}").FontFamily(font);
        }

    }
}
