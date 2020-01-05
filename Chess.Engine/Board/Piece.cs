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
            return p switch
            {
                Player.White => Player.Black,
                Player.Black => Player.White,
                _ => Player.None,
            };
        }

        public static string ToAbbr(this PieceType p)
        {
            return p switch
            {
                PieceType.Pawn => "p",
                PieceType.Knight => "N",
                PieceType.Bishop => "B",
                PieceType.Rook => "R",
                PieceType.Queen => "Q",
                PieceType.King => "K",
                _ => " ",
            };
        }
        public static string ToAbbr(this Piece p)
        {
            return p.GetPieceType().ToAbbr();
        }
        public static char ToAbbrUnicode(this Piece p)
        {
            return p switch
            {
                Piece.WhitePawn => (char)0x2659,
                Piece.WhiteKnight => (char)0x2658,
                Piece.WhiteBishop => (char)0x2657,
                Piece.WhiteRook => (char)0x2656,
                Piece.WhiteQueen => (char)0x2655,
                Piece.WhiteKing => (char)0x2654,
                Piece.BlackPawn => (char)0x265F,
                Piece.BlackKnight => (char)0x265E,
                Piece.BlackBishop => (char)0x265D,
                Piece.BlackRook => (char)0x265C,
                Piece.BlackQueen => (char)0x265B,
                Piece.BlackKing => (char)0x265A,
                _ => ' ',
            };
        }
    }

}
