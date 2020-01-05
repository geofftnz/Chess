using Chess.Engine.Exceptions;
using Chess.Engine.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Engine.Board
{
    public class BoardState : ICloneable
    {
        /// <summary>
        /// board array
        /// </summary>
        public Piece[] Board { get; } = new Piece[64];

        private int[] Flags = new int[6];
        public bool WhiteCastlingKingSideAvailable { get => Flags[0] == 1; set => Flags[0] = value ? 1 : 0; }
        public bool WhiteCastlingQueenSideAvailable { get => Flags[1] == 1; set => Flags[1] = value ? 1 : 0; }
        public bool BlackCastlingKingSideAvailable { get => Flags[2] == 1; set => Flags[2] = value ? 1 : 0; }
        public bool BlackCastlingQueenSideAvailable { get => Flags[3] == 1; set => Flags[3] = value ? 1 : 0; }

        public Square? EnPassantTargetSquare { get => (Flags[4] == -1) ? null : (Square?)Flags[4]; set => Flags[4] = value.HasValue ? (int)value.Value : -1; }
        public Player EnPassantTargetPlayer { get => (Player)Flags[5]; set => Flags[5] = (int)value; }

        public BoardState()
        {
            Clear();
            EnPassantTargetSquare = null;
            EnPassantTargetPlayer = Player.None;
        }

        public BoardState(string pieces) : this(pieces.Split(' ').Select(a => a.ToPositionedPiece()))
        {

        }
        public BoardState(IEnumerable<PositionedPiece> positionedPieces) : this()
        {
            foreach (var positionedPiece in positionedPieces)
            {
                SetPieceAt(positionedPiece);
            }
        }

        public static BoardState InitialBoard => new BoardState().SetupBoard();

        public Piece PieceAt(Square s)
        {
            return Board[(int)s];
        }

        public bool IsEmpty(Square s) => PieceAt(s) == Piece.None;

        public void SetPieceAt(Square s, Piece p)
        {
            Board[(int)s] = p;
        }

        public void SetPieceAt(PositionedPiece p)
        {
            SetPieceAt(p.square, p.piece);
        }

        public void ClearSquare(Square s)
        {
            Board[(int)s] = Piece.None;
        }

        public BoardState Clear()
        {
            for (int i = 0; i < 64; i++)
            {
                Board[i] = Piece.None;
            }
            return this;
        }

        public BoardState SetupBoard()
        {
            Clear();

            for (int i = 0; i < 8; i++)
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

            WhiteCastlingKingSideAvailable = true;
            WhiteCastlingQueenSideAvailable = true;
            BlackCastlingKingSideAvailable = true;
            BlackCastlingQueenSideAvailable = true;

            return this;
        }

        public object Clone()
        {
            var newBoard = new BoardState();
            Board.CopyTo(newBoard.Board, 0);
            Flags.CopyTo(newBoard.Flags, 0);

            return newBoard;
        }

        public BoardState Apply(Move m)
        {
            // validate move
            ValidateMove(m);

            // apply move

            // remove piece from original position
            ClearSquare(m.From);

            // add piece to target position

            // are we promoting a pawn?
            if (m.PromoteTo != PieceType.None)
            {
                SetPieceAt(m.To, m.Player.GetPiece(m.PromoteTo));
            }
            else
            {

                if (m.IsCapturing)
                {
                    // if we're capturing en-passant, we need to remove the target piece
                    if (m.IsEnPassant)
                    {
                        if (EnPassantTargetSquare.HasValue)
                        {
                            ClearSquare(EnPassantTargetSquare.Value);
                        }
                        else
                        {
                            throw new InvalidMoveException(InvalidMoveReason.EnPassantNotValid);
                        }
                    }
                }
                SetPieceAt(m.To, m.Piece);
            }

            // Set en-passant for this move
            EnPassantTargetSquare = m.GeneratedEnPassantOpportunity;
            EnPassantTargetPlayer = EnPassantTargetSquare.HasValue ? m.Player : Player.None;

            // Set castling flags if appropriate
            if (m.Piece == Piece.WhiteKing)
            {
                WhiteCastlingKingSideAvailable = false;
                WhiteCastlingQueenSideAvailable = false;
            }
            if (m.Piece == Piece.WhiteRook)
            {
                switch (m.From)
                {
                    case Square.a1:
                        WhiteCastlingQueenSideAvailable = false;
                        break;
                    case Square.h1:
                        WhiteCastlingKingSideAvailable = false;
                        break;
                    default: break;
                }
            }

            if (m.Piece == Piece.BlackKing)
            {
                BlackCastlingKingSideAvailable = false;
                BlackCastlingQueenSideAvailable = false;
            }
            if (m.Piece == Piece.BlackRook)
            {
                switch (m.From)
                {
                    case Square.a8:
                        BlackCastlingQueenSideAvailable = false;
                        break;
                    case Square.h8:
                        BlackCastlingKingSideAvailable = false;
                        break;
                    default: break;
                }
            }
            return this;
        }

        public BoardState CloneAndApply(Move m)
        {
            BoardState newBoard = (BoardState)Clone();
            newBoard.Apply(m);
            return newBoard;
        }

        public bool MoveResultsInCheckForPlayer(Move move, Player player)
        {
            var b = CloneAndApply(move);
            return b.IsPlayerInCheck(player);
        }

        public void ValidateMove(Move m)
        {
            if (PieceAt(m.From) != m.Piece)
            {
                throw new InvalidMoveException(InvalidMoveReason.PieceNotAtSpecifiedSquare);
            }
            if (PieceAt(m.To).GetPlayer() == m.Player)
            {
                throw new InvalidMoveException(InvalidMoveReason.TargetSquareOccupiedByPlayer);
            }

            if (!m.IsCapturing && !m.IsEnPassant && !IsEmpty(m.To) && PieceAt(m.To).GetPlayer() != m.Player)
            {
                throw new InvalidMoveException(InvalidMoveReason.CapturingButNotMarkedAsCapture);
            }

            if (m.IsEnPassant && EnPassantTargetPlayer != m.Player.GetOpponent())
            {
                throw new InvalidMoveException(InvalidMoveReason.EnPassantNotValid);
            }
        }

        public IEnumerable<Move> GetMoves(Player player, bool skipTestForCheck = false)
        {
            for (int i = 0; i < 64; i++)
            {
                var p = PieceAt((Square)i);
                if (p.GetPlayer() == player)
                {
                    foreach (var move in GetMoves((Square)i, skipTestForCheck))
                    {
                        yield return move;
                    }
                }
            }
        }

        public IEnumerable<Move> GetMoves(Square square, bool skipTestForCheck = false)
        {
            var piece = PieceAt(square);

            if (piece == Piece.None)
                yield break;

            foreach (var move in this.GetMoves(piece, square, skipTestForCheck))
                yield return move;

            yield break;
        }

        public Move GenerateMove(Square from, Square to)
        {
            var piece = PieceAt(from);
            if (piece == Piece.None)
                throw new InvalidMoveException(InvalidMoveReason.PieceNotAtSpecifiedSquare);

            return new Move(piece, from, to);
        }

        /// <summary>
        /// A player is in check if any available opponent move could capture their king
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public bool IsPlayerInCheck(Player player)
        {
            return GetMoves(player.GetOpponent(), true).Where(m => m.CapturedPiece == player.GetPiece(PieceType.King)).Any();
        }

        /// <summary>
        /// A player is checkmated if they are in check and have no available moves
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public bool IsPlayerCheckmated(Player player)
        {
            if (!IsPlayerInCheck(player))
                return false;

            // TODO: check for available moves.

            return false;
        }
    }
}
