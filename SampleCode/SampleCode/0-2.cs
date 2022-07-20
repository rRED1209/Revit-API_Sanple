//If 判斷式
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            int x = 5;
            int y = 5;
            if (x > y)
                Console.WriteLine("x(" + x + ") > y(" + y + ")");
            else if (x == y)
                Console.WriteLine("x(" + x + ") = y(" + y + ")");
            else
                Console.WriteLine("x(" + x + ") < y(" + y + ")");
        }
    }
}