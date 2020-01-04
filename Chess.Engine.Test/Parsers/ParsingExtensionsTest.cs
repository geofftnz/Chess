using Chess.Engine.Board;
using Chess.Engine.Parsers;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Chess.Engine.Test.Parsers
{
    public class ParsingExtensionsTest
    {

        [Theory]
        [InlineData("a1",Square.a1)]
        [InlineData("b1", Square.b1)]
        [InlineData("c1", Square.c1)]
        [InlineData("d1", Square.d1)]
        [InlineData("e1", Square.e1)]
        [InlineData("f1", Square.f1)]
        [InlineData("g1", Square.g1)]
        [InlineData("h1", Square.h1)]
        [InlineData("a8", Square.a8)]
        [InlineData("b8", Square.b8)]
        [InlineData("c8", Square.c8)]
        [InlineData("d8", Square.d8)]
        [InlineData("e8", Square.e8)]
        [InlineData("f8", Square.f8)]
        [InlineData("g8", Square.g8)]
        [InlineData("h8", Square.h8)]
        public void can_parse_square_references(string s, Square expected)
        {
            Assert.Equal(expected, s.ToSquare());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("a0")]
        [InlineData("a9")]
        [InlineData("i0")]
        [InlineData("a 1")]
        [InlineData("x")]
        public void throws_on_invalid_square_references(string s)
        {
            Assert.Throws<InvalidOperationException>(() => s.ToSquare());
        }

        [Theory]
        [InlineData("w",Player.White)]
        [InlineData("W", Player.White)]
        [InlineData("b", Player.Black)]
        [InlineData("B", Player.Black)]
        public void can_parse_player(string s, Player expected)
        {
            Assert.Equal(expected, s.ToPlayer());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("a")]
        public void throws_on_invalid_player(string s)
        {
            Assert.Throws<InvalidOperationException>(() => s.ToPlayer());
        }

        [Theory]
        [InlineData("p", PieceType.Pawn)]
        [InlineData("n", PieceType.Knight)]
        [InlineData("b", PieceType.Bishop)]
        [InlineData("r", PieceType.Rook)]
        [InlineData("q", PieceType.Queen)]
        [InlineData("k", PieceType.King)]
        [InlineData("K", PieceType.King)]
        public void can_parse_piecetype(string s, PieceType expected)
        {
            Assert.Equal(expected, s.ToPieceType());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("a")]
        public void throws_on_invalid_piecetype(string s)
        {
            Assert.Throws<InvalidOperationException>(() => s.ToPieceType());
        }

        [Theory]
        [InlineData("wP",Piece.WhitePawn)]
        [InlineData("wN", Piece.WhiteKnight)]
        [InlineData("wb", Piece.WhiteBishop)]
        [InlineData("wR", Piece.WhiteRook)]
        [InlineData("bQ", Piece.BlackQueen)]
        [InlineData("wK", Piece.WhiteKing)]
        public void can_parse_piece(string s,Piece p)
        {
            Assert.Equal(p, s.ToPiece());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("a")]
        [InlineData("w")]
        [InlineData("p")]
        public void throws_on_invalid_piece(string s)
        {
            Assert.ThrowsAny<Exception>(() => s.ToPiece());
        }


        public static IEnumerable<object[]> PositionedPieceTestData =>
            new List<object[]>
            {
                new object[]{"wpa2",new PositionedPiece(Piece.WhitePawn,Square.a2) },
                new object[]{"bqh4",new PositionedPiece(Piece.BlackQueen,Square.h4) },
                new object[]{"bnh1",new PositionedPiece(Piece.BlackKnight,Square.h1) },
            };

        [Theory]
        [MemberData(nameof(PositionedPieceTestData))]
        public void can_parse_positioned_piece(string s, PositionedPiece expected)
        {
            Assert.Equal(expected, s.ToPositionedPiece());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("w")]
        [InlineData("wp")]
        [InlineData("wpa")]
        [InlineData("xxxx")]
        public void throws_on_invalid_positioned_pieces(string s)
        {
            Assert.ThrowsAny<Exception>(() => s.ToPositionedPiece());
        }

    }
}
