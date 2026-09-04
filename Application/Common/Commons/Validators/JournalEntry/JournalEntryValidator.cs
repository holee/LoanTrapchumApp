namespace Application.Commons.Validators.JournalEntry;

public static class JournalEntryValidator
{
    public static ValidationResults Validate(JournalEntryDtoCreate entry)
    {
        var result = new ValidationResults();

        if (string.IsNullOrWhiteSpace(entry.ReferrenceNum))
        {
            result.Add("លេខបញ្ជីត្រូវបំពេញ។");
        }

        if (entry.JournalDate == default)
        {
            result.Add("កាលបរិច្ឆេទត្រូវបំពេញ។");
        }

        var lines = entry.JournalEntryLines.Where(l => !l.IsEmpty).ToList();

        if (lines.Count < 2)
        {
            result.Add("ការកត់ត្រាទិនានុប្បវត្តិត្រូវមានយ៉ាងហោចណាស់ ២ បន្ទាត់បែងចែក។");
        }

        foreach (var line in lines)
        {
            if (line.LedgerRefId is null)
            {
                result.Add("សូមជ្រើសរើសគណនី។", line.LineSeq);
            }

            if (line.Debit < 0 || line.Credit < 0)
            {
                result.Add("ចំនួនទឹកប្រាក់មិនអាចជាតម្លៃអវិជ្ជមានបានទេ។", line.LineSeq);
            }

            if (line.Debit > 0 && line.Credit > 0)
            {
                result.Add("សូមបញ្ចូលតែក្ដីដេប៊ីត ឬក្រិត មួយប៉ុណ្ណោះ មិនមែនទាំងពីរទេ។", line.LineSeq);
            }

            if (line.Debit == 0 && line.Credit == 0)
            {
                result.Add("សូមបញ្ចូលចំនួនដេប៊ីត ឬក្រិត។", line.LineSeq);
            }
        }

        var totalDebit = lines.Sum(l => l.Debit) ?? 0;

        var totalCredit = lines.Sum(l => l.Credit) ?? 0;

        if (totalDebit != totalCredit)
        {
            var difference = Math.Abs(totalDebit - totalCredit);

            var heavierSide = totalDebit > totalCredit ? "Debit" : "Credit";

            result.Add($"ចំនួនដេប៊ីត និងក្រិតត្រូវស្មើគ្នា។ {heavierSide} ខ្ពស់ជាង {difference:N2}។");
        }
        else if (totalDebit == 0)
        {
            result.Add("បញ្ជីនេះគ្មានចំនួនទឹកប្រាក់សម្រាប់បង្ហោះ។");
        }

        return result;
    }
}
