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
        public static int Columns { get; set; }
        public static bool GetOut { get; set; }
        public static bool Loading { get; set; }
        public static int[] GoalState { get; set; }

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

        public static void InitGoalState(int length)
        {
            var result = new int[length];
            int k = 0;

            for (int i = 0; i < length; i++) { result[i] = 0; }

            for (int i = 0; i < Columns / 2; i++)
            {
                //from left to right
                for (int j = i; j < Columns; j++)
                {
                    if (result[j + i * Columns] == 0)
                    {
                        result[j + i * Columns] = ++k;
                    }
                }

                //from up do down
                int z = i + 1;
                while (z < Columns)
                {
                    if (result[z * Columns + Columns - i - 1] == 0)
                    {
                        result[z * Columns + Columns - i - 1] = ++k;
                    }
                    z++;
                }

                //form right to left
                for (int j = length - (Columns * i); j > length - Columns - (Columns * i); j--)
                {
                    if (result[j - 1] == 0)
                    {
                        result[j - 1] = ++k;
                    }
                }

                //from down to up
                z = Columns;
                while(z > 0)
                {
                    if (result[z * Columns - Columns + i] == 0)
                    {
                        result[z * Columns - Columns + i] = ++k;
                    }
                    z--;
                }
            }

            for (int i = 0; i < length; i++)
            {
                if (result[i] == length)
                {
                    result[i] = 0;
                }
            }
            //result[length - 1] = 0;

            //for (int i = 0; i < length - 1; i++)
            //{
            //    result[i] = i + 1;
            //}

            GoalState = result;
            new Node(GoalState).Print();
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

        public static void ToRight(this Node parent, int i)
        {
            if (i % Columns < Columns - 1)
            {
                int[] copyPuzzle = new int[parent.Puzzle.Length];

                copyPuzzle.Copy(parent.Puzzle);
                copyPuzzle.Swap(i, i + 1);

                parent.Children.Add(CopyNode(parent, copyPuzzle));
            }
        }

        public static void ToLeft(this Node parent, int i)
        {
            if (i % Columns > 0)
            {
                int[] copyPuzzle = new int[parent.Puzzle.Length];

                copyPuzzle.Copy(parent.Puzzle);
                copyPuzzle.Swap(i, i - 1);

                parent.Children.Add(CopyNode(parent, copyPuzzle));
            }
        }

        public static void ToUp(this Node parent, int i)
        {
            if (i - Columns >= 0)
            {
                int[] copyPuzzle = new int[parent.Puzzle.Length];

                copyPuzzle.Copy(parent.Puzzle);
                copyPuzzle.Swap(i, i - Columns);

                parent.Children.Add(CopyNode(parent, copyPuzzle));
            }
        }

        public static void ToDown(this Node parent, int i)
        {
            if (i + Columns < parent.Puzzle.Length)
            {
                int[] copyPuzzle = new int[parent.Puzzle.Length];

                copyPuzzle.Copy(parent.Puzzle);
                copyPuzzle.Swap(i, i + Columns);

                parent.Children.Add(parent.CopyNode(copyPuzzle));
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

        private static Node CopyNode(this Node parent, int[] puzzle)
        {
            return new Node(puzzle)
            {
                Parent = parent,
                g = parent.g + 1,
                h = puzzle.GetH()
            };

        }

        private static int GetH(this int[] current)
        {
            var result = 0;

            if (Mode == Mode.Equals)
            {
                for (int i = 0; i < GoalState.Length; i++)
                {
                    if (current[i] != GoalState[i] && GoalState[i] != 0)
                    {
                        result++;
                    }
                }
            }
            else if (Mode == Mode.Manhattan)
            {
                for (int i = 0; i < GoalState.Length; i++)
                {
                    if (current[i] != 0)
                    {
                        var goalPosition = GetGoalPosition(current[i]);

                        var correctX = goalPosition / Columns;
                        var correctY = goalPosition % Columns;

                        var x = i / Columns;
                        var y = i % Columns;

                        result += Math.Abs(correctX - x) + Math.Abs(correctY - y);
                    }
                }
            }
            else if (Mode == Mode.Euclidean)
            {
                for (int i = 0; i < GoalState.Length; i++)
                {
                    if (current[i] != 0)
                    {
                        var goalPosition = GetGoalPosition(current[i]);

                        var correctX = goalPosition / Columns;
                        var correctY = goalPosition % Columns;

                        var x = i / Columns;
                        var y = i % Columns;

                        result += (correctX - x) * (correctX - x) + (correctY - y) * (correctY - y);
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

        private static int GetGoalPosition(int number)
        {
            for (int i = 0; i < GoalState.Length; i++)
            {
                if (GoalState[i] == number)
                {
                    return i;
                }
            }

            return 0;
        }
    }
}