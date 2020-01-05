using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Engine.Board
{
    public enum Piece
    {
        None = 0,

        WhitePawn = 1,
        WhiteKnight = 2,
        WhiteBishop = 3,
        WhiteRook = 4,
        WhiteQueen = 5,
        WhiteKing = 6,

        BlackPawn = 9,
        BlackKnight = 10,
        BlackBishop = 11,
        BlackRook = 12,
        BlackQueen = 13,
        BlackKing = 14

    }

    public enum PieceType
    {
        None = 0,
        Pawn = 1,
        Knight = 2,
        Bishop = 3,
        Rook = 4,
        Queen = 5,
        King = 6
    }

    public enum Player
    {
        None = 0,
        White = 1,
        Black = 2
    }

    public static class PieceExtensions
    {
        public static PieceType GetPieceType(this Piece p)
        {
            return (PieceType)((int)p & 0x07);
        }

        public static Player GetPlayer(this Piece p)
        {
            if (p.GetPieceType() == PieceType.None)
                return Player.None;

            return (Player)((((int)p & 0x08) >> 3) + 1);
        }

        public static Piece GetPiece(this Player player, PieceType type)
        {
            if (player == Player.None || type == PieceType.None)
                return Piece.None;

            return (Piece)((((int)player - 1) << 3) | (int)type);
        }
        public static Piece GetOpponentPiece(this Player player, PieceType type)
        {
            if (player == Player.White)
                return Player.Black.GetPiece(type);

            if (player == Player.Black)
                return Player.White.GetPiece(type);

            return Piece.None;
        }
        public static Piece GetPiece(this PieceType type, Player player)
        {
            if (player == Player.None || type == PieceType.None)
                return Piece.None;

            return (Piece)((((int)player - 1) << 3) | (int)type);
        }

        public static Player GetOpponent(this Player p)
        {
            switch (p)
            {
                case Player.White:
                    return Player.Black;
                case Player.Black:
                    return Player.White;
                default:
                    return Player.None;
            }
        }

        public static string ToAbbr(this PieceType p)
        {
            switch (p)
            {
                case PieceType.Pawn: return "p";
                case PieceType.Knight: return "N";
                case PieceType.Bishop: return "B";
                case PieceType.Rook: return "R";
                case PieceType.Queen: return "Q";
                case PieceType.King: return "K";
                default: return string.Empty;
            }
        }
        public static string ToAbbr(this Piece p)
        {
            return p.GetPieceType().ToAbbr();
        }
    }

}
