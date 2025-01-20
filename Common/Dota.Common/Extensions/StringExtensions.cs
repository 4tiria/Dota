namespace Dota.API.Helpers;

public static class StringExtensions
{
    public static string ToCamelCaseWithUnderscore(this string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return str;
        }

        // Убираем пробелы, дефисы и нижние подчеркивания
        var words = str.Split(new[] { ' ', '-', '_' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(word => char.ToUpperInvariant(word[0]) + word.Substring(1).ToLowerInvariant());

        return string.Join("_", words);
    }
}