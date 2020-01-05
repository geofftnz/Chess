using Chess.Engine.Board;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess
{
    public static class Renderer
    {
        public static void RenderToConsole(this BoardState b)
        {
            if (b == null)
                throw new ArgumentNullException(nameof(b));

            for(int rank = 8; rank >= 1; rank--)
            {
                for(int file = 0; file < 8; file++)
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
