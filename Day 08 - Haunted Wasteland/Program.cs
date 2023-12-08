using System.Text.RegularExpressions;

bool test = false;

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

int counter = 0;
string current = "AAA";

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

        if (current == "ZZZ")
        {
            Console.WriteLine(counter);
            return;
        }
    }
}