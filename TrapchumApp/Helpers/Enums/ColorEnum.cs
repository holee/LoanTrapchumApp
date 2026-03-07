namespace TrapchumApp.Helpers.Enums;

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
            ColorEnum.Info => "rz-background-color-info",
            ColorEnum.Success => "rz-background-color-success",
            ColorEnum.Warning => "rz-background-color-warning",
            ColorEnum.Primary => "rz-background-color-primary",
            ColorEnum.Secondary => "rz-background-color-secondary",
            ColorEnum.Danger => "rz-background-color-danger",
            _ => throw new ArgumentOutOfRangeException(nameof(color), color, null)
        };
    }
}