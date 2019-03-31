using System.Collections.Generic;

namespace Npuzzle
{
    public class Search
    {
        public Info ASrat(Node root)
        {
            bool goal = false;
            var timer = new Timer();
            Info result = new Info();
            List<Node> open = new List<Node>();
            List<Node> closed = new List<Node>();
            int[] goalState = Helper.GetGoalState(root.Puzzle.Length);

            root.g = 0;
            open.Add(root);
            Helper.Loading = true;

            if (root.Goal(goalState))
            {
                result.Iterations = 0;
                result.Result.SetResult(root);

                return result;
            }

            result.MaxOpenCount = 1;

            while (open.Count > 0 && !goal)
            {
                Node current = open.GetCurrent();

                closed.Add(current);
                open.Remove(current);

                current.Expand(goalState);

                for (int i = 0; i < current.Children.Count; i++)
                {
                    Node child = current.Children[i];

                    if (child.Goal(goalState))
                    {
                        goal = true;
                        result.Result.SetResult(child);
                    }

                    if (!open.ContainsNode(child) && !closed.ContainsNode(child))
                    {
                        open.Add(child);
                    }
                }

                if (open.Count > result.MaxOpenCount) { result.MaxOpenCount = open.Count; }

                result.Iterations++;
            }

            result.AllMoves = open.Count + closed.Count - 1;
            Helper.Loading = false;

            return result;
        }
    }
}
