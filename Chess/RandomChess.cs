using Chess.Engine.Board;
using Chess.Engine.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess
{
    public class RandomChess
    {

        public void Run()
        {
            var board = BoardState.InitialBoard;
            var random = new Random();
            IStrategy strategy = new BasicNoLookAheadStrategy();
            string reasoning;

            while (true)
            {
                var moves = board.GetMovesForNextPlayer().ToList();

                if (moves.Count == 0)
                {
                    Console.Clear();
                    board.RenderToConsole();
                    Console.WriteLine($"Checkmate by {board.NextPlayerToMove.GetOpponent()}");

                    while (Console.ReadKey().Key != ConsoleKey.Escape) ;

                    board = BoardState.InitialBoard;
                    continue;
                }

                var move = strategy.SelectNextMove(board, board.NextPlayerToMove, moves, out reasoning);

                Console.Clear();
                board.Apply(move);
                board.RenderToConsole();
                Console.WriteLine();
                Console.WriteLine($"{move.ToString()} because {reasoning}");
                Console.WriteLine($"{moves.Count} moves");
                Console.WriteLine(string.Join(" ",moves.Select(m=>m.ToAnnotation())));

                //if (board.WhiteInCheck || board.BlackInCheck)
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                {
                    break;
                }
                if (key.Key == ConsoleKey.R)
                {
                    board = BoardState.InitialBoard;
                }
            }
        }
    }
}
