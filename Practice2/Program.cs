using Kse.Algorithms.Samples;

var generator = new MapGenerator(new MapGeneratorOptions()
{
    Height = 60,
    Width = 60,
    Seed = 1337,
    AddTraffic = true,
});

Point start = new Point(4, 12);
Point goal = new Point(47, 42);

string[,] map = generator.Generate();

const string wall = "█";
const string space = " ";

List<Point> GetNeighbours(Point start, string[,] maze)
{
    var result = new List<Point>();
    TryAddWithOffset(1, 0);
    TryAddWithOffset(-1, 0);
    TryAddWithOffset(0, 1);
    TryAddWithOffset(0, -1);
    return result;

    void TryAddWithOffset(int offsetX, int offsetY)
    {
        var newColumn = start.Column + offsetX;
        var newRow = start.Row + offsetY;
        if (newColumn >= 0 && newRow >= 0 && newColumn < maze.GetLength(0) && newRow < maze.GetLength(1))
        {
            if (maze[newColumn, newRow] != wall)
            {
                result.Add(new Point(newColumn, newRow));
            }
        }
    }
}

List<Point> GetShortestPath(string[,] map, Point start, Point goal)
{
    var distances = new Dictionary<Point, int>();
    var origins = new Dictionary<Point, Point>();
    Point position = start;
    distances.Add(start, 0);

    while (position.Column != goal.Column || position.Row != goal.Row)
    {
        var neighbours = GetNeighbours(position, map);
        foreach (var neighbour in neighbours)
        {
            if (!origins.ContainsKey(neighbour))
            {
                origins.Add(neighbour, position);
                var traffic = map[position.Column, position.Row];
                var manhattanDistance = Math.Abs(position.Column - goal.Column) + Math.Abs(position.Row - goal.Row);
                var traffic1 = 1;
                if (traffic != space && traffic != wall)
                {
                    traffic1 = Int32.Parse(traffic);
                }
                
                distances.Add(neighbour, traffic1 + distances[position] + manhattanDistance);
            }
        }
        distances.Remove(position);
        position = distances.MinBy(kvp => kvp.Value).Key;
    }

    List<Point> Path = new List<Point>();
    while (position.Column != start.Column || position.Row != start.Row)
    {
        Path.Add(position);
        position = origins[position];
    }
    Path.Add(position);
    return Path;
}

var shortestPath = GetShortestPath(map, start, goal);

var speed = 0;
var speed_zone = 0;
for (var row = 0; row < map.GetLength(1); row++)
{
    for (var column = 0; column < map.GetLength(0); column++)
    {
        Point pix = new Point(column, row);
        if (shortestPath.Contains(pix))
        {
            if (pix.Column == start.Column && pix.Row == start.Row)
            {
                speed += 0;
            }
            else
            {
                speed += 60 -(Int32.Parse(map[column, row]) - 1)*6;
                speed_zone += 1;
            }
        }
    }
}

var average_speed = speed / speed_zone;

new MapPrinter().Print(map, shortestPath, start, goal);
Console.WriteLine();
Console.WriteLine($"Average speed - {average_speed}");
//klitynka - 1 kilemeter