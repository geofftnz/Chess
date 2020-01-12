using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chess.Engine.Analysis;
using Chess.Engine.Board;

namespace Chess.Engine.Strategies
{
    public class StaticAnalysisStrategy : IStrategy
    {
        private IStrategy fallbackStrategy = new RandomStrategy();
        private Random random = new Random();

        public Move SelectNextMove(BoardState board, Player player, IList<Move> moves, out string reasoning)
        {
            var currentAnalysis = new StaticAnalysis(board);
            var currentValue = currentAnalysis.GetBoardValue(player);
            var currentOpponentValue = currentAnalysis.GetBoardValue(player.GetOpponent());
            var currentDiffValue = currentValue - currentOpponentValue;

            var valuedMoves = moves
                .Select(m => new Tuple<Move, BoardState>(m, board.CloneAndApply(m)))
                .Select(m =>
                {
                    var a = new StaticAnalysis(m.Item2);
                    return new Tuple<Move, float>(m.Item1, a.GetBoardValue(player) - a.GetBoardValue(player.GetOpponent()));
                })
                .OrderByDescending(m => m.Item2)
                .ThenByDescending(m => Guid.NewGuid())
                .ToList();

            if (valuedMoves.Any())
            {
                var bestMove = valuedMoves.First();
                reasoning = $"Value diff {bestMove.Item2}";
                return bestMove.Item1;
            }

            return fallbackStrategy.SelectNextMove(board, player, moves, out reasoning);
        }

    }
}
