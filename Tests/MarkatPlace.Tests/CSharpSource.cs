using System.Text;

namespace MarkatPlace.Tests;

internal static class CSharpSource
{
    public static int SkipString(string source, int start)
    {
        var index = start;
        var interpolated = false;
        var verbatim = false;

        while (index < source.Length && (source[index] == '$' || source[index] == '@'))
        {
            interpolated |= source[index] == '$';
            verbatim |= source[index] == '@';
            index++;
        }

        if (index >= source.Length || source[index] != '"')
            return start + 1;

        index++;

        while (index < source.Length)
        {
            var character = source[index];

            if (verbatim && character == '"')
            {
                if (index + 1 < source.Length && source[index + 1] == '"')
                {
                    index += 2;
                    continue;
                }

                return index + 1;
            }

            if (!verbatim && character == '\\')
            {
                index += 2;
                continue;
            }

            if (character == '"')
                return index + 1;

            if (interpolated && character == '{')
            {
                if (index + 1 < source.Length && source[index + 1] == '{')
                {
                    index += 2;
                    continue;
                }

                index = SkipInterpolationHole(source, index);
                continue;
            }

            index++;
        }

        return index;
    }

    private static int SkipInterpolationHole(string source, int start)
    {
        var depth = 0;
        var index = start;

        while (index < source.Length)
        {
            var character = source[index];

            if (character is '"' or '$' or '@')
            {
                var next = SkipString(source, index);
                if (next > index + 1 || character == '"')
                {
                    index = next;
                    continue;
                }
            }

            if (character == '{')
            {
                depth++;
            }
            else if (character == '}')
            {
                depth--;
                if (depth == 0)
                    return index + 1;
            }

            index++;
        }

        return index;
    }

    public static string ReadArgumentList(string source, int start)
    {
        var depth = 1;
        var index = start;

        while (index < source.Length && depth > 0)
        {
            var character = source[index];

            if (character is '"' or '$' or '@')
            {
                var next = SkipString(source, index);
                if (next > index + 1 || character == '"')
                {
                    index = next;
                    continue;
                }
            }

            if (character is '(' or '[')
                depth++;
            else if (character is ')' or ']')
                depth--;

            index++;
        }

        return source[start..Math.Max(start, index - 1)];
    }

    public static IEnumerable<string> SplitArguments(string arguments)
    {
        var depth = 0;
        var start = 0;
        var index = 0;

        while (index < arguments.Length)
        {
            var character = arguments[index];

            if (character is '"' or '$' or '@')
            {
                var next = SkipString(arguments, index);
                if (next > index + 1 || character == '"')
                {
                    index = next;
                    continue;
                }
            }

            if (character is '(' or '[' or '{')
                depth++;
            else if (character is ')' or ']' or '}')
                depth--;
            else if (character == ',' && depth == 0)
            {
                yield return arguments[start..index];
                start = index + 1;
            }

            index++;
        }

        if (start < arguments.Length)
            yield return arguments[start..];
    }

    public static IEnumerable<string> StringLiterals(string text)
    {
        var index = 0;

        while (index < text.Length)
        {
            if (text[index] is '"' or '$' or '@')
            {
                var next = SkipString(text, index);
                if (next > index + 1 || text[index] == '"')
                {
                    yield return Unwrap(text[index..next]);
                    index = next;
                    continue;
                }
            }

            index++;
        }
    }

    private static string Unwrap(string literal)
    {
        var text = literal.TrimStart('$', '@');
        if (text.Length >= 2 && text[0] == '"' && text[^1] == '"')
            text = text[1..^1];

        var result = new StringBuilder();
        var depth = 0;

        foreach (var character in text)
        {
            if (character == '{')
                depth++;
            else if (character == '}')
                depth = Math.Max(0, depth - 1);
            else if (depth == 0)
                result.Append(character);
        }

        return result.ToString();
    }
}
