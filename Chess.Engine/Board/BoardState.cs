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

        public Square? EnPassantTargetSquare { get; set; } = null;
        public Player EnPassantTargetPlayer { get; set; } = Player.None;


        public BoardState()
        {
        }

        public BoardState(string pieces) : this(pieces.Split(' ').Select(a => a.ToPositionedPiece()))
        {

        }
        public BoardState(IEnumerable<PositionedPiece> positionedPieces)
        {
            Clear();

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

            return this;
        }

        public object Clone()
        {
            var newBoard = new BoardState();
            for (int i = 0; i < 64; i++)
            {
                newBoard.Board[i] = Board[i];
            }
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

                    }
                }
                SetPieceAt(m.To, m.Piece);
            }

            // Set en-passant for this move
            EnPassantTargetSquare = m.GeneratedEnPassantOpportunity;
            EnPassantTargetPlayer = EnPassantTargetSquare.HasValue ? m.Player : Player.None;

            return this;
        }

        public BoardState CloneAndApply(Move m)
        {
            BoardState newBoard = (BoardState)Clone();
            newBoard.Apply(m);
            return newBoard;
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

        public IEnumerable<Move> GetMoves(Player player)
        {
            for (int i = 0; i < 64; i++)
            {
                var p = PieceAt((Square)i);
                if (p.GetPlayer() == player)
                {
                    foreach(var move in GetMoves((Square)i))
                    {
                        yield return move;
                    }
                }
            }
        }

        public IEnumerable<Move> GetMoves(Square square)
        {
            var piece = PieceAt(square);

            if (piece == Piece.None)
                yield break;

            foreach (var move in this.GetMoves(piece, square))
                yield return move;

            yield break;
        }
    }
}
