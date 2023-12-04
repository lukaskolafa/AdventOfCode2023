using System.Text.RegularExpressions;

bool test = false;

string[] allLines = File.ReadAllLines(test ? @"..\..\..\test.txt" : @"..\..\..\input.txt");

Regex splitter = new Regex(@"^Card\ +(?<id>\d+):(?<rnd>[0-9\ ]+)\|(?<win>[0-9\ ]+)$", RegexOptions.Compiled);

double result = 0;

for (int i = 0; i < allLines.Length; i++)
{
    var line = allLines[i];

    var split = splitter.Match(line);

    var id = int.Parse(split.Groups["id"].Value);
    var rnd = split.Groups["rnd"].Value.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
    var win = split.Groups["win"].Value.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
    var intersect = rnd.Intersect(win).ToArray();

    if (intersect.Length > 0)
    {
        var points = Math.Pow(2, (intersect.Length - 1));

        result += points;
        Console.WriteLine("POINTS: " + points);
    }

    Console.WriteLine("ID: " + id);
    Console.WriteLine("RND: " + string.Join(", ", rnd));
    Console.WriteLine("WIN: " + string.Join(", ", win));
}

Console.WriteLine("----");
Console.WriteLine(result);

