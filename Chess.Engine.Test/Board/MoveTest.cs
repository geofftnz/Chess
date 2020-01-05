using Chess.Engine.Board;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Chess.Engine.Test.Board
{
    public class MoveTest
    {
        [Fact]
        public void generates_en_passant_white()
        {
            var m = new Move(Piece.WhitePawn, Square.b2, Square.b4);
            Assert.Equal(Square.b3, m.GeneratedEnPassantOpportunity);
        }
        [Fact]
        public void does_not_generates_en_passant_white()
        {
            var m = new Move(Piece.WhitePawn, Square.b3, Square.b4);
            Assert.Null(m.GeneratedEnPassantOpportunity);            
        }
        [Fact]
        public void generates_en_passant_black()
        {
            var m = new Move(Piece.BlackPawn, Square.b7, Square.b5);
            Assert.Equal(Square.b6, m.GeneratedEnPassantOpportunity);
        }
        [Fact]
        public void does_not_generates_en_passant_black()
        {
            var m = new Move(Piece.BlackPawn, Square.b6, Square.b5);
            Assert.Null(m.GeneratedEnPassantOpportunity);
        }
    }
}
