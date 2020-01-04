using Chess.Engine.Board;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Engine.Parsers
{
    public static class ParsingExtensions
    {

        public static PositionedPiece ToPositionedPiece(this string s)
        {
            if (string.IsNullOrEmpty(s))
                throw new InvalidOperationException("Empty string");

            if (s.Length < 4)
                throw new InvalidOperationException($"Could not parse <{s}> to positioned piece.");

            return new PositionedPiece(
                s.Substring(0, 2).ToPiece(),
                s.Substring(2, 2).ToSquare());
        }

        public static Piece ToPiece(this string s)
        {
            if (string.IsNullOrEmpty(s))
                throw new InvalidOperationException("Empty string");

            if (s.Length < 2)
                throw new InvalidOperationException($"Could not parse <{s}> to positioned piece.");

            return s.Substring(0, 1).ToPlayer().GetPiece(s.Substring(1, 1).ToPieceType());
        }

        public static PieceType ToPieceType(this string s)
        {
            if (string.IsNullOrEmpty(s))
                throw new InvalidOperationException("Empty string");

            switch (s.ToLowerInvariant())
            {
                case "p": return PieceType.Pawn;
                case "n": return PieceType.Knight;
                case "b": return PieceType.Bishop;
                case "r": return PieceType.Rook;
                case "q": return PieceType.Queen;
                case "k": return PieceType.King;
                default:
                    throw new InvalidOperationException($"Could not parse <{s}> to piece type.");
            }
        }

        public static Player ToPlayer(this string s)
        {
            if (string.IsNullOrEmpty(s))
                throw new InvalidOperationException("Empty string");

            switch (s.ToLowerInvariant())
            {
                case "w":
                    return Player.White;
                case "b":
                    return Player.Black;
                default:
                    throw new InvalidOperationException($"Could not parse <{s}> to player type.");
            }
        }

        public static Square ToSquare(this string s)
        {
            if (string.IsNullOrEmpty(s))
                throw new InvalidOperationException("Empty string");

            if (Enum.TryParse(typeof(Square), s, out var sq))
            {
                return (Square)sq;
            }
            throw new InvalidOperationException($"Could not parse <{s}> to square reference.");
        }

    }
}
