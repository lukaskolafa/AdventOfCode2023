using System.Text.RegularExpressions;

bool test = false;

string[] allLines = File.ReadAllLines(test ? @"..\..\..\test.txt" : @"..\..\..\input.txt");

int result = 0;

for (int i = 0; i < allLines.Length; i++)
{
    var line = allLines[i];

    // First index of a string in string
    var firstDigitMatch = first(line, "([0-9]|one|two|three|four|five|six|seven|eight|nine)");
    var lastDigitMatch = last(line, "([0-9]|one|two|three|four|five|six|seven|eight|nine)");

    var firstDigit = firstDigitMatch.Replace("one", "1").Replace("two", "2").Replace("three", "3").Replace("four", "4").Replace("five", "5").Replace("six", "6").Replace("seven", "7").Replace("eight", "8").Replace("nine", "9");
    var lastDigit = lastDigitMatch.Replace("one", "1").Replace("two", "2").Replace("three", "3").Replace("four", "4").Replace("five", "5").Replace("six", "6").Replace("seven", "7").Replace("eight", "8").Replace("nine", "9");

    result += int.Parse(firstDigit + lastDigit);

    Console.WriteLine(firstDigit + lastDigit);
}

Console.WriteLine(result);

string first(string input, string pattern)
{
    for (int i = 0; i < input.Length; i++)
    {
        Regex regex = new Regex("^.{" + i + "}" + pattern);

        if (regex.IsMatch(input))
        {
            return regex.Match(input).Groups[1].Value;
        }
    }

    throw new Exception("No match found");
}

string last(string input, string pattern)
{
    for (int i = 0; i < input.Length; i++)
    {
        Regex regex = new Regex(pattern + ".{" + i + "}$");

        if (regex.IsMatch(input))
        {
            return regex.Match(input).Groups[1].Value;
        }
    }

    throw new Exception("No match found");
}