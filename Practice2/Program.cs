using Kse.Algorithms.Samples;

var generator = new MapGenerator(new MapGeneratorOptions()
{
    Height = 30,
    Width = 30,
    Seed = 1337,
});

string[,] map = generator.Generate();
new MapPrinter().Print(map);

const string Wall = "█";
const string Space = " ";

Point start = new Point(4, 12);
Point goal = new Point(23, 22);

//List<Point> neighbours = new List<Point>();

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
            if (maze[newColumn, newRow] == Space)
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
                distances.Add(neighbour, 1 + distances[position]);
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
    return Path;
}
//make path out of what is in origin