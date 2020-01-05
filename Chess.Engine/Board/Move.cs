using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Chess.Engine.Board
{
    public class Move : IEquatable<Move>
    {
        public Square From { get; set; }
        public Square To { get; set; }
        public Piece Piece { get; set; }
        public Piece CapturedPiece { get; set; }
        public bool IsCapturing => CapturedPiece != Piece.None;
        public bool IsWithCheck { get; set; }
        public bool IsEnPassant { get; set; }
        public PieceType PromoteTo { get; set; }

        public Player Player => Piece.GetPlayer();
        public PieceType PieceType => Piece.GetPieceType();

        public bool IsWhiteKingSideCastle => From == Square.e1 && To == Square.g1 && Piece == Piece.WhiteKing;
        public bool IsWhiteQueenSideCastle => From == Square.e1 && To == Square.c1 && Piece == Piece.WhiteKing;
        public bool IsBlackKingSideCastle => From == Square.e8 && To == Square.g8 && Piece == Piece.BlackKing;
        public bool IsBlackQueenSideCastle => From == Square.e8 && To == Square.c8 && Piece == Piece.BlackKing;

        public bool IsCastle => IsWhiteKingSideCastle || IsWhiteQueenSideCastle || IsBlackKingSideCastle || IsBlackQueenSideCastle;
        public bool IsKingSideCastle => IsWhiteKingSideCastle || IsBlackKingSideCastle;
        public bool IsQueenSideCastle => IsWhiteQueenSideCastle || IsBlackQueenSideCastle;

        public Move(Piece piece, Square from, Square to, Piece capturedPiece = Piece.None, bool isWithCheck = false, bool isEnPassant = false, PieceType promoteTo = PieceType.None)
        {
            From = from;
            To = to;
            Piece = piece;
            CapturedPiece = capturedPiece;
            IsWithCheck = isWithCheck;
            IsEnPassant = isEnPassant;
            PromoteTo = promoteTo;
        }

        public bool Equals([AllowNull] Move other)
        {
            if (other == null)
                return false;

            return
                From == other.From &&
                To == other.To &&
                Piece == other.Piece &&
                IsCapturing == other.IsCapturing &&
                IsWithCheck == other.IsWithCheck &&
                IsEnPassant == other.IsEnPassant &&
                PromoteTo == other.PromoteTo;
        }

        public Square? GeneratedEnPassantOpportunity
        {
            get
            {
                if (Piece.GetPieceType() == PieceType.Pawn)
                {
                    switch (Player)
                    {
                        case Player.White:
                            if (From.GetRank() == 2 && To.GetRank() == 4)
                                return From.Offset(0, 1).Value;
                            break;
                        case Player.Black:
                            if (From.GetRank() == 7 && To.GetRank() == 5)
                                return From.Offset(0, -1).Value;
                            break;
                        default:
                            break;
                    }
                }
                return null;
            }
        }

        public override string ToString()
        {
            return $"{Player} {PieceType} {From}->{To}";
        }

        private string CheckAnnotation => (IsWithCheck ? "+" : "");
        public string ToAnnotation()
        {
            if (IsKingSideCastle)
                return "O-O" + CheckAnnotation;

            if (IsQueenSideCastle)
                return "O-O-O" + CheckAnnotation;

            if (PieceType == PieceType.Pawn)
            {
                if (IsCapturing)
                {
                    return $"{From.GetFile()}x{To}" + CheckAnnotation;
                }
                else
                {
                    return To.ToString() + CheckAnnotation;
                }
            }
            else
            {
                if (IsCapturing)
                {
                    // todo: ambiguity
                    return $"{Piece.ToAbbr()}x{To}" + CheckAnnotation;
                }
                else
                {
                    return $"{Piece.ToAbbr()}{To}" + CheckAnnotation;
                }
            }
        }
    }
}
