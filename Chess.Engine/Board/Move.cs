using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Engine.Board
{
    public class Move
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
    }
}
