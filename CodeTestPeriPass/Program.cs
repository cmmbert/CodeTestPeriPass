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
        for (int i = 0; i < parts.Count - 1; i++)
        {
            foreach (var combi in FindCombinationsWithPart(i, parts))
            {
                if(combinations.Add(combi))
                    Console.WriteLine(combi);
            }
        }
        watch.Stop();
        Console.WriteLine($"Found {combinations.Count} combinations in {watch.ElapsedMilliseconds}ms");
    }

    private static HashSet<string> FindCombinationsWithPart(int partIndex, List<string> parts)
    {
        var part = parts[partIndex];
        if (part.Length >= combinationLength)
            return new(); //part is too long to make a combination
        var combinations = new HashSet<string>();
        int partNeighbourOffset = 1;
        string neighbourPart = parts[partIndex + partNeighbourOffset];
        while (neighbourPart.StartsWith(part))
        {
            if (neighbourPart.Length == combinationLength)
            {
                string secondHalf = neighbourPart.Substring(part.Length, combinationLength - part.Length);
                if (parts.Contains(secondHalf))
                {
                    string combi = $"{part}+{secondHalf}={neighbourPart}";
                    combinations.Add(combi);
                }
                foreach (var combi in FindCombinationsOfWord(secondHalf, parts))
                    combinations.Add($"{part}+{combi}={neighbourPart}");
            }
            partNeighbourOffset++;
            if (partIndex + partNeighbourOffset >= parts.Count - 1)
                break;
            neighbourPart = parts[partIndex + partNeighbourOffset];
        }
        return combinations;
    }

    private static HashSet<string> FindCombinationsOfWord(string word, List<string> parts)
    {
        var combinations = new HashSet<string>();
        if (word.Length < 2)
        {
            combinations.Add(word);
            return combinations;
        }
        List<string> wordParts = new();
        foreach (var part in parts)
        {
            if (word.Contains(part))
                wordParts.Add(part);
        }
        foreach (var wordPart in wordParts)
        {
            if (!word.StartsWith(wordPart))
                continue;
            string remainingWord = word.Substring(wordPart.Length);
            if (string.IsNullOrEmpty(remainingWord))
                combinations.Add(wordPart);
            else
                foreach (var combi in FindCombinationsOfWord(remainingWord, wordParts))
                    combinations.Add($"{word.Substring(0, wordPart.Length)}+{combi}");
        }
        return combinations;
    }
}