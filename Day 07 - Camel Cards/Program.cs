using System.Text.RegularExpressions;

// too high 253776577

bool test = false;

var input = File.ReadLines(test ? @"..\..\..\test.txt" : @"..\..\..\input.txt").ToArray();

List<Hand> data = new List<Hand>();

Regex lineParser = new Regex(@"^(?<hand>.{5})\ (?<bid>\d+)$", RegexOptions.Compiled);

foreach (var line in input)
{
    var match = lineParser.Match(line);
    data.Add(new Hand(match.Groups["hand"].Value, int.Parse(match.Groups["bid"].Value)));
}

double result = 0;
int rank = 1;
foreach (var rankedHand in data.Order())
{
    Console.WriteLine($"Rank: {rank} Hand: {rankedHand.OriginalHand} HandValue: {rankedHand.Value} SortedHand: {rankedHand.SortedHand} Bid: {rankedHand.Bid} ");

    result += rank * rankedHand.Bid;
    rank++;
}

Console.WriteLine(result);

public class Card : IComparable
{
    static readonly char[] CardValues = {'A', 'K', 'Q', 'J', 'T', '9', '8', '7', '6', '5', '4', '3', '2' };

    public char Symbol { get; }

    public int Value => CardValues.Length - Array.IndexOf(CardValues, this.Symbol);

    public Card(char symbol)
    {
        this.Symbol = symbol;
    }

    public int CompareTo(object? obj)
    {
        var cmp = obj as Card;

        if (cmp == null)
        {
            throw new Exception("Invalid comparison");
        }

        return this.Value.CompareTo(cmp.Value);
    }
}

public class Hand : IComparable
{
    public string OriginalHand { get; }

    public string SortedHand { get; }

    public int Value { get; }

    public int Bid { get; }

    public int[] Counts { get; }

    public Hand(string data, int bid)
    {
        this.Bid = bid;
        this.OriginalHand = data;
        this.SortedHand = string.Join(string.Empty, this.OriginalHand.Select(x => new Card(x)).OrderDescending().Select(x => x.Symbol));

        int[] counts = new int[5];
        int lastValue = -1;
        int position = -1;
        for (int index = 0; index < 5; index++)
        {
            int newValue = new Card(this.SortedHand[index]).Value;

            if (newValue != lastValue)
            {
                position++;
                lastValue = newValue;
            }

            counts[position]++;
        }
        this.Counts = counts.OrderDescending().ToArray();

        if (this.Counts[0] == 5) // Five of a kind
        {
            this.Value = 7;
        }
        else if (this.Counts[0] == 4) // Four of a kind
        {
            this.Value = 6;
        }
        else if (this.Counts[0] == 3 && this.Counts[1] == 2) // Full house
        {
            this.Value = 5;
        }
        else if (this.Counts[0] == 3 && this.Counts[1] == 1) // Three of a kind
        {
            this.Value = 4;
        }
        else if (this.Counts[0] == 2 && this.Counts[1] == 2) // Two pair
        {
            this.Value = 3;
        }
        else if (this.Counts[0] == 2) // One pair
        {
            this.Value = 2;
        }
        else
        {
            this.Value = 1;
        }
    }

    public int CompareTo(object? obj)
    {
        var cmp = obj as Hand;

        if (cmp == null)
        {
            throw new Exception("Invalid comparison");
        }

        int byVal = this.Value.CompareTo(cmp.Value);

        if (byVal != 0)
        {
            return byVal;
        }

        for (int i = 0; i < 5; i++)
        {
            int byCard = new Card(this.OriginalHand[i]).CompareTo(new Card(cmp.OriginalHand[i]));

            if (byCard != 0)
            {
                return byCard;
            }
        }

        throw new Exception("Equal Hands");
    }
}