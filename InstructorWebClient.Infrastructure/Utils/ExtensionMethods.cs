using InstructorWebClient.Infrastructure.Utils;
using System.Globalization;

namespace InstructorWebClient.Infrastructure.Utils
{
    public static class ExtensionMethods
    {
        public static bool IsNotNullOrEmpty(this string inputtedString)
        {
            bool hasValue = false;
            if (!string.IsNullOrEmpty(inputtedString) && !string.IsNullOrWhiteSpace(inputtedString))
                hasValue = true;
            return hasValue;
        }

        public static bool IsNullOrEmpty(this string inputtedString)
        {
            bool hasValue = false;
            if (string.IsNullOrEmpty(inputtedString) || string.IsNullOrWhiteSpace(inputtedString))
                hasValue = true;
            return hasValue;
        }

        public static string ToTrim(this string inputtedString)
        {
            if (!string.IsNullOrEmpty(inputtedString))
                inputtedString = inputtedString.Trim();
            return inputtedString;
        }

        public static IEnumerable<TSource> Distinct<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (!seenKeys.Contains(keySelector(element)))
                {
                    seenKeys.Add(keySelector(element));
                    yield return element;
                }
            }
        }

        public static string StrToUpper(this string inputtedString)
        {
            if (!string.IsNullOrEmpty(inputtedString) && !string.IsNullOrWhiteSpace(inputtedString))
                inputtedString = inputtedString.Trim().ToUpper();
            return inputtedString;
        }

        public static string StrToLower(this string inputtedString)
        {
            if (!string.IsNullOrEmpty(inputtedString) && !string.IsNullOrWhiteSpace(inputtedString))
                inputtedString = inputtedString.Trim().ToLower();
            return inputtedString;
        }

        public static string ToPascalCase(this string inputtedString)
        {
            if (!string.IsNullOrEmpty(inputtedString) && !string.IsNullOrWhiteSpace(inputtedString))
            {
                var result = inputtedString.Trim().ToLower().Replace("_", " ");
                TextInfo info = CultureInfo.CurrentCulture.TextInfo;
                inputtedString = info.ToTitleCase(result);
            }

            return inputtedString;
        }

        public static string KeepNumbers(this string inputtedString)
        {
            if (!string.IsNullOrEmpty(inputtedString) && !string.IsNullOrWhiteSpace(inputtedString))
            {
                inputtedString = string.Join(string.Empty, inputtedString.Where(char.IsDigit));
            }
            return inputtedString;
        }

        public static string CommaSeparateString(this List<string> inputtedList)
        {
            string desc = string.Empty;
            if (inputtedList != null && inputtedList.Count > 0)
            {
                int count = 0;
                inputtedList = inputtedList.Distinct().ToList();
                foreach (var inputtedString in inputtedList)
                {
                    if (inputtedList.Count() - 1 == count)
                    {
                        desc += inputtedString;
                    }
                    else if (inputtedList.Count > 1)
                    {
                        desc += inputtedString + ", ";
                    }
                    else
                    {
                        desc += inputtedString;
                    }
                    count++;
                }
            }
            return desc.Trim();
        }

    }
}
