using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Engine.Board
{
    public class BoardState
    {
        /// <summary>
        /// board array
        /// </summary>
        public Piece[] Board { get; } = new Piece[64];

        public BoardState()
        {
        }

        public Piece PieceAt(Square s)
        {
            return Board[(int)s];
        }

        public void SetupBoard()
        {
            for(int i = 0; i < 64; i++)
            {
                Board[i] = Piece.None;
            }

            for(int i = 0; i < 8; i++)
            {
                Board[(int)((File)i).GetSquare(2)] = Piece.WhitePawn;
                Board[(int)((File)i).GetSquare(7)] = Piece.BlackPawn;
            }

            Board[(int)Square.a1] = Piece.WhiteRook;
            Board[(int)Square.b1] = Piece.WhiteKnight;
            Board[(int)Square.c1] = Piece.WhiteBishop;
            Board[(int)Square.d1] = Piece.WhiteQueen;
            Board[(int)Square.e1] = Piece.WhiteKing;
            Board[(int)Square.f1] = Piece.WhiteBishop;
            Board[(int)Square.g1] = Piece.WhiteKnight;
            Board[(int)Square.h1] = Piece.WhiteRook;

            Board[(int)Square.a8] = Piece.BlackRook;
            Board[(int)Square.b8] = Piece.BlackKnight;
            Board[(int)Square.c8] = Piece.BlackBishop;
            Board[(int)Square.d8] = Piece.BlackQueen;
            Board[(int)Square.e8] = Piece.BlackKing;
            Board[(int)Square.f8] = Piece.BlackBishop;
            Board[(int)Square.g8] = Piece.BlackKnight;
            Board[(int)Square.h8] = Piece.BlackRook;
        }

    }
}
