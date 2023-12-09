using System.Text.RegularExpressions;

bool test = false;

// Too low 919

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

    for (int i = 0; i < row.Length - 1; i++)
    {
        var newRow = new List<long>();
        pyramid.Add(newRow);
        height++; // faster then counting pyramid.Count

        long diff = 0;

        for (int j = 0; j < height; j++)
        {
            if (j == 0)
            {
                diff = row[i + 1] - row[i];
            }
            else
            {
                diff = pyramid[j - 1][height - j] - pyramid[j - 1][height - j - 1];
            }

            pyramid[j].Add(diff);
        }

        // no breaking if 0, even in case two in a row, we got bad result
    }

    // Debug
    Console.WriteLine("====================================");
    Console.WriteLine(string.Join(",", row));
    foreach (var pyramidRow in pyramid)
    {
        Console.WriteLine(string.Join(",", pyramidRow));
    }
    Console.WriteLine();

    long nextInRow = 0;
    for (int h = height - 1; h >= 0; h--)
    {
        if (h > 0)
        {
            nextInRow = pyramid[h - 1][0] - nextInRow;
        }
        else
        {
            nextInRow = row[0] - nextInRow;
        }

        Console.Write(nextInRow + ", ");
    }

    result += nextInRow;
    Console.WriteLine();
}

Console.WriteLine("====================================");
Console.WriteLine(result);