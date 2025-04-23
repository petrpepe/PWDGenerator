using System.Security.Cryptography;
using System.Text;

namespace PWDGenerator
{
    internal static class Program
    {
        private static readonly int bufferLimit = 100_000;
        static readonly int StripeCount = Environment.ProcessorCount * 2;
        static readonly object[] Locks = Enumerable.Range(0, StripeCount).Select(_ => new object()).ToArray();
        static readonly HashSet<ulong>[] GlobalHashStripes = Enumerable.Range(0, StripeCount).Select(_ => new HashSet<ulong>()).ToArray();

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
            dateFormats.Sort((a, b) => a.Length.CompareTo(b.Length));
            List<string> symbols = [.. symbolsString.ToCharArray().Select(c => c.ToString())];
            List<string> numberVariations = [];
            List<string> wordsList = [.. keywords.Split([',', ';', ' '], StringSplitOptions.RemoveEmptyEntries).Select(w => w.Trim().ToLower())];
            List<string> usedWordsList = [];

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

            numberVariations.Sort((a, b) => a.Length.CompareTo(b.Length));

            List<List<string>> allLists = [.. words, numberVariations, dateFormats];
            for (int i = 0; i < numOfSymbols; i++) allLists.Add(symbols);
            allLists.RemoveAll(l => l.Count == 0);

            List<string> outputBuffer = new(bufferLimit + 10);
            StringBuilder sb = new();
            string combinationsFile = Path.GetTempFileName();
            GenerateAllCombinations(allLists, maxChars, combinationsFile, outputBuffer, sb, bufferLimit);
            ProcessPermutationsInParallel(combinationsFile, fullPath, maxChars, bufferLimit);

            /*
            using StreamWriter writer = new(fullPath, false, Encoding.UTF8, 1024 * 64);
            using StreamReader reader = new(combinationsFile);

            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                PermuteRecursive([.. line.Split(',')], 0, 0, maxChars, writer, outputBuffer, sb, bufferLimit);
            }

            if (outputBuffer.Count > 0) FlushBuffer(writer, outputBuffer);

            File.Delete(combinationsFile);*/

            /*
            string permutationsFile = Path.GetTempFileName();
            using (var writer = new StreamWriter(permutationsFile))
            using (var reader = new StreamReader(combinationsFile))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    PermuteRecursive([.. line.Split(",")], 0, 0, maxChars, writer, outputBuffer, sb, bufferLimit);
                }

                // Write remaining buffer
                if (outputBuffer.Count > 0) FlushBuffer(writer, outputBuffer);
            }

            outputBuffer.Clear();

            CopyUniqueLines(permutationsFile, fullPath, bufferLimit);

            FileStream fs = new(fullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);

            using (StreamReader reader = new(permutationsFile))
            using (StreamWriter writer = new(fs))
            {
                HashSet<string> uniqueLines = [];
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    uniqueLines.Add(line);
                }
                writer.WriteLine(string.Join(Environment.NewLine, uniqueLines));
                uniqueLines.Clear();
            }

            fs?.Dispose();
            File.Delete(combinationsFile);
            File.Delete(permutationsFile);*/
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

        static void GenerateAllCombinations(List<List<string>> listOfLists, int maxChars, string combinationsFile,
        List<string> outputBuffer, StringBuilder sb, int bufferLimit)
        {
            using var writer = new StreamWriter(combinationsFile);

            GenerateCombinationsRecursive(listOfLists, 0, [], 0, maxChars, writer, outputBuffer, sb, bufferLimit);

            if (outputBuffer.Count > 0)
                FlushBuffer(writer, outputBuffer);
        }

        static void GenerateCombinationsRecursive(List<List<string>> lists, int depth, List<string> current, int currentLength,
            int maxLength, StreamWriter writer, List<string> outputBuffer, StringBuilder sb, int bufferLimit)
        {
            if (currentLength <= maxLength && currentLength >= 4)
            {
                sb.Clear();
                for (int i = 0; i < current.Count; i++)
                {
                    if (i > 0) sb.Append(',');
                    sb.Append(current[i]);
                }

                string combination = sb.ToString();

                outputBuffer.Add(combination);
                if (outputBuffer.Count >= bufferLimit) FlushBuffer(writer, outputBuffer);
            }

            if (depth >= lists.Count)
                return;

            for (int i = depth; i < lists.Count; i++)
            {
                foreach (string item in lists[i])
                {
                    int itemLength = item.Length;
                    if (currentLength + itemLength > maxLength) continue;

                    current.Add(item);
                    GenerateCombinationsRecursive(lists, i + 1, current, currentLength + itemLength, maxLength,
                        writer, outputBuffer, sb, bufferLimit);
                    current.RemoveAt(current.Count - 1);
                }
            }
        }

