using Kse.Algorithms.Samples;

var timesForProgram = 3;
int comparingAlgorithm = 0;

for (int program = 0; program < timesForProgram; program++)
{
    var map_height = 100;
    var map_width = 100;
    
    var generator = new MapGenerator(new MapGeneratorOptions()
        {
            Height = map_height,
            Width = map_width,
            //Seed = 1337,
            Noise = 0.2f,
            AddTraffic = true,
        });

    const string wall = "█";
    const string space = " ";
    
    
    Point start = new Point(0, 0);
    Point goal = new Point(47, 42);

    string[,] map = generator.Generate();
    while (map[start.Column, start.Row] == wall)
    {
        Random random;
        random = new Random();
        start = new Point(random.Next(map_width), random.Next(map_height));
    }
    while (map[goal.Column, goal.Row] == wall)
    {
        Random random;
        random = new Random();
        goal = new Point(random.Next(map_width), random.Next(map_height));
    }

    

    

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

    List<Point> AStarGetShortestPath(string[,] map, Point start, Point goal)
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

    List<Point> dijkstraGetShortestPath(string[,] map, Point start, Point goal)
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
                    var traffic1 = 1;
                    if (traffic != space && traffic != wall)
                    {
                        traffic1 = Int32.Parse(traffic);
                    }
                
                    distances.Add(neighbour, traffic1 + distances[position]);
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

    var AStarShortestPath = AStarGetShortestPath(map, start, goal);
    var dijkstraShortestPath = dijkstraGetShortestPath(map, start, goal);
    
    var roadAStar = 0;
    var roadDijkstra = 0;

    for (var row = 0; row < map.GetLength(1); row++)
    {
        
        for (var column = 0; column < map.GetLength(0); column++)
        {
            Point pix = new Point(column, row);
            if (AStarShortestPath.Contains(pix))
            {
                if (pix.Column == start.Column && pix.Row == start.Row)
                {
                    roadAStar += 0;
                }
                else
                {
                    roadAStar += 1;
                    
                }
            }
            if (dijkstraShortestPath.Contains(pix))
            {
                if (pix.Column == start.Column && pix.Row == start.Row)
                {
                    roadDijkstra += 0;
                }
                else
                {
                    roadDijkstra += 1;
                    
                }
            }
        }
    }
    
    new MapPrinter().Print(map, AStarShortestPath, start, goal);
    Console.WriteLine();

    if (roadDijkstra < roadAStar)
    {
        Console.WriteLine($"Dijkstra better - {roadDijkstra} steps");
        Console.WriteLine($"A-Star - {roadAStar} steps");
        comparingAlgorithm -= 1;
    }
    else if (roadDijkstra > roadAStar)
    {
        Console.WriteLine($"A-Star better - {roadAStar} steps");
        Console.WriteLine($"Dijkstra - {roadDijkstra} steps");
        comparingAlgorithm += 1;
    }
    else
    {
        Console.WriteLine($"Same count in steps - {roadAStar}");
    }
}

if (comparingAlgorithm < 0)
{
    Console.WriteLine($"Dijkstra better");
}
else if (comparingAlgorithm > 0)
{
    Console.WriteLine($"A-Star better");
}
else
{
    Console.WriteLine("Same results");
}
