﻿using Chess.Engine.Board;
using Chess.Engine.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Chess.Engine.Test.Board
{
    public class BoardStateTest
    {
        private BoardState initialBoard;
        public BoardStateTest()
        {
            initialBoard = new BoardState();
            initialBoard.SetupBoard();
        }


        [Theory]
        [InlineData(Square.a1, Piece.WhiteRook)]
        [InlineData(Square.b1, Piece.WhiteKnight)]
        [InlineData(Square.c1, Piece.WhiteBishop)]
        [InlineData(Square.d1, Piece.WhiteQueen)]
        [InlineData(Square.e1, Piece.WhiteKing)]
        [InlineData(Square.f1, Piece.WhiteBishop)]
        [InlineData(Square.g1, Piece.WhiteKnight)]
        [InlineData(Square.h1, Piece.WhiteRook)]

        [InlineData(Square.a2, Piece.WhitePawn)]
        [InlineData(Square.b2, Piece.WhitePawn)]
        [InlineData(Square.c2, Piece.WhitePawn)]
        [InlineData(Square.d2, Piece.WhitePawn)]
        [InlineData(Square.e2, Piece.WhitePawn)]
        [InlineData(Square.f2, Piece.WhitePawn)]
        [InlineData(Square.g2, Piece.WhitePawn)]
        [InlineData(Square.h2, Piece.WhitePawn)]

        [InlineData(Square.a8, Piece.BlackRook)]
        [InlineData(Square.b8, Piece.BlackKnight)]
        [InlineData(Square.c8, Piece.BlackBishop)]
        [InlineData(Square.d8, Piece.BlackQueen)]
        [InlineData(Square.e8, Piece.BlackKing)]
        [InlineData(Square.f8, Piece.BlackBishop)]
        [InlineData(Square.g8, Piece.BlackKnight)]
        [InlineData(Square.h8, Piece.BlackRook)]

        [InlineData(Square.a7, Piece.BlackPawn)]
        [InlineData(Square.b7, Piece.BlackPawn)]
        [InlineData(Square.c7, Piece.BlackPawn)]
        [InlineData(Square.d7, Piece.BlackPawn)]
        [InlineData(Square.e7, Piece.BlackPawn)]
        [InlineData(Square.f7, Piece.BlackPawn)]
        [InlineData(Square.g7, Piece.BlackPawn)]
        [InlineData(Square.h7, Piece.BlackPawn)]
        public void board_is_set_up_correctly(Square s, Piece p)
        {
            Assert.Equal(p, initialBoard.PieceAt(s));
        }

        [Fact]
        public void can_parse_initial_board_state()
        {
            var b = new BoardState("wpa1 bNc5 wqc6");
            Assert.Equal(Piece.WhitePawn, b.PieceAt(Square.a1));
            Assert.Equal(Piece.BlackKnight, b.PieceAt(Square.c5));
            Assert.Equal(Piece.WhiteQueen, b.PieceAt(Square.c6));
        }

        public static IEnumerable<object[]> InvalidMoves =>
            new List<object[]>
            {
                new object[] { BoardState.InitialBoard, new Move(Piece.WhitePawn, Square.a3, Square.a4), InvalidMoveReason.PieceNotAtSpecifiedSquare },
                new object[] { new BoardState("wpa3 bpa4"), new Move(Piece.WhitePawn, Square.a3, Square.a4), InvalidMoveReason.CapturingButNotMarkedAsCapture },
                new object[] { new BoardState("wpa3 wpa4"), new Move(Piece.WhitePawn, Square.a3, Square.a4), InvalidMoveReason.TargetSquareOccupiedByPlayer },
                new object[] { new BoardState("wpa5 bpb5"), new Move(Piece.BlackPawn, Square.b5, Square.a4,Piece.WhitePawn,false,true), InvalidMoveReason.EnPassantNotValid },
            };

        [Theory]
        [MemberData(nameof(InvalidMoves))]
        public void prevents_invalid_moves(BoardState b, Move m, InvalidMoveReason reason)
        {
            try
            {
                b.CloneAndApply(m);
                Assert.True(false);
            }
            catch (InvalidMoveException ex)
            {
                Assert.Equal(reason, ex.Reason);
            }
        }

        [Fact]
        public void sets_en_passant_white()
        {
            var b = new BoardState("wpa2");
            Assert.Null(b.EnPassantTargetSquare);
            Assert.Equal(Player.None, b.EnPassantTargetPlayer);

            b.Apply(new Move(Piece.WhitePawn, Square.a2, Square.a4));

            Assert.Equal(Square.a3, b.EnPassantTargetSquare);
            Assert.Equal(Player.White, b.EnPassantTargetPlayer);

        }
        [Fact]
        public void sets_en_passant_black()
        {
            var b = new BoardState("bpa7");
            Assert.Null(b.EnPassantTargetSquare);
            Assert.Equal(Player.None, b.EnPassantTargetPlayer);

            b.Apply(new Move(Piece.BlackPawn, Square.a7, Square.a5));

            Assert.Equal(Square.a6, b.EnPassantTargetSquare);
            Assert.Equal(Player.Black, b.EnPassantTargetPlayer);
        }

        [Fact]
        public void generates_moves_for_player()
        {
            var b = new BoardState("wpb2 wpc2 bpf7");
            var moves = b.GetMoves(Player.White).ToList();
            Assert.Equal(4, moves.Count);
            Assert.Contains(new Move(Piece.WhitePawn, Square.b2, Square.b3), moves);
            Assert.Contains(new Move(Piece.WhitePawn, Square.b2, Square.b4), moves);
            Assert.Contains(new Move(Piece.WhitePawn, Square.c2, Square.c3), moves);
            Assert.Contains(new Move(Piece.WhitePawn, Square.c2, Square.c4), moves);
        }

        [Theory]
        [InlineData("bkc6 wrc2 wke1", Player.Black)]
        [InlineData("bkc6 wrc2 wke1", Player.White, false)]
        [InlineData("bkh8 brf1 bba2 wpd7 wrc6 wnb4 wke6", Player.White)]
        public void detects_check(string boardState, Player p, bool result = true)
        {
            var b = new BoardState(boardState);
            Assert.Equal(result, b.IsPlayerInCheck(p));
        }


        [Fact]
        public void castling_flags_clear_for_white_on_king_move()
        {
            var b = BoardState.InitialBoard;
            Assert.True(b.WhiteCastlingKingSideAvailable);
            Assert.True(b.WhiteCastlingQueenSideAvailable);

            b.Apply(b.GenerateMove(Square.e2, Square.e3));
            Assert.True(b.WhiteCastlingKingSideAvailable);
            Assert.True(b.WhiteCastlingQueenSideAvailable);

            b.Apply(b.GenerateMove(Square.e1, Square.e2));
            Assert.False(b.WhiteCastlingKingSideAvailable);
            Assert.False(b.WhiteCastlingQueenSideAvailable);
        }

        [Fact]
        public void castling_flags_clear_for_white_on_king_rook_move()
        {
            var b = BoardState.InitialBoard;
            Assert.True(b.WhiteCastlingKingSideAvailable);
            Assert.True(b.WhiteCastlingQueenSideAvailable);

            b.Apply(b.GenerateMove(Square.h2, Square.h3));
            Assert.True(b.WhiteCastlingKingSideAvailable);
            Assert.True(b.WhiteCastlingQueenSideAvailable);

            b.Apply(b.GenerateMove(Square.h1, Square.h2));
            Assert.False(b.WhiteCastlingKingSideAvailable);
            Assert.True(b.WhiteCastlingQueenSideAvailable);
        }
        [Fact]
        public void castling_flags_clear_for_white_on_queen_rook_move()
        {
            var b = BoardState.InitialBoard;
            Assert.True(b.WhiteCastlingKingSideAvailable);
            Assert.True(b.WhiteCastlingQueenSideAvailable);

            b.Apply(b.GenerateMove(Square.a2, Square.a3));
            Assert.True(b.WhiteCastlingKingSideAvailable);
            Assert.True(b.WhiteCastlingQueenSideAvailable);

            b.Apply(b.GenerateMove(Square.a1, Square.a2));
            Assert.True(b.WhiteCastlingKingSideAvailable);
            Assert.False(b.WhiteCastlingQueenSideAvailable);
        }

        [Fact]
        public void castling_flags_clear_for_black_on_king_move()
        {
            var b = BoardState.InitialBoard;
            Assert.True(b.BlackCastlingKingSideAvailable);
            Assert.True(b.BlackCastlingQueenSideAvailable);

            b.Apply(b.GenerateMove(Square.e7, Square.e6));
            Assert.True(b.BlackCastlingKingSideAvailable);
            Assert.True(b.BlackCastlingQueenSideAvailable);

            b.Apply(b.GenerateMove(Square.e8, Square.e7));
            Assert.False(b.BlackCastlingKingSideAvailable);
            Assert.False(b.BlackCastlingQueenSideAvailable);
        }

        [Fact]
        public void castling_flags_clear_for_black_on_king_rook_move()
        {
            var b = BoardState.InitialBoard;
            Assert.True(b.BlackCastlingKingSideAvailable);
            Assert.True(b.BlackCastlingQueenSideAvailable);

            b.Apply(b.GenerateMove(Square.h7, Square.h6));
            Assert.True(b.BlackCastlingKingSideAvailable);
            Assert.True(b.BlackCastlingQueenSideAvailable);

            b.Apply(b.GenerateMove(Square.h8, Square.h7));
            Assert.False(b.BlackCastlingKingSideAvailable);
            Assert.True(b.BlackCastlingQueenSideAvailable);
        }
        [Fact]
        public void castling_flags_clear_for_black_on_queen_rook_move()
        {
            var b = BoardState.InitialBoard;
            Assert.True(b.BlackCastlingKingSideAvailable);
            Assert.True(b.BlackCastlingQueenSideAvailable);

            b.Apply(b.GenerateMove(Square.a7, Square.a6));
            Assert.True(b.BlackCastlingKingSideAvailable);
            Assert.True(b.BlackCastlingQueenSideAvailable);

            b.Apply(b.GenerateMove(Square.a8, Square.a7));
            Assert.True(b.BlackCastlingKingSideAvailable);
            Assert.False(b.BlackCastlingQueenSideAvailable);
        }

        [Fact]
        public void generates_opening_moves_for_white()
        {
            var b = BoardState.InitialBoard;
            var moves = b.GetMovesForNextPlayer().ToList();

            Assert.All(moves, m => Assert.Equal(Player.White, m.Player));
        }

        [Fact]
        public void can_enumerate_pieces()
        {
            var b = new BoardState("wpa2 wpb2 wke1 bpa7 bke8");

            Assert.Equal(5, b.Pieces.Count());
            Assert.Equal(3, b.Pieces.Where(p => p.piece.GetPlayer() == Player.White).Count());
        }

        [Theory]
        [InlineData(Player.White, 1, "wpa2 bpa3 bke8")]
        [InlineData(Player.White, 4, "wpa2 wbb4 bpa3 bke8")]
        public void can_get_player_value(Player player, int value, string board)
        {
            var b = new BoardState(board);
            Assert.Equal(value, (int)b.GetBoardValue(player));
        }

        [Theory]
        [InlineData(1, "wpa2")]
        [InlineData(-1, "bpa2")]
        [InlineData(1, "wpa2 wpa3 bpe7")]
        [InlineData(-2, "wpa2 wpa3 bpe7 bbd4")]
        public void can_get_differential_value(int value, string board)
        {
            var b = new BoardState(board);
            Assert.Equal(value, (int)b.GetBoardDifferentialValue());
        }

        [Fact]
        public void can_clone_board()
        {
            var b1 = BoardState.InitialBoard;
            var b2 = (BoardState)b1.Clone();

            for (int i = 0; i < b1.Board.Length; i++)
            {
                Assert.Equal(b1.Board[i], b2.Board[i]);
            }

            Assert.Equal(b1.BlackCastlingKingSideAvailable, b2.BlackCastlingKingSideAvailable);
            Assert.Equal(b1.BlackCastlingQueenSideAvailable, b2.BlackCastlingQueenSideAvailable);

        }

        [Theory]
        [InlineData("bke1 wkc6 brc2", true)]
        [InlineData("bke1 wkc6 brc2 wpc5", false)]
        [InlineData("bke8 wke1 bpf2", true)]
        public void is_white_in_check(string boardState, bool isInCheck)
        {
            var b = new BoardState(boardState);

            Assert.Equal(isInCheck, b.WhiteInCheck);
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

            Assert.Equal(isInCheck, b.BlackInCheck);
        }

        [Theory]
        [InlineData("wbc1", Piece.WhiteBishop, Square.g5)]
        public void can_simulate_move(string boardState, Piece p, Square target)
        {
            var b = new BoardState(boardState);
            var moves = b.SimulateMoves().ToList();

            Assert.Contains<Move>(moves, m => m.Piece == p && m.To == target && m.IsDefendingSquare);

        }

    }
}
