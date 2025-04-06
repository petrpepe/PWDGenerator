using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace PWDGenerator
{
    internal static class Program
    {
        private static readonly int bufferLimit = 500000;
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

        public static void GeneratePWDCombinations(string keywords, int maxNumber, string fullPath, DateTime? birthday, string symbolsString, int maxChars, int numOfSymbols)
        {
            File.WriteAllText(fullPath, "");
            List<List<string>> words = [];
            int? maxNumLength = maxNumber <= 0 ? null : maxNumber.ToString().Length;
            List<string> dateFormats = birthday.HasValue ? GenerateDateVariations(birthday.Value) : [];
            //dateFormats.Sort((a, b) => a.Length.CompareTo(b.Length));
            List<string> symbols = [.. symbolsString.ToCharArray().Select(c => c.ToString())];
            List<string> numberVariations = [];
            List<string> wordsList = [.. keywords.Split([',', ';', ' '], StringSplitOptions.RemoveEmptyEntries).Select(w => w.Trim().ToLower())];

            foreach (string word in wordsList)
            {
                words.Add([.. GenerateCapitalizationVariations(word, maxChars)]);
            }

            for (int i = 0; i <= maxNumber; i++)
            {
                if (maxNumLength == null)
                {
                    break;
                }

                numberVariations.Add(i.ToString());
                if (i.ToString().Length != maxNumber.ToString().Length)
                {
                    numberVariations.Add(i.ToString().PadLeft(maxNumLength ?? 0, '0'));
                }
            }

            //numberVariations.Sort((a,b) => a.Length.CompareTo(b.Length));

            List<List<string>> allLists = [.. words, numberVariations, dateFormats];
            for (int i = 0; i < numOfSymbols; i++)
            {
                allLists.Add(symbols);
            }
            allLists.RemoveAll(l => l.Count == 0);

            string permutationsFile = Path.GetTempFileName();

            string combinationsFile = Path.GetTempFileName();

            HashSet<string> buffer = new(bufferLimit + 10);
            GenerateAllCombinations(allLists, maxChars, combinationsFile, buffer);

            using (var writer = new StreamWriter(permutationsFile))
            using (var reader = new StreamReader(combinationsFile))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    PermuteRecursive([.. line.Split(",")], 0, maxChars, writer, buffer);
                }
                
                // Write remaining buffer
                if (buffer.Count > 0) FlushBuffer(writer, buffer);
            }

            buffer.Clear();

            CopyUniqueLines(permutationsFile, fullPath);

            /*FileStream fs = new(fullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);

            using (StreamReader reader = new(permutationsFile))
            using (StreamWriter writer = new(fs))
            {
                HashSet<string> uniqueLines = [];
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    uniqueLines.Add(line);
                }
                writer.WriteLine(string.Join("\n", uniqueLines));
                uniqueLines.Clear();
            }

            fs?.Dispose();*/
            File.Delete(combinationsFile);
            File.Delete(permutationsFile);
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

        static List<string> GenerateCapitalizationVariations(string word, int maxChars)
        {
            if (word.Length > maxChars) return [];
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

            dates.Add($"{year}");
            dates.Add($"{shortYear}");
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

        static void GenerateAllCombinations(List<List<string>> listOfLists, int maxChars, string combinationsFile, HashSet<string> buffer)
        {
            using var writer = new StreamWriter(combinationsFile);
            GenerateCombinationsRecursive(listOfLists, 0, [], maxChars, writer, buffer);
            if (buffer.Count > 0) FlushBuffer(writer, buffer);
        }

        static void GenerateCombinationsRecursive(List<List<string>> lists, int depth, List<string> current, int maxLength, StreamWriter writer, HashSet<string> buffer)
        {
            if (GetTotalLength(current) >= 4 && GetTotalLength(current) <= maxLength)
            {
                buffer.Add(string.Join(",", current));
                if (buffer.Count >= bufferLimit)
                    FlushBuffer(writer, buffer);
            }

            if (depth > lists.Count)
            {
                return;
            }

            for (int i = depth; i < lists.Count; i++)
            {
                foreach (string item in lists[i])
                {
                    current.Add(item);
                    if (GetTotalLength(current) >= 4 && GetTotalLength(current) <= maxLength)
                    {
                        GenerateCombinationsRecursive(lists, i + 1, current, maxLength, writer, buffer);
                    }
                    current.RemoveAt(current.Count - 1);
                }
            }
        }

        static void PermuteRecursive(List<string> list, int start, int maxLength, StreamWriter writer, HashSet<string> buffer)
        {
            if (start >= list.Count)
            {
                if (GetTotalLength(list) <= maxLength)
                {
                    buffer.Add(string.Join("", list));
                    if (buffer.Count >= bufferLimit)
                        FlushBuffer(writer, buffer);
                }
                return;
            }

            for (int i = start; i < list.Count; i++)
            {
                Swap(list, start, i);
                PermuteRecursive(list, start + 1, maxLength, writer, buffer);
                Swap(list, start, i);
            }
        }

        static void Swap(List<string> list, int i, int j)
        {
            string temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

        static int GetTotalLength(List<string> list)
        {
            return list.Sum(s => s.Length);
        }

        static void FlushBuffer(StreamWriter writer, HashSet<string> buffer)
        {
            foreach (var line in buffer)
                writer.WriteLine(line);
            buffer.Clear();
        }

        static void CopyUniqueLines(string inputFilePath, string outputFilePath)
        {
            string tempFilePath = Path.GetTempFileName();
            HashSet<string> uniqueLines = [];
            int bufferCount = 0;

            using (var reader = new StreamReader(inputFilePath))
            using (var tempWriter = new StreamWriter(tempFilePath))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (uniqueLines.Add(line))
                    {
                        tempWriter.WriteLine(line);
                        bufferCount++;

                        if (bufferCount >= bufferLimit)
                        {
                            uniqueLines.Clear();
                            bufferCount = 0;
                        }
                    }
                }
            }

            // Write unique lines from the temporary file to the final output file
            using (var tempReader = new StreamReader(tempFilePath))
            using (var writer = new StreamWriter(outputFilePath))
            {
                uniqueLines.Clear();
                string? line;
                while ((line = tempReader.ReadLine()) != null)
                {
                    if (uniqueLines.Add(line))
                    {
                        writer.WriteLine(line);
                    }
                }
            }

            File.Delete(tempFilePath);
        }
    }
}