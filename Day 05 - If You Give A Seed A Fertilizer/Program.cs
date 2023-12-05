using System.Text.RegularExpressions;

bool test = false;

string input = File.ReadAllText(test ? @"..\..\..\test.txt" : @"..\..\..\input.txt");
string[] inputParts = input.Split(":");

Regex numberRegex = new Regex(@"\d+", RegexOptions.Compiled);
long[] seeds = numberRegex.Matches(inputParts[1]).Select(x => long.Parse(x.Value)).ToArray();

List<List<(long Destination, long Source, long Length)>> segments = new();

for (long i = 2; i < inputParts.Length; i++)
{
    var numbers = numberRegex.Matches(inputParts[i]).Select(x => long.Parse(x.Value)).ToArray();

    var segment = new List<(long, long, long)>();

    for (long y = 0; y < numbers.Length; y += 3)
    {
        (long Destination, long Source, long Length) tuple = (numbers[y], numbers[y + 1], numbers[y + 2]);
        segment.Add(tuple);
    }

    segments.Add(segment);
}

List<long> finalStages = new();

foreach (long seed in seeds)
{
    long nextStage = seed;

    foreach (var segment in segments)
    {
        (long Destination, long Source, long Length)? foundMap = null;

        foreach (var change in segment)
        {
            if (nextStage >= change.Source && nextStage < change.Source + change.Length)
            {
                foundMap = change;
                break;
            }
        }

        if (foundMap.HasValue)
        {
            nextStage = foundMap.Value.Destination + (nextStage - foundMap.Value.Source);
        }
    }

    finalStages.Add(nextStage);
}

// Debug
for (long i = 0; i < seeds.Length; i++)
{
    Console.WriteLine("Seed: " + (i + 1) + " - " + seeds[i]);
}

foreach(var segment in segments)
{
    Console.WriteLine("Segment: " + string.Join(" ", segment.Select(s => $"({s.Destination}, {s.Source}, {s.Length})")));
}

// Output
Console.WriteLine("-----");

Console.WriteLine(string.Join(" ", finalStages));

Console.WriteLine(finalStages.Min());