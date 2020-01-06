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
            IStrategy strategy = new BasicLookAheadStrategy();

            while (true)
            {
                var moves = board.GetMovesForNextPlayer().ToList();

                if (moves.Count == 0)
                {
                    Console.WriteLine("No available moves");

                    while (Console.ReadKey().Key != ConsoleKey.Escape) ;

                    break;
                }

                var move = strategy.SelectNextMove(board, board.NextPlayerToMove, moves);

                Console.Clear();
                board.Apply(move);
                board.RenderToConsole();
                Console.WriteLine();
                Console.WriteLine(move.ToString());
                Console.WriteLine($"{moves.Count} moves");
                Console.WriteLine(string.Join(" ",moves.Select(m=>m.ToAnnotation())));

                //if (board.WhiteInCheck || board.BlackInCheck)
                Console.ReadKey();
            }
        }
    }
}
