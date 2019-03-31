using System;
using System.Collections.Generic;
using System.Linq;

namespace Npuzzle
{
    public enum Mode
    {
        Equals = 1,
        Manhattan = 2,
        Euclidean = 3
    }

    public static class Helper
    {
        public static Mode Mode { get; set; }
<<<<<<< HEAD
        public static int Columns { get; set; }
        public static bool GetOut { get; set; }
=======
        public static int Columns { get; set; }
        public static bool GetOut { get; set; }
>>>>>>> b3835595f101e9854e600dcbcc4be7e7ffc628c5
        public static bool Loading { get; set; }

        public static bool CheckPuzzleNumber(this int[] puzzle)
        {
            var tmp = puzzle.OrderBy(p => p).ToList();

            //tmp.OrderBy(p => p);

            for (int i = 0; i < tmp.Count; i++)
            {
                if (tmp[i] != i) { return false; }
            }

            return true;
        }

        public static Node GetCurrent(this List<Node> nodes)
        {
            var minF = nodes.Select(o => o.f).DefaultIfEmpty().Min();

            return nodes.FirstOrDefault(o => o.f == minF);
        }

        public static bool ContainsNode(this List<Node> nodes, Node find)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].IsSame(find.Puzzle)) { return true; }

            }

            return false;
        }

        public static int[] GetGoalState(int length)
        {
            var result = new int[length];

            result[length - 1] = 0;

            for (int i = 0; i < length - 1; i++)
            {
                result[i] = i + 1;
            }

            return result;
        }

        public static void SetResult(this List<Node> result, Node node)
        {
            Node current = node;

            result.Add(current);

            while (current.Parent != null)
            {
                current = current.Parent;
                result.Add(current);
            }
        }

        public static void Print(this Node node)
        {
            Console.WriteLine();

            int m = 0;

            for (int i = 0; i < Columns; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Console.Write($"{node.Puzzle[m++]} ");
                }

                Console.WriteLine();
            }
        }

        public static void ToRight(this Node parent, int i, int[] goalState)
        {
            if (i % Columns < Columns - 1)
            {
                int[] copyPuzzle = new int[parent.Puzzle.Length];

                copyPuzzle.Copy(parent.Puzzle);
                copyPuzzle.Swap(i, i + 1);

                parent.Children.Add(CopyNode(parent, copyPuzzle, goalState));
            }
        }

        public static void ToLeft(this Node parent, int i, int[] goalState)
        {
            if (i % Columns > 0)
            {
                int[] copyPuzzle = new int[parent.Puzzle.Length];

                copyPuzzle.Copy(parent.Puzzle);
                copyPuzzle.Swap(i, i - 1);

                parent.Children.Add(CopyNode(parent, copyPuzzle, goalState));
            }
        }

        public static void ToUp(this Node parent, int i, int[] goalState)
        {
            if (i - Columns >= 0)
            {
                int[] copyPuzzle = new int[parent.Puzzle.Length];

                copyPuzzle.Copy(parent.Puzzle);
                copyPuzzle.Swap(i, i - Columns);

                parent.Children.Add(CopyNode(parent, copyPuzzle, goalState));
            }
        }

        public static void ToDown(this Node parent, int i, int[] goalState)
        {
            if (i + Columns < parent.Puzzle.Length)
            {
                int[] copyPuzzle = new int[parent.Puzzle.Length];

                copyPuzzle.Copy(parent.Puzzle);
                copyPuzzle.Swap(i, i + Columns);

                parent.Children.Add(parent.CopyNode(copyPuzzle, goalState));
            }
        }

        private static void Copy(this int[] to, int[] from)
        {
            for (int i = 0; i < from.Length; i++)
            {
                to[i] = from[i];
            }
        }

        private static void Swap(this int[] puzzle, int from, int to)
        {
            int tmp = puzzle[to];

            puzzle[to] = puzzle[from];
            puzzle[from] = tmp;
        }

        private static Node CopyNode(this Node parent, int[] puzzle, int[] goalState)
        {
            return new Node(puzzle)
            {
                Parent = parent,
                g = parent.g + 1,
                h = puzzle.GetH(goalState)
            };

        }

        private static int GetH(this int[] current, int[] goalState)
        {
            var result = 0;

            if (Mode == Mode.Equals)
            {
                for (int i = 0; i < goalState.Length; i++)
                {
                    if (current[i] != goalState[i] && goalState[i] != 0)
                    {
                        result++;
                    }
                }
            }
            else if (Mode == Mode.Manhattan)
            {
                for (int i = 0; i < goalState.Length; i++)
                {
                    if (current[i] != 0)
                    {
                        var number = current[i];

                        var correctX = (number - 1) / Columns;
                        var correctY = (number - 1) % Columns;

                        var x = i / Columns;
                        var y = i % Columns;

                        result += Math.Abs(correctX - x) + Math.Abs(correctY - y);
                    }
                }
            }
            else if (Mode == Mode.Euclidean)
            {
                for (int i = 0; i < goalState.Length; i++)
                {
                    if (current[i] != 0)
                    {
                        var number = current[i];

                        var correctX = (number - 1) / Columns;
                        var correctY = (number - 1) % Columns;

                        var x = i / Columns;
                        var y = i % Columns;

                        result += (correctX - x) * (correctX - x) + (correctY - y)* (correctY - y);
                    }
                }
            }
            else
            {
                Console.WriteLine("Can't get mode from GetH method");
                return int.MaxValue;
            }

            return result;
        }
    }
}