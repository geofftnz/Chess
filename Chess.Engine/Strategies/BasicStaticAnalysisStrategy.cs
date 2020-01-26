using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chess.Engine.Analysis;
using Chess.Engine.Board;

namespace Chess.Engine.Strategies
{
    public class BasicStaticAnalysisStrategy : IStrategy
    {
        private IStrategy fallbackStrategy = new RandomStrategy();
        private Random random = new Random();

        public Move SelectNextMove(BoardState board, Player player, IList<Move> moves, out string reasoning)
        {
            var currentAnalysis = new StaticAnalysis(board);


            var bestMove = RankMoves(board, player, moves, currentAnalysis).FirstOrDefault();

            if (bestMove != null)
            {
                reasoning = bestMove.Item2;
                return bestMove.Item1;
            }

            return fallbackStrategy.SelectNextMove(board, player, moves, out reasoning);
        }

        private IEnumerable<Tuple<Move, string>> RankMoves(BoardState board, Player player, IList<Move> moves, StaticAnalysis analysis)
        {

            // can we place our opponent in check via an undefended square?
            foreach (var move in moves.Where(m => m.IsWithCheck && !analysis.Analysis[(int)m.To].IsDefendedBy(player.GetOpponent())))
                yield return new Tuple<Move, string>(move, "Undefended check");

            // can we promote a pawn to an undefended square?
            foreach (var move in moves.Where(m => m.PromoteTo != PieceType.None && !analysis.Analysis[(int)m.To].IsDefendedBy(player.GetOpponent())))
                yield return new Tuple<Move, string>(move, "Undefended promotion");

            // can we capture an undefended piece?
            foreach (var move in moves.Where(m => m.IsCapturing && !analysis.Analysis[(int)m.To].IsDefendedBy(player.GetOpponent())).OrderByDescending(m=>m.CapturedPiece.GetPieceValue()))
                yield return new Tuple<Move, string>(move, $"Undefended capture {move.Piece.GetPieceType()} > {move.CapturedPiece.GetPieceType()}");

            // can we promote a pawn?
            foreach (var move in moves.Where(m => m.PromoteTo != PieceType.None))
                yield return new Tuple<Move, string>(move, "Pawn promotion");

            // can we move a piece out of danger?
            foreach (var move in moves.Where(m => analysis.Analysis[(int)m.From].IsDefendedBy(player.GetOpponent()) && !analysis.Analysis[(int)m.To].IsDefendedBy(player.GetOpponent())).OrderByDescending(m => m.Piece.GetPieceValue()))
                yield return new Tuple<Move, string>(move, "Move out of danger");

            // advance pawn to undefended
            foreach (var move in moves.Where(m => m.PieceType == PieceType.Pawn && !analysis.Analysis[(int)m.To].IsDefendedBy(player.GetOpponent())).OrderBy(m => Guid.NewGuid()))
                yield return new Tuple<Move, string>(move, "Undefended pawn advance");


            // can we capture a piece more valuable than the capturing piece?
            foreach (var move in moves.Where(m => m.IsCapturing && m.Piece.GetPieceValue() > m.CapturedPiece.GetPieceValue()).OrderByDescending(m => m.CapturedPiece.GetPieceValue()))
                yield return new Tuple<Move, string>(move, $"Capture of more valuable piece {move.Piece.GetPieceType()} > {move.CapturedPiece.GetPieceType()}");

            // can we capture a piece equal to the capturing piece?
            foreach (var move in moves.Where(m => m.IsCapturing && m.Piece.GetPieceValue() == m.CapturedPiece.GetPieceValue()).OrderByDescending(m => m.CapturedPiece.GetPieceValue()))
                yield return new Tuple<Move, string>(move, "Equal-value trade");

            // move to an undefended square
            foreach (var move in moves.Where(m => !analysis.Analysis[(int)m.To].IsDefendedBy(player.GetOpponent())).OrderBy(m => Guid.NewGuid()))
                yield return new Tuple<Move, string>(move, "Undefended move");

            // advance pawn 
            foreach (var move in moves.Where(m => m.PieceType == PieceType.Pawn).OrderBy(m => Guid.NewGuid()))
                yield return new Tuple<Move, string>(move, "Advance pawn");
        }


    }
}
