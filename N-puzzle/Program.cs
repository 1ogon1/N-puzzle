using System;
using System.IO;
using System.Linq;

namespace Npuzzle
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Helper.GetOut = false;
            Node puzzle = GetPuzzle(true, string.Empty, null, "Hello. First choose mode", out int mode);

            if (puzzle != null)
            {
                //puzzle.Print();

                var inv = puzzle.IsSolvable();

                //Console.WriteLine($"IsSolvable: {inv}");
                //Console.ReadLine();

                if (!inv)
                {
                    Console.WriteLine($"Sorry!!! But your puzzle is unsolvable");
                }
                else
                {
                    if (puzzle.Correct())
                    {
                        Console.WriteLine("Your puzzle");
                        puzzle.Print();
                        Console.WriteLine("\nPress enter to start...");
                        Console.ReadLine();

                        var search = new Search();

                        Helper.InitGoalState(puzzle.Puzzle.Length);

                        var start = DateTime.Now;
                        var result = search.ASrat(puzzle);
                        var end = DateTime.Now;

                        PrintResult(result, mode, end - start);
                    }
                    else
                    {
                        puzzle.Print();
                        Console.WriteLine("You puzzle is not a correct.");

                    }
                }
            }

            Console.WriteLine("Thank you. Bye");
            Console.ReadLine();
        }

        private static Node GetPuzzle(bool firstStep, string medthod, string useFrom, string message, out int mode)
        {
            var puzzle16_2 = new int[16]
            {
                0, 1, 3, 4,
                5, 2, 7, 8,
                9, 6, 10 , 12,
                13, 14, 11, 15
            };

            //Euclidean distance, iterations - 96731, Iterations of start to end - 59, time - 00:20:36.9224890
            var puzzle16 = new int[16]
            {
                5, 4, 2, 6,
                1, 0, 13, 7,
                12, 9, 10, 14,
                8, 3, 15, 11
            };

            var puzzle9 = new int[9]
            {
                1, 2, 4,
                3, 5, 7,
                6, 0, 8
            };

            var puzzle9_2 = new int[9]
            {
                3, 4, 1,
                0, 2, 6,
                7, 5, 8
            };

            var puzzle9_3 = new int[9]
            {
                1, 2, 3,
                5, 6, 0,
                7, 8, 4
            };

            var puzzle9_4 = new int[9]
            {
                1, 2, 3,
                4, 5, 6,
                0, 8, 7
            };

            var puzzle9_5 = new int[9]
            {
                5, 2, 8,
                4, 1, 7,
                0, 3, 6
            };

            var puzzle9_6 = new int[9]
            {
                2, 4, 3,
                1, 0, 6,
                5, 7, 8
            };

            var puzzle9_7 = new int[9]
            {
                2, 3, 0,
                1, 8, 4,
                7, 6, 5
            };

            Node puzzle = null;
            bool tryAgain = false;

            if (firstStep)
            {
                Console.WriteLine($"{message}\n" +
                                   "Available medthods is:\n" +
                                   "1 - equals current and final mode\n" +
                                   "2 - Manhattan distance\n" +
                                   "3 - Euclidean distance");
                Console.Write("Choose medthod: ");
                medthod = Console.ReadLine();
            }
            else if (Helper.GetOut)
            {
                mode = 0;

                return null;
            }
            else
            {
                Console.Write($"Oops. {message}. You want try again? [y, n]: ");
                var again = Console.ReadLine();

                if (again == "y")
                {
                    puzzle = GetPuzzle(true, medthod, useFrom, "Okay. Choose mode again", out mode);
                    tryAgain = true;
                }
                else
                {
                    Helper.GetOut = true;

                    mode = 0;

                    return null;
                }
            }

            if (int.TryParse(medthod, out mode) && mode > 0 && mode <= 3 && !tryAgain)
            {
                Helper.Mode = (Mode)mode;

                if (string.IsNullOrWhiteSpace(useFrom))
                {
                    Console.WriteLine("You want to set puzzle from file or use my puzzle?\n" +
                                      "1 - from file\n" +
                                      "2 - default puzzle\n" +
                                      "3 - input from console");
                    Console.Write("User from: ");
                    useFrom = Console.ReadLine();
                }

                if (int.TryParse(useFrom, out int use) && use > 0 && use <= 3)
                {
                    if (use == 1)
                    {
                        puzzle = GetFromFile(puzzle9_2);

                        if (puzzle == null) { useFrom = null; }
                    }
                    else if (use == 2)
                    {
                        //var root = new Node(puzzle9);
                        //var root = new Node(puzzle9_2);
                        //var root = new Node(puzzle9_3);
                        //var root = new Node(puzzle16);
                        //var root = new Node(puzzle16_2);
                        //var root = new Node(puzzle9_4);
                        //var root = new Node(puzzle9_5);
                        //var root = new Node(puzzle9_6);
                        var root = new Node(puzzle9_7);

                        puzzle = root;
                    }
                    else
                    {
                        puzzle = GetFromInput();

                        if (puzzle == null) { useFrom = null; }
                    }
                }
                else
                {
                    puzzle = GetPuzzle(false, medthod, null, "Wrong input type", out mode);
                }
            }
            else if (!tryAgain)
            {
                puzzle = GetPuzzle(false, null, useFrom, "Mode is unavailable", out mode);
            }

            if (puzzle == null)
            {
                puzzle = GetPuzzle(false, medthod, useFrom, "Puzzle is null", out mode);
            }

            return puzzle;
        }

        private static Node GetFromFile(int[] puzzle9_2)
        {
            Node puzzle;

            var directory = AppContext.BaseDirectory.Replace("\\", "/").Replace("bin/Debug/", "");

            Console.Write("Please enter file name: ");

            var fileNme = Console.ReadLine();

            var file = $"{directory}{fileNme}";

            if (File.Exists(file))
            {
                var data = File.ReadAllText(file).Split(' ').ToList();

                var initPussle = new int[data.Count];

                for (int i = 0; i < data.Count; i++)
                {
                    if (int.TryParse(data[i], out int element))
                    {
                        initPussle[i] = element;
                    }
                }

                puzzle = new Node(initPussle);
            }
            else
            {
                Console.WriteLine($"File does not exist. Path: {file}\n");

                return null;
            }

            return puzzle;
        }

        private static Node GetFromInput()
        {
            Console.WriteLine("Okay. Now write your puzzle through the space. Something like this\n" +
                              "\"1 2 3 0 5 6....\"\n" +
                              "And puzzle must have a zero number");
            Console.Write("Your puzzle: ");
            var input = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(input))
            {
                input = input.Trim();
                var data = input.Split(' ').ToList();

                var initPussle = new int[data.Count];

                Console.WriteLine("data: " + data);
                Console.WriteLine("data.Count: " + data.Count);

                for (int i = 0; i < data.Count; i++)
                {
                    if (int.TryParse(data[i], out int element))
                    {
                        initPussle[i] = element;
                    }
                }

                return new Node(initPussle);
            }

            return null;
        }

        private static void PrintResult(Info result, int mode, TimeSpan time)
        {
            //Console.WriteLine();
            //Console.Write("Do you want to print result? [y, n]: ");

            //var status = Console.ReadLine();

            //if (status.ToLower() == "y") { Console.WriteLine("===RESULT==="); }
            Console.WriteLine("===RESULT===");

            if (result.Result.Count > 0)
            {
                result.Result.Reverse();
                int iterations = 0;
                for (int i = 0; i < result.Result.Count; i++)
                {
                    //if (status.ToLower() == "y") { result.Result[i].Print(); }
                    result.Result[i].Print();
                    iterations++;
                    //Console.ReadKey();                
                }
                Console.WriteLine();
                Console.WriteLine("===Statistics===");
                Console.WriteLine($"You choose {(Mode)mode} mode");
                Console.WriteLine($"All moves: {result.AllMoves}");
                Console.WriteLine($"Iterations: {result.Iterations}");
                Console.WriteLine($"Max opened states on the same time: {result.MaxOpenCount}");
                Console.WriteLine($"Iterations of start to end: {iterations}");
                Console.WriteLine($"Time: {time.ToString()}");
            }
            else
            {
                Console.WriteLine("No result");
            }
        }
    }
}