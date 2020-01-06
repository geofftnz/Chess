using Chess.Engine.Board;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Engine.Strategies
{
    public interface IStrategy
    {
        Move SelectNextMove(BoardState board, Player player, IList<Move> moves, out string reasoning);
    }
}
