using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Engine.Board
{
    public static class PieceTypeMovement
    {
        public static IEnumerable<Square> RookMoves(Square s)
        {
            Square? newSquare;
            for(int i = 1; i < 8; i++)
            {
                if ((newSquare = s.Offset(-i, 0)).HasValue)
                {
                    yield return (newSquare.Value);
                }
                if ((newSquare = s.Offset(i, 0)).HasValue)
                {
                    yield return (newSquare.Value);
                }
                if ((newSquare = s.Offset(0, -i)).HasValue)
                {
                    yield return (newSquare.Value);
                }
                if ((newSquare = s.Offset(0, i)).HasValue)
                {
                    yield return (newSquare.Value);
                }
            }
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
