using System.Text.RegularExpressions;

bool test = false;

// 1240643740 - low

var input = File.ReadLines(test ? @"..\..\..\test.txt" : @"..\..\..\input.txt").ToArray();

IDictionary<string, (string Left, string Right)> data = new Dictionary<string, (string, string)>();

var steps = input[0];

Regex lineParser = new Regex(@"^(?<src>.{3})\ =\ \((?<left>.{3}), (?<right>.{3})\)$", RegexOptions.Compiled);

for (int i = 2; i < input.Length; i++)
{
    var match = lineParser.Match(input[i]);
    data.Add(match.Groups["src"].Value, (match.Groups["left"].Value, match.Groups["right"].Value));
}

// debug
// foreach (var entry in data)
// {
//     Console.WriteLine($"{entry.Key} => {entry.Value.Left} {entry.Value.Right}");
// }

string[] candidates = data.Where(x => x.Key.EndsWith("A")).Select(x => x.Key).ToArray();
List<int> results = new List<int>();

for (int candidateIndex = 0; candidateIndex < candidates.Length; candidateIndex++)
{
    int counter = 0;
    string current = candidates[candidateIndex];

    while (true)
    {
        foreach (char step in steps)
        {
            if (step == 'L')
            {
                current = data[current].Left;
            }
            else
            {
                current = data[current].Right;
            }

            counter++;

            if (current.EndsWith("Z"))
            {
                results.Add(counter);
                goto end;
            }
        }
    }

    end:;
}

Console.WriteLine(LcmArray(results.Select(x => (long)x).ToArray()));

long LcmArray(long[] numbers)
{
    return numbers.Aggregate(Lcm);
}

long Lcm(long a, long b)
{
    return Math.Abs(a * b) / Gcd(a, b);
}

long Gcd(long a, long b)
{
    return b == 0 ? a : Gcd(b, a % b);
}