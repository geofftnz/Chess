using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Engine.Board
{
    /// <summary>
    /// Determines if a given board state represents check for a given player.
    /// 
    /// This is done inside-out (ie: from the King to each square that could check it)
    /// </summary>
    public static class Check
    {
        public static bool ScanForAttack(BoardState b, Player p, Square start, Piece[] pieces, int fileofs = 0, int rankofs = 1)
        {
            int i = 0;

            Square? square = start.Offset(fileofs, rankofs);
            while (i < 8 && square.HasValue)
            {
                var current = b.PieceAt(square.Value);

                // have we hit a piece?
                if (current != Piece.None)
                {
                    // is this square occupied by the same player? If so, we can't be in check
                    if (current.GetPlayer() == p)
                    {
                        return false;
                    }

                    // if this is the first iteration, then check for a king
                    if (i == 0 && current.GetPieceType() == PieceType.King)
                    {
                        return true;
                    }

                    // is this square occupied by any of the pieces that could attack along this line?
                    if (pieces.Any(p => p == current))
                    {
                        return true;
                    }
                    return false;
                }

                square = square.Value.Offset(fileofs, rankofs);
                i++;
            }
            return false;
        }


        public static bool IsPlayerInCheck(this BoardState board, Player player, Square square)
        {
            // check for pawns
            if (player == Player.White)
            {
                if (board.IsPieceAtOffset(square, -1, 1, Piece.BlackPawn))
                {
                    return true;
                }
                if (board.IsPieceAtOffset(square, 1, 1, Piece.BlackPawn))
                {
                    return true;
                }
            }
            if (player == Player.Black)
            {
                if (board.IsPieceAtOffset(square, -1, -1, Piece.WhitePawn))
                {
                    return true;
                }
                if (board.IsPieceAtOffset(square, 1, -1, Piece.WhitePawn))
                {
                    return true;
                }
            }

            // knights
            var opponentKnight = player.GetOpponentPiece(PieceType.Knight);
            foreach (var knightOffset in PieceTypeMovement.KnightOffsets)
            {
                if (board.IsPieceAtOffset(square, knightOffset.Item1, knightOffset.Item2, opponentKnight))
                {
                    return true;
                }
            }

            // scan along paths
            var orthogonalPieces = new Piece[]
            {
                player.GetOpponentPiece(PieceType.Rook),
                player.GetOpponentPiece(PieceType.Queen)
            };

            if (ScanForAttack(board, player, square, orthogonalPieces, 0, 1))
            {
                return true;
            }
            if (ScanForAttack(board, player, square, orthogonalPieces, 0, -1))
            {
                return true;
            }
            if (ScanForAttack(board, player, square, orthogonalPieces, 1, 0))
            {
                return true;
            }
            if (ScanForAttack(board, player, square, orthogonalPieces, -1, 0))
            {
                return true;
            }

            var diagonalPieces = new Piece[]
            {
                player.GetOpponentPiece(PieceType.Bishop),
                player.GetOpponentPiece(PieceType.Queen)
            };
            if (ScanForAttack(board, player, square, diagonalPieces, -1, -1))
            {
                return true;
            }
            if (ScanForAttack(board, player, square, diagonalPieces, 1, -1))
            {
                return true;
            }
            if (ScanForAttack(board, player, square, diagonalPieces, -1, 1))
            {
                return true;
            }
            if (ScanForAttack(board, player, square, diagonalPieces, 1, 1))
            {
                return true;
            }


            return false;
        }

    }
}