        static void ProcessPermutationsInParallel(string combinationsFile, string fullPath, int maxLength, int bufferLimit)
        {
            var allLines = File.ReadAllLines(combinationsFile);
            var chunkSize = bufferLimit / 2; // Tune this depending on memory/CPU
            var chunks = allLines.Chunk(chunkSize).ToArray();

            var fileLock = new object(); // Lock for writing to final file

            Parallel.ForEach(chunks, chunk =>
            {
                List<string> outputBuffer = new(bufferLimit + 10);
                StringBuilder sb = new();

                using var localWriter = new StringWriter(); // Temporary in-memory writer per thread

                foreach (var line in chunk)
                {
                    var parts = line.Split(',').ToList();
                    PermuteRecursive(parts, 0, 0, maxLength, localWriter, outputBuffer, sb, bufferLimit);
                }

                // Thread-safe final write
                lock (fileLock)
                {
                    using var writer = GetStreamWriterWithSizeLimit(fullPath, 950 * 1024 * 1024); ;
                    writer.Write(localWriter.ToString());
                    if (outputBuffer.Count > 0)
                        FlushBuffer(writer, outputBuffer);

                    static StreamWriter GetStreamWriterWithSizeLimit(string basePath, long sizeLimit)
                    {
                        string directory = Path.GetDirectoryName(basePath) ?? "";
                        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(basePath);
                        string extension = Path.GetExtension(basePath);

                        int fileIndex = 0;
                        string currentFilePath = basePath;

                        while (File.Exists(currentFilePath) && new FileInfo(currentFilePath).Length >= sizeLimit)
                        {
                            fileIndex++;
                            currentFilePath = Path.Combine(directory, $"{fileNameWithoutExtension}_{fileIndex}{extension}");
                        }

                        return new StreamWriter(currentFilePath, append: true, Encoding.UTF8, 1024 * 64);
                    }
                }
            });
        }

        static void PermuteRecursive(List<string> list, int start, int currentLength, int maxLength, StringWriter writer,
            List<string> outputBuffer, StringBuilder sb, int bufferLimit)
        {
            if (currentLength > maxLength)
                return;

            if (start >= list.Count)
            {
                if (currentLength <= maxLength && currentLength >= 4)
                {
                    sb.Clear();
                    foreach (var item in list) sb.Append(item);
                    string permutation = sb.ToString();

                    ulong hash = Fnv1A64(permutation); // Use fast, strong hash
                    int stripe = (int)(hash % (ulong)StripeCount);

                    lock (Locks[stripe])
                    {
                        if (GlobalHashStripes[stripe].Add(hash))
                        {
                            writer.WriteLine(permutation); // Write immediately
                        }
                    }
                }
                return;
            }

            for (int i = start; i < list.Count; i++)
            {
                Swap(list, start, i);
                int newLength = currentLength + list[start].Length;
                PermuteRecursive(list, start + 1, newLength, maxLength, writer, outputBuffer, sb, bufferLimit);
                Swap(list, start, i);
            }
        }

        static ulong Fnv1A64(string input)
        {
            const ulong offset = 14695981039346656037;
            const ulong prime = 1099511628211;

            ulong hash = offset;
            foreach (char c in input)
            {
                hash ^= c;
                hash *= prime;
            }
            return hash;
        }

        static void Swap(List<string> list, int i, int j)
        {
            (list[j], list[i]) = (list[i], list[j]);
        }

        static void FlushBuffer(StreamWriter writer, List<string> buffer)
        {
            writer.Write(string.Join(Environment.NewLine, buffer));
            writer.WriteLine();
            buffer.Clear();
        }

        static string GetHash(string input)
        {
            byte[] hashBytes = SHA512.HashData(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(hashBytes); // You could also use BitConverter.ToString(hashBytes)
        }
    }
}