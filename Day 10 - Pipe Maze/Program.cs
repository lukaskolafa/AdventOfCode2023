bool test = false;

var input = File.ReadLines(test ? @"..\..\..\test.txt" : @"..\..\..\input.txt").ToArray();

int?[,] distances = new int?[input.Length, input[0].Length];
Connection?[,] connections = new Connection?[input.Length, input[0].Length];

int startHeight = 0;
int startWidth = 0;

for (int h = 0; h < input.Length; h++)
{
    for (int w = 0; w < input[0].Length; w++)
    {
        if (input[h][w] == 'S')
        {
            distances[h, w] = 0;

            startHeight = h;
            startWidth = w;
        }

        connections[h, w] = new Connection(input[h][w]);
    }
}

(int Height, int Width)[] walkers = new (int Height, int Width)[2];
int walkersCount = 0;

var tmp = ConnectionPointExistFrom(startHeight, startWidth, Direction.North);
if (tmp != null) { walkers[walkersCount] = tmp.Value; walkersCount++; }

tmp = ConnectionPointExistFrom(startHeight, startWidth, Direction.South);
if (tmp != null) { walkers[walkersCount] = tmp.Value; walkersCount++; }

tmp = ConnectionPointExistFrom(startHeight, startWidth, Direction.West);
if (tmp != null) { walkers[walkersCount] = tmp.Value; walkersCount++; }

tmp = ConnectionPointExistFrom(startHeight, startWidth, Direction.East);
if (tmp != null) { walkers[walkersCount] = tmp.Value; walkersCount++; }

if (walkersCount != 2)
{
    throw new Exception("incorrect walkers");
}

distances[walkers[0].Height, walkers[0].Width] = 1;
distances[walkers[1].Height, walkers[1].Width] = 1;

int maxDistance = 0;

while (true)
{
    bool again = false;

    for (int i = 0; i < walkersCount; i++)
    {
        var walker = walkers[i];

        // Console.WriteLine("Walker: " + walker);

        var directions = connections[walker.Height, walker.Width]!.Directions;
        var currentDistance = distances[walker.Height, walker.Width]!.Value;

        foreach (var direction in directions)
        {
            var candidate = ConnectionPointExistFrom(walker.Height, walker.Width, direction);
            if (candidate != null)
            {
                var futureDistance = distances[candidate.Value.Height, candidate.Value.Width];

                if (futureDistance != null && futureDistance <= currentDistance)
                {
                    continue;
                }

                distances[candidate.Value.Height, candidate.Value.Width] = currentDistance + 1;
                walkers[i] = candidate.Value;
                again = true;

                if (currentDistance >= maxDistance)
                {
                    maxDistance++;
                }
            }
        }
    }

    if (!again)
    {
        break;
    }

    // PrintDistanceMatrix();
}

PrintLoop();
Console.WriteLine("====================================");
PrintSpace();

Console.WriteLine("====================================");
Console.WriteLine(maxDistance);


void PrintDistanceMatrix()
{
    for (int h = 0; h < input.Length; h++)
    {
        for (int w = 0; w < input[0].Length; w++)
        {
            var dist = distances[h, w];

            Console.Write(dist == null ? "." : dist);
        }

        Console.WriteLine();
    }
}

void PrintLoop()
{
    for (int h = 0; h < input.Length; h++)
    {
        for (int w = 0; w < input[0].Length; w++)
        {
            var dist = distances[h, w];

            Console.Write(dist == null ? " " : connections[h, w]!.Pipe);
        }

        Console.WriteLine();
    }
}

void PrintSpace()
{
    for (int h = 0; h < input.Length; h++)
    {
        for (int w = 0; w < input[0].Length; w++)
        {
            var dist = distances[h, w];

            Console.Write(dist == null ? "x" : " ");
        }

        Console.WriteLine();
    }
}


(int Height, int Width)? ConnectionPointExistFrom(int toH, int toW, Direction direction)
{
    switch (direction)
    {
        case Direction.North: return toH > 0 && connections[toH - 1, toW]!.Connects(Direction.South) ? (toH - 1, toW) : null ;
        case Direction.South: return toH < input.Length - 1 && connections[toH + 1, toW]!.Connects(Direction.North) ? (toH + 1, toW) : null;
        case Direction.West: return toW > 0 && connections[toH, toW - 1]!.Connects(Direction.East)? (toH, toW - 1) : null;
        case Direction.East: return toW < input[0].Length - 1 && connections[toH, toW + 1]!.Connects(Direction.West)? (toH, toW + 1) : null;
        default: throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
    }
}
public class Connection
{
    public Connection(char pipe)
    {
        this.Pipe = pipe;

        switch (pipe)
        {
            case '|': this.Directions = new [] { Direction.North, Direction.South};  break;
            case '-': this.Directions = new [] { Direction.West, Direction.East}; break;
            case 'L': this.Directions = new [] { Direction.North, Direction.East}; break;
            case 'J': this.Directions = new [] { Direction.West, Direction.North}; break;
            case '7': this.Directions = new [] { Direction.South, Direction.West}; break;
            case 'F': this.Directions = new [] { Direction.South, Direction.East}; break;
            case '.': this.Directions = Array.Empty<Direction>(); break;
            case 'S': this.Directions = Array.Empty<Direction>(); break;
            default: throw new Exception("Unknown pipe");
        }
    }

    public char Pipe { get; }

    public Direction[] Directions { get; }

    public bool Connects(Direction direction) => this.Directions.Contains(direction);
}

public enum Direction
{
    North,
    South,
    West,
    East
}
