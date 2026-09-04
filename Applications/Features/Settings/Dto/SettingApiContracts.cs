namespace Applications.Features.Settings.Dto;

/// <summary>
/// Wraps a plain text / boolean handler result so HTTP clients always receive a JSON object.
/// Used by the <c>/api/settings</c> minimal API consumed by the Web.UI WebAssembly client.
/// </summary>
public sealed record SettingMessageResponse(string Message);
