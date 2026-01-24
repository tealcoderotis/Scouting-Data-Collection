using System;
using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName = "TextMeshPro/Date Input Validator")]
public class DateInputValidator : TMPro.TMP_InputValidator
{
    private static readonly char[] format = "MM:DD:YYYY".ToCharArray();

    public override char Validate(ref string text, ref int pos, char ch)
    {
        if (ch < '0' || ch > '9' || pos >= text.Length) return '\0';

        StringBuilder stringBuilder = new(text);
        if (stringBuilder[pos] < '0' || stringBuilder[pos] > '9') ++pos;
        stringBuilder.Remove(pos, 1);
        stringBuilder.Insert(pos, ch.ToString());
        ++pos;
        if (pos < stringBuilder.Length && (stringBuilder[pos] < '0' || stringBuilder[pos] > '9')) ++pos;

        int month = int.Parse(stringBuilder.ToString(0, 2));
        if (month < 1 || month > 12)
        {
            month = Math.Clamp(month, 1, 12);
            string monthStr = month.ToString();
            for (int i = 0; i < monthStr.Length; ++i) stringBuilder[i] = monthStr[i];
        }
        int year = int.Parse(stringBuilder.ToString(6, 4));
        if (year < DateTime.MinValue.Year || year > DateTime.MaxValue.Year)
        {
            year = Math.Clamp(year, DateTime.MinValue.Year, DateTime.MaxValue.Year);
            string yearStr = year.ToString();
            for (int i = 0; i < yearStr.Length; ++i) stringBuilder[i + 6] = yearStr[i];
        }
        int day = int.Parse(stringBuilder.ToString(3, 2));
        if (day < 1 || day > DateTime.DaysInMonth(year, month))
        {
            day = Math.Clamp(day, 1, DateTime.DaysInMonth(year, month));
            string dayStr = day.ToString();
            for (int i = 0; i < dayStr.Length; ++i) stringBuilder[i + 3] = dayStr[i];
        }

        text = stringBuilder.ToString();
        return ch;
    }
}
