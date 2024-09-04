using System.Diagnostics;
internal class Program
{
    const int combinationLength = 6;
    const string inputFilePath = "Resources/input.txt";

    private static void Main(string[] args)
    {
        if (!File.Exists(inputFilePath))
        {
            Console.WriteLine("Wrong file path!");
            return;
        }
        HashSet<string> combinations = new();
        List<string> parts = new();
        var watch = Stopwatch.StartNew();
        //HashSet to get rid of duplicate values;
        HashSet<string> partsSet = File.ReadLines(inputFilePath).ToHashSet();
        //Order alphabetically and length and convert to list for easier manipulation
        parts = partsSet.OrderBy(x => x).ThenBy(x => x.Length).ToList();
        List<string> words = parts.Where(x => x.Length == combinationLength).ToList();
        foreach (var word in words)
        {
            foreach (var combi in FindCombinationsOfWord(word, parts))
            {
                if (combi.Length != combinationLength && combinations.Add(combi))
                    Console.WriteLine($"{combi}={word}");
            }
        }
        watch.Stop();
        Console.WriteLine($"Found {combinations.Count} combinations in {watch.ElapsedMilliseconds}ms");
    }

    private static HashSet<string> FindCombinationsOfWord(string word, List<string> parts)
    {
        var combinations = new HashSet<string>();
        if (word.Length < 2)
        {
            combinations.Add(word);
            return combinations;
        }
        List<string> filteredParts = new();
        foreach (var part in parts)
        {
            if (word.Contains(part))
                filteredParts.Add(part);
        }
        foreach (var wordPart in filteredParts)
        {
            if (!word.StartsWith(wordPart))
                continue;
            string remainingWord = word.Substring(wordPart.Length);
            if (string.IsNullOrEmpty(remainingWord))
            {
                combinations.Add(wordPart);
                continue;
            }
            foreach (var combi in FindCombinationsOfWord(remainingWord, filteredParts))
                combinations.Add($"{word.Substring(0, wordPart.Length)}+{combi}");
        }
        return combinations;
    }
}