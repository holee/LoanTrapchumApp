namespace Application.Commons.Validators;

public record ValidationIssue(string Message, int? LineNumber = null);
