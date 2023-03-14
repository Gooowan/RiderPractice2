namespace Kse.Algorithms.Samples
{
    using System;
    using System.Collections.Generic;

    public class MapPrinter
    {
        public void Print(string[,] maze, List<Point> Path, Point start, Point goal)
        {
            PrintTopLine();
            for (var row = 0; row < maze.GetLength(1); row++)
            {
                Console.Write($"{row}\t");
                
                for (var column = 0; column < maze.GetLength(0); column++)
                {
                    Console.Write(maze[column, row]);
                    /*
                    Point pix = new Point(column, row);
                    if (Path.Contains(pix))
                    {
                        if (pix.Column == start.Column && pix.Row == start.Row )
                        {
                            Console.Write("A");
                        }
                        else if (pix.Column == goal.Column && pix.Row == goal.Row)
                        {
                            Console.Write("B");
                        }
                        else
                        {
                            Console.Write("*");
                        }

                    }
                    else
                    {
                        Console.Write(maze[column, row]);
                    }
                    */
                }

                Console.WriteLine();
            }

            void PrintTopLine()
            {
                Console.Write($" \t");
                for (int i = 0; i < maze.GetLength(0); i++)
                {
                    Console.Write(i % 10 == 0? i / 10 : " ");
                }
    
                Console.Write($"\n \t");
                for (int i = 0; i < maze.GetLength(0); i++)
                {
                    Console.Write(i % 10);
                }
    
                Console.WriteLine("\n");
            }
        }
    }
}