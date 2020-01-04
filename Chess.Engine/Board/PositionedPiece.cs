using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Chess.Engine.Board
{
    public struct PositionedPiece : IEquatable<PositionedPiece>
    {
        public Piece piece;
        public Square square;
        public PositionedPiece(Piece piece, Square square)
        {
            this.piece = piece;
            this.square = square;
        }

        public bool Equals([AllowNull] PositionedPiece other)
        {
            return piece == other.piece && square == other.square;                
        }
    }
}
