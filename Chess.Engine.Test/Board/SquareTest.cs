using Chess.Engine.Board;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Chess.Engine.Test.Board
{
    public class SquareTest
    {

        [Theory]
        [InlineData(Square.a1, 1)]
        [InlineData(Square.a2, 2)]
        [InlineData(Square.a3, 3)]
        [InlineData(Square.a4, 4)]
        [InlineData(Square.a5, 5)]
        [InlineData(Square.a6, 6)]
        [InlineData(Square.a7, 7)]
        [InlineData(Square.a8, 8)]
        [InlineData(Square.b1, 1)]
        [InlineData(Square.c2, 2)]
        [InlineData(Square.d3, 3)]
        [InlineData(Square.e4, 4)]
        [InlineData(Square.f5, 5)]
        [InlineData(Square.g6, 6)]
        [InlineData(Square.h7, 7)]
        [InlineData(Square.h8, 8)]
        public void can_get_rank(Square s, int rank)
        {
            Assert.Equal(rank, s.GetRank());
        }

        [Theory]
        [InlineData(Square.a1, File.a)]
        [InlineData(Square.a2, File.a)]
        [InlineData(Square.a3, File.a)]
        [InlineData(Square.a4, File.a)]
        [InlineData(Square.a5, File.a)]
        [InlineData(Square.a6, File.a)]
        [InlineData(Square.a7, File.a)]
        [InlineData(Square.a8, File.a)]
        [InlineData(Square.b1, File.b)]
        [InlineData(Square.c2, File.c)]
        [InlineData(Square.d3, File.d)]
        [InlineData(Square.e4, File.e)]
        [InlineData(Square.f5, File.f)]
        [InlineData(Square.g6, File.g)]
        [InlineData(Square.h7, File.h)]
        [InlineData(Square.h8, File.h)]
        public void can_get_file(Square s, File f)
        {
            Assert.Equal(f, s.GetFile());
        }

        [Theory]
        [InlineData(File.a, 1, Square.a1)]
        [InlineData(File.a, 2, Square.a2)]
        [InlineData(File.a, 3, Square.a3)]
        [InlineData(File.a, 4, Square.a4)]
        [InlineData(File.a, 5, Square.a5)]
        [InlineData(File.a, 6, Square.a6)]
        [InlineData(File.a, 7, Square.a7)]
        [InlineData(File.a, 8, Square.a8)]
        [InlineData(File.d, 1, Square.d1)]
        [InlineData(File.d, 2, Square.d2)]
        [InlineData(File.d, 3, Square.d3)]
        [InlineData(File.d, 4, Square.d4)]
        [InlineData(File.d, 5, Square.d5)]
        [InlineData(File.d, 6, Square.d6)]
        [InlineData(File.d, 7, Square.d7)]
        [InlineData(File.d, 8, Square.d8)]
        [InlineData(File.h, 1, Square.h1)]
        [InlineData(File.h, 2, Square.h2)]
        [InlineData(File.h, 3, Square.h3)]
        [InlineData(File.h, 4, Square.h4)]
        [InlineData(File.h, 5, Square.h5)]
        [InlineData(File.h, 6, Square.h6)]
        [InlineData(File.h, 7, Square.h7)]
        [InlineData(File.h, 8, Square.h8)]
        public void can_get_square(File f, int r, Square s)
        {
            Assert.Equal(s, f.GetSquare(r));
        }

        [Theory]
        [InlineData(Square.a1, -1, 0, null)]
        [InlineData(Square.a2, -1, 0, null)]
        [InlineData(Square.a3, -1, 0, null)]
        [InlineData(Square.a4, -1, 0, null)]
        [InlineData(Square.a5, -1, 0, null)]
        [InlineData(Square.a6, -1, 0, null)]
        [InlineData(Square.a7, -1, 0, null)]
        [InlineData(Square.a8, -1, 0, null)]

        [InlineData(Square.h1, 1, 0, null)]
        [InlineData(Square.h2, 1, 0, null)]
        [InlineData(Square.h3, 1, 0, null)]
        [InlineData(Square.h4, 1, 0, null)]
        [InlineData(Square.h5, 1, 0, null)]
        [InlineData(Square.h6, 1, 0, null)]
        [InlineData(Square.h7, 1, 0, null)]
        [InlineData(Square.h8, 1, 0, null)]

        [InlineData(Square.a1, 0, -1, null)]
        [InlineData(Square.b1, 0, -1, null)]
        [InlineData(Square.c1, 0, -1, null)]
        [InlineData(Square.d1, 0, -1, null)]
        [InlineData(Square.e1, 0, -1, null)]
        [InlineData(Square.f1, 0, -1, null)]
        [InlineData(Square.g1, 0, -1, null)]
        [InlineData(Square.h1, 0, -1, null)]

        [InlineData(Square.a8, 0, 1, null)]
        [InlineData(Square.b8, 0, 1, null)]
        [InlineData(Square.c8, 0, 1, null)]
        [InlineData(Square.d8, 0, 1, null)]
        [InlineData(Square.e8, 0, 1, null)]
        [InlineData(Square.f8, 0, 1, null)]
        [InlineData(Square.g8, 0, 1, null)]
        [InlineData(Square.h8, 0, 1, null)]

        [InlineData(Square.a1, 0, 0, Square.a1)]
        [InlineData(Square.h8, 0, 0, Square.h8)]

        [InlineData(Square.c4, 1, 0, Square.d4)]
        [InlineData(Square.c4, -1, 0, Square.b4)]
        [InlineData(Square.c4, 0, 1, Square.c5)]
        [InlineData(Square.c4, 0, -1, Square.c3)]

        public void can_get_offset(Square s, int dfile, int drank, Square? dest)
        {
            Assert.Equal(dest, s.Offset(dfile, drank));
        }
    }
}
