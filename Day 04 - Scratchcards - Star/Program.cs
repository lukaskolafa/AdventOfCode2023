using System.Text.RegularExpressions;

bool test = false;

string[] allLines = File.ReadAllLines(test ? @"..\..\..\test.txt" : @"..\..\..\input.txt");

Regex splitter = new Regex(@"^Card\ +(?<id>\d+):(?<rnd>[0-9\ ]+)\|(?<win>[0-9\ ]+)$", RegexOptions.Compiled);

int[] countCards = new int[allLines.Length];
for (int i = 0; i < allLines.Length; i++)
{
    countCards[i] = 1;
}

for (int i = 0; i < allLines.Length; i++)
{
    var line = allLines[i];

    var split = splitter.Match(line);

    var rnd = split.Groups["rnd"].Value.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
    var win = split.Groups["win"].Value.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
    var intersect = rnd.Intersect(win).ToArray();

    for (int y = 1; y <= intersect.Length; y++)
    {
        countCards[i + y] += countCards[i];
    }
}

for (int cnt = 0; cnt < countCards.Length; cnt++)
{
    Console.WriteLine("Card: " + (cnt + 1) + " - " + countCards[cnt]);
}

Console.WriteLine("-----");

Console.WriteLine(countCards.Sum());