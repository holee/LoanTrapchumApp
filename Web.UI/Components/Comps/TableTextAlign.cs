namespace Web.UI.Components.Comps;

public enum TableTextAlign
{
    Left,
    Center,
    Right,
    Justify,
}

public static class TableTextAlignExtensions
{
    public static string ToCss(this TableTextAlign align) => align switch
    {
        TableTextAlign.Center  => "center",
        TableTextAlign.Right   => "right",
        TableTextAlign.Justify => "justify",
        _                      => "left",
    };
}
