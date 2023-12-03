using System.Text.RegularExpressions;

bool test = false;

string[] allLines = File.ReadAllLines(test ? @"..\..\..\test.txt" : @"..\..\..\input.txt");

int width = allLines[0].Length;
int height = allLines.Length;

int?[,] numberIndexMap = new int?[height, width];
List<int> numbers = new List<int>();

Regex numberMatcher = new Regex("([0-9]+)", RegexOptions.Compiled);

int numberIndex = 0;
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
            numberIndexMap[i, walker] = numberIndex;
        }

        numbers.Add(int.Parse(text));
        numberIndex++;
    }
}

int result = 0;

for (int i = 0; i < height; i++)
{
    for (int j = 0; j < width; j++)
    {
        if (allLines[i][j] == '*')
        {
            HashSet<int> foundIndexes = new HashSet<int>();

            GetSurround(new Coords { X = i, Y = j }, width, height).ForEach(c =>
            {
                if (numberIndexMap[c.X, c.Y].HasValue)
                {
                    foundIndexes.Add(numberIndexMap[c.X, c.Y]!.Value);
                }
            });

            if (foundIndexes.Count == 2)
            {
                var val1 = numbers[foundIndexes.First()];
                var val2 = numbers[foundIndexes.Last()];

                result += val1 * val2;

                Console.WriteLine($"{val1} * {val2} = {val1 * val2}");
            }
        }
    }
}

Console.WriteLine();
Console.WriteLine(result);

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
