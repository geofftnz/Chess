using Chess.Engine.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Chess.Engine.Test.Board
{
    public class PieceTypeMovementTest
    {

        [Theory]
        [InlineData(Square.d5, Square.d1)]
        [InlineData(Square.d5, Square.d2)]
        [InlineData(Square.d5, Square.d3)]
        [InlineData(Square.d5, Square.d4)]
        [InlineData(Square.d5, Square.d6)]
        [InlineData(Square.d5, Square.d7)]
        [InlineData(Square.d5, Square.d8)]
        [InlineData(Square.d5, Square.a5)]
        [InlineData(Square.d5, Square.b5)]
        [InlineData(Square.d5, Square.c5)]
        [InlineData(Square.d5, Square.e5)]
        [InlineData(Square.d5, Square.f5)]
        [InlineData(Square.d5, Square.g5)]
        [InlineData(Square.d5, Square.h5)]
        public void produces_rook_move(Square starting, Square endsAt)
        {
            var moves = PieceType.Rook.GetMoves(starting).ToList();
            Assert.Equal(14, moves.Count);
            Assert.Contains(endsAt, moves);
        }

        [Theory]
        [InlineData(Square.d4, Square.c6)]
        [InlineData(Square.d4, Square.e6)]
        [InlineData(Square.d4, Square.b5)]
        [InlineData(Square.d4, Square.f5)]
        [InlineData(Square.d4, Square.b3)]
        [InlineData(Square.d4, Square.f3)]
        [InlineData(Square.d4, Square.c2)]
        [InlineData(Square.d4, Square.e2)]
        [InlineData(Square.a1, Square.b3, 2)]
        [InlineData(Square.a1, Square.c2, 2)]
        public void produces_knight_move(Square starting, Square endsAt, int count = 8)
        {
            var moves = PieceType.Knight.GetMoves(starting).ToList();
            Assert.Equal(count, moves.Count);
            Assert.Contains(endsAt, moves);
        }

    }
}
