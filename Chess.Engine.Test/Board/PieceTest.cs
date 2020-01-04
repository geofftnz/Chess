using Chess.Engine.Board;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Chess.Engine.Test.Board
{
    public class PieceTest
    {
        [Theory]
        [InlineData(Piece.None, PieceType.None)]
        [InlineData(Piece.WhitePawn, PieceType.Pawn)]
        [InlineData(Piece.WhiteKnight, PieceType.Knight)]
        [InlineData(Piece.WhiteBishop, PieceType.Bishop)]
        [InlineData(Piece.WhiteRook, PieceType.Rook)]
        [InlineData(Piece.WhiteQueen, PieceType.Queen)]
        [InlineData(Piece.WhiteKing, PieceType.King)]
        [InlineData(Piece.BlackPawn, PieceType.Pawn)]
        [InlineData(Piece.BlackKnight, PieceType.Knight)]
        [InlineData(Piece.BlackBishop, PieceType.Bishop)]
        [InlineData(Piece.BlackRook, PieceType.Rook)]
        [InlineData(Piece.BlackQueen, PieceType.Queen)]
        [InlineData(Piece.BlackKing, PieceType.King)]
        public void can_get_piece_type(Piece p, PieceType t)
        {
            Assert.Equal(t, p.GetPieceType());
        }

        [Theory]
        [InlineData(Piece.None, Player.None)]
        [InlineData(Piece.WhitePawn, Player.White)]
        [InlineData(Piece.WhiteKnight, Player.White)]
        [InlineData(Piece.WhiteBishop, Player.White)]
        [InlineData(Piece.WhiteRook, Player.White)]
        [InlineData(Piece.WhiteQueen, Player.White)]
        [InlineData(Piece.WhiteKing, Player.White)]
        [InlineData(Piece.BlackPawn, Player.Black)]
        [InlineData(Piece.BlackKnight, Player.Black)]
        [InlineData(Piece.BlackBishop, Player.Black)]
        [InlineData(Piece.BlackRook, Player.Black)]
        [InlineData(Piece.BlackQueen, Player.Black)]
        [InlineData(Piece.BlackKing, Player.Black)]
        public void can_get_piece_player(Piece p, Player pl)
        {
            Assert.Equal(pl, p.GetPlayer());
        }

        [Theory]
        [InlineData(Player.None, PieceType.None, Piece.None)]
        [InlineData(Player.White, PieceType.None, Piece.None)]
        [InlineData(Player.Black,PieceType.None,Piece.None)]
        [InlineData(Player.White, PieceType.Pawn, Piece.WhitePawn)]
        [InlineData(Player.White, PieceType.Knight, Piece.WhiteKnight)]
        [InlineData(Player.White, PieceType.Bishop, Piece.WhiteBishop)]
        [InlineData(Player.White, PieceType.Rook, Piece.WhiteRook)]
        [InlineData(Player.White, PieceType.Queen, Piece.WhiteQueen)]
        [InlineData(Player.White, PieceType.King, Piece.WhiteKing)]
        [InlineData(Player.Black, PieceType.Pawn, Piece.BlackPawn)]
        [InlineData(Player.Black, PieceType.Knight, Piece.BlackKnight)]
        [InlineData(Player.Black, PieceType.Bishop, Piece.BlackBishop)]
        [InlineData(Player.Black, PieceType.Rook, Piece.BlackRook)]
        [InlineData(Player.Black, PieceType.Queen, Piece.BlackQueen)]
        [InlineData(Player.Black, PieceType.King, Piece.BlackKing)]
        public void can_get_piece_for_player(Player pl, PieceType t, Piece p)
        {
            Assert.Equal(p, pl.GetPiece(t));
        }
        [Theory]
        [InlineData(Player.None, PieceType.None, Piece.None)]
        [InlineData(Player.White, PieceType.None, Piece.None)]
        [InlineData(Player.Black, PieceType.None, Piece.None)]
        [InlineData(Player.White, PieceType.Pawn, Piece.WhitePawn)]
        [InlineData(Player.White, PieceType.Knight, Piece.WhiteKnight)]
        [InlineData(Player.White, PieceType.Bishop, Piece.WhiteBishop)]
        [InlineData(Player.White, PieceType.Rook, Piece.WhiteRook)]
        [InlineData(Player.White, PieceType.Queen, Piece.WhiteQueen)]
        [InlineData(Player.White, PieceType.King, Piece.WhiteKing)]
        [InlineData(Player.Black, PieceType.Pawn, Piece.BlackPawn)]
        [InlineData(Player.Black, PieceType.Knight, Piece.BlackKnight)]
        [InlineData(Player.Black, PieceType.Bishop, Piece.BlackBishop)]
        [InlineData(Player.Black, PieceType.Rook, Piece.BlackRook)]
        [InlineData(Player.Black, PieceType.Queen, Piece.BlackQueen)]
        [InlineData(Player.Black, PieceType.King, Piece.BlackKing)]
        public void can_get_piece_for_type(Player pl, PieceType t, Piece p)
        {
            Assert.Equal(p, t.GetPiece(pl));
        }
    }
}
