using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Engine.Board
{
    public static class PieceTypeMovement
    {
        public static IEnumerable<Square> RookMoves(Square s)
        {
            for (int i = 1; i < 8; i++)
            {
                { if (s.TryOffset(-i, 0, out var newSquare)) yield return newSquare; }
                { if (s.TryOffset(i, 0, out var newSquare)) yield return newSquare; }
                { if (s.TryOffset(0, -i, out var newSquare)) yield return newSquare; }
                { if (s.TryOffset(0, i, out var newSquare)) yield return newSquare; }
            }
        }

        public static IEnumerable<Square> KnightMoves(Square s)
        {
            { if (s.TryOffset(-1, -2, out var newSquare)) yield return newSquare; }
            { if (s.TryOffset(1, -2, out var newSquare)) yield return newSquare; }
            { if (s.TryOffset(-2, -1, out var newSquare)) yield return newSquare; }
            { if (s.TryOffset(2, -1, out var newSquare)) yield return newSquare; }

            { if (s.TryOffset(-1, 2, out var newSquare)) yield return newSquare; }
            { if (s.TryOffset(1, 2, out var newSquare)) yield return newSquare; }
            { if (s.TryOffset(-2, 1, out var newSquare)) yield return newSquare; }
            { if (s.TryOffset(2, 1, out var newSquare)) yield return newSquare; }
        }

        public static IEnumerable<Square> GetMoves(this PieceType p, Square s)
        {
            switch (p)
            {
                case PieceType.None:
                    break;
                case PieceType.Pawn:
                    break;
                case PieceType.Knight:
                    foreach (var move in KnightMoves(s))
                        yield return move;
                    break;
                case PieceType.Bishop:
                    break;
                case PieceType.Rook:
                    foreach (var move in RookMoves(s))
                        yield return move;
                    break;
                case PieceType.Queen:
                    break;
                case PieceType.King:
                    break;
                default:
                    break;
            }
        }


    }
}
