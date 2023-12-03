using System.Text.RegularExpressions;

bool test = false;

string[] allLines = File.ReadAllLines(test ? @"..\..\..\test.txt" : @"..\..\..\input.txt");

int width = allLines[0].Length;
int height = allLines.Length;
bool[,] activeFields = new bool[height, width];

Regex symbolMatcher = new Regex("[^0-9\\.]", RegexOptions.Compiled);

for (int i = 0; i < height; i++)
{
    for (int j = 0; j < width; j++)
    {
        if (symbolMatcher.IsMatch(allLines[i][j].ToString()))
        {
            GetSurround(new Coords { X = i, Y = j }, width, height).ForEach(c => activeFields[c.X, c.Y] = true);
        }
    }
}

for (int i = 0; i < height; i++)
{
    for (int j = 0; j < width; j++)
    {
        Console.Write(activeFields[i, j] ? "#" : ".");
    }

    Console.WriteLine();
}

Console.WriteLine();

for (int i = 0; i < height; i++)
{
    Console.WriteLine(allLines[i]);
}

Regex numberMatcher = new Regex("([0-9]+)", RegexOptions.Compiled);

List<int> numbers = new List<int>();

for (int i = 0; i < height; i++)
{
    foreach (var match in numberMatcher.Matches(allLines[i]))
    {
        var parsedMatch = (match as Match)!;
        var text = parsedMatch.Value;
        var length = text.Length;
        var index = parsedMatch.Index;

        for (var walker = index; walker < index + length; walker++)
        {
            if (activeFields[i, walker])
            {
                numbers.Add(int.Parse(text));
                break;
            }
        }
    }
}

numbers.ForEach(Console.WriteLine);

Console.WriteLine();
Console.WriteLine(numbers.Sum());

List<Coords> GetSurround(Coords center, int widthParam, int heightParam)
{
    var candidates = new[]
    {
        new Coords { X = center.X - 1, Y = center.Y - 1 },
        new Coords { X = center.X, Y = center.Y - 1 },
        new Coords { X = center.X + 1, Y = center.Y - 1 },
        new Coords { X = center.X - 1, Y = center.Y },
        new Coords { X = center.X + 1, Y = center.Y },
        new Coords { X = center.X - 1, Y = center.Y + 1 },
        new Coords { X = center.X, Y = center.Y + 1 },
        new Coords { X = center.X + 1, Y = center.Y + 1 },
    };

    return candidates.Where(c => c.X >= 0 && c.X < widthParam && c.Y >= 0 && c.Y < heightParam).ToList();
}
class Coords
{
    public int X { get; set; }
    public int Y { get; set; }
}
