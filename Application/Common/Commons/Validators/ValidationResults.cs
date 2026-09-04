using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Commons.Validators;

public class ValidationResults
{
    private readonly List<ValidationIssue> _issues = [];

    public IReadOnlyList<ValidationIssue> Issues => _issues;

    public bool IsValid => _issues.Count == 0;

    public void Add(string message, int? lineNumber = null) => _issues.Add(new ValidationIssue(message, lineNumber));
}
