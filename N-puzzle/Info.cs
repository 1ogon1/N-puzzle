using System;
using System.Collections.Generic;

namespace Npuzzle
{
    public class Info
    {
        public Info()
        {
            Iterations = 0;
            Result = new List<Node>();
        }

		public int AllMoves { get; set; }
        public int Iterations { get; set; }
        public int MaxOpenCount { get; set; }
           
        public List<Node> Result { get; set; }
    }
}
