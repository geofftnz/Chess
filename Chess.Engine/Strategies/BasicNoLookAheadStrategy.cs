using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chess.Engine.Board;

namespace Chess.Engine.Strategies
{
    public class BasicNoLookAheadStrategy : IStrategy
    {
        private Random random = new Random();

        public Move SelectNextMove(BoardState board, Player player, IList<Move> moves)
        {
            return RankedMoves(moves.OrderBy(m => Guid.NewGuid()).ToList()).FirstOrDefault();
        }

        private IEnumerable<Move> RankedMoves(IList<Move> moves)
        {
            // first look for moves placing the opponent in check
            foreach (var move in moves.Where(m => m.IsWithCheck))
                yield return move;

            // then look for capturing moves
            foreach (var move in moves.Where(m => m.IsCapturing).OrderByDescending(m => m.CapturedPiece.GetPieceValue()))
                yield return move;

            // then pick a random move
            foreach (var move in moves)
                yield return move;
        }

    }
}
