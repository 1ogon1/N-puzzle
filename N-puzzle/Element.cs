using System;
using System.Collections.Generic;
using System.Linq;

namespace Npuzzle
{
    public class Node
    {
        public int X;
        public int g;
        public int h;
        public int f => g + h;
        //public int Columns;
        public int[] Puzzle;
        public Node Parent;
        //public bool Correct = false;
        public List<Node> Children = new List<Node>();

        public Node(int[] puzzle)
        {
            //var result = Math.Sqrt(puzzle.Length);

            //if (int.TryParse(result.ToString(), out int column) && puzzle.CheckPuzzleNumber())
            //{
                //Correct = true;
                //Columns = column;
                //Helper.Columns = column;

                Puzzle = new int[puzzle.Length];

                Puzzle = puzzle;
            //}
        }

        public bool Correct()
        {
            if (int.TryParse(Math.Sqrt(Puzzle.Length).ToString(), out int column) && Puzzle.CheckPuzzleNumber())
            {
                //Columns = column;
                Helper.Columns = column;

                return true;
            }

            return false;
        }

        // This function returns true 
        // if given 8 puzzle is solvable. 
        public bool IsSolvable()
        {
            int inversions = 0;

            for (int i = 0; i < Puzzle.Length - 1; i++)
            {
                // Check if a larger number exists after the current
                // place in the array, if so increment inversions.
                for (int j = i + 1; j < Puzzle.Length; j++)
                {
                    if (Puzzle[i] > Puzzle[j])
                    {
                        inversions++;
                    }
                }

                // Determine if the distance of the blank space from the bottom 
                // right is even or odd, and increment inversions if it is odd.
                if (Puzzle[i] == 0 && i % 2 == 1)
                {
                    inversions++;
                }
            }

            //Console.WriteLine($"inversions: {inversions}");

            // If inversions is even, the puzzle is solvable.
            if (Helper.Columns % 2 == 0)
            {
                return true;
            }
            else
            {
                return (inversions % 2 == 0);
            }
        }

        public bool Goal()
        {
            for (int i = 0; i < Helper.GoalState.Length; i++)
            {
                if (Puzzle[i] != Helper.GoalState[i]) { return false; }
            }

            return true;
        }

        public bool IsSame(int[] puzzle)
        {
            for (int i = 0; i < puzzle.Length; i++)
            {
                if (Puzzle[i] != puzzle[i]) { return false; }
            }

            return true;
        }

        public void Expand()
        {
            for (int i = 0; i < Puzzle.Length; i++)
            {
                if (Puzzle[i] == 0)
                {
                    X = i;
                    break;
                }
            }

            this.ToRight(X);
            this.ToLeft(X);
            this.ToUp(X);
            this.ToDown(X);
        }
    }
}
