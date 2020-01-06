using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chess.Engine.Board;

namespace Chess.Engine.Strategies
{
    public class BasicNoLookAheadStrategy : IStrategy
    {
        private IStrategy fallbackStrategy = new RandomStrategy();
        private Random random = new Random();

        public Move SelectNextMove(BoardState board, Player player, IList<Move> moves, out string reasoning)
        {
            var rankedMoves = RankedMoves(moves.OrderBy(m => Guid.NewGuid()).ToList());

            if (rankedMoves.Any())
            {
                var bestMove = rankedMoves.First();
                reasoning = bestMove.IsWithCheck ? "Check" : "Capturing";
            }

            return fallbackStrategy.SelectNextMove(board, player, moves, out reasoning);
        }

        private IEnumerable<Move> RankedMoves(IList<Move> moves)
        {
            // first look for moves placing the opponent in check
            foreach (var move in moves.Where(m => m.IsWithCheck))
                yield return move;

            // then look for capturing moves
            foreach (var move in moves.Where(m => m.IsCapturing).OrderByDescending(m => m.CapturedPiece.GetPieceValue()))
                yield return move;
        }

    }
}
