namespace SportHub.Common.Helpers;

using System.Text.RegularExpressions;

using SportHub.Common.Extensions;

public static class RegExpHelper
{
    public static string CutHtml(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return null;
        }

        return Regex.Replace(input, "<.*?>", string.Empty);
    }

    public static string CutHtml(string input, string pattern)
    {
        if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(pattern))
        {
            return null;
        }

        return Regex.Replace(input, pattern, string.Empty);
    }

    public static bool IsMatch(string text, string pattern)
    {
        return Regex.IsMatch(text, pattern);
    }

    public static Match Match(string text, string pattern)
    {
        if (string.IsNullOrEmpty(text))
        {
            return null;
        }

        var match = Regex.Match(text, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        if (match.Success)
        {
            return match;
        }

        return null;
    }

    public static double? MatchDouble(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return null;
        }

        text = Regex.Replace(text, @"\(.*?\)", string.Empty);
        if (string.IsNullOrEmpty(text) || !text.Any(char.IsDigit))
        {
            return null;
        }

        text = text.Replace(",", ".").Replace("+", string.Empty).Trim();

        var match = Regex.Match(text, @"(\d+)\.(\d+)\.(\d+)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        if (match.Success)
        {
            return double.Parse($"{match.Groups[1].Value}{match.Groups[2].Value},{match.Groups[3].Value}");
        }

        match = Regex.Match(text, @"([-.\d]+)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        if (match.Success)
        {
            var number = match.Groups[1].Value/*.Replace(".", ",")*/;
            return double.Parse(number);
        }

        return null;
    }

    public static decimal? MatchDecimal(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return null;
        }

        text = Regex.Replace(text, @"\(.*?\)", string.Empty);
        if (string.IsNullOrEmpty(text) || !text.Any(char.IsDigit))
        {
            return null;
        }

        text = text.Replace(",", ".").Replace("+", string.Empty).Trim();

        var match = Regex.Match(text, @"(\d+)\.(\d+)\.(\d+)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        if (match.Success)
        {
            return decimal.Parse($"{match.Groups[1].Value}{match.Groups[2].Value},{match.Groups[3].Value}");
        }

        match = Regex.Match(text, @"([-.\d]+)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        if (match.Success)
        {
            var number = match.Groups[1].Value.Replace(".", ",");
            return decimal.Parse(number);
        }

        return null;
    }

    public static MatchCollection Matches(string text, string pattern)
    {
        return Regex.Matches(text, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
    }

    public static string MatchFirstGroup(string text, string pattern)
    {
        if (string.IsNullOrEmpty(text))
        {
            return null;
        }

        var match = Regex.Match(text, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        if (match.Success)
        {
            var result = match.Groups[1].Value.Trim().Decode();
            return result;
        }

        return null;
    }

    public static int? MatchInt(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return null;
        }

        text = text.Replace(",", string.Empty).Replace(".", string.Empty);

        var match = Regex.Match(text, @"([.\d]+)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        if (match.Success)
        {
            return int.Parse(match.Groups[1].Value);
        }

        return null;
    }

    public static string Replace(string text, string pattern, string replacement)
    {
        return Regex.Replace(text, pattern, replacement);
    }
}