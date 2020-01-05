using Chess.Engine.Board;
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

            while (true)
            {
                var moves = board.GetMovesForNextPlayer().ToList();

                if (moves.Count == 0)
                {
                    Console.WriteLine("No available moves");
                    break;
                }

                var move = moves[random.Next(0, moves.Count)];

                Console.Clear();
                board.Apply(move);
                board.RenderToConsole();
                Console.WriteLine();
                Console.WriteLine(move.ToString());

                Console.ReadKey();
            }
        }
    }
}
