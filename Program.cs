using System.Text;

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

        public static void GenerateCombinations(string keyword, int maxNumber, string fullPath, DateTime? birthday, string symbolsString, int maxChars)
        {
            File.WriteAllText(fullPath, "");
            List<string> wordVariations = keyword == "" ? [""] : GenerateCapitalizationVariations(keyword).ToHashSet().ToList();
            int? maxLength = maxNumber == 0 ? null : maxNumber.ToString().Length;
            List<string> dateFormats = birthday.HasValue ? GenerateDateVariations(birthday.Value).ToHashSet().ToList() : [];
            List<string> combinations3 = GenerateCombinationsFormat(2).ToHashSet().ToList();
            List<string> combinations4 = GenerateCombinationsFormat(3).ToHashSet().ToList();
            List<string> combinations5 = GenerateCombinationsFormat(4).ToHashSet().ToList();
            List<string> symbols = GenerateSymbolCombinations(symbolsString.ToCharArray()).ToHashSet().ToList();
            List<string> numberVariations = new();
            //int combinationsCount = (maxLength > 0 ? 1 : 0) + (birthday.HasValue ? 1 : 0) + (symbolsString.Length > 0 ? 1 : 0);


            for (int i = 0; i <= maxNumber; i++)
            {
                if (maxLength == null)
                {
                    break;
                }

                numberVariations.Add(i.ToString());
                if (i.ToString().Length != maxNumber.ToString().Length)
                {
                    numberVariations.Add(i.ToString().PadLeft(maxLength ?? 0, '0'));
                }
            }

            FileStream fs = File.Open(fullPath, FileMode.Append);

            List<List<string>> allLists = new() { wordVariations, dateFormats, numberVariations, symbols };
            //List<string> combinations = 
            GenerateCombinationsBetweenListsRecursive(allLists, "", 0, fs, maxChars);

            /*
            foreach (string word in wordVariations)
            {
                byte[] info = new UTF8Encoding(true).GetBytes(word + "\n");
                fs.Write(info, 0, info.Length);
                foreach (string symbol in symbols)
                {
                    foreach (string symbol2 in symbols.Skip(1).Concat(symbols.Take(1)))
                    {
                        for (int i = 0; i <= maxNumber; i++)
                        {
                            string formattedNumber = maxLength == null ? "" : i.ToString().PadLeft(maxLength ?? 0, '0');
                            foreach (string format in combinations4)
                            {
                                info = new UTF8Encoding(true).GetBytes(string.Format(format, word, formattedNumber, symbol, symbol2) + "\n");
                                fs.Write(info, 0, info.Length);
                            }

                            foreach (string date in dateFormats)
                            {
                                foreach (string format in combinations5)
                                {
                                    info = new UTF8Encoding(true).GetBytes(string.Format(format, word, formattedNumber, date, symbol, symbol2) + "\n");
                                    fs.Write(info, 0, info.Length);
                                }
                            }
                        }
                    }
                }
            }
            */
            fs.Close();

            HashSet<string> uniqueLines = [];

            using (StreamReader reader = new(fullPath))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Length >= 4)
                    {
                        uniqueLines.Add(line);
                    }
                }
                reader.Close();
            }
            File.WriteAllLines(fullPath, uniqueLines);
            uniqueLines = [];
        }

        public static List<string> GenerateCombinationsFormat(int length)
        {
            List<string> result = new();
            GenerateCombinationsFormatRecursive(length, "", result);
            return result;
        }

        private static void GenerateCombinationsFormatRecursive(int length, string current, List<string> result)
        {
            if (current.Length > 0)
            {
                result.Add(current);
            }

            if (current.Replace("{", "").Replace("}", "").Length == length + 1)
            {
                return;
            }

            for (int i = 0; i <= length; i++)
            {
                if (current.Contains("{" + i + "}"))
                {
                    continue;
                }
                GenerateCombinationsFormatRecursive(length, current + "{" + i + "}", result);
            }
        }

        static List<string> GenerateCapitalizationVariations(string word)
        {
            word = word.ToLower();
            List<string> variations = [];
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
            List<string> dates = [];

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

        static List<string> GenerateSymbolCombinations(char[] symbols)
        {
            List<string> results = new();
            GenerateSymbolRecursive(symbols, "", symbols.Length, results);
            return results;
        }

        static void GenerateSymbolRecursive(char[] symbols, string current, int length, List<string> results)
        {
            if (current.Length > 0)
            {
                results.Add(current);
            }

            if (current.Length == 1)
            {
                return;
            }

            foreach (char symbol in symbols)
            {
                GenerateSymbolRecursive(symbols, current + symbol, length, results);
            }
        }

        static void GenerateCombinationsBetweenListsRecursive(List<List<string>> lists, string current, int depth, FileStream fs, int maxChars)
        {
            if (depth == lists.Count)
            {
                if (!string.IsNullOrEmpty(current))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(current + "\n");
                    fs.Write(info, 0, info.Length);
                }
                //return;
            }

            foreach (var item in lists[depth])
            {
                // Recursively call for the next depth level
                GenerateCombinationsBetweenListsRecursive(lists, current + item, depth + 1, fs, maxChars);
            }
        }
    }
}