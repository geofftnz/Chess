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

        [Theory]
        [MemberData(nameof(Annotations))]
        public void generates_annotation(string annotation, Move m)
        {
            Assert.Equal(annotation, m.ToAnnotation());
        }

        public static IEnumerable<object[]> Annotations = new List<object[]>
        {
            // castling
            new object[]{"O-O",new Move(Piece.WhiteKing,Square.e1, Square.g1) },
            new object[]{"O-O",new Move(Piece.BlackKing,Square.e8, Square.g8) },
            new object[]{"O-O-O",new Move(Piece.WhiteKing,Square.e1, Square.c1) },
            new object[]{"O-O-O",new Move(Piece.BlackKing,Square.e8, Square.c8) },

            // castling with check
            new object[]{"O-O+",new Move(Piece.WhiteKing,Square.e1, Square.g1,isWithCheck:true) },
            new object[]{"O-O+",new Move(Piece.BlackKing,Square.e8, Square.g8, isWithCheck: true) },
            new object[]{"O-O-O+",new Move(Piece.WhiteKing,Square.e1, Square.c1, isWithCheck: true) },
            new object[]{"O-O-O+",new Move(Piece.BlackKing,Square.e8, Square.c8, isWithCheck: true) },

            // pawn moves
            new object[]{"e3",new Move(Piece.WhitePawn,Square.e2, Square.e3) },
            new object[]{"e4",new Move(Piece.WhitePawn,Square.e2, Square.e4) },
            new object[]{"exf5",new Move(Piece.WhitePawn,Square.e4, Square.f5,Piece.BlackPawn) },
            new object[]{"e3+",new Move(Piece.WhitePawn,Square.e2, Square.e3, isWithCheck: true) },
            new object[]{"e4+",new Move(Piece.WhitePawn,Square.e2, Square.e4, isWithCheck: true) },
            new object[]{"exf5+",new Move(Piece.WhitePawn,Square.e4, Square.f5,Piece.BlackPawn, isWithCheck: true) },

            // other moves
            new object[]{"Ra4",new Move(Piece.WhiteRook,Square.a1, Square.a4) },
        };

        [Fact]
        public void cannot_capture_own_piece()
        {
            var sut = new Move(Piece.WhiteBishop, Square.a1, Square.c3, Piece.WhiteKnight);

            Assert.False(sut.IsCapturing);
        }

        [Theory]
        [InlineData(Piece.WhiteBishop, Square.a1, Square.a4, Piece.WhitePawn)]
        public void is_defendingpiece_true(Piece p, Square from, Square to, Piece target)
        {
            var sut = new Move(p, from, to, target);
            Assert.True(sut.IsDefendingPiece);
        }

        [Theory]
        [InlineData(Piece.WhiteBishop, Square.a1, Square.a4, Piece.None)]
        [InlineData(Piece.WhiteBishop, Square.a1, Square.a4, Piece.BlackKnight)]
        public void is_defendingpiece_false(Piece p, Square from, Square to, Piece target)
        {
            var sut = new Move(p, from, to, target);
            Assert.False(sut.IsDefendingPiece);
        }

        [Theory]
        [InlineData(Piece.WhiteBishop, Square.a1, Square.a4, Piece.None)]
        [InlineData(Piece.WhitePawn, Square.d4, Square.c5, Piece.None)]
        [InlineData(Piece.WhitePawn, Square.d4, Square.e5, Piece.None)]
        [InlineData(Piece.WhiteBishop, Square.c1, Square.g5, Piece.None)]
        public void is_defendingsquare_true(Piece p, Square from, Square to, Piece target)
        {
            var sut = new Move(p, from, to, target);
            Assert.True(sut.IsDefendingSquare);
        }

        [Theory]
        [InlineData(Piece.WhiteBishop, Square.a1, Square.a4, Piece.WhiteKnight)]
        [InlineData(Piece.WhiteBishop, Square.a1, Square.a4, Piece.BlackKnight)]
        [InlineData(Piece.WhitePawn, Square.d4, Square.d5, Piece.None)]
        public void is_defendingsquare_false(Piece p, Square from, Square to, Piece target)
        {
            var sut = new Move(p, from, to, target);
            Assert.False(sut.IsDefendingSquare);
        }

    }
}
