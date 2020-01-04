using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Engine.Board
{
    public enum Square
    {
        a1 = 0,
        a2,
        a3,
        a4,
        a5,
        a6,
        a7,
        a8,
        b1,
        b2,
        b3,
        b4,
        b5,
        b6,
        b7,
        b8,
        c1,
        c2,
        c3,
        c4,
        c5,
        c6,
        c7,
        c8,
        d1,
        d2,
        d3,
        d4,
        d5,
        d6,
        d7,
        d8,
        e1,
        e2,
        e3,
        e4,
        e5,
        e6,
        e7,
        e8,
        f1,
        f2,
        f3,
        f4,
        f5,
        f6,
        f7,
        f8,
        g1,
        g2,
        g3,
        g4,
        g5,
        g6,
        g7,
        g8,
        h1,
        h2,
        h3,
        h4,
        h5,
        h6,
        h7,
        h8
    }

    public enum File
    {
        a = 0,
        b,
        c,
        d,
        e,
        f,
        g,
        h
    }

    public enum Colour
    {
        Dark = 0,
        Light = 1
    }


    public static class SquareExtensions
    {
        public static int GetRank(this Square s)
        {
            return ((int)s & 0x07) + 1;
        }

        public static File GetFile(this Square s)
        {
            return (File)((int)s >> 3);
        }

        public static Square GetSquare(this File f, int rank)
        {
            if (rank < 1 || rank > 8)
                throw new ArgumentOutOfRangeException("rank");

            return (Square)((int)f << 3 | (rank - 1));
        }

        public static Colour GetColour(this Square s)
        {
            return (Colour)(1 - ((int)s.GetFile() & 0x01) ^ (s.GetRank() & 0x01));
        }

        public static Square? Offset(this Square s, int dfile = 0, int drank = 0)
        {
            var currentRank = s.GetRank();
            var currentFile = s.GetFile();

            int newRank = currentRank + drank;
            int newFile = (int)currentFile + dfile;

            if (newRank >= 1 && newRank <= 8 && newFile >= 0 && newFile <= 7)
            {
                return ((File)newFile).GetSquare(newRank);
            }
            return null;
        }
    }
}
