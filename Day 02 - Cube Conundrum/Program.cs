﻿using System.Text.RegularExpressions;

bool test = false;

string[] allLines = File.ReadAllLines(test ? @"..\..\..\test.txt" : @"..\..\..\input.txt");

HashSet<int> validSets = new HashSet<int>(allLines.Length);

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

    foreach (var set in sets)
    {
        Console.WriteLine("---");

        var split2 = set.Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        foreach (var item in split2)
        {
            var split3 = item.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            var color = split3[1];
            var count = int.Parse(split3[0]);

            if ((color == "red" && count > 12) || (color == "blue" && count > 14) || (color == "green" && count > 13))
            {
                valid = false;
                break;
            }
        }
    }

    if (valid)
    {
        validSets.Add(id);
    }
}

Console.WriteLine(validSets.Sum());
