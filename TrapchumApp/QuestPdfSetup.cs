using QuestPDF;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace TrapchumApp;

internal static class QuestPdfSetup
{
    //            
    internal static void QuestPdfConfig()
    {
        Settings.License = LicenseType.Community;
        Settings.UseEnvironmentFonts = false;
        FontManager.RegisterFontFromEmbeddedResource("TrapchumApp.Resources.Fonts.KhmerOSsiemreap.ttf");
        FontManager.RegisterFontFromEmbeddedResource("TrapchumApp.Resources.Fonts.KhmerOSmuol.ttf");
    }
}