using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chess.Engine.Board;

namespace Chess.Engine.Strategies
{
    public class BasicLookAheadStrategy : IStrategy
    {
        private IStrategy fallbackStrategy = new BasicNoLookAheadStrategy();
        private int depthLimit = 3;

        public Move SelectNextMove(BoardState board, Player player, IList<Move> moves, out string reasoning)
        {

            var rankedMoves = moves.Select(m => GetMoveValue(board, player, m, depthLimit, GetAbsoluteDiffValueForPlayer(player, board)))
                                   .OrderByDescending(mv => mv.Item2)
                                   .ToList();

            var bestMove = rankedMoves.FirstOrDefault();
            var worstMoveValue = rankedMoves.Select(mv => mv.Item2).Min();

            // if the best move is better than any other move, then return it, otherwise use our fallback strategy.
            if (bestMove != null && bestMove.Item2 > worstMoveValue)
            {
                reasoning = $"Differential={bestMove.Item2}, depth={depthLimit}";
                return bestMove.Item1;
            }

            return fallbackStrategy.SelectNextMove(board, player, moves, out reasoning);
        }

        /// <summary>
        /// Move value is 
        /// </summary>
        /// <param name="board"></param>
        /// <param name="player"></param>
        /// <param name="moves"></param>
        /// <param name="depth"></param>
        /// <returns></returns>
        private Tuple<Move, float> GetMoveValue(BoardState board, Player player, Move move, int depth, float startingValue)
        {
            var newBoard = board.CloneAndApply(move);

            float value = GetAbsoluteDiffValueForPlayer(player, newBoard);

            // if we are recursing, then generate all possible opponent moves and take the highest board value
            if (depth > 1)
            {
                var nextMoves = newBoard.GetMovesForNextPlayer().ToList();
                if (nextMoves.Count == 0) // checkmate
                {
                    return new Tuple<Move, float>(move, -1000);
                }

                if (depth == depthLimit)
                {
                    value = nextMoves.AsParallel()
                        .Select(m => GetMoveValue(newBoard, newBoard.NextPlayerToMove, m, depth - 1, startingValue))
                        .Select(mv => mv.Item2)
                        .Max() - startingValue;
                }
                else
                {
                    value = nextMoves
                        .Select(m => GetMoveValue(newBoard, newBoard.NextPlayerToMove, m, depth - 1, startingValue))
                        .Select(mv => mv.Item2)
                        .Max() - startingValue;
                }

            }

            /*
            if (value > 0f)
            {
                Console.Clear();
                RenderToConsole(newBoard);
                Console.WriteLine($"Diff value for {player}: {value}");
                Console.WriteLine($"White value: {newBoard.GetBoardValue(Player.White)}");
                Console.WriteLine($"Black value: {newBoard.GetBoardValue(Player.Black)}");
                Console.ReadKey();
            }*/

            return new Tuple<Move, float>(move, value);
        }

        private static float GetAbsoluteDiffValueForPlayer(Player player, BoardState newBoard)
        {
            float value = newBoard.GetBoardDifferentialValue();
            if (player == Player.Black)
            {
                value = -value;
            }

            return value;
        }

        private void RenderToConsole(BoardState b)
        {
            if (b == null)
                throw new ArgumentNullException(nameof(b));

            for (int rank = 8; rank >= 1; rank--)
            {
                for (int file = 0; file < 8; file++)
                {
                    Square s = ((File)file).GetSquare(rank);
                    Console.BackgroundColor = s.GetColour() == Colour.Dark ? ConsoleColor.DarkGreen : ConsoleColor.Gray;

                    Piece p = b.PieceAt(s);

                    Console.ForegroundColor = p.GetPlayer() == Player.Black ? ConsoleColor.Black : ConsoleColor.White;

                    Console.Write($" {p.ToAbbr()} ");

                }
                Console.WriteLine();
            }


            Console.ResetColor();

        }

    }
}
