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
    }
}
