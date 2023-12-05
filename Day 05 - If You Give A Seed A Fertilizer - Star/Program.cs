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

long lowest = long.MaxValue;

for (int seedRange = 0; seedRange < seeds.Length; seedRange += 2)
{
    Console.WriteLine("SeedRange: " + seedRange);

    var length = seeds[seedRange + 1];

    for (long cnt = 0; cnt < length; cnt++)
    {
        var seed = seeds[seedRange] + cnt;

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

        if (lowest > nextStage)
        {
            lowest = nextStage;
        }
    }
}

Console.WriteLine("-----");

Console.WriteLine(lowest);