using System.Text.RegularExpressions;

bool test = false;

string[] allLines = File.ReadAllLines(test ? @"..\..\..\test.txt" : @"..\..\..\input.txt");

List<int> powers = new List<int>(allLines.Length);

for (int i = 0; i < allLines.Length; i++)
{
    var line = allLines[i];

    // Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green

    var split1 = line.Split(":", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    var idMatch = Regex.Match(split1[0], @"Game (\d+)");
    var sets = split1[1].Split(";", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

    var id = int.Parse(idMatch.Groups[1].Value);

    Console.WriteLine("ID: " + id);

    bool valid = true;

    int minBlue = 0;
    int minRed = 0;
    int minGreen = 0;

    foreach (var set in sets)
    {
        var split2 = set.Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        foreach (var item in split2)
        {
            var split3 = item.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            var color = split3[1];
            var count = int.Parse(split3[0]);

            if (color == "red" && count > minRed) { minRed = count; }
            if (color == "blue" && count > minBlue) { minBlue = count; }
            if (color == "green" && count > minGreen) { minGreen = count; }
        }
    }

    powers.Add(minRed * minBlue * minGreen);
    Console.WriteLine($"ID: {id} Power: {minRed * minBlue * minGreen}");
}

Console.WriteLine(powers.Sum());