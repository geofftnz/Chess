using Chess.Engine.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Engine.Analysis
{
    public class StaticAnalysis
    {
        public struct SquareAnalysis
        {
            public Square Square;
            public Piece Piece;

            public bool PieceUnderThreat;
            public bool IsDefendedPiece;
            public bool IsWhiteDefendedSquare;
            public bool IsBlackDefendedSquare;
            public bool IsHardPinned;  // cannot move due to threat to king
            //public bool IsSoftPinned;  // absence of this piece could result in capture of another.
            //public float SoftPinValue; // max value of lost piece if soft-pinned piece is moved.
        }


        public BoardState Board { get; set; }
        public SquareAnalysis[] Analysis = new SquareAnalysis[64];

        public StaticAnalysis(BoardState board)
        {
            Board = board;

            Analyse();
        }

        public void Analyse()
        {
            for (int i = 0; i < 64; i++)
            {
                Analysis[i].Square = (Square)i;
                Analysis[i].Piece = Board.Board[i];

                Analysis[i].PieceUnderThreat = false;
            }

            var moves = Board.SimulateMoves().ToList();

            // Threats
            foreach(var sq in moves.Where(m => m.IsCapturing && !m.WouldPlacePlayerInCheck).Select(m=>m.To))
            {
                Analysis[(int)sq].PieceUnderThreat = true;
            }

            // Defended pieces
            foreach (var sq in moves.Where(m => m.IsDefendingPiece && !m.WouldPlacePlayerInCheck).Select(m => m.To))
            {
                Analysis[(int)sq].IsDefendedPiece = true;
            }

            // Squares defended by White
            foreach (var sq in moves.Where(m => m.IsDefendingSquare && !m.WouldPlacePlayerInCheck && m.Player == Player.White).Select(m => m.To))
            {
                Analysis[(int)sq].IsWhiteDefendedSquare = true;
            }

            // Squares defended by Black
            foreach (var sq in moves.Where(m => m.IsDefendingSquare && !m.WouldPlacePlayerInCheck && m.Player == Player.Black).Select(m => m.To))
            {
                Analysis[(int)sq].IsBlackDefendedSquare = true;
            }

            // Pinned pieces
            foreach(var sq in moves.Where(m => m.WouldPlacePlayerInCheck).Select(m => m.From))
            {
                // no other legal moves for piece
                if (!moves.Any(m => m.From == sq && !m.WouldPlacePlayerInCheck))
                {
                    Analysis[(int)sq].IsHardPinned = true;
                }
            }

        }

    }
}
