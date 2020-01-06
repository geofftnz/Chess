using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chess.Engine.Board;

namespace Chess.Engine.Strategies
{
    public class BasicLookAheadStrategy : IStrategy
    {
        private IStrategy fallbackStrategy = new BasicNoLookAheadStrategy();

        public Move SelectNextMove(BoardState board, Player player, IList<Move> moves)
        {
            return fallbackStrategy.SelectNextMove(board, player, moves);
        }


    }
}
