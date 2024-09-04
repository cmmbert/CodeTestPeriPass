const int combinationLength = 6;
const string inputFilePath = "Resources/foobar.txt";

if (!File.Exists(inputFilePath))
{
    Console.WriteLine("Wrong file path!");
    return;
}

//HashSet to get rid of duplicate values;
HashSet<string> partsSet = File.ReadLines(inputFilePath).ToHashSet();
List<string> parts = partsSet.OrderBy(x => x).ThenBy(x => x.Length).ToList(); //Order alphabetically and length and convert to list for easier manipulation
for (int i = 0; i < parts.Count-1; i++)
{
    var part = parts[i];
    if (part.Length >= combinationLength) 
        continue; //part is too long to make a combination
    int partIndexOffset = 1;
    string currentWord = parts[i + partIndexOffset];
    while (currentWord.StartsWith(part))
    {
        if (currentWord.Length == combinationLength)
        {
            string secondHalf = currentWord.Substring(part.Length, combinationLength - part.Length);
            if (parts.Contains(secondHalf))
                Console.WriteLine($"{part}+{secondHalf}={currentWord}");
        }
        partIndexOffset++;
        if (i + partIndexOffset >= parts.Count - 1)
            break;
        currentWord = parts[i + partIndexOffset];
    }
}
return;