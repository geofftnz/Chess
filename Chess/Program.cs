using System;
using System.Text;

namespace Chess
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            

            var app = new RandomChess();
            app.Run();
        }
    }
}
