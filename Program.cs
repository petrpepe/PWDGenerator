using System.Collections.Generic;
using System.Text;

namespace PWDGenerator
{
    internal static class Program
    {
        private static int bufferLimit = 50000;
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

        public static void GenerateCombinations(string keyword, int maxNumber, string fullPath, DateTime? birthday, string symbolsString, int maxChars, int numOfSymbols)
        {
            File.WriteAllText(fullPath, "");
            List<string> wordVariations = keyword == "" ? [""] : [.. GenerateCapitalizationVariations(keyword)];
            int? maxLength = maxNumber == 0 ? null : maxNumber.ToString().Length;
            List<string> dateFormats = birthday.HasValue ? [.. GenerateDateVariations(birthday.Value)] : [];
            List<string> symbols = [.. GenerateSymbolCombinations(symbolsString.ToCharArray())];
            List<string> numberVariations = [];
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

            List<List<string>> allLists = [wordVariations, numberVariations, dateFormats];
            for (int i = 0; i < numOfSymbols; i++)
            {
                allLists.Add(symbols);
            }
            GenerateListsCombinationsAndPermutations(fullPath, allLists, maxChars);


            HashSet<string> uniqueLines = [];

            using (StreamReader reader = new(fullPath))
            {
                using StreamWriter writer = new(fullPath);
                List<string> buffer = [];
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    buffer.Add(line);
                    if (buffer.Count >= bufferLimit)
                    {
                        writer.WriteLine(fullPath, buffer);
                        buffer.Clear();
                    }
                }
                if (buffer.Count > 0)
                {
                    writer.WriteLine(fullPath, buffer);
                }
                reader.Close();
                reader.Dispose();
            }
            //File.WriteAllLines(fullPath, uniqueLines);
            uniqueLines.Clear();
        }

        public static List<string> GenerateCombinationsFormat(int length)
        {
            List<string> result = [];
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
            List<string> results = [];
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
        /*
            byte[] info = new UTF8Encoding(true).GetBytes(word + "\n");
            fs.Write(info, 0, info.Length);
         */
        public static void GenerateListsCombinationsAndPermutations(string fullPath, List<List<string>> lists, int maxChars)
        {
            string combinationsFile = Path.GetTempFileName();
            string permutationsFile = fullPath;

            // Generate combinations and write to temporary file
            using (StreamWriter writer = new(combinationsFile))
            {
                GenerateListsCombinations(lists, writer, maxChars);
                writer.Close();
                writer.Dispose();
            }

            // Read combinations, generate permutations, and write to another temporary file
            using (StreamWriter writer = new(permutationsFile))
            {
                using StreamReader reader = new(combinationsFile);
                string? line;
                HashSet<string> uniquePermutations = [];
                List<string> buffer = [];
                while ((line = reader.ReadLine()) != null)
                {
                    buffer.Add(line);
                    if (buffer.Count >= 5000)
                    {
                        ProcessBuffer(buffer, writer, maxChars);
                        buffer.Clear();
                    }
                }
                if (buffer.Count > 0)
                {
                    ProcessBuffer(buffer, writer, maxChars);
                }
                reader.Close();
                reader.Dispose();
                writer.Close();
                writer.Dispose();
            }
            
            File.Delete(combinationsFile);
        }

        private static void ProcessBuffer(List<string> buffer, StreamWriter writer, int maxChars)
        {
            foreach (var line in buffer)
            {
                var combination = line.Split(',').ToList();
                GenerateListsPermutations(combination, writer, maxChars);
            }
        }

        public static void GenerateListsCombinations<T>(List<List<T>> lists, StreamWriter writer, int maxChars)
        {
            List<string> buffer = [];
            if (lists == null || lists.Count == 0)
            {
                return;
            }

            // Generate combinations of varying lengths
            for (int length = 1; length <= lists.Count; length++)
            {
                GenerateListsCombinationsRecursive(lists, length, writer, buffer, maxChars);
            }
        }

        private static void GenerateListsCombinationsRecursive<T>(List<List<T>> lists, int length, StreamWriter writer, List<string> buffer, int maxChars, int start = 0, List<T>? current = null)
        {
            current ??= [];
            if (current.Count == length)
            {
                string result = string.Join("", current);
                if (result.Length >= 4 && result.Length <= maxChars)
                {
                    buffer.Add(string.Join(",", current));
                    if (buffer.Count >= bufferLimit)
                    {
                        writer.WriteLine(string.Join("\n", buffer));
                        buffer.Clear();
                    }
                }
                return;
            }

            for (int i = start; i < lists.Count; i++)
            {
                foreach (var element in lists[i])
                {
                    current.Add(element);
                    GenerateListsCombinationsRecursive(lists, length, writer, buffer, maxChars, i + 1, current);
                    current.RemoveAt(current.Count - 1);
                }
            }

            if (buffer.Count > 0)
            {
                writer.WriteLine(string.Join("\n", buffer));
                buffer.Clear();
            }
        }

        public static void GenerateListsPermutations<T>(List<T> list, StreamWriter writer, int maxChars)
        {
            List<string> buffer = [];
            GenerateListsPermutationsRecursive(list, 0, writer, maxChars, buffer);
        }

        private static void GenerateListsPermutationsRecursive<T>(List<T> list, int k, StreamWriter writer, int maxChars, List<string> buffer)
        {
            if (k == list.Count)
            {
                string result = string.Join("", list);
                if (result.Length >= 4 && result.Length <= maxChars)
                {
                    buffer.Add(result);
                    if (buffer.Count >= bufferLimit)
                    {
                        writer.WriteLine(string.Join("\n", buffer));
                        buffer.Clear();
                    }
                }
            }
            else
            {
                for (int i = k; i < list.Count; i++)
                {
                    Swap(list, k, i);
                    GenerateListsPermutationsRecursive(list, k + 1, writer, maxChars, buffer);
                    Swap(list, k, i); // backtrack
                }
            }

            if (buffer.Count > 0)
            {
                writer.WriteLine(string.Join("\n", buffer));
                buffer.Clear();
            }
        }

        private static void Swap<T>(List<T> list, int i, int j)
        {
            (list[j], list[i]) = (list[i], list[j]);
        }
    }
}