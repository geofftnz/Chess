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
            public Player Player;
            public PieceType PieceType;
            public float RawValue;

            public int AttackCount;  // number of pieces attacking this piece
            public bool PieceUnderThreat => AttackCount > 0;
            public bool IsDefendedPiece;
            public bool IsUndefendedAttack => PieceUnderThreat && !IsDefendedPiece;
            public bool IsWhiteDefendedSquare;
            public bool IsBlackDefendedSquare;
            public bool IsHardPinned;  // cannot move due to threat to king
            //public bool IsSoftPinned;  // absence of this piece could result in capture of another.
            //public float SoftPinValue; // max value of lost piece if soft-pinned piece is moved.

            public bool IsDefendedBy(Player player) =>
                (player == Player.White && (IsWhiteDefendedSquare || (IsDefendedPiece && Player == player))) ||
                (player == Player.Black && (IsBlackDefendedSquare || (IsDefendedPiece && Player == player)));
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
                Analysis[i].Player = Board.Board[i].GetPlayer();
                Analysis[i].PieceType = Board.Board[i].GetPieceType();
                Analysis[i].RawValue = Analysis[i].PieceType.GetPieceValue();

                Analysis[i].AttackCount = 0;
            }

            var moves = Board.SimulateMoves().ToList();

            // Threats
            foreach (var sq in moves.Where(m => m.IsCapturing && !m.WouldPlacePlayerInCheck).Select(m => m.To))
            {
                Analysis[(int)sq].AttackCount++;
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
            foreach (var sq in moves.Where(m => m.WouldPlacePlayerInCheck).Select(m => m.From))
            {
                // no other legal moves for piece
                if (!moves.Any(m => m.From == sq && !m.WouldPlacePlayerInCheck))
                {
                    Analysis[(int)sq].IsHardPinned = true;
                }
            }
        }

        public bool IsSquareDefendedBy(Square square, Player player)
        {
            return (player == Player.White) ? Analysis[(int)square].IsWhiteDefendedSquare : Analysis[(int)square].IsBlackDefendedSquare;
        }

        public float GetBoardValue(Player player, Move proposedMove = null)
        {
            float value = 0f;

            // value of material on the board (ex-King)
            value += GetMaterialValue(player);

            // rank pawns higher based on advancement
            value += GetPawnAdvancementValue(player) * 0.2f;

            // value of the pieces we're attacking
            value += GetValueOfAttackedUndefendedPieces(player.GetOpponent()) * 0.2f;

            // value of our attacked, undefended pieces.
            value += GetValueOfAttackedUndefendedPieces(player) * 0.2f;

            // value of squares we're defending
            value += GetCountOfDefendedSquares(player) * 0.1f;

            // de-weight value if we're moving to an opponent-controlled square
            value += GetProposedMoveValue(player, proposedMove);

            return value;
        }


        public float GetMaterialValue(Player player)
        {
            return Analysis.Where(sq => sq.Player == player && sq.PieceType != PieceType.King).Sum(sq => sq.RawValue);
        }
        public float GetPawnAdvancementValue(Player player)
        {
            int pawnHomeRank = player == Player.White ? 2 : 7;
            return Analysis.Where(sq => sq.Player == player && sq.PieceType == PieceType.Pawn).Select(sq => (float)Math.Abs(sq.Square.GetRank() - pawnHomeRank) / 8).Sum();
        }

        public float GetValueOfPieces(Player player, Predicate<SquareAnalysis> predicate) => Analysis.Where(sq => sq.Player == player).Where(sq => predicate(sq)).Select(sq => sq.RawValue).Sum();
        public float GetCountOfSquares(Predicate<SquareAnalysis> predicate) => Analysis.Where(sq => predicate(sq)).Count();

        public float GetValueOfAttackedUndefendedPieces(Player player)
        {
            return GetValueOfPieces(player, sq => sq.IsUndefendedAttack);
        }

        public float GetCountOfDefendedSquares(Player player)
        {
            return player == Player.White ?
                GetCountOfSquares(sq => sq.IsWhiteDefendedSquare) :
                GetCountOfSquares(sq => sq.IsBlackDefendedSquare);
        }
        private float GetProposedMoveValue(Player player, Move proposedMove)
        {
            if (proposedMove == null)
                return 0f;

            return -1.0f * (Analysis[(int)proposedMove.To].PieceUnderThreat ? proposedMove.Piece.GetPieceValue() : 0f);
            //return 2f * (IsSquareDefendedBy(proposedMove.To, player.GetOpponent()) ? proposedMove.Piece.GetPieceValue() : 0f);
        }

        public IEnumerable<KeyValuePair<string, float>> GetBoardValueComponents(Player player, Move proposedMove = null)
        {
            yield return new KeyValuePair<string, float>("Mat", GetMaterialValue(player));
            yield return new KeyValuePair<string, float>("PAd", GetPawnAdvancementValue(player));
            yield return new KeyValuePair<string, float>("OppUndef", GetValueOfAttackedUndefendedPieces(player.GetOpponent()));
            yield return new KeyValuePair<string, float>("OurUndef", -GetValueOfAttackedUndefendedPieces(player));
            yield return new KeyValuePair<string, float>("NDefSq", GetCountOfDefendedSquares(player));
            yield return new KeyValuePair<string, float>("Move", GetProposedMoveValue(player, proposedMove));
        }

    }
}
