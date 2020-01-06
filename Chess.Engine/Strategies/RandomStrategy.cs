using System;
using System.Collections.Generic;
using System.Text;
using Chess.Engine.Board;

namespace Chess.Engine.Strategies
{
    public class RandomStrategy : IStrategy
    {
        private Random random = new Random();

        public Move SelectNextMove(BoardState board, Player player, IList<Move> moves)
        {
            return moves[random.Next(0, moves.Count)];
        }
    }
}
