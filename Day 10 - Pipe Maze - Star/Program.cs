bool test = false;

var input = File.ReadLines(test ? @"..\..\..\test.txt" : @"..\..\..\input.txt").ToArray();

Status[,] path = new Status[input.Length, input[0].Length];
Connection?[,] connections = new Connection?[input.Length, input[0].Length];

int startHeight = 0;
int startWidth = 0;

for (int h = 0; h < input.Length; h++)
{
    for (int w = 0; w < input[0].Length; w++)
    {
        if (input[h][w] == 'S')
        {
            path[h, w] = Status.Start;

            startHeight = h;
            startWidth = w;
        }

        connections[h, w] = new Connection(input[h][w]);
    }
}

Direction lastDirection = Direction.West;
(int Height, int Width) firstStep = (-1, -1);
foreach (var direction in connections[startHeight, startWidth]!.Directions)
{
    (int Height, int Width)? candidate = ConnectionPointExistFrom(startHeight, startWidth, direction);

    if (candidate != null)
    {
        firstStep = candidate.Value;
        lastDirection = direction;
        break;
    }
}

(int Height, int Width) nextStep = firstStep;


PrintLoop();

while (true)
{
    bool again = false;

    path[nextStep.Height, nextStep.Width] = Status.Pipe;
    // PrintLoop();

    var directions = connections[nextStep.Height, nextStep.Width]!.Directions;

    foreach (var direction in directions)
    {
        var walk = StepFrom(nextStep.Height, nextStep.Width, direction);
        if (path[walk.Height, walk.Width] == Status.Pipe) // this would be the way back
        {
            continue;
        }

        if (path[walk.Height, walk.Width] == Status.Start) // this would be the way back or we finished the loop
        {
            continue;
        }

        MarkInnerAlongPath(nextStep.Height, nextStep.Width, direction, lastDirection);
        nextStep = walk;
        lastDirection = direction;
        again = true;
        break;
    }

    if (!again)
    {
        break;
    }
}

for (int h = 0; h < input.Length; h++)
{
    for (int w = 0; w < input[0].Length -1; w++)
    {
        if (path[h, w] == Status.Inner && path[h, w + 1] == Status.Nothing)
        {
            path[h, w + 1] = Status.Inner;
        }
    }
}

var count = 0;
for (int h = 0; h < input.Length; h++)
{
    for (int w = 0; w < input[0].Length; w++)
    {
        if (path[h, w] == Status.Inner)
        {
            count++;
        }
    }
}

PrintLoop();

Console.WriteLine(count);

void MarkInnerAlongPath(int height, int width, Direction direction, Direction lastDirection)
{
    switch (direction)
    {
        case Direction.West:
            ConditionalMarkInner(height - 1, width);

            if (lastDirection == Direction.North)
            {
                ConditionalMarkInner(height - 1, width + 1);
                ConditionalMarkInner(height, width + 1);
            }

            break;

        case Direction.East:
            ConditionalMarkInner(height + 1, width);

            if (lastDirection == Direction.South)
            {
                ConditionalMarkInner(height + 1, width - 1);
                ConditionalMarkInner(height, width - 1);
            }

            break;

        case Direction.North:
            ConditionalMarkInner(height, width + 1);

            if (lastDirection == Direction.East)
            {
                ConditionalMarkInner(height + 1, width + 1);
                ConditionalMarkInner(height + 1, width);
            }

            break;
        case Direction.South:
            ConditionalMarkInner(height, width - 1);

            if (lastDirection == Direction.West)
            {
                ConditionalMarkInner(height - 1, width - 1);
                ConditionalMarkInner(height - 1, width);
            }

            break;
        default: throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
    }
}

void ConditionalMarkInner(int height, int width)
{
    try
    {
        if (path[height, width] == Status.Nothing)
        {
            path[height, width] = Status.Inner;
        }
    }
    catch (Exception)
    {
    }
}

void PrintLoop()
{
    Console.WriteLine("====================================");
    Console.WriteLine();

    for (int h = 0; h < input.Length; h++)
    {
        for (int w = 0; w < input[0].Length; w++)
        {
            var stat = path[h, w];

            string d;

            switch(stat)
            {
                case Status.Pipe: d = "x"; break;
                case Status.Inner: d = "I"; break;
                case Status.Outer: d = "O"; break;
                case Status.Nothing: d = "."; break;
                case Status.Start: d = "S"; break;
                default: throw new ArgumentOutOfRangeException(nameof(stat), stat, null);
            }

            Console.Write(d);
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

(int Height, int Width) StepFrom(int fromH, int fromW, Direction direction)
{
    switch (direction)
    {
        case Direction.North: return (fromH - 1, fromW);
        case Direction.South: return (fromH + 1, fromW);
        case Direction.West: return (fromH, fromW - 1);
        case Direction.East: return (fromH, fromW + 1);
        default: throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
    }
}

public class Connection
{
    public Connection(char pipe)
    {
        switch (pipe)
        {
            case '|': this.Directions = new [] { Direction.North, Direction.South};  break;
            case '-': this.Directions = new [] { Direction.West, Direction.East}; break;
            case 'L': this.Directions = new [] { Direction.North, Direction.East}; break;
            case 'J': this.Directions = new [] { Direction.West, Direction.North}; break;
            case '7': this.Directions = new [] { Direction.South, Direction.West}; break;
            case 'F': this.Directions = new [] { Direction.South, Direction.East}; break;
            case '.': this.Directions = Array.Empty<Direction>(); break;
            case 'S': this.Directions = new [] { Direction.North, Direction.South, Direction.West, Direction.East }; break; // Starting point is a special case, we have to try all directions to find the first step.
            default: throw new Exception("Unknown pipe");
        }
    }

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

public enum Status
{
    Nothing,
    Inner,
    Outer,
    Pipe,
    Start
}
