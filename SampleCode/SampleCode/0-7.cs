//Exception 拋出例外
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
            try
            {
                throw new DivideByZeroException("Invalid Division");
            }
            catch (DivideByZeroException e)
            {
                Console.WriteLine("Exception");
            }
        }
    }
}