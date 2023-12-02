using System.Text.RegularExpressions;

bool test = false;

string[] allLines = File.ReadAllLines(test ? @"..\..\..\test.txt" : @"..\..\..\input.txt");

int result = 0;

for (int i = 0; i < allLines.Length; i++)
{
    var line = allLines[i];
    var firstDigitMatch = Regex.Match(line, "^[^0-9]*([0-9]).*");
    var lastDigitMatch = Regex.Match(line, ".*([0-9])[^0-9]*$");

    var firstDigit = firstDigitMatch.Groups[1].Value;
    var lastDigit = lastDigitMatch.Groups[1].Value;

    result += int.Parse(firstDigit + lastDigit);

    Console.WriteLine(firstDigit + lastDigit);
}

Console.WriteLine(result);