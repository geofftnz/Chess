﻿using Chess.Engine.Board;
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

    }
}
