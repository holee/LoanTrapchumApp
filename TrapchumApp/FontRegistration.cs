using QuestPDF.Drawing;

namespace TrapchumApp;

public static class FontRegistration
{
    public static void RegisterFonts(IWebHostEnvironment env)
    {
        var fonts = new[]
        {
            "KhmerOSmuol.ttf",
            "KhmerOSSiemreap.ttf",
        };

        foreach (var font in fonts)
        {
            var path = Path.Combine(env.ContentRootPath, "Fonts", font);

            if (File.Exists(path))
            {
                FontManager.RegisterFont(File.OpenRead(path));
            }
        }
    }
}
