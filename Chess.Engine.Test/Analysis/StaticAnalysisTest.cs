using Chess.Engine.Analysis;
using Chess.Engine.Board;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Chess.Engine.Test.Analysis
{
    public class StaticAnalysisTest
    {

        [Theory]
        [InlineData("bba6 wnc5", Square.a6)]
        [InlineData("bpd3 wpc2", Square.d3)]
        [InlineData("bqh8 wqa1", Square.a1)]
        [InlineData("bqh8 wqa1", Square.h8)]
        public void threat_true(string boardState, Square squareUnderThreat)
        {
            StaticAnalysis sut = new StaticAnalysis(new BoardState(boardState));

            Assert.True(sut.Analysis[(int)squareUnderThreat].PieceUnderThreat);
        }

        [Theory]
        [InlineData("wpd3 wpe4", Square.e4)]
        [InlineData("wqa1 wrg7", Square.g7)]
        [InlineData("wqa1 wbg7", Square.g7)]
        [InlineData("wqa1 wbg7", Square.a1)]
        public void defended_piece_true(string boardState, Square square)
        {
            StaticAnalysis sut = new StaticAnalysis(new BoardState(boardState));

            Assert.True(sut.Analysis[(int)square].IsDefendedPiece);

        }
        [Theory]
        [InlineData("wpd3 wpc4", Square.e4)]
        [InlineData("wkd1 wrd4 bqd8 wbg4", Square.g4)] // would place white in check
        public void defended_piece_false(string boardState, Square square)
        {
            StaticAnalysis sut = new StaticAnalysis(new BoardState(boardState));

            Assert.False(sut.Analysis[(int)square].IsDefendedPiece);

        }

        [Theory]
        [InlineData("wpd4", Square.c5)]
        [InlineData("wpd4", Square.e5)]
        [InlineData("wra1 bra5", Square.a2)]
        [InlineData("wra1 bra5", Square.a3)]
        [InlineData("wra1 bra5", Square.a4)]
        public void white_defended_square_true(string boardState, Square square)
        {
            StaticAnalysis sut = new StaticAnalysis(new BoardState(boardState));

            Assert.True(sut.Analysis[(int)square].IsWhiteDefendedSquare);
        }

        [Theory]
        [InlineData("wpd4", Square.d5)]
        [InlineData("wra1 bra5", Square.a5)]
        [InlineData("wra1 bra5", Square.a6)]
        [InlineData("wra1 bra5", Square.a7)]
        [InlineData("wra1 bra5", Square.a8)]
        public void white_defended_square_false(string boardState, Square square)
        {
            StaticAnalysis sut = new StaticAnalysis(new BoardState(boardState));

            Assert.False(sut.Analysis[(int)square].IsWhiteDefendedSquare);
        }

        [Theory]
        [InlineData("bpd4", Square.c3)]
        [InlineData("bpd4", Square.e3)]
        [InlineData("wra1 bra5", Square.a2)]
        [InlineData("wra1 bra5", Square.a3)]
        [InlineData("wra1 bra5", Square.a4)]
        [InlineData("wra1 bra5", Square.a6)]
        [InlineData("wra1 bra5", Square.a7)]
        [InlineData("wra1 bra5", Square.a8)]
        public void black_defended_square_true(string boardState, Square square)
        {
            StaticAnalysis sut = new StaticAnalysis(new BoardState(boardState));

            Assert.True(sut.Analysis[(int)square].IsBlackDefendedSquare);
        }

        [Theory]
        [InlineData("bpd4", Square.d3)]
        [InlineData("wra1 bra5", Square.a1)]
        [InlineData("wra3 bra5", Square.a1)]
        [InlineData("wra3 bra5", Square.a2)]
        public void black_defended_square_false(string boardState, Square square)
        {
            StaticAnalysis sut = new StaticAnalysis(new BoardState(boardState));

            Assert.False(sut.Analysis[(int)square].IsBlackDefendedSquare);
        }

        [Theory]
        [InlineData("wkd1 wbd3 bqd8", Square.d3)]  // bishop pinned by queen
        public void hard_pinned_true(string boardState, Square square)
        {
            StaticAnalysis sut = new StaticAnalysis(new BoardState(boardState));
            Assert.True(sut.Analysis[(int)square].IsHardPinned);
        }

        [Theory]
        [InlineData("wkd1 wrd3 bqd8", Square.d3)]  // rook not completely pinned
        public void hard_pinned_false(string boardState, Square square)
        {
            StaticAnalysis sut = new StaticAnalysis(new BoardState(boardState));
            Assert.False(sut.Analysis[(int)square].IsHardPinned);
        }

        [Theory]
        [InlineData("wba1 bqh8", Square.h7, 0)]
        [InlineData("wba1 bqh8",Square.h8,1)]
        [InlineData("wba1 wrh1 bqh8", Square.h8, 2)]
        [InlineData("wba1 wrh1 bqh8", Square.a1, 1)]
        [InlineData("wba1 wrh1 bqh8", Square.h1, 1)]
        public void attack_count(string boardState, Square target, int attackCount)
        {
            StaticAnalysis sut = new StaticAnalysis(new BoardState(boardState));
            Assert.Equal(attackCount,sut.Analysis[(int)target].AttackCount);

        }
    }
}
