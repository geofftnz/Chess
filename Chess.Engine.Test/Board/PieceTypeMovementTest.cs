﻿using Chess.Engine.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Chess.Engine.Test.Board
{
    public class PieceTypeMovementTest
    {

        private static List<Square> SetupPieceAndGetMoveTargets(Piece p, Square starting)
        {
            return SetupPieceAndGetMoves(p,starting).Select(m => m.To).ToList();
        }
        private static List<Move> SetupPieceAndGetMoves(Piece piece, Square square)
        {
            var b = new BoardState();
            b.SetPieceAt(square, piece);
            var moves = b.GetMoves(piece, square).ToList();
            return moves;
        }


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
            var moves = SetupPieceAndGetMoveTargets(Piece.WhiteRook, starting);
            Assert.Equal(14, moves.Count);
            Assert.Contains(endsAt, moves);
        }

        [Fact]
        public void blocked_rook_cant_move()
        {
            var b = new BoardState("wra1 wpa2 wpb1");
            var moves = b.GetMoves(Piece.WhiteRook, Square.a1).ToList();
            Assert.Empty(moves);
        }

        [Fact]
        public void rook_can_capture()
        {
            var b = new BoardState("wra1 bpa2 wpb1");
            var moves = b.GetMoves(Piece.WhiteRook, Square.a1).ToList();
            Assert.Single(moves);
            Assert.Equal(new Move(Piece.WhiteRook, Square.a1, Square.a2, Piece.BlackPawn), moves[0]);
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
            List<Square> moves = SetupPieceAndGetMoveTargets(Piece.WhiteKnight, starting);
            Assert.Equal(count, moves.Count);
            Assert.Contains(endsAt, moves);
        }


        [Theory]
        [InlineData(Square.d5, Square.a8)]
        [InlineData(Square.d5, Square.b7)]
        [InlineData(Square.d5, Square.c6)]
        [InlineData(Square.d5, Square.g8)]
        [InlineData(Square.d5, Square.f7)]
        [InlineData(Square.d5, Square.e6)]
        [InlineData(Square.d5, Square.c4)]
        [InlineData(Square.d5, Square.b3)]
        [InlineData(Square.d5, Square.a2)]
        [InlineData(Square.d5, Square.e4)]
        [InlineData(Square.d5, Square.f3)]
        [InlineData(Square.d5, Square.g2)]
        [InlineData(Square.d5, Square.h1)]
        public void produces_bishop_move(Square starting, Square endsAt)
        {
            List<Square> moves = SetupPieceAndGetMoveTargets(Piece.WhiteBishop, starting);
            Assert.Contains(endsAt, moves);
        }

        [Theory]
        [InlineData(Square.d5, Square.c4)]
        [InlineData(Square.d5, Square.c5)]
        [InlineData(Square.d5, Square.c6)]
        [InlineData(Square.d5, Square.d4)]
        [InlineData(Square.d5, Square.d6)]
        [InlineData(Square.d5, Square.e4)]
        [InlineData(Square.d5, Square.e5)]
        [InlineData(Square.d5, Square.e6)]
        [InlineData(Square.a1, Square.a2, 3)]
        [InlineData(Square.a1, Square.b2, 3)]
        [InlineData(Square.a1, Square.b1, 3)]
        public void produces_king_move(Square starting, Square endsAt, int count = 8)
        {
            List<Square> moves = SetupPieceAndGetMoveTargets(Piece.WhiteKing, starting);
            Assert.Equal(count, moves.Count);
            Assert.Contains(endsAt, moves);
        }


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
        [InlineData(Square.d5, Square.a8)]
        [InlineData(Square.d5, Square.b7)]
        [InlineData(Square.d5, Square.c6)]
        [InlineData(Square.d5, Square.g8)]
        [InlineData(Square.d5, Square.f7)]
        [InlineData(Square.d5, Square.e6)]
        [InlineData(Square.d5, Square.c4)]
        [InlineData(Square.d5, Square.b3)]
        [InlineData(Square.d5, Square.a2)]
        [InlineData(Square.d5, Square.e4)]
        [InlineData(Square.d5, Square.f3)]
        [InlineData(Square.d5, Square.g2)]
        [InlineData(Square.d5, Square.h1)]
        public void produces_queen_move(Square starting, Square endsAt, int count = 27)
        {
            List<Square> moves = SetupPieceAndGetMoveTargets(Piece.WhiteQueen, starting);
            Assert.Equal(count, moves.Count);
            Assert.Contains(endsAt, moves);
        }

        [Fact]
        public void produces_pawn_move_white_normal()
        {
            var moves = SetupPieceAndGetMoves(Piece.WhitePawn, Square.c2);
            Assert.Contains(new Move(Piece.WhitePawn, Square.c2, Square.c3), moves);
        }


        [Fact]
        public void produces_pawn_move_black_normal()
        {
            var moves = SetupPieceAndGetMoves(Piece.BlackPawn, Square.c7);
            Assert.Contains(new Move(Piece.BlackPawn, Square.c7, Square.c6), moves);
        }

        [Fact]
        public void produces_pawn_move_white_initial()
        {
            var moves = SetupPieceAndGetMoves(Piece.WhitePawn, Square.c2);
            Assert.Contains(new Move(Piece.WhitePawn, Square.c2, Square.c4), moves);
        }
        [Fact]
        public void produces_pawn_move_black_initial()
        {
            var moves = SetupPieceAndGetMoves(Piece.BlackPawn, Square.c7);
            Assert.Contains(new Move(Piece.BlackPawn, Square.c7, Square.c5), moves);
        }

        [Fact]
        public void produces_pawn_move_white_capturing()
        {
            var b = new BoardState("wpc4 bpb5 bpc5 bpd5");
            var moves = b.GetMoves(Piece.WhitePawn, Square.c4).ToList();
            Assert.Equal(2, moves.Count);
            Assert.Contains(new Move(Piece.WhitePawn, Square.c4, Square.b5, Piece.BlackPawn), moves);
            Assert.Contains(new Move(Piece.WhitePawn, Square.c4, Square.d5, Piece.BlackPawn), moves);
        }

        [Fact]
        public void produces_pawn_move_black_capturing()
        {
            var b = new BoardState("bpc4 wpb3 wpc3 wpd3");
            var moves = b.GetMoves(Piece.BlackPawn, Square.c4).ToList();
            Assert.Equal(2, moves.Count);
            Assert.Contains(new Move(Piece.BlackPawn, Square.c4, Square.b3, Piece.WhitePawn), moves);
            Assert.Contains(new Move(Piece.BlackPawn, Square.c4, Square.d3, Piece.WhitePawn), moves);
        }

        [Fact]
        public void produces_pawn_move_white_promoting()
        {
            var moves = SetupPieceAndGetMoves(Piece.WhitePawn, Square.c7);
            Assert.Contains(new Move(Piece.WhitePawn, Square.c7, Square.c8, Piece.None, false, false, PieceType.Queen), moves);
        }
        [Fact]
        public void produces_pawn_move_black_promoting()
        {
            var moves = SetupPieceAndGetMoves(Piece.BlackPawn, Square.c2);
            Assert.Contains(new Move(Piece.BlackPawn, Square.c2, Square.c1, Piece.None, false, false, PieceType.Queen), moves);
        }

        [Fact]
        public void produces_pawn_move_white_capturing_promoting()
        {
            var b = new BoardState("wpc7 bpb8 bpc8 bpd8");
            var moves = b.GetMoves(Piece.WhitePawn, Square.c7).ToList();
            Assert.Equal(2, moves.Count);
            Assert.Contains(new Move(Piece.WhitePawn, Square.c7, Square.b8, Piece.BlackPawn, false, false, PieceType.Queen), moves);
            Assert.Contains(new Move(Piece.WhitePawn, Square.c7, Square.d8, Piece.BlackPawn, false, false, PieceType.Queen), moves);
        }

        [Fact]
        public void produces_pawn_move_black_capturing_promoting()
        {
            var b = new BoardState("bpc2 wpb1 wpc1 wpd1");
            var moves = b.GetMoves(Piece.BlackPawn, Square.c2).ToList();
            Assert.Equal(2, moves.Count);
            Assert.Contains(new Move(Piece.BlackPawn, Square.c2, Square.b1, Piece.WhitePawn, false, false, PieceType.Queen), moves);
            Assert.Contains(new Move(Piece.BlackPawn, Square.c2, Square.d1, Piece.WhitePawn, false, false, PieceType.Queen), moves);
        }

        [Fact]
        public void produces_pawn_move_black_capturing_en_passant()
        {
            var b = new BoardState("bpb4 wpa4");
            b.EnPassantTargetPlayer = Player.White;
            b.EnPassantTargetSquare = Square.a3;

            var moves = b.GetMoves(Piece.BlackPawn, Square.b4).ToList();
            Assert.Contains(new Move(Piece.BlackPawn, Square.b4, Square.a3, Piece.WhitePawn, false, true), moves);
        }

        [Fact]
        public void produces_pawn_move_white_capturing_en_passant()
        {
            var b = new BoardState("bpf5 wpe5");
            b.EnPassantTargetPlayer = Player.Black;
            b.EnPassantTargetSquare = Square.f6;

            var moves = b.GetMoves(Piece.WhitePawn, Square.e5).ToList();
            Assert.Contains(new Move(Piece.WhitePawn, Square.e5, Square.f6, Piece.BlackPawn, false, true), moves);
        }

        [Theory]
        [InlineData("brf1 wkb2", Player.White, Square.b2, Square.b1)]
        [InlineData("brf1 wkb2", Player.White, Square.b2, Square.a1)]
        [InlineData("brf1 wkb2", Player.White, Square.b2, Square.c1)]
        [InlineData("brf4 wka4 wpb4", Player.White, Square.b4, Square.b5)]
        public void does_not_generate_moves_resulting_in_check(string boardState, Player p, Square from, Square to)
        {
            var b = new BoardState(boardState);
            var moves = b.GetMoves(p).ToList();
            Assert.DoesNotContain(moves, m => m.From == from && m.To == to);
        }

    }
}
