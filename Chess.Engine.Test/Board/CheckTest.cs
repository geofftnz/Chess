using Chess.Engine.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Chess.Engine.Test.Board
{
    public class CheckTest
    {
        [Theory]
        [InlineData("bke1 wkc6 brc2", true)]
        [InlineData("bke1 wkc6 brc2 wpc5", false)]
        [InlineData("bke8 wke1 bpf2", true)]
        [InlineData("wkd4 bnb3", true)]
        [InlineData("wkd4 wnb3", false)]
        public void is_white_in_check(string boardState, bool isInCheck)
        {
            var b = new BoardState(boardState);
            var sq = b.FindPiece(Piece.WhiteKing).Single();

            Assert.Equal(isInCheck, b.IsPlayerInCheck(Player.White, sq));
        }
        [Theory]
        [InlineData("wke1 bkc6 wrc2", true)]
        [InlineData("bra8 bke8 brh8 wqe5 wke1", true)]
        [InlineData("bkh8 wba1 wke1", true)]
        [InlineData("bkh8 wqa1 wke1", true)]
        [InlineData("bkh8 wra1 wke1", false)]
        [InlineData("bkh8 wra8 wke1", true)]
        public void is_black_in_check(string boardState, bool isInCheck)

        {
            var b = new BoardState(boardState);
            var sq = b.FindPiece(Piece.BlackKing).Single();

            Assert.Equal(isInCheck, b.IsPlayerInCheck(Player.Black, sq));
        }

    }
}
