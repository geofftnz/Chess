using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Engine.Board
{
    /// <summary>
    /// All this is useless now because it needs the board context
    /// </summary>
    public static class PieceTypeMovement
    {

        /// <summary>
        /// Attempt to move the piece in the given direction, stopping if blocked by the same player's piece or at a capture.
        /// </summary>
        /// <param name="b"></param>
        /// <param name="p"></param>
        /// <param name="start"></param>
        /// <param name="fileofs"></param>
        /// <param name="rankofs"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static IEnumerable<Square> Walk(BoardState b, Piece p, Square start, int fileofs = 0, int rankofs = 1, int limit = 7)
        {
            int i = 0;
            Square? square = start.Offset(fileofs, rankofs);
            while (i < limit && square.HasValue)
            {
                var current = b.PieceAt(square.Value);
                // is this square occupied by the same player?
                if (current.GetPlayer() == p.GetPlayer())
                {
                    break;
                }

                // return this square
                yield return square.Value;

                // is this square occupied by the other player?
                if (current.GetPieceType() != PieceType.None && current.GetPlayer() != p.GetPlayer())
                {
                    break;
                }

                square = square.Value.Offset(fileofs, rankofs);
                i++;
            }
        }

        public static IEnumerable<Move> RookMoves(BoardState b, Piece p, Square s)
        {
            // collect potential target squares
            foreach (var square in
                Walk(b, p, s, -1, 0)
                .Concat(Walk(b, p, s, 1, 0))
                .Concat(Walk(b, p, s, 0, -1))
                .Concat(Walk(b, p, s, 0, 1)))
            {
                yield return new Move(p, s, square, b.PieceAt(square));
            }
        }

        public static IEnumerable<Move> BishopMoves(BoardState b, Piece p, Square s)
        {
            // collect potential target squares
            foreach (var square in
                Walk(b, p, s, -1, -1)
                .Concat(Walk(b, p, s, 1, -1))
                .Concat(Walk(b, p, s, -1, 1))
                .Concat(Walk(b, p, s, 1, 1)))
            {
                yield return new Move(p, s, square, b.PieceAt(square));
            }
        }

        public static IEnumerable<Move> QueenMoves(BoardState b, Piece p, Square s)
        {
            // collect potential target squares
            foreach (var square in
                Walk(b, p, s, -1, -1)
                .Concat(Walk(b, p, s, 1, -1))
                .Concat(Walk(b, p, s, -1, 1))
                .Concat(Walk(b, p, s, 1, 1))
                .Concat(Walk(b, p, s, -1, 0))
                .Concat(Walk(b, p, s, 1, 0))
                .Concat(Walk(b, p, s, 0, -1))
                .Concat(Walk(b, p, s, 0, 1))
                )
            {
                yield return new Move(p, s, square, b.PieceAt(square));
            }
        }

        public static IEnumerable<Square> CastlingMoves(BoardState b, Piece p, Square s)
        {

            // white king
            if (p == Piece.WhiteKing && s == Square.e1 && (b.WhiteCastlingKingSideAvailable || b.WhiteCastlingQueenSideAvailable))
            {
                if (!b.WhiteInCheck)
                {
                    // king-side
                    if (b.WhiteCastlingKingSideAvailable)
                    {
                        // check for empty squares
                        if (b.IsEmpty(Square.f1) && b.IsEmpty(Square.g1))
                        {
                            // ensure no check on f1 or g1
                            if (!b.CloneAndApply(new Move(p, s, Square.f1)).IsPlayerInCheck(p.GetPlayer()) &&
                                !b.CloneAndApply(new Move(p, s, Square.g1)).IsPlayerInCheck(p.GetPlayer()))
                            {
                                yield return Square.g1;
                            }
                        }
                    }
                    // queen-side
                    if (b.WhiteCastlingQueenSideAvailable)
                    {
                        // check for empty squares
                        if (b.IsEmpty(Square.b1) && b.IsEmpty(Square.c1) && b.IsEmpty(Square.d1))
                        {
                            // ensure no check on c1 or d1
                            if (!b.CloneAndApply(new Move(p, s, Square.c1)).IsPlayerInCheck(p.GetPlayer()) &&
                                !b.CloneAndApply(new Move(p, s, Square.d1)).IsPlayerInCheck(p.GetPlayer()))
                            {
                                yield return Square.c1;
                            }
                        }
                    }
                }
            }
            // black king
            if (p == Piece.BlackKing && s == Square.e8 && (b.BlackCastlingKingSideAvailable || b.BlackCastlingQueenSideAvailable))
            {
                if (!b.BlackInCheck)
                {
                    // king-side
                    if (b.BlackCastlingKingSideAvailable)
                    {
                        // check for empty squares
                        if (b.IsEmpty(Square.f8) && b.IsEmpty(Square.g8))
                        {
                            // ensure no check on f8 or g8
                            if (!b.CloneAndApply(new Move(p, s, Square.f8)).IsPlayerInCheck(p.GetPlayer()) &&
                                !b.CloneAndApply(new Move(p, s, Square.g8)).IsPlayerInCheck(p.GetPlayer()))
                            {
                                yield return Square.g8;
                            }
                        }
                    }
                    // queen-side
                    if (b.BlackCastlingQueenSideAvailable)
                    {
                        // check for empty squares
                        if (b.IsEmpty(Square.b8) && b.IsEmpty(Square.c8) && b.IsEmpty(Square.d8))
                        {
                            // ensure no check on c8 or d8
                            if (!b.CloneAndApply(new Move(p, s, Square.c8)).IsPlayerInCheck(p.GetPlayer()) &&
                                !b.CloneAndApply(new Move(p, s, Square.d8)).IsPlayerInCheck(p.GetPlayer()))
                            {
                                yield return Square.c8;
                            }
                        }
                    }
                }
            }


            yield break;
        }

        public static IEnumerable<Move> KingMoves(BoardState b, Piece p, Square s)
        {
            // collect potential target squares
            foreach (var square in
                Walk(b, p, s, -1, -1, 1)
                .Concat(Walk(b, p, s, 1, -1, 1))
                .Concat(Walk(b, p, s, -1, 1, 1))
                .Concat(Walk(b, p, s, 1, 1, 1))
                .Concat(Walk(b, p, s, -1, 0, 1))
                .Concat(Walk(b, p, s, 1, 0, 1))
                .Concat(Walk(b, p, s, 0, -1, 1))
                .Concat(Walk(b, p, s, 0, 1, 1))
                .Concat(CastlingMoves(b, p, s))
                )
            {
                yield return new Move(p, s, square, b.PieceAt(square));
            }
        }

        private static IEnumerable<Tuple<int, int>> KnightOffsets = new List<Tuple<int, int>>
        {
            new Tuple<int, int>(-1,-2),
            new Tuple<int, int>(1,-2),
            new Tuple<int, int>(-2,-1),
            new Tuple<int, int>(2,-1),
            new Tuple<int, int>(-1,2),
            new Tuple<int, int>(1,2),
            new Tuple<int, int>(-2,1),
            new Tuple<int, int>(2,1)
        };
        public static IEnumerable<Move> KnightMoves(BoardState b, Piece p, Square s)
        {
            foreach (var square in KnightOffsets
                .Select(ofs => s.Offset(ofs.Item1, ofs.Item2))
                .Where(sq => sq.HasValue)
                .Select(sq => sq.Value))
            {
                var current = b.PieceAt(square);
                var targetplayer = current.GetPlayer();

                switch (targetplayer)
                {
                    case Player.None:  // empty square, move to
                        yield return new Move(p, s, square, current);
                        break;
                    case Player.White:
                        if (p.GetPlayer() == Player.Black)  // can capture
                        {
                            yield return new Move(p, s, square, current);
                        }
                        break;
                    case Player.Black:
                        if (p.GetPlayer() == Player.White)  // can capture
                        {
                            yield return new Move(p, s, square, current);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        public static IEnumerable<Move> PawnMoves(BoardState b, Piece p, Square s)
        {
            int forward = p.GetPlayer() == Player.White ? 1 : -1;
            int promotionRank = p.GetPlayer() == Player.White ? 8 : 1;
            int[] captureFiles = new int[] { -1, 1 };

            // can we move forward into an empty square?
            var target1 = s.Offset(0, forward) ?? throw new InvalidOperationException("Pawn should not be on the last rank without promoting");
            if (b.IsEmpty(target1))
            {
                if (target1.GetRank() == promotionRank)
                {
                    // TODO: choose promotion piece
                    yield return new Move(p, s, target1, Piece.None, false, false, PieceType.Queen);
                }
                else
                {
                    yield return new Move(p, s, target1, Piece.None);
                }
            }

            // are we on our initial move and can move 2?
            var target2 = s.Offset(0, forward * 2);
            if (((p.GetPlayer() == Player.White && s.GetRank() == 2) ||
                 (p.GetPlayer() == Player.Black && s.GetRank() == 7)) &&
                b.IsEmpty(target2.Value) && b.IsEmpty(target1))
            {
                yield return new Move(p, s, target2.Value, Piece.None);
            }

            // can we capture?
            foreach (var target3 in captureFiles
                .Select(f => s.Offset(f, forward))
                .Where(f => f.HasValue)
                .Select(f => f.Value)
                .Where(sq => !b.IsEmpty(sq) && b.PieceAt(sq).GetPlayer() != p.GetPlayer())
                )
            {
                if (target1.GetRank() == promotionRank)
                {
                    // TODO: choose promotion piece
                    yield return new Move(p, s, target3, b.PieceAt(target3), false, false, PieceType.Queen);
                }
                else
                {
                    yield return new Move(p, s, target3, b.PieceAt(target3));
                }
            }

            // can we capture en-passant?
            if (b.EnPassantTargetSquare.HasValue && b.EnPassantTargetPlayer != p.GetPlayer())
            {
                foreach (var target3 in captureFiles
                    .Select(f => s.Offset(f, forward))
                    .Where(f => f.HasValue)
                    .Select(f => f.Value)
                    .Where(sq => sq == b.EnPassantTargetSquare)
                    )
                {
                    yield return new Move(p, s, target3, p.GetPlayer().GetOpponentPiece(PieceType.Pawn), false, true);
                }
            }

            yield break;
        }

        public static IEnumerable<Move> GetMovesBeforeCheckTest(this BoardState b, Piece p, Square s)
        {
            switch (p.GetPieceType())
            {
                case PieceType.None:
                    break;
                case PieceType.Pawn:
                    foreach (var move in PawnMoves(b, p, s))
                        yield return move;
                    break;
                case PieceType.Knight:
                    foreach (var move in KnightMoves(b, p, s))
                        yield return move;
                    break;
                case PieceType.Bishop:
                    foreach (var move in BishopMoves(b, p, s))
                        yield return move;
                    break;
                case PieceType.Rook:
                    foreach (var move in RookMoves(b, p, s))
                        yield return move;
                    break;
                case PieceType.Queen:
                    foreach (var move in QueenMoves(b, p, s))
                        yield return move;
                    break;
                case PieceType.King:
                    foreach (var move in KingMoves(b, p, s))
                        yield return move;
                    break;
                default:
                    break;
            }
        }

        public static IEnumerable<Move> GetMoves(this BoardState b, Piece p, Square s, bool skipTestForCheck = false)
        {
            foreach (var move in b.GetMovesBeforeCheckTest(p, s))
            {
                if (!skipTestForCheck && b.MoveResultsInCheckForPlayer(move, move.Player.GetOpponent()))
                {
                    move.IsWithCheck = true;
                }

                if (skipTestForCheck || !b.MoveResultsInCheckForPlayer(move, move.Player))
                {
                    yield return move;
                }
            }
        }
    }
}
