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
        public bool IsCapturing { get; set; }
        public bool IsWithCheck { get; set; }
        public bool IsEnPassant { get; set; }
        public PieceType PromoteTo { get; set; }

        public Player Player => Piece.GetPlayer();
        public PieceType PieceType => Piece.GetPieceType();

        public Move(Piece piece, Square from, Square to, bool isCapturing = false, bool isWithCheck = false, bool isEnPassant = false, PieceType promoteTo = PieceType.None)
        {
            From = from;
            To = to;
            Piece = piece;
            IsCapturing = isCapturing;
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


    }
}
