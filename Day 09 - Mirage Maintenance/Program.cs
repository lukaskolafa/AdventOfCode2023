using System.Text.RegularExpressions;

bool test = false;

// high 1853145141
//      1853145119

var input = File.ReadLines(test ? @"..\..\..\test.txt" : @"..\..\..\input.txt").ToArray();

List<long[]> data = new List<long[]>();

Regex numberRegex = new Regex(@"-?\d+", RegexOptions.Compiled);

foreach (string line in input)
{
    data.Add(numberRegex.Matches(line).Select(x => long.Parse(x.Value)).ToArray());
}

long result = 0;

foreach (long[] row in data)
{
    List<List<long>> pyramid = new List<List<long>>();

    int height = 0;

    bool shouldBreakNextRound = false;

    for (int i = row.Length - 1; i >= 0; i--)
    {
        var newRow = new List<long>();
        pyramid.Add(newRow);
        height++; // faster then counting pyramid.Count

        long diff = 0;

        for (int j = 0; j < height; j++)
        {
            if (j == 0)
            {
                diff = row[i] - row[i - 1];
            }
            else
            {
                diff = pyramid[j - 1][height - j - 1] - pyramid[j - 1][height - j];
            }

            pyramid[j].Add(diff);
        }

        if (shouldBreakNextRound && diff == 0)
        {
            break;
        }

        if (diff == 0)
        {
            shouldBreakNextRound = true; // We can break only if we have 2 zeros in a row, because one zero can be still a rising graph crossing the x axis
        }
        else
        {
            shouldBreakNextRound = false;
        }
    }

    // Debug
    Console.WriteLine("====================================");
    Console.WriteLine(string.Join(",", row));
    foreach (var pyramidRow in pyramid)
    {
        Console.WriteLine(string.Join(",", pyramidRow));
    }
    Console.WriteLine();

    long sum = 0;
    for (int h = height - 1; h >= 0; h--)
    {
        if (h > 0)
        {
            sum += pyramid[h - 1][0];
        }
        else
        {
            sum += row[^1];
        }
    }

    Console.WriteLine(sum);

    result += sum;
}

Console.WriteLine("====================================");
Console.WriteLine(result);