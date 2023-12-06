using System.Text.RegularExpressions;

bool test = false;

var input = File.ReadLines(test ? @"..\..\..\test.txt" : @"..\..\..\input.txt").ToArray();

Regex numberRegex = new Regex(@"\d+", RegexOptions.Compiled);

long[] times = numberRegex.Matches(input.First().Replace(" ", string.Empty)).Select(x => long.Parse(x.Value)).ToArray();
long[] records = numberRegex.Matches(input.Last().Replace(" ", string.Empty)).Select(x => long.Parse(x.Value)).ToArray();

long result = 1;

for (int i = 0; i < times.Length; i++)
{
    long raceSuccessCount = 0;

    long optimum = times[i] / 2; // This is calculated by derivation of the distance function.

    // Count of steps to reach record, we could use compare function, but we are lazy, we just count up.
    // We count from optimum both ways up and down (optimization could be to count once and deal with the central point properly)
    long holdMs = optimum;
    while (true)
    {
        if (Distance(holdMs, times[i]) > records[i])
        {
            raceSuccessCount++;
        }
        else
        {
            break;
        }

        holdMs++;
    }

    holdMs = optimum - 1;

    while (true)
    {
        if (Distance(holdMs, times[i]) > records[i])
        {
            raceSuccessCount++;
        }
        else
        {
            break;
        }

        holdMs--;
    }

    Console.WriteLine("Race: " + (i + 1) + " - " + raceSuccessCount);

    result *= raceSuccessCount;
}

Console.WriteLine(result);

double Distance(long holdMs, long totalMs)
{
    return holdMs * (totalMs - holdMs);
}