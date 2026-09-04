namespace Web.UI.Helpers.Enums;

public enum ColorEnum
{
    Info = 1,
    Success,
    Warning,
    Primary,
    Secondary,
    Danger,
}


public static class ColorEnumExtension
{
    public static string ToBootstrapColor(this ColorEnum color)
    {
        return color switch
        {
            ColorEnum.Info => "card-info",
            ColorEnum.Success => "card-success",
            ColorEnum.Warning => "card-warning",
            ColorEnum.Primary => "card-primary",
            ColorEnum.Secondary => "card-secondary",
            ColorEnum.Danger => "card-danger",
            _ => throw new ArgumentOutOfRangeException(nameof(color), color, null)
        };
    }
}