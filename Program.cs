namespace PWDGenerator
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }

        public static void GenerateCombinations(string baseWord, int maxNumber, string fullPath, DateTime birthday)
        {
            int maxLength = maxNumber.ToString().Length;
            HashSet<string> generated = new();

            List<string> wordVariations = GenerateCapitalizationVariations(baseWord);
            List<string> symbols = new() { "-", "_", "*", "!", "@", "#", ".", "," };
            List<string> dateFormats = GenerateDateVariations(birthday);

            string[] symbolCombinations = {
                "{0}{1}", "{1}{0}", "{0}{1}{1}", "{1}{0}{1}", "{1}{1}{0}",
                "{0}{1}{2}", "{0}{2}{1}", "{1}{0}{2}", "{2}{0}{1}", "{1}{2}{0}", "{2}{1}{0}",
                "{2}{0}{1}{1}", "{2}{1}{0}{1}", "{2}{1}{1}{0}", "{0}{2}{1}{1}", "{1}{2}{1}{0}",
                "{1}{2}{0}{1}", "{1}{0}{2}{1}", "{0}{1}{2}{1}", "{1}{1}{2}{0}",
                "{1}{0}{1}{2}", "{0}{1}{1}{2}", "{1}{1}{0}{2}"
            };

            foreach (string word in wordVariations)
            {
                generated.Add(word);
                for (int i = 1; i <= maxNumber; i++)
                {
                    for (int padding = 1; padding <= maxLength; padding++)
                    {
                        string formattedNumber = i.ToString().PadLeft(padding, '0');
                        if (formattedNumber.ToString().Length >= 4) { generated.Add(formattedNumber); }
                        generated.Add($"{word}{formattedNumber}");
                        generated.Add($"{formattedNumber}{word}");
                        foreach (string symbol in symbols)
                        {
                            foreach (string format in symbolCombinations)
                            {
                                generated.Add(string.Format(format, word, symbol, formattedNumber));
                            }
                        }
                        foreach (string date in dateFormats)
                        {
                            if (date.ToString().Length >= 4) { generated.Add(date); }
                            generated.Add($"{word}{date}");
                            generated.Add($"{date}{word}");
                            generated.Add($"{word}{formattedNumber}{date}");
                            generated.Add($"{word}{date}{formattedNumber}");
                            generated.Add($"{date}{word}{formattedNumber}");
                            generated.Add($"{formattedNumber}{word}{date}");
                            foreach (string symbol in symbols)
                            {
                                generated.Add($"{symbol}{word}{formattedNumber}{date}");
                                generated.Add($"{symbol}{word}{date}{formattedNumber}");
                                generated.Add($"{symbol}{date}{word}{formattedNumber}");
                                generated.Add($"{symbol}{formattedNumber}{word}{date}");
                                generated.Add($"{word}{formattedNumber}{date}{symbol}");
                                generated.Add($"{word}{date}{formattedNumber}{symbol}");
                                generated.Add($"{date}{word}{formattedNumber}{symbol}");
                                generated.Add($"{formattedNumber}{word}{date}{symbol}");

                            }
                        }
                    }
                }
            }

            File.WriteAllText(fullPath, string.Join("\n", generated));
        }

        static List<string> GenerateCapitalizationVariations(string word)
        {
            word = word.ToLower();
            List<string> variations = new();
            int length = word.Length;
            int totalVariations = 1 << length; // 2^length possible variations

            for (int i = 0; i < totalVariations; i++)
            {
                char[] chars = word.ToCharArray();
                for (int j = 0; j < length; j++)
                {
                    if ((i & (1 << j)) != 0) // Bitwise check to toggle capitalization
                    {
                        chars[j] = char.ToUpper(chars[j]);
                    }
                }
                variations.Add(new string(chars));
            }

            return variations;
        }

        static List<string> GenerateDateVariations(DateTime dateTime)
        {
            List<string> dates = new();

            string year = dateTime.Year.ToString(); // "2025"
            string shortYear = dateTime.ToString("yy"); // "25"
            string month = dateTime.ToString("MM"); // "03"
            string day = dateTime.ToString("dd"); // "31"

            dates.Add(year);
            dates.Add(shortYear);
            dates.Add($"{day}");
            dates.Add($"{month}");
            dates.Add($"{year}");
            dates.Add($"{day}{month}");
            dates.Add($"{month}{day}");
            dates.Add($"{day}{month}{year}");
            dates.Add($"{year}{month}{day}");
            dates.Add($"{day}{month}{shortYear}");
            dates.Add($"{shortYear}{month}{day}");
            dates.Add($"{month}{day}{year}");
            dates.Add($"{year}{day}{month}");
            dates.Add($"{month}{day}{shortYear}");
            dates.Add($"{shortYear}{day}{month}");

            return dates;
        }
    }
}